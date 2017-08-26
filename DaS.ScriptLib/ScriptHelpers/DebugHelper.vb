Imports System.Text

Public Class DebugHelper

    Public Shared Sub BeginIndefiniteBenchmarkLoop()

        Dim numCalls = 0
        Dim timer As Double = 0
        Dim prevClock = Date.Now
        Dim rand As New Random
        While True
            Dim curClock = Date.Now
            Dim tickDelta = curClock.Subtract(prevClock).Ticks
            Dim deltaSeconds As Double = tickDelta / 10000000 '10 million ticks in 1 second
            timer += deltaSeconds

            Dim halfMaxHp = Game.Player.MaxHP.Value / 2
            Dim randHP = rand.NextDouble() * halfMaxHp
            Game.Player.HP.Value = halfMaxHp + randHP

            numCalls += 1

            Dim hz = (numCalls / timer)

            Funcs.SetBriefingMsg($"Setting HP manually through VB: {timer.ToString("8")} ({hz.ToString("N2")} Hz)")

            prevClock = curClock
        End While
    End Sub

    Private Shared lock_Dump As New Object()
    Private Shared debugDumpFileName As String = "Debug_Dump.txt"

    Public Shared Sub PrintArray(arr As Object())
        Console.WriteLine(arr.ToString() & "{" & String.Join(", ", arr.Select(Function(x) x.ToString)) & "}")
    End Sub

    Public Shared Sub PrintArray(arr As Integer())
        Console.WriteLine(arr.ToString() & "{" & String.Join(", ", arr.Select(Function(x) x.ToString)) & "}")
    End Sub

    Public Shared Sub Dump(ByVal text As String)
        SyncLock lock_Dump
            Dim time = System.DateTime.Now
            Dim fullTimeStr = time.ToLongTimeString()
            Dim ampm = fullTimeStr.Substring(fullTimeStr.Length - 3)
            fullTimeStr = fullTimeStr.Substring(0, fullTimeStr.Length - 3)
            Dim ms = time.Millisecond.ToString("000")

            text = "[" & fullTimeStr & "." & ms & ampm & "] " & text
            If Not IO.File.Exists(debugDumpFileName) Then
                Using newFile = IO.File.CreateText(debugDumpFileName)
                    newFile.WriteLine(text)
                End Using
            Else
                Using sw As New IO.StreamWriter(debugDumpFileName, True)
                    sw.WriteLine(text)
                End Using
            End If
        End SyncLock
    End Sub

    Public Shared Sub Dump(format As String, args As Object())
        Dump(String.Format(format, args))
    End Sub
End Class
