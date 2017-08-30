Public Class ParamInfo
    Public ReadOnly Property IsOptional As Boolean = False
    Public ReadOnly Property ParamType As Type
    Public ReadOnly Property IngameParamType As IngameFuncReturnType = IngameFuncReturnType.Undefinerino
    Public ReadOnly Property Name As String = "?FuncParamName?"
    Public ReadOnly Property HasDefaultValue As Boolean = False
    Public ReadOnly Property DefaultValue As Object = Nothing

    Public Overrides Function ToString() As String
        Dim pType = ScriptRes.GetGenericTypeFormatted(ParamType) & " "
        Dim pName = Name
        Dim pVal = If(HasDefaultValue, " = " & If(DefaultValue, "null"), "")
        Return $"{pType}{pName}{pVal}"
    End Function

    Public Sub New(info As Reflection.ParameterInfo)
        IsOptional = info.IsOptional
        ParamType = info.ParameterType
        Name = info.Name
        HasDefaultValue = info.HasDefaultValue
        DefaultValue = info.DefaultValue
    End Sub

    Public Sub New(str As String)
        Dim equalsIndex = str.IndexOf("=")
        Dim defaultValueSTRING = Nothing
        If equalsIndex >= 0 Then
            defaultValueSTRING = str.Substring(equalsIndex).Trim()
            str = str.Substring(0, equalsIndex).Trim()
        End If

        Dim splitStr = str.Split(" ")

        Dim hasDefinedType As Boolean = False

        For Each token In splitStr
            If Not hasDefinedType Then
                IngameParamType = ScriptRes.ingameFuncReturnTypes_ByName(token)
                ParamType = ScriptRes.types_ByIngameFuncReturnType(IngameParamType)
                hasDefinedType = True
            Else
                Name = token
                Exit For
            End If
        Next

        If equalsIndex >= 0 Then
            Dim conv As ComponentModel.TypeConverter = ComponentModel.TypeDescriptor.GetConverter(ParamType)
            DefaultValue = conv.ConvertFromString(defaultValueSTRING)
            'If it has a default value that means its optional m8
            IsOptional = True
        Else
            DefaultValue = Nothing
        End If
    End Sub
End Class
