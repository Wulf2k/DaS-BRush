Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class EntityAIController

        Public ReadOnly Pointer As Integer

        Public Sub New(ptr As Integer)
            Pointer = ptr
        End Sub

        Public Sub CopyFrom(other As EntityAIController)
            'TODO
        End Sub

        Public Property EntityPtr As Integer
            Get
                Return RInt32(Pointer + &H14)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H14, value)
            End Set
        End Property

        Public ReadOnly Property Entity As Entity
            Get
                Return New Entity(Function() EntityPtr)
            End Get
        End Property

        Public Property AIScript As Integer
            Get
                Return RInt32(Pointer + &H78)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H78, value)
            End Set
        End Property

        Public Property AnimationID As Integer
            Get
                Return RInt32(Pointer + &H9C)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H9C, value)
            End Set
        End Property

        Public Property AIScript2 As Integer
            Get
                Return RInt32(Pointer + &H80)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H80, value)
            End Set
        End Property

        Public Property PosX As Single
            Get
                Return RFloat(Pointer + &H1E0)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1E0, value)
            End Set
        End Property

        Public Property PosY As Single
            Get
                Return RFloat(Pointer + &H1E4)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1E4, value)
            End Set
        End Property

        Public Property PosZ As Single
            Get
                Return RFloat(Pointer + &H1E8)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1E8, value)
            End Set
        End Property

        Public Property RotZ As Single
            Get
                Return RFloat(Pointer + &H1E8)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1E8, value)
            End Set
        End Property

        Public Property RotY As Single
            Get
                Return RFloat(Pointer + &H1F0)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1F0, value)
            End Set
        End Property

        Public Property RotX As Single
            Get
                Return RFloat(Pointer + &H1F4)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1F4, value)
            End Set
        End Property

        Public Property AnimationID2 As Integer
            Get
                Return RInt32(Pointer + &H208)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H208, value)
            End Set
        End Property

    End Class

End Namespace
