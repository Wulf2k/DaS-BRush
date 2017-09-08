Namespace Injection.Structures
    Module MutatableDwordHelperModule
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
