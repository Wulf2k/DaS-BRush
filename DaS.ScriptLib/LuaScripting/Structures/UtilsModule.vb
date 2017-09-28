Imports DaS.ScriptLib.Injection
Imports Neo.IronLua

Namespace LuaScripting.Structures

    Public Class Utils

        Public Shared ReadOnly GlobalInstance As New Utils()

        Public Const TableName As String = "Utils"
        Public Const MinLoadingScreenDur As Double = 3.0

        'Friend Sub RegisterFunctions(lua As NLua.Lua)
        '    Dim methods = GetType(Utils).GetMethods(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
        '    For Each m In methods
        '        Dim mFunc = lua.RegisterFunction(m.Name, Me, m)
        '    Next
        'End Sub

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

        Public Function WaitForGameAndMeasureDuration() As Double
            Dim startTime = DateTime.Now
            WaitForGame()

            '1.0 to make sure it's not using integer division. 
            'Can't trust AmbiguousBasic.net to do everything correctly on its own amirite Kappa
            Return (1.0 * DateTime.Now.Subtract(startTime).Ticks / TimeSpan.TicksPerSecond)
        End Function

        Public Sub WaitUntilAfterNextLoadingScreen()
            Dim waitDuration As Double = 0
            Do
                waitDuration = WaitForGameAndMeasureDuration()
            Loop Until waitDuration >= MinLoadingScreenDur
        End Sub

        Public Function GetIngameDllAddress(moduleName As String) As UInteger

            Dim modules(255 - 1) As UInteger
            Dim cbNeeded As Integer = 0
            PSAPI.EnumProcessModules(DARKSOULS.GetHandle(), modules, 4 * 1024, cbNeeded)

            Dim numModules = cbNeeded / IntPtr.Size

            For i = 0 To numModules - 1

                Dim disModule = New IntPtr(modules(i))
                Dim name As New Text.StringBuilder()
                PSAPI.GetModuleBaseName(DARKSOULS.GetHandle(), disModule, name, 255)

                If (name.ToString().ToUpper().Equals(moduleName.ToUpper())) Then
                    Return modules(i)
                End If

            Next

            Return 0
        End Function

        Private Shared ReadOnly LuaCompatibleValueTypes As List(Of Type) = {
            GetType(String),
            GetType(LuaMethod),
            GetType(LuaTable),
            GetType(Single),
            GetType(Double),
            GetType(Byte),
            GetType(SByte),
            GetType(Int16),
            GetType(UInt16),
            GetType(Int32),
            GetType(UInt32),
            GetType(Int64),
            GetType(UInt64),
            GetType(Boolean)
        }.ToList()

        Public Sub BreakViewTable(table As LuaTable)
            Throw New Exception("BreakViewTable called.")
        End Sub

        Public Function GetClrObjMembers(obj As Object) As LuaTable
            Return GetClrObjMembers_Internal(obj, 0)
        End Function

        Private Function GetClrObjMembers_Internal(obj As Object, recursion As Integer) As LuaTable
            Dim tbl = New LuaTable()

            If obj Is Nothing Then
                Return tbl
            ElseIf recursion > 3 Then
                tbl(1) = "[REACHED MAX RECURSION]"
                Return tbl
            End If

            Dim objType = obj.GetType()

            If TypeOf obj Is Type Then
                tbl(1) = "Type: " & DirectCast(obj, Type).Name
                Return tbl
            End If

            Dim fieldArr = objType.GetFields(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)

            For Each field In fieldArr
                tbl(field.Name) = If(LuaCompatibleValueTypes.Contains(field.FieldType), field.GetValue(obj), GetClrObjMembers_Internal(field.GetValue(obj), recursion + 1))
            Next

            Dim propArr = objType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)

            For Each prop In propArr
                If prop.GetIndexParameters().Length > 0 Then
                    tbl(prop.Name) = "Indexed Property: " & prop.PropertyType.Name & "["
                Else
                    Dim propVal As Object = "[ERROR GETTING VALUE]"

                    Try
                        propVal = prop.GetValue(obj)
                    Catch ex As Exception

                    End Try

                    tbl(prop.Name) = If(LuaCompatibleValueTypes.Contains(prop.PropertyType), propVal, GetClrObjMembers_Internal(propVal, recursion + 1))
                End If
            Next
            Return tbl
        End Function

    End Class

End Namespace