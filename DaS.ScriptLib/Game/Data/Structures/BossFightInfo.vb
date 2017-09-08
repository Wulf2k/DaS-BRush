Namespace Game.Data.Structures
    Public Class BossFightInfo
        Public ReadOnly Property SpawnFlag As EventFlag.Boss = EventFlag.Boss.AsylumDemon
        Public ReadOnly Property Name As String = "?BossName?"

        Public PlayerWarp As Loc = Loc.Zero
        Public World As Integer = 0
        Public Area As Integer = 0
        Public WarpID As Integer = -1
        Public PlayerAnim As Integer = 7410
        Public AdditionalFlags As Integer() = {}
        Public EntranceLua As String = ""

        Public Sub New(spawnFlag As EventFlag.Boss, name As String)
            Me.SpawnFlag = spawnFlag
            Me.Name = name
        End Sub

    End Class
End Namespace