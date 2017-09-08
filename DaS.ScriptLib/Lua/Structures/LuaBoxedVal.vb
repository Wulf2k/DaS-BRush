Namespace Lua.Structures

    Public Class LuaBoxedVal
        Public ReadOnly Value As Object

        Public Sub New(val As Object)
            Value = val
        End Sub

        Public Overrides Function ToString() As String
            Return Value.ToString()
        End Function
    End Class
End Namespace