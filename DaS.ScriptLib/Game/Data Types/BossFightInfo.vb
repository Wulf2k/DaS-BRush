Public Class BossFightInfo
    Public Name As String = "?BossRushBossName?"
    Public BonfireID As Integer = 0
    Public PlayerWarp As EntityLocation = EntityLocation.Zero
    Public AdditionalFlags As Integer() = {}
    Public EventFlag As Integer = -1
    Public EntranceLua As String = ""

    Public Sub New(name As String)
        Me.Name = name
    End Sub

End Class