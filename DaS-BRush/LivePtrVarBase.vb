Imports DaS_BRush

Public MustInherit Class LivePtrVarBase
    Inherits TypedVarBase

    Public Sub New(type As VarType, addrFunc As Func(Of Integer))
        MyBase.New(type, True)

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

    Public Overrides Property Bytes As Byte()
        Get
            Return RBytes(AddressFunc(), ValueSize)
        End Get
        Set(value As Byte())
            WBytes(AddressFunc(), value)
        End Set
    End Property

    Public MustOverride Overrides ReadOnly Property ValueSize As Byte
End Class

Public Class LivePtrVarInt
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(VarType.Int, addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 4
        End Get
    End Property
End Class

Public Class LivePtrVarFloat
    Inherits LivePtrVarBase

    Public Sub New(addrFunc As Func(Of Integer))
        MyBase.New(VarType.Float, addrFunc)
    End Sub

    Public Overrides ReadOnly Property ValueSize As Byte
        Get
            Return 4
        End Get
    End Property
End Class