Imports System.Text.RegularExpressions
Imports System.Threading

<HideFromScripting>
Public Class Lua
    Public ReadOnly Property State As LuaRunnerState = LuaRunnerState.Stopped

    Private _thread As Thread
    Private _shutdownEvent As New ManualResetEvent(False)
    Private _pauseEvent As New ManualResetEvent(True)

    Public Shared ReadOnly LuaDummyAutoComplete As Dictionary(Of String, String)

    Public Event OnStart(args As LuaRunnerEventArgs)

    Public Event OnFinishAny(args As LuaRunnerEventArgs)

    Public Event OnFinishSuccess(args As LuaRunnerEventArgs)

    Public Event OnFinishError(args As LuaRunnerEventArgs, err As Exception)

    Public Event OnStop()

    Public ReadOnly Property LuaState As NLua.Lua

    Private Shared ReadOnly Property QuickRunner As Lua = New Lua()

    Public Shared ReadOnly QuickExprVarName = "DaS_Scripting__QUICK_EXPRESSION_VAR"

    Shared Sub New()
        LuaDummyAutoComplete = New Dictionary(Of String, String)
        LuaDummyAutoComplete.Add("luaState:Import", "void luaState:Import(string file)")
    End Sub

    Public Sub New()
        LuaState = New NLua.Lua()

        LuaState.DoString("local luaState = 0")
        LuaState.Item("luaState") = Me

        InitCLR()

        LoadDefaultFunctions()
    End Sub

    Private Sub InitCLR()
        LuaState.LoadCLRPackage()
        LuaState.DoString("import ('System', 'System') ")
        LuaState.DoString("import ('Dark Souls Scripting Library', 'DaS.ScriptLib') ")
    End Sub


    Public Shared Sub Run(text As String)
        QuickRunner._doExecuteScript(text)
    End Sub

    Public Shared Sub Run(formatText As String, ParamArray args As Object())
        Run(String.Format(formatText, args))
    End Sub

    Public Shared Function RunBlock(ParamArray lines As String())
        For i = 0 To lines.Length - 1
            QuickRunner._doExecuteScript(lines(i))
        Next

        Return QuickRunner.LuaState
    End Function

    Public Sub Import(file As String)
        Dbg.PrintInfo($"Importing lua script '{file}'...")
        Try
            LuaState.DoString(GetRegexedLuaScript(System.IO.File.ReadAllText(file)))
        Catch ex As Exception
            Dbg.PrintErr($"Import FAILED: {ex.Message}")
            Throw New Exception($"Failed to import '{file}'.{vbCrLf}{vbCrLf}{ex.Message}", ex)
            Return
        End Try
        Dbg.PrintInfo($"Import Successful...")
    End Sub

    Public Shared Function Expr(ByVal expression As String)
        Return QuickRunner.LuaState.DoString(GetRegexedLuaScript("return " & expression))(0)
    End Function

    Public Shared Function Expr(Of T)(ByVal expression As String) As T
        Return QuickRunner.LuaState.DoString(GetRegexedLuaScript("return " & expression))(0)
    End Function

    'Public Shared Function ExprMulti(ByVal expression As String)
    '    Return ExecuteFormattedLua(QuickRunner.LuaState, "return " & expression)(0)
    'End Function

    'Public Shared Function ExprMulti(Of T)(ByVal expression As String) As T
    '    Return ExecuteFormattedLua(QuickRunner.LuaState, "return " & expression)(0)
    'End Function

    'No <HideFromScripting> on this class, as it uses the same function as the other custom functions do and we don't want
    'it to be skipped. However, GetType(Lua.ScriptHelperFunctions) will be excluded from the type list passed to that function.
    Public Class Help

        Public Shared Function LuaHelp_GetIngameFuncInfo(name As String) As IngameFuncInfo
            Return CType(ScriptRes.autoCompleteFuncInfoByName(name), IngameFuncInfo)
        End Function

        <HideFromScripting>
        Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String, params As NLua.LuaTable) As T
            Return AsmExecutor.FuncCall(Of T)(retType, func, params)
        End Function

        Public Shared Function LuaHelp_FuncCall_Int32(retType As IngameFuncReturnType, func As String, params As NLua.LuaTable) As Int32
            Return FuncCall(Of Int32)(retType, func, params)
        End Function

        Public Shared Function LuaHelp_FuncCall_Single(retType As IngameFuncReturnType, func As String, params As NLua.LuaTable) As Single
            Return FuncCall(Of Single)(retType, func, params)
        End Function

        Public Shared Function LuaHelp_FuncCall_Boolean(retType As IngameFuncReturnType, func As String, params As NLua.LuaTable) As Boolean
            Return FuncCall(Of Boolean)(retType, func, params)
        End Function

        Public Shared Function LuaHelp_FuncCall_String(retType As IngameFuncReturnType, func As String, params As NLua.LuaTable) As String
            Return FuncCall(Of String)(retType, func, params)
        End Function

    End Class

    Private Sub LoadDefaultFunctions()
        Dim customFuncs_FuncsClass = ScriptRes.autoCompleteFuncInfoByName_FuncsClass.Values.OfType(Of CustomFuncInfo)

        For Each cfi In customFuncs_FuncsClass
            Try
                LuaState.DoString($"{cfi.Name} = function ({String.Join(", ", cfi.ParamList.Select(Function(p) "__" & cfi.Name.Replace(".", "_") & "_" & p.Name))}) return end")

            Catch ex As Exception
                Throw New Exception($"ahh fuck {If(cfi?.Name, "")} {If(cfi?.MethodDefinition, "")}", ex)
            End Try

        Next

        Dim luaHelpFuncs = ScriptRes.luaScriptHelperFuncInfoByName.Values.OfType(Of CustomFuncInfo)
        For Each cfi In luaHelpFuncs
            LuaState.DoString($"{cfi.Name} = function ({String.Join(", ", cfi.ParamList.Select(Function(p) "__" & cfi.Name.Replace(".", "_") & "_" & p.Name))}) return end")
        Next

        For Each cfi In customFuncs_FuncsClass
            LuaState.RegisterFunction(cfi.Name, cfi.MethodDefinition)
        Next

        For Each cfi In luaHelpFuncs
            LuaState.DoString($"{cfi.Name} = function ({String.Join(", ", cfi.ParamList.Select(Function(p) "__" & cfi.Name.Replace(".", "_") & "_" & p.Name))}) return end")
            LuaState.RegisterFunction(cfi.Name, cfi.MethodDefinition)
        Next

        For Each f In ScriptRes.caselessIngameFuncNames.Values
            Dim info As IngameFuncInfo = CType(ScriptRes.autoCompleteFuncInfoByName(f), IngameFuncInfo)

            Dim luaChunk = ScriptRes.LuaFuncCallFunctionRegistrationTemplate.
                              Replace("--[[NAME]]", f).
                              Replace("--[[RETURN_TYPE]]", ScriptRes.IngameFuncReturnTypeEnumName & "." & info.ReturnType.ToString()).
                              Replace("--[[PARAMS]]", String.Join(", ", info.ParamList.Select(Function(p) "__" & f.Replace(".", "_") & "_" & p.Name))).
                              Replace("--[[FUNCCALL_TYPE]]", ScriptRes.types_ByIngameFuncReturnType(info.ReturnType).Name)

            Try
                LuaState.DoString(luaChunk)
            Catch ex As Exception
                Throw New Exception($"ahh fuck{vbCrLf}{luaChunk}", ex)
            End Try



        Next
    End Sub

    Private Shared Function GetFuncCallTypeFormat(funcName As String) As String
        '  "$1Funcs.FuncCall('"
        Dim info = TryCast(ScriptRes.autoCompleteFuncInfoByName_FuncsClass(funcName), IngameFuncInfo)
        Dim retType As IngameFuncReturnType = If(info IsNot Nothing, info.ReturnType, IngameFuncReturnType.Undefinerino)
        Return "$1Funcs.FuncCall_" & ScriptRes.types_ByIngameFuncReturnType(retType).Name &
               "(" & ScriptRes.IngameFuncReturnTypeEnumName & "." & retType.ToString() & ", '"

    End Function

    Public Shared Function GetRegexedLuaScript(scriptText As String) As String
        Dim txt = " " & String.Join(" ", scriptText)

        ' Changes this:
        '     ChrFadeIn(10000, 0, 0)
        ' To this:
        '     Funcs.FuncCall('ChrFadeIn', 10000, 0, 0)
        ' etc
        'For Each f In ScriptRes.funcNames_Ingame
        '    Dim rx = New Regex("(\W)(" & f & "\()(\w)", RegexOptions.IgnoreCase)
        '    txt = rx.Replace(txt, GetFuncCallTypeFormat(f) & f & "', $3")
        'Next

        'txt = " " & txt

        ' Changes this:
        '     GetEntityPtr(10000)
        ' To this:
        '     Funcs.GetEntityPtr(10000)
        ' etc
        'For Each f In ScriptRes.funcNames_Custom
        '    Dim rx = New Regex("(\W)(" & f & "\()", RegexOptions.IgnoreCase)
        '    txt = rx.Replace(txt, "$1Funcs." & f & "(")
        'Next

        'txt = " " & txt

        ' Changes this:
        '     AddDeathCount()
        ' To this:
        '     Funcs.FuncCall('AddDeathCount')
        ' etc
        'For Each f In ScriptRes.funcNames_Ingame
        '    Dim rx = New Regex("(\W)(" & f & "\()(\))", RegexOptions.IgnoreCase)
        '    txt = rx.Replace(txt, GetFuncCallTypeFormat(f) & f & "'$3")
        'Next

        Return txt.Trim()
    End Function

    Public Sub StartExecution(scriptText As String)
        If State = LuaRunnerState.Stopped Or State = LuaRunnerState.Finished Then
            _thread = New Thread(AddressOf _doExecuteScript) With {.IsBackground = True}
            _thread.Start(scriptText)
        Else
            Throw New LuaRunnerThreadingException("The LuaRunner's State must be that of LuaRunnerState.Stopped before calling StartExecution().")
        End If
    End Sub

    Public Sub StopExecution()
        _State = LuaRunnerState.Stopped
        RaiseEvent OnStop()
        If _thread IsNot Nothing AndAlso _thread.IsAlive Then
            _thread.Abort()
        End If
    End Sub

    Private Sub _doExecuteScript(ByVal script As String)
        _State = LuaRunnerState.Running

        RaiseEvent OnStart(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState))

        Try
            LuaState.DoString(script)
            RaiseEvent OnFinishSuccess(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState))
        Catch ex As System.Threading.ThreadAbortException
            Exit Try
        Catch ex As NLua.Exceptions.LuaException
            RaiseEvent OnFinishError(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState), ex)
        Finally
            _State = LuaRunnerState.Finished
            RaiseEvent OnFinishAny(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState))
        End Try
    End Sub

    Public Function DoStringRegexed(script As String, Optional chunkName As String = Nothing) As Object()
        Dim regexedScript = GetRegexedLuaScript(script)
        If chunkName IsNot Nothing Then
            Return LuaState.DoString(regexedScript, chunkName)
        Else
            Return LuaState.DoString(regexedScript)
        End If
    End Function

End Class

Public Enum LuaRunnerState
    Stopped
    Running
    Finished
End Enum

Public Class LuaRunnerEventArgs
    Public ReadOnly Text As String
    Public ReadOnly ExecutingThread As Thread
    Public ReadOnly LuaState As NLua.Lua

    Public Sub New(ByRef text As String, ByRef executingThread As Thread, ByRef luaState As NLua.Lua)
        Me.Text = text
        Me.ExecutingThread = executingThread
        Me.LuaState = luaState
    End Sub

End Class

Public Class LuaRunnerThreadingException
    Inherits Exception

    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub

End Class