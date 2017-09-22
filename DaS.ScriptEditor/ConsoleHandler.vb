Imports System.ComponentModel
Imports System.IO
Imports AutocompleteMenuNS
Imports DaS.ScriptLib
Imports DaS.ScriptLib.Lua
Imports ICSharpCode.TextEditor
Imports ScintillaNET
Imports DaS.ScriptLib.Injection

Public Class ConsoleHandler

    Public Const LUA_INDENT_WIDTH = 4
    Public ReadOnly ParentScriptTab As ScriptEditorTab
    'Public ReadOnly cons As Scintilla
    Public ReadOnly cons As ICSharpCode.TextEditor.TextEditorControlEx
    Public ReadOnly auto As AutocompleteMenuNS.AutocompleteMenu
    Private __currentDocument As FileInfo = Nothing

    Public ScriptThread As Threading.Thread

    Public Const LUA_MATH_ETC = "utf8 utf8.char utf8.charpattern utf8.codepoint utf8.codes utf8.len utf8.offset string string.byte string.char string.dump string.find string.format string.gmatch string.gsub string.len string.lower string.match string.rep string.reverse string.sub string.upper string.pack string.packsize string.unpack table table.concat table.insert table.remove table.sort  table.pack table.unpack table.move math math.abs math.acos math.asin math.atan math.ceil math.cos math.deg math.exp math.floor math.fmod math.huge math.log math.max math.min math.modf math.pi math.rad math.random math.randomseed math.sin math.sqrt math.tan math.maxinteger math.mininteger math.tointeger math.type math.ult"
    Public Const LUA_SYS_ETC = "coroutine coroutine.create coroutine.resume coroutine.running coroutine.status coroutine.wrap coroutine.yield coroutine.isyieldable package package.cpath package.loaded package.loadlib package.path package.preload package.config package.searchers package.searchpath io io.close io.flush io.input io.lines io.open io.output io.popen io.read io.stderr io.stdin io.stdout io.tmpfile io.type io.write os os.clock os.date os.difftime os.execute os.exit os.getenv os.remove os.rename os.setlocale os.time os.tmpname debug debug.debug debug.gethook debug.getinfo debug.getlocal debug.getmetatable debug.getregistry debug.getupvalue debug.sethook debug.setlocal debug.setmetatable debug.setupvalue debug.traceback debug.getuservalue debug.setuservalue debug.upvalueid debug.upvaluejoin"

    Public ReadOnly Property AutoCompleteOpened As Boolean = False
    Public ReadOnly Property AutoCompleteOpenedBuffer As Integer = 0
    Public ReadOnly Property AutoCompleteOpenedBufferMax As Integer = 2
    Public ReadOnly Property AutoCompleteOpenedBufferTimer As New Timer() With {.Interval = 33, .Enabled = True}

    Private ReadOnly Property LastDiskHash As Byte() = New Byte() {}

    'TODO:NEWLUA Public ReadOnly Property curMod As DsMod

    Public Sub ResetMod(script As String)

        'TODO:Meow
        'TODO:NEWLUA _curMod = New DsMod(curMod.ScriptText)
    End Sub

    Public Property currentDocument As FileInfo
        Get
            Return __currentDocument
        End Get
        Private Set(value As FileInfo)
            __currentDocument = value
            UpdateTabText(True, False)
        End Set
    End Property

    Private _currentDocumentModified As Boolean = False

    Public Property currentDocumentModified As Boolean
        Get
            Return _currentDocumentModified
        End Get
        Private Set(value As Boolean)
            _currentDocumentModified = value
            UpdateTabText(True, False)
            If Not value Then
                _LastDiskHash = GetMd5OfString(cons.Text)
            End If
        End Set
    End Property

    Public Sub UpdateTabText(updateMainWindowTitleBar As Boolean, updateMainWindowToolStrip As Boolean)
        ParentScriptTab.Invoke(
            Sub()
                If currentDocument Is Nothing Then
                    ParentScriptTab.Text = "Untitled.lua"
                Else
                    ParentScriptTab.Text = currentDocument.Name
                End If

                If currentDocumentModified Then
                    ParentScriptTab.Text &= "*"
                End If

                If ScriptThread IsNot Nothing Then
                    ParentScriptTab.Text &= " [Running]"
                End If
            End Sub)

        If updateMainWindowTitleBar Then
            ParentScriptTab.ParentConsoleWindow.Invoke(
            Sub()
                If ParentScriptTab.ParentConsoleWindow.FocusedConsHandler Is Me Then ParentScriptTab.ParentConsoleWindow.Text = ParentScriptTab.Text & " - Dark Souls Script Console"
            End Sub)
        End If

        If updateMainWindowToolStrip Then
            ParentScriptTab.ParentConsoleWindow.UpdateToolStrip()
        End If

    End Sub

    Dim prevConsoleLineNumberWidth As Integer = 0
    Dim consoleLastSyntaxHighlightEndPos = 0

    Private Shared ReadOnly Property AutoCompleteListBase As List(Of String)
    Private autoCompleteListUser As New List(Of String)

    Public ReadOnly Property AutoCompleteString As String
    'TODO: NEWLUA Public ReadOnly Property CurrentFuncInfoList As List(Of FuncInfo)
    Public ReadOnly Property CurrentCallTipIndex As Integer

    Public Sub New(ByRef dad As ScriptEditorTab)
        Me.ParentScriptTab = dad

        'TODO:NEWLUA curMod = New DsMod()
        'cons = New Scintilla()
        cons = New TextEditorControlEx()
        auto = New AutocompleteMenuNS.AutocompleteMenu()

        InitializeConsole()
        InitAutocomplete()
        HookEvents()

        'dad.Controls.Add(cons)
        dad.Controls.Add(cons)
    End Sub

    Private Sub InitializeConsole()
        'cons.Dock = DockStyle.Fill
        'cons.AutoCCancelAtStart = False
        'cons.AutoCIgnoreCase = True
        'cons.AutoCOrder = Order.Presorted
        'cons.AutoCAutoHide = True
        'cons.AutoCDropRestOfWord = True
        'cons.AutoCMaxWidth = 64
        'cons.BufferedDraw = True
        'cons.BorderStyle = BorderStyle.FixedSingle
        'cons.CaretLineBackColor = SystemColors.ControlLightLight
        'cons.CaretLineVisible = True
        'cons.HScrollBar = False
        'cons.IdleStyling = IdleStyling.All
        'cons.Lexer = Lexer.Lua
        'cons.Location = New Point(121, 246)
        'cons.Name = "cons"
        'cons.PhasesDraw = Phases.Multiple
        'cons.Size = New Size(628, 73)
        'cons.TabIndex = 10
        'cons.TabStop = False
        'cons.Technology = Technology.DirectWrite
        'cons.WrapMode = WrapMode.Word
        'cons.WrapVisualFlagLocation = WrapVisualFlagLocation.StartByText
        'cons.WrapVisualFlags = WrapVisualFlags.Margin
        'cons.AllowDrop = True


        'cons_newlua.AllowCaretBeyondEOL = True
        cons.AutoScroll = True
        cons.AutoSize = False
        cons.BracketMatchingStyle = ICSharpCode.TextEditor.Document.BracketMatchingStyle.After
        cons.ConvertTabsToSpaces = True
        cons.EnableFolding = True
        cons.Font = New Font("Consolas", 10)
        cons.HideVScrollBarIfPossible = True
        cons.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.Auto
        cons.IsIconBarVisible = True
        'cons_newlua.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow
        cons.Name = "cons_newlua"
        cons.Parent = ParentScriptTab
        cons.SetHighlighting("Lua")
        cons.ShowLineNumbers = True
        cons.ShowMatchingBracket = True
        cons.SyntaxHighlighting = "Lua"
        cons.TabIndent = 4
        cons.TabStop = False
        cons.Dock = DockStyle.Fill
        cons.VerticalScroll.Visible = True
        cons.VerticalScroll.Enabled = False

        cons.ActiveTextAreaControl.TextEditorProperties.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit

    End Sub

    Private Sub HookEvents()

        'AddHandler cons.TextChanged, AddressOf Console_TextChanged
        'AddHandler cons.CharAdded, AddressOf Console_CharAdded
        'AddHandler cons.Delete, AddressOf Console_Delete
        AddHandler cons.KeyDown, AddressOf Console_KeyDown
        AddHandler cons.DragDrop, AddressOf Console_DragDrop
        'AddHandler cons.AutoCSelection, AddressOf Console_AutoCSelect
        'AddHandler cons.AutoCCompleted, AddressOf Console_AutoCCompleted
        AddHandler cons.Enter, AddressOf Console_Enter
        'AddHandler cons.DragEnter, AddressOf ConsoleDragEnter

        AddHandler AutoCompleteOpenedBufferTimer.Tick, AddressOf AutoCompleteOpenedBufferTimer_Tick

    End Sub

    Private Sub Console_Enter(sender As Object, e As EventArgs)

        CheckCurDocOnDiskStatus()

    End Sub

    'Private Sub LuaRunner_OnStart(args As LuaRunnerEventArgs)
    '    UpdateTabText(True, True)
    'End Sub

    'Private Sub LuaRunner_OnStop()
    '    UpdateTabText(True, True)
    'End Sub

    'Private Sub LuaRunner_OnFinishAny(args As LuaRunnerEventArgs)
    '    UpdateTabText(True, True)
    'End Sub

    'Private Sub LuaRunner_OnFinishError(args As LuaRunnerEventArgs, err As Exception)
    '    If MessageBox.Show("Error running script '" & args.ExecutingThread.Name & "': " & vbCrLf & vbCrLf & err.Message & vbCrLf & vbCrLf &
    '                       "Would you like to crash the program now for debugging? (Hint: choose 'No')", "Error",
    '                       MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
    '        Throw err
    '    End If
    'End Sub

    Private Sub AutoCompleteOpenedBufferTimer_Tick(sender As Object, e As EventArgs)
        If AutoCompleteOpened Then
            _AutoCompleteOpenedBuffer = AutoCompleteOpenedBufferMax
        End If

        If AutoCompleteOpenedBuffer > 0 Then
            _AutoCompleteOpenedBuffer -= 1
        ElseIf AutoCompleteOpenedBuffer < 0 Then
            _AutoCompleteOpenedBuffer = 0
        End If
    End Sub

    Private Function GetAutoCWord(index As Integer) As String
        Return AutoCompleteString.Split(" ")(index)
    End Function

    'TODO: NEWLUA Private Sub NextCallTip()
    '    If CurrentCallTipIndex >= CurrentFuncInfoList.Count - 1 Then
    '        _CurrentCallTipIndex = 0
    '    Else
    '        _CurrentCallTipIndex += 1
    '    End If
    'End Sub

    'TODO: NEWLUA Private Sub PreviousCallTip()
    '    If CurrentCallTipIndex <= 0 Then
    '        _CurrentCallTipIndex = CurrentFuncInfoList.Count - 1
    '    Else
    '        _CurrentCallTipIndex -= 1
    '    End If
    'End Sub


    'TODO: NEWLUA Private Sub UpdateCallTip(txt As String, startPos As Integer)
    '    If Not ScriptRes.autoCompleteFuncInfoByName.ContainsKey(txt) Then
    '        If cons.CallTipActive Then
    '            cons.CallTipCancel()
    '        End If
    '        _CurrentFuncInfoList = New FuncInfo() {New IngameFuncInfo("0|?FuncName?(?FuncArgs?)", "")}.ToList()
    '        Return
    '    End If
    '    If Not cons.CallTipActive Or Not CurrentFuncInfoList?.First()?.Name = txt Then
    '        _CurrentFuncInfoList = New FuncInfo() {ScriptRes.autoCompleteFuncInfoByName(txt)}.ToList()
    '        _CurrentCallTipIndex = 0
    '    End If
    '    _CurrentCallTipIndex = Math.Max(Math.Min(CurrentFuncInfoList.Count - 1, CurrentCallTipIndex), 0)
    '    cons.CallTipShow(startPos, CurrentFuncInfoList(CurrentCallTipIndex).UsageString)

    'End Sub

    Private Sub Console_AutoCCompleted(sender As Object, e As AutoCSelectionEventArgs)
        'TODO:NEWLUA UpdateCallTip(e.Text, e.Position)
    End Sub

    Private Sub Console_AutoCSelect(sender As Object, e As AutoCSelectionEventArgs)
        'TODO:NEWLUA UpdateCallTip(e.Text, e.Position)
    End Sub

    'Private Sub ConsoleDragEnter(sender As Object, e As DragEventArgs)
    'End Sub

    Private Sub Console_DragDrop(sender As Object, e As DragEventArgs)
        e.Effect = DragDropEffects.Copy
        Dim files = CType(e.Data.GetData(DataFormats.StringFormat), String())
        For Each f In files
            ParentScriptTab.ParentConsoleWindow.AddTab()
            ParentScriptTab.ParentConsoleWindow.FocusedConsHandler.LoadImmediate(f)
        Next
    End Sub

    Private Sub InitAutocomplete()
        auto.TargetControlWrapper = AutocompleteMenuNS.TextBoxWrapper.Create(cons)

        RebuildAutoComplete()

        auto.AllowsTabKey = True
        auto.AppearInterval = 250
        AddHandler auto.Opening, AddressOf AutoCompleteListOpening
        AddHandler auto.Selected, AddressOf AutoCompleteListSelected
        AddHandler auto.Selecting, AddressOf AutoCompleteListSelecting
        auto.AutoPopup = True
        auto.CaptureFocus = False
        auto.MaximumSize = New Size(512, 512)
        auto.MinFragmentLength = 2
        auto.Font = New Font(New FontFamily("Segoe UI Light"), 8, FontStyle.Regular)
        auto.ToolTipDuration = Integer.MaxValue
        'Image 0 = function image, image 1 = field image
        auto.ImageList = New ImageList()
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("MethodIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("EstusIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("FieldIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("TypeIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("LuaIcon"))
    End Sub

    Private Sub AutoCompleteListSelecting(sender As Object, e As SelectingEventArgs)
        _AutoCompleteOpened = False
    End Sub

    Private Sub AutoCompleteListSelected(sender As Object, e As SelectedEventArgs)
        _AutoCompleteOpened = False
        auto.ToolTipInstance.RemoveAll()
    End Sub

    Private Sub AutoCompleteListOpening(sender As Object, e As CancelEventArgs)
        _AutoCompleteOpened = True
    End Sub


    Private Sub RebuildAutoComplete()

        auto.Items = (New LuaInterface()).State.Globals.ToArray()

    End Sub
    '    Dim fullAutoCompleteList = AutoCompleteListBase.Concat(autoCompleteListUser).OrderBy(Function(x) x)

    '    auto.Items = New String() {}

    '    For Each ac In fullAutoCompleteList
    '        Dim imageIndex = -1
    '        Dim menuText = ac
    '        Dim toolTipTitle = ""
    '        Dim toolTipText = ""
    '        'If Lua.LuaDummyAutoComplete.ContainsKey(ac) Then
    '        '    menuText = Lua.LuaDummyAutoComplete(ac)
    '        '    toolTipTitle = "Lua State Function"
    '        '    toolTipText = "Member of the lua state class that runs this lua script."
    '        '    imageIndex = 4
    '        If ScriptRes.autoCompleteFuncInfoByName.ContainsKey(ac) Then
    '            menuText = ScriptRes.autoCompleteFuncInfoByName(ac).UsageString

    '            If TryCast(ScriptRes.autoCompleteFuncInfoByName(ac), IngameFuncInfo) IsNot Nothing Then
    '                toolTipTitle = "Vanilla Dark Souls Lua Function"
    '                imageIndex = 1
    '            ElseIf TryCast(ScriptRes.autoCompleteFuncInfoByName(ac), CustomFuncInfo) IsNot Nothing Then
    '                toolTipTitle = "DaS-Scripting Library Function"
    '                imageIndex = 0
    '            End If

    '            toolTipText = "?PlaceHolderToolTip?"

    '        ElseIf ScriptRes.propTypes.ContainsKey(ac) Then
    '            imageIndex = 2
    '            menuText = ScriptRes.propTypes(ac)
    '            toolTipTitle = "DaS-Scripting Library Field"
    '            toolTipText = "Don't mess with stuff in the scripting library unless you know what you're doing.™"

    '        ElseIf ScriptRes.autoCompleteAdditionalTypes.Any(Function(x) x.Name = ac) Then
    '            imageIndex = 3
    '            menuText = "typedef " & ac
    '            toolTipTitle = "DaS-Scripting Library Type"
    '            toolTipText = "Don't mess with stuff in the scripting library unless you know what you're doing.™"
    '        Else
    '            imageIndex = 4
    '            toolTipTitle = "Lua Default Element"
    '            toolTipText = "You can find the documentation on the internet somewhere.™"
    '        End If

    '        auto.AddItem(New AutocompleteMenuNS.AutocompleteItem(ac, imageIndex, menuText, toolTipTitle, toolTipText))
    '    Next

    '    _AutoCompleteString = String.Join(" ", AutoCompleteListBase.Concat(autoCompleteListUser).OrderBy(Function(x) x).ToArray())
    '    cons.SetKeywords(2, AutoCompleteString)
    'End Sub

    Shared Sub New()
        Dim aclist As New List(Of String)
        aclist.AddRange(LUA_MATH_ETC.Split(" "))
        aclist.AddRange(LUA_SYS_ETC.Split(" "))
        'TODO:NEWLUA aclist.AddRange(ScriptRes.autoCompleteFuncInfoByName.Keys)
        'TODO:NEWLUA aclist.AddRange(ScriptRes.propTypes.Keys)
        'TODO:NEWLUA aclist.AddRange(ScriptRes.autoCompleteAdditionalTypes.Select(Function(x) x.Name))
        'aclist.AddRange(Lua.LuaDummyAutoComplete.Keys)
        aclist.AddRange("and break do else elseif end false for function goto if in local nil not or repeat return then true until while".Split(" "))

        _AutoCompleteListBase = aclist.OrderBy(Function(x) x).ToList()
    End Sub

    '
    ' Note: when you wanna override a detected keypress and not have it do its original thing,
    '       all you gotta do is not do a Return afterwards, since at the very end of the method,
    '       it sets the key to be overrided (and any time you don't wanna override, simply
    '       do a Return before it reaches that thing at the end of the method)
    '
    ' Please keep everything in this function in the scope of the ConsoleWindow (i.e. dad.Mum.X)
    ' because we might need to change when the event fires due to the way it currently is requiring
    ' you to have your "focus" actually inside the script text box itself (so the hotkeys dont work
    ' if you just clicked into the menus on top etc)
    ' Since we can at least ASSUME that this console window SHOULD be the one in focus (unless some weird
    ' bug is making the keydown event fire on a background tab...), you should just be able to assume
    ' that dad.Mum.FocusedConsHandler is the same as this ConsoleHandler
    Private Sub Console_KeyDown(sender As Object, e As KeyEventArgs)

        If ParentScriptTab.ParentConsoleWindow.FocusedConsHandler IsNot Me Then
            e.Handled = True
            e.SuppressKeyPress = True
            Return
        End If

        ' Some really useful debug thingies:
        ' Console.Write("e.KeyCode = [ " & e.KeyCode.ToString() & " ]")
        ' Console.WriteLine(", e.Modifiers = [ " & e.Modifiers.ToString() & " ]")

        If e.Control AndAlso e.Shift AndAlso Not e.Alt Then 'CTRL + SHIFT + (KEY)
            If e.KeyCode = Keys.S Then
                SaveAs()
            Else
                Return
            End If
        ElseIf e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then 'CTRL + (KEY)
            If e.KeyCode = Keys.S Then
                Save()
            ElseIf e.KeyCode = Keys.T Then
                ParentScriptTab.ParentConsoleWindow.AddTab()
            ElseIf e.KeyCode = Keys.W Then
                ParentScriptTab.ParentConsoleWindow.CloseSelectedTab()
            ElseIf e.KeyCode = Keys.N Then
                ParentScriptTab.ParentConsoleWindow.AddTab()
                NewDoc()
            ElseIf e.KeyCode = Keys.O Then
                Dim openedFile = GetOpen()
                If openedFile IsNot Nothing Then
                    If currentDocumentModified Or cons.Text.Length > 0 Then
                        ParentScriptTab.ParentConsoleWindow.AddTab()
                    End If
                    LoadImmediate(openedFile.FullName)
                End If
                'We don't return if the if fails, since that just means the user closed
                'the open file dialog (they still chose to press the open hotkey specifically)
                'ElseIf e.KeyCode = Keys.D0 Then
                '    cons.Zoom = 0
                'ElseIf e.KeyCode = Keys.Oemplus Then
                '    If cons.Zoom <= 19 Then cons.ZoomIn()
                'ElseIf e.KeyCode = Keys.OemMinus Then
                '    ' Seemed to stop zooming out at -8, then continue to subtract the value all the way to -10, making the next
                '    ' few zoom-ins not actually zoom in until it gets above -8
                '    If cons.Zoom >= -7 Then cons.ZoomOut()
            Else
                Return
            End If
        ElseIf Not e.Control AndAlso e.Shift AndAlso Not e.Alt Then 'SHIFT + (KEY)
            If False Then 'If AutoCompleteOpenedBuffer = 0 AndAlso e.KeyCode = Keys.Tab Then
                'UnIndentSelection()
            Else
                Return
            End If
        ElseIf Not e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then '(KEY)
            If False Then 'If e.KeyCode = Keys.Return Then
                'Dim unIndentModeOrWhatever = CheckIfThirdEnterKeyPressInARow()

                'If dat bitch aint lexed an shiet, we need to do a boring thing
                'If Not LexDatBitchOnPressEnterKey() Then
                '    GoToNextLine_RetainingTheIndentLevelInTheProcess()
                '    cons.ScrollCaret()
                '    'If unIndentModeOrWhatever Then
                '    '    UnIndent()
                '    'End If
                'End If
                'ElseIf e.KeyCode = Keys.Back Then
                '    Dim curLine = cons.CurrentLine
                '    Dim currentLineIndent = GetLineIndent(curLine)
                '    Dim previousLineIndent = GetLineIndent(curLine - 1)
                '    Dim currentLineEmpty = String.IsNullOrWhiteSpace(cons.Lines(cons.CurrentLine).Text.Trim())

                '    If (currentLineEmpty AndAlso currentLineIndent = previousLineIndent AndAlso GetCurrentColumn() > 0) Then
                '        UnIndentLine(cons.CurrentLine)
                '    Else
                '        e.Handled = False
                '        e.SuppressKeyPress = False
                '        Return
                '    End If
            ElseIf e.KeyCode = Keys.F5 Then

                If (ScriptThread Is Nothing) Then
                    ParentScriptTab.ParentConsoleWindow.tsbtnRun_Click(Nothing, New EventArgs())
                Else
                    ParentScriptTab.ParentConsoleWindow.tsbtnStop_Click(Nothing, New EventArgs())
                End If
            ElseIf e.KeyCode = Keys.F7 Then
                DARKSOULS.TryAttachToDarkSouls()
                ParentScriptTab.ParentConsoleWindow.UpdateToolStrip()
                'ElseIf e.KeyCode = Keys.Tab Then
                '    If AutoCompleteOpenedBuffer = 0 Then
                '        If cons.LineFromPosition(cons.SelectionStart) <>
                '                cons.LineFromPosition(cons.SelectionEnd) Then
                '            IndentSelection()
                '        Else
                '            cons.AddText(New String(" ", GetDistToNextTabStop()))
                '        End If
                '    End If
            Else
                Return
            End If
        Else
            Return
        End If

        e.Handled = True
        e.SuppressKeyPress = True
    End Sub

    'Private Sub ResetConsoleSelection()
    '    cons.SelectionStart = cons.CurrentPosition
    '    cons.SelectionEnd = cons.CurrentPosition
    'End Sub

    'Public Sub IndentSelection()
    '    DentSelection(False)
    'End Sub

    'Public Sub UnIndentSelection()
    '    DentSelection(True)
    'End Sub

    'Private Sub DentSelection(un As Boolean)
    '    Dim oldSelectionStart = cons.SelectionStart
    '    Dim oldSelectionEnd = cons.SelectionEnd

    '    Dim firstLine = cons.LineFromPosition(cons.SelectionStart)
    '    Dim lastLine = cons.LineFromPosition(cons.SelectionEnd)

    '    Dim selectionStartShift = 0
    '    Dim selectionEndShift = 0

    '    For i = firstLine To lastLine
    '        Dim shiftAmount = If(un, UnIndentLine(i), IndentLine(i))
    '        If i = firstLine Then
    '            selectionStartShift = shiftAmount
    '        ElseIf i = lastLine Then
    '            selectionEndShift = shiftAmount
    '        End If
    '    Next

    '    cons.SelectionStart = oldSelectionStart
    '    cons.SelectionEnd = oldSelectionEnd

    '    cons.SelectionStart += (selectionStartShift * If(un, -1, 1))
    '    cons.SelectionEnd += (selectionEndShift * If(un, -1, 1))
    'End Sub

    'Private Function IndentLine(line As Integer) As Integer
    '    Dim lineIndent = GetLineIndent(line)
    '    Dim returnToPos = cons.CurrentPosition

    '    Dim lineStart = cons.Lines(line).Position
    '    cons.CurrentPosition = lineStart + lineIndent
    '    Dim shiftDistance = GetDistToNextTabStop()
    '    cons.AddText(New String(" "c, shiftDistance))

    '    cons.CurrentPosition = returnToPos
    '    ResetConsoleSelection()
    '    Return shiftDistance
    'End Function

    'Private Function UnIndentLine(line As Integer) As Integer
    '    Dim lineIndent = GetLineIndent(line)

    '    If lineIndent = 0 Then
    '        Return 0
    '    End If

    '    Dim returnToPos = cons.CurrentPosition

    '    Dim lineStart = cons.Lines(line).Position
    '    cons.CurrentPosition = lineStart + lineIndent
    '    ' Don't delete stuff on a previous line
    '    Dim shiftDistance = Math.Min(GetDistToPrevTabStop(), cons.CurrentPosition - lineStart)
    '    cons.SelectionStart = cons.CurrentPosition - shiftDistance
    '    cons.SelectionEnd = cons.CurrentPosition
    '    cons.ReplaceSelection("")

    '    cons.CurrentPosition = returnToPos
    '    ResetConsoleSelection()
    '    Return shiftDistance
    'End Function

    'Private Function GetCurrentColumn() As Integer
    '    Return cons.CurrentPosition - cons.Lines(cons.CurrentLine).Position
    'End Function

    'Private Function GetDistToPrevTabStop() As Integer
    '    Dim moduloValueThing = (GetCurrentColumn() Mod LUA_INDENT_WIDTH)
    '    Return If(moduloValueThing = 0, LUA_INDENT_WIDTH, moduloValueThing)
    'End Function

    'Private Function GetDistToNextTabStop() As Integer
    '    Dim moduloValueThing = (LUA_INDENT_WIDTH - (GetCurrentColumn() Mod LUA_INDENT_WIDTH))
    '    Return If(moduloValueThing = 0, LUA_INDENT_WIDTH, moduloValueThing)
    'End Function

    Private Shared Function GetUnicodeStringBytes(str As String) As Byte()
        Dim byteList As New List(Of Byte)
        For Each chr As Char In str
            Dim currentChar = BitConverter.GetBytes(chr)
            For Each b In currentChar
                byteList.Add(b)
            Next
        Next
        Return byteList.ToArray()
    End Function

    Private Shared Function GetMd5OfString(str As String) As Byte()
        Dim md5 = Security.Cryptography.MD5.Create()
        Dim strBuffer As Byte() = GetUnicodeStringBytes(str)
        Return md5.ComputeHash(strBuffer, 0, strBuffer.Length)
    End Function

    Private Shared Function GetMd5OfFileOnDisk(fileName As String) As Byte()
        Dim fileContents As String = Nothing
        Using fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read)
            Using sr = New StreamReader(fs)
                fileContents = sr.ReadToEnd()
            End Using
        End Using
        If String.IsNullOrWhiteSpace(fileContents) Then
            Return New Byte() {}
        Else
            Return GetMd5OfString(fileContents)
        End If
    End Function

    Public Sub CheckCurDocOnDiskStatus()
        If currentDocument Is Nothing Then
            Return
        End If

        If _checkCurDocStillExistsOnDisk() Then
            _checkCurDocChangedOnDisk()
        End If

        _LastDiskHash = GetMd5OfFileOnDisk(currentDocument.FullName)
    End Sub

    Private Function _checkCurDocStillExistsOnDisk() As Boolean
        If currentDocument Is Nothing Then
            Return False
        End If

        If Not File.Exists(currentDocument.FullName) Then
            Dim msgBoxResult = MessageBox.
                Show($"The file '{currentDocument.FullName}' no longer exists on-disk." & vbCrLf &
                    "Would you like to keep it open in the editor despite this?",
                    "Keep Non-Existant File Open?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If msgBoxResult = DialogResult.No Then
                ParentScriptTab.ParentConsoleWindow.cwTabs.TabPages.Remove(ParentScriptTab)
            End If

            Return False
        End If

        Return True
    End Function

    Private Function _checkCurDocChangedOnDisk() As Boolean
        If currentDocument Is Nothing Then
            Return False
        End If

        Dim currentDiskHash = GetMd5OfFileOnDisk(currentDocument.FullName)

        ' The weird LastDiskHash.Where gets each byte that is the same as that byte number on the other md5 hash
        ' If the number of perfectly matching bytes is the same as the actual array length, obviously all are equal
        If (Not (LastDiskHash.Length = currentDiskHash.Length)) OrElse
            (Not (LastDiskHash.Where(Function(x, i) x = currentDiskHash(i)).Count() = LastDiskHash.Length)) Then
            currentDocumentModified = True
            Dim msgBoxResult = MessageBox.
                Show($"The file {currentDocument.FullName} has been modified by another program since last time you saved." & vbCrLf &
                    "Would you like overwrite what you have in the editor with what is on-disk?",
                    "File Changed Elsewhere", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If msgBoxResult = DialogResult.Yes Then
                _loadImmediatelyWithNoSavePrompt(currentDocument.FullName)
            End If

            Return True
        End If

        Return False
    End Function

    Public Function GetOpen() As FileInfo
        Using dlg = New OpenFileDialog With {
        .CheckPathExists = True,
        .DefaultExt = "lua",
        .FileName = If(Not currentDocument Is Nothing, currentDocument.Name, ""),
        .Filter = "Lua Scripts (*.lua)|*.lua|All files (*.*)|*.*",
        .AddExtension = True,
        .CheckFileExists = True,
        .ShowReadOnly = False,
        .InitialDirectory = If(Not currentDocument Is Nothing, currentDocument.DirectoryName, New FileInfo(Reflection.Assembly.GetExecutingAssembly().FullName).DirectoryName),
        .RestoreDirectory = False,
        .SupportMultiDottedExtensions = True,
        .Title = "Open File"
        }
            If dlg.ShowDialog() = DialogResult.OK Then
                Return New FileInfo(dlg.FileName)
            End If
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' -----!WARNING!-----
    ''' <para/>
    ''' Please use <see cref="LoadImmediate(String)"/> unless you have a good reason to load a new file
    ''' with no "save unsaved changes?" prompt (e.g. using a different prompt, such as the "this file has been modified
    ''' outside of the editor. would you like to re-open?" prompt).
    ''' </summary>
    ''' <param name="fileName">File to load.</param>
    Private Sub _loadImmediatelyWithNoSavePrompt(fileName As String)

        cons.LoadFile(fileName)

        currentDocument = New FileInfo(fileName)
        'cons.Text = File.ReadAllText(currentDocument.FullName)
        currentDocumentModified = False

        ParentScriptTab.ParentConsoleWindow.Status("Loaded " & currentDocument.Name & " from disk.")
    End Sub

    Public Sub LoadImmediate(fileName As String)
        If PromptSaveChanges() Then
            _loadImmediatelyWithNoSavePrompt(fileName)
        End If
    End Sub

    Public Function SaveAs() As Boolean
        Using dlg = New SaveFileDialog With {
            .CheckPathExists = True,
            .DefaultExt = "lua",
            .FileName = If(Not currentDocument Is Nothing, currentDocument.Name, ""),
            .Filter = "Lua Scripts (*.lua)|*.lua|All files (*.*)|*.*",
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

        'File.WriteAllText(currentDocument.FullName, cons.Text)
        cons.SaveFile(currentDocument.FullName)
        currentDocumentModified = False
        UpdateTabText(True, False)
        ParentScriptTab.ParentConsoleWindow.Status("Saved " & currentDocument.Name & ".")
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
            Return Save()
        End If

        Return False
    End Function

    Public Function NewDoc() As Boolean
        If PromptSaveChanges() Then
            cons.Text = ""
            currentDocument = Nothing
            currentDocumentModified = False
            _LastDiskHash = New Byte() {}
            ParentScriptTab.ParentConsoleWindow.Status("Created new document.")
            Return True
        End If
        Return False
    End Function
End Class