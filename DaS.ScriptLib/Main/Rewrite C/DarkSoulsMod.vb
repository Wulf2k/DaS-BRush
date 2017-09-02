''' <summary>
''' --Example of wtf to do in mod lua:
''' Mod.OnInit = function()
'''     --Stuff
''' end
''' Mod.OnUpdate = function()
'''     --Do stuff at very high frequency
''' end
''' </summary>
Public Class DarkSoulsMod
    Public ReadOnly ScriptText As String

    Public OnInit As NLua.LuaFunction
    Public OnUpdate As NLua.LuaFunction

    Public ReadOnly UpdateThread As Threading.Thread

    Private isExit = False

    Public ReadOnly curLua As Lua

    Public ReadOnly Property IsRunning As Boolean
        Get
            Return Not isExit
        End Get
    End Property

    Public Sub [Stop]()
        isExit = True
    End Sub


    Public Sub New()
        isExit = True
    End Sub

    Public Sub New(script As String, Optional modName As String = "Mod")

        ScriptText = script

        curLua = New Lua()

        curLua.State.DoString($"function Mod_{modName} (Mod) {vbCrLf}{ScriptText}{vbCrLf}end")
        curLua.State.GetFunction($"Mod_{modName}").Call(Me)

        If OnInit IsNot Nothing Then
            OnInit.Call()
        End If

        Dim UpdateThread = New Threading.Thread(AddressOf UpdateThread_Work) With {.IsBackground = True, .Name = "Lua_Mod_" & modName}
    End Sub

    Private Sub UpdateThread_Work()
        If OnUpdate IsNot Nothing Then
            While Not isExit
                OnUpdate.Call()
            End While

        End If
    End Sub
End Class
