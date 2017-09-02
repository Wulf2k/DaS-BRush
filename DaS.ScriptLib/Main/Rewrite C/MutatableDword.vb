Imports System.Runtime.InteropServices

Namespace Global.DaS.ScriptLib
    <HideFromScripting>
    Public Module MutatableDwordConversionHelper
        'TODO: Manually map a lua function named "ToDword" to the functions in here (NLua has no "Module" support, since it is primarily C# focused)

        Public Function ToDword(val As Int32) As Int32
            Return val
        End Function

        Public Function ToDword(val As UInt32) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(val As Single) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(val As Short) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(val As UShort) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(val As Boolean) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte) As Int32
            Return New MutatableDword(b1, b2, b3, b4).Int1
        End Function

        Public Function ToDword(val As Byte) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(sb1 As SByte, sb2 As SByte, sb3 As SByte, sb4 As SByte) As Int32
            Return New MutatableDword(sb1, sb2, sb3, sb4).Int1
        End Function

        Public Function ToDword(val As SByte) As Int32
            Return New MutatableDword(val).Int1
        End Function

        Public Function ToDword(bytes As Byte()) As Int32
            Return New MutatableDword(bytes).Int1
        End Function

        Public Function ToDword(sbytes As SByte()) As Int32
            Return New MutatableDword(sbytes).Int1
        End Function

        Public Function ToDwordLossy(val As Double) As Int32
            Return New MutatableDword(Convert.ToSingle(val)).Int1
        End Function

        Public Function ToDwordLossy(val As IntPtr) As Int32
            Return val.ToInt32()
        End Function
    End Module
End Namespace

<StructLayout(LayoutKind.Explicit)>
Public Structure MutatableDword

    Public Sub New(val As Int32)
        Int1 = val
    End Sub

    Public Sub New(val As UInt32)
        UInt1 = val
    End Sub

    Public Sub New(val As Single)
        Float1 = val
    End Sub

    Public Sub New(val As Boolean)
        Byte1 = If(val, 1, 0)
    End Sub

    Public Sub New(val As Byte())
        Byte1 = val(0)
        Byte2 = val(1)
        Byte3 = val(2)
        Byte4 = val(3)
    End Sub

    Public Sub New(val As SByte())
        SByte1 = val(0)
        SByte2 = val(1)
        SByte3 = val(2)
        SByte4 = val(3)
    End Sub

    Public Sub New(b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte)
        Byte1 = b1
        Byte2 = b2
        Byte3 = b3
        Byte4 = b4
    End Sub

    Public Sub New(sb1 As SByte, sb2 As SByte, sb3 As SByte, sb4 As SByte)
        SByte1 = sb1
        SByte2 = sb2
        SByte3 = sb3
        SByte4 = sb4
    End Sub

    Public Sub New(val As Byte)
        Byte1 = val
    End Sub

    Public Sub New(val As SByte)
        SByte1 = val
    End Sub

    Public Sub New(val As Short)
        Short1 = val
    End Sub

    Public Sub New(val As UShort)
        UShort1 = val
    End Sub

    <FieldOffset(0)>
    Public ReadOnly Byte1 As Byte
    <FieldOffset(1)>
    Public ReadOnly Byte2 As Byte
    <FieldOffset(2)>
    Public ReadOnly Byte3 As Byte
    <FieldOffset(3)>
    Public ReadOnly Byte4 As Byte

    <FieldOffset(0)>
    Public ReadOnly SByte1 As SByte
    <FieldOffset(1)>
    Public ReadOnly SByte2 As SByte
    <FieldOffset(2)>
    Public ReadOnly SByte3 As SByte
    <FieldOffset(3)>
    Public ReadOnly SByte4 As SByte

    Public ReadOnly Property AsByteArray As Byte()
        Get
            Return New Byte() {Byte1, Byte2, Byte3, Byte4}
        End Get
    End Property

    <FieldOffset(0)>
    Public ReadOnly Int1 As Int32

    <FieldOffset(0)>
    Public ReadOnly UInt1 As UInt32

    <FieldOffset(0)>
    Public ReadOnly Float1 As Single

    <FieldOffset(0)>
    Public ReadOnly Short1 As Short

    <FieldOffset(2)>
    Public ReadOnly Short2 As Short

    <FieldOffset(0)>
    Public ReadOnly UShort1 As UShort

    <FieldOffset(2)>
    Public ReadOnly UShort2 As UShort

    Public ReadOnly Property Bool1 As Boolean
        Get
            Return (Byte1 <> 0)
        End Get
    End Property

    Public ReadOnly Property Bool2 As Boolean
        Get
            Return (Byte2 <> 0)
        End Get
    End Property

    Public ReadOnly Property Bool3 As Boolean
        Get
            Return (Byte3 <> 0)
        End Get
    End Property

    Public ReadOnly Property Bool4 As Boolean
        Get
            Return (Byte4 <> 0)
        End Get
    End Property

End Structure
