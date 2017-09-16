Namespace Game.Data.Structures

    Public Structure Heading

        Private _ingameValue As Single

        Public Sub New(headingAngle As Single)
            HeadingValue = headingAngle
        End Sub

        Public Property PlanarValue As Double
            Get
                Return _ingameValue
            End Get
            Set(value As Double)
                _ingameValue = CType(value, Single)
            End Set
        End Property

        'Value converted as a double to prevent any noticable loss of precision in the conversion.
        Public Property HeadingValue As Double
            Get
                Return (_ingameValue / Math.PI * 180.0) + 180.0
            End Get
            Set(value As Double)
                _ingameValue = CType((value * Math.PI / 180.0) - Math.PI, Single)
            End Set
        End Property

    End Structure

End Namespace
