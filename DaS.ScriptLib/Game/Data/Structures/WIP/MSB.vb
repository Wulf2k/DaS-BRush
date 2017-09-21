Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class MSB

        Public Shared ReadOnly Property Pointer As Integer
            Get
                Return RInt32(&H13785A0)
            End Get
        End Property

        Public Shared ReadOnly Property FirstEntryPointer As Integer
            Get
                Return RInt32(Pointer + &HC)
            End Get
        End Property

        Public Shared ReadOnly Property Entries(i As Integer)
            Get
                Return "fuck off vb"
            End Get
        End Property

        Private Shared Sub ReadEntries()



        End Sub

    End Class

End Namespace