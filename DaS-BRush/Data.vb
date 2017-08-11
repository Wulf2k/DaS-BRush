Imports System.IO

Public Class Data

    Private Const NamespaceString As String = "DaS_BRush"
    Private Const ResourcePathPrefix As String = NamespaceString & "."
    Private Const ScriptDir As String = "Script"
    Public Const ScriptFileExtension = "2ks"
    Private Shared ReadOnly ThisAssembly As Reflection.Assembly
    Private Shared ReadOnly EmbeddedResourceNames As String() ' Useful for debugging lol

    Public Shared clsFuncNames As New Hashtable
    Public Shared clsFuncLocs As New Hashtable
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

    Public Shared customFuncNameFancyCase As New Dictionary(Of String, String)
    Public Shared customFuncMethodInfo As New Dictionary(Of String, Reflection.MethodInfo)
    Public Shared ingameFuncNameFancyCase As New Dictionary(Of String, String)

    Private Shared _scriptList As New Dictionary(Of String, Script)
    Public Shared Scripts As New ScriptListAccessor()
    Public Class ScriptListAccessor
        Default Public ReadOnly Property Item(ByVal scriptName As String) As Script
            Get
                Return _scriptList(scriptName)
            End Get
        End Property
    End Class

    Private Shared Sub LoadScript(scriptName As String)
        _scriptList.Add(scriptName, New Script(scriptName, GetTextRes(scriptName & "." & ScriptFileExtension)))
    End Sub

    Shared Sub New()
        ThisAssembly = Reflection.Assembly.GetExecutingAssembly()
        EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring(ResourcePathPrefix.Length)).ToArray() ' Removes "DaS_BRush.Resources." from beginning
        LoadAllScripts()
        initClls()
        EnumerateCustomFunctions()
    End Sub

    Private Shared Sub EnumerateCustomFunctions()
        Dim methods = GetType(Funcs).GetMethods()

        For Each m In methods
            'I tried checking for BindingFlags.Static but I kept getting an empty array.
            If m.Name = "ToString" Or m.Name = "Equals" Or m.Name = "GetHashCode" Or m.Name = "GetType" Then
                Continue For
            End If

            Dim attributes = m.CustomAttributes

            For Each a In attributes
                If a.AttributeType = GetType(HideFuncAttribute) AndAlso
                   a.ConstructorArguments.Count > 0 AndAlso
                   a.ConstructorArguments.First.ArgumentType = GetType(HideReason) AndAlso
                   Not CType(a.ConstructorArguments.First.Value, HideReason) = HideReason.DebugBypassHide Then
                    Continue For
                End If
            Next

            Dim nameFancy = m.Name
            Dim nameIndex = nameFancy.ToUpper()

            customFuncMethodInfo.Add(nameIndex, m)
            customFuncNameFancyCase.Add(nameIndex, nameFancy)
        Next
    End Sub



    Private Shared Sub LoadAllScripts()
        Dim scriptNames As String() = EmbeddedResourceNames.Where(Function(x) x.Split(".").Last.ToLower = ScriptFileExtension).ToArray()
        For Each scr In scriptNames
            LoadScript(scr.Substring(0, scr.Length - ScriptFileExtension.Length - 1)) 'remove extension from accessable name
        Next
    End Sub

    Private Shared Function GetTextRes(relPath As String) As String
        Dim result As String
        Using strm As Stream = ThisAssembly.GetManifestResourceStream(GetRelEmbedResPath(relPath))
            Using strmReader As New StreamReader(strm)
                result = strmReader.ReadToEnd()
            End Using
        End Using
        Return result
    End Function

    Private Shared Function GetRelEmbedResPath(relPath As String) As String
        Return ResourcePathPrefix & relPath
    End Function

    Public Shared Sub initClls()
        cllItemCats = {clsWeapons, clsArmor, clsRings, clsGoods}
        cllItemCatsIDs = {clsWeaponsIDs, clsArmorIDs, clsRingsIDs, clsGoodsIDs}
        Dim fancyFuncNames = ParseItems(clsFuncNames, clsFuncLocs, GetTextRes("CL.FuncLocs.txt"), True)

        For Each n In fancyFuncNames
            ingameFuncNameFancyCase.Add(n.ToUpper(), n)
        Next
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
        Data.ParseItems(clsWeapons, clsWeaponsIDs, GetTextRes("CL.Weapons.txt"))
        Data.ParseItems(clsArmor, clsArmorIDs, GetTextRes("CL.Armor.txt"))
        Data.ParseItems(clsRings, clsRingsIDs, GetTextRes("CL.Rings.txt"))
        Data.ParseItems(clsGoods, clsGoodsIDs, GetTextRes("CL.Goods.txt"))
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
