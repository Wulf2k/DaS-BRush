Imports System.Threading
Imports Neo.IronLua
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.LuaScripting.Structures

Namespace LuaScripting

    Public Class DSLua

        Private Shared L As Lua
        Public Shared G As LuaGlobal
        Private Shared AsmCaller As New DSAsmCaller()

        Public Shared ReadOnly Property DG As Object
            Get
                Return G
            End Get
        End Property

        Public Shared Function Expr(Of T)(exp As String) As T
            Return CTypeDynamic(Of T)(DoString("return (" & exp & ");")(0))
        End Function

        Public Shared Function Expr(exp As String) As Object
            Return DoString("return (" & exp & ");")(0)
        End Function

        Private Shared ReadOnly Property EmptyLuaTable As LuaTable = New LuaTable()

        Public Shared Function CallIngameFunc_FromLua(returnType As Integer, funcAddress As Integer, args As LuaTable) As Object
            Return AsmCaller.CallIngameFunc_FromLua(returnType, funcAddress, args, EmptyLuaTable)
        End Function

        Public Shared Function CallIngameFuncREG_FromLua(returnType As Integer, funcAddress As Integer, args As LuaTable, specialRegisters As LuaTable) As Object
            Return AsmCaller.CallIngameFunc_FromLua(returnType, funcAddress, args, specialRegisters)
        End Function

        Public Shared Function CallIngameFunc(returnType As Integer, funcAddress As Integer, ParamArray args As Object()) As Object
            Return AsmCaller.CallIngameFunc(returnType, funcAddress, args, Nothing)
        End Function

        Public Shared Function CallIngameFuncREG(returnType As Integer, funcAddress As Integer, specialRegisters As Dictionary(Of String, Object), ParamArray args As Object()) As Object
            Return AsmCaller.CallIngameFunc(returnType, funcAddress, args, specialRegisters)
        End Function

        Public Shared Function DoString(luaStr As String, ParamArray args As KeyValuePair(Of String, Object)()) As LuaResult
            Return G.DoChunk(luaStr, "DSLua.DoString()", args)
        End Function

        'Private Const LOOP_INTERVAL = 5000

        Private Shared CleanExitTrigger As New EventWaitHandle(False, EventResetMode.ManualReset)
        'Private Shared NextEvent As DSLuaEvent = DSLuaEvent.None

        Private Shared EventThread As Thread

        'Private Shared ____scriptQueueLock As New Object()

        'Private Shared __scriptQueue As New Queue(Of DSLuaScript)

        Public Shared Event OnScriptFinished(script As Script)

        'Private Shared ReadOnly Property ScriptQueue As Queue(Of DSLuaScript)
        '    Get
        '        Dim result As Queue(Of DSLuaScript) = Nothing
        '        SyncLock ____scriptQueueLock
        '            result = __scriptQueue
        '        End SyncLock
        '        Return result
        '    End Get
        'End Property

        'Private Shared ScriptQueueCallback As New EventWaitHandle(False, EventResetMode.ManualReset)

        'Private Shared __scriptThreadsLock = New Object()
        'Private Shared __scriptThreads As New Dictionary(Of DSLuaScript, Thread)

        'Friend Shared ReadOnly Property ScriptThreads As Dictionary(Of DSLuaScript, Thread)
        '    Get
        '        Dim result As Dictionary(Of DSLuaScript, Thread) = Nothing
        '        SyncLock __scriptThreadsLock
        '            result = __scriptThreads
        '        End SyncLock
        '        Return result
        '    End Get
        'End Property

        'Public Shared Function RegisterScript(script As DSLuaScript) As Thread
        '    ScriptQueue.Enqueue(script)
        '    ScriptQueueCallback.Reset()
        '    ProcEvent(DSLuaEvent.ExecuteScriptSingle)
        '    Dim scriptQueued As Boolean = False
        '    Do
        '        scriptQueued = ScriptQueueCallback.WaitOne(1000)
        '    Loop Until scriptQueued
        '    Return ScriptThreads(script)
        'End Function

        'Friend Shared Function ProcEvent(e As DSLuaEvent) As Boolean
        '    NextEvent = e
        '    Return EventTrigger.Set()
        'End Function

        Private Shared ScriptList As New Dictionary(Of Guid, Script)

        Private Shared ____scriptLock As New Object()

        Private Shared Sub ScriptAdd(script As Script)
            SyncLock ____scriptLock
                ScriptList.Add(script.UUID, script)
            End SyncLock
        End Sub

        Private Shared Sub ScriptRemove(script As Script)
            SyncLock ____scriptLock
                ScriptList.Remove(script.UUID)
            End SyncLock
        End Sub

        Friend Shared Sub ForceInitCleanExitWaitThread()
            If If(EventThread?.IsAlive, False) Then
                EventThread.Abort()
            End If
            EventThread = New Thread(AddressOf DoCleanExitWait) With {.Name = "DSLua.CleanExitWait_" & Guid.NewGuid().ToString(), .IsBackground = True}
            EventThread.Start()
        End Sub

        Friend Shared Sub ForceStopAllScripts()
            For Each script In ScriptList.Values
                script.Abort()
            Next
        End Sub

        Public Shared Sub Init()
            If Not DARKSOULS.Attached Then
                DARKSOULS.TryAttachToDarkSouls(True)
            End If
            Environment.Init()
            ForceInitCleanExitWaitThread()
        End Sub

        Private Shared Sub PerformCleanExit()
            ForceStopAllScripts()
            DARKSOULS.Close()
        End Sub

        Private Shared Sub DoCleanExitWait()
            Dim doCleanExit As Boolean = False

            Try
                Do
                    doCleanExit = CleanExitTrigger.WaitOne(5000)
                Loop Until doCleanExit
            Catch

            Finally
                PerformCleanExit()
            End Try
        End Sub

    End Class

End Namespace
