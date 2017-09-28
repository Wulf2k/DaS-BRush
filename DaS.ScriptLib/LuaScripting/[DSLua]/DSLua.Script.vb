Imports System.Threading
Imports Neo.IronLua
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.LuaScripting.Structures

Namespace LuaScripting

    Partial Public Class DSLua
        Public Class Script

            Public Event OnRun()
            Friend Sub RaiseOnRun()
                RaiseEvent OnRun()
            End Sub

            Public Event OnFinished()
            Friend Sub RaiseOnFinished()
                RaiseEvent OnFinished()
                RaiseEvent OnScriptFinished(Me)
            End Sub

            Private Event OnException(e As Exception, waitHandle As EventWaitHandle)
            Private Sub RaiseOnException(e As Exception, waitHandle As EventWaitHandle)
                RaiseEvent OnException(e, waitHandle)
            End Sub

            Public ReadOnly Text As String
            Public ReadOnly Name As String
            Public ReadOnly IsDebug As Boolean
            Friend ReadOnly Thread As Threading.Thread
            Friend ReadOnly UUID As Guid

            Public ReadOnly Property FinishTrigger As New EventWaitHandle(False, EventResetMode.ManualReset)

            Private Sub New(scriptText As String, scriptName As String, dbg As Boolean)
                If Not DARKSOULS.Attached Then
                    DARKSOULS.TryAttachToDarkSouls(True)
                End If

                IsDebug = dbg

                Text = scriptText
                Name = scriptName
                Thread = New Threading.Thread(AddressOf DoExecute) With {.Name = "DSLuaScript:" & Name, .IsBackground = False}
                UUID = Guid.NewGuid()
                ScriptAdd(Me)
            End Sub

            Public Sub Run(acknowledgeException As Action(Of Exception, EventWaitHandle))
                FinishTrigger.Reset()
                Dim onExceptionAction As OnExceptionEventHandler =
                    Sub(e, h)
                        RemoveHandler OnException, onExceptionAction
                        acknowledgeException(e, h)
                    End Sub

                AddHandler OnException, onExceptionAction

                Thread.Start(Me)
            End Sub

            Public Sub RunSync(acknowledgeException As Action(Of Exception, EventWaitHandle))
                Run(acknowledgeException)
                AwaitTermination()
            End Sub

            Public Sub Abort()
                Thread.Abort()
            End Sub

            Friend Shared Sub DoExecute(scriptObj As Object)

                Dim s = DirectCast(scriptObj, Script)

                s.RaiseOnRun()

                Dim exceptionAknowledgedTrigger As New EventWaitHandle(False, EventResetMode.ManualReset)
                Dim exceptionAknowledged As Boolean = False

                Try
                    Dim chunk = L.CompileChunk(s.Text, s.Name, New LuaCompileOptions() With {.DebugEngine = If(s.IsDebug, LuaStackTraceDebugger.Default, Nothing)})
                    G.DoChunk(chunk)

                    exceptionAknowledgedTrigger.Set()
                Catch ex As ThreadAbortException
                    Console.WriteLine($"DSLuaScript ""{s.Name}"" aborted. [UUID: {s.UUID.ToString()}]")

                    exceptionAknowledgedTrigger.Set()
                Catch e As Exception
                    s.RaiseOnException(e, exceptionAknowledgedTrigger)
                Finally
                    Do
                        exceptionAknowledged = exceptionAknowledgedTrigger.WaitOne(5000)
                    Loop Until exceptionAknowledged

                    s.RaiseOnFinished()
                    s.FinishTrigger.Set()
                    ScriptRemove(s)
                End Try

            End Sub

            Public Sub AwaitTermination()
                Dim ended As Boolean = False
                Do
                    ended = FinishTrigger.WaitOne(5000)
                Loop Until ended
            End Sub

            Public Shared Function FromFile(luaFilePath As String, Optional dbg As Boolean = False) As Script
                Return New Script(IO.File.ReadAllText(luaFilePath), New IO.FileInfo(luaFilePath).Name, dbg)
            End Function

            Public Shared Function FromString(luaStr As String, displayName As String, Optional dbg As Boolean = False) As Script
                Return New Script(luaStr, displayName, dbg)
            End Function

        End Class
    End Class

End Namespace
