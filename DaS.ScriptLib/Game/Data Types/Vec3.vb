Public Class Vec3
    Public X As Single = 0
    Public Y As Single = 0
    Public Z As Single = 0

    Public Sub New(x As Single, y As Single, z As Single)
        Me.X = x
        Me.Y = y
        Me.Z = z
    End Sub

    Public Function Plus(v As Vec3) As Vec3
        Return New Vec3(X + v.X, Y + v.Y, Z + v.Z)
    End Function

    Public Function Minus(v As Vec3) As Vec3
        Return New Vec3(X - v.X, Y - v.Y, Z - v.Z)
    End Function

    Public Function Times(v As Vec3) As Vec3
        Return New Vec3(X * v.X, Y * v.Y, Z * v.Z)
    End Function

    Public Function DividedBy(v As Vec3) As Vec3
        Return New Vec3(X / v.X, Y / v.Y, Z / v.Z)
    End Function

    Public Function MagnitudeSquared() As Single
        Return Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)
    End Function

    Public Function Magnitude() As Single
        Return Math.Sqrt(MagnitudeSquared())
    End Function

    Public Function DistanceSquaredTo(other As Vec3) As Single
        Return other.Minus(Me).MagnitudeSquared()
    End Function

    Public Function DistanceTo(other As Vec3) As Single
        Return Math.Sqrt(DistanceSquaredTo(other))
    End Function

    Public Function GetLateralAngleTo(other As Vec3) As Single
        Return Math.Atan2(other.Z - Z, other.X - X)
    End Function

    Public Shared Function GetLateralUnit(angle As Single) As Vec3
        Return New Vec3(Math.Cos(angle), 0, Math.Sin(angle))
    End Function

    Public Shared ReadOnly Zero As New Vec3(0, 0, 0)
    Public Shared ReadOnly One As New Vec3(1, 1, 1)
    Public Shared ReadOnly Up As New Vec3(0, 1, 0)
    Public Shared ReadOnly Down As New Vec3(0, -1, 0)
    Public Shared ReadOnly Left As New Vec3(-1, 0, 0)
    Public Shared ReadOnly Right As New Vec3(1, 0, 0)
    Public Shared ReadOnly Front As New Vec3(0, 0, 1)
    Public Shared ReadOnly Back As New Vec3(0, 0, -1)

    Public Shared Operator <>(ByVal left As Vec3, ByVal right As Vec3) As Boolean
        Return (left.X <> right.X) Or (left.Y <> right.Y) Or (left.Z <> right.Z)
    End Operator

    Public Shared Operator =(ByVal left As Vec3, ByVal right As Vec3) As Boolean
        Return (left.X = right.X) AndAlso (left.Y = right.Y) AndAlso (left.Z = right.Z)
    End Operator

End Class