' We aren't bitmasking these things or anything like that so we dont need to allocate more than 1 byte unless we somehow have over 256 of them :Thonk:
Imports System.Globalization

Public Enum ScriptVarType As Byte
    null = 0
    unknown = 1
    int = 2
    float = 3
End Enum

'TODO: Add support for strings.
Public MustInherit Class ScriptVar

    Public Shared Function GetStringFromType(varType As ScriptVarType) As String
        Return varType.ToString()
    End Function

    Public Shared Function GetTypeFromString(typeStr As String) As ScriptVarType
        Return [Enum].Parse(GetType(ScriptVarType), typeStr)
    End Function

    Public Shared Function Parse(nameStr As String, typeStr As String, Optional valueStr As String = "") As ScriptVar
        Select Case GetTypeFromString(typeStr)
            Case ScriptVarType.int : Return New ScriptVarInt(nameStr, If(String.IsNullOrWhiteSpace(valueStr), Nothing, CTypeDynamic(valueStr, GetType(Integer))))
            Case ScriptVarType.float : Return New ScriptVarFloat(nameStr, If(String.IsNullOrWhiteSpace(valueStr), Nothing, Convert.ToSingle(valueStr, New CultureInfo("en-us"))))
            Case Else
                Throw New Exception("''" & typeStr & "'' is not a valid variable type.")
        End Select
    End Function

    Public Function GetTypeName() As String
        Return GetStringFromType(Type)
    End Function

    Public Const NULL_STRING As String = "[NULL]"

    Private _bytes As Byte()

    Public Property Bytes As Byte()
        Get
            Return _bytes
        End Get
        Private Set(value As Byte())
            _bytes = value
        End Set
    End Property

    Public ReadOnly Type As ScriptVarType
    Public ReadOnly Name As String

    Public Sub New(name As String, type As ScriptVarType, value As Byte())
        Me.Name = name.ToLower()
        Me.Type = type
        Me.Bytes = value
    End Sub

    Public Sub New(name As String, type As ScriptVarType)
        Me.Name = name.ToLower()
        Me.Type = type
        Me.Bytes = New Byte() {}
    End Sub

    Public Function ReadFromFunctionResult(result As Integer) As ScriptVar
        If Type = ScriptVarType.int Then
            ValueInt = result
        ElseIf Type = ScriptVarType.float Then
            'TODO: MAKE SURE THIS IS CORRECT ...?
            ValueFloat = BitConverter.ToSingle(BitConverter.GetBytes(result), 0)
        End If

        Return Me
    End Function

    Private Function CheckBytesLength(requiredLength As Integer, Optional canBeNull As Boolean = False) As Byte()
        If (Not canBeNull) And Bytes.Length = 0 Then
            Throw New DidNotSetVariableValueException(Me)
        ElseIf Not Bytes.Length = requiredLength Then
            Throw New InvalidVariableValueTypeException(Me)
        End If

        Return Bytes
    End Function

    Protected Property NullableValueInt As Integer?
        Get
            Return BitConverter.ToInt32(CheckBytesLength(4), 0)
        End Get
        Set(nullableValue As Integer?)
            Bytes = If(nullableValue.HasValue, BitConverter.GetBytes(nullableValue.Value), Bytes)
        End Set
    End Property

    Property ValueInt As Integer
        Get
            Return NullableValueInt.Value
        End Get
        Set(value As Integer)
            NullableValueInt = value
        End Set
    End Property

    Protected Property NullableValueFloat As Single?
        Get
            Return BitConverter.ToSingle(CheckBytesLength(4), 0)
        End Get
        Set(nullableValue As Single?)
            Bytes = If(nullableValue.HasValue, BitConverter.GetBytes(nullableValue.Value), Bytes)
        End Set
    End Property

    Property ValueFloat As Single
        Get
            Return NullableValueFloat.Value
        End Get
        Set(value As Single)
            NullableValueFloat = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return ToString(False)
    End Function

    Public Overloads Function ToString(forceAsHex) As String
        'If forceAsHex is specified, it will jump directly to the ScriptVarType.Unknown case,
        'which happens to be a hexadecimal representation of the byte array.
        Select Case If(forceAsHex, ScriptVarType.unknown, Type)
            Case ScriptVarType.null : Return NULL_STRING
            Case ScriptVarType.float : Return ValueFloat.ToString("0.0")
            Case ScriptVarType.int : Return ValueInt.ToString()
            Case ScriptVarType.unknown : End
            Case Else ' Includes ScriptVarType.Unknown
                Dim byteStr = "0x"
                For Each b As Byte In Bytes
                    byteStr = byteStr & b.ToString("XX")
                Next
                Return byteStr
        End Select

    End Function

End Class

Public Class ScriptVarInt
    Inherits ScriptVar

    Public Sub New(name As String, Optional ByVal val As Integer? = Nothing)
        MyBase.New(name, ScriptVarType.int)
        NullableValueInt = val
    End Sub
End Class

Public Class ScriptVarFloat
    Inherits ScriptVar

    Public Sub New(name As String, Optional ByVal val As Single? = Nothing)
        MyBase.New(name, ScriptVarType.float)
        NullableValueFloat = val
    End Sub
End Class