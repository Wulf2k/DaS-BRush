Imports System.IO
Imports ScintillaNET

Public Class ConsoleHandler

    Public ReadOnly dad As Form

    Public ReadOnly cons As Scintilla
    Public ReadOnly resultBox As TextBox
    Public ReadOnly btnRunPause As Button
    Public ReadOnly btnStop As Button
    Public ReadOnly btnHelp As Button
    Public ReadOnly lblHook As Label

    Public ReadOnly tsbtnRunPause As ToolStripButton
    Public ReadOnly tsbtnStop As ToolStripButton
    Public ReadOnly tsmiHelp As ToolStripMenuItem
    Public ReadOnly tsHook As ToolStripLabel

    Private __currentDocument As FileInfo = Nothing

    Private Property currentDocument As FileInfo
        Get
            Return __currentDocument
        End Get
        Set(value As FileInfo)
            If Not value Is Nothing Then
                dad.Text = value.Name & " - Dark Souls Script Console"
            End If
            __currentDocument = value
        End Set
    End Property

    Private currentDocumentModified As Boolean = False

    Private guiRefreshTimer As New Timer() With {.Interval = 16, .Enabled = True}

    Dim consoleScript As New Script("Untitled", "")
    Dim consoleScriptParams As ScriptThreadParams = ScriptThreadParams.GetNoThread(consoleScript)
    Shared ConsoleOutputText As String = ""
    Dim prevConsoleLineNumberWidth As Integer = 0
    Dim consoleLastSyntaxHighlightEndPos = 0
    Dim consoleLexScriptVars As New Dictionary(Of String, ScriptVarBase)

    Dim additionalAutocomplete As String() = {}
    Private Shared _baseAutoCompleteString = Nothing
    Public ReadOnly Property autoCompleteString As String
        Get
            If _baseAutoCompleteString Is Nothing Then
                Dim ingameFuncNames = Data.ingameFuncNameFancyCase.Values.ToArray()
                Dim moddedFunctionNames = Data.customFuncNameFancyCase.Values.ToArray()

                Return String.Join(" ", ingameFuncNames.Concat(moddedFunctionNames).Concat(additionalAutocomplete).OrderBy(Function(x) x))
            End If

            Return _baseAutoCompleteString
        End Get
    End Property

    Public Sub New(ByRef cons As Scintilla,
                   ByRef resultBox As TextBox,
                   ByRef btnRunPause As Button,
                   ByRef btnStop As Button,
                   ByRef btnHelp As Button,
                   ByRef lblHook As Label)
        Me.cons = cons
        Me.resultBox = resultBox
        Me.btnRunPause = btnRunPause
        Me.btnStop = btnStop
        Me.btnHelp = btnHelp
        Me.lblHook = lblHook

        InitStyle()
        InitAutocomplete()
        HookEvents()
    End Sub

    Public Sub New(ByRef dad As Form,
                   ByRef cons As Scintilla,
                   ByRef resultBox As TextBox,
                   ByRef tsbtnRunPause As ToolStripButton,
                   ByRef tsbtnStop As ToolStripButton,
                   ByRef tsbtnSave As ToolStripButton,
                   ByRef tsbtnNew As ToolStripButton,
                   ByRef tsbtnOpen As ToolStripButton,
                   ByRef tsmiHelp As ToolStripMenuItem,
                   ByRef tsHook As ToolStripLabel)
        Me.dad = dad
        Me.cons = cons
        Me.resultBox = resultBox
        Me.tsbtnRunPause = tsbtnRunPause
        Me.tsbtnStop = tsbtnStop
        'Me.tsbtnSave = tsbtnSave
        'Me.tsbtnNew = tsbtnNew
        'Me.tsbtnOpen = tsbtnOpen
        Me.tsmiHelp = tsmiHelp
        Me.tsHook = tsHook

        InitStyle()
        InitAutocomplete()
        HookEvents()
    End Sub

    Private Sub HookEvents()

        If Not btnRunPause Is Nothing Then AddHandler btnRunPause.Click, AddressOf RunButtonClicked
        If Not btnStop Is Nothing Then AddHandler btnStop.Click, AddressOf CancelButtonClicked
        If Not btnHelp Is Nothing Then AddHandler btnHelp.Click, AddressOf HelpButtonClicked

        If Not tsbtnRunPause Is Nothing Then AddHandler tsbtnRunPause.Click, AddressOf RunButtonClicked
        If Not tsbtnStop Is Nothing Then AddHandler tsbtnStop.Click, AddressOf CancelButtonClicked
        'If Not tsbtnSave Is Nothing Then AddHandler tsbtnSave.Click, AddressOf SaveButtonClicked
        'If Not tsbtnNew Is Nothing Then AddHandler tsbtnNew.Click, AddressOf NewButtonClicked
        'If Not tsbtnOpen Is Nothing Then AddHandler tsbtnOpen.Click, AddressOf OpenButtonClicked
        If Not tsmiHelp Is Nothing Then AddHandler tsmiHelp.Click, AddressOf HelpButtonClicked

        AddHandler cons.TextChanged, AddressOf ConsoleTextChanged
        AddHandler cons.CharAdded, AddressOf ConsoleCharAdded
        AddHandler cons.Delete, AddressOf ConsoleDelete

        AddHandler guiRefreshTimer.Tick, AddressOf GuiRefreshTimerTick

    End Sub

    Private Sub InitAutocomplete()
        CheckAllVars()

        cons.AutoCMaxHeight = 16
    End Sub

    Private Sub InitStyle()


        cons.StyleResetDefault()

        cons.Styles(Style.Default).Font = "Consolas"
        cons.Styles(Style.Default).Size = 10

        cons.Lexer = Lexer.PureBasic
        cons.Styles(Style.PureBasic.Comment).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.Number).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.HexNumber).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.BinNumber).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.Keyword).Bold = True
        cons.Styles(Style.PureBasic.Keyword).ForeColor = Color.Blue
        cons.Styles(Style.PureBasic.Keyword).Weight = 7000
        cons.Styles(Style.PureBasic.String).ForeColor = Color.Violet
        cons.Styles(Style.PureBasic.Preprocessor).ForeColor = Color.Blue
        cons.Styles(Style.PureBasic.Operator).ForeColor = Color.Red
        cons.Styles(Style.PureBasic.Keyword2).ForeColor = Color.BlueViolet
        cons.Styles(Style.PureBasic.Keyword3).ForeColor = Color.Red
        cons.Styles(Style.PureBasic.Keyword4).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.Label).ForeColor = Color.Green
        cons.Styles(Style.PureBasic.CommentBlock).Bold = True
        cons.Styles(Style.PureBasic.CommentBlock).ForeColor = Color.Orange
        cons.Styles(Style.PureBasic.CommentBlock).Italic = True
        cons.Styles(Style.PureBasic.CommentBlock).Weight = 700
        cons.SetKeywords(0, autoCompleteString)
        cons.SetKeywords(2, "")
        cons.SetKeywords(3, "new int float string true false lbl goto")
    End Sub

    Private Sub CheckAllVars()
        consoleLexScriptVars.Clear()
        For Each line In cons.Lines
            Script.CheckSpecialActions(consoleLexScriptVars, line.Text, True)
        Next

        additionalAutocomplete = consoleLexScriptVars.Values.Select(Function(x) x.Name).Where(Function(x) Not String.IsNullOrWhiteSpace(x)).ToArray()
    End Sub

    Private Sub RunButtonClicked(sender As Object, e As EventArgs)
        consoleScript = New Script(If(Not currentDocument Is Nothing, currentDocument.Name, "Untitled"), cons.Lines.Select(Function(x) x.Text).ToArray())
        consoleScriptParams = consoleScript.ExecuteInBackground()
    End Sub

    Private Sub CancelButtonClicked(sender As Object, e As EventArgs)
        consoleScriptParams.ThisThread.Abort()
        consoleScript.CleanAbortedThreads()
    End Sub

    Private Sub HelpButtonClicked(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://docs.google.com/spreadsheets/d/1Gff9pSGpYCJeNAXzUamqAInqFUwk4BhC6dC9Qk3_cDI/edit#gid=0"
        Process.Start(webAddress)
    End Sub

    Private Sub ConsoleTextChanged(sender As Object, e As EventArgs)
        Dim newLineNumberWidth = cons.Lines.Count.ToString().Length

        If Not (newLineNumberWidth = prevConsoleLineNumberWidth) Then
            cons.Margins(0).Width = cons.TextWidth(Style.LineNumber, New String("9", newLineNumberWidth + 1)) + 2
        End If

        currentDocumentModified = True

        prevConsoleLineNumberWidth = newLineNumberWidth
    End Sub

    Private Sub ConsoleCharAdded(sender As Object, e As CharAddedEventArgs)
        Dim currentPos = cons.CurrentPosition
        Dim wordStartPos = cons.WordStartPosition(currentPos, True)
        Dim lengthEntered = currentPos - wordStartPos

        If (lengthEntered > 0 And Not cons.AutoCActive) Then
            CheckAllVars()
            cons.AutoCShow(lengthEntered, autoCompleteString)
        End If

        If ChrW(e.Char) = " "c Then
            cons.Colorize(consoleLastSyntaxHighlightEndPos, currentPos)

            consoleLastSyntaxHighlightEndPos = currentPos

        End If
    End Sub

    Private Sub ConsoleDelete(sender As Object, e As ModificationEventArgs)
        If Not String.IsNullOrWhiteSpace(e.Text.Trim()) Then
            CheckAllVars()
        End If
    End Sub

    Private Sub GuiRefreshTimerTick()

        SyncLock consoleScript.outputLock
            resultBox.Text = consoleScript.outStr
        End SyncLock

        Dim isConsoling = ConsoleExecuting()

        If Not btnRunPause Is Nothing Then btnRunPause.Enabled = Not isConsoling
        If Not btnStop Is Nothing Then btnStop.Enabled = isConsoling
        'TODO: Make run button change to break button while running like VS does (i dont feel like implementing the actual break part of it)
        If Not tsbtnRunPause Is Nothing Then tsbtnRunPause.Enabled = Not isConsoling
        If Not tsbtnStop Is Nothing Then tsbtnStop.Enabled = isConsoling

        If Not lblHook Is Nothing Then
            SyncLock Hook.DetectedDarkSoulsVersion
                lblHook.Text = Hook.DetectedDarkSoulsVersion
            End SyncLock
        End If

        If Not tsHook Is Nothing Then
            SyncLock Hook.DetectedDarkSoulsVersion
                tsHook.Text = Hook.DetectedDarkSoulsVersion
            End SyncLock
        End If

    End Sub

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

    Public Sub Open()
        If PromptSaveChanges() Then
            Using dlg = New OpenFileDialog With {
            .CheckPathExists = True,
            .DefaultExt = "2ks",
            .FileName = If(Not currentDocument Is Nothing, currentDocument.Name, ""),
            .Filter = "Wulf2k™ Scripts (*.2ks)|*.2ks|All files (*.*)|*.*",
            .AddExtension = True,
            .CheckFileExists = True,
            .ShowReadOnly = False,
            .InitialDirectory = If(Not currentDocument Is Nothing, currentDocument.DirectoryName, New FileInfo(Reflection.Assembly.GetExecutingAssembly().FullName).DirectoryName),
            .RestoreDirectory = False,
            .SupportMultiDottedExtensions = True,
            .Title = "Open File"
            }
                If dlg.ShowDialog() = DialogResult.OK Then
                    currentDocument = New FileInfo(dlg.FileName)
                    cons.Text = File.ReadAllText(currentDocument.FullName)
                    currentDocumentModified = False
                End If
            End Using
        End If
    End Sub

    Public Function SaveAs() As Boolean
        Using dlg = New SaveFileDialog With {
            .CheckPathExists = True,
            .DefaultExt = "2ks",
            .FileName = If(Not currentDocument Is Nothing, currentDocument.Name, ""),
            .Filter = "Wulf2k™ Scripts (*.2ks)|*.2ks|All files (*.*)|*.*",
            .AddExtension = True,
            .OverwritePrompt = True,
            .InitialDirectory = If(Not currentDocument Is Nothing, currentDocument.DirectoryName, New FileInfo(Reflection.Assembly.GetExecutingAssembly().FullName).DirectoryName),
            .RestoreDirectory = False,
            .SupportMultiDottedExtensions = True,
            .Title = "Save File As"
            }
            If dlg.ShowDialog() = DialogResult.OK Then
                currentDocument = New FileInfo(dlg.FileName)
                Return Save()
            End If
        End Using

        Return False
    End Function

    Public Function Save() As Boolean
        If currentDocument Is Nothing Then
            Return SaveAs()
        End If

        File.WriteAllText(currentDocument.FullName, cons.Text)
        currentDocumentModified = False
        Return True
    End Function

    Public Function PromptSaveChanges() As Boolean

        If Not currentDocumentModified Then
            Return True
        End If

        Dim docName = If(Not currentDocument Is Nothing, currentDocument.Name, "Untitled")
        Dim dlgRes = MessageBox.Show("Would you like to save changes made to """ & docName & """?",
                                       "Save """ & docName & """?", MessageBoxButtons.YesNoCancel)

        If dlgRes = DialogResult.Cancel Then
            Return False
        ElseIf dlgRes = DialogResult.No Then
            Return True
        ElseIf dlgRes = DialogResult.Yes Then
            If Save() Then
                Return True
            Else
                Return PromptSaveChanges()
            End If
        End If

        Return False
    End Function

    Public Sub NewDoc()
        If PromptSaveChanges() Then
            cons.Text = ""
            currentDocument = Nothing
            currentDocumentModified = False
        End If
    End Sub

End Class
