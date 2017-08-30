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

    Private _paramList As List(Of ParamInfo) = New List(Of ParamInfo)
    Public Property ParamList As List(Of ParamInfo)
        Get
            Return _paramList
        End Get
        Protected Set(value As List(Of ParamInfo))
            _paramList = value
        End Set
    End Property

    Public MustOverride ReadOnly Property UsageString As String
End Class

Public Class CustomFuncInfo
    Inherits FuncInfo
    Public ReadOnly Property ReturnType As String
    Public ReadOnly Property MethodDefinition As Reflection.MethodInfo

    Public Overrides ReadOnly Property UsageString As String
        Get
            Return $"{ReturnType} {Name}({String.Join(", ", ParamList.Select(Function(p) p.ToString()))})"
        End Get
    End Property

    Public Sub New(mi As Reflection.MethodInfo, prependTypeName As Boolean)
        Name = If(prependTypeName, ScriptRes.GetFormattedTypeName(mi.DeclaringType) & ".", "") & mi.Name
        ParamList = mi.GetParameters().Select(Function(p) New ParamInfo(p)).ToList()

        MethodDefinition = mi
        ReturnType = ScriptRes.GetGenericTypeFormatted(mi.ReturnType)
    End Sub

End Class

Public Class IngameFuncInfo
    Inherits FuncInfo

    Public ReadOnly Property Address As Integer
    Public ReadOnly Property ReturnType As IngameFuncReturnType
    Public ReadOnly Property ReturnTypeDisplayName As String
    Public ReadOnly Property ReturnValueDescription As String

    Public Overrides ReadOnly Property UsageString As String
        Get
            Return $"{ScriptRes.GetFormattedTypeName(ScriptRes.types_ByIngameFuncReturnType(ReturnType))} {Name}({String.Join(", ", ParamList.Select(Function(p) p.ToString()))})"
        End Get
    End Property

    Public Sub New(lineText As String, lineNumber As Integer)
        Dim splitStr = lineText.Split("|").Select(Function(x) x.Trim()).ToArray()
        Try
            Address = Integer.Parse(splitStr(0))
            Name = splitStr(1)
            ReturnType = IngameFuncReturnType.Undefinerino
            ReturnTypeDisplayName = "function"
            ReturnValueDescription = "???"

            If splitStr.Length >= 3 Then
                ParamList = splitStr(2).Split(",").Select(Function(p) New ParamInfo(p.Trim())).ToList()
                If splitStr.Length >= 4 Then
                    ReturnType = ScriptRes.ingameFuncReturnTypes_ByName(splitStr(3))
                    If Not (ReturnType = IngameFuncReturnType.Undefinerino) Then
                        ReturnTypeDisplayName = ScriptRes.GetFormattedTypeName(ScriptRes.types_ByIngameFuncReturnType(ReturnType))
                    End If

                    If splitStr.Length >= 5 Then
                        ReturnValueDescription = splitStr(4)
                    End If
                End If
            Else
                ParamList = New List(Of ParamInfo)

                For i = 1 To 10
                    ParamList.Add(New ParamInfo($"? param{i}"))
                Next

            End If
        Catch ex As Exception
            Throw New Exception($"Line #{lineNumber} in {ScriptRes.IngameFunctionsFileName} (""{lineText}"") could not be parsed.", ex)
        End Try
    End Sub

End Class