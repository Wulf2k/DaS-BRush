Imports System.Globalization

'TODO: Add support for strings.
Public MustInherit Class ScriptVarBase
    Inherits TypedVarBase

    Public Shared Function Parse(nameStr As String, typeStr As String, Optional valueStr As String = "") As ScriptVarBase
        Select Case GetVarTypeFromString(typeStr)
            Case VarType.Int : Return New ScriptVarInt(nameStr, If(String.IsNullOrWhiteSpace(valueStr), Nothing, CTypeDynamic(valueStr, GetType(Integer))))
            Case VarType.Float : Return New ScriptVarFloat(nameStr, If(String.IsNullOrWhiteSpace(valueStr), Nothing, Convert.ToSingle(valueStr, New CultureInfo("en-us"))))
            Case Else
                Throw New Exception("''" & typeStr & "'' is not a valid variable type.")
        End Select
    End Function

    Public ReadOnly Name As String

    Public Sub New(name As String, type As VarType, value As Byte())
        MyBase.New(type, value)
        Me.Name = name
    End Sub

    Public Sub New(name As String, type As VarType)
        MyBase.New(type)
        Me.Name = name
    End Sub

    Public Function ReadFromFunctionResult(result As Integer) As ScriptVarBase
        If Type = VarType.Int Then
            ValueInt = result
        ElseIf Type = VarType.Float Then
            'TODO: MAKE SURE THIS IS CORRECT ...?
            ValueFloat = BitConverter.ToSingle(BitConverter.GetBytes(result), 0)
        End If

        Return Me
    End Function
End Class

Public Class ScriptVarInt
    Inherits ScriptVarBase

    Public Sub New(name As String, Optional ByVal val As Integer? = Nothing)
        MyBase.New(name, VarType.Int)
        NullableValueInt = val
    End Sub
End Class

Public Class ScriptVarFloat
    Inherits ScriptVarBase

    Public Sub New(name As String, Optional ByVal val As Single? = Nothing)
        MyBase.New(name, VarType.float)
        NullableValueFloat = val
    End Sub
End Class