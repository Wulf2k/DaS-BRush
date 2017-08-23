Imports System.Runtime.CompilerServices

Module ScriptFormattingExtensions

    <Extension()>
    Public Function RemoveDuplicates(ByVal txt As String, searchKey As String, Optional removePaddingBetweenKeys As Boolean = True) As String
        Return String.Join(searchKey, (From piece In txt.Split(searchKey)
                                       Select p = If(removePaddingBetweenKeys, piece.Trim, piece)
                                       Where Not String.IsNullOrWhiteSpace(p)).ToArray)
    End Function

    <Extension()>
    Public Function FormatText(txt As String) As String
        Return txt.Trim.RemoveDuplicates(" ")
    End Function

End Module

Module WinFormsExtensions

    <Extension()>
    Public Sub FuckOff(ByRef numBox As NumericUpDown, val As Decimal)
        If Not numBox.Focused Then
            numBox.Value = val
        End If
    End Sub

End Module