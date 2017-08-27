Public Class ScriptEditorTab
    Inherits TabPage

    Public ReadOnly ConsHandler As ConsoleHandler
    Public ReadOnly ParentConsoleWindow As ConsoleWindow

    Public Sub New(parent As ConsoleWindow)
        ParentConsoleWindow = parent
        ConsHandler = New ConsoleHandler(Me)
    End Sub

End Class