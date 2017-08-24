Public MustInherit Class FuncInfo
    Private _name As String
    Public Property Name As String
        Get
            Return _name
        End Get
        Protected Set(value As String)
            _name = value
        End Set
    End Property
    Public MustOverride ReadOnly Property UsageString As String
End Class

Public Class CustomFuncInfo
    Inherits FuncInfo
    Public ReadOnly Property Params As String
    Public ReadOnly Property ReturnType As String

    Public Overrides ReadOnly Property UsageString As String
        Get
            Return $"{ReturnType} {Name}({Params})"
        End Get
    End Property

    Public Sub New(mi As Reflection.MethodInfo, prependTypeName As Boolean)
        Name = If(prependTypeName, ScriptRes.GetFormattedTypeName(mi.DeclaringType) & ".", "") & mi.Name

        Params = String.Join(", ", mi.GetParameters().
            Select(Function(p)
                       Dim pOut = If(p.IsOut, "out ", "")
                       Dim pType = ScriptRes.GetGenericTypeFormatted(p.ParameterType) & " "
                       Dim pName = p.Name
                       Dim pVal = If(p.HasDefaultValue, " = " & If(p.DefaultValue, "null"), "")
                       Return $"{pOut}{pType}{pName}{pVal}"
                   End Function))

        ReturnType = ScriptRes.GetGenericTypeFormatted(mi.ReturnType)
    End Sub
End Class

Public Class IngameFuncInfo
    Inherits FuncInfo

    Public ReadOnly Property Address As Integer
    Public ReadOnly Property UsageText As String

    Public Overrides ReadOnly Property UsageString As String
        Get
            Return $"{UsageText}"
        End Get
    End Property

    Public Sub New(lineText As String, lineNumber As Integer)
        Dim splitStr = lineText.Split("|")
        Try
            Address = Integer.Parse(splitStr(0))
            Dim restStart = splitStr(1).IndexOf("(")
            If restStart >= 0 Then
                Name = splitStr(1).Substring(0, restStart)
                UsageText = splitStr(1)

                If splitStr.Length = 3 Then
                    UsageText = splitStr(2).Trim() & " " & splitStr(1)
                Else
                    UsageText = splitStr(1)
                End If
            Else
                Name = splitStr(1)
                UsageText = "function " & splitStr(1) & "(???)"
            End If
        Catch ex As Exception
            Throw New Exception($"Line #{lineNumber} in {ScriptRes.IngameFunctionsFileName} (""{lineText}"") could not be parsed.", ex)
        End Try
    End Sub

End Class