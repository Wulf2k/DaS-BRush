Imports DaS.ScriptLib.Injection

Namespace Lua.Structures

    Public Class UtilsModule

        Public Const TableName As String = "Utils"

        Friend Sub RegisterFunctions(lua As NLua.Lua)
            Dim methods = GetType(UtilsModule).GetMethods(Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public)
            For Each m In methods
                Dim mFunc = lua.RegisterFunction(m.Name, Me, m)
            Next
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Function BitmaskCheck(input As ULong, mask As ULong) As Boolean
            Return ((input And mask) = mask)
        End Function

    End Class

End Namespace