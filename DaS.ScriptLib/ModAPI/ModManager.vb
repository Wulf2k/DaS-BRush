Imports DaS.ScriptLib.Lua

Namespace ModAPI

    Public Class ModManager

        Private Shared __mods As New Dictionary(Of String, DSMod)
        Public Shared Mods As ObjectModel.ReadOnlyDictionary(Of String, DSMod)

        Public Shared Function LoadFromFile(name As String, filePath As String) As DSMod
            Return LoadFromString(name, IO.File.ReadAllText(filePath))
        End Function

        Public Shared Function LoadFromString(name As String, str As String) As DSMod
            __mods.Add(name, New DSMod() With {.Name = name, .DoLoadFunctionsLuaString = str})
            Return Mods(name)
        End Function

        Public Shared Function LoadAllModsInDirectory(dir As String) As Integer

            If Not IO.Directory.Exists(dir) Then
                Return 0
            End If

            For Each file In IO.Directory.GetFiles(dir)
                LoadFromFile(New IO.FileInfo(file).Name, file)
            Next

            Return Mods.Count

        End Function

        Public Shared Sub ModCall(funcName As String, ParamArray args As Object())

            For Each kvp In Mods
                kvp.Value.Funcs(funcName).Call(args)
            Next

        End Sub

        Public Shared Function ModCallAndReturnAll(funcName As String, ParamArray args As Object()) As Dictionary(Of String, Object())

            Dim result As New Dictionary(Of String, Object())

            For Each kvp In Mods
                result.Add(kvp.Key, kvp.Value.Funcs(funcName).Call(args))
            Next

            Return result
        End Function

        Public Shared Sub InitAllMods()
            For Each kvp In Mods
                kvp.Value.Init()
            Next
        End Sub

    End Class

End Namespace
