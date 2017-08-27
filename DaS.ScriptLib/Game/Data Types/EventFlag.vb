Public Class EventFlag
    Public ID As Integer = 0
    Public State As Boolean = False

    Public Sub ApplyFlag()
        Lua.Run($"SetEventFlag({ID}, {If(State, 1, 0)})")
    End Sub

    Public Shared Sub ApplyAll(flagList As Integer())
        For Each flag In flagList
            Dim fuckVb = New EventFlag(flag)
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

    Public Shared Function GetList(ParamArray flags As Integer()) As List(Of EventFlag)
        Return flags.Select(Function(x) New EventFlag(x)).ToList()
    End Function

End Class