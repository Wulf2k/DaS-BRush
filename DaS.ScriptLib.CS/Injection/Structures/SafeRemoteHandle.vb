Imports System.Runtime.ConstrainedExecution
Imports Microsoft.Win32.SafeHandles

Namespace Injection.Structures

    Public Class SafeRemoteHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid
        Public ReadOnly Size As Int32
        Public Sub New(size As Integer)
            MyBase.New(True)
            Me.Size = size
            AddHandler DARKSOULS.OnDetach, AddressOf DARKSOULS_OnDetach
            Alloc()
        End Sub
        Private Sub Alloc()
            Dim dsHandle = DARKSOULS.GetHandle()
            Dim h = Kernel.VirtualAllocEx(dsHandle, 0, Size, Kernel.MEM_COMMIT, Kernel.PAGE_EXECUTE_READWRITE)
            SetHandle(h)
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

            Dim dsHandle = DARKSOULS.GetHandle()

            Return Kernel.VirtualFreeEx(dsHandle, handle, 0, Kernel.MEM_RELEASE) <> 0
        End Function
        Public Sub MemPatch(src As Byte(), Optional srcIndex As Int32? = Nothing, Optional destOffset As Int32? = Nothing, Optional ByVal numBytes As Int32? = Nothing)
            Dim dsHandle = DARKSOULS.GetHandle()
            If dsHandle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return
            End If

            If (If(destOffset, 0) + If(numBytes, src.Length)) > Size Then
                Throw New Exception("Bytes will not fit in allocated space.")
                Return
            End If
            Dim buf(numBytes - 1) As Byte
            Array.Copy(src, If(srcIndex, 0), buf, 0, If(numBytes, src.Length))
            Kernel.WriteProcessMemory_SAFE(dsHandle, handle + If(destOffset, 0), buf, If(numBytes, src.Length), Nothing)
        End Sub

        Public Sub MemPatch(src As SafeMarshalledHandle, Optional destOffset As Int32? = Nothing, Optional ByVal numBytes As Int32? = Nothing)
            If handle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return
            End If

            If (If(destOffset, 0) + If(numBytes, src.Size)) > Size Then
                Throw New Exception("Bytes will not fit in allocated space.")
                Return
            End If
            Dim buf(numBytes - 1) As Byte

            Dim dsHandle = DARKSOULS.GetHandle()

            Kernel.WriteProcessMemory_SAFE(dsHandle, handle + If(destOffset, 0), src.GetHandle(), If(numBytes, src.Size), Nothing)
        End Sub

        Public Function GetFuncReturnValue() As Byte()
            Dim result(DSAsmCaller.INT32_SIZE - 1) As Byte

            If handle.ToInt32() < DARKSOULS.SafeBaseMemoryOffset Then
                Return result
            End If

            Dim dsHandle = DARKSOULS.GetHandle()

            If Not Kernel.ReadProcessMemory_SAFE(dsHandle, handle + DSAsmCaller.FUNC_RETURN_ADDR_OFFSET, result, DSAsmCaller.INT32_SIZE, Nothing) Then
                'Throw New Exception("Kernel.ReadProcessMemory Fail for SafeRemoteHandle.GetFuncReturnValue()")
            End If
            Return result
        End Function
    End Class

End Namespace