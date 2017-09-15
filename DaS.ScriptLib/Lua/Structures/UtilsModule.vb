Imports DaS.ScriptLib.Injection

Namespace Lua.Structures

    Public Class UtilsModule

        Public Const TableName As String = "Utils"
        Public Const MinLoadingScreenDur As Double = 3.0


        Friend Sub RegisterFunctions(lua As NLua.Lua)
            Dim methods = GetType(UtilsModule).GetMethods(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
            For Each m In methods
                Dim mFunc = lua.RegisterFunction(m.Name, Me, m)
            Next
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Function BitmaskCheck(input As ULong, mask As ULong) As Boolean
            Return ((input And mask) = mask)
        End Function

        'Function WAIT_FOR_GAME()
        '    ingameTimeStopped = True
        '    repeat
        '    ingameTime = RInt32(RInt32(0x1378700) + 0x68)
        '    ingameTimeStopped = (ingameTime == prevIngameTime)
        '    prevIngameTime = ingameTime
        '    until(Not ingameTimeStopped)
        '    End

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Sub WaitForGame()
            Dim curIngameTime As Integer = 0, prevIngameTime As Integer = 0, ingameTimeStopped As Boolean = True
            Do
                Dim ingameTimePointer As Integer = RInt32(&H1378700)
                If ingameTimePointer = 0 Then Continue Do

                curIngameTime = RInt32(ingameTimePointer + &H68)
                If curIngameTime = 0 Then Continue Do

                ingameTimeStopped = (prevIngameTime = 0 OrElse prevIngameTime = curIngameTime)

                prevIngameTime = curIngameTime

                Funcs.Wait(16)
            Loop While ingameTimeStopped
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Function WaitForGameAndMeasureDuration() As Double
            Dim startTime = DateTime.Now
            WaitForGame()

            '1.0 to make sure it's not using integer division. 
            'Can't trust AmbiguousBasic.net to do everything correctly on its own amirite Kappa
            Return (1.0 * DateTime.Now.Subtract(startTime).Ticks / TimeSpan.TicksPerSecond)
        End Function

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Sub WaitUntilAfterNextLoadingScreen()
            Dim waitDuration As Double = 0
            Do
                waitDuration = WaitForGameAndMeasureDuration()
            Loop Until waitDuration >= MinLoadingScreenDur
        End Sub

    End Class

End Namespace