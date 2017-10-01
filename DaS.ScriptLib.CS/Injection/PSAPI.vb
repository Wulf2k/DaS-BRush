Imports System.Runtime.InteropServices
Imports System.Text

Namespace Injection.PSAPI

    Friend Module PSAPI


        <DllImport("psapi.dll", SetLastError:=True)>
        Friend Function EnumProcessModules(ByVal hProcess As IntPtr, <MarshalAs(UnmanagedType.LPArray, ArraySubType:=UnmanagedType.U4)> <[In]()> <Out()> ByVal lphModule As UInteger(), ByVal cb As UInteger, <MarshalAs(UnmanagedType.U4)> ByRef lpcbNeeded As UInteger) As Boolean
        End Function

        <DllImport("psapi.dll")>
        Friend Function GetModuleBaseName(ByVal hProcess As IntPtr, ByVal hModule As IntPtr, ByVal lpBaseName As StringBuilder, nSize As UInteger) As UInteger
        End Function

    End Module

End Namespace