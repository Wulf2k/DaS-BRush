Imports System.Globalization
Imports System.Reflection
Imports System.Threading

Public Structure ScriptThreadParams
    Public ScriptRef As Script
    Public ThisThread As Thread
    Public IsItEvenAThread As Boolean
    Public IsFinished As Boolean

    Public Sub New(ByRef scrRef As Script, ByRef thisThred As Thread)
        ScriptRef = scrRef
        ThisThread = thisThred
        IsItEvenAThread = True
        IsFinished = False
    End Sub
    Public Shared Function GetNoThread(ByRef scrRef As Script) As ScriptThreadParams
        Return New ScriptThreadParams() With {.IsItEvenAThread = False, .IsFinished = True, .ScriptRef = scrRef}
    End Function
End Structure

Public Class Script
    Public ReadOnly Lines As String()
    Public ReadOnly Name As String
    Public ReadOnly WorkerThreads As List(Of Thread)

    Public outputLock As New object
    Public outStr As String

    Public Sub New(name As String, scriptTxt As String)
        Lines = scriptTxt.FormatText.Split(vbCrLf)
        Me.Name = name
        WorkerThreads = New List(Of Thread)
    End Sub

    Public Sub New(name As String, scriptLines As String())
        Lines = scriptLines
        Me.Name = name
        WorkerThreads = New List(Of Thread)
    End Sub

    Public Function Execute() As ScriptThreadParams
        Dim params = ScriptThreadParams.GetNoThread(Me)
        _doExecute(params)
        Return params
    End Function

    Private Sub _doExecute(ByVal threadParams As ScriptThreadParams)

        Dim scriptVars As New Dictionary(Of String, ScriptVarBase)
        Dim lineNumber As Integer = 0
        For Each line In Lines

            lineNumber = lineNumber + 1

            Try
                Dim result As Integer
                Dim trimmedLine = line.Trim()
                If String.IsNullOrWhiteSpace(trimmedLine) Then
                    Continue For
                End If


                result = ExecuteScriptLine(scriptVars, trimmedLine)

                SyncLock outputLock
                    outStr &= "Line " & lineNumber & " - ''" & trimmedLine & "'':" & Environment.NewLine &
                           "    Hex: 0x" & Hex(result) & Environment.NewLine &
                           "    Int: " & result & Environment.NewLine &
                           "    Float: " & BitConverter.ToSingle(BitConverter.GetBytes(result), 0) & Environment.NewLine
                End SyncLock

            Catch tae As ThreadAbortException
                Return ' Dont show error box every time u click cancel
            Catch ex As Exception
                If MsgBox("[" & ex.GetType.ToString & "]" & Environment.NewLine &
                          "Could not parse line " & lineNumber & " (""" & line & """):" & Environment.NewLine & Environment.NewLine & ex.Message & Environment.NewLine & Environment.NewLine & "Would you like to continue executing the script from the next line?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return
                End If
            End Try
        Next

        If threadParams.IsItEvenAThread Then
            threadParams.ThisThread.Abort()
            threadParams.ScriptRef.WorkerThreads.Remove(threadParams.ThisThread)
        End If

        threadParams.IsFinished = True

    End Sub

    Public Sub CleanAbortedThreads()
        For i = 0 To WorkerThreads.Count - 1
            WorkerThreads.Remove(WorkerThreads(i))
            CleanAbortedThreads()
            Return
        Next
    End Sub

    Public Function ExecuteInBackground() As ScriptThreadParams
        Dim newThread = New Thread(AddressOf _doExecute) With {.IsBackground = True}
        Dim params = New ScriptThreadParams(Me, newThread)

        newThread.Start(params)

        Return params
    End Function

    'TODO: only here so current Script.Run implementation works while we go through and convert everything to the new format
    Public Shared Function RunOneLine(txt As String) As Integer
        Return New Script("Untitled", txt).ExecuteScriptLine(New Dictionary(Of String, ScriptVarBase), txt)
    End Function

    Public Shared Function RunOneLine(vars As Dictionary(Of String, ScriptVarBase), txt As String) As Integer
        Return New Script("Untitled", txt).ExecuteScriptLine(vars, txt)
    End Function

    Public Shared Function IsScriptVarDefined(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), name As String)
        Return scriptVars.ContainsKey(name.ToUpper())
    End Function

    Public Shared Function GetScriptVar(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), name As String) As ScriptVarBase
        Return scriptVars(name.ToUpper())
    End Function

    Public Shared Function DefineScriptVar(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), svar As ScriptVarBase, Optional lexingOnly As Boolean = False) As ScriptVarBase
        If IsScriptVarDefined(scriptVars, svar.Name) Then
            If Not lexingOnly Then
                Throw New ScriptVarAlreadyDefinedException(svar)
            End If
        Else
            scriptVars.Add(svar.Name.ToUpper(), svar)
        End If

        Return scriptVars(svar.Name.ToUpper())
    End Function

    Public Shared Function CheckSpecialActions(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), fullLine As String, Optional lexingOnly As Boolean = False) As Boolean

        Dim params = New List(Of String)
        Dim splitByEq = fullLine.Split("=")

        Dim first = True

        For Each eq In splitByEq

            Dim splitBySpace = eq.Trim().Split(" ")

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

        Return CheckSpecialActions(scriptVars, params.ToArray, lexingOnly)

    End Function



    Public Shared Function CheckSpecialActions(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), params As String(), Optional lexingOnly As Boolean = False) As Boolean
        If params.Length > 0 AndAlso params(0).ToUpper = "NEW" Then
            If params.Length = 3 Then
                DefineScriptVar(scriptVars, ScriptVarBase.Parse(params(2), params(1)), lexingOnly)
                Return True
            ElseIf params.Length >= 5 AndAlso params(3) = "=" Then

                If params.Length = 5 Then
                    DefineScriptVar(scriptVars, ScriptVarBase.Parse(params(2), params(1), params(4)), lexingOnly)
                    Return True
                Else
                    Dim valueStr = ""
                    For i = 4 To params.Length - 1
                        valueStr = valueStr & params(i) & " "
                    Next

                    Dim intResult = If(lexingOnly, 0, Script.RunOneLine(scriptVars, valueStr))
                    Dim sv = DefineScriptVar(scriptVars, ScriptVarBase.Parse(params(2), params(1)).ReadFromFunctionResult(intResult), lexingOnly)

                    Return True

                End If

            End If
        End If

        Return False
    End Function

    Private Function ExecuteScriptLine(ByRef scriptVars As Dictionary(Of String, ScriptVarBase), ByVal inputStr As String) As Integer

        If CheckSpecialActions(scriptVars, inputStr.Trim) Then
            Return 0
        End If

        Dim str = inputStr
        Dim action As String
        Dim params() As String = {}

        Dim storedVal As String = ""

        str = str.ToUpper()

        If str.Contains("=") Then
            storedVal = str.Replace(" ", "").Split("=")(0)
            str = str.Split("=")(1).TrimStart(" ")
        End If


        action = str.Split(" ")(0).ToUpper
        If Data.clsFuncLocs.Contains(action) Then
            str = "FUNCCALL " & action & ", " & str.ToUpper.Replace(action, "")
            action = "FUNCCALL"
        Else
            str = str.ToUpper.Replace(action, "")
        End If

        If str.Contains(" ") Then
            str = str.Replace(action & " ", "")
            params = str.Replace(" ", "").Split(",")
        End If



        For i = 0 To params.Count - 1
            Dim p = params(i).ToUpper
            If p = "TRUE" Then
                params(i) = "1"
            ElseIf p = "FALSE" Then
                params(i) = "0"
            ElseIf IsScriptVarDefined(scriptVars, p) Then
                params(i) = GetScriptVar(scriptVars, p).ToString()
            End If
        Next


        Dim t As Type = GetType(Funcs)
        Dim pt As Type
        Dim method As MethodInfo

        method = Data.customFuncMethodInfo(action)
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

        If IsScriptVarDefined(scriptVars, storedVal) Then
            scriptVars(storedVal.ToUpper()).ReadFromFunctionResult(result)
        End If

        Return result

    End Function

End Class
