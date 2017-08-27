Imports System.Windows.Forms

Public Enum DbgPrintType
    Normal
    Info
    Warn
    Err
End Enum

Public Class Dbg

    Public Shared Event OnPrintClearAll()

    Public Shared Event OnPrint(time As Date, type As DbgPrintType, text As String)

    Public Shared Sub Print(text As String)
        RaiseEvent OnPrint(DateTime.Now, DbgPrintType.Normal, text)
    End Sub

    Public Shared Sub PrintClearAll()
        RaiseEvent OnPrintClearAll()
    End Sub

    Public Shared Sub PrintInfo(text As String)
        RaiseEvent OnPrint(DateTime.Now, DbgPrintType.Info, text)
    End Sub

    Public Shared Sub PrintWarn(text As String)
        RaiseEvent OnPrint(DateTime.Now, DbgPrintType.Warn, text)
    End Sub

    Public Shared Sub PrintErr(text As String)
        RaiseEvent OnPrint(DateTime.Now, DbgPrintType.Err, text)
    End Sub

    Private Shared lock_Dump As New Object()
    Private Shared debugDumpFileName As String = "Debug_Dump.txt"

    'Public Shared Sub PrintArray(arr As Object())
    '    Console.WriteLine(arr.ToString() & "{" & String.Join(", ", arr.Select(Function(x) x.ToString)) & "}")
    'End Sub

    'Public Shared Sub PrintArray(arr As Integer())
    '    Console.WriteLine(arr.ToString() & "{" & String.Join(", ", arr.Select(Function(x) x.ToString)) & "}")
    'End Sub

    Public Shared Sub Dump(ByVal text As String)
        SyncLock lock_Dump
            Dim time = System.DateTime.Now
            Dim fullTimeStr = time.ToLongTimeString()
            Dim ampm = fullTimeStr.Substring(fullTimeStr.Length - 3)
            fullTimeStr = fullTimeStr.Substring(0, fullTimeStr.Length - 3)
            Dim ms = time.Millisecond.ToString("000")

            text = "[" & fullTimeStr & "." & ms & ampm & "] " & text
            If Not IO.File.Exists(debugDumpFileName) Then
                Using newFile = IO.File.CreateText(debugDumpFileName)
                    newFile.WriteLine(text)
                End Using
            Else
                Using sw As New IO.StreamWriter(debugDumpFileName, True)
                    sw.WriteLine(text)
                End Using
            End If
        End SyncLock
    End Sub

    Public Shared Sub Dump(format As String, args As Object())
        Dump(String.Format(format, args))
    End Sub

    Public Shared Function PopupErr(msg As String) As DialogResult
        Return Popup(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Function

    Public Shared Function PopupOk(msg As String) As DialogResult
        Return Popup(msg, "(Untitled)")
    End Function

    Public Shared Function PopupOk(msg As String, title As String) As DialogResult
        Return Popup(msg, title)
    End Function

    Public Shared Function Popup(msg As String) As DialogResult
        Return Popup(msg, "(Untitled)", MessageBoxButtons.OK, MessageBoxIcon.None)
    End Function

    Public Shared Function Popup(msg As String, title As String) As DialogResult
        Return Popup(msg, title, MessageBoxButtons.OK, MessageBoxIcon.None)
    End Function

    Public Shared Function Popup(msg As String, title As String, buttons As MessageBoxButtons) As DialogResult
        Return Popup(msg, title, buttons, MessageBoxIcon.None)
    End Function

    Public Shared Function Popup(msg As String,
                                 title As String,
                                 buttons As MessageBoxButtons,
                                 icon As MessageBoxIcon) As DialogResult
        Return MessageBox.Show(msg, title, buttons, icon)
    End Function

End Class