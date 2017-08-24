Public MustInherit Class LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        AddressFunc = addrFunc
    End Sub

    Public ReadOnly AddressFunc As Func(Of Integer)

    Private _offset As Integer
    Public Property Offset
        Get
            Return _offset
        End Get
        Private Set(value)
            _offset = value
        End Set
    End Property

    Public Property Bytes As Byte()
        Get
            Return RBytes(AddressFunc(), ValueSize)
        End Get
        Set(value As Byte())
            WBytes(AddressFunc(), value)
        End Set
    End Property

    Protected Function CheckBytesLength(Optional canBeNull As Boolean = False) As Byte()
        If (Not canBeNull) And Bytes.Length = 0 Then
            Throw New Exception("No bytes...?")
        ElseIf Not Bytes.Length = ValueSize Then
            Throw New Exception("Weird bytes...?")
        End If

        Return Bytes
    End Function

    Public MustOverride ReadOnly Property ValueSize As Byte
End Class

Public Class LivePtrVarInt
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 4
        End Get
    End Property

    Protected Property NullableValue As Integer?
        Get
            Return BitConverter.ToInt32(CheckBytesLength(), 0)
        End Get
        Set(nv As Integer?)
            Bytes = If(nv.HasValue, BitConverter.GetBytes(nv.Value), Bytes)
        End Set
    End Property

    Property Value As Integer
        Get
            Return NullableValue.Value
        End Get
        Set(value As Integer)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarInt) As Integer
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarFloat
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 4
        End Get
    End Property

    Protected Property NullableValue As Single?
        Get
            Return BitConverter.ToSingle(CheckBytesLength(), 0)
        End Get
        Set(nv As Single?)
            Bytes = If(nv.HasValue, BitConverter.GetBytes(nv.Value), Bytes)
        End Set
    End Property

    Property Value As Single
        Get
            Return NullableValue.Value
        End Get
        Set(value As Single)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarFloat) As Single
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarByte
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 1
        End Get
    End Property

    Protected Property NullableValue As Byte?
        Get
            Return CheckBytesLength()(0)
        End Get
        Set(nv As Byte?)
            Bytes = If(nv.HasValue, New Byte() {nv.Value}, Bytes)
        End Set
    End Property

    Property Value As Byte
        Get
            Return NullableValue.Value
        End Get
        Set(value As Byte)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarByte) As Byte
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarBool
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 1
        End Get
    End Property

    Protected Property NullableValue As Boolean?
        Get
            Return CheckBytesLength()(0)
        End Get
        Set(nv As Boolean?)
            Bytes = If(nv.HasValue, BitConverter.GetBytes(nv.Value), Bytes)
        End Set
    End Property

    Property Value As Boolean
        Get
            Return NullableValue.Value
        End Get
        Set(value As Boolean)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarBool) As Boolean
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarLong
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 8
        End Get
    End Property

    Protected Property NullableValue As Long?
        Get
            Return BitConverter.ToInt32(CheckBytesLength(), 0)
        End Get
        Set(nv As Long?)
            Bytes = If(nv.HasValue, BitConverter.GetBytes(nv.Value), Bytes)
        End Set
    End Property

    Property Value As Long
        Get
            Return NullableValue.Value
        End Get
        Set(value As Long)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarLong) As Long
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarAsciiStr
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 10
        End Get
    End Property

    Property Value As String
        Get
            Return RAsciiStr(AddressFunc())
        End Get
        Set(value As String)
            WAsciiStr(AddressFunc(), value)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarAsciiStr) As String
        Return live.Value
    End Operator
End Class


Public Class LivePtrVarUnicodeStr
    Inherits LivePtrVarBase

    Private _strVal As String

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return _strVal.Length
        End Get
    End Property

    Public Property Value As String
        Get
            _strVal = RUnicodeStr(AddressFunc())
            Return _strVal
        End Get
        Set(value As String)
            _strVal = value
            WUnicodeStr(AddressFunc(), value)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarUnicodeStr) As String
        Return live.Value
    End Operator
End Class

Public Class LivePtrVarShort
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 2
        End Get
    End Property

    Protected Property NullableValue As Short?
        Get
            Return BitConverter.ToInt32(CheckBytesLength(), 0)
        End Get
        Set(nv As Short?)
            Bytes = If(nv.HasValue, BitConverter.GetBytes(nv.Value), Bytes)
        End Set
    End Property

    Property Value As Short
        Get
            Return NullableValue.Value
        End Get
        Set(value As Short)
            NullableValue = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function

    Public Shared Widening Operator CType(ByVal live As LivePtrVarShort) As Short
        Return live.Value
    End Operator
End Class