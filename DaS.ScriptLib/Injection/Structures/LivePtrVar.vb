Imports DaS.ScriptLib.Game.Data.Structures

Namespace Injection.Structures

    Public Class LivePtrVar(Of T As Structure)

        Public Sub New(addressFunc As Func(Of Integer),
                       Optional toBytesFunc As Func(Of IEnumerable(Of Byte)) = Nothing,
                       Optional fromBytesFunc As Func(Of Byte(), T) = Nothing)
            Me.AddressFunc = addressFunc
            Me.ToBytesFunc = If(toBytesFunc, AddressOf DefaultToBytesFunction)
            Me.FromBytesFunc = If(fromBytesFunc, AddressOf DefaultFromBytesFunction)

            AddHandler DARKSOULS.OnAttach, AddressOf OnHookToGame

            UpdateOffset()
        End Sub

        Private Function DefaultToBytesFunction() As IEnumerable(Of Byte)

            Dim typ = GetType(T)


            If typ = GetType(Heading) Then Return BitConverter.GetBytes(CTypeDynamic(Of Heading)(Value).PlanarValue)


            If typ = GetType(Integer) Then Return BitConverter.GetBytes(CTypeDynamic(Of Integer)(Value))
            If typ = GetType(Single) Then Return BitConverter.GetBytes(CTypeDynamic(Of Single)(Value))
            If typ = GetType(Byte) Then Return New Byte() {CTypeDynamic(Of Byte)(Value)}
            If typ = GetType(Boolean) Then Return BitConverter.GetBytes(CTypeDynamic(Of Boolean)(Value))
            If typ = GetType(Char) Then Return BitConverter.GetBytes(CTypeDynamic(Of Char)(Value))
            If typ = GetType(Short) Then Return BitConverter.GetBytes(CTypeDynamic(Of Short)(Value))
            If typ = GetType(Long) Then Return BitConverter.GetBytes(CTypeDynamic(Of Long)(Value))
            If typ = GetType(UShort) Then Return BitConverter.GetBytes(CTypeDynamic(Of UShort)(Value))
            If typ = GetType(UInteger) Then Return BitConverter.GetBytes(CTypeDynamic(Of UInteger)(Value))
            If typ = GetType(ULong) Then Return BitConverter.GetBytes(CTypeDynamic(Of ULong)(Value))
            If typ = GetType(Double) Then Return BitConverter.GetBytes(CTypeDynamic(Of Short)(Value))
            If typ = GetType(SByte) Then Return New Byte() {CType(CTypeDynamic(Of SByte)(Value), Byte)}

            Return Nothing

        End Function

        Private Function DefaultFromBytesFunction(bytes As Byte()) As T

            Dim typ = GetType(T)



            If typ = GetType(Heading) Then
                Return CTypeDynamic(Of T)(New Heading() With {.PlanarValue = BitConverter.ToDouble(bytes, 0)})
            End If



            If typ = GetType(Integer) Then Return CTypeDynamic(Of T)(BitConverter.ToInt32(bytes, 0))
            If typ = GetType(Single) Then Return CTypeDynamic(Of T)(BitConverter.ToInt32(bytes, 0))
            If typ = GetType(Byte) Then Return CTypeDynamic(Of T)(bytes(0))
            If typ = GetType(Boolean) Then Return CTypeDynamic(Of T)(BitConverter.ToBoolean(bytes, 0))
            If typ = GetType(Char) Then Return CTypeDynamic(Of T)(BitConverter.ToChar(bytes, 0))
            If typ = GetType(Short) Then Return CTypeDynamic(Of T)(BitConverter.ToInt16(bytes, 0))
            If typ = GetType(Long) Then Return CTypeDynamic(Of T)(BitConverter.ToInt64(bytes, 0))
            If typ = GetType(UShort) Then Return CTypeDynamic(Of T)(BitConverter.ToUInt16(bytes, 0))
            If typ = GetType(UInteger) Then Return CTypeDynamic(Of T)(BitConverter.ToUInt32(bytes, 0))
            If typ = GetType(ULong) Then Return CTypeDynamic(Of T)(BitConverter.ToUInt64(bytes, 0))
            If typ = GetType(Double) Then Return CTypeDynamic(Of T)(BitConverter.ToDouble(bytes, 0))
            If typ = GetType(SByte) Then Return CTypeDynamic(Of T)(CType(bytes(0), SByte))

        End Function

        Private Sub OnHookToGame()
            UpdateOffset()
        End Sub

        Private Sub UpdateOffset()
            _offset = AddressFunc.Invoke()
        End Sub

        Public ReadOnly AddressFunc As Func(Of Integer)
        Public ReadOnly ToBytesFunc As Func(Of IEnumerable(Of Byte))
        Public ReadOnly FromBytesFunc As Func(Of Byte(), T)

        Private _offset As Integer? = Nothing

        Public ReadOnly Property Offset As Integer
            Get
                If Not _offset.HasValue Then
                    UpdateOffset()
                    If Not _offset.HasValue Then
                        Return -1
                    End If
                End If

                Return _offset.Value
            End Get
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

        Public ReadOnly Property ValueSize As Byte
            Get
                Return Len(New T())
            End Get
        End Property

        Public Function GetValue() As T
            Return Value
        End Function

        Public Sub SetValue(newVal As T)
            Value = newVal
        End Sub

        Public Property Value As T
            Get
                _offset = AddressFunc.Invoke()

                Dim bytes = RBytes(_offset.Value, ValueSize)

                Dim typ = GetType(T)
                If (typ = GetType(Integer)) Then
                    Return CTypeDynamic(Of T)(BitConverter.ToInt32(bytes, 0))
                ElseIf (typ = GetType(Byte)) Then
                    Return CTypeDynamic(Of T)(bytes(0))
                ElseIf (typ = GetType(Boolean)) Then
                    Return CTypeDynamic(Of T)(bytes(0) <> 0)
                ElseIf (typ = GetType(Single)) Then
                    Return CTypeDynamic(Of T)(BitConverter.ToSingle(bytes, 0))
                End If
            End Get
            Set(value As T)
                _offset = AddressFunc.Invoke()

                Dim typ = GetType(T)
                If (typ = GetType(Integer)) Then
                    WInt32(_offset.Value, CTypeDynamic(Of Integer)(value))
                ElseIf (typ = GetType(Byte)) Then
                    WBytes(_offset.Value, {CTypeDynamic(Of Byte)(value)})
                ElseIf (typ = GetType(Boolean)) Then
                    WBool(_offset.Value, CTypeDynamic(Of Boolean)(value))
                ElseIf (typ = GetType(Single)) Then
                    WFloat(_offset.Value, CTypeDynamic(Of Single)(value))
                End If

            End Set
        End Property
    End Class

End Namespace