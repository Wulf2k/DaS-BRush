Namespace Game.Data.Structures

    'UNDER CONSTRUCTION
    Public Class MSBEntry

        Public ReadOnly Pointer As Integer

        Public Sub New(i As Integer)
            Pointer = MSB.FirstEntryPointer + (4 * i)
        End Sub

    End Class

End Namespace
