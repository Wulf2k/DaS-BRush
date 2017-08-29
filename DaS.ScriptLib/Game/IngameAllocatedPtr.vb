Public Class IngameAllocatedPtr
    Implements IDisposable

    Public ReadOnly Property Address As IntPtr = IntPtr.Zero

    Friend Sub New(Optional hand As IntPtr? = Nothing)
        AddHandler OnDetachFromCurrentProcess, AddressOf Proc_OnDetachFromProcess
        AddHandler OnAttachToCurrentProcess, AddressOf Proc_OnAttachToProcess

        TryAlloc(hand)
    End Sub

    Private Sub Proc_OnDetachFromProcess(ByVal hand As IntPtr)
        TryDealloc(hand)
    End Sub

    Private Sub Proc_OnAttachToProcess(ByVal hand As IntPtr)
        TryAlloc(hand)
    End Sub

    Private Sub TryAlloc(Optional hand As IntPtr? = Nothing)
        Dim TargetBufferSize = 1024
        _Address = VirtualAllocEx(If(hand, _targetProcessHandle), 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Sub

    Private Sub TryDealloc(Optional hand As IntPtr? = Nothing)
        If Not Address = IntPtr.Zero Then
            Dim TargetBufferSize = 1024
            VirtualFreeEx(If(hand, _targetProcessHandle), Address, TargetBufferSize, MEM_COMMIT)
            _Address = IntPtr.Zero
        End If
    End Sub

    Private __disposedValue As Boolean ' To detect redundant Dispose() calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not __disposedValue Then
            TryDealloc()
        End If
        __disposedValue = True
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class