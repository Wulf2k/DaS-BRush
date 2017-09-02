Imports System.IO

'WILL GREATLY HELP YOU NOT WANT TO KILL YOURSELF IN InitIngameFuncTypes()
Imports IFRTT = System.Tuple(Of DaS.ScriptLib.IngameFuncReturnType, System.Type, String)

Public Class ScriptRes

    Private Const NamespaceString As String = "DaS.ScriptLib"
    Private Const ResourcePathPrefix As String = NamespaceString & "."
    Private Shared ReadOnly ThisAssembly As Reflection.Assembly
    Private Shared ReadOnly EmbeddedResourceNames As String() ' Useful for debugging lol

    Public Shared ReadOnly IngameFunctionsFileName As String = "IngameFunctions.txt"

    Public Shared clsBonfires As New Hashtable
    Public Shared clsBonfiresIDs As New Hashtable
    Public Shared clsItemCats As New Hashtable
    Public Shared clsItemCatsIDs As New Hashtable
    Public Shared cllItemCats As Hashtable()
    Public Shared cllItemCatsIDs As Hashtable()
    Public Shared clsWeapons As New Hashtable
    Public Shared clsWeaponsIDs As New Hashtable
    Public Shared clsArmor As New Hashtable
    Public Shared clsArmorIDs As New Hashtable
    Public Shared clsRings As New Hashtable
    Public Shared clsRingsIDs As New Hashtable
    Public Shared clsGoods As New Hashtable
    Public Shared clsGoodsIDs As New Hashtable

    Public Shared listBonfireNames As New List(Of String)

    'TODO: ORGANIZE THESE FUNCTION DICTIONARY/LIST CLUSTERFUCK INTO INDIVIDUAL OBJECTS THAT ENCAPSULATE EVERYTHING YOU NEED TO KNOW ABOUT THAT CATEGORY OF FUNCTIONS IN A SINGLE OBJECT


    Public Shared ReadOnly caselessIngameFuncNames As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Public Shared ReadOnly autoCompleteAdditionalTypes As Type()
    Public Shared ReadOnly propTypes As Dictionary(Of String, String)
    Public Shared ReadOnly autoCompleteFuncInfoByName As Dictionary(Of String, FuncInfo)
    Public Shared ReadOnly autoCompleteFuncInfoByName_FuncsClass As Dictionary(Of String, FuncInfo)

    Public Shared ReadOnly Property ingameFuncReturnTypes_ByName As Dictionary(Of String, IngameFuncReturnType)
    Public Shared ReadOnly Property types_ByIngameFuncReturnType As Dictionary(Of IngameFuncReturnType, Type)

    Public Shared ReadOnly Property IngameFuncReturnTypeEnumName As String

    Public Shared ReadOnly Property luaScriptHelperFuncInfoByName As Dictionary(Of String, FuncInfo)



    ''' <summary>
    ''' Needs to be called before IngameFuncInfos are created.
    ''' </summary>
    Private Shared Sub InitIngameFuncTypes()
        _IngameFuncReturnTypeEnumName = GetType(IngameFuncReturnType).Name

        _ingameFuncReturnTypes_ByName = New Dictionary(Of String, IngameFuncReturnType)
        _types_ByIngameFuncReturnType = New Dictionary(Of IngameFuncReturnType, Type)

        'IFRTT comes from the "Imports" statement at the top of the file
        'IngameFuncReturnType, System.Type, name in file
        Dim funcTypeTuples As IFRTT() = {
            New IFRTT(IngameFuncReturnType.Undefinerino, GetType(Integer), "?"),
            New IFRTT(IngameFuncReturnType.Boolerino, GetType(Boolean), "bool"),
            New IFRTT(IngameFuncReturnType.Floatarino, GetType(Single), "float"),
            New IFRTT(IngameFuncReturnType.Integerino, GetType(Integer), "int"),
            New IFRTT(IngameFuncReturnType.AsciiStringerino, GetType(String), "ascii_string"),
            New IFRTT(IngameFuncReturnType.UnicodeStringerino, GetType(String), "unicode_string")
        }

        For Each ifrt In funcTypeTuples
            ingameFuncReturnTypes_ByName.Add(ifrt.Item3, ifrt.Item1) 'String, IngameFuncReturnType
            types_ByIngameFuncReturnType.Add(ifrt.Item1, ifrt.Item2) 'IngameFuncReturnType, Type
        Next
    End Sub

    Shared Sub New()
        ThisAssembly = Reflection.Assembly.GetExecutingAssembly()
        EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring(ResourcePathPrefix.Length)).ToArray() ' Removes "DaS.ScriptLib.Resources." from beginning

        initClls()

        InitIngameFuncTypes()

        'autocomplete init shit im too lazy to move to its own method:
        Dim typerinos = ThisAssembly.GetExportedTypes().
            Where(Function(x) As Boolean
                      Return x.Namespace = NamespaceString AndAlso
                      Not x.Name.EndsWith("Handler") AndAlso
                      Not x.Name.EndsWith("Attribute") AndAlso
                      Not x.Name.EndsWith("Delegate") AndAlso
                      CheckCustomAttributes(x.CustomAttributes)
                  End Function).ToList()

        typerinos.Remove(GetType(Funcs))
        'typerinos.Remove(GetType(Lua.Help))
        autoCompleteAdditionalTypes = typerinos.ToArray()

        autoCompleteFuncInfoByName = New Dictionary(Of String, FuncInfo)
        autoCompleteFuncInfoByName_FuncsClass = New Dictionary(Of String, FuncInfo)

        propTypes = New Dictionary(Of String, String)

        For Each t In autoCompleteAdditionalTypes
            EnumerateCustomFuncInfo(autoCompleteFuncInfoByName, t, True)
            EnumerateCustomFields(t, propTypes)
        Next

        EnumerateCustomFuncInfo(autoCompleteFuncInfoByName_FuncsClass, GetType(Funcs), False)
        EnumerateCustomFuncInfo(autoCompleteFuncInfoByName, GetType(Funcs), False)
        EnumerateIngameFuncInfo(autoCompleteFuncInfoByName, GetTextRes(IngameFunctionsFileName).Split(Environment.NewLine))



        luaScriptHelperFuncInfoByName = New Dictionary(Of String, FuncInfo)

        'EnumerateCustomFuncInfo(luaScriptHelperFuncInfoByName, GetType(Lua.Help), False)

    End Sub

    Private Shared Function CheckCustomAttributes(attr As IEnumerable(Of Reflection.CustomAttributeData), Optional forceTrueGhetto As Boolean = False) As Boolean
        If forceTrueGhetto Then
            Return True
        End If
        For Each a In attr
            If a.AttributeType = GetType(HideFromScriptingAttribute) Then
                Return False
            End If
        Next
        Return True
    End Function

    Friend Shared Function GetFormattedTypeName(t As Type)
        Dim fn = t.FullName
        Dim ns = t.Namespace & "."
        Return If(fn.StartsWith(ns), fn.Substring(ns.Length), fn).Replace("+", ".")
    End Function

    Private Shared Sub EnumerateIngameFuncInfo(ByRef dict As Dictionary(Of String, FuncInfo), textLines As String())
        For i = 0 To textLines.Length - 1
            Dim trimmedLine As String = textLines(i).Trim()
            Dim commentStart = trimmedLine.IndexOf("#")
            'IndexOf returns -1 if search string isn't found.
            If commentStart >= 0 Then
                trimmedLine = trimmedLine.Substring(0, commentStart).Trim()
            End If

            'If either the line is blank or had nothing but a "#" comment.
            If String.IsNullOrWhiteSpace(trimmedLine) Then
                Continue For
            End If

            Dim fi = New IngameFuncInfo(trimmedLine, i + 1)

            If dict.ContainsKey(fi.Name) Then
                Throw New Exception($"Ingame func name {fi.Name} already exists within dictionary.")
            Else
                dict.Add(fi.Name, fi)
            End If

            caselessIngameFuncNames.Add(fi.Name.ToUpper(), fi.Name)
        Next
    End Sub

    Private Shared Sub EnumerateCustomFields(typerino As Type, ByRef dict As Dictionary(Of String, String))

        Dim fielderinos = typerino.GetFields()

        For Each f In fielderinos
            If f.IsAssembly OrElse
                Not f.IsStatic OrElse
                f.IsPrivate OrElse
                f.Name.EndsWith("EventHandler") OrElse
                f.Name.EndsWith("EventHandler.Invoke") OrElse
                f.Name.EndsWith("EventHandler.BeginInvoke") OrElse
                f.Name.EndsWith("EventHandler.EndInvoke") OrElse
                (Not CheckCustomAttributes(f.CustomAttributes)) Then
                Continue For
            End If

            Dim nameStr As String = GetFormattedTypeName(f.DeclaringType) & "." & f.Name

            If (Not dict.ContainsKey(nameStr)) Then
                dict.Add(nameStr, $"{GetGenericTypeFormatted(f.FieldType)} {nameStr}")
            End If
        Next
    End Sub

    Private Shared Sub EnumerateCustomFuncInfo(ByRef dict As Dictionary(Of String, FuncInfo), typerino As Type, prependTypeName As Boolean, Optional forceIncludeMembersWithHideAttribute As Boolean = False)
        Dim methods = typerino.GetMethods()

        For Each m In methods
            If Not m.IsStatic OrElse
                m.IsVirtual OrElse
                m.Name = "ToString" OrElse
                m.Name = "Equals" OrElse
                m.Name = "GetHashCode" OrElse
                m.Name = "GetType" OrElse
                m.Name.StartsWith("get_") OrElse
                m.Name.StartsWith("set_") OrElse
                m.Name.StartsWith("add_") OrElse
                m.Name.StartsWith("remove_") OrElse
                m.Name.StartsWith("op_") OrElse
                m.Name.EndsWith("EventHandler") OrElse
                m.Name.EndsWith("EventHandler.Invoke") OrElse
                m.Name.EndsWith("EventHandler.BeginInvoke") OrElse
                m.Name.EndsWith("EventHandler.EndInvoke") OrElse
                (Not CheckCustomAttributes(m.CustomAttributes, forceIncludeMembersWithHideAttribute)) _
            Then
                Continue For
            End If

            Dim cfi As New CustomFuncInfo(m, prependTypeName)

            If dict.ContainsKey(cfi.Name) Then
                Throw New Exception($"Custom func name {m.Name} already exists within dictionary.")
            Else
                dict.Add(cfi.Name, cfi)
            End If
        Next
    End Sub

    Friend Shared Function GetGenericTypeFormatted(t As Type)
        Dim genericArgs = t.GetGenericArguments()
        If Not (t.IsGenericType Or t.IsConstructedGenericType Or t.ContainsGenericParameters) Then
            Return t.Name
        End If
        Return If(Not t.IsGenericType, t.Name, t.Name.Replace("`" & genericArgs.Length.ToString(), "") & "<" & String.Join(", ", genericArgs.Select(Function(x) x.Name)) & ">")
    End Function

    Private Shared Function GetTextRes(relPath As String) As String
        Dim result As String
        Using strm As Stream = ThisAssembly.GetManifestResourceStream(GetRelEmbedResPath(relPath))
            Using strmReader As New StreamReader(strm)
                result = strmReader.ReadToEnd()
            End Using
        End Using
        Return result
    End Function

    Public Shared Function GetBonfireName(id As Integer) As String
        Return CType(clsBonfires(id), String)
    End Function

    Private Shared Function GetRelEmbedResPath(relPath As String) As String
        Return ResourcePathPrefix & relPath
    End Function

    Public Shared Sub initClls()
        cllItemCats = {clsWeapons, clsArmor, clsRings, clsGoods}
        cllItemCatsIDs = {clsWeaponsIDs, clsArmorIDs, clsRingsIDs, clsGoodsIDs}
        '-----------------------Bonfires-----------------------
        listBonfireNames = ParseItems(clsBonfires, clsBonfiresIDs, GetTextRes("CL.Bonfires.txt"))
        '-----------------------Item Categories-----------------------
        clsItemCats.Clear()
        clsItemCats.Add(0, "Weapons")
        clsItemCats.Add(268435456, "Armor")
        clsItemCats.Add(536870912, "Rings")
        clsItemCats.Add(1073741824, "Goods")

        clsItemCatsIDs.Clear()
        For Each itemCat In clsItemCats.Keys
            clsItemCatsIDs.Add(clsItemCats(itemCat), itemCat)
        Next

        '-----------------------Items-----------------------
        ScriptRes.ParseItems(clsWeapons, clsWeaponsIDs, GetTextRes("CL.Weapons.txt"))
        ScriptRes.ParseItems(clsArmor, clsArmorIDs, GetTextRes("CL.Armor.txt"))
        ScriptRes.ParseItems(clsRings, clsRingsIDs, GetTextRes("CL.Rings.txt"))
        ScriptRes.ParseItems(clsGoods, clsGoodsIDs, GetTextRes("CL.Goods.txt"))
    End Sub

    Public Shared Function ParseItems(ByRef cls As Hashtable, ByRef clsIDs As Hashtable, ByRef txt As String, Optional forceUppercaseKeys As Boolean = False) As List(Of String)
        Dim nameList As New List(Of String)
        Dim tmpList = txt.Replace(Chr(&HD), "").Split(Chr(&HA))
        Dim tmp1 As Integer
        Dim tmp2 As String

        cls.Clear()
        For i = 0 To tmpList.Length - 1
            If tmpList(i).Contains("|") Then
                tmp1 = tmpList(i).Split("|")(0)
                tmp2 = tmpList(i).Split("|")(1)
                cls.Add(tmp1, tmp2)
            End If
        Next

        nameList.Clear()
        clsIDs.Clear()
        For Each item In cls.Keys
            Dim name = CType(cls(item), String)
            Dim nameeeee = If(forceUppercaseKeys, name.ToUpper(), name)
            clsIDs.Add(nameeeee, item)
            nameList.Add(cls(item))
        Next

        nameList.Sort()
        Return nameList
    End Function

End Class