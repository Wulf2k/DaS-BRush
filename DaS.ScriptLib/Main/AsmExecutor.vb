Imports System.Globalization
Imports System.Runtime.InteropServices

Friend Class AsmExecutor

    'TODO:  Deal with jumps to points not yet defined
    Public bytes() As Byte = {}

    Public pos As Int32

    Public Shared ReadOnly DefaultReturnValue = 1337
    Public Shared ReadOnly ReturnValueCheckInterval = 5

    Private reg8 As Hashtable = New Hashtable
    Private reg16 As Hashtable = New Hashtable
    Private reg32 As Hashtable = New Hashtable
    Private code As Hashtable = New Hashtable
    Private vars As Hashtable = New Hashtable

    Private varrefs As New SortedList(Of Integer, String)

    Const MaxFuncRetries = 8

    Public Sub New()
        pos = 0
        init()
    End Sub

    Private Sub init()

        reg8.Clear()
        reg8.Add("al", 0)
        reg8.Add("cl", 1)
        reg8.Add("dl", 2)
        reg8.Add("bl", 3)
        reg8.Add("ah", 4)
        reg8.Add("ch", 5)
        reg8.Add("dh", 6)
        reg8.Add("bh", 7)

        reg16.Clear()
        reg16.Add("ax", 0)
        reg16.Add("cx", 1)
        reg16.Add("dx", 2)
        reg16.Add("bx", 3)

        reg32.Clear()
        reg32.Add("eax", 0)
        reg32.Add("ecx", 1)
        reg32.Add("edx", 2)
        reg32.Add("ebx", 3)
        reg32.Add("esp", 4)
        reg32.Add("ebp", 5)
        reg32.Add("esi", 6)
        reg32.Add("edi", 7)

        code.Clear()
        code.Add("inc", &H40)
        code.Add("dec", &H48)
        'code.Add("push", &H50)
        code.Add("pop", &H58)
        code.Add("pushad", &H60)
        code.Add("popad", &H61)
    End Sub

    Public Sub Add(ByVal newbytes() As Byte)
        bytes = bytes.Concat(newbytes).ToArray
    End Sub

    Public Sub AddVar(ByVal name As String, hexval As String)
        AddVar(name, Convert.ToInt32(Microsoft.VisualBasic.Right(hexval, hexval.Length - 2), 16))
    End Sub

    Public Sub AddVar(ByVal name As String, val As IntPtr)
        AddVar(name, CInt(val))
    End Sub

    Public Sub AddVar(ByVal name As String, val As Int32)
        name = name.Replace(":", "")

        If Not vars.Contains(name) Then
            vars.Add(name, val)
        Else
            vars(name) = val
            For Each entry In varrefs
                If entry.Value = name Then
                    Dim tmpbyt() As Byte

                    Select Case bytes(entry.Key)
                        Case &HE8, &HE9
                            tmpbyt = BitConverter.GetBytes(val - (pos - (bytes.Length - entry.Key)) - 5)
                            Array.Copy(tmpbyt, 0, bytes, entry.Key + 1, tmpbyt.Length)

                        Case &HF
                            tmpbyt = BitConverter.GetBytes(val - (pos - (bytes.Length - entry.Key)) - 6)
                            Array.Copy(tmpbyt, 0, bytes, entry.Key + 2, tmpbyt.Length)

                    End Select
                End If
            Next

        End If
    End Sub

    Public Sub Clear()
        bytes = {}
        vars.Clear()
        varrefs.Clear()
        pos = 0

    End Sub

    Private Sub ParseInput(ByVal str As String,
                           ByRef cmd As String,
                           ByRef reg1 As String, ByRef reg2 As String,
                           ByRef ptr1 As Boolean, ByRef ptr2 As Boolean,
                           ByRef plus1 As Int32, ByRef plus2 As Int32,
                           ByRef val1 As Int32, ByRef val2 As Int32)

        'Raw parameters
        Dim params As String = ""
        Dim param1 As String = ""
        Dim param2 As String = ""

        'Separate Command from params
        If str.Contains(" ") Then
            cmd = str.Split(" ")(0)
            params = Microsoft.VisualBasic.Right(str, str.Length - cmd.Length)
            params = params.Replace(" ", "")
        Else
            cmd = str
        End If

        'Check for section name
        If cmd.Contains(":") Then
            AddVar(cmd, pos)
            Return
        End If

        'Split params
        If params.Contains(",") Then
            param2 = params.Split(",")(1)
        End If
        param1 = params.Split(",")(0)

        'Check if immediate or pointers
        If param1.Contains("[") Then
            ptr1 = True
            param1 = param1.Replace("[", "")
            param1 = param1.Replace("]", "")
        End If
        If param2.Contains("[") Then
            ptr2 = True
            param2 = param2.Replace("[", "")
            param2 = param2.Replace("]", "")
        End If

        'Check if there are offsets in params
        If param1.Contains("+") Or param1.Contains("-") Then
            If param1.Contains("0x") Then
                plus1 = Convert.ToInt32(param1(3) & Microsoft.VisualBasic.Right(param1, param1.Length - 6), 16)
            Else
                plus1 = Convert.ToInt32(param1(3) & Microsoft.VisualBasic.Right(param1, param1.Length - 4))
            End If
            param1 = param1.Split("+")(0)
            param1 = param1.Split("-")(0)
        End If
        If param2.Contains("+") Or param2.Contains("-") Then
            If param2.Contains("0x") Then
                'plus2 = Convert.ToInt32(param2(3) & Microsoft.VisualBasic.Right(param2, param2.Length - 6), 16)
                plus2 = Convert.ToInt32(Microsoft.VisualBasic.Right(param2, param2.Length - 4), 16)
                If param2(3) = "-" Then plus2 *= -1
            Else
                plus2 = Convert.ToInt32(param2(3) & Microsoft.VisualBasic.Right(param2, param2.Length - 4))
            End If
            param2 = param2.Split("+")(0)
            param2 = param2.Split("-")(0)
        End If

        'If not registers, convert params from hex to dec
        If param1.Contains("0x") Then
            val1 = Convert.ToInt32(param1, 16)
        End If
        If param2.Contains("0x") Then
            val2 = Convert.ToInt32(param2, 16)
        End If

        'If numeric, set values
        If IsNumeric(param1) Then
            val1 = param1
        End If
        If IsNumeric(param2) Then
            val2 = param2
        End If

        'Define registers, if not values
        If reg32.Contains(param1) Then reg1 = param1
        If reg32.Contains(param2) Then reg2 = param2
        If reg16.Contains(param1) Then reg1 = param1
        If reg16.Contains(param2) Then reg2 = param2
        If reg8.Contains(param1) Then reg1 = param1
        If reg8.Contains(param2) Then reg2 = param2

        'If param is previously defined section
        If vars.Contains(param1) Then
            val1 = vars(param1)
            varrefs.Add(bytes.Length, param1)
        End If
        If vars.Contains(param2) Then
            val2 = vars(param2)
            varrefs.Add(bytes.Length, param2)
        End If

    End Sub

    Public Sub Asm(ByVal str As String)
        Dim cmd As String = ""

        'Registers used
        Dim reg1 As String = ""
        Dim reg2 As String = ""

        'Are registers immediate or pointers
        Dim ptr1 As Boolean = False
        Dim ptr2 As Boolean = False

        'Offsets from registers
        Dim plus1 As Int32 = 0
        Dim plus2 As Int32 = 0

        'Values, if not registers
        Dim val1 As Int32 = 0
        Dim val2 As Int32 = 0

        ParseInput(str, cmd, reg1, reg2, ptr1, ptr2, plus1, plus2, val1, val2)

        Dim newbytes() As Byte = {}

        'Check if command is simple 1-byte command
        If code.Contains(cmd) Then
            newbytes = {0}
            newbytes(0) = code(cmd)
            If reg32.Contains(reg1) Then
                newbytes(0) = newbytes(0) Or reg32(reg1)
            End If
            Add(newbytes)
            pos += newbytes.Count
            Return
        End If

        Select Case cmd
            Case "add"
                If reg32.Contains(reg1) And reg2 = "" Then
                    newbytes = {&H81, &HC0}
                    If Math.Abs(val2) < &H80 Then
                        newbytes(0) = newbytes(0) Or 2
                        newbytes = newbytes.Concat({val2 And &HFF}).ToArray
                    Else
                        If reg1 = "eax" Then
                            newbytes = {5}
                        End If
                        newbytes = newbytes.Concat(BitConverter.GetBytes(val2)).ToArray
                    End If
                    newbytes(1) = newbytes(1) Or reg32(reg1)
                End If

                If reg32.Contains(reg1) And reg32.Contains(reg2) Then
                    newbytes = {1, 0}
                    If ptr1 Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                    If ptr2 Then
                        newbytes(0) = newbytes(0) Or &H2
                        newbytes(1) = newbytes(1) Or (reg32(reg1) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg2)
                    End If

                    If Not (ptr1 Or ptr2) Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes(1) = newbytes(1) Or &HC0
                    End If

                    Dim offset
                    offset = plus1 + plus2

                    If Math.Abs(offset) < &H80 Then
                        If offset > 0 Then
                            newbytes(1) = newbytes(1) Or &H40
                            newbytes = newbytes.Concat({offset And &HFF}).ToArray
                        End If
                    End If
                    If Math.Abs(offset) > &H7F Then
                        newbytes(1) = newbytes(1) Or &H80
                        newbytes = newbytes.Concat(BitConverter.GetBytes(offset)).ToArray
                    End If

                    If Not ptr1 And Not ptr2 Then
                        newbytes = {1, &HC0}
                        newbytes(1) = newbytes(1) Or reg32(reg2) * 8
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "and"
                If reg32.Contains(reg1) And reg2 = "" Then
                    newbytes = {&H83, &HE0}
                    If Math.Abs(val2) < &H80 Then
                        newbytes(0) = newbytes(0) Or 2
                        newbytes = newbytes.Concat({val2 And &HFF}).ToArray
                    Else
                        If reg1 = "eax" Then
                            newbytes = {&H25}
                        End If
                        newbytes = newbytes.Concat(BitConverter.GetBytes(val2)).ToArray
                    End If
                    newbytes(1) = newbytes(1) Or reg32(reg1)
                End If

                If reg32.Contains(reg1) And reg32.Contains(reg2) Then
                    newbytes = {&H21, 0}
                    If ptr1 Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                    If ptr2 Then
                        newbytes(0) = newbytes(0) Or &H2
                        newbytes(1) = newbytes(1) Or (reg32(reg1) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg2)
                    End If

                    If Not (ptr1 Or ptr2) Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes(1) = newbytes(1) Or &HC0
                    End If

                    Dim offset
                    offset = plus1 + plus2

                    If Math.Abs(offset) < &H80 Then
                        If offset > 0 Then
                            newbytes(1) = newbytes(1) Or &H40
                            newbytes = newbytes.Concat({offset And &HFF}).ToArray
                        End If
                    End If
                    If Math.Abs(offset) > &H7F Then
                        newbytes(1) = newbytes(1) Or &H80
                        newbytes = newbytes.Concat(BitConverter.GetBytes(offset)).ToArray
                    End If

                    If Not ptr1 And Not ptr2 Then
                        newbytes = {&H21, &HC0}
                        newbytes(1) = newbytes(1) Or reg32(reg2) * 8
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "call"
                If Not ptr1 Then
                    If reg32.Contains(reg1) Then
                        'Is only a register
                        newbytes = {&HFF, &HD0}
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    Else
                        newbytes = {&HE8}
                        Dim addr = Convert.ToInt32(val1) - pos - 5
                        newbytes = newbytes.Concat(BitConverter.GetBytes(addr)).ToArray

                    End If
                Else
                    'Is an offset from a register
                    If Math.Abs(plus1) < &H80 Then
                        If plus1 = 0 Then
                            newbytes = {&HFF, &H10}
                            newbytes(1) = newbytes(1) Or reg32(reg1)
                        Else
                            newbytes = {&HFF, &H50, 0}
                            newbytes(1) = newbytes(1) Or reg32(reg1)
                            newbytes(2) = plus1
                        End If
                    Else
                        newbytes = {&HFF, &H90}
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes = newbytes.Concat(BitConverter.GetBytes(plus1)).ToArray
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "cmp"
                If reg32.Contains(reg1) And reg2 = "" Then
                    newbytes = {&H81, &HF8}
                    If Math.Abs(val2) < &H80 Then
                        newbytes(0) = newbytes(0) Or 2
                        newbytes = newbytes.Concat({val2 And &HFF}).ToArray
                    Else
                        If reg1 = "eax" Then
                            newbytes = {&H3D}
                        End If
                        newbytes = newbytes.Concat(BitConverter.GetBytes(val2)).ToArray
                    End If
                    newbytes(1) = newbytes(1) Or reg32(reg1)
                End If

                If reg32.Contains(reg1) And reg32.Contains(reg2) Then
                    newbytes = {&H39, 0}
                    If ptr1 Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                    If ptr2 Then
                        newbytes(0) = newbytes(0) Or &H2
                        newbytes(1) = newbytes(1) Or (reg32(reg1) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg2)
                    End If

                    If Not (ptr1 Or ptr2) Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes(1) = newbytes(1) Or &HC0
                    End If

                    Dim offset
                    offset = plus1 + plus2

                    If Math.Abs(offset) < &H80 Then
                        If offset > 0 Then
                            newbytes(1) = newbytes(1) Or &H40
                            newbytes = newbytes.Concat({offset And &HFF}).ToArray
                        End If
                    End If
                    If Math.Abs(offset) > &H7F Then
                        newbytes(1) = newbytes(1) Or &H80
                        newbytes = newbytes.Concat(BitConverter.GetBytes(offset)).ToArray
                    End If

                    If Not ptr1 And Not ptr2 Then
                        newbytes = {&H39, &HC0}
                        newbytes(1) = newbytes(1) Or reg32(reg2) * 8
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "je"
                newbytes = {&HF, &H84}
                Dim addr = Convert.ToInt32(val1) - pos - 6
                newbytes = newbytes.Concat(BitConverter.GetBytes(addr)).ToArray
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "jmp"
                If Not ptr1 Then
                    If reg32.Contains(reg1) Then
                        'Is only a register
                        newbytes = {&HFF, &HE0}
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    Else
                        newbytes = {&HE9}
                        Dim addr = Convert.ToInt32(val1) - pos - 5
                        newbytes = newbytes.Concat(BitConverter.GetBytes(addr)).ToArray

                    End If
                Else
                    'Is an offset from a register
                    If Math.Abs(plus1) < &H80 Then
                        If plus1 = 0 Then
                            newbytes = {&HFF, &H20}
                            newbytes(1) = newbytes(1) Or reg32(reg1)
                        Else
                            newbytes = {&HFF, &H60, 0}
                            newbytes(1) = newbytes(1) Or reg32(reg1)
                            newbytes(2) = plus1 And &HFF
                        End If
                    Else
                        newbytes = {&HFF, &HA0}
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes = newbytes.Concat(BitConverter.GetBytes(plus1)).ToArray
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "jne"
                newbytes = {&HF, &H85}
                Dim addr = Convert.ToInt32(val1) - pos - 6
                newbytes = newbytes.Concat(BitConverter.GetBytes(addr)).ToArray
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "mov"
                'TODO:  Complete
                If reg8.Contains(reg1) And reg8.Contains(reg2) Then
                    newbytes = {&H88, &HC0}
                    newbytes(1) = newbytes(1) Or reg8(reg1)
                    newbytes(1) = newbytes(1) Or reg8(reg2) * 8
                    'TODO:  Complete
                End If

                If reg32.Contains(reg1) And reg2 = "" Then
                    newbytes = {&HB8}
                    newbytes(0) = newbytes(0) Or reg32(reg1)
                    newbytes = newbytes.Concat(BitConverter.GetBytes(val2)).ToArray
                End If

                If reg32.Contains(reg1) And reg32.Contains(reg2) Then
                    newbytes = {&H89, 0}

                    If ptr1 Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    End If
                    If ptr2 Then
                        newbytes(0) = newbytes(0) Or &H2
                        newbytes(1) = newbytes(1) Or (reg32(reg1) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg2)
                    End If

                    If Not (ptr1 Or ptr2) Then
                        newbytes(1) = newbytes(1) Or (reg32(reg2) * 8)
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes(1) = newbytes(1) Or &HC0
                    End If

                    Dim offset
                    offset = plus1 + plus2

                    If (ptr1 And reg1 = "esp") Or (ptr2 And reg2 = "esp") Then
                        newbytes = newbytes.Concat({&H24}).ToArray
                    End If

                    If Math.Abs(offset) < &H80 Then
                        If Math.Abs(offset) > 0 Or (ptr2 And reg2 = "ebp") Or (ptr1 And reg1 = "ebp") Then
                            newbytes(1) = newbytes(1) Or &H40
                            newbytes = newbytes.Concat({offset And &HFF}).ToArray
                        End If
                    End If
                    If Math.Abs(offset) > &H7F Then
                        newbytes(1) = newbytes(1) Or &H80
                        newbytes = newbytes.Concat(BitConverter.GetBytes(offset)).ToArray
                    End If
                End If

                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "push"
                If Not ptr1 Then
                    If reg32.Contains(reg1) Then
                        'Is only a register
                        newbytes = {&H50}
                        newbytes(0) = newbytes(0) Or reg32(reg1)
                    Else
                        If Math.Abs(val1) < &H100 Then
                            newbytes = {&H6A, 0}
                            newbytes(1) = val1 And &HFF
                        Else
                            newbytes = {&H68}
                            newbytes = newbytes.Concat(BitConverter.GetBytes(val1)).ToArray
                        End If
                    End If
                Else
                    'Is an offset from a register
                    If Math.Abs(plus1) < &H80 Then
                        If plus1 = 0 Then
                            'No Offset
                            newbytes = {&HFF, &H30}
                        Else
                            'Offset between 0 and 0xFF
                            newbytes = {&HFF, &H70, 0}
                            newbytes(2) = plus1 And &HFF
                        End If
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                    Else
                        'Offset is > 0xFF
                        newbytes = {&HFF, &HB0}
                        newbytes(1) = newbytes(1) Or reg32(reg1)
                        newbytes = newbytes.Concat(BitConverter.GetBytes(plus1)).ToArray
                    End If
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "ret"
                newbytes = {&HC2}
                If Math.Abs(val1) > 0 Then
                    newbytes = newbytes.Concat(BitConverter.GetBytes(Convert.ToInt16(val1))).ToArray
                Else
                    newbytes(0) = newbytes(0) Or 1
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

            Case "shl", "shr"
                'TODO:  Handle reg1 = ax, al
                If reg32.Contains(reg1) Then
                    If reg2 = "cl" Then
                        newbytes = {&HD3, &HE0}
                    End If
                    If reg2 = "" Then
                        newbytes = {&HC1, &HE0}
                        newbytes = newbytes.Concat({val2 And &HFF}).ToArray
                    End If
                    newbytes(1) = newbytes(1) Or reg32(reg1)
                    If cmd = "shr" Then newbytes(1) = newbytes(1) Or &H8
                End If
                Add(newbytes)
                pos += newbytes.Count
                Return

        End Select
    End Sub

    Public Overrides Function ToString() As String
        Dim tmpstr As String = ""

        For Each byt In bytes
            tmpstr += "0x" & Hex(byt).PadLeft(2, "0") & ", "
        Next

        Return tmpstr
    End Function

    Private Shared Function GetFuncCallParamValue(funcInfoParamList As List(Of ParamInfo), index As Integer, paramVal As Object) As Object
        Return CTypeDynamic(paramVal, funcInfoParamList(index).ParamType)
    End Function

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
            Case Else
                'TODO: Check the endianness when converting <= 4 byte values
                Dim byt As Byte() = {0, 0, 0, 0}
                Dim bytConv As Byte() = BitConverter.GetBytes(CTypeDynamic(obj, pInfo.ParamType))
                bytConv.CopyTo(byt, 0)
                Return BitConverter.ToInt32(byt, 0)
        End Select
    End Function

    Friend Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, __func As String, luaTable As NLua.LuaTable) As T

        Dim result As T

        Dim param As IntPtr = Marshal.AllocHGlobal(4)
        Dim a As New AsmExecutor

        Dim func = __func.ToUpper

        Dim funcInfoParamList = ScriptRes.autoCompleteFuncInfoByName(ScriptRes.caselessIngameFuncNames(func)).ParamList

        Dim Params() As Object = luaTable.Values.OfType(Of Object).
            TakeWhile(Function(x) x IsNot Nothing).
            Select(Function(x, i) GetFuncCallParamValue(funcInfoParamList, i, x)).
            ToArray()

        Using funcPtr = New IngameAllocatedPtr(1024)
            a.pos = funcPtr.Address
            a.AddVar("funcloc", CType(ScriptRes.autoCompleteFuncInfoByName(ScriptRes.caselessIngameFuncNames(func.ToUpper)), IngameFuncInfo).Address)
            a.AddVar("returnedloc", funcPtr.Address + &H200)

            a.Asm("push ebp")
            a.Asm("mov ebp,esp")
            a.Asm("push eax")

            Dim pointerList = New List(Of IngameAllocatedPtr)

            'Parse params, add as variables to the ASM
            For i As Integer = Params.Length - 1 To 0 Step -1

                a.AddVar("param" & i, GetFuncCallArgAsInteger(pointerList, funcInfoParamList(i), Params(i)))
                a.Asm("mov eax,param" & i)
                a.Asm("push eax")

            Next
            a.Asm("call funcloc")
            a.Asm("mov ebx,returnedloc")
            a.Asm("mov [ebx],eax")
            a.Asm("pop eax")

            For Each p In Params
                a.Asm("pop eax")
            Next

            a.Asm("mov esp,ebp")
            a.Asm("pop ebp")
            a.Asm("ret")

            Marshal.FreeHGlobal(param)

            WriteProcessMemory(_targetProcessHandle, funcPtr.Address, a.bytes, 1024, 0)


            Dim waitResult As WaitObjResult = WaitObjResult.WAIT_FAILED
            Dim tryCount As Integer = 0

            Dim threadHandle = CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr.Address, 0, 0, 0)

            If Not (threadHandle = IntPtr.Zero) Then
                waitResult = WaitForSingleObject(threadHandle, &HFFFFFFFF)
            End If

            'Waits briefly and retries:
            While (waitResult = WaitObjResult.WAIT_FAILED)

                Threading.Thread.Sleep(ReturnValueCheckInterval)

                If Not (threadHandle = IntPtr.Zero) Then
                    waitResult = WaitForSingleObject(threadHandle, &HFFFFFFFF)
                End If

                tryCount += 1

                If tryCount > MaxFuncRetries Then
                    Dbg.PrintErr($"CallFunc to {__func} reached max retry count of {MaxFuncRetries}.")
                    CloseHandle(threadHandle)
                    Return Nothing ' this might cause an error
                End If

            End While

            CloseHandle(threadHandle)

            For Each ptr In pointerList
                ptr.Dispose() 'Deallocates memory for any params that were allocated in game memory.
            Next

            result = GetFuncCallResult(Of T)(retType, RInt32(funcPtr.Address + &H200))

            Dbg.PrintInfo($"FuncCall to '{func}' returned {result} in {tryCount} tries.")
        End Using

        Return result
    End Function

End Class