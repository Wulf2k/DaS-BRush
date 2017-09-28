Imports System.Globalization
Imports System.Runtime.InteropServices
Imports Managed.X86
Imports Reg8 = Managed.X86.X86Register8
Imports Reg16 = Managed.X86.X86Register16
Imports Reg32 = Managed.X86.X86Register32
Imports Addr = Managed.X86.X86Address
Imports Lbl = Managed.X86.X86Label
Imports Cond = Managed.X86.X86ConditionCode
Imports System.IO
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.Injection.Structures
Imports DaS.ScriptLib.LuaScripting.Structures
Imports DaS.ScriptLib.LuaScripting
Imports Neo.IronLua

Namespace Injection
    Public Class DSAsmCaller
        Implements IDisposable

        Public Const FUNCTION_CALL_ASM_BUFFER_SIZE As Int32 = 1024
        Public Const ReturnValueCheckInterval As Int32 = 5
        Public Const FUNC_RETURN_ADDR_OFFSET As Int32 = &H200
        Public Const MAX_WAIT As Int32 = 33
        Public Const BUFFER_STACK_SIZE As Int32 = 5
        Public Const PLACEHOLDER_INT32 As Int32 = &HE110D00D
        Public Const INT32_SIZE As Int32 = 4

        Public Const FUNCCALL_ERR As Int32 = &HFFFFFFFF

        Private AsmBuffer As New MemoryStream(1024)

        Private Buffer_Result As Object
        Private Buffer_ParamPointerList As New List(Of SafeRemoteHandle)
        Private Buffer_DefaultEmptyStack As New List(Of Int32)
        Private Buffer_Stack(BUFFER_STACK_SIZE - 1) As Int32
        Private Buffer_ResultBytes(INT32_SIZE - 1) As Byte
        Private Buffer_StackCounter As Int32 = 0

        Private Buffer_SquashIntoDwordResult As Int32 = 0

        Public ReadOnly Property CodeHandle As New SafeRemoteHandle(FUNCTION_CALL_ASM_BUFFER_SIZE)

        Private AsmLocBegin As MoveableAddressOffset
        Private AsmLocAtECX As MoveableAddressOffset
        Private AsmLocAfterEachStackMov(BUFFER_STACK_SIZE - 1) As MoveableAddressOffset
        Private AsmLocAfterLuaFunctionCall As MoveableAddressOffset
        Private AsmLocAfterSetReturnLocation As MoveableAddressOffset
        Private AsmLocAfterReturn As MoveableAddressOffset

        Private Function GetNewCopyOfAsmBuffer() As Byte()
            Return AsmBuffer.ToArray()
        End Function

        Private Function WriteAsm(address As IntPtr, bytes As Byte(), count As IntPtr) As Boolean
            If address.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return False
            End If
            Return Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), address, bytes, count, Nothing) AndAlso
                Kernel.FlushInstructionCache(DARKSOULS.GetHandle(), address, count)
        End Function

        Private Function InjectEntireCodeBuffer() As Boolean
            Return WriteAsm(CodeHandle.GetHandle(), AsmBuffer.ToArray(), AsmBuffer.Position)
        End Function

        Private Sub CompletelyReInitializeAndInjectCodeInNewLocation()
            UndoCodeInjection()
            CodeHandle.Dispose()
            _CodeHandle = New SafeRemoteHandle(FUNCTION_CALL_ASM_BUFFER_SIZE)
        End Sub

        Private Sub UndoCodeInjection()
            If CodeHandle IsNot Nothing AndAlso Not CodeHandle.IsClosed Then
                CodeHandle.Close()
            End If
        End Sub

        Public ReadOnly Property IsCodeInjected As Boolean
            Get
                Return (Not CodeHandle.IsClosed) AndAlso (Not CodeHandle.IsInvalid)
            End Get
        End Property

        Public Sub New()
            HookEvents()
            CompletelyReInitializeAndInjectCodeInNewLocation()
        End Sub

        Private Sub HookEvents()
            AddHandler DARKSOULS.OnAttach, AddressOf Proc_OnAttachToCurrentProcess
            AddHandler DARKSOULS.OnDetach, AddressOf Proc_OnDetachFromCurrentProcess
        End Sub

        Private Sub UnhookEvents()
            RemoveHandler DARKSOULS.OnAttach, AddressOf Proc_OnAttachToCurrentProcess
            RemoveHandler DARKSOULS.OnDetach, AddressOf Proc_OnDetachFromCurrentProcess
        End Sub

        Private Sub Proc_OnDetachFromCurrentProcess()
            UndoCodeInjection()
        End Sub

        Private Sub Proc_OnAttachToCurrentProcess()
            CompletelyReInitializeAndInjectCodeInNewLocation()
        End Sub

        Private Sub InitAsmBuffer(funcAddr As Integer, parameters As IEnumerable(Of Object), specialRegisters As Object, allocPtrList As List(Of SafeRemoteHandle))
            Dim args = parameters.ToArray()

            AsmBuffer.Position = 0
            Dim asm As New X86Writer(AsmBuffer, CodeHandle.GetHandle())
            'ASM START:
            asm.Push32(Reg32.EBP)
            asm.Mov32(Reg32.EBP, Reg32.ESP)
            asm.Push32(Reg32.EAX)

            For i = args.Length - 1 To 0 Step -1
                asm.Mov32(Reg32.EAX, SquashIntoDword(allocPtrList, args(i)))
                asm.Push32(Reg32.EAX)
            Next

            If specialRegisters IsNot Nothing Then
                If TypeOf specialRegisters Is LuaTable Then
                    Dim regs = DirectCast(specialRegisters, LuaTable)
                    For Each kvp In regs
                        asm.Mov32([Enum].Parse(GetType(Reg32), kvp.Key), SquashIntoDword(allocPtrList, kvp.Value))
                    Next
                ElseIf TypeOf specialRegisters Is Dictionary(Of String, Object) Then
                    Dim regs = DirectCast(specialRegisters, Dictionary(Of String, Object))
                    For Each r In regs.Keys
                        asm.Mov32([Enum].Parse(GetType(Reg32), r), SquashIntoDword(allocPtrList, regs(r)))
                    Next
                End If
            End If

            'CALL LUA FUNCTION:
            asm.Call(New IntPtr(funcAddr))
            AsmLocAfterLuaFunctionCall = New MoveableAddressOffset(Me, asm.Position)
            'SET RETURN POS:
            asm.Mov32(Reg32.EBX, CodeHandle.GetHandle().ToInt32() + FUNC_RETURN_ADDR_OFFSET)
            asm.Mov32(New Addr(Reg32.EBX, 0), Reg32.EAX) 'mov [ebx], eax
            asm.Pop32(Reg32.EAX)

            For i = args.Length - 1 To 0 Step -1
                asm.Pop32(Reg32.EAX)
            Next

            asm.Mov32(Reg32.ESP, Reg32.EBP)
            asm.Pop32(Reg32.EBP)
            asm.Retn()
        End Sub

        Private Sub ____freeClrManagedResources()
            UnhookEvents()

            AsmBuffer.Dispose()
            AsmBuffer = Nothing

            Buffer_Result = Nothing
            Buffer_ParamPointerList.Clear()
            Buffer_ParamPointerList = Nothing

            Buffer_DefaultEmptyStack.Clear()
            Buffer_DefaultEmptyStack = Nothing
            Buffer_Stack = Nothing
            Buffer_ResultBytes = Nothing
            Buffer_StackCounter = Nothing

            AsmLocBegin = Nothing
            AsmLocAfterEachStackMov = Nothing
            AsmLocAfterLuaFunctionCall = Nothing
            AsmLocAfterSetReturnLocation = Nothing
            AsmLocAfterReturn = Nothing
        End Sub

        Private Sub ____freeNativeUnmanagedResources()
            UndoCodeInjection()
            CodeHandle.Dispose()
            _CodeHandle = Nothing
        End Sub

        Private Function ExecuteAsm() As Byte()
            Dim threadHandle = New SafeRemoteThreadHandle(CodeHandle)
            If Not threadHandle.IsClosed And Not threadHandle.IsInvalid Then
                Kernel.WaitForSingleObject(threadHandle.GetHandle(), MAX_WAIT)
            End If
            threadHandle.Close()
            threadHandle.Dispose()
            threadHandle = Nothing

            Return CodeHandle.GetFuncReturnValue()
        End Function

        Private Function SquashIntoDword(ByRef allocPtrList As List(Of SafeRemoteHandle), arg As Object) As Int32
            Dim typ = arg.GetType()

            Buffer_SquashIntoDwordResult = 0

            If typ = GetType(DSLuaBoxedVal) Then
                'Sorry about this double-unbox here. Hope it doesn't create too much overhead.
                'I tried to do it with generics but the only feasable way to do that would
                'be to check LuaBoxedVal(Of Int32), LuaBoxedVal(Of Single), etc. one-by-one
                'and I figured those if/else's would generate more overhead, even for other 
                'things Not boxed inside of one of these.
                Buffer_SquashIntoDwordResult = SquashIntoDword(allocPtrList, DirectCast(arg, DSLuaBoxedVal).Value)

                arg = Nothing
            ElseIf typ = GetType(Int32) Then
                Buffer_SquashIntoDwordResult = arg

                arg = Nothing
            ElseIf typ = GetType(Double) Then
                Buffer_SquashIntoDwordResult = ToDwordLossy(arg)

                arg = Nothing
            ElseIf typ = GetType(Boolean) Then
                Buffer_SquashIntoDwordResult = If(DirectCast(arg, Boolean), 1, 0)
            ElseIf TypeOf arg Is BoxedString Then
                Dim bs = DirectCast(arg, BoxedString)

                Dim hand = New SafeRemoteHandle(If(bs.Uni, (bs.Str.Length + 1) * 2, bs.Str.Length + 1))
                Dim handVal = hand.GetHandle()

                If (bs.Uni) Then
                    WUnicodeStr(handVal, bs.Str)
                Else
                    WAsciiStr(handVal, bs.Str)
                End If

                allocPtrList.Add(hand)
                Buffer_SquashIntoDwordResult = handVal

            ElseIf typ = GetType(IntPtr) Then
                Buffer_SquashIntoDwordResult = ToDwordLossy(arg)

                arg = Nothing
            Else
                Dim size = Marshal.SizeOf(arg)

                If size <= INT32_SIZE Then
                    Dim toDwordFailed = False
                    Try
                        Return ToDword(arg)
                    Catch ex As Exception
                        toDwordFailed = True
                    End Try
                    If toDwordFailed Then
                        Dim ptrToArg As IntPtr = Marshal.AllocHGlobal(size) 'Allocate a place for our arg
                        Try
                            Marshal.StructureToPtr(arg, ptrToArg, True) 'Move arg to where that pointer points
                            Dim argByt(size - 1) As Byte 'Make a new byte array the size of the arg
                            Marshal.Copy(ptrToArg, argByt, 0, size) 'Copy bytes from [ptrToArg] to argByt
                            Buffer_SquashIntoDwordResult = ToDword(argByt)
                        Catch ex As Exception
                            Throw ex
                        Finally
                            Marshal.FreeHGlobal(ptrToArg)
                        End Try
                    End If
                Else
                    'Allocate a place for our arg
                    Dim ptrToArg As IntPtr = Marshal.AllocHGlobal(size)

                    Try
                        Dim hand = New SafeRemoteHandle(size)
                        Dim unmanagedArg = New SafeMarshalledHandle(arg)
                        hand.MemPatch(unmanagedArg)
                        allocPtrList.Add(hand)

                        Buffer_SquashIntoDwordResult = unmanagedArg.GetHandle().ToInt32()

                        If unmanagedArg IsNot Nothing Then
                            unmanagedArg.Close()
                            unmanagedArg.Dispose()
                            unmanagedArg = Nothing
                        End If


                        '##### OLD METHOD: #####
                        'Move arg to where that pointer points
                        'Marshal.StructureToPtr(arg, ptrToArg, True)
                        ''Make a new byte array the size of the arg
                        'Dim argByt(size - 1) As Byte
                        ''Copy bytes from where we just moved that object to, over to our byte array
                        'Marshal.Copy(ptrToArg, argByt, 0, size)
                        '' > argByt NOW CONTAINS ARG AS BYTES <
                        'Dim ingamePtrToArg As New IngameAllocatedPtr(size)
                        'WriteProcessMemory(CurrentProcessHandle, ingamePtrToArg.Address, argByt, size, New Integer())
                        'allocPtrList.Add(ingamePtrToArg)
                        'Return ingamePtrToArg.Address
                    Catch ex As Exception
                        Throw ex 'We mainly here for dat Finally my boi
                    Finally
                        Marshal.FreeHGlobal(ptrToArg)
                    End Try
                End If
            End If

            Return Buffer_SquashIntoDwordResult
        End Function

        Private Function GetFunctionCallResult(returnType As FuncReturnType, result As Byte()) As Object
            Select Case returnType
                Case FuncReturnType.VOID : Return 0
                Case FuncReturnType.INT : Return New MutatableDword(result).Int1
                Case FuncReturnType.UINT : Return New MutatableDword(result).UInt1
                Case FuncReturnType.SHORT : Return New MutatableDword(result).Short1
                Case FuncReturnType.USHORT : Return New MutatableDword(result).UShort1
                Case FuncReturnType.BOOL : Return New MutatableDword(result).Bool1
                Case FuncReturnType.BYTE : Return New MutatableDword(result).Byte1
                Case FuncReturnType.SBYTE : Return New MutatableDword(result).SByte1
                Case FuncReturnType.FLOAT : Return New MutatableDword(result).Float1
                Case FuncReturnType.DOUBLE : Return CType(New MutatableDword(result).Float1, Double)
                Case FuncReturnType.STR_ANSI : Return RAsciiStr(New MutatableDword(result).Int1, -1)
                Case FuncReturnType.STR_UNI : Return RUnicodeStr(New MutatableDword(result).Int1, -1)
                Case Else
                    Return BitConverter.ToInt32(result, 0)
            End Select
        End Function

        Public Function CallIngameFunc(returnType As FuncReturnType, functionAddress As Integer, args As IEnumerable(Of Object), specialRegisters As Object) As Object

            If CodeHandle.IsClosed OrElse CodeHandle.IsInvalid Then
                CompletelyReInitializeAndInjectCodeInNewLocation()
            End If

            Buffer_ParamPointerList.Clear()

            InitAsmBuffer(functionAddress, args, specialRegisters, Buffer_ParamPointerList)

            If Not InjectEntireCodeBuffer() Then
                Dbg.PrintErr("WARNING: CODE INJECT FAILURE")
            End If

            'luai.DebugUpdate()

            For Each ptr In Buffer_ParamPointerList
                ptr.Dispose()
            Next


            Buffer_ResultBytes = ExecuteAsm()

            Buffer_Result = GetFunctionCallResult(returnType, Buffer_ResultBytes)

            For Each ptr In Buffer_ParamPointerList
                ptr.Close()
                ptr.Dispose()
            Next

            Buffer_ParamPointerList.Clear()

            Return Buffer_Result
        End Function

        Public Function CallIngameFunc_FromLua(returnType As FuncReturnType, functionAddress As Integer, args As LuaTable, specialRegisters As Object) As Object
            Return CallIngameFunc(returnType, functionAddress, args.ArrayList, specialRegisters)
        End Function

#Region "IDisposable Support"
        Private ____disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not ____disposedValue Then
                If disposing Then
                    ____freeClrManagedResources()
                End If

                ____freeNativeUnmanagedResources()
            End If
            ____disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace