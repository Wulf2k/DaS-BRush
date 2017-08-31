Imports DaS.ScriptLib

Public Class IngameAllocatedPtr
    Inherits AllocatedPtr(Of IngameAllocatedPtr)

    Private procHandle As IntPtr? = Nothing

    Protected Overrides ReadOnly Property ThisInstance As IngameAllocatedPtr
        Get
            Return Me
        End Get
    End Property

    Friend Sub New(bufferSize As Integer)
        MyBase.New(bufferSize)

        AddHandler OnDetachFromCurrentProcess, AddressOf Proc_OnDetachFromProcess
        AddHandler OnAttachToCurrentProcess, AddressOf Proc_OnAttachToProcess
    End Sub

    Protected Overrides Function PAllocate() As Boolean
        Address = VirtualAllocEx(If(procHandle, _targetProcessHandle), 0, BufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
        Return Not (Address = IntPtr.Zero)
    End Function

    Protected Overrides Function PDeallocate() As Boolean
        'Documentation says you MUST pass 0 for dwSize if the type parameter is MEM_RELEASE
        'It also says that any non-zero result is success and 0 is failure (I still wonder WHAT it returns but whatever..?)
        Return VirtualFreeEx(If(procHandle, _targetProcessHandle), Address, 0, MEM_RELEASE) <> 0
    End Function

    Private Sub Proc_OnDetachFromProcess(ByVal hand As IntPtr)
        procHandle = hand
        DeAllocOnce()
    End Sub

    Private Sub Proc_OnAttachToProcess(ByVal hand As IntPtr)
        procHandle = hand
        AllocOnce()
    End Sub

    Protected Overrides Sub PDisposeOnce()
        MyBase.PDisposeOnce()

        RemoveHandler OnDetachFromCurrentProcess, AddressOf Proc_OnDetachFromProcess
        RemoveHandler OnAttachToCurrentProcess, AddressOf Proc_OnAttachToProcess
    End Sub

    Protected Overrides Sub PWriteBuffer(bytes() As Byte, specificLength As Integer)
        WriteProcessMemory(If(procHandle, _targetProcessHandle), Address, bytes, specificLength, 0)
    End Sub

    Protected Overrides Function PReadBuffer(specificLength As Integer) As Byte()
        Dim bytes(specificLength) As Byte
        ReadProcessMemory(If(procHandle, _targetProcessHandle), Address, bytes, specificLength, 0)
        Return bytes
    End Function
End Class