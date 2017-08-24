Public Class EntityLocation
    Public Pos As Vec3
    Public Rot As Single

    Public Sub New()
        Pos = Vec3.Zero
        Rot = 0
    End Sub

    Public Sub New(pos As Vec3, rot As Single)
        Me.Pos = pos
        Me.Rot = rot
    End Sub

    Public Sub New(pos As Vec3)
        Me.New(pos, 0)
    End Sub

    Public Sub New(posX, posY, posZ, rot)
        Pos = New Vec3(posX, posY, posZ)
        Me.Rot = rot
    End Sub

    Public Sub New(posX, posY, posZ)
        Pos = New Vec3(posX, posY, posZ)
        Rot = 0
    End Sub

    Public Function AngleTo(other As EntityLocation)
        Return Pos.GetLateralAngleTo(other.Pos)
    End Function

    Public Shared ReadOnly Zero As New EntityLocation(0, 0, 0, 0)

    Public Shared Operator <>(ByVal left As EntityLocation, ByVal right As EntityLocation) As Boolean
        Return (left.Pos <> right.Pos) Or (left.Rot <> right.Rot)
    End Operator

    Public Shared Operator =(ByVal left As EntityLocation, ByVal right As EntityLocation) As Boolean
        Return (left.Pos = right.Pos) AndAlso (left.Rot = right.Rot)
    End Operator
End Class
