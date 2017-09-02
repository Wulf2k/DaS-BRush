Friend Class MoveableAddressOffset
    Implements IDisposable
    Private baseAddress As IntPtr = IntPtr.Zero
    Private offset As IntPtr = IntPtr.Zero
    Private isDisposed As Boolean = False

    Private ReadOnly Property funcCallerInstance As LuaFunctionCaller

    Public Property Location As IntPtr
        Get
            Return baseAddress + offset
        End Get
        Set(value As IntPtr)
            offset = (value - baseAddress)
        End Set
    End Property

    Public Sub New(ByRef _funcCaller As LuaFunctionCaller, ByVal loc As IntPtr)
        funcCallerInstance = _funcCaller
        baseAddress = funcCallerInstance.CurrentCodeLocation
        Location = loc

        AddHandler funcCallerInstance.CurrentCodeLocationChanged, AddressOf funcCaller_CurrentCodeLocationChanged
    End Sub

    Private Sub funcCaller_CurrentCodeLocationChanged(newOffset As IntPtr)
        baseAddress = newOffset
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not isDisposed Then
            RemoveHandler funcCallerInstance.CurrentCodeLocationChanged, AddressOf funcCaller_CurrentCodeLocationChanged
            _funcCallerInstance = Nothing
            isDisposed = True
        End If
    End Sub
End Class
