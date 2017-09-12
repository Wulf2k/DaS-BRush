
Imports System.Collections.ObjectModel
Imports System.Reflection
Imports DaS.ScriptLib.Lua.Structures

<AttributeUsage(AttributeTargets.Method, AllowMultiple:=True, Inherited:=True)>
Public Class LUAFAttribute
    Inherits Attribute
    ''' <summary>
    ''' The actual path used to access this member from within a Lua script.
    ''' Also serves to tell GUI's autocomplete what text to place.
    ''' </summary>
    Public LuaPath As String = Nothing
    Public Description As String = Nothing

    Private ____overriddenTypeNames As ReadOnlyDictionary(Of Type, String)
    Public ReadOnly Property OverriddenTypeNames As ReadOnlyDictionary(Of Type, String)
        Get
            If ____overriddenTypeNames Is Nothing Then
                Dim inputDict As New Dictionary(Of Type, String)

                inputDict.Add(GetType(Byte), "byte")
                inputDict.Add(GetType(Int16), "short")
                inputDict.Add(GetType(Int32), "int")
                inputDict.Add(GetType(Int64), "long")

                inputDict.Add(GetType(SByte), "sbyte")
                inputDict.Add(GetType(UInt16), "ushort")
                inputDict.Add(GetType(UInt32), "uint")
                inputDict.Add(GetType(UInt64), "ulong")

                inputDict.Add(GetType(Boolean), "bool")

                inputDict.Add(GetType(Single), "float")
                inputDict.Add(GetType(Double), "double")

                inputDict.Add(GetType(Char), "char")
                inputDict.Add(GetType(String), "string")
                inputDict.Add(GetType(BoxedStringAnsi), "string_ansi")
                inputDict.Add(GetType(BoxedStringUni), "string_uni")

                inputDict.Add(GetType(Void), "void")

                ____overriddenTypeNames = New ReadOnlyDictionary(Of Type, String)(inputDict)
            End If

            Return ____overriddenTypeNames
        End Get
    End Property

    Public Sub New(Optional Path As String = Nothing, Optional Desc As String = Nothing)

        'These seemingly-pointless nullable operators keep it from throwing a NullReferenceException ( ͡ ° ͜  ʖ ͡ °)
        LuaPath = If(Path, Nothing)
        Description = If(Desc, Nothing)

    End Sub

    Public Function GetMethodHeaderString(m As MethodInfo)

        Return $"{GetFancyTypeNameString(m.ReturnType)} {If(LuaPath, m.Name)}({String.Join(", ", m.GetParameters().Select(Function(p) GetFancyParameterString(p)))})"

    End Function

    Public Function GetFancyParameterString(p As ParameterInfo) As String
        Dim sb As New Text.StringBuilder()
        If p.IsOptional Then
            sb.Append("[")
        End If
        If p.IsOut Then
            sb.Append("out ")
        End If
        sb.Append(GetFancyTypeNameString(p.ParameterType))
        sb.Append(p.Name)

        If p.HasDefaultValue Then
            sb.Append(" = ")
            If p.DefaultValue Is Nothing Then
                sb.Append("null")
            Else
                sb.Append(p.DefaultValue.ToString())
            End If
        End If

        If p.IsOptional Then
            sb.Append("]")
        End If

        Return sb.ToString()
    End Function

    Public Function GetFancyTypeNameString(t As Type) As String
        If Not t.IsGenericType Then
            If OverriddenTypeNames.ContainsKey(t) Then
                Return OverriddenTypeNames(t)
            Else
                Return t.Name
            End If
        End If
        Dim sb As New Text.StringBuilder
        sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`")))
        sb.Append(t.GetGenericArguments().
            Aggregate("<",
                      Function(agg As String, typ As Type)
                          Return agg & If(agg = "<", "", ",") & GetFancyTypeNameString(typ)
                      End Function))
        sb.Append(">")

        Return sb.ToString()
    End Function

End Class
