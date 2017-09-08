Imports System.Runtime.ConstrainedExecution
Imports Microsoft.Win32.SafeHandles

Namespace Injection.Structures
    Public Class SafeRemoteThreadHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid
        Public ReadOnly Func As SafeRemoteHandle
        Public Sub New(ByRef func As SafeRemoteHandle)
            MyBase.New(True)
            Me.Func = func
            AddHandler DARKSOULS.OnDetach, AddressOf DARKSOULS_OnDetach
            Alloc()
        End Sub
        Private Sub Alloc()
            SetHandle(Kernel.CreateRemoteThread(DARKSOULS.GetHandle(), 0, 0, Func.GetHandle(), 0, 0, 0))
        End Sub
        Public Function GetHandle() As IntPtr
            If IsClosed Or IsInvalid Then
                Alloc()
            End If
            Return handle
        End Function
        Private Sub DARKSOULS_OnDetach()
            SetHandleAsInvalid()
        End Sub
        <ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>
        Protected Overrides Function ReleaseHandle() As Boolean
            RemoveHandler DARKSOULS.OnDetach, AddressOf DARKSOULS_OnDetach
            Return Kernel.CloseHandle(handle)
        End Function

    End Class
End Namespace
