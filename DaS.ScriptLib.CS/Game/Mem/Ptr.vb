Imports DaS.ScriptLib.Injection.Structures

Namespace Game.Mem

    Public Class Ptr

        Public Shared PtrToPtrToCharDataPtr1 As New LivePtrVar(Of Int32)(Function() &H137DC70)
        Public Shared PtrToCharDataPtr1 As New LivePtrVar(Of Int32)(Function() PtrToPtrToCharDataPtr1.Value + &H4)
        Public Shared CharDataPtr1 As New LivePtrVar(Of Int32)(Function() PtrToCharDataPtr1.Value)

        Public Shared GameStatsPtr As New LivePtrVar(Of Int32)(Function() &H1378700)
        Public Shared CharDataPtr2 As New LivePtrVar(Of Int32)(Function() GameStatsPtr.Value + &H8)

        Public Shared CharMapDataPtr As New LivePtrVar(Of Int32)(Function() CharDataPtr1.Value + &H28)

        Public Shared MenuPtr As New LivePtrVar(Of Int32)(Function() &H13786D0)
        Public Shared LinePtr As New LivePtrVar(Of Int32)(Function() &H1378388)
        Public Shared KeyPtr As New LivePtrVar(Of Int32)(Function() &H1378640)

        Public Shared EntityControllerPtr As New LivePtrVar(Of Int32)(Function() CharMapDataPtr.Value + &H54)
        Public Shared EntityAnimPtr As New LivePtrVar(Of Int32)(Function() CharMapDataPtr.Value + &H14)
        Public Shared PtrToPtrToEntityAnimInstancePtr As New LivePtrVar(Of Int32)(Function() EntityAnimPtr.Value + &HC)
        Public Shared PtrToEntityAnimInstancePtr As New LivePtrVar(Of Int32)(Function() PtrToPtrToEntityAnimInstancePtr.Value + &H10)
        Public Shared EntityAnimInstancePtr As New LivePtrVar(Of Int32)(Function() PtrToEntityAnimInstancePtr.Value)

    End Class

End Namespace
