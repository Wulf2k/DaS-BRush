Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class EntityController

        Public ReadOnly Property Pointer As Integer
            Get
                Return _getOffset()
            End Get
        End Property

        Private _getOffset As Func(Of Integer)

        Public Sub New(getOffsetFunc As Func(Of Integer))
            _getOffset = getOffsetFunc
        End Sub

        Public Property MoveX As Single
            Get
                Return RFloat(Pointer + &H10)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H10, value)
            End Set
        End Property

        Public Property MoveY As Single
            Get
                Return RFloat(Pointer + &H18)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H18, value)
            End Set
        End Property

        Public Property CamRotSpeedH As Single
            Get
                Return RFloat(Pointer + &H50)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H50, value)
            End Set
        End Property

        Public Property CamRotSpeedV As Single
            Get
                Return RFloat(Pointer + &H54)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H54, value)
            End Set
        End Property

        Public Property CamRotH As Single
            Get
                Return RFloat(Pointer + &H60)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H60, value)
            End Set
        End Property

        Public Property CamRotV As Single
            Get
                Return RFloat(Pointer + &H64)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H64, value)
            End Set
        End Property

        Public Property R1Held_Sometimes As Boolean
            Get
                Return RBool(Pointer + &H10)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H10, value)
            End Set
        End Property

        Public Property L2OrR1Held_Sometimes As Boolean
            Get
                Return RBool(Pointer + &H85)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H85, value)
            End Set
        End Property

        Public Property RMouseHeld As Boolean
            Get
                Return RBool(Pointer + &H89)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H89, value)
            End Set
        End Property

        Public Property R1Held As Boolean
            Get
                Return RBool(Pointer + &H8B)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H8B, value)
            End Set
        End Property

        Public Property XHeld As Boolean
            Get
                Return RBool(Pointer + &H92)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H92, value)
            End Set
        End Property

        Public Property L1Held As Boolean
            Get
                Return RBool(Pointer + &H97)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H97, value)
            End Set
        End Property

        Public Property L2OrTabHeld As Boolean
            Get
                Return RBool(Pointer + &H98)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H98, value)
            End Set
        End Property

        Public Property L1Held2 As Boolean
            Get
                Return RBool(Pointer + &HB7)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HB7, value)
            End Set
        End Property

        Public Property R1HeldOrCircleTapped As Boolean
            Get
                Return RBool(Pointer + &HB9)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HB9, value)
            End Set
        End Property

        Public Property RMouseOrR2Held As Boolean
            Get
                Return RBool(Pointer + &HBE)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HBE, value)
            End Set
        End Property

        Public Property R1OrLMouseHeld As Boolean
            Get
                Return RBool(Pointer + &HC0)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HC0, value)
            End Set
        End Property

        Public Property L2Held As Boolean
            Get
                Return RBool(Pointer + &HC2)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HC2, value)
            End Set
        End Property

        Public Property CircleTapped As Boolean
            Get
                Return RBool(Pointer + &HC2)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HC2, value)
            End Set
        End Property

        Public Property XHeld2 As Boolean
            Get
                Return RBool(Pointer + &HC7)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HC7, value)
            End Set
        End Property

        Public Property L1Held3 As Boolean
            Get
                Return RBool(Pointer + &HCC)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HCC, value)
            End Set
        End Property

        Public Property L2Held2 As Boolean
            Get
                Return RBool(Pointer + &HCD)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HCD, value)
            End Set
        End Property

        Public Property L1Held4 As Boolean
            Get
                Return RBool(Pointer + &HEC)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &HEC, value)
            End Set
        End Property

        Public Property SecondsR1Held As Single
            Get
                Return RFloat(Pointer + &HF0)
            End Get
            Set(value As Single)
                WFloat(Pointer + &HF0, value)
            End Set
        End Property

        Public Property SecondsGuarding As Single
            Get
                Return RFloat(Pointer + &HF4)
            End Get
            Set(value As Single)
                WFloat(Pointer + &HF4, value)
            End Set
        End Property

        Public Property SecondsR2Held As Single
            Get
                Return RFloat(Pointer + &H104)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H104, value)
            End Set
        End Property

        Public Property SecondsR1Held2 As Single
            Get
                Return RFloat(Pointer + &H10C)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H10C, value)
            End Set
        End Property

        Public Property SecondsL1Held As Single
            Get
                Return RFloat(Pointer + &H13C)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H13C, value)
            End Set
        End Property

        Public Property SecondsGuarding2 As Single
            Get
                Return RFloat(Pointer + &H144)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H144, value)
            End Set
        End Property

        Public Property AnimationID As Integer
            Get
                Return RInt32(Pointer + &H1E8)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H1E8, value)
            End Set
        End Property

        Public Property AIControllerPtr As Integer
            Get
                Return RInt32(Pointer + &H230)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &H230, value)
            End Set
        End Property

        Public Property AIController As EntityAIController
            Get
                Return New EntityAIController(AIControllerPtr)
            End Get
            Set(value As EntityAIController)
                AIController.CopyFrom(value)
            End Set
        End Property

    End Class

End Namespace
