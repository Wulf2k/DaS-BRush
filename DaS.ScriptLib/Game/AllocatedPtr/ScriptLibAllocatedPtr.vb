Imports System.Runtime.InteropServices
Imports DaS.ScriptLib

Public Class ScriptLibAllocatedPtr
    Inherits AllocatedPtr(Of ScriptLibAllocatedPtr)

    Friend Sub New(bufferSize As Integer)
        MyBase.New(bufferSize)
    End Sub

    Protected Overrides ReadOnly Property ThisInstance As ScriptLibAllocatedPtr
        Get
            Return Me
        End Get
    End Property

    'TODO: MAKE NOT TERRIBLE
    Protected Overrides Sub PWriteBuffer(bytes() As Byte, specificLength As Integer)
        For i = 0 To specificLength
            Marshal.WriteByte(Address, i, bytes(i))
        Next
    End Sub

    Protected Overrides Function PAllocate() As Boolean
        Address = Marshal.AllocHGlobal(BufferSize)
        Return Address <> 0
    End Function

    Protected Overrides Function PDeallocate() As Boolean
        Marshal.FreeHGlobal(Address)
        Return True
    End Function

    'TODO: MAKE NOT TERRIBLE
    Protected Overrides Function PReadBuffer(specificLength As Integer) As Byte()
        Dim bytes(specificLength) As Byte
        For i As Integer = 0 To specificLength - 1
            bytes(i) = Marshal.ReadByte(Address, i)
        Next
        Return bytes
    End Function
End Class
