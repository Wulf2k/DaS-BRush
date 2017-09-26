Imports System.Threading

Namespace Lua.Structures

    Public Enum LuaThreadStatus As Integer
        Init = 0
        Running = 1
        Aborted = 2
        Finished = 3
    End Enum

    Public Class LuaThread

        Private ReadOnly [Function] As NLua.LuaFunction
        Private ReadOnly Thread As Threading.Thread


        Private ReadOnly ____status_Lock As New Object()
        Private __status As LuaThreadStatus = LuaThreadStatus.Init
        Public Property Status As LuaThreadStatus
            Get
                Dim result As LuaThreadStatus = LuaThreadStatus.Running
                SyncLock ____status_Lock
                    result = __status
                End SyncLock
                Return result
            End Get
            Private Set(value As LuaThreadStatus)
                SyncLock ____status_Lock
                    __status = value
                End SyncLock
            End Set
        End Property


        Private ReadOnly ____result_Lock As New Object()
        Private _result As Object() = Nothing
        Private Property Result As Object()
            Get
                Dim resultResult As Object() = Nothing
                SyncLock ____result_Lock
                    resultResult = _result
                End SyncLock
                Return resultResult
            End Get
            Set(value As Object())
                SyncLock ____result_Lock
                    _result = value
                End SyncLock
            End Set
        End Property


        Public ReadOnly Property IsRunning
            Get
                Return (Status = LuaThreadStatus.Running)
            End Get
        End Property

        Private WHand As EventWaitHandle

        'See the Lua header file for more info.
        Public Function Await() As Object()
            Dim stillWaiting As Boolean = True
            Do
                'Current Thread will sleep for up to 5 seconds at a time, un-blocking the INSTANT WHand.Set() is called in the background thread, 
                'thus greatly reducing CPU usage during the wait without waiting an un-necessarily long time! \( ͡° ͜ʖ ͡°)/
                If WHand.WaitOne(TimeSpan.FromSeconds(5)) Then
                    stillWaiting = False
                End If
            Loop While stillWaiting

            Return _result
        End Function

        Public Sub New(luaFunc As NLua.LuaFunction)
            [Function] = luaFunc
            Status = LuaThreadStatus.Init
            WHand = New EventWaitHandle(False, EventResetMode.ManualReset)
            Result = Nothing
            Thread = New Thread(New ParameterizedThreadStart(
                Sub(params)
                    Result = _callFunctionAndReturnValue(params) 'this SHOULD appropriately set the result via this lambda experssion here
                    If Not WHand.Set() Then
                        Throw New Exception("Wait handle Set() failed somehow (what the actual fuck)")
                    End If
                End Sub))
        End Sub

        'See the Lua header file for more info.
        Public Function Start(params As Object) As Boolean
            If Status = LuaThreadStatus.Running Then
                Return False
            End If

            Result = Nothing

            If Not WHand.Reset() Then
                Throw New Exception("Wait handle Reset() failed somehow (what the actual fuck)")
            End If

            Status = LuaThreadStatus.Running
            Thread.Start(params)
            Return True
        End Function

        'See the Lua header file for more info.
        Public Function Abort() As Boolean
            If Status <> LuaThreadStatus.Running Then
                Return False
            End If

            Thread.Abort()
            WHand.Set()
            Status = LuaThreadStatus.Aborted
            Return True
        End Function

        Private Function _callFunctionAndReturnValue(params As Object) As Object()
            If TypeOf params Is Object() Then
                Return [Function].Call(params)
            ElseIf TypeOf params Is NLua.LuaTable Then
                Return [Function].Call(DirectCast(params, NLua.LuaTable).Values.Cast(Of Object).ToArray())
            Else
                Return [Function].Call(params)
            End If
        End Function

    End Class

End Namespace