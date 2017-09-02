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

Public Class LuaFunctionCaller
    Implements IDisposable

    Public Const FUNCTION_CALL_ASM_BUFFER_SIZE As Int32 = 1024
    Public Const ReturnValueCheckInterval As Int32 = 5
    Public Const FUNC_RETURN_ADDR_OFFSET As Int32 = &H200
    Public Const MAX_WAIT As Int32 = 33
    Public Const BUFFER_STACK_SIZE As Int32 = 10
    Public Const PLACEHOLDER_INT32 As Int32 = &HE110D00D
    Public Const INT32_SIZE As Int32 = 4

    Public Const FUNCCALL_ERR As Int32 = &HFFFFFFFF

    Public Const RETURN_TYPE_VOID As Int32 = -1
    Public Const RETURN_TYPE_INT As Int32 = 0
    Public Const RETURN_TYPE_UINT As Int32 = 1
    Public Const RETURN_TYPE_SHORT As Int32 = 2
    Public Const RETURN_TYPE_USHORT As Int32 = 3
    Public Const RETURN_TYPE_BYTE As Int32 = 4
    Public Const RETURN_TYPE_SBYTE As Int32 = 5
    Public Const RETURN_TYPE_BOOL As Int32 = 6
    Public Const RETURN_TYPE_FLOAT As Int32 = 7
    Public Const RETURN_TYPE_DOUBLE As Int32 = 8
    Public Const RETURN_TYPE_STR As Int32 = 9
    Public Const RETURN_TYPE_UNISTR As Int32 = 10

    Public CurrentProcessHandle As IntPtr = IntPtr.Zero

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
        AsmBuffer.Position = address - CurrentCodeLocation
        AsmBuffer.Write(dwBytes, 0, INT32_SIZE)
        AsmBuffer.Position = oldLoc
        Return WriteAsm(address, GetAsmDword(dword), INT32_SIZE)
    End Function

    Private Function WriteAsm(address As IntPtr, bytes As Byte(), count As IntPtr) As Boolean
        Return WriteProcessMemory(CurrentProcessHandle, address, bytes, count, 0) AndAlso
            FlushInstructionCache(CurrentProcessHandle, address, count)
    End Function

    Private Function InjectEntireCodeBuffer() As Boolean
        Return WriteAsm(CurrentCodeLocation, AsmBuffer.ToArray(), FUNCTION_CALL_ASM_BUFFER_SIZE)
    End Function

    Public Event CurrentCodeLocationChanged(newOffset As IntPtr)

    Private ____current_code_location_do_not_touch As IntPtr = IntPtr.Zero
    Public Property CurrentCodeLocation As IntPtr
        Get
            Return ____current_code_location_do_not_touch
        End Get
        Private Set(value As IntPtr)
            ____current_code_location_do_not_touch = value
            RaiseEvent CurrentCodeLocationChanged(____current_code_location_do_not_touch)
        End Set
    End Property

    Private Sub CompletelyReInitializeAndInjectCodeInNewLocation()
        UndoCodeInjection()
        CurrentCodeLocation = VirtualAllocEx(CurrentProcessHandle, 0, FUNCTION_CALL_ASM_BUFFER_SIZE, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        InjectEntireCodeBuffer()
        PatchUpdateFunctionReturnAddress()
    End Sub

    Private Sub UndoCodeInjection()
        If IsCodeInjected Then
            VirtualFreeEx(CurrentProcessHandle, CurrentCodeLocation, 0, MEM_RELEASE)
            CurrentCodeLocation = IntPtr.Zero
        End If
    End Sub

    Private AsmLocBegin As MoveableAddressOffset
    Private AsmLocAfterEachStackMov(BUFFER_STACK_SIZE - 1) As MoveableAddressOffset
    Private AsmLocAfterLuaFunctionCall As MoveableAddressOffset
    Private AsmLocAfterSetReturnLocation As MoveableAddressOffset
    Private AsmLocAfterReturn As MoveableAddressOffset

    Public ReadOnly Property IsCodeInjected As Boolean
        Get
            Return CurrentCodeLocation <> IntPtr.Zero
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
        WriteAsmDword(AsmLocAfterLuaFunctionCall.Location - New IntPtr(INT32_SIZE),
               (newAddress.ToInt32() - AsmLocAfterLuaFunctionCall.Location.ToInt32()) - 5)
    End Sub

    Private Sub PatchUpdateFunctionReturnAddress()
        WriteAsmDword(AsmLocAfterSetReturnLocation.Location, CurrentCodeLocation + FUNC_RETURN_ADDR_OFFSET)
    End Sub

    Public Sub New(processHandle As IntPtr)
        HookEvents()
        InitAsmBuffer()
        CompletelyReInitializeAndInjectCodeInNewLocation()
    End Sub

    Private Sub HookEvents()
        AddHandler Proc.OnAttachToCurrentProcess, AddressOf Proc_OnAttachToCurrentProcess
        AddHandler Proc.OnDetachFromCurrentProcess, AddressOf Proc_OnDetachFromCurrentProcess
    End Sub

    Private Sub UnhookEvents()
        RemoveHandler Proc.OnAttachToCurrentProcess, AddressOf Proc_OnAttachToCurrentProcess
        RemoveHandler Proc.OnDetachFromCurrentProcess, AddressOf Proc_OnDetachFromCurrentProcess
    End Sub

    Private Sub Proc_OnDetachFromCurrentProcess(handle As IntPtr)
        UndoCodeInjection()
    End Sub

    Private Sub Proc_OnAttachToCurrentProcess(handle As IntPtr)
        CompletelyReInitializeAndInjectCodeInNewLocation()
    End Sub

    Private Sub InitAsmBuffer()
        AsmBuffer.Position = 0
        Dim asm As New X86Writer(AsmBuffer, CurrentCodeLocation)
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
        asm.Mov32(Reg32.EBX, CurrentCodeLocation.ToInt32() + FUNC_RETURN_ADDR_OFFSET)
        '(LOC)
        AsmLocAfterSetReturnLocation = New MoveableAddressOffset(Me, asm.Position)
        asm.Mov32(New X86Address(Reg32.EBX, 0), Reg32.EAX) 'mov [ebx], eax
        asm.Pop32(Reg32.EAX)
        For i = 1 To BUFFER_STACK_SIZE
            asm.Pop32(Reg32.EAX)
        Next
        asm.Mov32(Reg32.ESP, Reg32.EBP)
        asm.Pop32(Reg32.EBP)
        asm.Retn()
        '(LOC)
        AsmLocAfterReturn = New MoveableAddressOffset(Me, asm.Position)

        Dim remainingBytes = FUNCTION_CALL_ASM_BUFFER_SIZE - AsmBuffer.Position

        If remainingBytes > 0 Then
            Dim dummyByteArr(remainingBytes) As Byte
            Array.Clear(dummyByteArr, 0, remainingBytes)
            AsmBuffer.Write(dummyByteArr, 0, remainingBytes)
        End If

    End Sub

    Private Sub ____freeClrManagedResources()
        UnhookEvents()
    End Sub

    Private Sub ____freeNativeUnmanagedResources()
        UndoCodeInjection()
    End Sub

    Private Function ExecuteAsm() As Byte()
        Dim threadHandle As IntPtr = IntPtr.Zero
        threadHandle = CreateRemoteThread(CurrentProcessHandle, 0, 0, CurrentCodeLocation, 0, 0, 0)
        WaitForSingleObject(threadHandle, MAX_WAIT)
        CloseHandle(threadHandle)

        Dim result(INT32_SIZE - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, CurrentCodeLocation + FUNC_RETURN_ADDR_OFFSET, result, INT32_SIZE, New Integer())
        Return result
    End Function

    Private Function SquashIntoDword(ByRef allocPtrList As List(Of IngameAllocatedPtr), arg As Object) As Int32
        Dim size = Marshal.SizeOf(arg)
        If size <= INT32_SIZE Then
            If TypeOf arg Is Int32 Then
                Return arg
            ElseIf TypeOf arg Is Double Then
                Return ToDwordLossy(arg)
            ElseIf TypeOf arg Is IntPtr Then
                Return ToDwordLossy(arg)
            Else
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
                Return Convert.ToInt32(arg) 'If it reaches here without returning, just give up and try good ol' Convert.ToInt32()
            End If
        Else
            'Allocate a place for our arg
            Dim ptrToArg As IntPtr = Marshal.AllocHGlobal(size)

            Try
                'Move arg to where that pointer points
                Marshal.StructureToPtr(arg, ptrToArg, True)
                allocPtrList.Add(New IngameAllocatedPtr(size))
                Dim allocPtrAddr = allocPtrList(allocPtrList.Count - 1).Address
                WriteProcessMemory(CurrentProcessHandle, allocPtrAddr, ptrToArg, size, New Integer())
                Return allocPtrAddr

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
    End Function

    Private Function GetFunctionCallResult(returnType As Int32, result As Byte()) As Object
        Select Case returnType
            Case RETURN_TYPE_VOID : Return 0
            Case RETURN_TYPE_INT : Return New MutatableDword(result).Int1
            Case RETURN_TYPE_UINT : Return New MutatableDword(result).UInt1
            Case RETURN_TYPE_SHORT : Return New MutatableDword(result).Short1
            Case RETURN_TYPE_USHORT : Return New MutatableDword(result).UShort1
            Case RETURN_TYPE_BOOL : Return New MutatableDword(result).Bool1
            Case RETURN_TYPE_BYTE : Return New MutatableDword(result).Byte1
            Case RETURN_TYPE_SBYTE : Return New MutatableDword(result).SByte1
            Case RETURN_TYPE_FLOAT : Return New MutatableDword(result).Float1
            Case RETURN_TYPE_DOUBLE : Return CType(New MutatableDword(result).Float1, Double)
            Case RETURN_TYPE_STR : Return RAsciiStr(New MutatableDword(result).Int1)
            Case RETURN_TYPE_UNISTR : Return RUnicodeStr(New MutatableDword(result).Int1)
            Case Else
                Return BitConverter.ToInt32(result, 0)
        End Select
    End Function

    Public Function CallIngameLua(returnType As Int32, functionAddress As Integer, args As NLua.LuaTable) As Object
        Dim result As Int32 = FUNCCALL_ERR
        PatchLuaFunctionCallAddress(functionAddress)
        If args IsNot Nothing AndAlso args.Values.Count > 0 Then
            Dim ptrList As New List(Of IngameAllocatedPtr)
            Try
                PatchStackValues(args.Values.OfType(Of Object).Select(Function(a) SquashIntoDword(ptrList, a)))

                result = GetFunctionCallResult(returnType, ExecuteAsm())
            Catch ex As Exception
                Throw ex
            Finally
                For Each ptr In ptrList
                    ptr.Dispose()
                Next
            End Try
        End If

        Return result
    End Function

    Public Function FuncCall(Of T)(retType As IngameFuncReturnType, __func As String, luaTable As NLua.LuaTable) As T

        Dim result As T = Nothing

        Dim func = __func.ToUpper

        Dim funcInfoParamList = ScriptRes.autoCompleteFuncInfoByName(ScriptRes.caselessIngameFuncNames(func)).ParamList

        Dim Params() As Object = luaTable.Values.OfType(Of Object).
            ToArray()

        Using funcPtr = New IngameAllocatedPtr(FUNCTION_CALL_ASM_BUFFER_SIZE)
            Dim asmBuffer As New IO.MemoryStream(FUNCTION_CALL_ASM_BUFFER_SIZE)
            Dim asm As New X86Writer(asmBuffer, funcPtr.Address)
            Dim luaFuncAddress As New IntPtr(CType(ScriptRes.autoCompleteFuncInfoByName(ScriptRes.caselessIngameFuncNames(func.ToUpper)), IngameFuncInfo).Address)

            asm.Push32(X86Register32.EBP)
            asm.Mov32(X86Register32.EBP, X86Register32.ESP)
            asm.Push32(X86Register32.EAX)

            Dim pointerList = New List(Of IngameAllocatedPtr)

            'Parse params, add as variables to the ASM
            For i As Integer = Params.Length - 1 To 0 Step -1
                'Dim arg = GetFuncCallArgAsInteger(pointerList, funcInfoParamList(i), Params(i))
                'Dim debug_arg = GetFuncCallResult(Of T)(funcInfoParamList(i).IngameParamType, arg)
                'Dbg.PrintInfo($"Pushing function parameter value {arg.ToString()} ({debug_arg.ToString()}) before calling {__func}")
                asm.Mov32(X86Register32.EAX, GetFuncCallArgAsInteger(pointerList, funcInfoParamList(i), Params(i)))
                asm.Push32(X86Register32.EAX)
            Next

            asm.Call(luaFuncAddress)
            asm.Mov32(X86Register32.EBX, funcPtr.Address.ToInt32() + FUNC_RETURN_ADDR_OFFSET)
            asm.Mov32(New X86Address(X86Register32.EBX, 0), X86Register32.EAX) 'mov [ebx], eax
            asm.Pop32(X86Register32.EAX)

            For Each p In Params
                asm.Pop32(X86Register32.EAX)
            Next

            asm.Mov32(X86Register32.ESP, X86Register32.EBP)
            asm.Pop32(X86Register32.EBP)
            asm.Retn()

            'Write from the beginning up to where the opcode bytes stop
            If Not WriteProcessMemory(_targetProcessHandle, funcPtr.Address, asmBuffer.ToArray(), CType(asmBuffer.Position, Int32), 0) Then
                Throw New Exception($"Failed to write process memory for FuncCall of {func}")
            End If

            If Not FlushInstructionCache(_targetProcessHandle, funcPtr.Address, CType(asmBuffer.Position, Int32)) Then
                Throw New Exception($"Failed to flush instruction cache for FuncCall of {func}")
            End If

            Dim threadHandle As IntPtr = IntPtr.Zero
            threadHandle = CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr.Address, 0, 0, 0)
            WaitForSingleObject(threadHandle, MAX_WAIT)

            If Not CloseHandle(threadHandle) Then
                Throw New Exception($"Failed to close thread handle for FuncCall of {func}")
            End If

            asmBuffer.Dispose()

            For Each ptr In pointerList
                ptr.Dispose() 'Deallocates memory for any params that were allocated in game memory.
            Next

            result = GetFuncCallResult(Of T)(retType, RInt32(funcPtr.Address + FUNC_RETURN_ADDR_OFFSET))

            'Dbg.PrintInfo($"FuncCall to '{__func}' returned {result}.")
        End Using

        Return result
    End Function

    Public Function GetFuncCallResult(Of T)(ByVal retType As IngameFuncReturnType, ByVal num As Integer) As T
        Dim typeT = GetType(T)

        Select Case retType
            Case IngameFuncReturnType.Undefinerino : Return CTypeDynamic(Of T)(num)
            Case IngameFuncReturnType.Integerino : Return CTypeDynamic(Of T)(num)
            Case IngameFuncReturnType.Boolerino : Return CTypeDynamic(Of T)(If(num > 0, True, False))
            Case IngameFuncReturnType.Floatarino : Return CTypeDynamic(Of T)(BitConverter.ToSingle(BitConverter.GetBytes(num), 0))
            Case IngameFuncReturnType.AsciiStringerino : Return CTypeDynamic(Of T)(RAsciiStr(num))
            Case IngameFuncReturnType.UnicodeStringerino : Return CTypeDynamic(Of T)(RUnicodeStr(num))
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetFuncCallArgAsInteger(ByRef ptrList As List(Of IngameAllocatedPtr), pInfo As ParamInfo, ByVal obj As Object) As Integer
        If obj Is Nothing Then
            Return 0
        End If

        Select Case pInfo.IngameParamType
            Case IngameFuncReturnType.AsciiStringerino
                Dim text As String = CType(obj, String)
                Dim strPtr As New IngameAllocatedPtr(text.Length + 1)
                WAsciiStr(strPtr.Address, text)
                ptrList.Add(strPtr)
                Return strPtr.Address
            Case IngameFuncReturnType.UnicodeStringerino
                Dim text As String = CType(obj, String)
                Dim strPtr As New IngameAllocatedPtr((text.Length + 1) * 2)
                WUnicodeStr(strPtr.Address, text)
                ptrList.Add(strPtr)
                Return strPtr.Address
            Case IngameFuncReturnType.Integerino
                If (obj.GetType() = GetType(Single)) Then
                    Dim objFloat = CType(obj, Single)
                    Return CType(objFloat, Integer)
                ElseIf obj.GetType() = GetType(Double) Then
                    Dim objDouble = CType(obj, Double)
                    Return CType(objDouble, Integer)
                End If
        End Select

        'TODO: Check the endianness when converting <= 4 byte values
        Dim byt As Byte() = {0, 0, 0, 0}
        Dim bytConv As Byte() = BitConverter.GetBytes(CTypeDynamic(obj, pInfo.ParamType))
        bytConv.CopyTo(byt, 0)
        Return BitConverter.ToInt32(byt, 0)
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