Imports System.Globalization
Imports System.Runtime.InteropServices
Imports Managed.X86

Friend Class AsmExecutor

    'TODO:  Deal with jumps to points not yet defined

    Public Shared ReadOnly BufferSize As Integer = 1024
    Public Shared ReadOnly ReturnValueCheckInterval As Integer = 5
    Public Shared ReadOnly ReturnLblOffset As Integer = &H200
    ''' <summary>
    ''' Milliseconds
    ''' </summary>
    Public Const MAX_WAIT As Integer = 33

    Friend Shared Function GetFuncCallResult(Of T)(ByVal retType As IngameFuncReturnType, ByVal num As Integer) As T
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

    Friend Shared Function GetFuncCallArgAsInteger(ByRef ptrList As List(Of IngameAllocatedPtr), pInfo As ParamInfo, ByVal obj As Object) As Integer
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
            Case Else
                'TODO: Check the endianness when converting <= 4 byte values
                Dim byt As Byte() = {0, 0, 0, 0}
                Dim bytConv As Byte() = BitConverter.GetBytes(CTypeDynamic(obj, pInfo.ParamType))
                bytConv.CopyTo(byt, 0)
                Return BitConverter.ToInt32(byt, 0)
        End Select
    End Function

    Friend Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, __func As String, luaTable As NLua.LuaTable) As T

        Dim result As T = Nothing

        Dim func = __func.ToUpper

        Dim funcInfoParamList = ScriptRes.autoCompleteFuncInfoByName(ScriptRes.caselessIngameFuncNames(func)).ParamList

        Dim Params() As Object = luaTable.Values.OfType(Of Object).
            ToArray()

        Using funcPtr = New IngameAllocatedPtr(BufferSize)
            Dim asmBuffer As New IO.MemoryStream(BufferSize)

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
            asm.Mov32(X86Register32.EBX, funcPtr.Address.ToInt32() + ReturnLblOffset)
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

            result = GetFuncCallResult(Of T)(retType, RInt32(funcPtr.Address + ReturnLblOffset))

            'Dbg.PrintInfo($"FuncCall to '{__func}' returned {result}.")
        End Using

        Return result
    End Function

End Class