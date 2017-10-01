Namespace LuaScripting.Structures

    Public Class DSLuaBoxedVal
        Implements IDisposable
        Public ReadOnly Property Value As Object

        Public Sub New(val As Object)
            Value = val
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            If TypeOf Value Is IDisposable Then
                DirectCast(Value, IDisposable).Dispose()
            Else
                _Value = Nothing
            End If
        End Sub

        Public Sub __gc()
            Dispose()
        End Sub

        Public Overrides Function ToString() As String
            Return Value.ToString()
        End Function
    End Class
End Namespace