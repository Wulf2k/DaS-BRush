Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    Public Class EntityLocation

        Public ReadOnly Pointer As Integer

        Public Sub New(ptr As Integer)
            Pointer = ptr
        End Sub

        Public Sub CopyFrom(other As EntityLocation)
            Dim oAngle = other.Angle
            Dim oX = other.X
            Dim oY = other.Y
            Dim oZ = other.Z

            Angle = oAngle
            X = oX
            Y = oY
            Z = oZ
        End Sub

        Public Property Heading As Double
            Get
                Return (RFloat(Pointer + &H4) / Math.PI * 180) + 180
            End Get
            Set(value As Double)
                WFloat(Pointer + &H4, CType(value * Math.PI / 180, Single) - CType(Math.PI, Single))
            End Set
        End Property

        Public Property Angle As Double
            Get
                Return RFloat(Pointer + &H4)
            End Get
            Set(value As Double)
                WFloat(Pointer + &H4, CType(value, Single))
            End Set
        End Property

        Public Property X As Single
            Get
                Return RFloat(Pointer + &H10)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H10, value)
            End Set
        End Property

        Public Property Y As Single
            Get
                Return RFloat(Pointer + &H14)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H14, value)
            End Set
        End Property

        Public Property Z As Single
            Get
                Return RFloat(Pointer + &H18)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H18, value)
            End Set
        End Property

    End Class

End Namespace
