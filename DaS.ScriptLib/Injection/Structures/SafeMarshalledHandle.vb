Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Namespace Injection.Structures

    Public Class SafeMarshalledHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid

        Public ReadOnly Obj As Object
        Public ReadOnly Size As Int32

        Public Sub New(obj As Object)
            MyBase.New(True)
            Size = Marshal.SizeOf(obj)
            Me.Obj = obj
            Alloc()
        End Sub

        <ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>
        Protected Overrides Function ReleaseHandle() As Boolean
            Marshal.FreeHGlobal(handle)
            Return True
        End Function

        Private Sub Alloc()
            SetHandle(Marshal.AllocHGlobal(Size))
            Marshal.StructureToPtr(Obj, handle, True)
        End Sub
        Public Function GetHandle() As IntPtr
            If IsClosed Or IsInvalid Then
                Alloc()
            End If
            Return handle
        End Function
    End Class

End Namespace