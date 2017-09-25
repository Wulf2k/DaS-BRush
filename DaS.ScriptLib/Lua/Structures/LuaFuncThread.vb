Namespace Lua.Structures

    Public Class LuaFuncThread

        Private ReadOnly [Function] As NLua.LuaFunction
        Private ReadOnly Thread As Threading.Thread

        Public ReadOnly Property HasReturned As Boolean = False
        Public ReadOnly Property Running As Boolean = False

        Public Function Await() As Object()
            While Not HasReturned
                Threading.Thread.SpinWait(1)
            End While
            Return Result
        End Function

        'No idea wut I'm doin tbh
        Private Result As Object()

        Public Sub New(luaFunc As NLua.LuaFunction)
            [Function] = luaFunc
            Thread = New Threading.Thread(AddressOf DoFunction)
        End Sub

        Public Sub StartThread(params As Object)
            _Running = True
            _HasReturned = False
            Thread.Start(params)
        End Sub

        Public Sub AbortThread()
            Thread.Abort()
            _Running = False
        End Sub

        Private Sub DoFunction(params As Object)
            If TypeOf params Is Object() Then
                Result = [Function].Call(params)
            ElseIf TypeOf params Is NLua.LuaTable Then
                Result = [Function].Call(DirectCast(params, NLua.LuaTable).Values.Cast(Of Object).ToArray())
            Else
                Result = [Function].Call(params)
            End If
            _HasReturned = True
            _Running = False
        End Sub

    End Class

End Namespace