Public Class ConsoleWindow

    Dim consHandler As ConsoleHandler

    Private Sub ConsoleWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        consHandler = New ConsoleHandler(Me, scConsole, txtConsoleResult, tsbtnRun, tsbtnStop, tsbtnSave, tsbtnNew, tsbtnOpen, tsmiHelp, tsHook)
    End Sub

    Private Sub tsbtnNew_Click(sender As Object, e As EventArgs) Handles tsbtnNew.Click
        consHandler.NewDoc()
    End Sub

    Private Sub tsbtnOpen_Click(sender As Object, e As EventArgs) Handles tsbtnOpen.Click
        consHandler.Open()
    End Sub

    Private Sub tsbtnSave_Click(sender As Object, e As EventArgs) Handles tsbtnSave.Click
        consHandler.Save()
    End Sub

    Private Sub tsNew_Click(sender As Object, e As EventArgs) Handles tsNew.Click
        consHandler.NewDoc()
    End Sub

    Private Sub tsOpen_Click(sender As Object, e As EventArgs) Handles tsOpen.Click
        consHandler.Open()
    End Sub

    Private Sub tsSave_Click(sender As Object, e As EventArgs) Handles tsSave.Click
        consHandler.Save()
    End Sub

    Private Sub tsSaveAs_Click(sender As Object, e As EventArgs) Handles tsSaveAs.Click
        consHandler.SaveAs()
    End Sub

    Private Sub tsExit_Click(sender As Object, e As EventArgs) Handles tsExit.Click
        If consHandler.PromptSaveChanges() Then
            Close()
        End If
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Hook.InitHook()
    End Sub
End Class