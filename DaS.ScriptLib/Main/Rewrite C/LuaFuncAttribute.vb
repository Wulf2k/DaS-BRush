<AttributeUsage(AttributeTargets.Method)>
Public Class LuaFuncAttribute
    Inherits Attribute
    Public ReadOnly Property NameInLua As String

    Public Sub New(nameInLua As String)
        Me.NameInLua = nameInLua
    End Sub

End Class
