Namespace Game.Data.Structures
    Public Class Loc
        Public Pos As Vec3
        Public Rot As Heading

        Public Sub New()
            Pos = Vec3.Zero
            Rot = New Heading(0)
        End Sub

        Public Sub New(pos As Vec3, rot As Single)
            Me.Pos = pos
            Me.Rot = New Heading(rot)
        End Sub

        Public Sub New(pos As Vec3, rot As Heading)
            Me.Pos = pos
            Me.Rot = rot
        End Sub

        Public Sub New(pos As Vec3)
            Me.New(pos, 0)
        End Sub

        Public Sub New(posX As Single, posY As Single, posZ As Single, rotHeading As Single)
            Pos = New Vec3(posX, posY, posZ)
            Me.Rot = New Heading(rotHeading)
        End Sub

        Public Sub New(posX, posY, posZ)
            Pos = New Vec3(posX, posY, posZ)
            Rot = New Heading(0)
        End Sub

        Public ReadOnly Property IsZero As Boolean
            Get
                Return Pos.X = 0 AndAlso Pos.Y = 0 AndAlso Pos.Z = 0 AndAlso Rot.HeadingValue = 0
            End Get
        End Property

        Public Function AngleTo(other As Loc)
            Return Pos.GetLateralAngleTo(other.Pos)
        End Function

        Public Shared ReadOnly Zero As New Loc(0, 0, 0, 0)

        Public Shared Operator <>(ByVal left As Loc, ByVal right As Loc) As Boolean
            Return (left.Pos <> right.Pos) Or (left.Rot.PlanarValue <> right.Rot.PlanarValue)
        End Operator

        Public Shared Operator =(ByVal left As Loc, ByVal right As Loc) As Boolean
            Return (left.Pos = right.Pos) AndAlso (left.Rot.PlanarValue = right.Rot.PlanarValue)
        End Operator

    End Class
End Namespace