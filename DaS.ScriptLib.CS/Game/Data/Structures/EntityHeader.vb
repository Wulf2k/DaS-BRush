Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    <Flags>
    Public Enum EntityHeaderFlagsA As Byte
        None = &H0
        Disabled = &H1
        PlayerHide = &H4
    End Enum

    Public Class EntityHeader

        Public Const Size As Integer = &H20

        Public ReadOnly Pointer As Integer

        Public Sub New(ptr As Integer)
            Pointer = ptr
        End Sub

        Public Sub CopyFrom(other As EntityHeader)
            'Write ENTIRE STRUCT properly
            WBytes(Pointer, RBytes(other.Pointer, Size))
        End Sub

        Public Property EntityPtr As Integer
            Get
                Return RInt32(Pointer + &H0)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H0, value)
            End Set
        End Property

        Public ReadOnly Property Entity As Entity
            Get
                Return New Entity(Function() EntityPtr)
            End Get
        End Property

        Public Property CloneValue As Integer
            Get
                Return RInt32(Pointer + &H8)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H8, value)
            End Set
        End Property

        Public ReadOnly Property UnknownPtr1 As Integer
            Get
                Return RInt32(Pointer + &H10)
            End Get
        End Property

        Public Property FlagsA As EntityHeaderFlagsA
            Get
                Return CType(RByte(Pointer + &H14), EntityHeaderFlagsA)
            End Get
            Set(value As EntityHeaderFlagsA)
                WByte(Pointer + &H14, CType(value, Byte))
            End Set

        End Property

        Public Property LocationPtr As Integer
            Get
                Return RInt32(Pointer + &H18)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H18, value)
            End Set
        End Property

        Public Property Location As EntityLocation
            Get
                Return New EntityLocation(LocationPtr)
            End Get
            Set(value As EntityLocation)
                Dim fuckVb = New EntityLocation(LocationPtr)
                fuckVb.CopyFrom(value)
            End Set
        End Property

        Public ReadOnly Property UnknownPtr2 As Integer
            Get
                Return RInt32(Pointer + &H1C)
            End Get
        End Property

        'TODO: See if writeable, also see wtf it even is lol
        Public ReadOnly Property DeadFlag As Boolean
            Get
                Return RBool(UnknownPtr2 + &H14)
            End Get
        End Property
#Region "Flags"
        Public Function GetFlagA(flg As EntityHeaderFlagsA) As Boolean
            Return FlagsA.HasFlag(flg)
        End Function

        Public Sub SetFlagA(flg As EntityHeaderFlagsA, state As Boolean)
            If state Then
                FlagsA = FlagsA Or flg
            Else
                FlagsA = FlagsA And (Not flg)
            End If
        End Sub

        Public Property PlayerHide As Boolean
            Get
                Return GetFlagA(EntityHeaderFlagsA.PlayerHide)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityHeaderFlagsA.PlayerHide, value)
            End Set
        End Property

        Public Property Disabled As Boolean
            Get
                Return GetFlagA(EntityHeaderFlagsA.Disabled)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityHeaderFlagsA.Disabled, value)
            End Set
        End Property
#End Region


    End Class

End Namespace
