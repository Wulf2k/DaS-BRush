Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading
Imports System.Globalization
Imports System.Reflection

Public Class frmForm1



    'TODO:
    'Check equipment durability
    'Handle equipment management

    'Set tails to be pre-cut to avoid drops
    'Centipede, intro cinematic

    'Asylum Demon, Close that damn door

    'Bed of Chaos, reset platform-collapsing

    'Grant ring for 4K fight

    'Force animation through fog gate?

    '50001550 = Stop Rite of Kindling dropping?
    'Check ItemLotParam for boss soul EventFlags


    Public Const VersionCheckUrl = "http://wulf2k.ca/pc/das/das-brush-ver.txt"
    Public Const NoteCheckUrl = "http://wulf2k.ca/pc/das/das-brush-notes.txt"

    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Private trd As Thread
    Private rushTimer As Thread




    Dim clsFuncNames As New Hashtable
    Dim clsFuncLocs As New Hashtable

    Dim clsBonfires As New Hashtable
    Dim clsBonfiresIDs As New Hashtable

    Dim clsItemCats As New Hashtable
    Dim clsItemCatsIDs As New Hashtable


    Dim cllItemCats As Hashtable()
    Dim cllItemCatsIDs As Hashtable()

    Dim clsWeapons As New Hashtable
    Dim clsWeaponsIDs As New Hashtable

    Dim clsArmor As New Hashtable
    Dim clsArmorIDs As New Hashtable

    Dim clsRings As New Hashtable
    Dim clsRingsIDs As New Hashtable

    Dim clsGoods As New Hashtable
    Dim clsGoodsIDs As New Hashtable

    Dim nodeDumpPtr As Integer = 0


    Dim charptr1 As Integer
    Dim charmapdataptr As Integer
    Dim charposdataptr As Integer
    Dim charptr2 As Integer
    Dim charptr3 As Integer
    Dim enemyptr As Integer
    Dim enemyptr2 As Integer
    Dim enemyptr3 As Integer
    Dim enemyptr4 As Integer
    Dim tendptr As Integer


    Dim gamestatsptr As Integer
    Dim bonfireptr As Integer

    Dim funcPtr As Integer
    Dim dropPtr As Integer


    Dim delay As Integer = 33


    Dim playerHP As Integer
    Dim playerStam As Integer

    Dim playerMaxHP As Integer
    Dim playerMaxStam As Integer

    Dim playerFacing As Single
    Dim playerXpos As Single
    Dim playerYpos As Single
    Dim playerZpos As Single

    Dim rushMode As Boolean = False
    Dim rushName As String = ""

    Dim GenDiagResponse As Integer
    Dim GenDiagVal As Integer


    Private Async Sub updatecheck()
        Try
            Dim client As New Net.WebClient()
            Dim content As String = Await client.DownloadStringTaskAsync(VersionCheckUrl)

            Dim lines() As String = content.Split({vbCrLf, vbLf}, StringSplitOptions.None)
            Dim stableVersion = lines(0)
            Dim stableUrl = lines(1)


            If stableVersion > lblVer.Text.Replace("-", "") Then
                btnUpdate.Visible = True
                btnUpdate.Tag = stableUrl
            Else
                btnUpdate.Visible = False
            End If

            content = Await client.DownloadStringTaskAsync(NoteCheckUrl)
            txtNotes.Text = content

        Catch ex As Exception
            'Fail silently since nobody wants to be bothered for an update check.
        End Try
    End Sub



    Private Sub initClls()
        Dim nameList As New List(Of String)

        cllItemCats = {clsWeapons, clsArmor, clsRings, clsGoods}
        cllItemCatsIDs = {clsWeaponsIDs, clsArmorIDs, clsRingsIDs, clsGoodsIDs}


        ParseItems(clsFuncNames, clsFuncLocs, My.Resources.FuncLocs)
        '-----------------------Bonfires-----------------------
        nameList = ParseItems(clsBonfires, clsBonfiresIDs, My.Resources.Bonfires)
        For Each bonfire In nameList
            cmbBonfire.Items.Add(bonfire)
        Next
        cmbBonfire.SelectedItem = "Nothing"


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
        ParseItems(clsWeapons, clsWeaponsIDs, My.Resources.Weapons)
        ParseItems(clsArmor, clsArmorIDs, My.Resources.Armor)
        ParseItems(clsRings, clsRingsIDs, My.Resources.Rings)
        ParseItems(clsGoods, clsGoodsIDs, My.Resources.Goods)

    End Sub

    Private Function ParseItems(ByRef cls As Hashtable, ByRef clsIDs As Hashtable, ByRef txt As String) As List(Of String)
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



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim oldFileArg As String = Nothing
        For Each arg In Environment.GetCommandLineArgs().Skip(1)
            If arg.StartsWith("--old-file=") Then
                oldFileArg = arg.Substring("--old-file=".Length)
            Else
                MsgBox("Unknown command line arguments")
                oldFileArg = Nothing
                Exit For
            End If
        Next
        If oldFileArg IsNot Nothing Then
            If oldFileArg.EndsWith(".old") Then
                Dim t = New Thread(
                Sub()
                    Try
                        'Give the old version time to shut down
                        Thread.Sleep(1000)
                        File.Delete(oldFileArg)
                    Catch ex As Exception
                        Me.Invoke(Function() MsgBox("Deleting old version failed: " & vbCrLf & ex.Message, MsgBoxStyle.Exclamation))
                    End Try
                End Sub)
                t.Start()
            Else
                MsgBox("Deleting old version failed: Invalid filename ", MsgBoxStyle.Exclamation)
            End If
        End If

        updatecheck()

        initClls()

        refTimer = New System.Windows.Forms.Timer
        refTimer.Interval = delay
        refTimer.Enabled = True


        If ScanForProcess("DARK SOULS", True) Then
            'Check if this process is even Dark Souls
            checkDarkSoulsVersion()
            If isHooked Then
                dropAlloc()
                funcAlloc()

                setsaveenable(False)
            End If
        End If


    End Sub

    Private Sub funcAlloc()
        Dim TargetBufferSize = 1024
        funcPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Sub
    Private Sub dropAlloc()
        Dim TargetBufferSize = 1024
        dropPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Sub

    Private Sub checkDarkSoulsVersion()
        isHooked = True
        refTimer.Enabled = True


        If (RUInt32(&H400080) = &HFC293654&) Then
            lblRelease.Text = "Dark Souls (Latest Release Ver.)"

            Dim tmpProtect As Integer
            VirtualProtectEx(_targetProcessHandle, &H10CC000, &H1DE000, 4, tmpProtect)

        Else
            If (RUInt32(&H400080) = &HE91B11E2&) Then
                lblRelease.Text = "Dark Souls (Invalid Beta)"
            Else
                lblRelease.Text = "None"
            End If
            isHooked = False
            refTimer.Enabled = False
        End If
    End Sub

    Private Sub refTimer_Tick() Handles refTimer.Tick



        If Not trd Is Nothing Then

            If trd.ThreadState = &H10 Or trd.ThreadState = &H100 Then
                gbBosses.Visible = True
                btnBeginBossRush.Enabled = True
                btnBeginReverseRush.Enabled = True
                btnX.Visible = False
                btnX.Enabled = False
            Else
                gbBosses.Visible = False
                btnBeginBossRush.Enabled = False
                btnBeginReverseRush.Enabled = False
                btnX.Visible = True
                btnX.Enabled = True
            End If
        Else
            gbBosses.Visible = True
            btnBeginBossRush.Enabled = True
            btnBeginReverseRush.Enabled = True
            btnX.Visible = False
            btnX.Enabled = False
        End If

        checkDarkSoulsVersion()

        If Not isHooked Then
            Return
        End If

        bonfireptr = RUInt32(&H13784A0)
        charptr1 = RInt32(&H137DC70)
        charptr1 = RInt32(charptr1 + &H4)
        charptr1 = RInt32(charptr1)

        gamestatsptr = RUInt32(&H1378700)
        charptr2 = RUInt32(gamestatsptr + &H8)

        charmapdataptr = RInt32(charptr1 + &H28)
        charposdataptr = RInt32(charmapdataptr + &H1C)




        Select Case tabs.TabPages(tabs.SelectedIndex).Text
            Case "Player"
                playerHP = RInt32(charptr1 + &H2D4)
                playerMaxHP = RInt32(charptr1 + &H2D8)

                lblHP.Text = playerHP & " / " & playerMaxHP
                lblStam.Text = playerStam & " / " & playerMaxStam

                playerStam = RInt32(charptr1 + &H2E4)
                playerMaxStam = RInt32(charptr1 + &H2E8)

                playerFacing = (RFloat(charposdataptr + &H4) + Math.PI) / (Math.PI * 2) * 360
                playerXpos = RFloat(charposdataptr + &H10)
                playerYpos = RFloat(charposdataptr + &H14)
                playerZpos = RFloat(charposdataptr + &H18)

                Dim stableXpos As Single
                Dim stableYpos As Single
                Dim stableZpos As Single

                Dim tmpptr As Integer
                tmpptr = &H13784A0

                tmpptr = RInt32(tmpptr)

                stableXpos = RFloat(tmpptr + &HB70)
                stableYpos = RFloat(tmpptr + &HB74)
                stableZpos = RFloat(tmpptr + &HB78)


                lblFacing.Text = "Heading: " & playerFacing.ToString("0.00") & "°"
                lblXpos.Text = playerXpos.ToString("0.00")
                lblYpos.Text = playerYpos.ToString("0.00")
                lblZpos.Text = playerZpos.ToString("0.00")

                lblstableXpos.Text = stableXpos.ToString("0.00")
                lblstableYpos.Text = stableYpos.ToString("0.00")
                lblstableZpos.Text = stableZpos.ToString("0.00")

                Dim bonfireID As Integer
                bonfireID = RInt32(bonfireptr + &HB04)
                If Not cmbBonfire.DroppedDown Then
                    If clsBonfires(bonfireID) = "" Then
                        clsBonfires.Add(bonfireID, bonfireID.ToString)
                        clsBonfiresIDs.Add(bonfireID.ToString, bonfireID)
                        cmbBonfire.Items.Add(bonfireID.ToString)
                    End If
                    cmbBonfire.SelectedItem = clsBonfires(bonfireID)
                End If


            Case "Stats"
                Try
                    If Not nmbMaxHP.Focused Then nmbMaxHP.Value = RInt32(charptr2 + &H14)
                    If Not nmbMaxStam.Focused Then nmbMaxStam.Value = RInt32(charptr2 + &H30)

                    If Not nmbVitality.Focused Then nmbVitality.Value = RInt32(charptr2 + &H38)
                    If Not nmbAttunement.Focused Then nmbAttunement.Value = RInt32(charptr2 + &H40)
                    If Not nmbEnd.Focused Then nmbEnd.Value = RInt32(charptr2 + &H48)
                    If Not nmbStr.Focused Then nmbStr.Value = RInt32(charptr2 + &H50)
                    If Not nmbDex.Focused Then nmbDex.Value = RInt32(charptr2 + &H58)
                    If Not nmbIntelligence.Focused Then nmbIntelligence.Value = RInt32(charptr2 + &H60)
                    If Not nmbFaith.Focused Then nmbFaith.Value = RInt32(charptr2 + &H68)

                    If Not nmbHumanity.Focused Then nmbHumanity.Value = RInt32(charptr2 + &H7C)
                    If Not nmbResistance.Focused Then nmbResistance.Value = RInt32(charptr2 + &H80)

                    If Not nmbGender.Focused Then nmbGender.Value = RInt32(charptr2 + &HC2)

                    If Not nmbClearCount.Focused Then nmbClearCount.Value = RInt32(gamestatsptr + &H3C)

                Catch ex As Exception

                End Try
        End Select


    End Sub





    Public Sub dropitem(ByVal cat As String, item As String, num As Integer)
        Dim TargetBufferSize = 1024
        Dim Rtn As Integer

        Dim bytes() As Byte
        Dim bytes2() As Byte

        Dim bytcat As Integer = &H1
        Dim bytitem As Integer = &H6
        Dim bytcount As Integer = &H10
        Dim bytptr1 As Integer = &H15
        Dim bytptr2 As Integer = &H32
        Dim bytjmp As Integer = &H38

        bytes = {&HBD, 0, 0, 0, 0, &HBB, &HF0, &H0, &H0, &H0, &HB9, &HFF, &HFF, &HFF, &HFF, &HBA, 0, 0, 0, 0, &HA1, &HD0, &H86, &H37, &H1, &H89, &HA8, &H28, &H8, &H0, &H0, &H89, &H98, &H2C, &H8, &H0, &H0, &H89, &H88, &H30, &H8, &H0, &H0, &H89, &H90, &H34, &H8, &H0, &H0, &HA1, &HBC, &HD6, &H37, &H1, &H50, &HE8, 0, 0, 0, 0, &HC3}

        'cllItemCatsIDs(clsItemCatsIDs("Weapons") / &H10000000)("Target Shield+15"))

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(clsItemCatsIDs(cat)))
        Array.Copy(bytes2, 0, bytes, bytcat, bytes2.Length)

        Dim tmpCat As Integer
        tmpCat = Convert.ToInt32(clsItemCatsIDs(cat) / &H10000000)
        If tmpCat = 4 Then tmpCat = 3

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(cllItemCatsIDs(tmpCat)(item)))
        Array.Copy(bytes2, 0, bytes, bytitem, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(num))
        Array.Copy(bytes2, 0, bytes, bytcount, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H13786D0))
        Array.Copy(bytes2, 0, bytes, bytptr1, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H137D6BC))
        Array.Copy(bytes2, 0, bytes, bytptr2, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(0 - ((dropPtr + &H3C) - (&HDC8C60))))
        Array.Copy(bytes2, 0, bytes, bytjmp, bytes2.Length)

        Rtn = WriteProcessMemory(_targetProcessHandle, dropPtr, bytes, TargetBufferSize, 0)
        'MsgBox(Hex(dropPtr))
        CreateRemoteThread(_targetProcessHandle, 0, 0, dropPtr, 0, 0, 0)

        Thread.Sleep(5)
    End Sub
    Public Sub funccall_old(ByVal func As String, LUAparams() As String)
        Dim bytes() As Byte
        Dim bytes2() As Byte

        Dim bytParams = New Integer() {&H1D, &H17, &H11, &HB, &H5}
        Dim bytJmp As Integer = &H23

        bytes = {&H55, &H8B, &HEC, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HE8, 0, 0, 0, 0, &H58, &H58, &H58, &H58, &H58, &H58, &H8B, &HE5, &H5D, &HC3}

        For i As Integer = 4 To 0 Step -1
            If LUAparams(i) = "False" Then LUAparams(i) = 0
            If LUAparams(i) = "True" Then LUAparams(i) = 1
            If LUAparams(i).Contains(".") Then
                bytes2 = BitConverter.GetBytes(Convert.ToSingle(LUAparams(i), New CultureInfo("en-us")))
            Else
                bytes2 = BitConverter.GetBytes(Convert.ToInt32(LUAparams(i), New CultureInfo("en-us")))
            End If

            Array.Copy(bytes2, 0, bytes, bytParams(i), bytes2.Length)
        Next
        bytes2 = BitConverter.GetBytes(CInt(0 - ((funcPtr + bytJmp + 4) - clsFuncLocs(func))))

        Array.Copy(bytes2, 0, bytes, bytJmp, bytes2.Length)
        WriteProcessMemory(_targetProcessHandle, funcPtr, bytes, 1024, 0)
        CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr, 0, 0, 0)
        Thread.Sleep(5)
    End Sub


    Public Function funccall(func As String, param1 As String, param2 As String, param3 As String, param4 As String, param5 As String) As Integer

        Dim Params() As String = {param1, param2, param3, param4, param5}
        Dim param As IntPtr = Marshal.AllocHGlobal(4)
        Dim intParam As Integer
        Dim floatParam As Single
        Dim a As New asm



        a.pos = funcPtr
        a.AddVar("funcloc", clsFuncLocs(func))
        a.AddVar("returnedloc", funcPtr + &H200)

        a.Asm("push ebp")
        a.Asm("mov ebp,esp")
        a.Asm("push eax")

        'Parse params, add as variables to the ASM
        For i As Integer = 4 To 0 Step -1
            If Params(i).ToLower = "false" Then Params(i) = "0"
            If Params(i).ToLower = "true" Then Params(i) = "1"
            If Params(i).Length < 1 Then Params(i) = "0"

            If Params(i).Contains(".") Then
                floatParam = Convert.ToSingle(Params(i), New CultureInfo("en-us"))
                Marshal.StructureToPtr(floatParam, param, False)
                a.AddVar("param" & i, Marshal.ReadInt32(param))
            Else
                intParam = Convert.ToInt32(Params(i), New CultureInfo("en-us"))
                a.AddVar("param" & i, intParam)
            End If

            a.Asm("mov eax,param" & i)
            a.Asm("push eax")

        Next
        a.Asm("call funcloc")
        a.Asm("mov ebx,returnedloc")
        a.Asm("mov [ebx],eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("mov esp,ebp")
        a.Asm("pop ebp")
        a.Asm("ret")

        Marshal.FreeHGlobal(param)


        WriteProcessMemory(_targetProcessHandle, funcPtr, a.bytes, 1024, 0)
        MsgBox(Hex(funcPtr))
        CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr, 0, 0, 0)
        Thread.Sleep(1)



        Return RInt32(funcPtr + &H200)


    End Function


    Public Sub SetEventFlag(ByVal flag As Integer, state As Boolean)
        funccall_old("SetEventFlag", {flag, state And 1, 0, 0, 0})
    End Sub
    Public Sub Warp(ByVal entityID As Integer, point As Integer)
        funccall_old("Warp", {entityID, point, 0, 0, 0})
    End Sub
    Public Sub WarpNextStage(ByVal world As Integer, block As Integer, area As Integer)
        funccall_old("WarpNextStage", {world, block, 0, 0, area})
    End Sub
    Public Sub WarpNextStage_Bonfire(ByVal bonfireID As Integer)
        funccall_old("WarpNextStage_Bonfire", {bonfireID, 0, 0, 0, 0})
    End Sub
    Public Sub warp_coords(ByVal x As Single, y As Single, z As Single)
        WFloat(charmapdataptr + &HD0, x)
        WFloat(charmapdataptr + &HD4, y)
        WFloat(charmapdataptr + &HD8, z)
        WBytes(charmapdataptr + &HC8, {1})
    End Sub

    Public Sub blackscreen()
        Dim tmpptr As Integer
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        WFloat(tmpptr + &H270, 0)
        WFloat(tmpptr + &H274, 0)
        WFloat(tmpptr + &H278, 0)
    End Sub
    Public Sub clearplaytime()
        Dim tmpPtr As Integer = RIntPtr(&H1378700)
        WInt32(tmpPtr + &H68, 0)
    End Sub
    Public Sub disableai(ByVal state As Byte)
        WBytes(&H13784EE, {state})
    End Sub
    Public Sub fadein()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378520)
        tmpptr = RInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 0.0


        For i = 0 To 33
            val = val + 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next

        WBytes(tmpptr + &H26D, {0})
    End Sub
    Public Sub fadeout()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378520)
        tmpptr = RInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 1.0





        For i = 0 To 33
            val = val - 0.03
            val = val - 0.03
            val = val - 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next
    End Sub

    Public Sub setcampos(ByVal xpos As Single, ypos As Single, zpos As Single, xrot As Single, yrot As Single)
        Dim tmpPtr As Integer

        tmpPtr = RInt32(&H1378714)

        WFloat(tmpPtr + &HB0, xpos)
        WFloat(tmpPtr + &HB4, ypos)
        WFloat(tmpPtr + &HB8, zpos)


        tmpPtr = RInt32(&H137D6DC)
        tmpPtr = RInt32(tmpPtr + &H3C)
        tmpPtr = RInt32(tmpPtr + &H60)


        WFloat(tmpPtr + &H144, xrot)
        WFloat(tmpPtr + &H150, yrot)



    End Sub
    Public Sub setfreecam(ByVal state As Boolean)
        If state Then
            'WBytes(&HEFDBAF, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&H404E59, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&H404E63, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&HF06C46, {&H90, &H90, &H90, &H90, &H90, &H90, &H90, &H90})




        Else
            'WBytes(&HEFDBAF, {&HE8, &H7c, &H72, &H50, &HFF})
            WBytes(&H404E59, {&H66, &HF, &HD6, &H46, &H20})
            WBytes(&H404E63, {&H66, &HF, &HD6, &H46, &H28})
            WBytes(&HF06C46, {&HF3, &HF, &H11, &H83, &H44, &H1, &H0, &H0})




        End If
    End Sub
    Public Sub setclearcount(ByVal clearCount As Integer)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H1378700)

        WInt32(tmpPtr + &H3C, clearCount)

    End Sub
    Public Sub setcaption(ByVal str As String)
        Dim tmpptr As Integer
        Dim alpha As Byte

        Dim state As Boolean
        state = (str.Length > 0)

        If state Then
            alpha = 254
        Else
            alpha = 0
        End If

        tmpptr = RInt32(&H13786D0)

        WInt32(tmpptr + &H40, state And 4)
        WInt32(tmpptr + &HB18, alpha)
        WInt32(tmpptr + &HB14, 100)

        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H10)

        WUniStr(tmpptr + &H12C, str & ChrW(0))

    End Sub
    Public Sub setsaveenable(ByVal state As Byte)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H13784A0)

        WBytes(tmpPtr + &HB40, {state})
    End Sub
    Public Sub setsaveslot(ByVal slot As Integer)
        WInt32(RInt32(&H13784A0) + &HA70, slot)
    End Sub
    Public Sub setunknownnpcname(ByVal name As String)

        If name.Length > 21 Then name = Strings.Left(name, 21) 'Prevent runover into code
        WUniStr(&H11A784C, name + ChrW(0))
    End Sub
    Public Sub showhud(ByVal state As Byte)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)
        tmpptr = RUInt32(tmpptr + &H2C)

        WBytes(tmpptr + &HD, {state})
    End Sub
    Public Sub playerhide(ByVal state As Byte)
        WBytes(&H13784E7, {state})
    End Sub
    Public Sub waitforload()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378700)

        Dim msPlayed As Integer
        Dim loading As Boolean = True

        msPlayed = RInt32(tmpptr + &H68)

        Do While loading
            loading = (msPlayed = RInt32(tmpptr + &H68))
            Thread.Sleep(33)
        Loop
    End Sub


    Public Sub setbriefingmsg(ByVal str As String)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H7C)

        WUniStr(tmpptr + &H3B7A, str + ChrW(0))
        funccall_old("RequestOpenBriefingMsg", {"10010721", "1", 0, 0, 0})

    End Sub
    Public Sub setgendialog(ByVal str As String, type As Integer, Optional btn0 As String = "", Optional btn1 As String = "")
        '50002 = Overridden Maintext
        '65000 = Overridden Button 0
        '70000 = Overridden Button 1

        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H174)

        str = str.Replace("\n", ChrW(&HA))

        'Weird issues if exactly 6 characters
        If str.Length = 6 Then str = str & "  "
        WUniStr(tmpptr + &H1A5C, str + ChrW(0))

        'Set Default Ok/Cancel if not overridden
        WInt32(&H12E33E4, 1)
        WInt32(&H12E33E8, 2)

        'Clear previous values
        WInt32(&H12E33F8, -1)
        WInt32(&H12E33FC, -1)

        WInt32(&H12E33E0, 50002)
        If btn0.Length > 0 Then
            WInt32(&H12E33E4, 65000)
            WUniStr(tmpptr + &H2226, btn0 + ChrW(0))
        End If
        If btn1.Length > 0 Then
            WInt32(&H12E33E8, 70000)
            WUniStr(tmpptr + &H350C, btn1 + ChrW(0))
        End If

        tmpptr = RInt32(&H13786D0)
        WInt32(tmpptr + &H60, type)


        'Wait for response
        GenDiagResponse = -1
        GenDiagVal = -1

        tmpptr = &H12E33F8

        While GenDiagResponse = -1
            GenDiagResponse = RInt32(tmpptr)
            GenDiagVal = RInt32(tmpptr + &H4)
            Thread.Sleep(33)
        End While
        Thread.Sleep(500)


    End Sub



    Public Sub bossasylum()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(16, False) 'Boss Death Flag
            SetEventFlag(11810000, False) 'Tutorial Complete Flag
            SetEventFlag(11815395, True) 'Boss at lower position

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1810998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)
            'facing 180 degrees
            warp_coords(3.15, 198.15, -6)
            SetEventFlag(11815390, True)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})

            If firstTry And rushName = "Normal" Then
                clearplaytime()
                firstTry = False
            End If

            If rushMode Then
                bossDead = WaitForBossDeath(0, &H8000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossbedofchaos()


        SetEventFlag(10, False) 'Boss 

        SetEventFlag(11410000, False)
        SetEventFlag(11410200, False) 'Center Platform flag
        SetEventFlag(11410291, False) 'Arm flag
        SetEventFlag(11410292, False) 'Arm flag

        'non-standard transition to allow quit-out
        'warp before fog gate to set last solid position

        playerhide(True)
        showhud(False)
        fadeout()
        funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

        WarpNextStage_Bonfire(1410980)

        Thread.Sleep(1000)

        waitforload()
        blackscreen()
        playerhide(True)
        funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        Warp(10000, 1412998)
        Thread.Sleep(250)
        Warp(10000, 1412997)

        Thread.Sleep(1250)
        fadein()
        showhud(True)
        playerhide(False)
        funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})

    End Sub
    Public Sub bossbellgargoyles()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})
            SetEventFlag(3, False) 'Boss Death Flag
            SetEventFlag(11010000, False) 'Boss Cinematic Viewed Flag



            'Non-standard due to co-ords warp

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1010998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)

            SetEventFlag(11015390, True) 'Boss Fog Used
            SetEventFlag(11015393, True) 'Boss Area Entered
            Thread.Sleep(250)

            'facing 0 degrees
            warp_coords(10.8, 48.92, 87.26)


            Thread.Sleep(1250)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})

            If rushMode Then
                bossDead = WaitForBossDeath(0, &H10000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If
        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossblackdragonkalameet()
        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})


            SetEventFlag(11210004, False)

            SetEventFlag(121, False)
            SetEventFlag(11210539, True)
            SetEventFlag(11210535, True)
            SetEventFlag(11210067, False)
            SetEventFlag(11210066, False)
            SetEventFlag(11210056, True)

            SetEventFlag(1821, True)
            SetEventFlag(11210592, True)


            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1210998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)
            'facing 107 degrees
            warp_coords(876.04, -344.73, 749.75)


            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H2300, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosscaprademon()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11010902, False)


            'Non-standard due to random deaths
            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1010998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})
            Thread.Sleep(500)
            'facing 238 degrees
            warp_coords(-73.17, -43.56, -15.17)
            'Warp(10000, 1012887)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})


            If rushMode Then
                bossDead = WaitForBossDeath(&HF70, &H2000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossceaselessdischarge()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11410900, False) 'Boss death flag
            SetEventFlag(51410180, True) 'Corpse Loot reset

            SetEventFlag(11415385, True)
            SetEventFlag(11415378, True)
            SetEventFlag(11415373, True)
            SetEventFlag(11415372, True)

            'Non-standard due to co-ords warp

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1410998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)

            warp_coords(250.53, -283.15, 72.1)
            Thread.Sleep(250)
            'facing 30 degrees
            warp_coords(402.45, -278.15, 15.5)




            Thread.Sleep(1250)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H3C70, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosscentipededemon()
        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})
            SetEventFlag(11410901, False)

            'StandardTransition(1410998, 1412896)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1410998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)

            Warp(10000, 1412896)
            SetEventFlag(11415380, True)
            SetEventFlag(11415383, True)
            SetEventFlag(11415382, True)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(&H3C70, &H4000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosschaoswitchquelaag()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(9, False)
            'StandardTransition(1400980, 1402997)

            playerhide(True)
            showhud(False)
            fadeout()

            funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

            WarpNextStage_Bonfire(1400980)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(17.2, -236.9, 113.6)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H400000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosscrossbreedpriscilla()


        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})
            SetEventFlag(4, False) 'Boss Death flag
            SetEventFlag(1691, True) 'Priscilla Hostile flag
            SetEventFlag(1692, True) 'Priscilla Dead flag

            SetEventFlag(11100531, False) 'Boss Disabled flag

            SetEventFlag(11100000, False) 'Previous victory flag




            'StandardTransition(1102961, 1102997)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1102961)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(-22.72, 60.55, 711.86)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossdarksungwyndolin()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11510900, False) 'Boss Death Flag
            SetEventFlag(11510523, False) 'Boss Disabled Flag

            'StandardTransition(1510982, 1512896)
            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1510982)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(435.1, 60.2, 255.0)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(&H4670, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossdemonfiresage()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11410410, False)
            'StandardTransition(1410998, 1412416)

            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1410998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(148.04, -341.04, 95.57)


            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(&H3C30, &H20)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossfourkings()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(13, False)
            SetEventFlag(1677, True) 'Kaathe Angry/gone
            'StandardTransition(1600999, 1602996)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1600999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(82.24, -163.2, 0.29)

            dropitem("Rings", "Covenant of Artorias", 1)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H40000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossgapingdragon()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(2, False) 'Boss Death Flag
            SetEventFlag(11000853, True) 'Channeler Death Flag
            'StandardTransition(1000999, 1002997)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1000999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            Thread.Sleep(500)
            SetEventFlag(11005390, True)
            SetEventFlag(11005392, True)
            SetEventFlag(11005393, True)
            SetEventFlag(11005394, True)
            SetEventFlag(11005397, True)
            SetEventFlag(11000000, False)


            Warp(10000, 1002997)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)

            If rushMode Then
                bossDead = WaitForBossDeath(0, &H20000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)

    End Sub
    Public Sub bossgravelordnito()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})


            SetEventFlag(7, False)
            'StandardTransition(1310998, 1312110)

            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1310998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})
            Thread.Sleep(500)

            'Warp(10000, 1312110)
            warp_coords(-126.84, -265.12, -30.78)
            SetEventFlag(11315390, True)
            SetEventFlag(11315393, True)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H1000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)

    End Sub
    Public Sub bossgwyn()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(15, False)
            'StandardTransition(1800999, 1802996)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1800999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(418.15, -115.92, 169.58)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)

            If firstTry And rushName = "Reverse" Then
                clearplaytime()
                firstTry = False
            End If

            If rushMode Then
                bossDead = WaitForBossDeath(0, &H10000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossirongolem()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11, False) 'Boss Death Flag
            SetEventFlag(11500865, True) 'Bomb-Tossing Giant Death Flag
            'StandardTransition(1500999, 1502997)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1500999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(85.5, 82, 255.1)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H100000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossknightartorias()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})


            SetEventFlag(11210001, False)
            SetEventFlag(11210513, False) 'Ciaran Present

            'Non-standard due to co-ords warp

            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1210998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)
            'facing 75.8 degrees
            warp_coords(1034.11, -330.0, 810.68)


            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H2300, &H40000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossmanus()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11210002, False)
            'StandardTransition(1210982, 1212997)

            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1210982)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(857.53, -576.69, 873.38)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(&H2300, &H20000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossmoonlightbutterfly()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11200900, False)
            SetEventFlag(11205383, False)
            'StandardTransition(1200999, 1202245)

            'Non-standard due to flags
            'timing of warp/flags matters

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1200999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})




            Thread.Sleep(500)
            warp_coords(181.39, 7.53, 29.01)
            Thread.Sleep(4000)
            SetEventFlag(11205383, True)

            warp_coords(178.82, 8.12, 30.77)



            Thread.Sleep(2000)
            fadein()
            showhud(True)

            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H1E70, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)

    End Sub
    Public Sub bossoands()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(12, False)



            'Non-standard due to co-ords warp

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1510998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)
            'facing 90 degrees
            warp_coords(539.9, 142.6, 254.79)


            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H80000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosspinwheel()
        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})


            SetEventFlag(6, False)
            'StandardTransition(1300999, 1302999)

            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1300999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(46, -165.8, 152.02)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H2000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosssanctuaryguardian()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11210000, False)
            SetEventFlag(11210001, False)


            'Non-standard due to co-ords warp

            playerhide(True)
            showhud(False)
            fadeout()
            funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

            WarpNextStage_Bonfire(1210998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})


            Thread.Sleep(500)
            'facing = 45 deg
            warp_coords(931.82, -318.63, 472.45)


            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H2300, &H80000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossseath()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(14, False)
            SetEventFlag(11700000, False)

            'StandardTransition(1700999, 1702997)
            playerhide(True)
            showhud(False)
            fadeout()


            WarpNextStage_Bonfire(1700999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(109, 134.05, 856.48)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H20000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bosssif()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(5, False)
            SetEventFlag(11200000, False)
            SetEventFlag(11200001, False)
            SetEventFlag(11200002, False)
            SetEventFlag(11205392, False)
            SetEventFlag(11205393, False)
            SetEventFlag(11205394, False)
            'StandardTransition(1200999, 1202999)

            playerhide(True)
            showhud(False)
            fadeout()

            funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

            WarpNextStage_Bonfire(1200999)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})
            Thread.Sleep(500)
            'Warp_Coords(274, -19.82, -266.43)
            Thread.Sleep(500)
            'Warp(10000, 1202999)
            warp_coords(254.31, -16.02, -320.32)

            Thread.Sleep(1000)
            fadein()
            showhud(True)
            playerhide(False)
            funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(0, &H4000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub
    Public Sub bossstraydemon()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})


            SetEventFlag(11810000, True)
            SetEventFlag(11810900, False)


            'StandardTransition(1810998, 1812996)

            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1810998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)
            funccall_old("DisableDamage", {10000, 1, 0, 0, 0})

            Thread.Sleep(500)

            Warp(10000, 1812996)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)
            Thread.Sleep(1000)
            funccall_old("DisableDamage", {10000, 0, 0, 0, 0})
            If rushMode Then
                bossDead = WaitForBossDeath(&H5A70, &H8000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)


    End Sub
    Public Sub bosstaurusdemon()

        Dim bossDead As Boolean = False

        Do
            funccall_old("RequestFullRecover", {0, 0, 0, 0, 0})

            SetEventFlag(11010901, False)
            'StandardTransition(1010998, 1012897)
            playerhide(True)
            showhud(False)
            fadeout()

            WarpNextStage_Bonfire(1010998)

            Thread.Sleep(1000)

            waitforload()
            blackscreen()
            playerhide(True)

            Thread.Sleep(500)


            warp_coords(49.81, 16.9, -118.87)

            Thread.Sleep(1500)
            fadein()
            showhud(True)
            playerhide(False)

            If rushMode Then
                bossDead = WaitForBossDeath(&HF70, &H4000000)
                If Not bossDead Then
                    funccall_old("AddTrueDeathCount", {0, 0, 0, 0, 0})
                    funccall_old("SetTextEffect", {16, 0, 0, 0, 0})
                    Thread.Sleep(5000)
                End If
            End If

        Loop While rushMode And Not bossDead
        Thread.Sleep(5000)
    End Sub

    Public Sub scenarioartoriasandciaran()


        SetEventFlag(11210001, False) 'Artorias Disabled
        SetEventFlag(11210513, True) 'Ciaran Present


        SetEventFlag(1863, False) 'Ciaran Hostile
        SetEventFlag(1864, False) 'Ciaran Dead

        'Non-standard due to co-ords warp

        playerhide(True)
        showhud(False)
        fadeout()

        funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

        WarpNextStage_Bonfire(1210998)

        Thread.Sleep(1000)

        waitforload()
        blackscreen()


        playerhide(True)
        funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        'facing 75.8 degrees
        warp_coords(1034.11, -330.0, 810.68)


        Thread.Sleep(1500)
        fadein()
        showhud(True)
        playerhide(False)
        funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})

        SetEventFlag(1863, True) 'Ciaran Hostile
        funccall_old("SetBossGauge", {6740, 1, 10001, 0, 0})
        setunknownnpcname("Lord's Blade Ciaran")
    End Sub
    Public Sub scenariotriplesanctuaryguardian()



        SetEventFlag(11210000, False)
        SetEventFlag(11210001, True)


        'Non-standard due to co-ords warp

        playerhide(True)
        showhud(False)
        fadeout()
        funccall_old("SetHp", {10000, "1.0", 0, 0, 0})

        WarpNextStage_Bonfire(1210998)

        Thread.Sleep(1000)

        waitforload()
        blackscreen()
        playerhide(True)
        funccall_old("SetDisableGravity", {10000, 1, 0, 0, 0})


        Thread.Sleep(500)
        'facing = 45 deg
        warp_coords(931.82, -318.63, 472.45)


        Thread.Sleep(1500)
        fadein()
        showhud(True)
        playerhide(False)
        funccall_old("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub

    Public Sub beginbossrush()


        Dim msg As String

        showhud(False)

        setgendialog("Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not GenDiagResponse = 1 Then
            setgendialog("So much shame...", 2, "I know", "I don't care")
            showhud(True)
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)
            Return
        End If
        If GenDiagVal > 6 Then GenDiagVal = 6
        setclearcount(GenDiagVal)

        msg = "Welcome to the Boss Rush." & Environment.NewLine
        msg = msg & "Saving has been disabled." & Environment.NewLine


        For i = 10 To 1 Step -1
            setbriefingmsg(msg & i)
            Thread.Sleep(1000)
        Next


        setbriefingmsg("Begin")

        funccall_old("CroseBriefingMsg", {0, 0, 0, 0, 0})
        Thread.Sleep(1000)


        rushTimer = New Thread(AddressOf BeginRushTimer)
        rushTimer.IsBackground = True




        rushTimer.Start()
        rushMode = True
        rushName = "Normal"

        bossasylum()
        bosstaurusdemon()
        bossbellgargoyles()
        bosscaprademon()
        bossgapingdragon()
        bossmoonlightbutterfly()
        bosssif()
        bosschaoswitchquelaag()
        bossstraydemon()
        bossirongolem()
        bossoands()
        bosspinwheel()
        bossgravelordnito()
        bosssanctuaryguardian()
        bossknightartorias()
        bossmanus()
        bossceaselessdischarge()
        bossdemonfiresage()
        bosscentipededemon()
        bossblackdragonkalameet()
        bossseath()
        bossfourkings()
        bosscrossbreedpriscilla()
        bossdarksungwyndolin()
        bossgwyn()


        gamestatsptr = RInt32(&H1378700)
        Dim rushTime As TimeSpan
        rushTime = TimeSpan.FromMilliseconds(RInt32(gamestatsptr + &H68))
        setbriefingmsg("Congratulations." & ChrW(&HA) & Strings.Left(rushTime.ToString, 12) & ChrW(&HA) &
                       "NG: " & RInt32(gamestatsptr + &H3C) & ChrW(&HA) &
                       "Deaths: " & RInt32(gamestatsptr + &H58))
        blackscreen()
        showhud(False)
        Thread.Sleep(10000)
        fadein()
        showhud(True)
        funccall_old("CroseBriefingMsg", {0, 0, 0, 0, 0})

        rushTimer.Abort()
    End Sub
    Public Sub beginreversebossrush()
        'Reverse Boss Order
        'Gwyn
        'Dark Sun Gwyndolin
        'Crossbreed Priscilla
        'Four Kings
        'Seath
        'Black Dragon Kalameet
        'Centipede Demon
        'Demon Firesage
        'Ceaseless Discharge
        'Manus
        'Knight Artorias
        'Sanctuary Guardian
        'Gravelord Nito
        'Pinwheel
        'Ornstein And Smough
        'Iron Golem
        'Stray Demon
        'Chaos Witch Quelaag
        'Sif
        'Moonlight Butterfly
        'Gaping Dragon
        'Capra Demon
        'Bell Gargoyles
        'Taurus Demon
        'Asylum Demon
        Dim msg As String

        showhud(False)

        setgendialog("Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not GenDiagResponse = 1 Then
            setgendialog("So much shame...", 2, "I know", "I don't care")
            showhud(True)
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)
            Return
        End If
        If GenDiagVal > 6 Then GenDiagVal = 6
        setclearcount(GenDiagVal)

        msg = "Welcome to the Reverse Boss Rush." & Environment.NewLine
        msg = msg & "Saving has been disabled." & Environment.NewLine


        For i = 10 To 1 Step -1
            setbriefingmsg(msg & i)
            Thread.Sleep(1000)
        Next


        setbriefingmsg("Begin")

        funccall_old("CroseBriefingMsg", {0, 0, 0, 0, 0})
        Thread.Sleep(1000)


        rushTimer = New Thread(AddressOf BeginRushTimer)
        rushTimer.IsBackground = True




        rushTimer.Start()
        rushMode = True
        rushName = "Reverse"


        bossgwyn()
        bossdarksungwyndolin()
        bosscrossbreedpriscilla()
        bossfourkings()
        bossseath()
        bossblackdragonkalameet()
        bosscentipededemon()
        bossdemonfiresage()
        bossceaselessdischarge()
        bossmanus()
        bossknightartorias()
        bosssanctuaryguardian()
        bossgravelordnito()
        bosspinwheel()
        bossoands()
        bossirongolem()
        bossstraydemon()
        bosschaoswitchquelaag()
        bosssif()
        bossmoonlightbutterfly()
        bossgapingdragon()
        bosscaprademon()
        bossbellgargoyles()
        bosstaurusdemon()
        bossasylum()

        gamestatsptr = RInt32(&H1378700)
        Dim rushTime As TimeSpan
        rushTime = TimeSpan.FromMilliseconds(RInt32(gamestatsptr + &H68))
        setbriefingmsg("Congratulations." & ChrW(&HA) & Strings.Left(rushTime.ToString, 12) & ChrW(&HA) &
                       "NG: " & RInt32(gamestatsptr + &H3C) & ChrW(&HA) &
                       "Deaths: " & RInt32(gamestatsptr + &H58))
        blackscreen()
        showhud(False)
        Thread.Sleep(10000)
        fadein()
        showhud(True)
        funccall_old("CroseBriefingMsg", {0, 0, 0, 0, 0})

        rushTimer.Abort()

    End Sub

    Private Function WaitForBossDeath(ByVal boost As Integer, match As Integer) As Boolean
        Dim eventPtr As Integer
        eventPtr = RInt32(&H137D7D4)
        eventPtr = RInt32(eventPtr)

        Dim hpPtr As Integer
        hpPtr = RInt32(&H137DC70)
        hpPtr = RInt32(hpPtr + 4)
        hpPtr = RInt32(hpPtr)
        hpPtr = hpPtr + &H2D4

        Dim bossdead As Boolean = False
        Dim selfdead As Boolean = False

        While Not (bossdead Or selfdead)
            bossdead = (RInt32(eventPtr + boost) And match)
            selfdead = (RInt32(hpPtr) = 0)
            Console.WriteLine(Hex(eventPtr) & " - " & Hex(RInt32(eventPtr)))
            Thread.Sleep(33)
        End While

        If bossdead Then
            Return True
        Else
            Return False
        End If


    End Function




    Private Sub btnBossAsylumDemon_Click(sender As Object, e As EventArgs) Handles btnBossAsylumDemon.Click, Button1.Click
        trd = New Thread(AddressOf bossasylum)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBedOfChaos_Click(sender As Object, e As EventArgs) Handles btnBossBedOfChaos.Click, Button6.Click
        trd = New Thread(AddressOf bossbedofchaos)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBellGargoyles_Click(sender As Object, e As EventArgs) Handles btnBossBellGargoyles.Click, Button4.Click
        trd = New Thread(AddressOf bossbellgargoyles)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBlackDragonKalameet_Click(sender As Object, e As EventArgs) Handles btnBossBlackDragonKalameet.Click, Button26.Click
        trd = New Thread(AddressOf bossblackdragonkalameet)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCapraDemon_Click(sender As Object, e As EventArgs) Handles btnBossCapraDemon.Click, Button3.Click
        trd = New Thread(AddressOf bosscaprademon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCeaselessDischarge_Click(sender As Object, e As EventArgs) Handles btnBossCeaselessDischarge.Click, Button5.Click
        trd = New Thread(AddressOf bossceaselessdischarge)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCentipedeDemon_Click(sender As Object, e As EventArgs) Handles btnBossCentipedeDemon.Click, Button7.Click
        trd = New Thread(AddressOf bosscentipededemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossChaosWitchQuelaag_Click(sender As Object, e As EventArgs) Handles btnBossChaosWitchQuelaag.Click, Button8.Click
        trd = New Thread(AddressOf bosschaoswitchquelaag)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCrossbreedPriscilla_Click(sender As Object, e As EventArgs) Handles btnBossCrossbreedPriscilla.Click, Button9.Click
        trd = New Thread(AddressOf bosscrossbreedpriscilla)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDarkSunGwyndolin_Click(sender As Object, e As EventArgs) Handles btnBossDarkSunGwyndolin.Click, Button10.Click
        trd = New Thread(AddressOf bossdarksungwyndolin)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDemonFiresage_Click(sender As Object, e As EventArgs) Handles btnBossDemonFiresage.Click, Button11.Click
        trd = New Thread(AddressOf bossdemonfiresage)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossFourKings_Click(sender As Object, e As EventArgs) Handles btnBossFourKings.Click, Button13.Click
        trd = New Thread(AddressOf bossfourkings)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGapingDragon_Click(sender As Object, e As EventArgs) Handles btnBossGapingDragon.Click, Button14.Click
        trd = New Thread(AddressOf bossgapingdragon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGravelordNito_Click(sender As Object, e As EventArgs) Handles btnBossGravelordNito.Click, Button15.Click
        trd = New Thread(AddressOf bossgravelordnito)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGwyn_Click(sender As Object, e As EventArgs) Handles btnBossGwyn.Click, Button16.Click
        trd = New Thread(AddressOf bossgwyn)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossIronGolem_Click(sender As Object, e As EventArgs) Handles btnBossIronGolem.Click, Button17.Click
        trd = New Thread(AddressOf bossirongolem)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossKnightArtorias_Click(sender As Object, e As EventArgs) Handles btnBossKnightArtorias.Click, Button18.Click
        trd = New Thread(AddressOf bossknightartorias)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossManus_Click(sender As Object, e As EventArgs) Handles btnBossManus.Click, Button19.Click
        trd = New Thread(AddressOf bossmanus)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossMoonlightButterfly_Click(sender As Object, e As EventArgs) Handles btnBossMoonlightButterfly.Click, Button20.Click
        trd = New Thread(AddressOf bossmoonlightbutterfly)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossOandS_Click(sender As Object, e As EventArgs) Handles btnBossOandS.Click, Button12.Click
        trd = New Thread(AddressOf bossoands)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossPinwheel_Click(sender As Object, e As EventArgs) Handles btnBossPinwheel.Click, Button2.Click
        trd = New Thread(AddressOf bosspinwheel)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSanctuaryGuardian_Click(sender As Object, e As EventArgs) Handles btnBossSanctuaryGuardian.Click, Button21.Click
        trd = New Thread(AddressOf bosssanctuaryguardian)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSeath_Click(sender As Object, e As EventArgs) Handles btnBossSeath.Click, Button22.Click
        trd = New Thread(AddressOf bossseath)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSif_Click(sender As Object, e As EventArgs) Handles btnBossSif.Click, Button24.Click
        trd = New Thread(AddressOf bosssif)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossStrayDemon_Click(sender As Object, e As EventArgs) Handles btnBossStrayDemon.Click, Button23.Click
        trd = New Thread(AddressOf bossstraydemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossTaurusDemon_Click(sender As Object, e As EventArgs) Handles btnBossTaurusDemon.Click, Button25.Click
        trd = New Thread(AddressOf bosstaurusdemon)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnScenarioArtoriasCiaran_Click(sender As Object, e As EventArgs) Handles btnScenarioArtoriasCiaran.Click
        trd = New Thread(AddressOf scenarioartoriasandciaran)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnScenarioTripleSanctuary_Click(sender As Object, e As EventArgs) Handles btnScenarioTripleSanctuary.Click
        trd = New Thread(AddressOf scenariotriplesanctuaryguardian)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnBeginBossRush_Click(sender As Object, e As EventArgs) Handles btnBeginBossRush.Click
        trd = New Thread(AddressOf beginbossrush)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBeginReverseRush_Click(sender As Object, e As EventArgs) Handles btnBeginReverseRush.Click
        trd = New Thread(AddressOf beginreversebossrush)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub BeginRushTimer()

        gamestatsptr = RInt32(&H1378700)
        Dim menuptr = RInt32(&H13786D0)
        Dim lineptr = RInt32(&H1378388)
        Dim keyptr = RInt32(&H1378640)

        Dim rushTime As TimeSpan
        Dim msPlayed As Integer
        Dim clearCount As Integer
        Dim trueDeathCount As Integer
        Dim msg As String

        WFloat(lineptr + &H78, 1100)
        WFloat(lineptr + &H7C, 675)

        WFloat(keyptr + &H78, 600)
        WFloat(keyptr + &H7C, 605)

        'Clear TrueDeaths
        WInt32(gamestatsptr + &H58, 0)

        Do
            WInt32(menuptr + &H154, RInt32(menuptr + &H1C)) 'LineHelp
            WInt32(menuptr + &H158, RInt32(menuptr + &H1C)) 'KeyGuide

            msPlayed = RInt32(gamestatsptr + &H68)
            rushTime = TimeSpan.FromMilliseconds(msPlayed)
            clearCount = RInt32(gamestatsptr + &H3C)
            trueDeathCount = RInt32(gamestatsptr + &H58)


            msg = "NG" & clearCount & " - "
            msg = msg & Strings.Left(rushTime.ToString, 12) & ChrW(0)
            WUniStr(&H11A7770, msg)

            msg = "Deaths: " & trueDeathCount & ChrW(0)
            WUniStr(&H11A7758, msg) 'LineHelp



            Thread.Sleep(33)
        Loop
    End Sub

    Private Sub btnDonate_Click(sender As Object, e As EventArgs) Handles btnDonate.Click
        Dim webAddress As String = "http://paypal.me/wulf2k/"
        Process.Start(webAddress)
    End Sub

    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        If ScanForProcess("DARK SOULS", True) Then
            'Check if this process is even Dark Souls
            checkDarkSoulsVersion()
            If isHooked Then
                dropAlloc()
                funcAlloc()

                setsaveenable(False)
            End If
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click


        'SetGenDialog("These utensils were called\n'The Best Ever'\nBy Pro Utensils Magazine", 2, "Spoons", "Forks")

        'Select Case GenDiagResponse
        'Case 0
        'SetGenDialog("You have failed to answer.", 1)
        'Case 1
        'SetGenDialog("You have selected Spoons.", 1)
        'Case 2
        'SetGenDialog("You have selected Forks.", 1)
        'End Select

        'SetBriefingMsg("Test")
        'funcCall("SetTextEffect", {16, 0, 0, 0, 0})


        Dim str As String
        Dim action As String
        Dim params() As String = {}



        'str = "funcCall SetHp, 10000, 1.0"
        'str = "DisableAI true"
        str = "SetHp 10000, 1.0"


        action = str.Split(" ")(0).ToLower
        If clsFuncLocs.Contains(action) Then
            str = "funccall " & action & ", " & str.ToLower.Replace(action & " ", "")
            action = "funccall"
        End If

        If str.Contains(" ") Then
            str = str.Replace(action & " ", "")
            params = str.Replace(" ", "").Split(",")
        End If

        For i = 0 To params.Count - 1
            If params(i).ToLower = "true" Then params(i) = "1"
            If params(i).ToLower = "false" Then params(i) = "0"
        Next

        'Ugly hack to work around parameter count issue with funcCall
        If action = "funccall" Then
            For i = 0 To (6 - params.Count) - 1
                Array.Resize(params, params.Length + 1)
                params(params.Length - 1) = "0"
            Next
        End If



        Dim t As Type = Me.GetType
        Dim method As MethodInfo

        method = t.GetMethod(action)

        Dim typedParams() As Object = {}


        For i = 0 To method.GetParameters.Count - 1
            Array.Resize(typedParams, typedParams.Length + 1)

            If method.GetParameters(i).ParameterType.IsByRef Then
                typedParams(typedParams.Length - 1) = CTypeDynamic(params(i), method.GetParameters(i).ParameterType.GetElementType())
            Else
                typedParams(typedParams.Length - 1) = CTypeDynamic(params(i), method.GetParameters(i).ParameterType())
            End If

        Next


        method.Invoke(Me, params)

        Dim result As Integer
        result = RInt32(funcPtr + &H200)
        WInt32(funcPtr + &H200, 1337)

        MsgBox(result)




    End Sub



    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim updateWindow As New UpdateWindow(sender.Tag)
        updateWindow.ShowDialog()
        If updateWindow.WasSuccessful Then
            Process.Start(updateWindow.NewAssembly, """--old-file=" & updateWindow.OldAssembly & """")
            Me.Close()
        End If
    End Sub

    Private Sub btnDisableAI_Click(sender As Object, e As EventArgs) Handles btnDisableAI.Click
        disableai(True)
    End Sub

    Private Sub btnEnableAI_Click(sender As Object, e As EventArgs) Handles btnEnableAI.Click
        disableai(False)
    End Sub

    Private Sub nmbVitality_ValueChanged(sender As Object, e As EventArgs) Handles nmbVitality.ValueChanged
        WInt32(charptr2 + &H38, sender.Value)
    End Sub

    Private Sub nmbAttunement_ValueChanged(sender As Object, e As EventArgs) Handles nmbAttunement.ValueChanged
        WInt32(charptr2 + &H40, sender.Value)
    End Sub

    Private Sub nmbEnd_ValueChanged(sender As Object, e As EventArgs) Handles nmbEnd.ValueChanged
        WInt32(charptr2 + &H48, sender.Value)
    End Sub

    Private Sub nmbStr_ValueChanged(sender As Object, e As EventArgs) Handles nmbStr.ValueChanged
        WInt32(charptr2 + &H50, sender.Value)
    End Sub

    Private Sub nmbDex_ValueChanged(sender As Object, e As EventArgs) Handles nmbDex.ValueChanged
        WInt32(charptr2 + &H58, sender.Value)
    End Sub

    Private Sub nmbResistance_ValueChanged(sender As Object, e As EventArgs) Handles nmbResistance.ValueChanged
        WInt32(charptr2 + &H80, sender.Value)
    End Sub

    Private Sub nmbIntelligence_ValueChanged(sender As Object, e As EventArgs) Handles nmbIntelligence.ValueChanged
        WInt32(charptr2 + &H60, sender.Value)
    End Sub

    Private Sub nmbFaith_ValueChanged(sender As Object, e As EventArgs) Handles nmbFaith.ValueChanged
        WInt32(charptr2 + &H68, sender.Value)
    End Sub

    Private Sub nmbHumanity_ValueChanged(sender As Object, e As EventArgs) Handles nmbHumanity.ValueChanged
        WInt32(charptr2 + &H7C, sender.Value)
    End Sub

    Private Sub nmbGender_ValueChanged(sender As Object, e As EventArgs) Handles nmbGender.ValueChanged
        Wint16(charptr2 + &HC2, sender.value)
    End Sub

    Private Sub nmbMaxHP_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxHP.ValueChanged
        WInt32(charptr2 + &H14, sender.Value)
    End Sub

    Private Sub nmbMaxStam_ValueChanged(sender As Object, e As EventArgs) Handles nmbMaxStam.ValueChanged
        WInt32(charptr2 + &H30, sender.Value)
    End Sub

    Private Sub btnX_Click(sender As Object, e As EventArgs) Handles btnX.Click

        showhud(True)
        WInt32(RInt32(&H13786D0) + &H154, -1)
        WInt32(RInt32(&H13786D0) + &H158, -1)

        If rushMode Then rushTimer.Abort()
        rushMode = False
        trd.Abort()
        Console.WriteLine(trd.ThreadState)
    End Sub
End Class
