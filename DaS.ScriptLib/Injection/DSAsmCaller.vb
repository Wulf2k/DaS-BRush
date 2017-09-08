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
Imports DaS.ScriptLib.Lua.Structures
Imports DaS.ScriptLib.Lua

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

        Private Function GetNewCopyOfAsmBuffer() As Byte()
            Return AsmBuffer.ToArray()
        End Function

        Private Function GetAsmDword(dword As Int32) As Byte()
            Return BitConverter.GetBytes(dword)
        End Function

        Private Function WriteAsmDword(address As IntPtr, dword As Int32) As Boolean
            Dim dwBytes = GetAsmDword(dword)
            Dim oldLoc = AsmBuffer.Position
            AsmBuffer.Position = address - CodeHandle.GetHandle()
            AsmBuffer.Write(dwBytes, 0, INT32_SIZE)
            AsmBuffer.Position = oldLoc
            Return WriteAsm(address, GetAsmDword(dword), INT32_SIZE)
        End Function

        Private Function WriteAsm(address As IntPtr, bytes As Byte(), count As IntPtr) As Boolean
            Return Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), address, bytes, count, Nothing) AndAlso
                Kernel.FlushInstructionCache(DARKSOULS.GetHandle(), address, count)
        End Function

        Private Function InjectEntireCodeBuffer() As Boolean
            Return WriteAsm(CodeHandle.GetHandle(), AsmBuffer.ToArray(), FUNCTION_CALL_ASM_BUFFER_SIZE)
        End Function

        Public ReadOnly Property CodeHandle As New SafeRemoteHandle(FUNCTION_CALL_ASM_BUFFER_SIZE)

        Private Sub CompletelyReInitializeAndInjectCodeInNewLocation()
            UndoCodeInjection()
            CodeHandle.Dispose()
            _CodeHandle = New SafeRemoteHandle(FUNCTION_CALL_ASM_BUFFER_SIZE)
            InjectEntireCodeBuffer()
            PatchUpdateFunctionReturnAddress()
        End Sub

        Private Sub UndoCodeInjection()
            If Not CodeHandle.IsClosed Then
                CodeHandle.Close()
            End If
        End Sub

        Private AsmLocBegin As MoveableAddressOffset
        Private AsmLocAfterEachStackMov(BUFFER_STACK_SIZE - 1) As MoveableAddressOffset
        Private AsmLocAfterLuaFunctionCall As MoveableAddressOffset
        Private AsmLocAfterSetReturnLocation As MoveableAddressOffset
        Private AsmLocAfterReturn As MoveableAddressOffset

        Public ReadOnly Property IsCodeInjected As Boolean
            Get
                Return (Not CodeHandle.IsClosed) AndAlso (Not CodeHandle.IsInvalid)
            End Get
        End Property

        ''' <summary>
        ''' Note that this function reverses the order for you (pass the stack values as you would read
        ''' them from left to right in the source code).
        ''' </summary>
        ''' <param name="stackValues"></param>
        Private Sub PatchStackValues(stackValues As List(Of Int32))
            If stackValues.Count > BUFFER_STACK_SIZE Then
                Throw New ArgumentException($"Tried to write a list of stack values larger than the max stack buffer size constant (BUFFER_STACK_SIZE = 10, stackValues.Count = {stackValues.Count})", "stackValues")
            End If

            Dim stackBuffer(BUFFER_STACK_SIZE * INT32_SIZE) As Byte

            For i = 0 To BUFFER_STACK_SIZE - 1
                'Since the last thing asm.Move32 writes is the actual stack value itself, we need to simply
                'write the value at (the location immediately after each MOV - INT32_SIZE)
                WriteAsmDword(AsmLocAfterEachStackMov(i).Location - New IntPtr(INT32_SIZE),
                       If(i <= (stackValues.Count - 1), stackValues(i), 0))
                'Once we get past the length of stackValues, we write 0's to fill the rest.
            Next
        End Sub

        ''' <summary>
        ''' Pretty much just clears all stack values.
        ''' </summary>
        Private Sub PatchStackValues()
            PatchStackValues(New List(Of Int32))
        End Sub

        Private Sub PatchLuaFunctionCallAddress(newAddress As IntPtr)
            'public void Call(Int32 displacement) {
            '   writer.Write(new byte[] { 0xE8 });
            '   writer.Write(displacement - 5);
            '}

            'The '- 1' below is to account for the initial 0xE8 byte above ( FailFish )
            WriteAsmDword(AsmLocAfterLuaFunctionCall.Location - New IntPtr(INT32_SIZE),
                   (newAddress.ToInt32() - (AsmLocAfterLuaFunctionCall.Location.ToInt32() - INT32_SIZE - 1)) - 5)
        End Sub

        Private Sub PatchUpdateFunctionReturnAddress()
            WriteAsmDword(AsmLocAfterSetReturnLocation.Location - New IntPtr(INT32_SIZE), CodeHandle.GetHandle() + FUNC_RETURN_ADDR_OFFSET)
        End Sub

        Public Sub New()
            HookEvents()
            InitAsmBuffer()
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

        Private Sub InitAsmBuffer()
            AsmBuffer.Position = 0
            Dim asm As New X86Writer(AsmBuffer, CodeHandle.GetHandle())
            'ASM START:
            '(LOC)
            AsmLocBegin = New MoveableAddressOffset(Me, asm.Position)
            asm.Push32(Reg32.EBP)
            asm.Mov32(Reg32.EBP, Reg32.ESP)
            asm.Push32(Reg32.EAX)
            'STACK:
            For i As Integer = (BUFFER_STACK_SIZE - 1) To 0 Step -1
                asm.Mov32(Reg32.EAX, PLACEHOLDER_INT32)
                '(LOC)
                AsmLocAfterEachStackMov(i) = New MoveableAddressOffset(Me, asm.Position)
                asm.Push32(Reg32.EAX)
            Next
            'CALL LUA FUNCTION:
            asm.Call(New IntPtr(PLACEHOLDER_INT32))
            '(LOC)
            AsmLocAfterLuaFunctionCall = New MoveableAddressOffset(Me, asm.Position)
            'SET RETURN POS:
            asm.Mov32(Reg32.EBX, CodeHandle.GetHandle().ToInt32() + FUNC_RETURN_ADDR_OFFSET)
            '(LOC)
            AsmLocAfterSetReturnLocation = New MoveableAddressOffset(Me, asm.Position)
            asm.Mov32(New Addr(Reg32.EBX, 0), Reg32.EAX) 'mov [ebx], eax
            asm.Pop32(Reg32.EAX)
            For i = 1 To BUFFER_STACK_SIZE
                asm.Pop32(Reg32.EAX)
            Next
            asm.Mov32(Reg32.ESP, Reg32.EBP)
            asm.Pop32(Reg32.EBP)
            asm.Retn()
            '(LOC)
            AsmLocAfterReturn = New MoveableAddressOffset(Me, asm.Position)

            While AsmBuffer.Position < FUNCTION_CALL_ASM_BUFFER_SIZE
                AsmBuffer.WriteByte(0)
            End While

        End Sub

        Private Sub ____freeClrManagedResources()
            UnhookEvents()
        End Sub

        Private Sub ____freeNativeUnmanagedResources()
            UndoCodeInjection()
        End Sub

        Private Function ExecuteAsm() As Byte()
            Dim threadHandle = New SafeRemoteThreadHandle(CodeHandle)
            Kernel.WaitForSingleObject(threadHandle.GetHandle(), MAX_WAIT)
            threadHandle.Close()
            threadHandle.Dispose()

            Return CodeHandle.GetFuncReturnValue()
        End Function

        Private Function SquashIntoDword(ByRef allocPtrList As List(Of SafeRemoteHandle), arg As Object) As Int32
            Dim typ = arg.GetType()

            If typ = GetType(LuaBoxedVal) Then
                'Sorry about this double-unbox here. Hope it doesn't create too much overhead.
                'I tried to do it with generics but the only feasable way to do that would
                'be to check LuaBoxedVal(Of Int32), LuaBoxedVal(Of Single), etc. one-by-one
                'and I figured those if/else's would generate more overhead, even for other 
                'things Not boxed inside of one of these.
                Return SquashIntoDword(allocPtrList, DirectCast(arg, LuaBoxedVal).Value)
            ElseIf typ = GetType(Int32) Then
                Return arg
            ElseIf typ = GetType(Double) Then
                Return ToDwordLossy(arg)
            ElseIf typ = GetType(IntPtr) Then
                Return ToDwordLossy(arg)
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
                            Return ToDword(argByt)
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
                        unmanagedArg.Close()
                        unmanagedArg.Dispose()
                        allocPtrList.Add(hand)

                        Return unmanagedArg.GetHandle().ToInt32()

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

            Return Convert.ToInt32(arg) 'If it reaches here without returning, just give up and try good ol' Convert.ToInt32()
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
                Case FuncReturnType.STRING : Return RAsciiStr(New MutatableDword(result).Int1)
                Case FuncReturnType.UNISTRING : Return RUnicodeStr(New MutatableDword(result).Int1)
                Case Else
                    Return BitConverter.ToInt32(result, 0)
            End Select
        End Function

        Public Function CallIngameLua(returnType As FuncReturnType, functionAddress As Integer, args As NLua.LuaTable) As Object
            InjectEntireCodeBuffer()

            Dim result As Int32 = FUNCCALL_ERR
            PatchLuaFunctionCallAddress(functionAddress)

            Dim ptrList As New List(Of SafeRemoteHandle)

            If args Is Nothing OrElse args.Values.Count = 0 Then
                PatchStackValues(New List(Of Integer)())
            Else
                Dim objArgs = args.Values.OfType(Of Object)
                PatchStackValues(objArgs.Select(Function(a) SquashIntoDword(ptrList, a)).ToList())
            End If

            Dim resByt = ExecuteAsm()
            result = GetFunctionCallResult(returnType, resByt)

            For Each ptr In ptrList
                ptr.Close()
                ptr.Dispose()
            Next
            ptrList.Clear()

            'If args IsNot Nothing Then
            '    Dim passedArgStr = String.Join(", ", args.Values.OfType(Of Object).Select(Function(x) x.ToString()))
            '    Lua.Dbg.Print($"Call to {LuaInterface.IngameFuncNames(functionAddress)}({passedArgStr}) returned {result.ToString()}")
            'Else
            '    Lua.Dbg.Print($"Call to {LuaInterface.IngameFuncNames(functionAddress)}() returned {result.ToString()}")
            'End If

            Return result
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