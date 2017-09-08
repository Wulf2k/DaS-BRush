Imports DaS.ScriptLib.Lua

Namespace ModAPI
    Public Class DSMod
        Private __name As String
        Private __safeName As String
        Private __doLoadFuncStr As String

        Public Property DoLoadFunctionsLuaString As String
            Get
                Return __doLoadFuncStr
            End Get
            Friend Set(value As String)
                __doLoadFuncStr = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return __name
            End Get
            Friend Set(value As String)
                __name = value
                Try
                    'While lowercase \w means alphaumeric, uppercase \W means the exact opposite, and we want to replace those with underscores
                    __safeName = Text.RegularExpressions.Regex.Replace(value, "[\W]", "_")
                Catch e As Text.RegularExpressions.RegexMatchTimeoutException
                    __safeName = value
                End Try
            End Set
        End Property

        Public ReadOnly Property LUAI As LuaInterface

        Public ReadOnly Property ScriptText As String = ""

        Private OnInit As NLua.LuaFunction
        Private OnUpdate As NLua.LuaFunction

        Private _funcs As New Dictionary(Of ModFunc, NLua.LuaFunction)
        Public Funcs As ObjectModel.ReadOnlyDictionary(Of ModFunc, NLua.LuaFunction)

        Friend Sub CallAnonFunc(func As NLua.LuaFunction)

            Dim funcName As String = "NEW_FUNCTION_" & New Guid().ToString()

            LUAI.State.DoString($"{funcName} = loadstring([[{DoLoadFunctionsLuaString}]], '{funcName}')")
            LUAI.State.DoString($"{funcName}()")
            ' Unload function because we don't need it anymore.
            LUAI.State.DoString($"{funcName} = nil")

        End Sub

        Friend Sub Init()
            LUAI.State("LUAI") = LUAI
            LUAI.State("Mod") = Me

            Dim funcNames = [Enum].GetNames(GetType(ModFunc))

            For Each f In funcNames
                LUAI.State.DoString($"local function Mod:{f} () return end")
            Next

            Dim createFuncName As String = "NEW_FUNCTION_" & New Guid().ToString()



            For Each f In funcNames
                _funcs(f) = LUAI.State.Item($"Mod:{f}")
            Next

            Funcs = New ObjectModel.ReadOnlyDictionary(Of ModFunc, NLua.LuaFunction)(_funcs)
        End Sub

    End Class

End Namespace