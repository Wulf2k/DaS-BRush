Namespace Global.DaS_BRush
    Module ProcessAccess

        Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean
        Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpThreadAttributes As IntPtr, ByVal dwStackSize As Integer, ByVal lpStartAddress As IntPtr, ByVal lpParameter As IntPtr, ByVal dwCreationFlags As Integer, ByRef lpThreadId As IntPtr) As Integer
        Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Int32) As IntPtr
        Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
        Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByVal lpNumberOfBytesWritten As Integer) As Boolean
        Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
        Declare Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flNewProtect As Integer, ByRef lpflOldProtect As Integer) As Boolean


        Public Const PROCESS_VM_READ = &H10
        Public Const TH32CS_SNAPPROCESS = &H2
        Public Const MEM_COMMIT = 4096
        Public Const PAGE_READWRITE = 4
        Public Const PAGE_EXECUTE_READWRITE = &H40
        Public Const PROCESS_CREATE_THREAD = (&H2)
        Public Const PROCESS_VM_OPERATION = (&H8)
        Public Const PROCESS_VM_WRITE = (&H20)
        Public Const PROCESS_ALL_ACCESS = &H1F0FFF

        Public isHooked As Boolean = False

        Public _targetProcess As Process = Nothing 'to keep track of it. not used yet.
        Public _targetProcessHandle As IntPtr = IntPtr.Zero 'Used for ReadProcessMemory


        Public Function ScanForProcess(ByVal windowCaption As String, Optional automatic As Boolean = False) As Boolean
            Dim _allProcesses() As Process = Process.GetProcesses
            For Each pp As Process In _allProcesses
                If pp.MainWindowTitle.ToLower.Equals(windowCaption.ToLower) Then
                    'found it! proceed.
                    Return TryAttachToProcess(pp, automatic)
                End If
            Next
            Return False
        End Function
        Public Function TryAttachToProcess(ByVal proc As Process, Optional automatic As Boolean = False) As Boolean
            If Not (_targetProcessHandle = IntPtr.Zero) Then
                DetachFromProcess()
            End If

            _targetProcess = proc
            _targetProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, _targetProcess.Id)
            If _targetProcessHandle = 0 Then
                If Not automatic Then 'Showing 2 message boxes as soon as you start the program is too annoying.
                    MessageBox.Show("Failed to attach to process.Please run with administrative privileges.")
                End If

                Return False
            Else
                'if we get here, all connected and ready to use ReadProcessMemory()
                Return True
                'MessageBox.Show("OpenProcess() OK")
            End If

        End Function
        Public Sub DetachFromProcess()
            If Not (_targetProcessHandle = IntPtr.Zero) Then
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


        Public Function RInt8(ByVal addr As IntPtr) As SByte
            Dim _rtnBytes(0) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 1, vbNull)
            Return _rtnBytes(0)
        End Function
        Public Function RInt16(ByVal addr As IntPtr) As Int16
            Dim _rtnBytes(1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToInt16(_rtnBytes, 0)
        End Function
        Public Function RInt32(ByVal addr As IntPtr) As Int32
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)

            Return BitConverter.ToInt32(_rtnBytes, 0)
        End Function
        Public Function RInt64(ByVal addr As IntPtr) As Int64
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToInt64(_rtnBytes, 0)
        End Function
        Public Function RUInt16(ByVal addr As IntPtr) As UInt16
            Dim _rtnBytes(1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
            Return BitConverter.ToUInt16(_rtnBytes, 0)
        End Function
        Public Function RUInt32(ByVal addr As IntPtr) As UInt32
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToUInt32(_rtnBytes, 0)
        End Function
        Public Function RUInt64(ByVal addr As IntPtr) As UInt64
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToUInt64(_rtnBytes, 0)
        End Function
        Public Function RFloat(ByVal addr As IntPtr) As Single
            Dim _rtnBytes(3) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
            Return BitConverter.ToSingle(_rtnBytes, 0)
        End Function
        Public Function RDouble(ByVal addr As IntPtr) As Double
            Dim _rtnBytes(7) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
            Return BitConverter.ToDouble(_rtnBytes, 0)
        End Function
        Public Function RIntPtr(ByVal addr As IntPtr) As IntPtr
            Dim _rtnBytes(IntPtr.Size - 1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, IntPtr.Size, Nothing)
            If IntPtr.Size = 4 Then
                Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
            Else
                Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
            End If
        End Function
        Public Function RBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
            Dim _rtnBytes(size - 1) As Byte
            ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, size, vbNull)
            Return _rtnBytes
        End Function
        Private Function RAsciiStr(ByVal addr As UInteger) As String
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
        Private Function RUnicodeStr(ByVal addr As UInteger) As String
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

        Public Sub Wint16(ByVal addr As IntPtr, val As Int16)
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 2, Nothing)
        End Sub
        Public Sub WInt32(ByVal addr As IntPtr, val As Int32)
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WUInt32(ByVal addr As IntPtr, val As UInt32)
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WFloat(ByVal addr As IntPtr, val As Single)
            WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
        End Sub
        Public Sub WBytes(ByVal addr As IntPtr, val As Byte())
            WriteProcessMemory(_targetProcessHandle, addr, val, val.Length, Nothing)
        End Sub
        Public Sub WAsciiStr(addr As IntPtr, str As String)
            WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.ASCII.GetBytes(str), str.Length, Nothing)
        End Sub
        Public Sub WUniStr(addr As IntPtr, str As String)
            WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.Unicode.GetBytes(str), str.Length * 2, Nothing)
        End Sub
    End Module
End Namespace


