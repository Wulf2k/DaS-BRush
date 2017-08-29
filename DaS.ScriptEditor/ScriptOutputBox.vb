Imports DaS.ScriptLib

Public Class ScriptOutputBox
    Inherits RichTextBox

    Private isAddingLine As Boolean = False

    Public Event OnAddLine(line As String)
    Public Event OnClearAllLines()

    Public Sub InitEvents()
        AddHandler Dbg.OnPrint, AddressOf Dbg_OnPrint
        AddHandler Dbg.OnPrintClearAll, AddressOf Dbg_OnPrintClearAll
    End Sub

    Public Property AutoScroll As Boolean = True

    Private Sub Dbg_OnPrintClearAll()
        ResetText()

        RaiseEvent OnClearAllLines()
    End Sub

    Private Sub DoActionAndReturnToSelection(act As Action(Of ScriptOutputBox))

        Dim oldSelStart = SelectionStart
        Dim oldSelLength = SelectionLength

        act.Invoke(Me)

        SelectionStart = oldSelStart
        SelectionLength = oldSelLength

    End Sub

    Private Sub DoActionOnSpecificSelection(selStart As Integer, selEnd As Integer, act As Action(Of ScriptOutputBox))
        DoActionAndReturnToSelection(
        Sub(o)
            o.SelectionStart = selStart
            o.SelectionLength = selEnd - selStart

            act(o)
        End Sub)
    End Sub

    Private Sub SelDeleteRange(rangeStart As Integer, rangeEnd As Integer)
        [Select](rangeStart, rangeEnd - rangeStart)
        SelectedText = ""
    End Sub

    Private Sub SelAddTextWithSpecificRtf(ByVal text As String, Optional col As Color? = Nothing, Optional bold As Boolean? = Nothing, Optional italic As Boolean? = Nothing, Optional siz As Single? = Nothing)
        Dim style As FontStyle = Font.Style

        If bold.HasValue Then
            style = style Or FontStyle.Bold

            If Not bold.Value Then
                style = style Xor FontStyle.Bold
            End If
        End If

        If italic.HasValue Then
            style = style Or FontStyle.Italic

            If Not italic.Value Then
                style = style Xor FontStyle.Italic
            End If
        End If

        SelAddTextWithSpecificRtf(text, If(col, ForeColor), New Font(Font.FontFamily, If(siz, Font.Size), style))
    End Sub

    Private Sub SelAddTextWithSpecificRtf(ByVal text As String, ByVal col As Color, ByVal fnt As Font)
        If (TextLength + text.Length) > MaxLength Then
            SelDeleteRange(0, (TextLength + text.Length) - MaxLength)
            SelDeleteRange(0, text.IndexOf(vbCrLf) + vbCrLf.Length)
        End If

        Dim selStart = TextLength
        AppendText(text)
        Dim selEnd = TextLength

        [Select](selStart, selEnd - selStart)

        SelectionFont = fnt
        SelectionColor = col
    End Sub

    Private Sub Dbg_OnPrint(time As Date, type As DbgPrintType, txt As String)
        Invoke(
        Sub()
            DoActionAndReturnToSelection(
            Sub(o)
                Dim printCol As Color = ForeColor
                Dim printBold As Boolean = False
                Dim printItalic As Boolean = False

                If type = DbgPrintType.Info Then
                    printCol = Color.Blue
                ElseIf type = DbgPrintType.Warn Then
                    printCol = Color.DarkGoldenrod
                    printItalic = True
                ElseIf type = DbgPrintType.Err Then
                    printCol = Color.Red
                    printBold = True
                End If

                Dim longTime = time.ToLongTimeString()
                Dim timeWithoutAmPm = longTime.Substring(0, longTime.Length - 3)
                Dim timeAmPm = longTime.Substring(longTime.Length - 2, 2)

                o.SelAddTextWithSpecificRtf("[" & timeWithoutAmPm & "." & time.Millisecond.ToString("000") & " " & timeAmPm & " - " & type.ToString() & "] >  ", printCol, printBold, printItalic, o.Font.Size * 0.75)
                o.SelAddTextWithSpecificRtf(txt & vbCrLf, printCol, printBold, printItalic)
            End Sub
            )

            If AutoScroll Then
                SelectionStart = TextLength - 1
                ScrollToCaret()
            End If

            RaiseEvent OnAddLine(Text)
        End Sub
        )

    End Sub

End Class
