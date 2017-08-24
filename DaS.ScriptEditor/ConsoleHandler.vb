Imports System.IO
Imports System.Text.RegularExpressions
Imports ScintillaNET
Imports DaS_Scripting
Imports System.ComponentModel
Imports AutocompleteMenuNS

Public Class ConsoleHandler

    Public Const LUA_INDENT_WIDTH = 4
    Public ReadOnly ParentScriptTab As ScriptEditorTab
    Public ReadOnly cons As Scintilla
    Public ReadOnly auto As AutocompleteMenuNS.AutocompleteMenu
    Private __currentDocument As FileInfo = Nothing

    Public Const LUA_MATH_ETC = "utf8 utf8.char utf8.charpattern utf8.codepoint utf8.codes utf8.len utf8.offset string string.byte string.char string.dump string.find string.format string.gmatch string.gsub string.len string.lower string.match string.rep string.reverse string.sub string.upper string.pack string.packsize string.unpack table table.concat table.insert table.remove table.sort  table.pack table.unpack table.move math math.abs math.acos math.asin math.atan math.ceil math.cos math.deg math.exp math.floor math.fmod math.huge math.log math.max math.min math.modf math.pi math.rad math.random math.randomseed math.sin math.sqrt math.tan math.maxinteger math.mininteger math.tointeger math.type math.ult"
    Public Const LUA_SYS_ETC = "coroutine coroutine.create coroutine.resume coroutine.running coroutine.status coroutine.wrap coroutine.yield coroutine.isyieldable package package.cpath package.loaded package.loadlib package.path package.preload package.config package.searchers package.searchpath io io.close io.flush io.input io.lines io.open io.output io.popen io.read io.stderr io.stdin io.stdout io.tmpfile io.type io.write os os.clock os.date os.difftime os.execute os.exit os.getenv os.remove os.rename os.setlocale os.time os.tmpname debug debug.debug debug.gethook debug.getinfo debug.getlocal debug.getmetatable debug.getregistry debug.getupvalue debug.sethook debug.setlocal debug.setmetatable debug.setupvalue debug.traceback debug.getuservalue debug.setuservalue debug.upvalueid debug.upvaluejoin"

    Public ReadOnly Property AutoCompleteOpened As Boolean = False
    Public ReadOnly Property AutoCompleteOpenedBuffer As Integer = 0
    Public ReadOnly Property AutoCompleteOpenedBufferMax As Integer = 2
    Public ReadOnly Property AutoCompleteOpenedBufferTimer As New Timer() With {.Interval = 33, .Enabled = True}

    Private ReadOnly Property LastDiskHash As Byte() = New Byte() {}

    Public ReadOnly Property luaRunner As Lua

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

                If luaRunner.State = LuaRunnerState.Running Then
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
    Public ReadOnly Property CurrentFuncInfoList As List(Of FuncInfo)
    Public ReadOnly Property CurrentCallTipIndex As Integer

    Public Sub New(ByRef dad As ScriptEditorTab)
        Me.ParentScriptTab = dad

        luaRunner = New Lua()
        cons = New Scintilla()
        auto = New AutocompleteMenuNS.AutocompleteMenu()

        InitializeConsole()
        InitStyle()
        InitAutocomplete()
        HookEvents()

        dad.Controls.Add(cons)
    End Sub

    Private Sub InitializeConsole()
        cons.Dock = DockStyle.Fill
        cons.AutoCCancelAtStart = False
        cons.AutoCIgnoreCase = True
        cons.AutoCOrder = Order.Presorted
        cons.AutoCAutoHide = True
        cons.AutoCDropRestOfWord = True
        cons.AutoCMaxWidth = 64
        cons.BufferedDraw = True
        cons.BorderStyle = BorderStyle.FixedSingle
        cons.CaretLineBackColor = SystemColors.ControlLightLight
        cons.CaretLineVisible = True
        cons.HScrollBar = False
        cons.IdleStyling = IdleStyling.All
        cons.Lexer = Lexer.Lua
        cons.Location = New Point(121, 246)
        cons.Name = "cons"
        cons.PhasesDraw = Phases.Multiple
        cons.Size = New Size(628, 73)
        cons.TabIndex = 10
        cons.TabStop = False
        cons.Technology = Technology.DirectWrite
        cons.WrapMode = WrapMode.Word
        cons.WrapVisualFlagLocation = WrapVisualFlagLocation.StartByText
        cons.WrapVisualFlags = WrapVisualFlags.Margin
        cons.AllowDrop = True
    End Sub

    Private Sub HookEvents()

        AddHandler cons.TextChanged, AddressOf Console_TextChanged
        AddHandler cons.CharAdded, AddressOf Console_CharAdded
        AddHandler cons.Delete, AddressOf Console_Delete
        AddHandler cons.KeyDown, AddressOf Console_KeyDown
        AddHandler cons.DragDrop, AddressOf Console_DragDrop
        AddHandler cons.AutoCSelection, AddressOf Console_AutoCSelect
        AddHandler cons.AutoCCompleted, AddressOf Console_AutoCCompleted
        AddHandler cons.Enter, AddressOf Console_Enter
        'AddHandler cons.DragEnter, AddressOf ConsoleDragEnter
        AddHandler luaRunner.OnStart, AddressOf LuaRunner_OnStart
        AddHandler luaRunner.OnStop, AddressOf LuaRunner_OnStop
        AddHandler luaRunner.OnFinishAny, AddressOf LuaRunner_OnFinishAny
        AddHandler luaRunner.OnFinishError, AddressOf LuaRunner_OnFinishError
        AddHandler AutoCompleteOpenedBufferTimer.Tick, AddressOf AutoCompleteOpenedBufferTimer_Tick

    End Sub

    Private Sub Console_Enter(sender As Object, e As EventArgs)

        CheckCurDocOnDiskStatus()

    End Sub

    Private Sub LuaRunner_OnStart(args As LuaRunnerEventArgs)
        UpdateTabText(True, True)
    End Sub

    Private Sub LuaRunner_OnStop()
        UpdateTabText(True, True)
    End Sub

    Private Sub LuaRunner_OnFinishAny(args As LuaRunnerEventArgs)
        UpdateTabText(True, True)
    End Sub

    Private Sub LuaRunner_OnFinishError(args As LuaRunnerEventArgs, err As Exception)
        If MessageBox.Show("Error running script '" & args.ExecutingThread.Name & "': " & vbCrLf & vbCrLf & err.Message & vbCrLf & vbCrLf &
                           "Would you like to crash the program now for debugging? (Hint: choose 'No')", "Error",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
            Throw err
        End If
    End Sub

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

    Private Sub NextCallTip()
        If CurrentCallTipIndex >= CurrentFuncInfoList.Count - 1 Then
            _CurrentCallTipIndex = 0
        Else
            _CurrentCallTipIndex += 1
        End If
    End Sub

    Private Sub PreviousCallTip()
        If CurrentCallTipIndex <= 0 Then
            _CurrentCallTipIndex = CurrentFuncInfoList.Count - 1
        Else
            _CurrentCallTipIndex -= 1
        End If
    End Sub

    Private Sub UpdateCallTip(txt As String, startPos As Integer)
        If Not ScriptRes.autoCompleteFuncInfoByName.ContainsKey(txt) Then
            If cons.CallTipActive Then
                cons.CallTipCancel()
            End If
            _CurrentFuncInfoList = New FuncInfo() {New IngameFuncInfo("0|?FuncName?(?FuncArgs?)", "")}.ToList()
            Return
        End If
        If Not cons.CallTipActive Or Not CurrentFuncInfoList?.First()?.Name = txt Then
            _CurrentFuncInfoList = ScriptRes.autoCompleteFuncInfoByName(txt)
            _CurrentCallTipIndex = 0
        End If
        _CurrentCallTipIndex = Math.Max(Math.Min(CurrentFuncInfoList.Count - 1, CurrentCallTipIndex), 0)
        cons.CallTipShow(startPos, CurrentFuncInfoList(CurrentCallTipIndex).UsageString)

    End Sub

    Private Sub Console_AutoCCompleted(sender As Object, e As AutoCSelectionEventArgs)
        UpdateCallTip(e.Text, e.Position)
    End Sub

    Private Sub Console_AutoCSelect(sender As Object, e As AutoCSelectionEventArgs)
        UpdateCallTip(e.Text, e.Position)
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
        auto.TargetControlWrapper = AutocompleteMenuNS.ScintillaWrapper.Create(cons)
        Dim fullAutoCompleteList = AutoCompleteListBase.Concat(autoCompleteListUser).OrderBy(Function(x) x)

        For Each ac In fullAutoCompleteList
            Dim imageIndex = -1
            Dim menuText = ac
            Dim toolTipTitle = ""
            Dim toolTipText = ""
            Dim addedParenthesis = ""
            If ScriptRes.autoCompleteFuncInfoByName.ContainsKey(ac) Then
                menuText = ScriptRes.autoCompleteFuncInfoByName(ac).First().UsageString

                If TryCast(ScriptRes.autoCompleteFuncInfoByName(ac).First(), IngameFuncInfo) IsNot Nothing Then
                    toolTipTitle = "Vanilla Dark Souls Lua Function"
                    imageIndex = 1
                ElseIf TryCast(ScriptRes.autoCompleteFuncInfoByName(ac).First(), CustomFuncInfo) IsNot Nothing Then
                    toolTipTitle = "DaS-Scripting Library Function"
                    imageIndex = 0
                End If

                addedParenthesis = "("
                toolTipText = "Can be called as:    " & String.Join(", ", ScriptRes.autoCompleteFuncInfoByName(ac).Select(Function(x) x.UsageString))

            ElseIf ScriptRes.propTypes.ContainsKey(ac) Then
                imageIndex = 2
                menuText = ScriptRes.propTypes(ac)
                toolTipTitle = "DaS-Scripting Library Field"
                toolTipText = "Don't mess with stuff in the scripting library unless you know what you're doing.™"

            ElseIf ScriptRes.autoCompleteAdditionalTypes.Any(Function(x) x.Name = ac) Then
                imageIndex = 3
                menuText = "typedef " & ac
                toolTipTitle = "DaS-Scripting Library Type"
                toolTipText = "Don't mess with stuff in the scripting library unless you know what you're doing.™"
            Else
                imageIndex = 4
                toolTipTitle = "Lua Default Element"
                toolTipText = "You can find the documentation on the internet somewhere.™"
            End If

            auto.AddItem(New AutocompleteMenuNS.AutocompleteItem(ac & addedParenthesis, imageIndex, menuText, toolTipTitle, toolTipText))
        Next

        auto.AllowsTabKey = True
        auto.AppearInterval = 33
        AddHandler auto.Opening, AddressOf AutoCompleteListOpening
        AddHandler auto.Selected, AddressOf AutoCompleteListSelected
        AddHandler auto.Selecting, AddressOf AutoCompleteListSelecting
        auto.AutoPopup = True
        auto.CaptureFocus = False
        auto.MaximumSize = New Size(512, 512)
        auto.MinFragmentLength = 1
        auto.Font = New Font(New FontFamily("Consolas"), 8, FontStyle.Bold)
        auto.ToolTipDuration = Integer.MaxValue
        'TODO: ADD METHOD/FIELD IMAGES FROM VISUAL STUDIO 2013 ON THIS BITCH
        'Image 0 = function image, image 1 = field image
        auto.ImageList = New ImageList()
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("MethodIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("EstusIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("FieldIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("TypeIcon"))
        auto.ImageList.Images.Add(ConsoleWindow.EmbeddedImage("LuaIcon"))

        cons.AutoCMaxWidth = 128
        cons.AutoCMaxHeight = 24
    End Sub

    Private Sub AutoCompleteListSelecting(sender As Object, e As SelectingEventArgs)
        _AutoCompleteOpened = False
    End Sub

    Private Sub AutoCompleteListSelected(sender As Object, e As SelectedEventArgs)
        _AutoCompleteOpened = False
    End Sub

    Private Sub AutoCompleteListOpening(sender As Object, e As CancelEventArgs)
        _AutoCompleteOpened = True
    End Sub

    Private Sub RebuildAutoCompleteStr()
        _AutoCompleteString = String.Join(" ", AutoCompleteListBase.Concat(autoCompleteListUser).OrderBy(Function(x) x).ToArray())
        cons.SetKeywords(2, AutoCompleteString)
    End Sub

    Shared Sub New()
        Dim aclist As New List(Of String)
        aclist.AddRange(LUA_MATH_ETC.Split(" "))
        aclist.AddRange(LUA_SYS_ETC.Split(" "))
        aclist.AddRange(ScriptRes.autoCompleteFuncInfoByName.Keys)
        aclist.AddRange(ScriptRes.propTypes.Keys)
        aclist.AddRange(ScriptRes.autoCompleteAdditionalTypes.Select(Function(x) x.Name))
        _AutoCompleteListBase = aclist.OrderBy(Function(x) x).ToList()
    End Sub

    Private Sub InitStyle()
        cons.StyleResetDefault()

        cons.Styles(Style.Default).Font = "Consolas"
        cons.Styles(Style.Default).Size = 10

        cons.Lexer = Lexer.Lua
        cons.Styles(Style.Lua.Number).ForeColor = Color.Green
        cons.Styles(Style.Lua.LiteralString).ForeColor = Color.Green
        cons.Styles(Style.Lua.Word).Bold = True
        cons.Styles(Style.Lua.Word).ForeColor = Color.Blue
        cons.Styles(Style.Lua.Word).Weight = 7000
        cons.Styles(Style.Lua.String).ForeColor = Color.Violet
        cons.Styles(Style.Lua.Preprocessor).ForeColor = Color.Blue
        cons.Styles(Style.Lua.Operator).ForeColor = Color.Red
        cons.Styles(Style.Lua.Word2).ForeColor = Color.Violet
        cons.Styles(Style.Lua.Word3).ForeColor = Color.Teal
        cons.Styles(Style.Lua.Word4).ForeColor = Color.Red
        cons.Styles(Style.Lua.Word5).ForeColor = Color.DarkGoldenrod
        cons.Styles(Style.Lua.Label).ForeColor = Color.DarkCyan
        cons.Styles(Style.Lua.Comment).Bold = True
        cons.Styles(Style.Lua.Comment).ForeColor = Color.Green
        cons.Styles(Style.Lua.Comment).Italic = True
        cons.Styles(Style.Lua.Comment).Weight = 700
        cons.Styles(Style.Lua.CommentLine).Bold = True
        cons.Styles(Style.Lua.CommentLine).ForeColor = Color.Green
        cons.Styles(Style.Lua.CommentLine).Italic = True
        cons.Styles(Style.Lua.CommentLine).Weight = 700
        cons.Styles(Style.Lua.CommentDoc).Bold = True
        cons.Styles(Style.Lua.CommentDoc).ForeColor = Color.Green
        cons.Styles(Style.Lua.CommentDoc).Italic = True
        cons.Styles(Style.Lua.CommentDoc).Weight = 700
        cons.SetKeywords(0, "and break do else elseif end false for function goto if in local nil not or repeat return then true until while")
        cons.SetKeywords(1, "assert collectgarbage dofile error getmetatable ipairs load loadfile next pairs pcall print rawequal rawget rawset require select setmetatable tonumber tostring type xpcall")

        cons.AutomaticFold = AutomaticFold.Click
        'cons.CallTipSetPosition(True)
    End Sub

    Private Sub Console_TextChanged(sender As Object, e As EventArgs)
        Dim newLineNumberWidth = cons.Lines.Count.ToString().Length
        If Not (newLineNumberWidth = prevConsoleLineNumberWidth) Then
            cons.Margins(0).Width = cons.TextWidth(Style.LineNumber, New String("9", newLineNumberWidth + 1)) + 2
        End If

        currentDocumentModified = True

        prevConsoleLineNumberWidth = newLineNumberWidth
    End Sub

    Private Sub Console_CharAdded(sender As Object, e As CharAddedEventArgs)

        If e.Char = &H13 Then
            cons.DeleteRange(cons.CurrentPosition - 1, 1)
            Return
        End If

        ' Console.WriteLine("Char Added: " & ChrW(e.Char) & " (0x" & e.Char.ToString("X4") & ")")

        Dim currentPos = cons.CurrentPosition
        Dim wordStartPos = cons.WordStartPosition(currentPos, True)
        Dim lengthEntered = currentPos - wordStartPos

        If (lengthEntered > 0 And Not cons.AutoCActive) Then
            'TODO: LEX SHIT AND UPDATE AUTOCOMPLETE
            'cons.AutoCShow(lengthEntered, autoCompleteString)
        End If

        If ChrW(e.Char) = " "c Then
            cons.Colorize(consoleLastSyntaxHighlightEndPos, currentPos)

            consoleLastSyntaxHighlightEndPos = currentPos

        End If
    End Sub

    Private Sub Console_Delete(sender As Object, e As ModificationEventArgs)
        If Not String.IsNullOrWhiteSpace(e.Text.Trim()) Then
            'TODO: LEX SHIT AND UPDATE AUTOCOMPLETE
        End If
    End Sub

    'Private Function CheckIfNextWordIsToken(token As String)
    '    Dim remainingWordPieces = cons.Text.Substring(cons.CurrentPosition).Trim().Split(" ")
    '    If remainingWordPieces.Length = 0 Then
    '        Return False
    '    Else
    '        Return remainingWordPieces.First() = token
    '    End If
    'End Function

    'Private Function InsertClosingTokenAndIndent(closingToken As String) As Boolean

    '    If CheckIfNextWordIsToken(closingToken) Then
    '        GoToNextLineAndIndentDeeper()
    '    Else
    '        GoToNextLine_RetainingTheIndentLevelInTheProcess()
    '        GoToNextLine_RetainingTheIndentLevelInTheProcess()
    '        Dim indentLevel = cons.Lines(cons.CurrentLine).Length
    '        If (closingToken.Contains(vbCrLf)) Then
    '            Throw New Exception("Muh formatting")
    '        End If
    '        cons.AddText(closingToken)
    '        cons.CurrentPosition = cons.Lines(cons.CurrentLine - 1).Position + indentLevel
    '        cons.AddText(New String(" "c, LUA_INDENT_WIDTH))
    '    End If

    '    Return True 'Literally only here to make the code easier to use in one specific case lol
    'End Function

    Private Function GoToNextLineAndIndentDeeper() As Boolean
        GoToNextLine_RetainingTheIndentLevelInTheProcess()
        cons.AddText(New String(" "c, LUA_INDENT_WIDTH))

        Return True
    End Function

    Private Function LexDatBitchOnPressEnterKey() As Boolean
        Dim tokens = cons.Lines(cons.CurrentLine).Text.Trim.Split(" ")
        If tokens.Length >= 1 Then
            Dim first = tokens.First()

            If first = "else" Then
                GoToNextLineAndIndentDeeper()
                Return True
            End If

            If tokens.Length >= 2 Then
                Dim last = tokens.Last()

                If first = "while" And last = "do" Then
                    Return GoToNextLineAndIndentDeeper() 'Return InsertClosingTokenAndIndent("end")
                ElseIf first = "if" And last = "then" Then
                    Return GoToNextLineAndIndentDeeper() 'Return InsertClosingTokenAndIndent("end")
                ElseIf first = "for" And last = "do" Then
                    Return GoToNextLineAndIndentDeeper() 'Return InsertClosingTokenAndIndent("end")
                ElseIf first = "elseif" And last = "then" Then
                    Return GoToNextLineAndIndentDeeper() 'Return GoToNextLineAndIndentDeeper()
                ElseIf first = "function" And last.TrimEnd.Last = ")"c Then
                    Return GoToNextLineAndIndentDeeper() 'Return InsertClosingTokenAndIndent("end")
                End If
            Else
                If first = "repeat" Then
                    Return GoToNextLineAndIndentDeeper() 'Return InsertClosingTokenAndIndent("until --Condition")
                End If
            End If
        End If
        Return False
    End Function

    Private Function GetLineIndent(index As Integer) As Integer
        Dim lineText = cons.Lines(index).Text
        Dim trim = lineText.TrimStart
        Return If(Not String.IsNullOrWhiteSpace(trim),
            lineText.IndexOf(lineText.TrimStart()),
            cons.Lines(index).Text.Replace(vbCrLf, "").Length)
    End Function

    Public Sub GoToNextLine_RetainingTheIndentLevelInTheProcess()
        Dim currentLineIndent = GetLineIndent(cons.CurrentLine)
        cons.AddText(vbCrLf)
        cons.AddText(New String(" "c, currentLineIndent))
    End Sub

    'Private Function CheckIfThirdEnterKeyPressInARow() As Boolean
    '    Dim curLine = cons.CurrentLine
    '    If curLine = 0 Then
    '        Return False 'BRO ur literally on the first line how would it be ur second enter key press
    '    End If

    '    Dim currentLineIndent = GetLineIndent(curLine)
    '    Dim previousLineIndent = GetLineIndent(curLine - 1)
    '    Dim currentLineEmpty = String.IsNullOrWhiteSpace(cons.Lines(curLine).Text)
    '    Dim previousLineEmpty = String.IsNullOrWhiteSpace(cons.Lines(curLine - 1).Text)
    '    If (currentLineIndent = previousLineIndent) And currentLineEmpty And previousLineEmpty Then
    '        Return True
    '    End If

    '    Return False
    'End Function






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
            ElseIf e.KeyCode = Keys.D0 Then
                cons.Zoom = 0
            ElseIf e.KeyCode = Keys.Oemplus Then
                If cons.Zoom <= 19 Then cons.ZoomIn()
            ElseIf e.KeyCode = Keys.OemMinus Then
                ' Seemed to stop zooming out at -8, then continue to subtract the value all the way to -10, making the next
                ' few zoom-ins not actually zoom in until it gets above -8
                If cons.Zoom >= -7 Then cons.ZoomOut()
            Else
                Return
            End If
        ElseIf Not e.Control AndAlso e.Shift AndAlso Not e.Alt Then 'SHIFT + (KEY)
            If AutoCompleteOpenedBuffer = 0 AndAlso e.KeyCode = Keys.Tab Then
                UnIndentSelection()
            Else
                Return
            End If
        ElseIf Not e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then '(KEY)
            If e.KeyCode = Keys.Return Then
                'Dim unIndentModeOrWhatever = CheckIfThirdEnterKeyPressInARow()

                'If dat bitch aint lexed an shiet, we need to do a boring thing
                If Not LexDatBitchOnPressEnterKey() Then
                    GoToNextLine_RetainingTheIndentLevelInTheProcess()
                    cons.ScrollCaret()
                    'If unIndentModeOrWhatever Then
                    '    UnIndent()
                    'End If
                End If
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
                If (luaRunner.State = LuaRunnerState.Stopped Or
                    luaRunner.State = LuaRunnerState.Finished) Then
                    luaRunner.StartExecution(cons.Text)

                ElseIf (luaRunner.State = LuaRunnerState.Running) Then
                    luaRunner.StopExecution()
                End If
            ElseIf e.KeyCode = Keys.F7 Then
                Game.InitHook()
                ParentScriptTab.ParentConsoleWindow.UpdateToolStrip()
            ElseIf e.KeyCode = Keys.Tab Then
                If AutoCompleteOpenedBuffer = 0 Then
                    If cons.LineFromPosition(cons.SelectionStart) <>
                            cons.LineFromPosition(cons.SelectionEnd) Then
                        IndentSelection()
                    Else
                        cons.AddText(New String(" ", GetDistToNextTabStop()))
                    End If
                End If
            Else
                Return
            End If
        Else
            Return
        End If

        e.Handled = True
        e.SuppressKeyPress = True
    End Sub

    Private Sub ResetConsoleSelection()
        cons.SelectionStart = cons.CurrentPosition
        cons.SelectionEnd = cons.CurrentPosition
    End Sub

    Public Sub IndentSelection()
        DentSelection(False)
    End Sub

    Public Sub UnIndentSelection()
        DentSelection(True)
    End Sub

    Private Sub DentSelection(un As Boolean)
        Dim oldSelectionStart = cons.SelectionStart
        Dim oldSelectionEnd = cons.SelectionEnd

        Dim firstLine = cons.LineFromPosition(cons.SelectionStart)
        Dim lastLine = cons.LineFromPosition(cons.SelectionEnd)

        Dim selectionStartShift = 0
        Dim selectionEndShift = 0

        For i = firstLine To lastLine
            Dim shiftAmount = If(un, UnIndentLine(i), IndentLine(i))
            If i = firstLine Then
                selectionStartShift = shiftAmount
            ElseIf i = lastLine Then
                selectionEndShift = shiftAmount
            End If
        Next

        cons.SelectionStart = oldSelectionStart
        cons.SelectionEnd = oldSelectionEnd

        cons.SelectionStart += (selectionStartShift * If(un, -1, 1))
        cons.SelectionEnd += (selectionEndShift * If(un, -1, 1))
    End Sub

    Private Function IndentLine(line As Integer) As Integer
        Dim lineIndent = GetLineIndent(line)
        Dim returnToPos = cons.CurrentPosition

        Dim lineStart = cons.Lines(line).Position
        cons.CurrentPosition = lineStart + lineIndent
        Dim shiftDistance = GetDistToNextTabStop()
        cons.AddText(New String(" "c, shiftDistance))

        cons.CurrentPosition = returnToPos
        ResetConsoleSelection()
        Return shiftDistance
    End Function

    Private Function UnIndentLine(line As Integer) As Integer
        Dim lineIndent = GetLineIndent(line)

        If lineIndent = 0 Then
            Return 0
        End If

        Dim returnToPos = cons.CurrentPosition

        Dim lineStart = cons.Lines(line).Position
        cons.CurrentPosition = lineStart + lineIndent
        ' Don't delete stuff on a previous line
        Dim shiftDistance = Math.Min(GetDistToPrevTabStop(), cons.CurrentPosition - lineStart)
        cons.SelectionStart = cons.CurrentPosition - shiftDistance
        cons.SelectionEnd = cons.CurrentPosition
        cons.ReplaceSelection("")

        cons.CurrentPosition = returnToPos
        ResetConsoleSelection()
        Return shiftDistance
    End Function

    Private Function GetCurrentColumn() As Integer
        Return cons.CurrentPosition - cons.Lines(cons.CurrentLine).Position
    End Function

    Private Function GetDistToPrevTabStop() As Integer
        Dim moduloValueThing = (GetCurrentColumn() Mod LUA_INDENT_WIDTH)
        Return If(moduloValueThing = 0, LUA_INDENT_WIDTH, moduloValueThing)
    End Function

    Private Function GetDistToNextTabStop() As Integer
        Dim moduloValueThing = (LUA_INDENT_WIDTH - (GetCurrentColumn() Mod LUA_INDENT_WIDTH))
        Return If(moduloValueThing = 0, LUA_INDENT_WIDTH, moduloValueThing)
    End Function

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
        currentDocument = New FileInfo(fileName)
        cons.Text = File.ReadAllText(currentDocument.FullName)
        currentDocumentModified = False
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

        File.WriteAllText(currentDocument.FullName, cons.Text)
        currentDocumentModified = False
        UpdateTabText(True, False)
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
            Return True
        End If
        Return False
    End Function

    Private Async Function ConsoleLoadAsync(filePath As String) As Task

        Dim fi As New FileInfo(filePath)

        Try

            Dim loader = CType(cons.Invoke(Function() As ILoader
                                               Return cons.CreateLoader(256)
                                           End Function), ILoader)

            If loader Is Nothing Then
                Throw New ApplicationException("Unable to create Scintilla document ILoader (ask Meowmaritus he'll know what it means lol).")
            End If

            Dim cts = New Threading.CancellationTokenSource()
            Dim document = Await ConsoleDoLoadFileAsync(loader, filePath, cts.Token)

            cons.Invoke(Sub(doc)
                            cons.Document = doc
                            cons.ReleaseDocument(doc)
                        End Sub, document)

        Catch ex As OperationCanceledException


        Catch ex As Exception
            MessageBox.Show(Me, "There was an error loading '" & fi.Name & "':" & vbCrLf & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Function

    Private Async Function ConsoleDoLoadFileAsync(loader As ILoader, path As String, cancellationToken As Threading.CancellationToken) As Task(Of Document)
        Try
            Using file = New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, True)
                Using reader = New StreamReader(file)


                    Dim count = 0

                    Dim buffer As Char()
                    ReDim buffer(4096)

                    While ((count = Await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(False)) > 0)
                        ' Check for cancellation
                        cancellationToken.ThrowIfCancellationRequested()

                        ' Add the data to the document
                        If Not loader.AddData(buffer, count) Then
                            Throw New IOException("The data could not be added to the loader.")
                        End If

                    End While

                    Return loader.ConvertToDocument()


                End Using
            End Using
        Catch
            loader.Release()
            Throw
        End Try
    End Function


End Class
