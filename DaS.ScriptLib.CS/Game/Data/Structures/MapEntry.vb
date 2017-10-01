Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class MapEntry

        Public ReadOnly Pointer As Integer

        Public Sub New(ptr As Integer)
            Pointer = ptr
        End Sub

        Public ReadOnly Property PointerToBlockAndArea As Integer
            Get
                Return RInt32(Pointer + &H4)
            End Get
        End Property

        Public ReadOnly Property Block As Byte
            Get
                Return RByte(PointerToBlockAndArea + &H6)
            End Get
        End Property

        Public ReadOnly Property Area As Byte
            Get
                Return RByte(PointerToBlockAndArea + &H7)
            End Get
        End Property

        Public ReadOnly Property EntityCount As Integer
            Get
                Return RInt32(Pointer + &H3C)
            End Get
        End Property

        Public ReadOnly Property StartOfEntityStruct As Integer
            Get
                Return RInt32(Pointer + &H40)
            End Get
        End Property

        Public Function GetEntityHeaders() As EntityHeader()
            Dim result(EntityCount - 1) As EntityHeader

            For i = 0 To result.Length - 1
                result(i) = New EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i))
            Next

            Return result
        End Function

        Public Function GetEntities() As Entity()
            Dim result(EntityCount - 1) As Entity

            For i = 0 To EntityCount - 1
                result(i) = New EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i)).Entity
            Next

            Return result
        End Function

        Public Function GetEntityLocations() As EntityLocation()
            Dim result(EntityCount - 1) As EntityLocation

            For i = 0 To EntityCount - 1
                result(i) = New EntityHeader(StartOfEntityStruct + (EntityHeader.Size * i)).Location
            Next

            Return result
        End Function

    End Class

End Namespace
