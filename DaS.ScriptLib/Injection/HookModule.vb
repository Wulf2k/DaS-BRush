﻿Imports System.Threading
Imports DaS.ScriptLib.Injection.Structures
Imports DaS.ScriptLib.Lua

Namespace Injection
    Public Module Hook

        Public ReadOnly Property DARKSOULS As SafeDarkSoulsHandle = New SafeDarkSoulsHandle()

        Sub Init()
            DARKSOULS.TryAttachToDarkSouls(True)
        End Sub

        Public Class Injected
            Public Shared ItemDropPtr As New SafeRemoteHandle(1024)
        End Class

        Private Sub CheckReadAddr(ByVal addr As IntPtr)
            If addr = 0 Then
                Try
                    Throw New Exception()
                Catch ex As Exception
                    Console.WriteLine($"WARN: Tried to read from address 0. Stack Trace:{vbCrLf}{ex.StackTrace}")
                End Try
            End If
        End Sub

        Private Function CheckWriteAddr(ByVal addr As IntPtr) As Boolean
            If addr = 0 Then
                Try
                    Throw New Exception()
                Catch ex As Exception
                    Console.WriteLine($"WARN: Tried to write to address 0. Stack Trace:{vbCrLf}{ex.StackTrace}")
                End Try
                Return False
            End If
            Return True
        End Function

        Public Function RInt8(ByVal addr As IntPtr) As SByte
            CheckReadAddr(addr)
            Dim _rtnBytes(0) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 1, vbNull)
            Return _rtnBytes(0)
        End Function

        Public Function RInt16(ByVal addr As IntPtr) As Int16
            CheckReadAddr(addr)
            Dim _rtnBytes(1) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToInt16(_rtnBytes, 0)
        End Function

        Public Function RInt32(ByVal addr As IntPtr) As Int32
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToInt32(_rtnBytes, 0)
        End Function

        Public Function RInt64(ByVal addr As IntPtr) As Int64
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToInt64(_rtnBytes, 0)
        End Function

        Public Function RUInt16(ByVal addr As IntPtr) As UInt16
            CheckReadAddr(addr)
            Dim _rtnBytes(1) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToUInt16(_rtnBytes, 0)
        End Function

        Public Function RUInt32(ByVal addr As IntPtr) As UInt32
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToUInt32(_rtnBytes, 0)
        End Function

        Public Function RUInt64(ByVal addr As IntPtr) As UInt64
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToUInt64(_rtnBytes, 0)
        End Function

        Public Function RFloat(ByVal addr As IntPtr) As Single
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToSingle(_rtnBytes, 0)
        End Function

        Public Function RDouble(ByVal addr As IntPtr) As Double
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToDouble(_rtnBytes, 0)
        End Function

        Public Function RIntPtr(ByVal addr As IntPtr) As IntPtr
            CheckReadAddr(addr)
            Dim _rtnBytes(IntPtr.Size - 1) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, IntPtr.Size, Nothing)
            If IntPtr.Size = 4 Then
                Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
            Else
                Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
            End If
        End Function

        Public Function RBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
            CheckReadAddr(addr)
            Dim _rtnBytes(size - 1) As Byte
            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, _rtnBytes, size, vbNull)
            Return _rtnBytes
        End Function

        Public Function RAsciiStr(ByVal addr As UInteger) As String
            CheckReadAddr(addr)
            Dim Str As String = ""
            Dim cont As Boolean = True
            Dim loc As Integer = 0

            Dim bytes(&H10) As Byte

            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, bytes, &H10, vbNull)

            While (cont And loc < &H10)
                If bytes(loc) > 0 Then

                    Str = Str + Convert.ToChar(bytes(loc))

                    loc += 1
                Else
                    cont = False
                End If
            End While

            Return Str
        End Function

        Public Function RUnicodeStr(ByVal addr As UInteger) As String
            Dim Str As String = ""
            Dim cont As Boolean = True
            Dim loc As Integer = 0

            Dim bytes(&H20) As Byte

            Kernel.ReadProcessMemory(DARKSOULS.GetHandle(), addr, bytes, &H20, vbNull)

            While (cont And loc < &H20)
                If bytes(loc) > 0 Then

                    Str = Str + Convert.ToChar(bytes(loc))

                    loc += 2
                Else
                    cont = False
                End If
            End While

            Return Str
        End Function

        Public Sub WBool(ByVal addr As IntPtr, val As Boolean)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 1, Nothing)
        End Sub

        Public Sub WInt16(ByVal addr As IntPtr, val As Int16)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 2, Nothing)
        End Sub

        Public Sub WInt32(ByVal addr As IntPtr, val As Int32)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        Public Sub WUInt32(ByVal addr As IntPtr, val As UInt32)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        Public Sub WFloat(ByVal addr As IntPtr, val As Single)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub

        Public Sub WBytes(ByVal addr As IntPtr, val As Byte())
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, val, val.Length, Nothing)
        End Sub

        Public Sub WAsciiStr(addr As IntPtr, str As String)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, System.Text.Encoding.ASCII.GetBytes(str).Concat(New Byte() {0}).ToArray(), str.Length + 1, Nothing)
        End Sub

        Public Sub WUnicodeStr(addr As IntPtr, str As String)
            If Not CheckWriteAddr(addr) Then Return
            Kernel.WriteProcessMemory(DARKSOULS.GetHandle(), addr, System.Text.Encoding.Unicode.GetBytes(str).Concat(New Byte() {0, 0}).ToArray(), str.Length * 2 + 2, Nothing)
        End Sub

        <NLua.LuaGlobal(Name:="RInt8")>
        Public Function RInt8(ByVal addr As Integer) As SByte
            Return RInt8(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RInt16")>
        Public Function RInt16(ByVal addr As Integer) As Int16
            Return RInt16(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RInt32")>
        Public Function RInt32(ByVal addr As Integer) As Int32
            Return RInt32(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RInt64")>
        Public Function RInt64(ByVal addr As Integer) As Int64
            Return RInt64(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RUInt16")>
        Public Function RUInt16(ByVal addr As Integer) As UInt16
            Return RUInt16(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RUInt32")>
        Public Function RUInt32(ByVal addr As Integer) As UInt32
            Return RUInt32(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RUInt64")>
        Public Function RUInt64(ByVal addr As Integer) As UInt64
            Return RUInt64(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RFloat")>
        Public Function RFloat(ByVal addr As Integer) As Single
            Return RFloat(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RDouble")>
        Public Function RDouble(ByVal addr As Integer) As Double
            Return RDouble(New IntPtr(addr))
        End Function

        Public Function RIntPtr(ByVal addr As Integer) As IntPtr
            Return RIntPtr(New IntPtr(addr))
        End Function

        <NLua.LuaGlobal(Name:="RBytes")>
        Public Function RBytes(ByVal addr As Integer, ByVal size As Int32) As Byte()
            Return RBytes(New IntPtr(addr), size)
        End Function

        <NLua.LuaGlobal(Name:="RByte")>
        Public Function RByte(ByVal addr As Integer) As Byte
            Return RBytes(New IntPtr(addr), 1)(0)
        End Function

        <NLua.LuaGlobal(Name:="RAsciiStr")>
        Public Function RAsciiStr(ByVal addr As Integer) As String
            Return RAsciiStr(CType(addr, UInteger))
        End Function

        <NLua.LuaGlobal(Name:="RUnicodeStr")>
        Public Function RUnicodeStr(ByVal addr As Integer) As String
            Return RUnicodeStr(CType(addr, UInteger))
        End Function

        Public Function RInt8(ByVal addr As Long) As SByte
            Return RInt8(New IntPtr(addr))
        End Function

        Public Function RInt16(ByVal addr As Long) As Int16
            Return RInt16(New IntPtr(addr))
        End Function

        Public Function RInt32(ByVal addr As Long) As Int32
            Return RInt32(New IntPtr(addr))
        End Function

        Public Function RInt64(ByVal addr As Long) As Int64
            Return RInt64(New IntPtr(addr))
        End Function

        Public Function RUInt16(ByVal addr As Long) As UInt16
            Return RUInt16(New IntPtr(addr))
        End Function

        Public Function RUInt32(ByVal addr As Long) As UInt32
            Return RUInt32(New IntPtr(addr))
        End Function

        Public Function RUInt64(ByVal addr As Long) As UInt64
            Return RUInt64(New IntPtr(addr))
        End Function

        Public Function RFloat(ByVal addr As Long) As Single
            Return RFloat(New IntPtr(addr))
        End Function

        Public Function RDouble(ByVal addr As Long) As Double
            Return RDouble(New IntPtr(addr))
        End Function

        Public Function RIntPtr(ByVal addr As Long) As IntPtr
            Return RIntPtr(New IntPtr(addr))
        End Function

        Public Function RBytes(ByVal addr As Long, ByVal size As Int32) As Byte()
            Return RBytes(New IntPtr(addr), size)
        End Function

        Public Function RAsciiStr(ByVal addr As Long) As String
            Return RAsciiStr(CType(addr, UInteger))
        End Function

        Public Function RUnicodeStr(ByVal addr As Long) As String
            Return RUnicodeStr(CType(addr, UInteger))
        End Function

        <NLua.LuaGlobal(Name:="WBool")>
        Public Sub WBool(ByVal addr As Integer, val As Boolean)
            WBool(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WInt16")>
        Public Sub WInt16(ByVal addr As Integer, val As Int16)
            WInt16(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WInt32")>
        Public Sub WInt32(ByVal addr As Integer, val As Int32)
            WInt32(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WUInt32")>
        Public Sub WUInt32(ByVal addr As Integer, val As UInt32)
            WUInt32(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WFloat")>
        Public Sub WFloat(ByVal addr As Integer, val As Single)
            WFloat(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WBytes")>
        Public Sub WBytes(ByVal addr As Integer, val As Byte())
            WBytes(New IntPtr(addr), val)
        End Sub

        <NLua.LuaGlobal(Name:="WAsciiStr")>
        Public Sub WAsciiStr(addr As Integer, str As String)
            WAsciiStr(New IntPtr(addr), str)
        End Sub

        <NLua.LuaGlobal(Name:="WUnicodeStr")>
        Public Sub WUnicodeStr(addr As Integer, str As String)
            WUnicodeStr(New IntPtr(addr), str)
        End Sub

        Public Sub WInt16(ByVal addr As Long, val As Int16)
            WInt16(New IntPtr(addr), val)
        End Sub

        Public Sub WInt32(ByVal addr As Long, val As Int32)
            WInt32(New IntPtr(addr), val)
        End Sub

        Public Sub WUInt32(ByVal addr As Long, val As UInt32)
            WUInt32(New IntPtr(addr), val)
        End Sub

        Public Sub WFloat(ByVal addr As Long, val As Single)
            WFloat(New IntPtr(addr), val)
        End Sub

        Public Sub WBytes(ByVal addr As Long, val As Byte())
            WBytes(New IntPtr(addr), val)
        End Sub

        Public Sub WAsciiStr(addr As Long, str As String)
            WAsciiStr(New IntPtr(addr), str)
        End Sub

        Public Sub WUnicodeStr(addr As Long, str As String)
            WUnicodeStr(New IntPtr(addr), str)
        End Sub

    End Module
End Namespace