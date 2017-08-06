Imports System.Globalization
Imports System.Reflection

Public Class ScriptEnvironment
    Private _scriptvars As Dictionary(Of String, ScriptVar)

    Public clsFuncNames As New Hashtable
    Public clsFuncLocs As New Hashtable

    Public Shared Current As ScriptEnvironment

    Public funcPtr As Integer

    Public Sub funcAlloc()
        Dim TargetBufferSize = 1024
        funcPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Sub

    Shared Sub New()
        Current = New ScriptEnvironment()
    End Sub

    Public Sub New()
        _scriptvars = New Dictionary(Of String, ScriptVar)
        DataHash.ParseItems(clsFuncNames, clsFuncLocs, My.Resources.FuncLocs)
    End Sub

    Public Shared Function Run(ByVal str As String) As Integer
        Return Current.ExecuteScriptLine(str)
    End Function

    Private Function IsScriptVarDefined(name As String)
        Return _scriptvars.ContainsKey(name.ToLower())
    End Function

    Private Function GetScriptVar(name As String) As ScriptVar
        Return _scriptvars(name.ToLower())
    End Function

    Private Function DefineScriptVar(svar As ScriptVar) As ScriptVar
        If IsScriptVarDefined(svar.Name) Then
            Throw New ScriptVarAlreadyDefinedException(svar)
        End If

        _scriptvars.Add(svar.Name, svar)

        Return _scriptvars(svar.Name)
    End Function

    Private Function CheckSpecialActions(fullLine As String) As Boolean

        Dim params = New List(Of String)
        Dim splitByEq = fullLine.Split(New Char() {"="c})

        Dim first = True

        For Each eq In splitByEq

            Dim splitBySpace = eq.Trim().Split(New Char() {" "c})

            If Not first Then
                params.Add("=")
            End If

            For Each sp In splitBySpace
                Dim sptr = sp.Trim
                If Not String.IsNullOrWhiteSpace(sptr) Then
                    params.Add(sp)
                End If
            Next

            first = False
        Next

        Return CheckSpecialActions(params.ToArray)

    End Function



    Private Function CheckSpecialActions(params() As String) As Boolean
        If params(0) = "new" Then
            If params.Length = 3 Then
                DefineScriptVar(ScriptVar.Parse(params(2), params(1)))
                Return True
            ElseIf params.Length >= 5 And params(3) = "=" Then

                If params.Length = 5 Then
                    DefineScriptVar(ScriptVar.Parse(params(2), params(1), params(4)))
                    Return True
                Else
                    Dim valueStr = ""
                    For i = 4 To params.Length - 1
                        valueStr = valueStr & params(i) & " "
                    Next

                    Dim intResult = ScriptEnvironment.Run(valueStr)
                    Dim sv = DefineScriptVar(ScriptVar.Parse(params(2), params(1)).ReadFromFunctionResult(intResult))

                    Return True

                End If

            End If
        End If

        Return False
    End Function

    Public Sub Clear()

        _scriptvars.Clear()

    End Sub

    Private Function ExecuteScriptLine(ByVal inputStr As String) As Integer

        If CheckSpecialActions(inputStr.ToLower.Trim) Then
            Return 0
        End If

        Dim str = inputStr
        Dim action As String
        Dim params() As String = {}

        Dim storedVal As String = ""

        Str = Str.ToLower

        If Str.Contains("=") Then
            storedVal = Str.Replace(" ", "").Split("=")(0)
            Str = Str.Split("=")(1).TrimStart(" ")
        End If


        action = Str.Split(" ")(0).ToLower
        If clsFuncLocs.Contains(action) Then
            Str = "funccall " & action & ", " & Str.ToLower.Replace(action, "")
            action = "funccall"
        Else
            Str = Str.ToLower.Replace(action, "")
        End If

        If Str.Contains(" ") Then
            Str = Str.Replace(action & " ", "")
            params = Str.Replace(" ", "").Split(",")
        End If



        For i = 0 To params.Count - 1
            Dim p = params(i).ToLower
            If p = "true" Then
                params(i) = "1"
            ElseIf p = "false" Then
                params(i) = "0"
            ElseIf IsScriptVarDefined(p) Then
                params(i) = GetScriptVar(p).ToString()
            End If
        Next


        Dim t As Type = GetType(ScriptFunction)
        Dim pt As Type
        Dim method As MethodInfo

        method = t.GetMethod(action)
        'Check function validity.
        If ((method Is Nothing) Or (Not (method.IsPublic And method.IsStatic))) Then
            Throw New FunctionNoExistException(action)
        End If

        For i = 0 To (method.GetParameters.Count - params.Length) - 1
            Array.Resize(params, params.Length + 1)
            params(params.Length - 1) = "0"
        Next

        Dim typedParams() As Object = {}
        For i = 0 To method.GetParameters.Count - 1
            Array.Resize(typedParams, typedParams.Length + 1)

            If method.GetParameters(i).ParameterType.IsByRef Then
                pt = method.GetParameters(i).ParameterType.GetElementType()
            Else
                pt = method.GetParameters(i).ParameterType()
            End If

            'Fix for non-decimal using regions
            If pt.Name = "Single" Then
                typedParams(typedParams.Length - 1) = Convert.ToSingle(params(i), New CultureInfo("en-us"))
            Else
                typedParams(typedParams.Length - 1) = CTypeDynamic(params(i), pt)
            End If
        Next


        Dim result As Integer
        result = method.Invoke(Me, typedParams)

        If IsScriptVarDefined(storedVal) Then
            _scriptvars(storedVal).ReadFromFunctionResult(result)
        End If

        Return result

    End Function


End Class
