﻿Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports DaS.ScriptLib
Imports DaS.ScriptLib.Lua
Imports DaS.ScriptLib.Game.Data.Helpers
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.Game
Imports DaS.ScriptLib.Game.Data

Public Class frmForm1



    'TODO:
    'Check equipment durability
    'Handle equipment management

    'Set tails to be pre-cut to avoid drops
    'Centipede, intro cinematic

    'Asylum Demon, Close that damn door

    'Bed of Chaos, reset platform-collapsing

    'Grant ring for 4K fight

    'Force animation through fog gate?

    '50001550 = Stop Rite of Kindling dropping?
    'Check ItemLotParam for boss soul EventFlags


    Dim _originalExStyle = -1
    Dim _doubleBufferino As Boolean = True
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            If _originalExStyle = -1 Then
                _originalExStyle = MyBase.CreateParams.ExStyle
            End If

            Dim handleParam = MyBase.CreateParams

            If _doubleBufferino Then
                handleParam.ExStyle = handleParam.ExStyle Or &H2000000
            Else
                handleParam.ExStyle = _originalExStyle
            End If

            Return handleParam
        End Get
    End Property



    Private Sub _turnOffDoubleBufferino()
        _doubleBufferino = False
        Me.MaximizeBox = True
    End Sub

    Public Const VersionCheckUrl = "http://wulf2k.ca/pc/das/das-brush-ver.txt"
    Public Const NoteCheckUrl = "http://wulf2k.ca/pc/das/das-brush-notes.txt"

    'Dim consoleWindow As DaS.ScriptEditor.ConsoleWindow

    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Dim delay As Integer = 33

    Dim playerHP As Integer
    Dim playerStam As Integer

    Dim playerMaxHP As Integer
    Dim playerMaxStam As Integer

    Dim playerFacing As Single
    Dim playerXpos As Single
    Dim playerYpos As Single
    Dim playerZpos As Single

    Public lua As New LuaInterface()

    Public intvar1 As Integer
    Public intvar2 As Integer

    Dim luaThread As Thread

    Private flpCustomBossOrderSelectedControl As Control

    Private Async Sub updatecheck()
        Try
            Dim client As New Net.WebClient()
            Dim content As String = Await client.DownloadStringTaskAsync(VersionCheckUrl)

            Dim lines() As String = content.Split({vbCrLf, vbLf}, StringSplitOptions.None)
            Dim stableVersion = lines(0)
            Dim stableUrl = lines(1)


            If stableVersion > lblVer.Text.Replace("-", "") Then
                btnUpdate.Visible = True
                btnUpdate.Tag = stableUrl
            Else
                btnUpdate.Visible = False
            End If

            content = Await client.DownloadStringTaskAsync(NoteCheckUrl)
            txtNotes.Text = content

        Catch ex As Exception
            'Fail silently since nobody wants to be bothered for an update check.
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim oldFileArg As String = Nothing
        For Each arg In Environment.GetCommandLineArgs().Skip(1)
            If arg.StartsWith("--old-file=") Then
                oldFileArg = arg.Substring("--old-file=".Length)
            Else
                MsgBox("Unknown command line arguments")
                oldFileArg = Nothing
                Exit For
            End If
        Next
        If oldFileArg IsNot Nothing Then
            If oldFileArg.EndsWith(".old") Then
                Dim t = New Thread(
                Sub()
                    Try
                        'Give the old version time to shut down
                        Thread.Sleep(1000)
                        File.Delete(oldFileArg)
                    Catch ex As Exception
                        Me.Invoke(Function() MsgBox("Deleting old version failed: " & vbCrLf & ex.Message, MsgBoxStyle.Exclamation))
                    End Try
                End Sub)
                t.Start()
            Else
                MsgBox("Deleting old version failed: Invalid filename ", MsgBoxStyle.Exclamation)
            End If
        End If

        updatecheck()

        '-----------------------Bonfires-----------------------
        For Each bonfire In ScriptLibResources.clsBonfires.Keys
            Dim id = CType(bonfire, Integer)
            cmbBonfire.Items.Add(CType(ScriptLibResources.clsBonfires(bonfire), String) & " [" & id.ToString() & "]")
        Next
        cmbBonfire.SelectedItem = "Nothing [0]"

        DARKSOULS.TryAttachToDarkSouls(True)
        lblRelease.Text = DARKSOULS.VersionDisplayName

        refTimer = New System.Windows.Forms.Timer
        refTimer.Interval = delay
        refTimer.Enabled = DARKSOULS.Attached

        Dim bossNameList = Game.Data.Misc.BossFights.Values.Select(Function(x) x.Name)

        For Each bossName In bossNameList
            comboBossList.Items.Add(bossName)
        Next

        comboBossList.SelectedIndex = 0

        Dim bossRushScript As String = ""

        Using strm = Assembly.GetExecutingAssembly().GetManifestResourceStream("DaS.BossRush.BossRush.lua")
            Using sr As New StreamReader(strm)
                bossRushScript = sr.ReadToEnd()
            End Using
        End Using

        lua.State.DoString(bossRushScript)
    End Sub

    Private Sub RefreshGUI()
        Dim isBossRushing = lua.State.IsExecuting

        btnLoadBossScenario.Enabled = Not isBossRushing

        btnBeginBossRush.Enabled = Not isBossRushing
        btnCancelBossRush.Enabled = isBossRushing

        Select Case tabs.TabPages(tabs.SelectedIndex).Text
            Case "Player"

                lblHP.Text = Mem.Player.HP.Value & " / " & Mem.Player.MaxHP.Value
                lblStam.Text = Mem.Player.Stamina.Value & " / " & Mem.Player.MaxStamina.Value

                lblFacing.Text = "Heading: " & ((Mem.Player.Heading.Value + Math.PI) / (Math.PI * 2) * 360).ToString("0.00") & "°"
                lblXpos.Text = Mem.Player.PosX.Value.ToString("0.00")
                lblYpos.Text = Mem.Player.PosY.Value.ToString("0.00")
                lblZpos.Text = Mem.Player.PosZ.Value.ToString("0.00")

                lblstableXpos.Text = Mem.Player.StablePosX.Value.ToString("0.00")
                lblstableYpos.Text = Mem.Player.StablePosY.Value.ToString("0.00")
                lblstableZpos.Text = Mem.Player.StablePosZ.Value.ToString("0.00")

                Dim bonfireID As Integer
                bonfireID = Mem.Player.BonfireID.Value
                If Not cmbBonfire.DroppedDown Then
                    If ScriptLibResources.clsBonfires(bonfireID) = "" Then
                        ScriptLibResources.clsBonfires.Add(bonfireID, bonfireID.ToString)
                        ScriptLibResources.clsBonfiresIDs.Add(bonfireID.ToString, bonfireID)
                        cmbBonfire.Items.Add("??? [" & bonfireID.ToString & "]")
                    End If
                    cmbBonfire.SelectedItem = ScriptLibResources.clsBonfires(bonfireID) & " [" & bonfireID.ToString() & "]"
                End If


            Case "Stats"
                Try
                    nmbMaxHP.FuckOff(Mem.Player.Stats.MaxHP.Value)
                    nmbMaxStam.FuckOff(Mem.Player.Stats.MaxStamina.Value)
                    nmbVitality.FuckOff(Mem.Player.Stats.VIT.Value)
                    nmbAttunement.FuckOff(Mem.Player.Stats.ATN.Value)
                    nmbEnd.FuckOff(Mem.Player.Stats.ENDurance.Value)
                    nmbStr.FuckOff(Mem.Player.Stats.STR.Value)
                    nmbDex.FuckOff(Mem.Player.Stats.DEX.Value)
                    nmbIntelligence.FuckOff(Mem.Player.Stats.INT.Value)
                    nmbFaith.FuckOff(Mem.Player.Stats.FTH.Value)
                    nmbResistance.FuckOff(Mem.Player.Stats.RES.Value)
                    nmbHumanity.FuckOff(Mem.Player.Stats.Humanity.Value)
                    nmbGender.FuckOff(Mem.Player.Stats.ExternalGenitals.Value)
                    nmbClearCount.FuckOff(Mem.GameStats.ClearCount.Value)
                Catch ex As Exception
                    Console.WriteLine("Error displaying stats.")
                End Try

        End Select

    End Sub

    Private Sub refTimer_Tick() Handles refTimer.Tick

        RefreshGUI()

    End Sub








    'Private Sub btnBossAsylumDemon_Click(sender As Object, e As EventArgs) Handles btnBossAsylumDemon.Click, Button1.Click
    '    trd = New Thread(AddressOf Funcs.BossAsylumDemon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossBedOfChaos_Click(sender As Object, e As EventArgs) Handles btnBossBedOfChaos.Click, Button6.Click
    '    trd = New Thread(AddressOf Funcs.BossBedOfChaos)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossBellGargoyles_Click(sender As Object, e As EventArgs) Handles btnBossBellGargoyles.Click, Button4.Click
    '    trd = New Thread(AddressOf Funcs.BossBellGargoyles)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossBlackDragonKalameet_Click(sender As Object, e As EventArgs) Handles btnBossBlackDragonKalameet.Click, Button26.Click
    '    trd = New Thread(AddressOf Funcs.BossBlackDragonKalameet)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossCapraDemon_Click(sender As Object, e As EventArgs) Handles btnBossCapraDemon.Click, Button3.Click
    '    trd = New Thread(AddressOf Funcs.BossCapraDemon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossCeaselessDischarge_Click(sender As Object, e As EventArgs) Handles btnBossCeaselessDischarge.Click, Button5.Click
    '    trd = New Thread(AddressOf Funcs.BossCeaselessDischarge)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossCentipedeDemon_Click(sender As Object, e As EventArgs) Handles btnBossCentipedeDemon.Click, Button7.Click
    '    trd = New Thread(AddressOf Funcs.BossCentipedeDemon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossChaosWitchQuelaag_Click(sender As Object, e As EventArgs) Handles btnBossChaosWitchQuelaag.Click, Button8.Click
    '    trd = New Thread(AddressOf Funcs.BossChaosWitchQuelaag)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossCrossbreedPriscilla_Click(sender As Object, e As EventArgs) Handles btnBossCrossbreedPriscilla.Click, Button9.Click
    '    trd = New Thread(AddressOf Funcs.BossCrossbreedPriscilla)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossDarkSunGwyndolin_Click(sender As Object, e As EventArgs) Handles btnBossDarkSunGwyndolin.Click, Button10.Click
    '    trd = New Thread(AddressOf Funcs.BossDarkSunGwyndolin)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossDemonFiresage_Click(sender As Object, e As EventArgs) Handles btnBossDemonFiresage.Click, Button11.Click
    '    trd = New Thread(AddressOf Funcs.BossDemonFiresage)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossFourKings_Click(sender As Object, e As EventArgs) Handles btnBossFourKings.Click, Button13.Click
    '    trd = New Thread(AddressOf Funcs.BossFourKings)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossGapingDragon_Click(sender As Object, e As EventArgs) Handles btnBossGapingDragon.Click, Button14.Click
    '    trd = New Thread(AddressOf Funcs.BossGapingDragon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossGravelordNito_Click(sender As Object, e As EventArgs) Handles btnBossGravelordNito.Click, Button15.Click
    '    trd = New Thread(AddressOf Funcs.BossGravelordNito)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossGwyn_Click(sender As Object, e As EventArgs) Handles btnBossGwyn.Click, Button16.Click
    '    trd = New Thread(AddressOf Funcs.BossGwynLordOfCinder)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossIronGolem_Click(sender As Object, e As EventArgs) Handles btnBossIronGolem.Click, Button17.Click
    '    trd = New Thread(AddressOf Funcs.BossIronGolem)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossKnightArtorias_Click(sender As Object, e As EventArgs) Handles btnBossKnightArtorias.Click, Button18.Click
    '    trd = New Thread(AddressOf Funcs.BossKnightArtorias)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossManus_Click(sender As Object, e As EventArgs) Handles btnBossManus.Click, Button19.Click
    '    trd = New Thread(AddressOf Funcs.BossManusFatherOfTheAbyss)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossMoonlightButterfly_Click(sender As Object, e As EventArgs) Handles btnBossMoonlightButterfly.Click, Button20.Click
    '    trd = New Thread(AddressOf Funcs.BossMoonlightButterfly)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossOandS_Click(sender As Object, e As EventArgs) Handles btnBossOandS.Click, Button12.Click
    '    trd = New Thread(AddressOf Funcs.BossOrnsteinAndSmough)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossPinwheel_Click(sender As Object, e As EventArgs) Handles btnBossPinwheel.Click, Button2.Click
    '    trd = New Thread(AddressOf Funcs.BossPinwheel)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossSanctuaryGuardian_Click(sender As Object, e As EventArgs) Handles btnBossSanctuaryGuardian.Click, Button21.Click
    '    trd = New Thread(AddressOf Funcs.BossSanctuaryGuardian)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossSeath_Click(sender As Object, e As EventArgs) Handles btnBossSeath.Click, Button22.Click
    '    trd = New Thread(AddressOf Funcs.BossSeathTheScaleless)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossSif_Click(sender As Object, e As EventArgs) Handles btnBossSif.Click, Button24.Click
    '    trd = New Thread(AddressOf Funcs.BossGreatGreyWolfSif)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossStrayDemon_Click(sender As Object, e As EventArgs) Handles btnBossStrayDemon.Click, Button23.Click
    '    trd = New Thread(AddressOf Funcs.BossStrayDemon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub
    'Private Sub btnBossTaurusDemon_Click(sender As Object, e As EventArgs) Handles btnBossTaurusDemon.Click, Button25.Click
    '    trd = New Thread(AddressOf Funcs.BossTaurusDemon)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    'Private Sub btnScenarioArtoriasCiaran_Click(sender As Object, e As EventArgs) Handles btnScenarioArtoriasCiaran.Click
    '    trd = New Thread(AddressOf Funcs.ScenarioArtoriasAndCiaran)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    'Private Sub btnScenarioTripleSanctuary_Click(sender As Object, e As EventArgs) Handles btnScenarioTripleSanctuary.Click
    '    trd = New Thread(AddressOf Funcs.ScenarioTripleSanctuaryGuardian)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    'Private Sub btnBeginBossRush_Click(sender As Object, e As EventArgs) Handles btnBeginBossRush.Click

    '    If (cboxReverseOrder.Checked) Then
    '        trd = New Thread(AddressOf Funcs.BeginReverseBossRush)
    '    Else
    '        trd = New Thread(AddressOf Funcs.BeginBossRush)
    '    End If
    '    trd.IsBackground = True
    '    trd.Start()

    'End Sub

    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        DARKSOULS.TryAttachToDarkSouls()
        lblRelease.Text = DARKSOULS.VersionDisplayName
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim updateWindow As New UpdateWindow(sender.Tag)
        updateWindow.ShowDialog()
        If updateWindow.WasSuccessful Then
            Process.Start(updateWindow.NewAssembly, """--old-file=" & updateWindow.OldAssembly & """")
            Me.Close()
        End If
    End Sub

    Private Sub nmbVitality_ValueChanged(sender As Object, e As EventArgs) Handles nmbVitality.ValueChanged
        Mem.Player.Stats.VIT.Value = sender.Value
    End Sub

    Private Sub nmbAttunement_ValueChanged(sender As Object, e As EventArgs) Handles nmbAttunement.ValueChanged
        Mem.Player.Stats.ATN.Value = sender.Value
    End Sub

    Private Sub nmbEnd_ValueChanged(sender As Object, e As EventArgs) Handles nmbEnd.ValueChanged
        Mem.Player.Stats.ENDurance.Value = sender.Value
    End Sub

    Private Sub nmbStr_ValueChanged(sender As Object, e As EventArgs) Handles nmbStr.ValueChanged
        Mem.Player.Stats.STR.Value = sender.Value
    End Sub

    Private Sub nmbDex_ValueChanged(sender As Object, e As EventArgs) Handles nmbDex.ValueChanged
        Mem.Player.Stats.DEX.Value = sender.Value
    End Sub

    Private Sub nmbResistance_ValueChanged(sender As Object, e As EventArgs) Handles nmbResistance.ValueChanged
        Mem.Player.Stats.RES.Value = sender.Value
    End Sub

    Private Sub nmbIntelligence_ValueChanged(sender As Object, e As EventArgs) Handles nmbIntelligence.ValueChanged
        Mem.Player.Stats.INT.Value = sender.Value
    End Sub

    Private Sub nmbFaith_ValueChanged(sender As Object, e As EventArgs) Handles nmbFaith.ValueChanged
        Mem.Player.Stats.FTH.Value = sender.Value
    End Sub

    Private Sub nmbHumanity_ValueChanged(sender As Object, e As EventArgs) Handles nmbHumanity.ValueChanged
        Mem.Player.Stats.Humanity.Value = sender.Value
    End Sub

    Private Sub nmbGender_ValueChanged(sender As Object, e As EventArgs) Handles nmbGender.ValueChanged
        Mem.Player.Stats.ExternalGenitals.Value = sender.Value
    End Sub

    Private Sub nmbMaxHP_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxHP.ValueChanged
        Mem.Player.Stats.MaxHP.Value = sender.Value
    End Sub

    Private Sub nmbMaxStam_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxStam.ValueChanged
        Mem.Player.Stats.MaxStamina.Value = sender.Value
    End Sub

    'Private Sub btnScenarioPinwheelDefense_Click(sender As Object, e As EventArgs) Handles btnScenarioPinwheelDefense.Click
    '    trd = New Thread(AddressOf Funcs.ScenarioPinwheelDefense)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    Private Sub SomethingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiTestSomething.Click
        'ScriptEnvironment.Run("SetEventFlag 16, 0)
        'warp_coords_facing(71.72, 60, 300.56, 1.0)


        'TODO:  Fix the below.  Non-console scripts should have persistent vars.
        'Script.RunOneLine("New Int intvar = GetEntityPtr 6270")
        'Script.RunOneLine("ControlEntity intvar1, 0")
    End Sub

    Private Sub tsbtnDisableAI_Click(sender As Object, e As EventArgs) Handles tsbtnDisableAI.Click
        Funcs.DisableAI(True)
    End Sub

    Private Sub tsbtnEnableAI_Click(sender As Object, e As EventArgs) Handles tsbtnEnableAI.Click
        Funcs.DisableAI(False)
    End Sub

    Private Sub tsbtnEnablePlayerExterminate_Click(sender As Object, e As EventArgs) Handles tsbtnEnablePlayerExterminate.Click
        Funcs.PlayerExterminate(True)
    End Sub

    Private Sub tsbtnDisablePlayerExterminate_Click(sender As Object, e As EventArgs) Handles tsbtnDisablePlayerExterminate.Click
        Funcs.PlayerExterminate(False)
    End Sub

    'Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
    '    LegacyScripting.RunOneLine("New Int Player = 10000")
    '    LegacyScripting.RunOneLine("ChrFadeIn Player, 5.0, 0.0")
    'End Sub

    'Private Sub btnScenarioOandSandOandS_Click(sender As Object, e As EventArgs) Handles btnScenarioOandSandOandS.Click
    '    trd = New Thread(AddressOf Funcs.ScenarioOandSandOandS)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    'Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
    '    trd = New Thread(AddressOf Funcs.BeginTestBossRush)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    'Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
    '    trd = New Thread(AddressOf Funcs.ScenarioOandSandOandS_Debug)
    '    trd.IsBackground = True
    '    trd.Start()
    'End Sub

    Private Sub btnCancelBossRush_Click(sender As Object, e As EventArgs) Handles btnCancelBossRush.Click
        LuaInterface.E("ShowHUD(true)")
        Funcs.SetKeyGuideTextClear()
        Funcs.SetLineHelpTextClear()
        If (luaThread IsNot Nothing AndAlso luaThread.IsAlive) Then
            luaThread.Abort()
        End If
        BossRushHelper.StopBossRushTimer()
    End Sub

    'Private Sub btnNewConsole_Click(sender As Object, e As EventArgs) Handles btnNewConsole.Click

    '    If consoleWindow Is Nothing Then
    '        consoleWindow = New DaS.ScriptEditor.ConsoleWindow()
    '        consoleWindow.Show()
    '        AddHandler consoleWindow.FormClosed, AddressOf CurrentConsEditor_Closed
    '    Else
    '        consoleWindow.BringToFront()
    '        consoleWindow.Activate()
    '    End If

    'End Sub

    'Private Sub CurrentConsEditor_Closed(sender As Object, e As FormClosedEventArgs)
    '    RemoveHandler consoleWindow.FormClosed, AddressOf CurrentConsEditor_Closed
    '    consoleWindow = Nothing
    'End Sub

    Private Sub btnLoadBossScenario_Click(sender As Object, e As EventArgs) Handles btnLoadBossScenario.Click
        If luaThread IsNot Nothing AndAlso luaThread.IsAlive Then
            luaThread.Abort()
        End If
        LuaInterface.E($"SetClearCount({numBossScenarioNg.Value})")
        luaThread = New Thread(AddressOf DoBossScenario) With {.IsBackground = True}
        luaThread.Start(CType(comboBossList.SelectedItem, String))
    End Sub

    Private Sub DoBossScenario(bossName As String)
        Dim byName = Data.Misc.BossFights.
            Select(Function(kvp) New KeyValuePair(Of String, Integer)(kvp.Value.Name, CType(kvp.Key, Integer))).
            ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value)

        lua.State.DoString($"SpawnPlayerAtBoss({byName(bossName)})")
    End Sub

    Private Sub radioStandard_CheckedChanged(sender As Object, e As EventArgs) Handles radioStandard.CheckedChanged
        If radioStandard.Checked Then
            radioReverse.Checked = False
            radioRandom.Checked = False
            radioCustom.Checked = False
            checkSkipBedOfChaos.Enabled = True
            btnFlpCustomAdd.Enabled = False
            btnFlpCustomRemove.Enabled = False
            tlpCustomOrder.Enabled = False
        End If
    End Sub

    Private Sub radioReverse_CheckedChanged(sender As Object, e As EventArgs) Handles radioReverse.CheckedChanged
        If radioReverse.Checked Then
            radioStandard.Checked = False
            radioRandom.Checked = False
            radioCustom.Checked = False
            checkSkipBedOfChaos.Enabled = True
            btnFlpCustomAdd.Enabled = False
            btnFlpCustomRemove.Enabled = False
            tlpCustomOrder.Enabled = False
        End If
    End Sub

    Private Sub radioRandom_CheckedChanged(sender As Object, e As EventArgs) Handles radioRandom.CheckedChanged
        If radioRandom.Checked Then
            radioReverse.Checked = False
            radioStandard.Checked = False
            radioCustom.Checked = False
            checkSkipBedOfChaos.Enabled = True
            btnFlpCustomAdd.Enabled = False
            btnFlpCustomRemove.Enabled = False
            tlpCustomOrder.Enabled = False
        End If
    End Sub

    Private Sub radioCustom_CheckedChanged(sender As Object, e As EventArgs) Handles radioCustom.CheckedChanged
        If radioCustom.Checked Then
            radioReverse.Checked = False
            radioRandom.Checked = False
            radioStandard.Checked = False
            checkSkipBedOfChaos.Enabled = False
            btnFlpCustomAdd.Enabled = True
            btnFlpCustomRemove.Enabled = True
            tlpCustomOrder.Enabled = True
        End If
    End Sub

    Private Sub btnBeginBossRush_Click(sender As Object, e As EventArgs) Handles btnBeginBossRush.Click
        If Not lua.State.IsExecuting Then
            If luaThread IsNot Nothing AndAlso luaThread.IsAlive Then
                luaThread.Abort()
            End If
            luaThread = New Thread(AddressOf BeginBossRushOnNewThread) With {.IsBackground = True}
            luaThread.Start()
        End If
    End Sub

    Private Sub BeginBossRushOnNewThread()
        lua.State.Item("ShowBossNames") = (Not checkHideBossNames.Checked)
        lua.State.Item("TimeBetweenBosses") = CType(numCountdown.Value, Single)
        lua.State.Item("RandomizeBossNG") = checkRandomizeNg.Checked
        lua.State.Item("InfiniteLives") = checkInfiniteLives.Checked
        lua.State.Item("RefillHpEachFight") = checkHealEachFight.Checked

        Dim bossRushOrder = ""
        If radioStandard.Checked Then
            bossRushOrder = "Standard"
        ElseIf radioReverse.Checked Then
            bossRushOrder = "Reverse"
        ElseIf radioRandom.Checked Then
            bossRushOrder = "Random"
        ElseIf radioCustom.Checked Then
            bossRushOrder = "Custom"
        End If

        lua.State.Item("BossRushOrder") = bossRushOrder
        lua.State.Item("BossRushCustomOrder") = String.Join(";", GetFlpCustomBossOrderAsArray())
        lua.State.Item("BossRushExcludeBed") = checkSkipBedOfChaos.Checked

        lua.State.GetFunction("BeginBossRush").Call()
    End Sub

    Private Sub btnFlpCustomRemove_Click(sender As Object, e As EventArgs) Handles btnFlpCustomRemove.Click
        If tlpCustomOrder.Controls.Count > 0 Then
            tlpCustomOrder.Controls.RemoveAt(tlpCustomOrder.Controls.Count - 1)
            tlpCustomOrder.RowCount = tlpCustomOrder.Controls.Count
        End If
    End Sub

    Private Sub btnFlpCustomAdd_Click(sender As Object, e As EventArgs) Handles btnFlpCustomAdd.Click
        Dim comboNewEntry As New ComboBox()
        Dim oldTop = comboNewEntry.Top
        comboNewEntry.Dock = DockStyle.Fill
        comboNewEntry.Size = New Size(tlpCustomOrder.Size.Width - 16, 24)
        comboNewEntry.TabStop = False
        For Each bossName In comboBossList.Items
            comboNewEntry.Items.Add(bossName)
        Next
        If tlpCustomOrder.Controls.Count >= 2 Then
            comboNewEntry.SelectedItem = tlpCustomOrder.Controls.Cast(Of ComboBox).ToArray()(tlpCustomOrder.Controls.Count - 2).SelectedItem
        Else
            comboNewEntry.SelectedItem = Game.Data.Misc.BossFights(EventFlag.Boss.AsylumDemon).Name
        End If

        comboNewEntry.SelectionLength = 0

        tlpCustomOrder.RowCount = tlpCustomOrder.Controls.Count + 1
        tlpCustomOrder.Controls.Add(comboNewEntry)
        tlpCustomOrder.SetRow(comboNewEntry, tlpCustomOrder.Controls.Count - 1)
    End Sub

    Private Function GetFlpCustomBossOrderAsArray() As String()
        Dim byName = Data.Misc.BossFights.
            Select(Function(kvp) New KeyValuePair(Of String, Integer)(kvp.Value.Name, CType(kvp.Key, Integer))).
            ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value)

        Dim arr As String() = New String() {}
        tlpCustomOrder.Invoke(
            Sub()
                arr = tlpCustomOrder.Controls.
                OfType(Of ComboBox).
                Select(Function(x) byName(CType(x.SelectedItem, String)).ToString()).
                ToArray()
            End Sub)
        Return arr
    End Function

    Private Sub frmForm1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        _turnOffDoubleBufferino()
    End Sub

    Private Sub cmbBonfire_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbBonfire.SelectedValueChanged
        Dim selStr = CType(cmbBonfire.SelectedItem, String)
        selStr = selStr.Substring(selStr.IndexOf("[") + 1)
        selStr = selStr.Substring(0, selStr.Length - 1)
        Mem.Player.BonfireID.Value = Integer.Parse(selStr.Trim())
    End Sub

    Private Sub frmForm1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' Checks if this is the last form open in this process
        If Application.OpenForms.Count = 0 Then
            DARKSOULS.Close()
        End If
    End Sub

    'Private Sub flpCustomBossOrder_ControlAdded(sender As Object, e As ControlEventArgs)
    '    Dim bottomDist = (e.Control.Top + e.Control.Size.Height + 8) - tlpCustomOrder.Size.Height

    '    If bottomDist > 0 Then
    '        tlpCustomOrder.Size = New Size(tlpCustomOrder.Size.Width, tlpCustomOrder.Size.Height + bottomDist)
    '    End If
    'End Sub
End Class
