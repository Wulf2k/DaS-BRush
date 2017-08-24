<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmForm1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmForm1))
        Me.tabs = New System.Windows.Forms.TabControl()
        Me.tabBosses = New System.Windows.Forms.TabPage()
        Me.tlpCustomOrder = New System.Windows.Forms.TableLayoutPanel()
        Me.btnFlpCustomRemove = New System.Windows.Forms.Button()
        Me.btnFlpCustomAdd = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.numCountdown = New System.Windows.Forms.NumericUpDown()
        Me.checkHideBossNames = New System.Windows.Forms.CheckBox()
        Me.checkInfiniteLives = New System.Windows.Forms.CheckBox()
        Me.checkHealEachFight = New System.Windows.Forms.CheckBox()
        Me.checkRandomizeNg = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.checkSkipBedOfChaos = New System.Windows.Forms.CheckBox()
        Me.btnLoadBossScenario = New System.Windows.Forms.Button()
        Me.radioCustom = New System.Windows.Forms.RadioButton()
        Me.comboBossList = New System.Windows.Forms.ComboBox()
        Me.radioRandom = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.radioReverse = New System.Windows.Forms.RadioButton()
        Me.btnCancelBossRush = New System.Windows.Forms.Button()
        Me.radioStandard = New System.Windows.Forms.RadioButton()
        Me.btnBeginBossRush = New System.Windows.Forms.Button()
        Me.tabScenarios = New System.Windows.Forms.TabPage()
        Me.btnScenarioOandSandOandS = New System.Windows.Forms.Button()
        Me.btnScenarioPinwheelDefense = New System.Windows.Forms.Button()
        Me.btnScenarioTripleSanctuary = New System.Windows.Forms.Button()
        Me.btnScenarioArtoriasCiaran = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblPlaytime = New System.Windows.Forms.Label()
        Me.lblHP = New System.Windows.Forms.Label()
        Me.lblStam = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblstableZpos = New System.Windows.Forms.Label()
        Me.lblstableYpos = New System.Windows.Forms.Label()
        Me.lblstableXpos = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.cmbBonfire = New System.Windows.Forms.ComboBox()
        Me.lblBonfire = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.lblFacing = New System.Windows.Forms.Label()
        Me.lblZpos = New System.Windows.Forms.Label()
        Me.lblYpos = New System.Windows.Forms.Label()
        Me.lblXpos = New System.Windows.Forms.Label()
        Me.tabStats = New System.Windows.Forms.TabPage()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.nmbClearCount = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nmbMaxHP = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nmbMaxStam = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nmbGender = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.nmbVitality = New System.Windows.Forms.NumericUpDown()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.nmbAttunement = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.nmbEnd = New System.Windows.Forms.NumericUpDown()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.nmbStr = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.nmbDex = New System.Windows.Forms.NumericUpDown()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.nmbResistance = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.nmbIntelligence = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.nmbFaith = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nmbHumanity = New System.Windows.Forms.NumericUpDown()
        Me.tabTests = New System.Windows.Forms.TabPage()
        Me.toolstripTest = New System.Windows.Forms.ToolStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsmiTestSomething = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnDisableAI = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnEnableAI = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnEnablePlayerExterminate = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnDisablePlayerExterminate = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tabNotes = New System.Windows.Forms.TabPage()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.tabAbout = New System.Windows.Forms.TabPage()
        Me.AboutPage1 = New DaS_BRush.AboutPage()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.lblRelease = New System.Windows.Forms.Label()
        Me.btnReconnect = New System.Windows.Forms.Button()
        Me.lblVer = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.btnNewConsole = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.numBossScenarioNg = New System.Windows.Forms.NumericUpDown()
        Me.tabs.SuspendLayout()
        Me.tabBosses.SuspendLayout()
        CType(Me.numCountdown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabScenarios.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabStats.SuspendLayout()
        CType(Me.nmbClearCount, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbMaxHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbMaxStam, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbGender, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbVitality, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbAttunement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbStr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbDex, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbResistance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbIntelligence, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbFaith, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nmbHumanity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabTests.SuspendLayout()
        Me.toolstripTest.SuspendLayout()
        Me.tabNotes.SuspendLayout()
        Me.tabAbout.SuspendLayout()
        CType(Me.numBossScenarioNg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabs
        '
        Me.tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabs.Controls.Add(Me.tabBosses)
        Me.tabs.Controls.Add(Me.tabScenarios)
        Me.tabs.Controls.Add(Me.tabMain)
        Me.tabs.Controls.Add(Me.tabStats)
        Me.tabs.Controls.Add(Me.tabTests)
        Me.tabs.Controls.Add(Me.tabNotes)
        Me.tabs.Controls.Add(Me.tabAbout)
        Me.tabs.HotTrack = True
        Me.tabs.Location = New System.Drawing.Point(2, 34)
        Me.tabs.Name = "tabs"
        Me.tabs.SelectedIndex = 0
        Me.tabs.Size = New System.Drawing.Size(520, 325)
        Me.tabs.TabIndex = 4
        '
        'tabBosses
        '
        Me.tabBosses.AutoScroll = True
        Me.tabBosses.AutoScrollMargin = New System.Drawing.Size(8, 8)
        Me.tabBosses.Controls.Add(Me.numBossScenarioNg)
        Me.tabBosses.Controls.Add(Me.Label17)
        Me.tabBosses.Controls.Add(Me.tlpCustomOrder)
        Me.tabBosses.Controls.Add(Me.btnFlpCustomRemove)
        Me.tabBosses.Controls.Add(Me.btnFlpCustomAdd)
        Me.tabBosses.Controls.Add(Me.Label6)
        Me.tabBosses.Controls.Add(Me.numCountdown)
        Me.tabBosses.Controls.Add(Me.checkHideBossNames)
        Me.tabBosses.Controls.Add(Me.checkInfiniteLives)
        Me.tabBosses.Controls.Add(Me.checkHealEachFight)
        Me.tabBosses.Controls.Add(Me.checkRandomizeNg)
        Me.tabBosses.Controls.Add(Me.Label5)
        Me.tabBosses.Controls.Add(Me.checkSkipBedOfChaos)
        Me.tabBosses.Controls.Add(Me.btnLoadBossScenario)
        Me.tabBosses.Controls.Add(Me.radioCustom)
        Me.tabBosses.Controls.Add(Me.comboBossList)
        Me.tabBosses.Controls.Add(Me.radioRandom)
        Me.tabBosses.Controls.Add(Me.Label4)
        Me.tabBosses.Controls.Add(Me.radioReverse)
        Me.tabBosses.Controls.Add(Me.btnCancelBossRush)
        Me.tabBosses.Controls.Add(Me.radioStandard)
        Me.tabBosses.Controls.Add(Me.btnBeginBossRush)
        Me.tabBosses.Location = New System.Drawing.Point(4, 22)
        Me.tabBosses.Margin = New System.Windows.Forms.Padding(0)
        Me.tabBosses.Name = "tabBosses"
        Me.tabBosses.Size = New System.Drawing.Size(512, 299)
        Me.tabBosses.TabIndex = 5
        Me.tabBosses.Text = "Boss Rush"
        Me.tabBosses.UseVisualStyleBackColor = True
        '
        'tlpCustomOrder
        '
        Me.tlpCustomOrder.AutoSize = True
        Me.tlpCustomOrder.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.tlpCustomOrder.ColumnCount = 1
        Me.tlpCustomOrder.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCustomOrder.Enabled = False
        Me.tlpCustomOrder.Location = New System.Drawing.Point(56, 127)
        Me.tlpCustomOrder.Name = "tlpCustomOrder"
        Me.tlpCustomOrder.RowCount = 1
        Me.tlpCustomOrder.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCustomOrder.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpCustomOrder.Size = New System.Drawing.Size(221, 6)
        Me.tlpCustomOrder.TabIndex = 95
        '
        'btnFlpCustomRemove
        '
        Me.btnFlpCustomRemove.Enabled = False
        Me.btnFlpCustomRemove.Location = New System.Drawing.Point(30, 127)
        Me.btnFlpCustomRemove.Margin = New System.Windows.Forms.Padding(0)
        Me.btnFlpCustomRemove.Name = "btnFlpCustomRemove"
        Me.btnFlpCustomRemove.Size = New System.Drawing.Size(23, 23)
        Me.btnFlpCustomRemove.TabIndex = 94
        Me.btnFlpCustomRemove.Text = "-"
        Me.btnFlpCustomRemove.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFlpCustomRemove.UseVisualStyleBackColor = True
        '
        'btnFlpCustomAdd
        '
        Me.btnFlpCustomAdd.Enabled = False
        Me.btnFlpCustomAdd.Location = New System.Drawing.Point(30, 150)
        Me.btnFlpCustomAdd.Margin = New System.Windows.Forms.Padding(0)
        Me.btnFlpCustomAdd.Name = "btnFlpCustomAdd"
        Me.btnFlpCustomAdd.Size = New System.Drawing.Size(23, 23)
        Me.btnFlpCustomAdd.TabIndex = 93
        Me.btnFlpCustomAdd.Text = "+"
        Me.btnFlpCustomAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFlpCustomAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(101, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(203, 13)
        Me.Label6.TabIndex = 91
        Me.Label6.Text = "Countdown before each boss in seconds:"
        '
        'numCountdown
        '
        Me.numCountdown.DecimalPlaces = 3
        Me.numCountdown.Location = New System.Drawing.Point(310, 93)
        Me.numCountdown.Name = "numCountdown"
        Me.numCountdown.Size = New System.Drawing.Size(142, 20)
        Me.numCountdown.TabIndex = 90
        Me.numCountdown.Value = New Decimal(New Integer() {30, 0, 0, 65536})
        '
        'checkHideBossNames
        '
        Me.checkHideBossNames.AutoSize = True
        Me.checkHideBossNames.Location = New System.Drawing.Point(104, 52)
        Me.checkHideBossNames.Name = "checkHideBossNames"
        Me.checkHideBossNames.Size = New System.Drawing.Size(161, 17)
        Me.checkHideBossNames.TabIndex = 89
        Me.checkHideBossNames.Text = "Hide Upcoming Boss Names"
        Me.checkHideBossNames.UseVisualStyleBackColor = True
        '
        'checkInfiniteLives
        '
        Me.checkInfiniteLives.AutoSize = True
        Me.checkInfiniteLives.Checked = True
        Me.checkInfiniteLives.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkInfiniteLives.Location = New System.Drawing.Point(104, 72)
        Me.checkInfiniteLives.Name = "checkInfiniteLives"
        Me.checkInfiniteLives.Size = New System.Drawing.Size(170, 17)
        Me.checkInfiniteLives.TabIndex = 88
        Me.checkInfiniteLives.Text = "Infinite Attempts on Each Boss"
        Me.checkInfiniteLives.UseVisualStyleBackColor = True
        '
        'checkHealEachFight
        '
        Me.checkHealEachFight.AutoSize = True
        Me.checkHealEachFight.Checked = True
        Me.checkHealEachFight.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkHealEachFight.Location = New System.Drawing.Point(271, 52)
        Me.checkHealEachFight.Name = "checkHealEachFight"
        Me.checkHealEachFight.Size = New System.Drawing.Size(199, 17)
        Me.checkHealEachFight.TabIndex = 87
        Me.checkHealEachFight.Text = "Fully Heal/Revive Before Each Boss"
        Me.checkHealEachFight.UseVisualStyleBackColor = True
        '
        'checkRandomizeNg
        '
        Me.checkRandomizeNg.AutoSize = True
        Me.checkRandomizeNg.Location = New System.Drawing.Point(271, 32)
        Me.checkRandomizeNg.Name = "checkRandomizeNg"
        Me.checkRandomizeNg.Size = New System.Drawing.Size(188, 17)
        Me.checkRandomizeNg.TabIndex = 86
        Me.checkRandomizeNg.Text = "Randomize Each Boss's NG-Level"
        Me.checkRandomizeNg.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 13)
        Me.Label5.TabIndex = 84
        Me.Label5.Text = "Boss Rush Order:"
        '
        'checkSkipBedOfChaos
        '
        Me.checkSkipBedOfChaos.AutoSize = True
        Me.checkSkipBedOfChaos.Checked = True
        Me.checkSkipBedOfChaos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.checkSkipBedOfChaos.Location = New System.Drawing.Point(104, 32)
        Me.checkSkipBedOfChaos.Name = "checkSkipBedOfChaos"
        Me.checkSkipBedOfChaos.Size = New System.Drawing.Size(131, 17)
        Me.checkSkipBedOfChaos.TabIndex = 5
        Me.checkSkipBedOfChaos.Text = "Exclude Bed of Chaos"
        Me.checkSkipBedOfChaos.UseVisualStyleBackColor = True
        '
        'btnLoadBossScenario
        '
        Me.btnLoadBossScenario.Location = New System.Drawing.Point(375, 256)
        Me.btnLoadBossScenario.Name = "btnLoadBossScenario"
        Me.btnLoadBossScenario.Size = New System.Drawing.Size(103, 22)
        Me.btnLoadBossScenario.TabIndex = 83
        Me.btnLoadBossScenario.Text = "Load"
        Me.btnLoadBossScenario.UseVisualStyleBackColor = True
        '
        'radioCustom
        '
        Me.radioCustom.AutoSize = True
        Me.radioCustom.Location = New System.Drawing.Point(23, 104)
        Me.radioCustom.Name = "radioCustom"
        Me.radioCustom.Size = New System.Drawing.Size(63, 17)
        Me.radioCustom.TabIndex = 4
        Me.radioCustom.Text = "Custom:"
        Me.radioCustom.UseVisualStyleBackColor = True
        '
        'comboBossList
        '
        Me.comboBossList.FormattingEnabled = True
        Me.comboBossList.Location = New System.Drawing.Point(298, 229)
        Me.comboBossList.Name = "comboBossList"
        Me.comboBossList.Size = New System.Drawing.Size(180, 21)
        Me.comboBossList.TabIndex = 82
        '
        'radioRandom
        '
        Me.radioRandom.AutoSize = True
        Me.radioRandom.Location = New System.Drawing.Point(23, 80)
        Me.radioRandom.Name = "radioRandom"
        Me.radioRandom.Size = New System.Drawing.Size(65, 17)
        Me.radioRandom.TabIndex = 3
        Me.radioRandom.Text = "Random"
        Me.radioRandom.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(295, 213)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(153, 13)
        Me.Label4.TabIndex = 81
        Me.Label4.Text = "Load Individual Boss Scenario:"
        '
        'radioReverse
        '
        Me.radioReverse.AutoSize = True
        Me.radioReverse.Location = New System.Drawing.Point(23, 56)
        Me.radioReverse.Name = "radioReverse"
        Me.radioReverse.Size = New System.Drawing.Size(65, 17)
        Me.radioReverse.TabIndex = 2
        Me.radioReverse.Text = "Reverse"
        Me.radioReverse.UseVisualStyleBackColor = True
        '
        'btnCancelBossRush
        '
        Me.btnCancelBossRush.Location = New System.Drawing.Point(298, 166)
        Me.btnCancelBossRush.Name = "btnCancelBossRush"
        Me.btnCancelBossRush.Size = New System.Drawing.Size(171, 23)
        Me.btnCancelBossRush.TabIndex = 80
        Me.btnCancelBossRush.Text = "Cancel"
        Me.btnCancelBossRush.UseVisualStyleBackColor = True
        '
        'radioStandard
        '
        Me.radioStandard.AutoSize = True
        Me.radioStandard.Checked = True
        Me.radioStandard.Location = New System.Drawing.Point(23, 32)
        Me.radioStandard.Name = "radioStandard"
        Me.radioStandard.Size = New System.Drawing.Size(68, 17)
        Me.radioStandard.TabIndex = 1
        Me.radioStandard.TabStop = True
        Me.radioStandard.Text = "Standard"
        Me.radioStandard.UseVisualStyleBackColor = True
        '
        'btnBeginBossRush
        '
        Me.btnBeginBossRush.Location = New System.Drawing.Point(298, 127)
        Me.btnBeginBossRush.Name = "btnBeginBossRush"
        Me.btnBeginBossRush.Size = New System.Drawing.Size(171, 33)
        Me.btnBeginBossRush.TabIndex = 77
        Me.btnBeginBossRush.Text = "Begin"
        Me.btnBeginBossRush.UseVisualStyleBackColor = False
        '
        'tabScenarios
        '
        Me.tabScenarios.AutoScroll = True
        Me.tabScenarios.AutoScrollMargin = New System.Drawing.Size(8, 8)
        Me.tabScenarios.Controls.Add(Me.btnScenarioOandSandOandS)
        Me.tabScenarios.Controls.Add(Me.btnScenarioPinwheelDefense)
        Me.tabScenarios.Controls.Add(Me.btnScenarioTripleSanctuary)
        Me.tabScenarios.Controls.Add(Me.btnScenarioArtoriasCiaran)
        Me.tabScenarios.Location = New System.Drawing.Point(4, 22)
        Me.tabScenarios.Name = "tabScenarios"
        Me.tabScenarios.Size = New System.Drawing.Size(512, 306)
        Me.tabScenarios.TabIndex = 9
        Me.tabScenarios.Text = "Scenarios"
        Me.tabScenarios.UseVisualStyleBackColor = True
        '
        'btnScenarioOandSandOandS
        '
        Me.btnScenarioOandSandOandS.Enabled = False
        Me.btnScenarioOandSandOandS.Location = New System.Drawing.Point(3, 94)
        Me.btnScenarioOandSandOandS.Name = "btnScenarioOandSandOandS"
        Me.btnScenarioOandSandOandS.Size = New System.Drawing.Size(197, 38)
        Me.btnScenarioOandSandOandS.TabIndex = 72
        Me.btnScenarioOandSandOandS.Text = "Onstein, Smough, Giant Ornstein, and Super Smough 4v1"
        Me.btnScenarioOandSandOandS.UseVisualStyleBackColor = True
        '
        'btnScenarioPinwheelDefense
        '
        Me.btnScenarioPinwheelDefense.Enabled = False
        Me.btnScenarioPinwheelDefense.Location = New System.Drawing.Point(3, 65)
        Me.btnScenarioPinwheelDefense.Name = "btnScenarioPinwheelDefense"
        Me.btnScenarioPinwheelDefense.Size = New System.Drawing.Size(198, 23)
        Me.btnScenarioPinwheelDefense.TabIndex = 71
        Me.btnScenarioPinwheelDefense.Text = "Pinwheel's Defense"
        Me.btnScenarioPinwheelDefense.UseVisualStyleBackColor = False
        '
        'btnScenarioTripleSanctuary
        '
        Me.btnScenarioTripleSanctuary.Enabled = False
        Me.btnScenarioTripleSanctuary.Location = New System.Drawing.Point(3, 36)
        Me.btnScenarioTripleSanctuary.Name = "btnScenarioTripleSanctuary"
        Me.btnScenarioTripleSanctuary.Size = New System.Drawing.Size(198, 23)
        Me.btnScenarioTripleSanctuary.TabIndex = 70
        Me.btnScenarioTripleSanctuary.Text = "3x Sanctuary Guardian"
        Me.btnScenarioTripleSanctuary.UseVisualStyleBackColor = False
        '
        'btnScenarioArtoriasCiaran
        '
        Me.btnScenarioArtoriasCiaran.Enabled = False
        Me.btnScenarioArtoriasCiaran.Location = New System.Drawing.Point(3, 7)
        Me.btnScenarioArtoriasCiaran.Name = "btnScenarioArtoriasCiaran"
        Me.btnScenarioArtoriasCiaran.Size = New System.Drawing.Size(198, 23)
        Me.btnScenarioArtoriasCiaran.TabIndex = 69
        Me.btnScenarioArtoriasCiaran.Text = "Knight Artorias + Ciaran"
        Me.btnScenarioArtoriasCiaran.UseVisualStyleBackColor = False
        '
        'tabMain
        '
        Me.tabMain.AutoScroll = True
        Me.tabMain.AutoScrollMargin = New System.Drawing.Size(8, 8)
        Me.tabMain.Controls.Add(Me.GroupBox2)
        Me.tabMain.Controls.Add(Me.GroupBox1)
        Me.tabMain.Location = New System.Drawing.Point(4, 22)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMain.Size = New System.Drawing.Size(512, 306)
        Me.tabMain.TabIndex = 0
        Me.tabMain.Text = "Player"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblPlaytime)
        Me.GroupBox2.Controls.Add(Me.lblHP)
        Me.GroupBox2.Controls.Add(Me.lblStam)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 10)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(371, 116)
        Me.GroupBox2.TabIndex = 48
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Status"
        '
        'lblPlaytime
        '
        Me.lblPlaytime.AutoSize = True
        Me.lblPlaytime.Location = New System.Drawing.Point(13, 63)
        Me.lblPlaytime.Name = "lblPlaytime"
        Me.lblPlaytime.Size = New System.Drawing.Size(33, 13)
        Me.lblPlaytime.TabIndex = 43
        Me.lblPlaytime.Text = "Time:"
        '
        'lblHP
        '
        Me.lblHP.AutoSize = True
        Me.lblHP.Location = New System.Drawing.Point(13, 27)
        Me.lblHP.Name = "lblHP"
        Me.lblHP.Size = New System.Drawing.Size(31, 13)
        Me.lblHP.TabIndex = 41
        Me.lblHP.Text = "HP:  "
        '
        'lblStam
        '
        Me.lblStam.AutoSize = True
        Me.lblStam.Location = New System.Drawing.Point(13, 40)
        Me.lblStam.Name = "lblStam"
        Me.lblStam.Size = New System.Drawing.Size(51, 13)
        Me.lblStam.TabIndex = 42
        Me.lblStam.Text = "Stamina: "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblstableZpos)
        Me.GroupBox1.Controls.Add(Me.lblstableYpos)
        Me.GroupBox1.Controls.Add(Me.lblstableXpos)
        Me.GroupBox1.Controls.Add(Me.Label32)
        Me.GroupBox1.Controls.Add(Me.Label31)
        Me.GroupBox1.Controls.Add(Me.Label27)
        Me.GroupBox1.Controls.Add(Me.cmbBonfire)
        Me.GroupBox1.Controls.Add(Me.lblBonfire)
        Me.GroupBox1.Controls.Add(Me.Label24)
        Me.GroupBox1.Controls.Add(Me.Label25)
        Me.GroupBox1.Controls.Add(Me.Label26)
        Me.GroupBox1.Controls.Add(Me.lblFacing)
        Me.GroupBox1.Controls.Add(Me.lblZpos)
        Me.GroupBox1.Controls.Add(Me.lblYpos)
        Me.GroupBox1.Controls.Add(Me.lblXpos)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 132)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(402, 306)
        Me.GroupBox1.TabIndex = 47
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Location"
        '
        'lblstableZpos
        '
        Me.lblstableZpos.Location = New System.Drawing.Point(230, 126)
        Me.lblstableZpos.Name = "lblstableZpos"
        Me.lblstableZpos.Size = New System.Drawing.Size(60, 23)
        Me.lblstableZpos.TabIndex = 51
        Me.lblstableZpos.Text = "(Z POS)"
        Me.lblstableZpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblstableYpos
        '
        Me.lblstableYpos.Location = New System.Drawing.Point(230, 101)
        Me.lblstableYpos.Name = "lblstableYpos"
        Me.lblstableYpos.Size = New System.Drawing.Size(60, 23)
        Me.lblstableYpos.TabIndex = 50
        Me.lblstableYpos.Text = "(Y POS)"
        Me.lblstableYpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblstableXpos
        '
        Me.lblstableXpos.Location = New System.Drawing.Point(230, 76)
        Me.lblstableXpos.Name = "lblstableXpos"
        Me.lblstableXpos.Size = New System.Drawing.Size(60, 23)
        Me.lblstableXpos.TabIndex = 49
        Me.lblstableXpos.Text = "(X POS)"
        Me.lblstableXpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(230, 44)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(60, 13)
        Me.Label32.TabIndex = 48
        Me.Label32.Text = "Last Stable"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(236, 57)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(47, 13)
        Me.Label31.TabIndex = 47
        Me.Label31.Text = "Position:"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(19, 55)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(47, 13)
        Me.Label27.TabIndex = 44
        Me.Label27.Text = "Position:"
        '
        'cmbBonfire
        '
        Me.cmbBonfire.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBonfire.FormattingEnabled = True
        Me.cmbBonfire.Location = New System.Drawing.Point(43, 260)
        Me.cmbBonfire.Name = "cmbBonfire"
        Me.cmbBonfire.Size = New System.Drawing.Size(322, 21)
        Me.cmbBonfire.TabIndex = 43
        '
        'lblBonfire
        '
        Me.lblBonfire.AutoSize = True
        Me.lblBonfire.Location = New System.Drawing.Point(19, 244)
        Me.lblBonfire.Name = "lblBonfire"
        Me.lblBonfire.Size = New System.Drawing.Size(66, 13)
        Me.lblBonfire.TabIndex = 44
        Me.lblBonfire.Text = "Last Bonfire:"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(23, 131)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(20, 13)
        Me.Label24.TabIndex = 43
        Me.Label24.Text = "Z: "
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(23, 106)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(20, 13)
        Me.Label25.TabIndex = 42
        Me.Label25.Text = "Y: "
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(23, 81)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(20, 13)
        Me.Label26.TabIndex = 41
        Me.Label26.Text = "X: "
        '
        'lblFacing
        '
        Me.lblFacing.AutoSize = True
        Me.lblFacing.Location = New System.Drawing.Point(19, 28)
        Me.lblFacing.Name = "lblFacing"
        Me.lblFacing.Size = New System.Drawing.Size(45, 13)
        Me.lblFacing.TabIndex = 40
        Me.lblFacing.Text = "Facing: "
        '
        'lblZpos
        '
        Me.lblZpos.Location = New System.Drawing.Point(97, 126)
        Me.lblZpos.Name = "lblZpos"
        Me.lblZpos.Size = New System.Drawing.Size(60, 23)
        Me.lblZpos.TabIndex = 39
        Me.lblZpos.Text = "(Z POS)"
        Me.lblZpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblYpos
        '
        Me.lblYpos.Location = New System.Drawing.Point(97, 101)
        Me.lblYpos.Name = "lblYpos"
        Me.lblYpos.Size = New System.Drawing.Size(60, 23)
        Me.lblYpos.TabIndex = 38
        Me.lblYpos.Text = "(Y POS)"
        Me.lblYpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblXpos
        '
        Me.lblXpos.Location = New System.Drawing.Point(97, 76)
        Me.lblXpos.Name = "lblXpos"
        Me.lblXpos.Size = New System.Drawing.Size(60, 23)
        Me.lblXpos.TabIndex = 37
        Me.lblXpos.Text = "(X POS)"
        Me.lblXpos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabStats
        '
        Me.tabStats.AutoScroll = True
        Me.tabStats.AutoScrollMargin = New System.Drawing.Size(8, 8)
        Me.tabStats.Controls.Add(Me.Label16)
        Me.tabStats.Controls.Add(Me.nmbClearCount)
        Me.tabStats.Controls.Add(Me.Label3)
        Me.tabStats.Controls.Add(Me.nmbMaxHP)
        Me.tabStats.Controls.Add(Me.Label7)
        Me.tabStats.Controls.Add(Me.nmbMaxStam)
        Me.tabStats.Controls.Add(Me.Label2)
        Me.tabStats.Controls.Add(Me.nmbGender)
        Me.tabStats.Controls.Add(Me.Label12)
        Me.tabStats.Controls.Add(Me.nmbVitality)
        Me.tabStats.Controls.Add(Me.Label13)
        Me.tabStats.Controls.Add(Me.nmbAttunement)
        Me.tabStats.Controls.Add(Me.Label14)
        Me.tabStats.Controls.Add(Me.nmbEnd)
        Me.tabStats.Controls.Add(Me.Label15)
        Me.tabStats.Controls.Add(Me.nmbStr)
        Me.tabStats.Controls.Add(Me.Label11)
        Me.tabStats.Controls.Add(Me.nmbDex)
        Me.tabStats.Controls.Add(Me.Label10)
        Me.tabStats.Controls.Add(Me.nmbResistance)
        Me.tabStats.Controls.Add(Me.Label9)
        Me.tabStats.Controls.Add(Me.nmbIntelligence)
        Me.tabStats.Controls.Add(Me.Label8)
        Me.tabStats.Controls.Add(Me.nmbFaith)
        Me.tabStats.Controls.Add(Me.Label1)
        Me.tabStats.Controls.Add(Me.nmbHumanity)
        Me.tabStats.Location = New System.Drawing.Point(4, 22)
        Me.tabStats.Name = "tabStats"
        Me.tabStats.Size = New System.Drawing.Size(512, 299)
        Me.tabStats.TabIndex = 2
        Me.tabStats.Text = "Stats"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(40, 294)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(52, 13)
        Me.Label16.TabIndex = 29
        Me.Label16.Text = "NG Level"
        '
        'nmbClearCount
        '
        Me.nmbClearCount.Location = New System.Drawing.Point(108, 292)
        Me.nmbClearCount.Maximum = New Decimal(New Integer() {1900, 0, 0, 0})
        Me.nmbClearCount.Name = "nmbClearCount"
        Me.nmbClearCount.Size = New System.Drawing.Size(65, 20)
        Me.nmbClearCount.TabIndex = 28
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(49, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Max HP"
        '
        'nmbMaxHP
        '
        Me.nmbMaxHP.Location = New System.Drawing.Point(108, 7)
        Me.nmbMaxHP.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.nmbMaxHP.Name = "nmbMaxHP"
        Me.nmbMaxHP.Size = New System.Drawing.Size(65, 20)
        Me.nmbMaxHP.TabIndex = 26
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(26, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Max Stamina"
        '
        'nmbMaxStam
        '
        Me.nmbMaxStam.Location = New System.Drawing.Point(108, 30)
        Me.nmbMaxStam.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.nmbMaxStam.Name = "nmbMaxStam"
        Me.nmbMaxStam.Size = New System.Drawing.Size(65, 20)
        Me.nmbMaxStam.TabIndex = 24
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 268)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "External Genitals"
        '
        'nmbGender
        '
        Me.nmbGender.Location = New System.Drawing.Point(108, 266)
        Me.nmbGender.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nmbGender.Name = "nmbGender"
        Me.nmbGender.Size = New System.Drawing.Size(65, 20)
        Me.nmbGender.TabIndex = 22
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(55, 58)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(37, 13)
        Me.Label12.TabIndex = 21
        Me.Label12.Text = "Vitality"
        '
        'nmbVitality
        '
        Me.nmbVitality.Location = New System.Drawing.Point(108, 56)
        Me.nmbVitality.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbVitality.Name = "nmbVitality"
        Me.nmbVitality.Size = New System.Drawing.Size(65, 20)
        Me.nmbVitality.TabIndex = 20
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(33, 81)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 13)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "Attunement"
        '
        'nmbAttunement
        '
        Me.nmbAttunement.Location = New System.Drawing.Point(108, 79)
        Me.nmbAttunement.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbAttunement.Name = "nmbAttunement"
        Me.nmbAttunement.Size = New System.Drawing.Size(65, 20)
        Me.nmbAttunement.TabIndex = 18
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(35, 104)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(59, 13)
        Me.Label14.TabIndex = 17
        Me.Label14.Text = "Endurance"
        '
        'nmbEnd
        '
        Me.nmbEnd.Location = New System.Drawing.Point(108, 102)
        Me.nmbEnd.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbEnd.Name = "nmbEnd"
        Me.nmbEnd.Size = New System.Drawing.Size(65, 20)
        Me.nmbEnd.TabIndex = 16
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(45, 127)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(47, 13)
        Me.Label15.TabIndex = 15
        Me.Label15.Text = "Strength"
        '
        'nmbStr
        '
        Me.nmbStr.Location = New System.Drawing.Point(108, 125)
        Me.nmbStr.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbStr.Name = "nmbStr"
        Me.nmbStr.Size = New System.Drawing.Size(65, 20)
        Me.nmbStr.TabIndex = 14
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(46, 150)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 13)
        Me.Label11.TabIndex = 13
        Me.Label11.Text = "Dexterity"
        '
        'nmbDex
        '
        Me.nmbDex.Location = New System.Drawing.Point(108, 148)
        Me.nmbDex.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbDex.Name = "nmbDex"
        Me.nmbDex.Size = New System.Drawing.Size(65, 20)
        Me.nmbDex.TabIndex = 12
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(32, 171)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Resistance"
        '
        'nmbResistance
        '
        Me.nmbResistance.Location = New System.Drawing.Point(108, 171)
        Me.nmbResistance.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbResistance.Name = "nmbResistance"
        Me.nmbResistance.Size = New System.Drawing.Size(65, 20)
        Me.nmbResistance.TabIndex = 10
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(33, 196)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Intelligence"
        '
        'nmbIntelligence
        '
        Me.nmbIntelligence.Location = New System.Drawing.Point(108, 194)
        Me.nmbIntelligence.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbIntelligence.Name = "nmbIntelligence"
        Me.nmbIntelligence.Size = New System.Drawing.Size(65, 20)
        Me.nmbIntelligence.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(62, 219)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Faith"
        '
        'nmbFaith
        '
        Me.nmbFaith.Location = New System.Drawing.Point(108, 217)
        Me.nmbFaith.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbFaith.Name = "nmbFaith"
        Me.nmbFaith.Size = New System.Drawing.Size(65, 20)
        Me.nmbFaith.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(41, 242)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Humanity"
        '
        'nmbHumanity
        '
        Me.nmbHumanity.Location = New System.Drawing.Point(108, 240)
        Me.nmbHumanity.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nmbHumanity.Name = "nmbHumanity"
        Me.nmbHumanity.Size = New System.Drawing.Size(65, 20)
        Me.nmbHumanity.TabIndex = 0
        '
        'tabTests
        '
        Me.tabTests.AutoScroll = True
        Me.tabTests.AutoScrollMargin = New System.Drawing.Size(8, 8)
        Me.tabTests.BackColor = System.Drawing.Color.Transparent
        Me.tabTests.Controls.Add(Me.toolstripTest)
        Me.tabTests.Location = New System.Drawing.Point(4, 22)
        Me.tabTests.Name = "tabTests"
        Me.tabTests.Size = New System.Drawing.Size(512, 306)
        Me.tabTests.TabIndex = 7
        Me.tabTests.Text = "Tests"
        '
        'toolstripTest
        '
        Me.toolstripTest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.toolstripTest.CanOverflow = False
        Me.toolstripTest.Dock = System.Windows.Forms.DockStyle.None
        Me.toolstripTest.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.toolstripTest.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1, Me.ToolStripSeparator1, Me.tsbtnDisableAI, Me.tsbtnEnableAI, Me.ToolStripSeparator2, Me.tsbtnEnablePlayerExterminate, Me.tsbtnDisablePlayerExterminate, Me.ToolStripSeparator3, Me.ToolStripButton1, Me.ToolStripSeparator4, Me.ToolStripButton2, Me.ToolStripButton3, Me.ToolStripLabel1})
        Me.toolstripTest.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.toolstripTest.Location = New System.Drawing.Point(2, 2)
        Me.toolstripTest.Name = "toolstripTest"
        Me.toolstripTest.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.toolstripTest.Size = New System.Drawing.Size(150, 221)
        Me.toolstripTest.TabIndex = 85
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiTestSomething})
        Me.ToolStripDropDownButton1.Enabled = False
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(148, 20)
        Me.ToolStripDropDownButton1.Text = "(Developer Test Stuff)"
        '
        'tsmiTestSomething
        '
        Me.tsmiTestSomething.Enabled = False
        Me.tsmiTestSomething.Name = "tsmiTestSomething"
        Me.tsmiTestSomething.Size = New System.Drawing.Size(146, 22)
        Me.tsmiTestSomething.Text = "Something...?"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(148, 6)
        '
        'tsbtnDisableAI
        '
        Me.tsbtnDisableAI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnDisableAI.Enabled = False
        Me.tsbtnDisableAI.Image = CType(resources.GetObject("tsbtnDisableAI.Image"), System.Drawing.Image)
        Me.tsbtnDisableAI.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDisableAI.Name = "tsbtnDisableAI"
        Me.tsbtnDisableAI.Size = New System.Drawing.Size(148, 19)
        Me.tsbtnDisableAI.Text = "Disable AI"
        '
        'tsbtnEnableAI
        '
        Me.tsbtnEnableAI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnEnableAI.Enabled = False
        Me.tsbtnEnableAI.Image = CType(resources.GetObject("tsbtnEnableAI.Image"), System.Drawing.Image)
        Me.tsbtnEnableAI.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnEnableAI.Name = "tsbtnEnableAI"
        Me.tsbtnEnableAI.Size = New System.Drawing.Size(148, 19)
        Me.tsbtnEnableAI.Text = "Enable AI"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(148, 6)
        '
        'tsbtnEnablePlayerExterminate
        '
        Me.tsbtnEnablePlayerExterminate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnEnablePlayerExterminate.Enabled = False
        Me.tsbtnEnablePlayerExterminate.Image = CType(resources.GetObject("tsbtnEnablePlayerExterminate.Image"), System.Drawing.Image)
        Me.tsbtnEnablePlayerExterminate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnEnablePlayerExterminate.Name = "tsbtnEnablePlayerExterminate"
        Me.tsbtnEnablePlayerExterminate.Size = New System.Drawing.Size(148, 19)
        Me.tsbtnEnablePlayerExterminate.Text = "Enable Player Exterminate"
        '
        'tsbtnDisablePlayerExterminate
        '
        Me.tsbtnDisablePlayerExterminate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnDisablePlayerExterminate.Enabled = False
        Me.tsbtnDisablePlayerExterminate.Image = CType(resources.GetObject("tsbtnDisablePlayerExterminate.Image"), System.Drawing.Image)
        Me.tsbtnDisablePlayerExterminate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnDisablePlayerExterminate.Name = "tsbtnDisablePlayerExterminate"
        Me.tsbtnDisablePlayerExterminate.Size = New System.Drawing.Size(148, 19)
        Me.tsbtnDisablePlayerExterminate.Text = "Disable Player Exterminate"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(148, 6)
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton1.Enabled = False
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(148, 19)
        Me.ToolStripButton1.Text = "UserVarPlayerFadeTest"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(148, 6)
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton2.Enabled = False
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(148, 19)
        Me.ToolStripButton2.Text = "BossRushEndingTest"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButton3.Enabled = False
        Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(148, 19)
        Me.ToolStripButton3.Text = "OandSandOandS_Debug"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Enabled = False
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(148, 15)
        Me.ToolStripLabel1.Text = "LUA TEST"
        '
        'tabNotes
        '
        Me.tabNotes.Controls.Add(Me.txtNotes)
        Me.tabNotes.Location = New System.Drawing.Point(4, 22)
        Me.tabNotes.Name = "tabNotes"
        Me.tabNotes.Size = New System.Drawing.Size(512, 299)
        Me.tabNotes.TabIndex = 8
        Me.tabNotes.Text = "Notes"
        Me.tabNotes.UseVisualStyleBackColor = True
        '
        'txtNotes
        '
        Me.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtNotes.Location = New System.Drawing.Point(0, 0)
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.ReadOnly = True
        Me.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNotes.Size = New System.Drawing.Size(512, 299)
        Me.txtNotes.TabIndex = 0
        '
        'tabAbout
        '
        Me.tabAbout.AutoScroll = True
        Me.tabAbout.AutoScrollMargin = New System.Drawing.Size(0, 8)
        Me.tabAbout.Controls.Add(Me.AboutPage1)
        Me.tabAbout.Location = New System.Drawing.Point(4, 22)
        Me.tabAbout.Name = "tabAbout"
        Me.tabAbout.Size = New System.Drawing.Size(512, 306)
        Me.tabAbout.TabIndex = 6
        Me.tabAbout.Text = "About"
        Me.tabAbout.UseVisualStyleBackColor = True
        '
        'AboutPage1
        '
        Me.AboutPage1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.AboutPage1.Location = New System.Drawing.Point(3, 23)
        Me.AboutPage1.Name = "AboutPage1"
        Me.AboutPage1.Size = New System.Drawing.Size(506, 181)
        Me.AboutPage1.TabIndex = 0
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(12, 11)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(64, 13)
        Me.Label22.TabIndex = 6
        Me.Label22.Text = "Hooked To:"
        '
        'lblRelease
        '
        Me.lblRelease.AutoSize = True
        Me.lblRelease.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRelease.Location = New System.Drawing.Point(77, 11)
        Me.lblRelease.Name = "lblRelease"
        Me.lblRelease.Size = New System.Drawing.Size(51, 13)
        Me.lblRelease.TabIndex = 5
        Me.lblRelease.Text = "Nothing"
        '
        'btnReconnect
        '
        Me.btnReconnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReconnect.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnReconnect.Location = New System.Drawing.Point(431, 5)
        Me.btnReconnect.Name = "btnReconnect"
        Me.btnReconnect.Size = New System.Drawing.Size(81, 23)
        Me.btnReconnect.TabIndex = 0
        Me.btnReconnect.TabStop = False
        Me.btnReconnect.Text = "Reconnect"
        Me.btnReconnect.UseVisualStyleBackColor = False
        '
        'lblVer
        '
        Me.lblVer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblVer.AutoSize = True
        Me.lblVer.Location = New System.Drawing.Point(51, 368)
        Me.lblVer.Name = "lblVer"
        Me.lblVer.Size = New System.Drawing.Size(76, 13)
        Me.lblVer.TabIndex = 77
        Me.lblVer.Text = "2017-08-24-12"
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(4, 367)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(45, 13)
        Me.Label18.TabIndex = 79
        Me.Label18.Text = "Version:"
        '
        'btnNewConsole
        '
        Me.btnNewConsole.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewConsole.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnNewConsole.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewConsole.Image = CType(resources.GetObject("btnNewConsole.Image"), System.Drawing.Image)
        Me.btnNewConsole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewConsole.Location = New System.Drawing.Point(331, 362)
        Me.btnNewConsole.Margin = New System.Windows.Forms.Padding(0)
        Me.btnNewConsole.Name = "btnNewConsole"
        Me.btnNewConsole.Size = New System.Drawing.Size(153, 23)
        Me.btnNewConsole.TabIndex = 80
        Me.btnNewConsole.TabStop = False
        Me.btnNewConsole.Text = "New Scripting Window"
        Me.btnNewConsole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnNewConsole.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnUpdate.Location = New System.Drawing.Point(258, 5)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(0)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(167, 23)
        Me.btnUpdate.TabIndex = 0
        Me.btnUpdate.TabStop = False
        Me.btnUpdate.Text = "New BRush Version Available"
        Me.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnUpdate.UseVisualStyleBackColor = True
        Me.btnUpdate.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(303, 260)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(32, 13)
        Me.Label17.TabIndex = 96
        Me.Label17.Text = "NG+:"
        '
        'numBossScenarioNg
        '
        Me.numBossScenarioNg.Location = New System.Drawing.Point(337, 257)
        Me.numBossScenarioNg.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.numBossScenarioNg.Name = "numBossScenarioNg"
        Me.numBossScenarioNg.Size = New System.Drawing.Size(32, 20)
        Me.numBossScenarioNg.TabIndex = 97
        '
        'frmForm1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 390)
        Me.Controls.Add(Me.btnNewConsole)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.lblVer)
        Me.Controls.Add(Me.btnReconnect)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.lblRelease)
        Me.Controls.Add(Me.tabs)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(540, 360)
        Me.Name = "frmForm1"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Boss-Rush! With Perma-death! ...Regular-Non-Mega-Pre-Alpha"
        Me.tabs.ResumeLayout(False)
        Me.tabBosses.ResumeLayout(False)
        Me.tabBosses.PerformLayout()
        CType(Me.numCountdown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabScenarios.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabStats.ResumeLayout(False)
        Me.tabStats.PerformLayout()
        CType(Me.nmbClearCount, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbMaxHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbMaxStam, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbGender, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbVitality, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbAttunement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbStr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbDex, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbResistance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbIntelligence, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbFaith, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nmbHumanity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabTests.ResumeLayout(False)
        Me.tabTests.PerformLayout()
        Me.toolstripTest.ResumeLayout(False)
        Me.toolstripTest.PerformLayout()
        Me.tabNotes.ResumeLayout(False)
        Me.tabNotes.PerformLayout()
        Me.tabAbout.ResumeLayout(False)
        CType(Me.numBossScenarioNg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub

    Friend WithEvents tabs As TabControl
    Friend WithEvents tabMain As TabPage
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblHP As Label
    Friend WithEvents lblStam As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblstableZpos As Label
    Friend WithEvents lblstableYpos As Label
    Friend WithEvents lblstableXpos As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label31 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents cmbBonfire As ComboBox
    Friend WithEvents lblBonfire As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents lblFacing As Label
    Friend WithEvents lblZpos As Label
    Friend WithEvents lblYpos As Label
    Friend WithEvents lblXpos As Label
    Friend WithEvents tabStats As TabPage
    Friend WithEvents Label12 As Label
    Friend WithEvents nmbVitality As NumericUpDown
    Friend WithEvents Label13 As Label
    Friend WithEvents nmbAttunement As NumericUpDown
    Friend WithEvents Label14 As Label
    Friend WithEvents nmbEnd As NumericUpDown
    Friend WithEvents Label15 As Label
    Friend WithEvents nmbStr As NumericUpDown
    Friend WithEvents Label11 As Label
    Friend WithEvents nmbDex As NumericUpDown
    Friend WithEvents Label10 As Label
    Friend WithEvents nmbResistance As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents nmbIntelligence As NumericUpDown
    Friend WithEvents Label8 As Label
    Friend WithEvents nmbFaith As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents nmbHumanity As NumericUpDown
    Friend WithEvents Label22 As Label
    Friend WithEvents lblRelease As Label
    Friend WithEvents tabBosses As TabPage
    Friend WithEvents lblPlaytime As Label
    Friend WithEvents tabAbout As TabPage
    Friend WithEvents btnReconnect As Button
    Friend WithEvents tabTests As TabPage
    Friend WithEvents btnBeginBossRush As Button
    Friend WithEvents lblVer As Label
    Friend WithEvents btnUpdate As Button
    Friend WithEvents tabNotes As TabPage
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents nmbGender As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents nmbMaxHP As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents nmbMaxStam As NumericUpDown
    Friend WithEvents tabScenarios As TabPage
    Friend WithEvents btnScenarioTripleSanctuary As Button
    Friend WithEvents btnScenarioArtoriasCiaran As Button
    Friend WithEvents Label16 As Label
    Friend WithEvents nmbClearCount As NumericUpDown
    Friend WithEvents btnScenarioPinwheelDefense As Button
    Friend WithEvents toolstripTest As ToolStrip
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents tsmiTestSomething As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsbtnDisableAI As ToolStripButton
    Friend WithEvents tsbtnEnableAI As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents tsbtnEnablePlayerExterminate As ToolStripButton
    Friend WithEvents tsbtnDisablePlayerExterminate As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents Label18 As Label
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents btnScenarioOandSandOandS As Button
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents ToolStripButton3 As ToolStripButton
    Friend WithEvents btnCancelBossRush As Button
    Friend WithEvents btnNewConsole As Button
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents AboutPage1 As AboutPage
    Friend WithEvents btnLoadBossScenario As Button
    Friend WithEvents comboBossList As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents radioCustom As RadioButton
    Friend WithEvents radioRandom As RadioButton
    Friend WithEvents radioReverse As RadioButton
    Friend WithEvents radioStandard As RadioButton
    Friend WithEvents checkSkipBedOfChaos As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents checkRandomizeNg As CheckBox
    Friend WithEvents checkHealEachFight As CheckBox
    Friend WithEvents checkInfiniteLives As CheckBox
    Friend WithEvents checkHideBossNames As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents numCountdown As NumericUpDown
    Friend WithEvents btnFlpCustomRemove As Button
    Friend WithEvents btnFlpCustomAdd As Button
    Friend WithEvents tlpCustomOrder As TableLayoutPanel
    Friend WithEvents numBossScenarioNg As NumericUpDown
    Friend WithEvents Label17 As Label
End Class