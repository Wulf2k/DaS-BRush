﻿Imports System.Threading

Public Class BossRushType
    Public Shared ReadOnly Standard As String = "Standard"
    Public Shared ReadOnly Reverse As String = "Reverse"
    Public Shared ReadOnly Random As String = "Random"
    Public Shared ReadOnly Custom As String = "Custom"
End Class

Public Class BossRushHelper
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

    ' Will assume boss is dead if valid data isn't saved for it in the BossInfo yet.
    Public Shared Function WaitForBossDeathByName(bossName As String) As Boolean
        Dim boss = Data.BossFights(bossName)
        Dim bossIsDead = False
        Do
            If (Lua.Expr(Of Integer)($"IsCompleteEvent({boss.EventFlag})") = 1) Then 'boss dead
                bossIsDead = True
            ElseIf (Lua.Expr(Of Boolean)("if GetHp(10000) == 0 then return false
Wait(100)
return GetHp(10000) == 0")) Then 'Code is weird because i kept having random flickering of the character health where it would show up as 0 for 1 frame idfk
                bossIsDead = False
                SpawnPlayerAtBoss(bossName)
            Else
                Thread.Sleep(100)
            End If
        Loop Until bossIsDead
        Return True
    End Function

    Public Shared Sub SpawnPlayerAtBoss(bossName As String)
        Dim boss As BossFightInfo = Data.BossFights(bossName)
        EventFlag.ApplyAll(boss.AdditionalFlags)
        Funcs.ShowHUD(False)

        If (boss.EventFlag >= 0) Then
            Lua.Run($"SetEventFlag({boss.EventFlag}, false)")
        End If

        ' If no bonfire ID is set for boss, just warp to player's last rested at bonfire. Falling in a gray void in the middle of nowhere
        Lua.Run($"WarpNextStage_Bonfire({If(boss.BonfireID > 0, boss.BonfireID, Game.Player.BonfireID.Value)})")
        Funcs.Wait(1000)
        Funcs.WaitForLoadEnd()
        Funcs.BlackScreen()

        Lua.RunBlock(
            "SetColiEnable(10000, false)
            DisableMove(10000, true)
            SetDisableGravity(10000, true)
            DisableMapHit(10000, true)
            PlayerHide(true)

            ForcePlayLoopAnimation(10000, 0)
            StopLoopAnimation(10000, 0)")

        Dim playerPtr = Funcs.GetEntityPtr(10000)

        If boss.PlayerWarp <> EntityLocation.Zero Then
            Funcs.SetEntityLocation(playerPtr, boss.PlayerWarp)
        End If

        Lua.RunBlock(
            "DisableMapHit(10000, false)
            CamReset(10000, 1)")

        If Not String.IsNullOrWhiteSpace(boss.EntranceLua) Then
            Lua.Run(boss.EntranceLua)
        End If

        'Still not 100% sure how the multiline strings work in VB lol
        Lua.RunBlock(
            "--Spawn with default co-op summon anim 
            --ForcePlayAnimation(10000, GetSummonAnimId(10000))
            --Wait(100)

            ShowHUD(true)
            --Wait(4000) 

            --Play walking through fogwall anim:
            ForcePlayAnimation(10000, 7410)
            FadeIn()
            --Wait(1100)
            SetColiEnable(10000, true)
            DisableMove(10000, false)
            SetDisableGravity(10000, false)
            PlayerHide(false)
            Wait(2100)
            ")

    End Sub

End Class
