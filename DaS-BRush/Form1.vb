Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.CompilerServices

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

    Dim consoleScript As New Script("Untitled", "")
    Dim consoleScriptParams As ScriptThreadParams = ScriptThreadParams.GetNoThread(consoleScript)

    Public Const VersionCheckUrl = "http://wulf2k.ca/pc/das/das-brush-ver.txt"
    Public Const NoteCheckUrl = "http://wulf2k.ca/pc/das/das-brush-notes.txt"

    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Private trd As Thread

    Dim delay As Integer = 33

    Shared ConsoleOutputText As String = ""


    Dim playerHP As Integer
    Dim playerStam As Integer

    Dim playerMaxHP As Integer
    Dim playerMaxStam As Integer

    Dim playerFacing As Single
    Dim playerXpos As Single
    Dim playerYpos As Single
    Dim playerZpos As Single


    Public intvar1 As Integer
    Public intvar2 As Integer


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
                        Script.RunOneLine("Wait 1000")
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
        For Each bonfire In Data.listBonfireNames
            cmbBonfire.Items.Add(bonfire)
        Next
        cmbBonfire.SelectedItem = "Nothing"

        If Hook.InitHook() Then
            Funcs.setsaveenable(False)
        End If

        refTimer = New System.Windows.Forms.Timer
        refTimer.Interval = delay
        refTimer.Enabled = Hook.IsHooked

    End Sub

    Private Function getIsBossRushThreadActive() As Boolean

        If Not trd Is Nothing Then

            'TODO: Check this shit
            'If no worky, replace with
            'If trd.ThreadState = ThreadState.Stopped Or trd.ThreadState = ThreadState.Aborted Then
            'etc...
            Return trd.IsAlive
        Else
            Return False
        End If

    End Function

    ReadOnly Property ConsoleExecuting As Boolean
        Get
            If consoleScriptParams.ThisThread Is Nothing Then
                Return False
            Else
                If consoleScriptParams.ThisThread.IsAlive And Not consoleScriptParams.IsFinished Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    Private Sub RefreshGUI()

        Hook.UpdateHook()

        lblRelease.Text = Hook.DetectedDarkSoulsVersion

        txtConsoleResult.Text = consoleScriptParams.OutputStr

        Dim isBossRushing = getIsBossRushThreadActive()

        gbBosses.Enabled = Not isBossRushing

        For Each bossBtn As Button In gbBosses.Controls
            If bossBtn Is Nothing Then
                Continue For
            Else
                bossBtn.Enabled = Not isBossRushing
            End If
        Next

        cboxReverseOrder.Enabled = Not isBossRushing
        btnBeginBossRush.Enabled = Not isBossRushing
        btnCancelBossRush.Enabled = isBossRushing

        Dim isConsoling = ConsoleExecuting()

        btnConsoleExecute.Enabled = Not isConsoling
        btnConsoleCancel.Enabled = isConsoling
        rtbConsole.Enabled = Not isConsoling

        Select Case tabs.TabPages(tabs.SelectedIndex).Text
            Case "Player"

                lblHP.Text = Hook.Player.HP.ValueInt & " / " & Hook.Player.MaxHP.ValueInt
                lblStam.Text = Hook.Player.Stamina.ValueInt & " / " & Hook.Player.MaxStamina.ValueInt

                lblFacing.Text = "Heading: " & ((Hook.Player.Heading.ValueFloat + Math.PI) / (Math.PI * 2) * 360).ToString("0.00") & "°"
                lblXpos.Text = Hook.Player.PosX.ValueFloat.ToString("0.00")
                lblYpos.Text = Hook.Player.PosY.ValueFloat.ToString("0.00")
                lblZpos.Text = Hook.Player.PosZ.ValueFloat.ToString("0.00")

                lblstableXpos.Text = Hook.Player.StablePosX.ValueFloat.ToString("0.00")
                lblstableYpos.Text = Hook.Player.StablePosY.ValueFloat.ToString("0.00")
                lblstableZpos.Text = Hook.Player.StablePosZ.ValueFloat.ToString("0.00")

                Dim bonfireID As Integer
                bonfireID = Hook.Player.BonfireID.ValueInt
                If Not cmbBonfire.DroppedDown Then
                    If Data.clsBonfires(bonfireID) = "" Then
                        Data.clsBonfires.Add(bonfireID, bonfireID.ToString)
                        Data.clsBonfiresIDs.Add(bonfireID.ToString, bonfireID)
                        cmbBonfire.Items.Add(bonfireID.ToString)
                    End If
                    cmbBonfire.SelectedItem = Data.clsBonfires(bonfireID)
                End If


            Case "Stats"
                nmbMaxHP.FuckOff(Hook.Stats.MaxHP.ValueInt)
                nmbMaxStam.FuckOff(Hook.Stats.MaxStamina.ValueInt)
                nmbVitality.FuckOff(Hook.Stats.VIT.ValueInt)
                nmbAttunement.FuckOff(Hook.Stats.ATN.ValueInt)
                nmbEnd.FuckOff(Hook.Stats.ENDurance.ValueInt)
                nmbStr.FuckOff(Hook.Stats.STR.ValueInt)
                nmbDex.FuckOff(Hook.Stats.DEX.ValueInt)
                nmbIntelligence.FuckOff(Hook.Stats.INT.ValueInt)
                nmbFaith.FuckOff(Hook.Stats.FTH.ValueInt)
                nmbResistance.FuckOff(Hook.Stats.RES.ValueInt)
                nmbHumanity.FuckOff(Hook.Stats.Humanity.ValueInt)
                nmbGender.FuckOff(Hook.Stats.ExternalGenitals.ValueInt)
                nmbClearCount.FuckOff(Hook.GameStats.ClearCount.ValueInt)
        End Select

    End Sub

    Private Sub refTimer_Tick() Handles refTimer.Tick

        RefreshGUI()

    End Sub








    Private Sub btnBossAsylumDemon_Click(sender As Object, e As EventArgs) Handles btnBossAsylumDemon.Click, Button1.Click
        trd = New Thread(AddressOf Funcs.bossasylum)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBedOfChaos_Click(sender As Object, e As EventArgs) Handles btnBossBedOfChaos.Click, Button6.Click
        trd = New Thread(AddressOf Funcs.bossbedofchaos)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBellGargoyles_Click(sender As Object, e As EventArgs) Handles btnBossBellGargoyles.Click, Button4.Click
        trd = New Thread(AddressOf Funcs.bossbellgargoyles)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBlackDragonKalameet_Click(sender As Object, e As EventArgs) Handles btnBossBlackDragonKalameet.Click, Button26.Click
        trd = New Thread(AddressOf Funcs.bossblackdragonkalameet)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCapraDemon_Click(sender As Object, e As EventArgs) Handles btnBossCapraDemon.Click, Button3.Click
        trd = New Thread(AddressOf Funcs.bosscaprademon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCeaselessDischarge_Click(sender As Object, e As EventArgs) Handles btnBossCeaselessDischarge.Click, Button5.Click
        trd = New Thread(AddressOf Funcs.bossceaselessdischarge)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCentipedeDemon_Click(sender As Object, e As EventArgs) Handles btnBossCentipedeDemon.Click, Button7.Click
        trd = New Thread(AddressOf Funcs.bosscentipededemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossChaosWitchQuelaag_Click(sender As Object, e As EventArgs) Handles btnBossChaosWitchQuelaag.Click, Button8.Click
        trd = New Thread(AddressOf Funcs.bosschaoswitchquelaag)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCrossbreedPriscilla_Click(sender As Object, e As EventArgs) Handles btnBossCrossbreedPriscilla.Click, Button9.Click
        trd = New Thread(AddressOf Funcs.bosscrossbreedpriscilla)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDarkSunGwyndolin_Click(sender As Object, e As EventArgs) Handles btnBossDarkSunGwyndolin.Click, Button10.Click
        trd = New Thread(AddressOf Funcs.bossdarksungwyndolin)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDemonFiresage_Click(sender As Object, e As EventArgs) Handles btnBossDemonFiresage.Click, Button11.Click
        trd = New Thread(AddressOf Funcs.bossdemonfiresage)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossFourKings_Click(sender As Object, e As EventArgs) Handles btnBossFourKings.Click, Button13.Click
        trd = New Thread(AddressOf Funcs.bossfourkings)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGapingDragon_Click(sender As Object, e As EventArgs) Handles btnBossGapingDragon.Click, Button14.Click
        trd = New Thread(AddressOf Funcs.bossgapingdragon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGravelordNito_Click(sender As Object, e As EventArgs) Handles btnBossGravelordNito.Click, Button15.Click
        trd = New Thread(AddressOf Funcs.bossgravelordnito)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGwyn_Click(sender As Object, e As EventArgs) Handles btnBossGwyn.Click, Button16.Click
        trd = New Thread(AddressOf Funcs.bossgwyn)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossIronGolem_Click(sender As Object, e As EventArgs) Handles btnBossIronGolem.Click, Button17.Click
        trd = New Thread(AddressOf Funcs.bossirongolem)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossKnightArtorias_Click(sender As Object, e As EventArgs) Handles btnBossKnightArtorias.Click, Button18.Click
        trd = New Thread(AddressOf Funcs.bossknightartorias)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossManus_Click(sender As Object, e As EventArgs) Handles btnBossManus.Click, Button19.Click
        trd = New Thread(AddressOf Funcs.bossmanus)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossMoonlightButterfly_Click(sender As Object, e As EventArgs) Handles btnBossMoonlightButterfly.Click, Button20.Click
        trd = New Thread(AddressOf Funcs.bossmoonlightbutterfly)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossOandS_Click(sender As Object, e As EventArgs) Handles btnBossOandS.Click, Button12.Click
        trd = New Thread(AddressOf Funcs.bossoands)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossPinwheel_Click(sender As Object, e As EventArgs) Handles btnBossPinwheel.Click, Button2.Click
        trd = New Thread(AddressOf Funcs.bosspinwheel)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSanctuaryGuardian_Click(sender As Object, e As EventArgs) Handles btnBossSanctuaryGuardian.Click, Button21.Click
        trd = New Thread(AddressOf Funcs.bosssanctuaryguardian)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSeath_Click(sender As Object, e As EventArgs) Handles btnBossSeath.Click, Button22.Click
        trd = New Thread(AddressOf Funcs.bossseath)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSif_Click(sender As Object, e As EventArgs) Handles btnBossSif.Click, Button24.Click
        trd = New Thread(AddressOf Funcs.bosssif)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossStrayDemon_Click(sender As Object, e As EventArgs) Handles btnBossStrayDemon.Click, Button23.Click
        trd = New Thread(AddressOf Funcs.bossstraydemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossTaurusDemon_Click(sender As Object, e As EventArgs) Handles btnBossTaurusDemon.Click, Button25.Click
        trd = New Thread(AddressOf Funcs.bosstaurusdemon)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnScenarioArtoriasCiaran_Click(sender As Object, e As EventArgs) Handles btnScenarioArtoriasCiaran.Click
        trd = New Thread(AddressOf Funcs.scenarioartoriasandciaran)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnScenarioTripleSanctuary_Click(sender As Object, e As EventArgs) Handles btnScenarioTripleSanctuary.Click
        trd = New Thread(AddressOf Funcs.scenariotriplesanctuaryguardian)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnBeginBossRush_Click(sender As Object, e As EventArgs) Handles btnBeginBossRush.Click

        If (cboxReverseOrder.Checked) Then
            trd = New Thread(AddressOf Funcs.beginreversebossrush)
        Else
            trd = New Thread(AddressOf Funcs.beginbossrush)
        End If
        trd.IsBackground = True
        trd.Start()

    End Sub

    Private Sub btnDonate_Click(sender As Object, e As EventArgs) Handles btnDonate.Click
        Dim webAddress As String = "http://paypal.me/wulf2k/"
        Process.Start(webAddress)
    End Sub

    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        Hook.InitHook()
    End Sub



    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim updateWindow As New UpdateWindow(sender.Tag)
        updateWindow.ShowDialog()
        If updateWindow.WasSuccessful Then
            Process.Start(updateWindow.NewAssembly, """--old-file=" & updateWindow.OldAssembly & """")
            Me.Close()
        End If
    End Sub

    Private Sub btnTestDisableAI_Click(sender As Object, e As EventArgs)
        Funcs.disableai(True)
    End Sub

    Private Sub btnTestEnableAI_Click(sender As Object, e As EventArgs)
        Funcs.disableai(False)
    End Sub

    Private Sub nmbVitality_ValueChanged(sender As Object, e As EventArgs) Handles nmbVitality.ValueChanged
        Hook.Stats.VIT.ValueInt = sender.Value
    End Sub

    Private Sub nmbAttunement_ValueChanged(sender As Object, e As EventArgs) Handles nmbAttunement.ValueChanged
        Hook.Stats.ATN.ValueInt = sender.Value
    End Sub

    Private Sub nmbEnd_ValueChanged(sender As Object, e As EventArgs) Handles nmbEnd.ValueChanged
        Hook.Stats.ENDurance.ValueInt = sender.Value
    End Sub

    Private Sub nmbStr_ValueChanged(sender As Object, e As EventArgs) Handles nmbStr.ValueChanged
        Hook.Stats.STR.ValueInt = sender.Value
    End Sub

    Private Sub nmbDex_ValueChanged(sender As Object, e As EventArgs) Handles nmbDex.ValueChanged
        Hook.Stats.DEX.ValueInt = sender.Value
    End Sub

    Private Sub nmbResistance_ValueChanged(sender As Object, e As EventArgs) Handles nmbResistance.ValueChanged
        Hook.Stats.RES.ValueInt = sender.Value
    End Sub

    Private Sub nmbIntelligence_ValueChanged(sender As Object, e As EventArgs) Handles nmbIntelligence.ValueChanged
        Hook.Stats.INT.ValueInt = sender.Value
    End Sub

    Private Sub nmbFaith_ValueChanged(sender As Object, e As EventArgs) Handles nmbFaith.ValueChanged
        Hook.Stats.FTH.ValueInt = sender.Value
    End Sub

    Private Sub nmbHumanity_ValueChanged(sender As Object, e As EventArgs) Handles nmbHumanity.ValueChanged
        Hook.Stats.Humanity.ValueInt = sender.Value
    End Sub

    Private Sub nmbGender_ValueChanged(sender As Object, e As EventArgs) Handles nmbGender.ValueChanged
        Hook.Stats.ExternalGenitals.ValueInt = sender.Value
    End Sub

    Private Sub nmbMaxHP_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxHP.ValueChanged
        Hook.Stats.MaxHP.ValueInt = sender.Value
    End Sub

    Private Sub nmbMaxStam_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxStam.ValueChanged
        Hook.Stats.MaxStamina.ValueInt = sender.Value
    End Sub

    Private Sub btnConsoleExecute_Click(sender As Object, e As EventArgs) Handles btnConsoleExecute.Click
        consoleScript = New Script(consoleScript.Name, String.Join(Environment.NewLine, rtbConsole.Lines))
        consoleScriptParams = consoleScript.ExecuteInBackground()
    End Sub



    Private Sub btnConsoleHelp_Click(sender As Object, e As EventArgs) Handles btnConsoleHelp.Click
        Dim webAddress As String = "https://docs.google.com/spreadsheets/d/1Gff9pSGpYCJeNAXzUamqAInqFUwk4BhC6dC9Qk3_cDI/edit#gid=0"
        Process.Start(webAddress)
    End Sub

    Private Sub btnScenarioPinwheelDefense_Click(sender As Object, e As EventArgs) Handles btnScenarioPinwheelDefense.Click
        trd = New Thread(AddressOf Funcs.scenariopinwheeldefense)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub SomethingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiTestSomething.Click
        'ScriptEnvironment.Run("SetEventFlag 16, 0)
        'warp_coords_facing(71.72, 60, 300.56, 1.0)

        Script.RunOneLine("intvar1 = GetEntityPtr 1010700")
        Script.RunOneLine("ControlEntity intvar1, 0")
    End Sub

    Private Sub tsbtnDisableAI_Click(sender As Object, e As EventArgs) Handles tsbtnDisableAI.Click
        Funcs.disableai(True)
    End Sub

    Private Sub tsbtnEnableAI_Click(sender As Object, e As EventArgs) Handles tsbtnEnableAI.Click
        Funcs.disableai(False)
    End Sub

    Private Sub tsbtnEnablePlayerExterminate_Click(sender As Object, e As EventArgs) Handles tsbtnEnablePlayerExterminate.Click
        Funcs.playerexterminate(True)
    End Sub

    Private Sub tsbtnDisablePlayerExterminate_Click(sender As Object, e As EventArgs) Handles tsbtnDisablePlayerExterminate.Click
        Funcs.playerexterminate(False)
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Script.RunOneLine("New Int Player = 10000")
        Script.RunOneLine("ChrFadeIn Player, 5.0, 0.0")
    End Sub

    Private Sub btnScenarioOandSandOandS_Click(sender As Object, e As EventArgs) Handles btnScenarioOandSandOandS.Click
        trd = New Thread(AddressOf Funcs.scenariooandsandoands)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        trd = New Thread(AddressOf Funcs.begintestbossrush)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        trd = New Thread(AddressOf Funcs.debug_scenariooandsandoands)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnCancelBossRush_Click(sender As Object, e As EventArgs) Handles btnCancelBossRush.Click
        Script.RunOneLine("ShowHUD 1")
        WInt32(RInt32(&H13786D0) + &H154, -1)
        WInt32(RInt32(&H13786D0) + &H158, -1)

        If Hook.rushMode Then Hook.rushTimer.Abort()
        Hook.rushMode = False
        trd.Abort()
        Console.WriteLine(trd.ThreadState)
    End Sub

    Private Sub btnConsoleCancel_Click(sender As Object, e As EventArgs) Handles btnConsoleCancel.Click
        consoleScriptParams.ThisThread.Abort()
        consoleScript.CleanAbortedThreads()
    End Sub
End Class
