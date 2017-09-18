Imports System.Threading
Imports DaS.ScriptLib.Injection.Structures
Imports DaS.ScriptLib.Lua

Namespace Injection
    Public Module Hook

        Public ReadOnly Property DARKSOULS As SafeDarkSoulsHandle = New SafeDarkSoulsHandle()

        Private ByteBuffer(7) As Byte

        Private LOCK_OBJ As New Object()

        Sub Init()
            DARKSOULS.TryAttachToDarkSouls(True)

            Array.Clear(ByteBuffer, 0, 8)
        End Sub

        Public Class Injected
            Public Shared ItemDropPtr As New SafeRemoteHandle(1024)
        End Class

        Private Function CheckAddress(ByVal addr As IntPtr) As Boolean
            Dim result = False
            SyncLock LOCK_OBJ
                result = (addr.ToInt32() >= DARKSOULS.SafeBaseMemoryOffset) AndAlso addr.ToInt32() < &H10000000 'may need adjusting
            End SyncLock
            Return result
        End Function

        <NLua.LuaGlobal(Name:="RInt8")>
        Public Function RInt8(ByVal addr As Long) As SByte
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 1, vbNull)
            Return ByteBuffer(0)
        End Function

        <NLua.LuaGlobal(Name:="RInt16")>
        Public Function RInt16(ByVal addr As Long) As Int16
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 2, vbNull)
            Return BitConverter.ToInt16(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RInt32")>
        Public Function RInt32(ByVal addr As Long) As Int32
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 4, vbNull)
            Return BitConverter.ToInt32(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RInt64")>
        Public Function RInt64(ByVal addr As Long) As Int64
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 8, vbNull)
            Return BitConverter.ToInt64(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RUInt16")>
        Public Function RUInt16(ByVal addr As Long) As UInt16
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 2, vbNull)
            Return BitConverter.ToUInt16(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RUInt32")>
        Public Function RUInt32(ByVal addr As Long) As UInt32
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 4, vbNull)
            Return BitConverter.ToUInt32(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RUInt64")>
        Public Function RUInt64(ByVal addr As Long) As UInt64
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 8, vbNull)
            Return BitConverter.ToUInt64(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RFloat")>
        Public Function RFloat(ByVal addr As Long) As Single
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 4, vbNull)
            Return BitConverter.ToSingle(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RDouble")>
        Public Function RDouble(ByVal addr As Long) As Double
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 8, vbNull)
            Return BitConverter.ToDouble(ByteBuffer, 0)
        End Function

        <NLua.LuaGlobal(Name:="RIntPtr")>
        Public Function RIntPtr(ByVal addr As Long) As IntPtr
            If Not CheckAddress(addr) Then Return New IntPtr(0)
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, IntPtr.Size, Nothing)
            If IntPtr.Size = 4 Then
                Return New IntPtr(BitConverter.ToUInt32(ByteBuffer, 0))
            Else
                Return New IntPtr(BitConverter.ToInt64(ByteBuffer, 0))
            End If
        End Function

        <NLua.LuaGlobal(Name:="RBytes")>
        Public Function RBytes(ByVal addr As Long, ByVal size As Int32) As Byte()
            If Not CheckAddress(addr) Then
                Dim dummyArr(size - 1) As Byte
                Return dummyArr
            End If
            Dim _rtnBytes(size - 1) As Byte
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, _rtnBytes, size, vbNull)
            Return _rtnBytes
        End Function

        <NLua.LuaGlobal(Name:="RByte")>
        Public Function RByte(ByVal addr As Long) As Byte
            If Not CheckAddress(addr) Then Return 0
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 1, vbNull)
            Return ByteBuffer(0)
        End Function

        <NLua.LuaGlobal(Name:="RAsciiStr")>
        Public Function RAsciiStr(ByVal addr As Long, maxLength As Integer) As String
            If Not CheckAddress(addr) Then Return Nothing

            Dim Str As New Text.StringBuilder(maxLength)
            Dim loc As Integer = 0

            Dim nextChr = "?"c

            If maxLength <> 0 Then

                Dim bytes(1) As Byte

                While (maxLength < 0 OrElse loc < maxLength)
                    Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, bytes, 1, vbNull)
                    nextChr = Text.Encoding.ASCII.GetChars(bytes)(0)

                    If nextChr = ChrW(0) Then
                        Exit While
                    Else
                        Str.Append(nextChr)
                    End If

                    loc += 1
                End While

            End If

            Return Str.ToString()
        End Function

        <NLua.LuaGlobal(Name:="RUnicodeStr")>
        Public Function RUnicodeStr(ByVal addr As Long, maxLength As Integer) As String
            If Not CheckAddress(addr) Then Return Nothing

            Dim Str As New Text.StringBuilder(maxLength)
            Dim loc As Integer = 0

            Dim nextChr = "?"c

            If maxLength <> 0 Then

                Dim bytes(2) As Byte

                While (maxLength < 0 OrElse loc < maxLength)
                    Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, bytes, 2, vbNull)
                    nextChr = Text.Encoding.Unicode.GetChars(bytes)(0)

                    If nextChr = ChrW(0) Then
                        Exit While
                    Else
                        Str.Append(nextChr)
                    End If

                    loc += 1
                End While

            End If

            Return Str.ToString()
        End Function

        <NLua.LuaGlobal(Name:="WBool")>
        Public Function RBool(ByVal addr As Long) As Boolean
            If Not CheckAddress(addr) Then Return False
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, ByteBuffer, 1, vbNull)
            Return (ByteBuffer(0) <> 0)
        End Function

        <NLua.LuaGlobal(Name:="WBool")>
        Public Sub WBool(ByVal addr As Long, val As Boolean)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 1, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WInt16")>
        Public Sub WInt16(ByVal addr As Long, val As Int16)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 2, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WInt32")>
        Public Sub WInt32(ByVal addr As Long, val As Int32)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WUint32")>
        Public Sub WUInt32(ByVal addr As Long, val As UInt32)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WFloat")>
        Public Sub WFloat(ByVal addr As Long, val As Single)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WBytes")>
        Public Sub WBytes(ByVal addr As Long, val As Byte())
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, val, val.Length, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WByte")>
        Public Sub WByte(ByVal addr As Long, val As Byte)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, New Byte() {val}, 1, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WAsciiStr")>
        Public Sub WAsciiStr(addr As Long, str As String)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, System.Text.Encoding.ASCII.GetBytes(str).Concat(New Byte() {0}).ToArray(), str.Length + 1, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="WUnicodeStr")>
        Public Sub WUnicodeStr(addr As Long, str As String)
            If Not CheckAddress(addr) Then Return
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), addr, System.Text.Encoding.Unicode.GetBytes(str).Concat(New Byte() {0, 0}).ToArray(), str.Length * 2 + 2, Nothing)
        End Sub
    End Module
End Namespace