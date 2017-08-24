Imports System.IO
Imports System.Reflection
Imports DaS_Scripting

Public Class ConsoleWindow

    Private guiRefreshTimer As New Timer() With {.Interval = 33, .Enabled = True}

    Private _EXITING As Boolean = False

    Private Shared ReadOnly Property ThisAssembly As Assembly
    Private ReadOnly EmbeddedResourceNames As String()

    Public Shared EmbeddedImage As New Dictionary(Of String, Image)

    Private currentContextTab As Integer = -1

    Private boldTab As New List(Of Boolean)()

    Private Sub ConsoleWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Game.InitHook()
        AddTab()
        AddHandler guiRefreshTimer.Tick, AddressOf GuiRefreshTimerTick

        'GetType(TabControl).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, cwTabs, New Object() {True})
    End Sub

    ' This cool override here forces EVERY CONTROL IN THE FORM to be double buffered NO MATTER WHAT.
    ' And you can even do the inverse of this override in any child controls that you don't want to be double buffered.
    ' Any other stuff in this class that refers to _originalExStyle or _doubleBufferino are part of the same hack.
    ' This hack is from this guy on a now-shut-down blog website.
    ' Here's an archived version of the article page:
    ' https://web.archive.org/web/20161204003214/http://www.angryhacker.com/blog/archive/2010/07/21/how-to-get-rid-of-flicker-on-windows-forms-applications.aspx
    '
    ' Anyways, THE REASON I DISABLED IT: Literally the only problem is that the scintilla control takes so goddamn long to initialize
    ' that you're forced to look at all of the controls being pitch black squares for like 2 seconds and it looks ugly.
    ' (on normal programs that start instantly you usually don't notice the black controls for like 1 frame)
    ' 
    '
    '
    'Dim _originalExStyle = -1
    'Dim _doubleBufferino As Boolean = True
    'Protected Overrides ReadOnly Property CreateParams As CreateParams
    '    Get
    '        If _originalExStyle = -1 Then
    '            _originalExStyle = MyBase.CreateParams.ExStyle
    '        End If

    '        Dim handleParam = MyBase.CreateParams

    '        If _doubleBufferino Then
    '            handleParam.ExStyle = handleParam.ExStyle Or &H2000000
    '        Else
    '            handleParam.ExStyle = _originalExStyle
    '        End If

    '        Return handleParam
    '    End Get
    'End Property
    ' 
    ' This method must be called in the form's "Shown" event:
    '
    'Private Sub _turnOffDoubleBufferino()
    '    _doubleBufferino = False
    '    Me.MaximizeBox = True
    'End Sub

    Public Sub AddTab()
        Dim tab = New ScriptEditorTab(Me)
        cwTabs.TabPages.Add(tab)
        cwTabs.SelectedIndex = cwTabs.TabCount - 1
        FocusedConsHandler.UpdateTabText(True, True)
    End Sub

    Private Sub GuiRefreshTimerTick()
        UpdateToolStrip()
    End Sub

    Public Sub UpdateToolStrip()
        Dim focusedScriptRunning As Boolean = False 'Defaults to false if no tabs are open

        If AreAnyTabsOpen Then
            focusedScriptRunning = (FocusedConsHandler.luaRunner.State = DaS_Scripting.LuaRunnerState.Running)
        End If

        ToolStrip1.
            Invoke(
            Sub(isConsoling As Boolean)
                If tsHook IsNot Nothing Then tsHook.Text = Game.DetectedDarkSoulsVersion
                If tsbtnRun IsNot Nothing Then tsbtnRun.Enabled = Not isConsoling
                If tsbtnStop IsNot Nothing Then tsbtnStop.Enabled = isConsoling
            End Sub, focusedScriptRunning)
    End Sub

    Public ReadOnly Property AreAnyTabsOpen As Boolean
        Get
            Return cwTabs IsNot Nothing AndAlso (cwTabs.TabPages.Count > 0 And cwTabs.SelectedIndex >= 0)
        End Get
    End Property

    Public ReadOnly Property FocusedConsHandler As ConsoleHandler
        Get
            ' This should not be accessed without checking AreAnyTabsOpen so an exception from null is ok
            Dim fch = Nothing
            Try
                fch = cwTabs.Invoke(
                Function() As ConsoleHandler
                    If cwTabs.TabPages.Count = 0 Then
                        AddTab()
                    End If

                    Return CType(cwTabs.SelectedTab, ScriptEditorTab).ConsHandler
                End Function
                    )
            Catch ex As Exception

            End Try

            Return CType(fch, ConsoleHandler)
        End Get
    End Property

    Public Shared Function LoadEmbeddedImage(relPath As String) As Image
        Dim result As Image
        Using strm As Stream = ThisAssembly.GetManifestResourceStream("DaS_Scripting_Console." & relPath)
            result = Image.FromStream(strm)
        End Using
        Return result
    End Function

    Private Sub LoadAllImages()
        Dim scriptNames As String() = EmbeddedResourceNames.Where(Function(x) x.Split(".").Last.ToLower = "png").ToArray()
        For Each scr In scriptNames
            Dim imageName = scr.Substring(0, scr.Length - ".png".Length)
            EmbeddedImage.Add(imageName, LoadEmbeddedImage(imageName & ".png"))
        Next
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _ThisAssembly = Assembly.GetExecutingAssembly()
        EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring("DaS_Scripting_Console.".Length)).ToArray()
        LoadAllImages()

        tsbtnNew.Image = EmbeddedImage("NewDoc")
        tsbtnOpen.Image = EmbeddedImage("OpenFile")
        tsbtnRun.Image = EmbeddedImage("StartExecution")
        tsbtnSave.Image = EmbeddedImage("SaveFile")
        tsbtnStop.Image = EmbeddedImage("StopExecution")
        tsbtnHook.Image = EmbeddedImage("Refresh")

        tsbtnNew.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbtnOpen.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbtnRun.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbtnSave.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbtnStop.DisplayStyle = ToolStripItemDisplayStyle.Image
        tsbtnHook.DisplayStyle = ToolStripItemDisplayStyle.Image

        cwTabs.TabPages.Clear()
    End Sub

    Private Sub tsbtnNew_Click(sender As Object, e As EventArgs) Handles tsbtnNew.Click
        AddTab()
        FocusedConsHandler.NewDoc()
    End Sub

    Private Sub tsbtnOpen_Click(sender As Object, e As EventArgs) Handles tsbtnOpen.Click
        Dim openedFile = FocusedConsHandler.GetOpen()
        If Not openedFile Is Nothing Then
            If FocusedConsHandler.currentDocumentModified Or FocusedConsHandler.cons.Text.Length > 0 Then
                AddTab()
            End If
            If Not AreAnyTabsOpen Then
                Throw New Exception("YOU HAD ONE JOB, OPEN BUTTON")
            End If
            FocusedConsHandler.LoadImmediate(openedFile.FullName)
        End If
    End Sub

    Private Sub tsbtnSave_Click(sender As Object, e As EventArgs) Handles tsbtnSave.Click
        FocusedConsHandler.Save()
    End Sub

    Private Sub tsNew_Click(sender As Object, e As EventArgs) Handles tsNew.Click
        AddTab()
        If Not AreAnyTabsOpen Then
            Throw New Exception("WHY")
        End If
        FocusedConsHandler.NewDoc()
    End Sub

    Private Sub tsOpen_Click(sender As Object, e As EventArgs) Handles tsOpen.Click
        Dim openedFile = FocusedConsHandler.GetOpen()
        If openedFile IsNot Nothing Then
            If FocusedConsHandler.currentDocumentModified Or FocusedConsHandler.cons.Text.Length > 0 Then
                AddTab()
            End If
            If Not AreAnyTabsOpen Then
                Throw New Exception("")
            End If
            FocusedConsHandler.LoadImmediate(openedFile.FullName)
        End If
    End Sub

    Private Sub tsSave_Click(sender As Object, e As EventArgs) Handles tsSave.Click
        FocusedConsHandler.Save()
    End Sub

    Private Sub tsSaveAs_Click(sender As Object, e As EventArgs) Handles tsSaveAs.Click
        FocusedConsHandler.SaveAs()
    End Sub

    Private Sub tsExit_Click(sender As Object, e As EventArgs) Handles tsExit.Click
        If CloseAllTabs() Then
            Close()
        End If
    End Sub

    Public Function CloseSelectedTab() As Boolean
        If FocusedConsHandler.PromptSaveChanges() Then
            CloseTab(cwTabs.SelectedIndex)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CloseTab(i As Integer) As Boolean
        If (Not AreAnyTabsOpen) Then
            Return True
        End If

        Dim tab = TryCast(cwTabs.TabPages(i), ScriptEditorTab)

        Dim isSel As Boolean = (i = cwTabs.SelectedIndex)

        If tab Is Nothing Then
            Throw New ArgumentException("Ahh fuck")
            Return False
        End If

        If tab.ConsHandler.PromptSaveChanges() Then

            'we're about to close the last page
            If cwTabs.TabPages.Count = 1 Then
                If Not _EXITING Then Close()

                Return True
            End If

            Dim oldSelIndex = cwTabs.SelectedIndex
            cwTabs.TabPages.RemoveAt(i)
            If (isSel) Then
                cwTabs.SelectTab(Math.Min(oldSelIndex, cwTabs.TabCount - 1))
                FocusedConsHandler.cons.Select()
            End If

            Return True
        Else
            Return False
        End If

    End Function

    Public Function CloseAllTabs(Optional isClosing As Boolean = False) As Boolean
        While cwTabs.TabCount > 1
            If Not CloseSelectedTab() Then
                Return False
            End If
        End While
        Return FocusedConsHandler.NewDoc()
    End Function

    Private Sub tsbtnHook_Click(sender As Object, e As EventArgs) Handles tsbtnHook.Click
        Game.InitHook()
        ToolStrip1.Invoke(Sub() tsHook.Text = Game.DetectedDarkSoulsVersion)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim fuckYouVb = New AboutPopup()
        fuckYouVb.ShowDialog()
    End Sub

    Private Sub tsmiHelp_Click(sender As Object, e As EventArgs) Handles tsmiHelp.Click
        Dim webAddress As String = "https://docs.google.com/spreadsheets/d/1Gff9pSGpYCJeNAXzUamqAInqFUwk4BhC6dC9Qk3_cDI/edit#gid=0"
        Process.Start(webAddress)
    End Sub

    Private Sub tsbtnRun_Click(sender As Object, e As EventArgs) Handles tsbtnRun.Click
        If FocusedConsHandler.luaRunner.State = LuaRunnerState.Stopped Or
            FocusedConsHandler.luaRunner.State = LuaRunnerState.Finished Then
            FocusedConsHandler.luaRunner.StartExecution(FocusedConsHandler.cons.Text)
            'ElseIf FocusedConsHandler.luaRunner.State = LuaRunnerState.Running Then
            '    FocusedConsHandler.luaRunner.PauseExecution()
            'ElseIf FocusedConsHandler.luaRunner.State = LuaRunnerState.Paused Then
            '    FocusedConsHandler.luaRunner.ResumeExecution()
        End If
    End Sub

    Private Sub tsbtnStop_Click(sender As Object, e As EventArgs) Handles tsbtnStop.Click
        If FocusedConsHandler.luaRunner.State = LuaRunnerState.Running Then
            FocusedConsHandler.luaRunner.StopExecution()
        End If
    End Sub

    Private Sub cwTabs_TabIndexChanged(sender As Object, e As EventArgs) Handles cwTabs.SelectedIndexChanged
        If Not AreAnyTabsOpen Then Return 'Yeah you'd think this wasnt needed 

        If Not cwTabs Is Nothing AndAlso cwTabs.TabCount > 0 AndAlso cwTabs.SelectedIndex >= 0 AndAlso cwTabs.SelectedIndex < cwTabs.TabCount AndAlso Not cwTabs.SelectedTab Is Nothing Then
            FocusedConsHandler.cons.Select()
        End If

        FocusedConsHandler.UpdateTabText(True, True)
    End Sub

    Private Sub ShowTabContextMenu(index As Integer, x As Integer, y As Integer)
        currentContextTab = index
        contextMenuTab.Show(cwTabs, New Point(x, y))
    End Sub

    Private Sub cwTabs_MouseClick(sender As Object, e As MouseEventArgs) Handles cwTabs.MouseClick
        If e.Button = MouseButtons.Middle Then
            For i = 0 To cwTabs.TabPages.Count - 1
                If cwTabs.GetTabRect(i).Contains(e.X, e.Y) Then
                    If i < cwTabs.TabCount Then
                        Try
                            CloseTab(i)
                        Catch ex As Exception

                        End Try
                    End If
                    Return
                End If
            Next
        ElseIf e.Button = MouseButtons.Right Then
            For i = 0 To cwTabs.TabPages.Count - 1
                If cwTabs.GetTabRect(i).Contains(e.X, e.Y) Then

                    If i < cwTabs.TabCount Then
                        Try
                            ShowTabContextMenu(i, e.X, e.Y)
                        Catch ex As Exception

                        End Try
                    End If

                    Return
                End If
            Next
        End If
    End Sub

    Private Sub NewTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewTabToolStripMenuItem.Click
        AddTab()
    End Sub

    Private Sub CloseTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseTabToolStripMenuItem.Click
        CloseTab(currentContextTab)
    End Sub

    Private Sub CloseAllOtherTabsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClloseAllOtherTabsToolStripMenuItem.Click
        Dim disTab = cwTabs.TabPages(currentContextTab)
        DoRecursiveTabClose(disTab)
    End Sub

    Function DoRecursiveTabClose(ByRef disTab As TabPage, Optional startIndex As Integer = 0) As Boolean
        For i = startIndex To cwTabs.TabPages.Count - 1
            Dim taberino = cwTabs.TabPages(i)
            If taberino IsNot disTab Then
                If Not CloseTab(i) Then
                    Return False
                End If
                Return DoRecursiveTabClose(disTab, startIndex)
            End If
        Next
        Return True
    End Function

    Private Sub CloseAllTabsToTheRightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllTabsToTheRightToolStripMenuItem.Click
        Dim disTab = cwTabs.TabPages(currentContextTab)
        DoRecursiveTabClose(disTab, currentContextTab + 1)
    End Sub

    Private Sub ConsoleWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        _EXITING = True
        e.Cancel = Not CloseAllTabs(True)
        If e.Cancel Then
            _EXITING = False
        End If
    End Sub

    'Private Sub ConsoleWindow_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    '    _turnOffDoubleBufferino()
    'End Sub
End Class