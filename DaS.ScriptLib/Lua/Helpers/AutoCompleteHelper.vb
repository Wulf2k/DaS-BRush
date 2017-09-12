Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions
Imports DaS.ScriptLib.Lua.Structures

Namespace Lua.helpers

    Public Class AutoCompleteHelper

        Public reg As New Regex("--\[(=*)\[(.|\n)*?\]\1\]")



        Public Const IngameFunctionsFancyFilename = "DarkSoulsFunctions.lua"

        Private Shared ReadOnly Property _ingameFunctionsUnmarked As ReadOnlyCollection(Of LuaAutoCompleteEntry)
        Public Shared ReadOnly Property IngameFunctionsUnmarked As ReadOnlyCollection(Of LuaAutoCompleteEntry)
            Get
                If _ingameFunctionsUnmarked Is Nothing Then
                    Dim inputList As New List(Of LuaAutoCompleteEntry)

                    Dim fancy = IngameFunctionsFancy

                    Dim noDupes = LuaInterface.IngameFuncAddresses.Keys.Where(Function(k) Not fancy.Any(Function(x) x.CompletionText = k))

                    For Each trash In noDupes
                        inputList.Add(New LuaAutoCompleteEntry() With {
                                      .CompletionText = trash,
                                      .ListDisplayText = $"int? {trash}(...?)",
                                      .Description = "An ingame Lua function that has not been mapped out yet. If you wish to use it, you must explicitly enforce parameter value types."
                        })
                    Next

                    __ingameFunctionsUnmarked = inputList.AsReadOnly()
                End If
                Return _ingameFunctionsUnmarked
            End Get
        End Property
        Private Shared ReadOnly Property _ingameFunctionsFancy As ReadOnlyCollection(Of LuaAutoCompleteEntry)
        Public Shared ReadOnly Property IngameFunctionsFancy As ReadOnlyCollection(Of LuaAutoCompleteEntry)
            Get
                If _ingameFunctionsFancy Is Nothing Then
                    Dim inputList As New List(Of LuaAutoCompleteEntry)

                    Dim lua = RemoveLuaComments(ScriptLibResources.GetEmbeddedTextResource(IngameFunctionsFancyFilename))

                    Dim spl = lua.Split(vbCrLf).Where(Function(x) x.Trim().StartsWith("function")).ToList()

                    For Each line In spl

                        line = line.Substring("function ".Length, line.Length - "function ".Length)
                        Dim paramStart = line.IndexOf("(")

                        inputList.Add(New LuaAutoCompleteEntry() With {
                                      .CompletionText = line.Substring(0, paramStart),
                                      .ListDisplayText = "function " & line
                        })

                    Next

                    __ingameFunctionsFancy = inputList.AsReadOnly()
                End If
                Return _ingameFunctionsFancy
            End Get
        End Property



        Public Shared Function RemoveLuaComments(input As String) As String

            '(?s)--\[(=*)\[(.*?)\]\1\]
            '--\[(=*)\[(.|\n)*?\]\1\]
            'Return Regex.Replace(input, "\[\[((?>[^\[\[\]\]]|(?R))*\]\])", "") 'TODO


            Dim index As Integer = 0
            Dim sb As New Text.StringBuilder()

            Do
                Dim nextCommentStart = input.Substring(index).IndexOf("--")

                If nextCommentStart >= 0 Then

                    nextCommentStart += index 'change relative index to absolute

                    'Add the actual text before the comment block begins
                    sb.Append(input.Substring(index, nextCommentStart - index))

                    'Check if block comment
                    If nextCommentStart <= input.Length - 2 AndAlso input.Substring(nextCommentStart, 2) = "[[" Then
                        'Begin search for block end immediately after block begins
                        Dim nextBlockEnd = input.Substring(nextCommentStart + "[[".Length).IndexOf("]]")

                        'If no comment block end is found, stop looking; the rest of the script is commented out.
                        If nextBlockEnd < 0 Then
                            Exit Do
                        End If

                        nextBlockEnd += nextCommentStart + "[[".Length 'change relative index to absolute

                        'Move index to just after the end of the block comment and continue.
                        index = nextBlockEnd + "]]".Length
                    Else
                        'Begin search for line end end immediately after comment begins
                        Dim nextBlockEnd = input.Substring(nextCommentStart + "--".Length).IndexOf(vbCrLf)

                        'If no newline is found, stop looking; the rest of the script is commented out.
                        If nextBlockEnd < 0 Then
                            Exit Do
                        End If

                        nextBlockEnd += nextCommentStart + "--".Length 'change relative index to absolute

                        'Move index to just after the end of the new line and continue.
                        index = nextBlockEnd + vbCrLf.Length
                    End If


                Else
                    'No comment Kappa
                    Exit Do
                End If
            Loop Until index >= input.Length

            Return sb.ToString()

        End Function

        'Private Shared Function __removeLuaLineComments(input As String) As String
        '    Dim sb As New Text.StringBuilder()

        '    Dim allLines = input.Split(vbCrLf)

        '    For Each line In allLines
        '        Dim commentStart = line.IndexOf("--")
        '        If commentStart >= 0 Then
        '            sb.AppendLine(line.Substring(0, commentStart))
        '        Else
        '            sb.AppendLine(line)
        '        End If
        '    Next

        '    Return sb.ToString()
        'End Function

    End Class

End Namespace
