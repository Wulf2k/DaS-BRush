Imports DaS.ScriptLib.LuaScripting

Namespace Game.Data.Helpers
    Public Class EventFlagHelper
        Public ID As Integer = 0
        Public State As Boolean = False

        Public Sub ApplyFlag()
            DSLua.Expr($"SetEventFlag({ID}, {State.ToString().ToLower()})")
        End Sub

        Public Shared Sub ApplyAll(flagList As Integer())
            For Each flag In flagList
                Dim fuckVb = New EventFlagHelper(flag)
                fuckVb.ApplyFlag()
            Next
        End Sub

        Public Sub New(id As Integer, state As Boolean)
            Me.ID = id
            Me.State = state
        End Sub

        Public Sub New(id As Integer)
            State = (id > 0)
            Me.ID = Math.Abs(id)
        End Sub

        Public Shared Function GetList(ParamArray flags As Integer()) As List(Of EventFlagHelper)
            Return flags.Select(Function(x) New EventFlagHelper(x)).ToList()
        End Function

    End Class
End Namespace