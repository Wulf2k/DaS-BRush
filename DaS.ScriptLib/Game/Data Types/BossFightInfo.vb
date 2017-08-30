Public Class BossFightInfo
    Public Name As String = "?BossRushBossName?"
    Public PlayerWarp As EntityLocation = EntityLocation.Zero
    Public World As Integer = 0
    Public Area As Integer = 0
    Public WarpID As Integer = -1
    Public PlayerAnim As Integer = 7410
    Public AdditionalFlags As Integer() = {}
    Public EventFlag As Integer = -1
    Public EntranceLua As String = ""

    Public Sub New(name As String)
        Me.Name = name
    End Sub

End Class