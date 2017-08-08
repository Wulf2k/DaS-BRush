Public Enum VarType As Byte
    None = 0
    Unknown = 1
    Int = 2
    Float = 3
End Enum

Public MustInherit Class TypedVarBase

    Public Shared ReadOnly NULL_STRING As String = "[NULL]"
    Public Shared ReadOnly VarTypeName As Dictionary(Of VarType, String)
    Public Shared ReadOnly VarTypeNameLowerCase As Dictionary(Of VarType, String)

    ' Static contructor is called only one time once the code is initialized
    Shared Sub New()
        VarTypeName = (From e In CType([Enum].GetValues(GetType(VarType)), VarType())
                       Select kvp = New KeyValuePair(Of VarType, String)(e, e.ToString())) _
                        .ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value) ' Apparently VB has had inline lambda functions for a long ass time now

        VarTypeNameLowerCase = VarTypeName.Select(Function(kvp) New KeyValuePair(Of VarType, String)(kvp.Key, kvp.Value.ToLower)).ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value)
    End Sub

    Public Shared Function GetStringFromVarType(type As VarType) As String
        Return VarTypeName(type)
    End Function

    Public Shared Function GetVarTypeFromString(ByVal typeStr As String) As VarType
        Return VarTypeNameLowerCase.Where(Function(kvp) kvp.Value = typeStr.Trim.ToLower).Select(Function(kvp) kvp.Key).First
    End Function

    Public ReadOnly Type As VarType

    Public Overridable ReadOnly Property ValueSize As Byte
        Get
            Return Bytes.Length
        End Get
    End Property

    Private _bytes As Byte()

    Public Overridable Property Bytes As Byte()
        Get
            Return _bytes
        End Get
        Set(value As Byte())
            _bytes = value
        End Set
    End Property

    Public Function GetTypeName() As String
        Return GetStringFromVarType(Type)
    End Function

    Public Sub New(type As VarType, bytes As Byte())
        Me.Type = type
        Me.Bytes = bytes
    End Sub

    Public Sub New(type As VarType, Optional doNotInitBytes As Boolean = False)
        Me.Type = type
        If Not doNotInitBytes Then
            Bytes = New Byte() {}
        End If
    End Sub

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
        Select Case If(forceAsHex, VarType.Unknown, Type)
            Case VarType.None : Return NULL_STRING
            Case VarType.Unknown ' Do nothing and continue to hex form below
            Case VarType.Float : Return ValueFloat.ToString("0.0")
            Case VarType.Int : Return ValueInt.ToString()
        End Select

        Dim byteStr = "0x"
        For Each b As Byte In Bytes
            byteStr = byteStr & b.ToString("XX")
        Next
        Return byteStr

    End Function

End Class
