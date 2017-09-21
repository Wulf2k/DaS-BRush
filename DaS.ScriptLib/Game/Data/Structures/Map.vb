Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class Map

        Public Const BasePointer As Integer = &H137D644

        Public Shared ReadOnly Property Pointer As Integer
            Get
                Return RInt32(BasePointer)
            End Get
        End Property

        Public Shared ReadOnly Property CharData1Address As Integer
            Get
                Return RInt32(Pointer + &H3C)
            End Get
        End Property

        Public Shared ReadOnly Property MapEntryCount As Integer
            Get
                Return RInt32(Pointer + &H70)
            End Get
        End Property

        Public Shared Function GetEntries() As MapEntry()
            Dim result(MapEntryCount - 1) As MapEntry
            For i = 0 To result.Length - 1
                result(i) = New MapEntry(Pointer + &H74 + (4 * i))
            Next
            Return result
        End Function

    End Class

End Namespace