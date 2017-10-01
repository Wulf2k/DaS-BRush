Imports DaS.ScriptLib.Injection.Structures

Namespace Game.Mem

    Public Class GameStats

        Public Shared ClearCount As New LivePtrVar(Of Int32)(Function() Ptr.GameStatsPtr.Value + &H3C)
        Public Shared TrueDeathCount As New LivePtrVar(Of Int32)(Function() Ptr.GameStatsPtr.Value + &H58)
        Public Shared TotalPlayTime As New LivePtrVar(Of Int32)(Function() Ptr.GameStatsPtr.Value + &H68)

    End Class

End Namespace