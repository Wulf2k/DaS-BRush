Imports System.Runtime.CompilerServices

Module WinFormsExtensions

    <Extension()>
    Public Sub FuckOff(ByRef numBox As NumericUpDown, val As Decimal)
        If Not numBox.Focused Then
            numBox.Value = val
        End If
    End Sub

End Module