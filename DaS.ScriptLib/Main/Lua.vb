Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Lua
    Public ReadOnly Property State As LuaRunnerState = LuaRunnerState.Stopped

    Private _thread As Thread
    Private _shutdownEvent As New ManualResetEvent(False)
    Private _pauseEvent As New ManualResetEvent(True)

    Public Event OnStart(args As LuaRunnerEventArgs)

    Public Event OnFinishAny(args As LuaRunnerEventArgs)

    Public Event OnFinishSuccess(args As LuaRunnerEventArgs)

    Public Event OnFinishError(args As LuaRunnerEventArgs, err As Exception)

    Public Event OnStop()

    Public ReadOnly Property LuaState As NLua.Lua

    Private Shared ReadOnly Property QuickRunner As Lua = New Lua()

    Public Shared ReadOnly QuickExprVarName = "DaS_Scripting__QUICK_EXPRESSION_VAR"

    Public Sub New()
        LuaState = New NLua.Lua()
        InitCLR()
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

    Public Shared Function GetRegexedLuaScript(scriptText As String) As String
        Dim txt = " " & String.Join(" ", scriptText)

        ' Changes this:
        '     ChrFadeIn(10000, 0, 0)
        ' To this:
        '     Funcs.FuncCall('ChrFadeIn', 10000, 0, 0)
        ' etc
        For Each f In ScriptRes.funcNames_Ingame
            Dim rx = New Regex("(\W)(" & f & "\()(\w)", RegexOptions.IgnoreCase)
            txt = rx.Replace(txt, "$1Funcs.FuncCall('" & f & "', $3")
        Next

        txt = " " & txt

        ' Changes this:
        '     GetEntityPtr(10000)
        ' To this:
        '     Funcs.GetEntityPtr(10000)
        ' etc
        For Each f In ScriptRes.funcNames_Custom
            Dim rx = New Regex("(\W)(" & f & "\()", RegexOptions.IgnoreCase)
            txt = rx.Replace(txt, "$1Funcs." & f & "(")
        Next

        txt = " " & txt

        ' Changes this:
        '     AddDeathCount()
        ' To this:
        '     Funcs.FuncCall('AddDeathCount')
        ' etc
        For Each f In ScriptRes.funcNames_Ingame
            Dim rx = New Regex("(\W)(" & f & "\()(\))", RegexOptions.IgnoreCase)
            txt = rx.Replace(txt, "$1Funcs.FuncCall('" & f & "'$3")
        Next

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
        Dim mangledScript = GetRegexedLuaScript(script)

        _State = LuaRunnerState.Running

        RaiseEvent OnStart(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState, mangledScript))

        Try
            LuaState.DoString(mangledScript)
            RaiseEvent OnFinishSuccess(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState, mangledScript))
        Catch ex As System.Threading.ThreadAbortException
            Exit Try
        Catch ex As NLua.Exceptions.LuaException
            RaiseEvent OnFinishError(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState, mangledScript), ex)
        Finally
            _State = LuaRunnerState.Finished
            RaiseEvent OnFinishAny(New LuaRunnerEventArgs(script, Thread.CurrentThread, LuaState, mangledScript))
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
    Public ReadOnly MangledText As String
    Public ReadOnly LuaState As NLua.Lua

    Public Sub New(ByRef text As String, ByRef executingThread As Thread, ByRef luaState As NLua.Lua, ByRef mangledText As String)
        Me.Text = text
        Me.ExecutingThread = executingThread
        Me.LuaState = luaState
        Me.MangledText = mangledText
    End Sub

End Class

Public Class LuaRunnerThreadingException
    Inherits Exception

    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub

End Class