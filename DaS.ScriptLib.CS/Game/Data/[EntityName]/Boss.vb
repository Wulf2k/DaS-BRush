Imports System.Collections.ObjectModel

Namespace Game.Data
    Partial Public Class EntityName

        Public Shared ReadOnly Property Boss As ReadOnlyDictionary(Of EventFlag.Boss, String)

        Private Shared Sub InitBoss()
            Dim d As New Dictionary(Of EventFlag.Boss, String)

            d.Add(EventFlag.Boss.Artorias, "Knight Artorias")

            _Boss = New ReadOnlyDictionary(Of EventFlag.Boss, String)(d)
        End Sub

    End Class
End Namespace
