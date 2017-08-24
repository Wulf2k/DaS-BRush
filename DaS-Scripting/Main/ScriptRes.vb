Imports System.IO

Public Class ScriptRes

    Private Const NamespaceString As String = "DaS_Scripting"
    Private Const ResourcePathPrefix As String = NamespaceString & "."
    Private Const ScriptDir As String = "Script"
    Public Const ScriptFileExtension = "2ks"
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


    Public Shared ReadOnly caselessIngameFuncNames As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Public Shared ReadOnly autoCompleteAdditionalTypes As Type()
    Public Shared ReadOnly propTypes As Dictionary(Of String, String)
    Public Shared ReadOnly autoCompleteFuncInfoByName As Dictionary(Of String, List(Of FuncInfo))
    Public Shared ReadOnly funcNames_Custom As List(Of String)
    Public Shared ReadOnly funcNames_Ingame As List(Of String)
    Public Shared ReadOnly funcNames_Additional As List(Of String)

    'Private Shared _scriptList As New Dictionary(Of String, LegacyScripting)
    'Public Shared Scripts As New ScriptListAccessor()
    'Public Class ScriptListAccessor
    '    Default Public ReadOnly Property Item(ByVal scriptName As String) As LegacyScripting
    '        Get
    '            Return _scriptList(scriptName)
    '        End Get
    '    End Property
    'End Class

    'Private Shared Sub LoadScript(scriptName As String)
    '    _scriptList.Add(scriptName, New LegacyScripting(scriptName, GetTextRes(scriptName & "." & ScriptFileExtension)))
    'End Sub

    Shared Sub New()
        ThisAssembly = Reflection.Assembly.GetExecutingAssembly()
        EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring(ResourcePathPrefix.Length)).ToArray() ' Removes "DaS_BRush.Resources." from beginning
        'LoadAllScripts()
        initClls()

        'autocomplete init shit im too lazy to move to its own method:
        Dim typerinos = ThisAssembly.GetExportedTypes().
            Where(Function(x)
                      Return x.Namespace = "DaS_Scripting" AndAlso
                      Not x.Name.EndsWith("Handler") AndAlso
                      Not x.Name.EndsWith("Attribute") AndAlso
                      Not x.Name.EndsWith("Delegate") AndAlso
                      CheckCustomAttributes(x.CustomAttributes)
                  End Function).ToList()

        typerinos.Remove(GetType(Funcs))
        autoCompleteAdditionalTypes = typerinos.ToArray()

        autoCompleteFuncInfoByName = New Dictionary(Of String, List(Of FuncInfo))
        funcNames_Custom = New List(Of String)
        funcNames_Ingame = New List(Of String)
        funcNames_Additional = New List(Of String)
        propTypes = New Dictionary(Of String, String)

        For Each t In autoCompleteAdditionalTypes
            EnumerateCustomFuncInfo(autoCompleteFuncInfoByName, funcNames_Additional, t, True)
            EnumerateCustomFields(t, propTypes)
        Next

        EnumerateCustomFuncInfo(autoCompleteFuncInfoByName, funcNames_Custom, GetType(Funcs), False)
        EnumerateIngameFuncInfo(autoCompleteFuncInfoByName, funcNames_Ingame, GetTextRes(IngameFunctionsFileName).Split(Environment.NewLine))
    End Sub

    Private Shared Function CheckCustomAttributes(attr As IEnumerable(Of Reflection.CustomAttributeData)) As Boolean
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

    Private Shared Sub EnumerateIngameFuncInfo(ByRef dict As Dictionary(Of String, List(Of FuncInfo)), ByRef funcNameList As List(Of String), textLines As String())
        Dim list = New List(Of FuncInfo)

        For i = 0 To textLines.Length - 1
            Dim fi = New IngameFuncInfo(textLines(i), i + 1)
            list.Add(fi)
            caselessIngameFuncNames.Add(fi.Name.ToUpper(), fi.Name)
        Next

        Dim appendDict = list.GroupBy(Function(x) x.Name).ToDictionary(Function(x) x.Key, Function(y) y.ToList())

        For Each kvp In appendDict
            funcNameList.Add(kvp.Key)

            If dict.ContainsKey(kvp.Key) Then
                dict(kvp.Key).AddRange(kvp.Value)
            Else
                dict.Add(kvp.Key, kvp.Value)
            End If
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

    Private Shared Sub EnumerateCustomFuncInfo(ByRef dict As Dictionary(Of String, List(Of FuncInfo)), ByRef funcNameList As List(Of String), typerino As Type, prependTypeName As Boolean)
        Dim methods = typerino.GetMethods()

        Dim list = New List(Of FuncInfo)

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
                (Not CheckCustomAttributes(m.CustomAttributes)) _
            Then
                Continue For
            End If

            Dim cfi = New CustomFuncInfo(m, prependTypeName)
            list.Add(cfi)
            funcNameList.Add(cfi.Name)
        Next

        Dim appendDict = list.GroupBy(Function(x) x.Name).ToDictionary(Function(x) x.Key, Function(y) y.ToList())

        For Each kvp In appendDict
            If dict.ContainsKey(kvp.Key) Then
                dict(kvp.Key).AddRange(kvp.Value)
            Else
                dict.Add(kvp.Key, kvp.Value)
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

    'Private Shared Sub LoadAllScripts()
    '    Dim scriptNames As String() = EmbeddedResourceNames.Where(Function(x) x.Split(".").Last.ToLower = ScriptFileExtension).ToArray()
    '    For Each scr In scriptNames
    '        LoadScript(scr.Substring(0, scr.Length - ScriptFileExtension.Length - 1)) 'remove extension from accessable name
    '    Next
    'End Sub

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
