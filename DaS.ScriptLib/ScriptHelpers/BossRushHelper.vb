Imports System.Threading

Public Class BossRushType
    Public Shared ReadOnly Standard As String = "Standard"
    Public Shared ReadOnly Reverse As String = "Reverse"
    Public Shared ReadOnly Random As String = "Random"
    Public Shared ReadOnly Custom As String = "Custom"
End Class

Public Class BossRushHelper
    Public Shared ReadOnly BossRushTimerInterval As Integer = 33

    Private Shared Sub BeginRushTimer()

        Dim msg As String

        Funcs.SetKeyGuideTextPos(600, 605)
        Funcs.SetLineHelpTextPos(1100, 675)

        'Clear TrueDeaths
        Game.GameStats.TrueDeathCount.Value = 0
        Game.GameStats.TotalPlayTime.Value = 0

        Do
            msg = Funcs.GetNgPlusText(Game.GameStats.ClearCount.Value) & " - "
            msg = msg & Strings.Left(TimeSpan.FromMilliseconds(Game.GameStats.TotalPlayTime.Value).ToString, 12) & ChrW(0)
            Funcs.SetKeyGuideText(msg)
            WUnicodeStr(&H11A7770, msg)
            msg = "Deaths: " & Game.GameStats.TrueDeathCount.Value & ChrW(0)
            Funcs.SetLineHelpText(msg)
            Thread.Sleep(33)
        Loop
    End Sub

    Private Shared rushTimer As Thread

    Public Shared Sub StartNewBossRushTimer()
        rushTimer = New Thread(AddressOf BeginRushTimer)
        rushTimer.IsBackground = True
        rushTimer.Start()
    End Sub

    Public Shared Function StopBossRushTimer() As String
        If rushTimer IsNot Nothing Then
            rushTimer.Abort()
        End If
        Funcs.SetKeyGuideTextClear()
        Funcs.SetLineHelpTextClear()
        Return Strings.Left(TimeSpan.FromMilliseconds(Game.GameStats.TotalPlayTime.Value).ToString, 12)
    End Function

    Public Shared Function GetBossRushOrder(rushType As String, excludeBedOfChaos As Boolean, customOrder As String) As String()

        Dim bossPool = Data.BossFights.Keys.ToList()

        Dim bossRushOrder As String() = {}

        If (rushType <> BossRushType.Custom) AndAlso excludeBedOfChaos Then
            bossPool.Remove(Data.Boss.BedOfChaos)
        End If

        If rushType = BossRushType.Standard Then
            Return bossPool.ToArray()
        ElseIf rushType = BossRushType.Reverse Then
            Dim reversedBossPool = New List(Of String)
            For i = bossPool.Count - 1 To 0 Step -1
                reversedBossPool.Add(bossPool(i))
            Next
            Return reversedBossPool.ToArray()
        ElseIf rushType = BossRushType.Random Then
            Dim randomBossPool As New List(Of String)

            While randomBossPool.Count < bossPool.Count
                Dim rand As New Random()
                Dim randIndex = rand.Next(bossPool.Count)
                randomBossPool.Add(bossPool(randIndex))
                bossPool.RemoveAt(randIndex)
            End While

            Return randomBossPool.ToArray()
        ElseIf rushType = BossRushType.Custom Then
            Return customOrder.Split(";").Select(Function(x) x.Trim()).ToArray()
        End If

        Return New String() {}
    End Function

    Public Shared Function CheckWaitPlayerDead(numFrames As Integer, Optional frameLength As Integer = 33) As Boolean
        Dim numFramesAlive = 0
        Dim numFramesDead = 0
        For i = 1 To numFrames
            If Game.Player.HP.Value = 0 Then
                numFramesDead += 1
            Else
                numFramesAlive += 1
            End If

            Thread.Sleep(frameLength)
        Next

        Return numFramesDead > numFramesAlive
    End Function

    ' Will assume boss is dead if valid data isn't saved for it in the BossInfo yet.
    Public Shared Function WaitForBossDeathByName(bossName As String) As Boolean
        Dim boss = Data.BossFights(bossName)
        Dim bossIsDead = False
        Dim playerIsDead = False
        Do
            If (Lua.E($"IsCompleteEvent({boss.EventFlag})") = 1) Then 'boss dead
                bossIsDead = True
            ElseIf Game.Player.HP.Value = 0 Then
                playerIsDead = True
            End If
            Thread.Sleep(33)
        Loop Until bossIsDead Or playerIsDead

        If bossIsDead Then
            Return True
        Else
            Return False 'Returns false to lua script and lua script handles permadeath failure message etc and retries
        End If
    End Function

End Class