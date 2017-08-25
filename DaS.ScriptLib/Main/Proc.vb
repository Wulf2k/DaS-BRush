Namespace Global.DaS.ScriptLib
    Public Module Proc
        Friend Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean
        Friend Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpThreadAttributes As IntPtr, ByVal dwStackSize As Integer, ByVal lpStartAddress As IntPtr, ByVal lpParameter As IntPtr, ByVal dwCreationFlags As Integer, ByRef lpThreadId As IntPtr) As Integer
        Friend Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Int32) As IntPtr
        Friend Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
        Friend Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByVal lpNumberOfBytesWritten As Integer) As Boolean
        Friend Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
        Friend Declare Function VirtualFreeEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal dwFreeType As Integer) As IntPtr
        Friend Declare Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flNewProtect As Integer, ByRef lpflOldProtect As Integer) As Boolean
        Friend Declare Function WaitForSingleObject Lib "kernel32.dll" (ByVal hHandle As IntPtr, dwMilliseconds As Integer) As Integer

        Friend Const PROCESS_VM_READ = &H10
        Friend Const TH32CS_SNAPPROCESS = &H2
        Friend Const MEM_COMMIT = 4096
        Friend Const PAGE_READWRITE = 4
        Friend Const PAGE_EXECUTE_READWRITE = &H40
        Friend Const PROCESS_CREATE_THREAD = (&H2)
        Friend Const PROCESS_VM_OPERATION = (&H8)
        Friend Const PROCESS_VM_WRITE = (&H20)
        Friend Const PROCESS_ALL_ACCESS = &H1F0FFF

        Friend _targetProcess As Process = Nothing 'to keep track of it. not used yet.
        Friend _targetProcessHandle As IntPtr = IntPtr.Zero 'Used for ReadProcessMemory

        Friend Event OnDetachFromCurrentProcess(ByVal handle As IntPtr)
        Friend Event OnAttachToCurrentProcess(ByVal handle As IntPtr)

        Friend Function ScanForProcess(ByVal windowCaption As String, Optional automatic As Boolean = False) As Boolean
            Dim _allProcesses() As Process = Process.GetProcesses
            For Each pp As Process In _allProcesses
                If pp.MainWindowTitle.ToLower.Equals(windowCaption.ToLower) Then
                    'found it! proceed.
                    Return TryAttachToProcess(pp, automatic)
                End If
            Next
            Return False
        End Function
        Friend Function TryAttachToProcess(ByVal proc As Process, Optional automatic As Boolean = False) As Boolean
            If Not (_targetProcessHandle = IntPtr.Zero) Then
                DetachFromProcess()
            End If

            _targetProcess = proc
            _targetProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, _targetProcess.Id)
            If _targetProcessHandle = 0 Then
                If Not automatic Then 'Showing 2 message boxes as soon as you start the program is too annoying.
                    System.Windows.Forms.MessageBox.
                        Show("Found Dark Souls process but failed to attach to it." & vbCrLf &
                             "Try explicitly running this program as an administrator.", "Failed to Attach",
                             System.Windows.Forms.MessageBoxButtons.OK,
                             System.Windows.Forms.MessageBoxIcon.Stop)
                End If

                Return False
            Else
                RaiseEvent OnAttachToCurrentProcess(_targetProcessHandle)

                'if we get here, all connected and ready to use ReadProcessMemory()
                Return True
                'MessageBox.Show("OpenProcess() OK")
            End If

        End Function
        Friend Sub DetachFromProcess()
            If Not (_targetProcessHandle = IntPtr.Zero) Then
                RaiseEvent OnDetachFromCurrentProcess(_targetProcessHandle)
                _targetProcess = Nothing
                Try
                    CloseHandle(_targetProcessHandle)
                    _targetProcessHandle = IntPtr.Zero
                    'MessageBox.Show("MemReader::Detach() OK")
                Catch ex As Exception
                    'MessageBox.Show("Warning: MemoryManager::DetachFromProcess::CloseHandle error " & Environment.NewLine & ex.Message)
                End Try
            End If
        End Sub

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
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 1, vbNull)
            Return _rtnBytes(0)
        End Function
        Public Function RInt16(ByVal addr As IntPtr) As Int16
            CheckReadAddr(addr)
            Dim _rtnBytes(1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToInt16(_rtnBytes, 0)
        End Function
        Public Function RInt32(ByVal addr As IntPtr) As Int32
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToInt32(_rtnBytes, 0)
        End Function
        Public Function RInt64(ByVal addr As IntPtr) As Int64
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToInt64(_rtnBytes, 0)
        End Function
        Public Function RUInt16(ByVal addr As IntPtr) As UInt16
            CheckReadAddr(addr)
            Dim _rtnBytes(1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToUInt16(_rtnBytes, 0)
        End Function
        Public Function RUInt32(ByVal addr As IntPtr) As UInt32
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToUInt32(_rtnBytes, 0)
        End Function
        Public Function RUInt64(ByVal addr As IntPtr) As UInt64
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToUInt64(_rtnBytes, 0)
        End Function
        Public Function RFloat(ByVal addr As IntPtr) As Single
            CheckReadAddr(addr)
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToSingle(_rtnBytes, 0)
        End Function
        Public Function RDouble(ByVal addr As IntPtr) As Double
            CheckReadAddr(addr)
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToDouble(_rtnBytes, 0)
        End Function
        Public Function RIntPtr(ByVal addr As IntPtr) As IntPtr
            CheckReadAddr(addr)
            Dim _rtnBytes(IntPtr.Size - 1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, IntPtr.Size, Nothing)
            If IntPtr.Size = 4 Then
                Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
            Else
                Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
            End If
        End Function
        Public Function RBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
            CheckReadAddr(addr)
            Dim _rtnBytes(size - 1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, size, vbNull)
            Return _rtnBytes
        End Function

        Friend Function RAsciiStr(ByVal addr As UInteger) As String
            CheckReadAddr(addr)
            Dim Str As String = ""
            Dim cont As Boolean = True
            Dim loc As Integer = 0

            Dim bytes(&H10) As Byte

            ReadProcessMemory(_targetProcessHandle, addr, bytes, &H10, vbNull)

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

        Friend Function RUnicodeStr(ByVal addr As UInteger) As String
            Dim Str As String = ""
            Dim cont As Boolean = True
            Dim loc As Integer = 0

            Dim bytes(&H20) As Byte


            ReadProcessMemory(_targetProcessHandle, addr, bytes, &H20, vbNull)

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
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 1, Nothing)
        End Sub
        Public Sub WInt16(ByVal addr As IntPtr, val As Int16)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 2, Nothing)
        End Sub
        Public Sub WInt32(ByVal addr As IntPtr, val As Int32)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WUInt32(ByVal addr As IntPtr, val As UInt32)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WFloat(ByVal addr As IntPtr, val As Single)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WBytes(ByVal addr As IntPtr, val As Byte())
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, val, val.Length, Nothing)
        End Sub
        Public Sub WAsciiStr(addr As IntPtr, str As String)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.ASCII.GetBytes(str), str.Length, Nothing)
        End Sub
        Public Sub WUniStr(addr As IntPtr, str As String)
            If Not CheckWriteAddr(addr) Then Return
            WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.Unicode.GetBytes(str), str.Length * 2, Nothing)
        End Sub



        Public Function RInt8(ByVal addr As Integer) As SByte
            Return RInt8(New IntPtr(addr))
        End Function
        Public Function RInt16(ByVal addr As Integer) As Int16
            Return RInt16(New IntPtr(addr))
        End Function
        Public Function RInt32(ByVal addr As Integer) As Int32
            Return RInt32(New IntPtr(addr))
        End Function
        Public Function RInt64(ByVal addr As Integer) As Int64
            Return RInt64(New IntPtr(addr))
        End Function
        Public Function RUInt16(ByVal addr As Integer) As UInt16
            Return RUInt16(New IntPtr(addr))
        End Function
        Public Function RUInt32(ByVal addr As Integer) As UInt32
            Return RUInt32(New IntPtr(addr))
        End Function
        Public Function RUInt64(ByVal addr As Integer) As UInt64
            Return RUInt64(New IntPtr(addr))
        End Function
        Public Function RFloat(ByVal addr As Integer) As Single
            Return RFloat(New IntPtr(addr))
        End Function
        Public Function RDouble(ByVal addr As Integer) As Double
            Return RDouble(New IntPtr(addr))
        End Function
        Public Function RIntPtr(ByVal addr As Integer) As IntPtr
            Return RIntPtr(New IntPtr(addr))
        End Function
        Public Function RBytes(ByVal addr As Integer, ByVal size As Int32) As Byte()
            Return RBytes(New IntPtr(addr), size)
        End Function
        Friend Function RAsciiStr(ByVal addr As Integer) As String
            Return RAsciiStr(CType(addr, UInteger))
        End Function
        Friend Function RUnicodeStr(ByVal addr As Integer) As String
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
        Private Function RAsciiStr(ByVal addr As Long) As String
            Return RAsciiStr(CType(addr, UInteger))
        End Function
        Private Function RUnicodeStr(ByVal addr As Long) As String
            Return RUnicodeStr(CType(addr, UInteger))
        End Function


        Public Sub WBool(ByVal addr As Integer, val As Boolean)
            WBool(New IntPtr(addr), val)
        End Sub
        Public Sub WInt16(ByVal addr As Integer, val As Int16)
            WInt16(New IntPtr(addr), val)
        End Sub
        Public Sub WInt32(ByVal addr As Integer, val As Int32)
            WInt32(New IntPtr(addr), val)
        End Sub
        Public Sub WUInt32(ByVal addr As Integer, val As UInt32)
            WUInt32(New IntPtr(addr), val)
        End Sub
        Public Sub WFloat(ByVal addr As Integer, val As Single)
            WFloat(New IntPtr(addr), val)
        End Sub
        Public Sub WBytes(ByVal addr As Integer, val As Byte())
            WBytes(New IntPtr(addr), val)
        End Sub
        Friend Sub WAsciiStr(addr As Integer, str As String)
            WAsciiStr(New IntPtr(addr), str)
        End Sub
        Friend Sub WUnicodeStr(addr As Integer, str As String)
            WUniStr(New IntPtr(addr), str)
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
        Friend Sub WAsciiStr(addr As Long, str As String)
            WAsciiStr(New IntPtr(addr), str)
        End Sub
        Friend Sub WUnicodeStr(addr As Long, str As String)
            WUniStr(New IntPtr(addr), str)
        End Sub
    End Module
End Namespace


