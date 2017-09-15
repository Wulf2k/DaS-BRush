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
            Dim dsHandle = DARKSOULS.GetHandle()
            Dim funcHandle = Func.GetHandle()
            If funcHandle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return
            End If

            SetHandle(Kernel.CreateRemoteThread(dsHandle, 0, 0, funcHandle, 0, 0, 0))
        End Sub
        Public Function GetHandle() As IntPtr
            If IsClosed OrElse IsInvalid OrElse handle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
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

            If handle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return False
            End If

            Return Kernel.CloseHandle(handle)
        End Function

    End Class
End Namespace
