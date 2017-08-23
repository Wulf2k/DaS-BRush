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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConsoleWindow))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsHookedToPreText = New System.Windows.Forms.ToolStripLabel()
        Me.scConsole = New ScintillaNET.Scintilla()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtConsoleResult = New System.Windows.Forms.TextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tsHook = New System.Windows.Forms.ToolStripLabel()
        Me.tsbtnNew = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnOpen = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnSave = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnRun = New System.Windows.Forms.ToolStripButton()
        Me.tsbtnStop = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.tsNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(862, 24)
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
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(120, 6)
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
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbtnNew, Me.tsbtnOpen, Me.tsbtnSave, Me.ToolStripSeparator1, Me.tsbtnRun, Me.tsbtnStop, Me.ToolStripSeparator5, Me.ToolStripButton1, Me.tsHook, Me.tsHookedToPreText})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(862, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'tsHookedToPreText
        '
        Me.tsHookedToPreText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsHookedToPreText.Name = "tsHookedToPreText"
        Me.tsHookedToPreText.Size = New System.Drawing.Size(66, 22)
        Me.tsHookedToPreText.Text = "Hooked to:"
        '
        'scConsole
        '
        Me.scConsole.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scConsole.AutoCCancelAtStart = False
        Me.scConsole.AutoCIgnoreCase = True
        Me.scConsole.AutoCOrder = ScintillaNET.Order.PerformSort
        Me.scConsole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.scConsole.CaretLineBackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(227, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.scConsole.CaretLineVisible = True
        Me.scConsole.HScrollBar = False
        Me.scConsole.IdleStyling = ScintillaNET.IdleStyling.All
        Me.scConsole.Lexer = ScintillaNET.Lexer.PureBasic
        Me.scConsole.Location = New System.Drawing.Point(-1, 3)
        Me.scConsole.Name = "scConsole"
        Me.scConsole.PhasesDraw = ScintillaNET.Phases.Multiple
        Me.scConsole.Size = New System.Drawing.Size(864, 269)
        Me.scConsole.TabIndex = 10
        Me.scConsole.TabStop = False
        Me.scConsole.Technology = ScintillaNET.Technology.DirectWrite
        Me.scConsole.WrapMode = ScintillaNET.WrapMode.Word
        Me.scConsole.WrapVisualFlagLocation = ScintillaNET.WrapVisualFlagLocation.StartByText
        Me.scConsole.WrapVisualFlags = ScintillaNET.WrapVisualFlags.Margin
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(3, 4)
        Me.Label17.Margin = New System.Windows.Forms.Padding(0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(63, 13)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "Last Result:"
        '
        'txtConsoleResult
        '
        Me.txtConsoleResult.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtConsoleResult.Location = New System.Drawing.Point(1, 20)
        Me.txtConsoleResult.Multiline = True
        Me.txtConsoleResult.Name = "txtConsoleResult"
        Me.txtConsoleResult.ReadOnly = True
        Me.txtConsoleResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtConsoleResult.Size = New System.Drawing.Size(860, 97)
        Me.txtConsoleResult.TabIndex = 7
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 49)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.scConsole)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtConsoleResult)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label17)
        Me.SplitContainer1.Panel2MinSize = 85
        Me.SplitContainer1.Size = New System.Drawing.Size(862, 394)
        Me.SplitContainer1.SplitterDistance = 271
        Me.SplitContainer1.TabIndex = 11
        Me.SplitContainer1.TabStop = False
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
        'tsbtnNew
        '
        Me.tsbtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnNew.Image = Global.DaS_BRush.My.Resources.Resources.NewIcon
        Me.tsbtnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnNew.Name = "tsbtnNew"
        Me.tsbtnNew.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnNew.Text = "ToolStripButton1"
        '
        'tsbtnOpen
        '
        Me.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnOpen.Image = Global.DaS_BRush.My.Resources.Resources.OpenIcon
        Me.tsbtnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnOpen.Name = "tsbtnOpen"
        Me.tsbtnOpen.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnOpen.Text = "ToolStripButton2"
        '
        'tsbtnSave
        '
        Me.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnSave.Image = Global.DaS_BRush.My.Resources.Resources.Save
        Me.tsbtnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnSave.Name = "tsbtnSave"
        Me.tsbtnSave.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnSave.Text = "ToolStripButton1"
        '
        'tsbtnRun
        '
        Me.tsbtnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnRun.Image = CType(resources.GetObject("tsbtnRun.Image"), System.Drawing.Image)
        Me.tsbtnRun.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnRun.Name = "tsbtnRun"
        Me.tsbtnRun.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnRun.Text = "Run"
        '
        'tsbtnStop
        '
        Me.tsbtnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbtnStop.Image = CType(resources.GetObject("tsbtnStop.Image"), System.Drawing.Image)
        Me.tsbtnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbtnStop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbtnStop.Name = "tsbtnStop"
        Me.tsbtnStop.Size = New System.Drawing.Size(23, 22)
        Me.tsbtnStop.Text = "Stop"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "Retry hooking to Dark Souls"
        '
        'tsNew
        '
        Me.tsNew.Image = Global.DaS_BRush.My.Resources.Resources.NewIcon
        Me.tsNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsNew.Name = "tsNew"
        Me.tsNew.Size = New System.Drawing.Size(123, 22)
        Me.tsNew.Text = "New"
        '
        'tsOpen
        '
        Me.tsOpen.Image = Global.DaS_BRush.My.Resources.Resources.OpenIcon
        Me.tsOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsOpen.Name = "tsOpen"
        Me.tsOpen.Size = New System.Drawing.Size(123, 22)
        Me.tsOpen.Text = "Open..."
        '
        'tsSave
        '
        Me.tsSave.Image = Global.DaS_BRush.My.Resources.Resources.Save
        Me.tsSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsSave.Name = "tsSave"
        Me.tsSave.Size = New System.Drawing.Size(123, 22)
        Me.tsSave.Text = "Save"
        '
        'ConsoleWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(862, 444)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.DoubleBuffered = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(512, 448)
        Me.Name = "ConsoleWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Untitled - Dark Souls Script Console"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents scConsole As ScintillaNET.Scintilla
    Friend WithEvents Label17 As Label
    Friend WithEvents txtConsoleResult As TextBox
    Friend WithEvents SplitContainer1 As SplitContainer
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
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents tsHook As ToolStripLabel
End Class
