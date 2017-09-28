Namespace LuaScripting.Structures

    Public MustInherit Class BoxedString
        Public Str As String
        Public Uni As Boolean

        Public Sub New(str As String, uni As Boolean)
            Me.Str = str
            Me.Uni = uni
        End Sub
    End Class

End Namespace
