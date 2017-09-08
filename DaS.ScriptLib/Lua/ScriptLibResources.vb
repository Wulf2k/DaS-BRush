Imports System.IO

Namespace Lua

    Public Class ScriptLibResources

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

        Shared Sub New()
            ThisAssembly = GetType(ScriptLibResources).Assembly
            EmbeddedResourceNames = ThisAssembly.GetManifestResourceNames().Select(Function(x) x.Substring(ResourcePathPrefix.Length)).ToArray() ' Removes "DaS.ScriptLib.Resources." from beginning

            initClls()
        End Sub

        Friend Shared Function GetEmbeddedTextResource(relPath As String) As String
            Dim result As String
            Using strm As Stream = GetType(ScriptLibResources).Assembly.GetManifestResourceStream(GetRelEmbedResPath(relPath))
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
            listBonfireNames = ParseItems(clsBonfires, clsBonfiresIDs, GetEmbeddedTextResource("CL.Bonfires.txt"))
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
            ParseItems(clsWeapons, clsWeaponsIDs, GetEmbeddedTextResource("CL.Weapons.txt"))
            ParseItems(clsArmor, clsArmorIDs, GetEmbeddedTextResource("CL.Armor.txt"))
            ParseItems(clsRings, clsRingsIDs, GetEmbeddedTextResource("CL.Rings.txt"))
            ParseItems(clsGoods, clsGoodsIDs, GetEmbeddedTextResource("CL.Goods.txt"))
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

End Namespace



