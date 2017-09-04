<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ConsoleWindow
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConsoleWindow))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiShowOutput = New System.Windows.Forms.ToolStripMenuItem()
        Me.OutputPanelPositionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputPosTop = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputPosBottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputPosLeft = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputPosRight = New System.Windows.Forms.ToolStripMenuItem()
        Me.OutputTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputWordWrap = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiOutputAutoScroll = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbtnNew = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnOpen = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnRun = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnStop = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbtnHook = New System.Windows.Forms.ToolStripButton()
        Me.tsHook = New System.Windows.Forms.ToolStripLabel()
        Me.tsHookedToPreText = New System.Windows.Forms.ToolStripLabel()
        Me.cwTabs = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.contextMenuTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseTabToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewTabToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClloseAllOtherTabsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllTabsToTheRightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.splitter = New System.Windows.Forms.SplitContainer()
        Me.btnHideOutput = New System.Windows.Forms.Button()
        Me.sobOutput = New DaS.ScriptEditor.ScriptOutputBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.statusStrip = New System.Windows.Forms.ToolStrip()
        Me.tslblStatus = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnOutputClear = New System.Windows.Forms.ToolStripButton()
        Me.tslblOutputLineCount = New System.Windows.Forms.ToolStripLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.cwTabs.SuspendLayout()
        Me.contextMenuTab.SuspendLayout()
        CType(Me.splitter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitter.Panel1.SuspendLayout()
        Me.splitter.Panel2.SuspendLayout()
        Me.splitter.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.statusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1153, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsNew, Me.tsOpen, Me.ToolStripSeparator4, Me.tsSave, Me.tsSaveAs, Me.ToolStripSeparator3, Me.tsExit})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'tsNew
        '
        Me.tsNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsNew.Name = "tsNew"
        Me.tsNew.Size = New System.Drawing.Size(123, 22)
        Me.tsNew.Text = "New"
        '
        'tsOpen
        '
        Me.tsOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsOpen.Name = "tsOpen"
        Me.tsOpen.Size = New System.Drawing.Size(123, 22)
        Me.tsOpen.Text = "Open..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(120, 6)
        '
        'tsSave
        '
        Me.tsSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsSave.Name = "tsSave"
        Me.tsSave.Size = New System.Drawing.Size(123, 22)
        Me.tsSave.Text = "Save"
        '
        'tsSaveAs
        '
        Me.tsSaveAs.Name = "tsSaveAs"
        Me.tsSaveAs.Size = New System.Drawing.Size(123, 22)
        Me.tsSaveAs.Text = "Save As..."
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(120, 6)
        '
        'tsExit
        '
        Me.tsExit.Name = "tsExit"
        Me.tsExit.Size = New System.Drawing.Size(123, 22)
        Me.tsExit.Text = "Exit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiShowOutput, Me.OutputPanelPositionToolStripMenuItem, Me.OutputTextToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'tsmiShowOutput
        '
        Me.tsmiShowOutput.Checked = True
        Me.tsmiShowOutput.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsmiShowOutput.Name = "tsmiShowOutput"
        Me.tsmiShowOutput.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.tsmiShowOutput.Size = New System.Drawing.Size(195, 22)
        Me.tsmiShowOutput.Text = "Show Output Panel"
        '
        'OutputPanelPositionToolStripMenuItem
        '
        Me.OutputPanelPositionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOutputPosTop, Me.tsmiOutputPosBottom, Me.tsmiOutputPosLeft, Me.tsmiOutputPosRight})
        Me.OutputPanelPositionToolStripMenuItem.Name = "OutputPanelPositionToolStripMenuItem"
        Me.OutputPanelPositionToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.OutputPanelPositionToolStripMenuItem.Text = "Output Panel Position"
        '
        'tsmiOutputPosTop
        '
        Me.tsmiOutputPosTop.Name = "tsmiOutputPosTop"
        Me.tsmiOutputPosTop.Size = New System.Drawing.Size(114, 22)
        Me.tsmiOutputPosTop.Text = "Top"
        '
        'tsmiOutputPosBottom
        '
        Me.tsmiOutputPosBottom.Name = "tsmiOutputPosBottom"
        Me.tsmiOutputPosBottom.Size = New System.Drawing.Size(114, 22)
        Me.tsmiOutputPosBottom.Text = "Bottom"
        '
        'tsmiOutputPosLeft
        '
        Me.tsmiOutputPosLeft.Name = "tsmiOutputPosLeft"
        Me.tsmiOutputPosLeft.Size = New System.Drawing.Size(114, 22)
        Me.tsmiOutputPosLeft.Text = "Left"
        '
        'tsmiOutputPosRight
        '
        Me.tsmiOutputPosRight.Checked = True
        Me.tsmiOutputPosRight.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsmiOutputPosRight.Name = "tsmiOutputPosRight"
        Me.tsmiOutputPosRight.Size = New System.Drawing.Size(114, 22)
        Me.tsmiOutputPosRight.Text = "Right"
        '
        'OutputTextToolStripMenuItem
        '
        Me.OutputTextToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiOutputWordWrap, Me.tsmiOutputAutoScroll})
        Me.OutputTextToolStripMenuItem.Name = "OutputTextToolStripMenuItem"
        Me.OutputTextToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.OutputTextToolStripMenuItem.Text = "Output Panel"
        '
        'tsmiOutputWordWrap
        '
        Me.tsmiOutputWordWrap.Name = "tsmiOutputWordWrap"
        Me.tsmiOutputWordWrap.Size = New System.Drawing.Size(134, 22)
        Me.tsmiOutputWordWrap.Text = "Word Wrap"
        '
        'tsmiOutputAutoScroll
        '
        Me.tsmiOutputAutoScroll.Checked = True
        Me.tsmiOutputAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsmiOutputAutoScroll.Name = "tsmiOutputAutoScroll"
        Me.tsmiOutputAutoScroll.Size = New System.Drawing.Size(134, 22)
        Me.tsmiOutputAutoScroll.Text = "Auto-Scroll"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiHelp, Me.ToolStripSeparator2, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'tsmiHelp
        '
        Me.tsmiHelp.Name = "tsmiHelp"
        Me.tsmiHelp.Size = New System.Drawing.Size(224, 22)
        Me.tsmiHelp.Text = "Wulf2k's Function Reference"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(221, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNew, Me.tsbtnOpen, Me.tsbtnSave, Me.ToolStripSeparator1, Me.tsbtnRun, Me.tsbtnStop, Me.ToolStripSeparator5, Me.tsbtnHook, Me.tsHook, Me.tsHookedToPreText})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(1153, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbtnNew
        '
        Me.tsbtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNew.Name = "tsbtnNew"
        Me.tsbtnNew.Size = New System.Drawing.Size(45, 22)
        Me.tsbtnNew.Text = "(NEW)"
        Me.tsbtnNew.ToolTipText = "New Script (Ctrl+N)"
        '
        'tsbtnOpen
        '
        Me.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnOpen.Name = "tsbtnOpen"
        Me.tsbtnOpen.Size = New System.Drawing.Size(50, 22)
        Me.tsbtnOpen.Text = "(OPEN)"
        Me.tsbtnOpen.ToolTipText = "Open... (Ctrl+O)"
        '
        'tsbtnSave
        '
        Me.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSave.Name = "tsbtnSave"
        Me.tsbtnSave.Size = New System.Drawing.Size(45, 22)
        Me.tsbtnSave.Text = "(SAVE)"
        Me.tsbtnSave.ToolTipText = "Save (Ctrl+S)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtnRun
        '
        Me.tsbtnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnRun.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnRun.Name = "tsbtnRun"
        Me.tsbtnRun.Size = New System.Drawing.Size(52, 22)
        Me.tsbtnRun.Text = "(START)"
        Me.tsbtnRun.ToolTipText = "Run (F5 while stopped)"
        '
        'tsbtnStop
        '
        Me.tsbtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnStop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnStop.Name = "tsbtnStop"
        Me.tsbtnStop.Size = New System.Drawing.Size(47, 22)
        Me.tsbtnStop.Text = "(STOP)"
        Me.tsbtnStop.ToolTipText = "Stop (F5 while running)"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'tsbtnHook
        '
        Me.tsbtnHook.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnHook.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbtnHook.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnHook.Name = "tsbtnHook"
        Me.tsbtnHook.Size = New System.Drawing.Size(50, 22)
        Me.tsbtnHook.Text = "(SCAN)"
        Me.tsbtnHook.ToolTipText = "Retry hooking to Dark Souls (F7)"
        '
        'tsHook
        '
        Me.tsHook.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsHook.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.tsHook.Name = "tsHook"
        Me.tsHook.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.tsHook.Size = New System.Drawing.Size(52, 22)
        Me.tsHook.Text = "Nothing"
        '
        'tsHookedToPreText
        '
        Me.tsHookedToPreText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsHookedToPreText.Name = "tsHookedToPreText"
        Me.tsHookedToPreText.Size = New System.Drawing.Size(66, 22)
        Me.tsHookedToPreText.Text = "Hooked to:"
        '
        'cwTabs
        '
        Me.cwTabs.AllowDrop = True
        Me.cwTabs.Controls.Add(Me.TabPage1)
        Me.cwTabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cwTabs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cwTabs.Location = New System.Drawing.Point(0, 0)
        Me.cwTabs.Name = "cwTabs"
        Me.cwTabs.SelectedIndex = 0
        Me.cwTabs.Size = New System.Drawing.Size(568, 534)
        Me.cwTabs.TabIndex = 2
        Me.cwTabs.TabStop = False
        '
        'TabPage1
        '
        Me.TabPage1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(560, 508)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'contextMenuTab
        '
        Me.contextMenuTab.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseTabToolStripMenuItem, Me.NewTabToolStripMenuItem, Me.ClloseAllOtherTabsToolStripMenuItem, Me.CloseAllTabsToTheRightToolStripMenuItem})
        Me.contextMenuTab.Name = "contextMenuTab"
        Me.contextMenuTab.Size = New System.Drawing.Size(206, 92)
        '
        'CloseTabToolStripMenuItem
        '
        Me.CloseTabToolStripMenuItem.Name = "CloseTabToolStripMenuItem"
        Me.CloseTabToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+W"
        Me.CloseTabToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.CloseTabToolStripMenuItem.Text = "Close Tab"
        '
        'NewTabToolStripMenuItem
        '
        Me.NewTabToolStripMenuItem.Name = "NewTabToolStripMenuItem"
        Me.NewTabToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+T"
        Me.NewTabToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.NewTabToolStripMenuItem.Text = "New Tab"
        '
        'ClloseAllOtherTabsToolStripMenuItem
        '
        Me.ClloseAllOtherTabsToolStripMenuItem.Name = "ClloseAllOtherTabsToolStripMenuItem"
        Me.ClloseAllOtherTabsToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.ClloseAllOtherTabsToolStripMenuItem.Text = "Close other tabs"
        '
        'CloseAllTabsToTheRightToolStripMenuItem
        '
        Me.CloseAllTabsToTheRightToolStripMenuItem.Name = "CloseAllTabsToTheRightToolStripMenuItem"
        Me.CloseAllTabsToTheRightToolStripMenuItem.Size = New System.Drawing.Size(205, 22)
        Me.CloseAllTabsToTheRightToolStripMenuItem.Text = "Close all tabs to the right"
        '
        'splitter
        '
        Me.splitter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.splitter.Location = New System.Drawing.Point(1, 47)
        Me.splitter.Name = "splitter"
        '
        'splitter.Panel1
        '
        Me.splitter.Panel1.Controls.Add(Me.cwTabs)
        Me.splitter.Panel1MinSize = 256
        '
        'splitter.Panel2
        '
        Me.splitter.Panel2.Controls.Add(Me.btnHideOutput)
        Me.splitter.Panel2.Controls.Add(Me.sobOutput)
        Me.splitter.Panel2.Controls.Add(Me.ToolStrip2)
        Me.splitter.Panel2MinSize = 256
        Me.splitter.Size = New System.Drawing.Size(1151, 534)
        Me.splitter.SplitterDistance = 568
        Me.splitter.TabIndex = 3
        Me.splitter.TabStop = False
        '
        'btnHideOutput
        '
        Me.btnHideOutput.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHideOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnHideOutput.Font = New System.Drawing.Font("Consolas", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHideOutput.Location = New System.Drawing.Point(555, 3)
        Me.btnHideOutput.Margin = New System.Windows.Forms.Padding(0)
        Me.btnHideOutput.Name = "btnHideOutput"
        Me.btnHideOutput.Size = New System.Drawing.Size(18, 18)
        Me.btnHideOutput.TabIndex = 2
        Me.btnHideOutput.TabStop = False
        Me.btnHideOutput.Text = "X"
        Me.btnHideOutput.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnHideOutput.UseVisualStyleBackColor = True
        '
        'sobOutput
        '
        Me.sobOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sobOutput.AutoScroll = True
        Me.sobOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.sobOutput.Font = New System.Drawing.Font("Segoe UI Light", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sobOutput.Location = New System.Drawing.Point(3, 22)
        Me.sobOutput.Name = "sobOutput"
        Me.sobOutput.ReadOnly = True
        Me.sobOutput.ShortcutsEnabled = False
        Me.sobOutput.Size = New System.Drawing.Size(571, 508)
        Me.sobOutput.TabIndex = 0
        Me.sobOutput.TabStop = False
        Me.sobOutput.Text = "Sup Wulf. Tell me how the toolstrip that says ""Console Output:"" immediately above" &
    " looks on your system while the program is running (it has to load the 'X' butto" &
    "n on launch)"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ToolStrip2.AutoSize = False
        Me.ToolStrip2.CanOverflow = False
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Padding = New System.Windows.Forms.Padding(2)
        Me.ToolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip2.Size = New System.Drawing.Size(580, 27)
        Me.ToolStrip2.TabIndex = 1
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(96, 20)
        Me.ToolStripLabel1.Text = "Console Output:"
        Me.ToolStripLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'statusStrip
        '
        Me.statusStrip.AllowMerge = False
        Me.statusStrip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.statusStrip.AutoSize = False
        Me.statusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.statusStrip.GripMargin = New System.Windows.Forms.Padding(0)
        Me.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslblStatus, Me.tsbtnOutputClear, Me.tslblOutputLineCount})
        Me.statusStrip.Location = New System.Drawing.Point(1, 580)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.statusStrip.Size = New System.Drawing.Size(1134, 22)
        Me.statusStrip.TabIndex = 4
        Me.statusStrip.Text = "ToolStrip3"
        '
        'tslblStatus
        '
        Me.tslblStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.tslblStatus.Name = "tslblStatus"
        Me.tslblStatus.Size = New System.Drawing.Size(199, 19)
        Me.tslblStatus.Text = "Welcome to Dark Souls Script Editor."
        '
        'tsbtnOutputClear
        '
        Me.tsbtnOutputClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbtnOutputClear.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.tsbtnOutputClear.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.tsbtnOutputClear.Image = CType(resources.GetObject("tsbtnOutputClear.Image"), System.Drawing.Image)
        Me.tsbtnOutputClear.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnOutputClear.Name = "tsbtnOutputClear"
        Me.tsbtnOutputClear.Size = New System.Drawing.Size(63, 19)
        Me.tsbtnOutputClear.Text = "(Clear)"
        Me.tsbtnOutputClear.ToolTipText = "Clear Output Panel Text"
        '
        'tslblOutputLineCount
        '
        Me.tslblOutputLineCount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tslblOutputLineCount.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!)
        Me.tslblOutputLineCount.Name = "tslblOutputLineCount"
        Me.tslblOutputLineCount.Size = New System.Drawing.Size(88, 19)
        Me.tslblOutputLineCount.Text = "Output Lines: 0"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 579)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1153, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ConsoleWindow
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1153, 601)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.splitter)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(512, 448)
        Me.Name = "ConsoleWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Untitled.lua - Dark Souls Script Editor"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.cwTabs.ResumeLayout(False)
        Me.contextMenuTab.ResumeLayout(False)
        Me.splitter.Panel1.ResumeLayout(False)
        Me.splitter.Panel2.ResumeLayout(False)
        CType(Me.splitter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitter.ResumeLayout(False)
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tsbtnRun As ToolStripButton
    Friend WithEvents tsbtnStop As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsbtnSave As ToolStripButton
    Friend WithEvents tsbtnNew As ToolStripButton
    Friend WithEvents tsbtnOpen As ToolStripButton
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiHelp As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsNew As ToolStripMenuItem
    Friend WithEvents tsOpen As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents tsSave As ToolStripMenuItem
    Friend WithEvents tsSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents tsExit As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents tsHookedToPreText As ToolStripLabel
    Friend WithEvents tsbtnHook As ToolStripButton
    Friend WithEvents tsHook As ToolStripLabel
    Friend WithEvents cwTabs As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents contextMenuTab As ContextMenuStrip
    Friend WithEvents CloseTabToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewTabToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClloseAllOtherTabsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllTabsToTheRightToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents splitter As SplitContainer
    Friend WithEvents sobOutput As ScriptOutputBox
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents ToolStripLabel1 As ToolStripLabel
    Friend WithEvents btnHideOutput As Button
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiShowOutput As ToolStripMenuItem
    Friend WithEvents OutputPanelPositionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiOutputPosTop As ToolStripMenuItem
    Friend WithEvents tsmiOutputPosBottom As ToolStripMenuItem
    Friend WithEvents tsmiOutputPosLeft As ToolStripMenuItem
    Friend WithEvents tsmiOutputPosRight As ToolStripMenuItem
    Friend WithEvents statusStrip As ToolStrip
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents tslblStatus As ToolStripLabel
    Friend WithEvents tslblOutputLineCount As ToolStripLabel
    Friend WithEvents tsbtnOutputClear As ToolStripButton
    Friend WithEvents OutputTextToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmiOutputWordWrap As ToolStripMenuItem
    Friend WithEvents tsmiOutputAutoScroll As ToolStripMenuItem
End Class
