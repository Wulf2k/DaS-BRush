Imports DaS.ScriptLib

Public MustInherit Class AllocatedPtr(Of T As AllocatedPtr(Of T))
    Implements IDisposable

    Private Property _address As IntPtr = IntPtr.Zero
    Public Property Address As IntPtr
        Get
            If (Not (_address = IntPtr.Zero)) Then
                Return _address
            Else
                Dbg.PrintWarn("Tried to access allocated pointer address with no game hook!")
                Return IntPtr.Zero
            End If
        End Get
        Protected Set(value As IntPtr)
            _address = value
        End Set
    End Property

    Protected MustOverride ReadOnly Property ThisInstance As T

    Public ReadOnly Property BufferSize As IntPtr
    Public ReadOnly Property IsAllocated As Boolean = False

    Friend Sub New(bufferSize As Integer, Optional childHandlesInit As Boolean = False)
        Me.BufferSize = bufferSize
        If (Not childHandlesInit) AndAlso (Not AllocOnce()) Then
            Throw New AllocatedPtrAccessException(Of T)(ThisInstance)
        End If
    End Sub

    Public Function AllocOnce() As Boolean
        ' Check if already allocated. If so, then   s u c c e s s
        If IsAllocated Then Return True
        _IsAllocated = PAllocate()
        Return IsAllocated
    End Function

    Public Function DeAllocOnce() As Boolean
        ' Check if already deallocated. If so, then   s u c c e s s
        If Not IsAllocated Then Return True
        _IsAllocated = Not PDeallocate()
        Return IsAllocated
    End Function

    Public Function DeAllocThenReAlloc() As Boolean
        DeAllocOnce()
        AllocOnce()
        Return IsAllocated
    End Function

    Public Sub WriteBytes(bytes As Byte(), Optional specificLength As Integer = -1)
        If (Not IsAllocated) OrElse (Address = IntPtr.Zero) Then
            Throw New AllocatedPtrIOException(Of T)(ThisInstance, True)
        ElseIf bytes.Length > BufferSize.ToInt32() Then
            Throw New ArgumentOutOfRangeException("bytes", "Cannot write byte buffer if it is larger than the allocated buffer size.")
        End If

        If specificLength >= 0 AndAlso bytes.Length <> specificLength Then
            Array.Resize(bytes, specificLength)
        End If

        PWriteBuffer(bytes, If(specificLength >= 0, specificLength, BufferSize))
    End Sub

    Public Function ReadBytes(Optional specificLength As Integer = -1) As Byte()
        If (Not IsAllocated) OrElse (Address = IntPtr.Zero) Then
            Throw New AllocatedPtrIOException(Of T)(ThisInstance, True)
        End If

        Dim bytes = PReadBuffer(If(specificLength >= 0, specificLength, BufferSize))

        If specificLength >= 0 AndAlso bytes.Length <> specificLength Then
            Array.Resize(bytes, specificLength)
        End If

        If bytes.Length > BufferSize.ToInt32() Then
            Throw New AllocatedPtrException(Of T)(ThisInstance, $"The buffer returned by inheriting class '{GetType(T).Name}' was larger than the allocated buffer size somehow.")
        End If

        Return bytes
    End Function

    Protected MustOverride Function PAllocate() As Boolean
    Protected MustOverride Function PDeallocate() As Boolean
    Protected Overridable Sub PDisposeOnce()
        DeAllocOnce()
    End Sub

    Protected MustOverride Sub PWriteBuffer(bytes As Byte(), specificLength As Integer)
    Protected MustOverride Function PReadBuffer(specificLength As Integer) As Byte()

    Private __disposedValue As Boolean ' To detect redundant Dispose() calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not __disposedValue Then
            PDisposeOnce()
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

Public Class AllocatedPtrException(Of T As AllocatedPtr(Of T))
    Inherits Exception

    Public ReadOnly AllocatedPtrInstance As T

    Public Sub New(ByRef apInst As T, msg As String)
        MyBase.New(msg)

        AllocatedPtrInstance = apInst
    End Sub

End Class

Public Class AllocatedPtrAccessException(Of T As AllocatedPtr(Of T))
    Inherits AllocatedPtrException(Of T)

    Public Sub New(ByRef apInst As T)
        MyBase.New(apInst, "Attempted to access Address (As IntPtr) of AllocatedPtr before successfully allocating it.")
    End Sub
End Class

Public Class AllocatedPtrIOException(Of T As AllocatedPtr(Of T))
    Inherits AllocatedPtrException(Of T)

    Public Sub New(ByRef apInst As T, isWrite As Boolean)
        MyBase.New(apInst, $"Attempted to {If(isWrite, "write to", "read from")} the address pointed to by this AllocatedPtr before successfully allocating it.")
    End Sub
End Class