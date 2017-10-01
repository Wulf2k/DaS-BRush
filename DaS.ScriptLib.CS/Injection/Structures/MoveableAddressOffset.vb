Namespace Injection.Structures

    Friend Class MoveableAddressOffset
        Private offset As IntPtr = IntPtr.Zero

        Private ReadOnly Property funcCallerInstance As DSAsmCaller

        Public Property Location As IntPtr
            Get
                Return funcCallerInstance.CodeHandle.GetHandle() + offset
            End Get
            Set(value As IntPtr)
                offset = (value - funcCallerInstance.CodeHandle.GetHandle())
            End Set
        End Property

        Public Sub New(ByRef _funcCaller As DSAsmCaller, ByVal loc As IntPtr)
            funcCallerInstance = _funcCaller
            Location = loc
        End Sub
    End Class

End Namespace