Imports System.Threading
Imports DaS.ScriptLib.Lua
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.Game.Data.EventFlag

Namespace Game.Data.Helpers
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
            Mem.GameStats.TrueDeathCount.Value = 0
            Mem.GameStats.TotalPlayTime.Value = 0

            Do
                msg = Funcs.GetNgPlusText(Mem.GameStats.ClearCount.Value) & " - "
                msg = msg & Strings.Left(TimeSpan.FromMilliseconds(Mem.GameStats.TotalPlayTime.Value).ToString, 12) & ChrW(0)
                Funcs.SetKeyGuideText(msg)
                WUnicodeStr(&H11A7770, msg)
                msg = "Deaths: " & Mem.GameStats.TrueDeathCount.Value & ChrW(0)
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
            Return Strings.Left(TimeSpan.FromMilliseconds(Mem.GameStats.TotalPlayTime.Value).ToString, 12)
        End Function

        Public Shared Function GetBossRushOrder(rushType As String, excludeBedOfChaos As Boolean, customOrder As String) As Boss()

            Dim bossPool = Misc.BossFights.Keys.ToList()

            Dim bossRushOrder As String() = {}

            If (rushType <> BossRushType.Custom) AndAlso excludeBedOfChaos Then
                bossPool.Remove(Boss.BedOfChaos)
            End If

            If rushType = BossRushType.Standard Then
                Return bossPool.ToArray()
            ElseIf rushType = BossRushType.Reverse Then
                Dim reversedBossPool = New List(Of Boss)
                For i = bossPool.Count - 1 To 0 Step -1
                    reversedBossPool.Add(bossPool(i))
                Next
                Return reversedBossPool.ToArray()
            ElseIf rushType = BossRushType.Random Then
                Dim randomBossPool As New List(Of Boss)

                While randomBossPool.Count < bossPool.Count
                    Dim rand As New Random()
                    Dim randIndex = rand.Next(bossPool.Count)
                    randomBossPool.Add(bossPool(randIndex))
                    bossPool.RemoveAt(randIndex)
                End While

                Return randomBossPool.ToArray()
            ElseIf rushType = BossRushType.Custom Then
                Return customOrder.Split(";").Select(Function(x) CType(Int32.Parse(x.Trim()), Boss)).ToArray()
            End If

            Return New Boss() {}
        End Function

        Public Shared Function CheckWaitPlayerDead(numFrames As Integer, Optional frameLength As Integer = 33) As Boolean
            Dim numFramesAlive = 0
            Dim numFramesDead = 0
            For i = 1 To numFrames
                If Mem.Player.HP.Value = 0 Then
                    numFramesDead += 1
                Else
                    numFramesAlive += 1
                End If

                Thread.Sleep(frameLength)
            Next

            Return numFramesDead > numFramesAlive
        End Function

        ' Will assume boss is dead if valid data isn't saved for it in the BossInfo yet.
        Public Shared Function WaitForBossDeathByEventFlag(bossFlag As Boss) As Boolean
            Dim boss = Misc.BossFights(bossFlag)
            Dim bossIsDead = False
            Dim playerIsDead = False
            Do
                If (LuaInterface.E($"IsCompleteEvent({CType(boss.SpawnFlag, Integer)})") = 1) Then 'boss dead
                    bossIsDead = True
                ElseIf Mem.Player.HP.Value = 0 Then
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
End Namespace