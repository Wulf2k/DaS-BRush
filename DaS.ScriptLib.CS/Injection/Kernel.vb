Imports System.Runtime.ConstrainedExecution
Imports DaS.ScriptLib.Injection.Structures

Namespace Injection
    Namespace Kernel
        Friend Module Kernel
            Friend Const PROCESS_VM_READ As Integer = &H10
            Friend Const TH32CS_SNAPPROCESS As Integer = &H2
            Friend Const MEM_COMMIT As Integer = 4096
            Friend Const PAGE_READWRITE As Integer = 4
            Friend Const PAGE_EXECUTE_READWRITE As Integer = &H40
            Friend Const PROCESS_CREATE_THREAD As Integer = (&H2)
            Friend Const PROCESS_VM_OPERATION As Integer = (&H8)
            Friend Const PROCESS_VM_WRITE As Integer = (&H20)
            Friend Const PROCESS_ALL_ACCESS As Integer = &H1F0FFF
            Friend Const MEM_RELEASE As Integer = &H8000
            Friend Const CREATE_SUSPENDED As Integer = &H4

            <ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>
            Friend Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean

            Friend Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As IntPtr,
                                                                      ByVal lpThreadAttributes As IntPtr,
                                                                      ByVal dwStackSize As Integer,
                                                                      ByVal lpStartAddress As IntPtr,
                                                                      ByVal lpParameter As IntPtr,
                                                                      ByVal dwCreationFlags As Integer,
                                                                      ByRef lpThreadId As IntPtr) As Integer

            'DOES NOT WORK
            'Friend Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                          ByVal lpThreadAttributes As IntPtr,
            '                                                          ByVal dwStackSize As Integer,
            '                                                          ByVal lpStartAddress As SafeRemoteHandle,
            '                                                          ByVal lpParameter As IntPtr,
            '                                                          ByVal dwCreationFlags As Integer,
            '                                                          ByRef lpThreadId As IntPtr) As Integer

            Friend Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32,
                                                                    ByVal bInheritHandle As Boolean,
                                                                    ByVal dwProcessId As Int32) As IntPtr

            Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr,
                                                                      ByVal lpBaseAddress As IntPtr,
                                                                      ByVal lpBuffer() As Byte,
                                                                      ByVal iSize As Integer,
                                                                      ByRef lpNumberOfBytesRead As Integer) As Boolean

            Friend Function ReadProcessMemory_SAFE(ByVal hProcess As IntPtr,
                                                                      ByVal lpBaseAddress As IntPtr,
                                                                      ByVal lpBuffer() As Byte,
                                                                      ByVal iSize As Integer,
                                                                      ByRef lpNumberOfBytesRead As Integer) As Boolean

                If VirtualProtectEx(hProcess, lpBaseAddress, iSize, PAGE_READWRITE, 0) <> 0 Then
                    Return ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, iSize, lpNumberOfBytesRead)
                Else
                    Return False
                End If

            End Function

            'Friend Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                          ByVal lpBaseAddress As SafeRemoteHandle,
            '                                                          ByVal lpBuffer() As Byte,
            '                                                          ByVal iSize As Integer,
            '                                                          ByRef lpNumberOfBytesRead As Integer) As Boolean

            Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr,
                                                                       ByVal lpBaseAddress As IntPtr,
                                                                       ByVal lpBuffer() As Byte,
                                                                       ByVal iSize As Integer,
                                                                       ByVal lpNumberOfBytesWritten As Integer) As Boolean

            Friend Function WriteProcessMemory_SAFE(ByVal hProcess As IntPtr,
                                                                       ByVal lpBaseAddress As IntPtr,
                                                                       ByVal lpBuffer() As Byte,
                                                                       ByVal iSize As Integer,
                                                                       ByVal lpNumberOfBytesWritten As Integer) As Boolean

                If VirtualProtectEx(hProcess, lpBaseAddress, iSize, PAGE_READWRITE, 0) <> 0 Then
                    Return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, iSize, lpNumberOfBytesWritten)
                Else
                    Return False
                End If

            End Function

            'Friend Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                           ByVal lpBaseAddress As IntPtr,
            '                                                           ByVal lpBuffer As SafeMarshalledHandle,
            '                                                           ByVal iSize As Integer,
            '                                                           ByVal lpNumberOfBytesWritten As Integer) As Boolean

            'Friend Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                           ByVal lpBaseAddress As SafeRemoteHandle,
            '                                                           ByVal lpBuffer() As Byte,
            '                                                           ByVal iSize As Integer,
            '                                                           ByVal lpNumberOfBytesWritten As Integer) As Boolean

            Friend Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr,
                                                                       ByVal lpAddress As IntPtr,
                                                                       ByVal dwSize As Int32,
                                                                       ByVal flAllocationType As Integer,
                                                                       ByVal flProtect As Integer) As IntPtr

            'Friend Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                           ByVal lpAddress As SafeRemoteHandle,
            '                                                           ByVal dwSize As Int32,
            '                                                           ByVal flAllocationType As Integer,
            '                                                           ByVal flProtect As Integer) As SafeRemoteHandle

            Friend Declare Function VirtualFreeEx Lib "kernel32.dll" (ByVal hProcess As IntPtr,
                                                                      ByVal lpAddress As IntPtr,
                                                                      ByVal dwSize As Int32,
                                                                      ByVal dwFreeType As Integer) As Boolean

            'Friend Declare Function VirtualFreeEx Lib "kernel32.dll" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                          ByVal lpAddress As SafeRemoteHandle,
            '                                                          ByVal dwSize As Int32,
            '                                                          ByVal dwFreeType As Integer) As Boolean

            Friend Declare Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As IntPtr,
                                                                         ByVal lpAddress As IntPtr,
                                                                         ByVal dwSize As Int32,
                                                                         ByVal flNewProtect As Integer,
                                                                         ByRef lpflOldProtect As Integer) As Boolean

            'Friend Declare Function VirtualProtectEx Lib "kernel32.dll" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                             ByVal lpAddress As SafeDarkSoulsHandle,
            '                                                             ByVal dwSize As Int32,
            '                                                             ByVal flNewProtect As Integer,
            '                                                             ByRef lpflOldProtect As Integer) As Boolean

            'Friend Declare Function WaitForSingleObject Lib "kernel32.dll" (ByVal hHandle As IntPtr,
            '                                                                dwMilliseconds As Integer) As Int32

            Friend Declare Function WaitForSingleObject Lib "kernel32.dll" (ByVal hHandle As IntPtr,
                                                                            dwMilliseconds As Integer) As Int32

            Friend Declare Function FlushInstructionCache Lib "kernel32.dll" (ByVal hProcess As IntPtr,
                                                                              ByVal lpBaseAddress As IntPtr,
                                                                              ByVal dwSize As Integer) As Boolean

            'Friend Declare Function FlushInstructionCache Lib "kernel32.dll" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                                  ByVal lpBaseAddress As SafeRemoteHandle,
            '                                                                  ByVal dwSize As Integer) As Boolean

            Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr,
                                                                       ByVal lpBaseAddress As IntPtr,
                                                                       ByVal lpBuffer As IntPtr,
                                                                       ByVal iSize As Integer,
                                                                       ByVal lpNumberOfBytesWritten As Integer) As Boolean

            Friend Function WriteProcessMemory_SAFE(ByVal hProcess As IntPtr,
                                                                       ByVal lpBaseAddress As IntPtr,
                                                                       ByVal lpBuffer As IntPtr,
                                                                       ByVal iSize As Integer,
                                                                       ByVal lpNumberOfBytesWritten As Integer) As Boolean

                If VirtualProtectEx(hProcess, lpBaseAddress, iSize, PAGE_READWRITE, 0) <> 0 Then
                    Return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, iSize, lpNumberOfBytesWritten)
                Else
                    Return False
                End If



            End Function



            'Friend Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As SafeDarkSoulsHandle,
            '                                                           ByVal lpBaseAddress As SafeRemoteHandle,
            '                                                           ByVal lpBuffer As IntPtr,
            '                                                           ByVal iSize As Integer,
            '                                                           ByVal lpNumberOfBytesWritten As Integer) As Boolean


        End Module
    End Namespace
End Namespace
