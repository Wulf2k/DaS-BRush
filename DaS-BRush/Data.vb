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

    Public Shared listBonfireNames As New List(Of String)

    Shared Sub New()
        ThisAssembly = Reflection.Assembly.GetExecutingAssembly()
        EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring(ResourcePathPrefix.Length)).ToArray() ' Removes "DaS_BRush.Resources." from beginning
        LoadAllScripts()
        initClls()
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
        ParseItems(clsFuncNames, clsFuncLocs, GetTextRes("CL.FuncLocs.txt"))
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



    Public Shared Function ParseItems(ByRef cls As Hashtable, ByRef clsIDs As Hashtable, ByRef txt As String) As List(Of String)
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
            clsIDs.Add(cls(item), item)
            nameList.Add(cls(item))
        Next

        nameList.Sort()
        Return nameList
    End Function

End Class
