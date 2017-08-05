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
    Private scriptTrd As Thread
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

    Public intvar1 As Integer
    Public intvar2 As Integer


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
                        Script("Wait 1000")
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

            WBytes(&HBE73FE, {&H20})
            WBytes(&HBE719F, {&H20})
            WBytes(&HBE722B, {&H20})

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

    Private Function getIsBossRushThreadActive() As Boolean

        If Not trd Is Nothing Then

            'TODO: Check this shit
            'If no worky, replace with
            'If trd.ThreadState = ThreadState.Stopped Or trd.ThreadState = ThreadState.Aborted Then
            'etc...
            Return trd.IsAlive
        Else
            Return False
        End If

    End Function

    Private Sub RefreshGUI()

        Dim isBossRushing = getIsBossRushThreadActive()

        gbBosses.Enabled = Not isBossRushing

        For Each bossBtn As Button In gbBosses.Controls
            If bossBtn Is Nothing Then
                Continue For
            Else
                bossBtn.Enabled = Not isBossRushing
            End If
        Next

        'btnBeginBossRush.Enabled = Not isBossRushing
        'btnBeginBossRush.Visible = Not isBossRushing
        cboxReverseOrder.Enabled = Not isBossRushing
        'btnX.Visible = isBossRushing
        'btnX.Enabled = isBossRushing

        If isBossRushing Then
            btnBeginBossRush.Text = "End Boss Rush"
        Else
            btnBeginBossRush.Text = "Begin Boss Rush"
        End If

        'If Not trd Is Nothing Then

        '    'TODO: Check this shit
        '    'If no worky, replace with
        '    'If trd.ThreadState = ThreadState.Stopped Or trd.ThreadState = ThreadState.Aborted Then
        '    If Not trd.IsAlive Then
        '        gbBosses.Visible = True
        '        btnBeginBossRush.Enabled = True
        '        btnBeginBossRush.Visible = True
        '        cboxReverseOrder.Enabled = True
        '        btnX.Visible = False
        '        btnX.Enabled = False
        '    Else
        '        gbBosses.Visible = False
        '        btnBeginBossRush.Enabled = False
        '        btnBeginBossRush.Visible = False
        '        cboxReverseOrder.Enabled = False
        '        btnX.Visible = True
        '        btnX.Enabled = True
        '    End If
        'Else
        '    gbBosses.Visible = True
        '    btnBeginBossRush.Enabled = True
        '    btnBeginBossRush.Visible = True
        '    cboxReverseOrder.Enabled = True
        '    btnX.Visible = False
        '    btnX.Enabled = False
        'End If

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

    Private Sub refTimer_Tick() Handles refTimer.Tick

        RefreshGUI()

    End Sub

    Private Sub dropitem(ByVal cat As String, item As String, num As Integer)
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

    Public Function funccall(func As String, Optional param1 As String = "", Optional param2 As String = "", Optional param3 As String = "", Optional param4 As String = "", Optional param5 As String = "") As Integer

        Dim Params() As String = {param1, param2, param3, param4, param5}
        Dim param As IntPtr = Marshal.AllocHGlobal(4)
        Dim intParam As Integer
        Dim floatParam As Single
        Dim a As New asm

        func = func.ToLower

        a.pos = funcPtr
        a.AddVar("funcloc", clsFuncLocs(func.ToLower))
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
        CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr, 0, 0, 0)
        Thread.Sleep(5)



        Return RInt32(funcPtr + &H200)


    End Function


    Public Sub warp_coords(ByVal x As Single, y As Single, z As Single, rotx As Integer)
        charptr1 = RInt32(&H137DC70)
        charptr1 = RInt32(charptr1 + &H4)
        charptr1 = RInt32(charptr1)
        charmapdataptr = RInt32(charptr1 + &H28)

        WFloat(charmapdataptr + &HD0, x)
        WFloat(charmapdataptr + &HD4, y)
        WFloat(charmapdataptr + &HD8, z)

        Dim facing As Single
        facing = ((rotx / 360) * 2 * Math.PI) - Math.PI


        WFloat(charmapdataptr + &HE4, facing)
        WBytes(charmapdataptr + &HC8, {1})
    End Sub
    Public Sub warpentity_coords(entityPtr As Integer, x As Single, y As Single, z As Single, rotx As Integer)
        entityPtr = RInt32(entityPtr + &H28)
        WFloat(entityPtr + &HD0, x)
        WFloat(entityPtr + &HD4, y)
        WFloat(entityPtr + &HD8, z)

        Dim facing As Single
        facing = ((rotx / 360) * 2 * Math.PI) - Math.PI


        WFloat(entityPtr + &HE4, facing)
        WBytes(entityPtr + &HC8, {1})
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
    Public Sub camfocusentity(entityptr As Integer)
        Dim camPtr As Integer = RInt32(&H137D648) + &HEC

        WInt32(camPtr, entityptr)
    End Sub
    Public Sub clearplaytime()
        Dim tmpPtr As Integer = RIntPtr(&H1378700)
        WInt32(tmpPtr + &H68, 0)
    End Sub
    Public Sub controlentity(entityPtr As Integer, state As Byte)
        entityPtr = RInt32(entityPtr + &H28)

        Dim ctrlptr As Integer = RInt32(&H137DC70)
        ctrlptr = RInt32(ctrlptr + 4)
        ctrlptr = RInt32(ctrlptr)
        ctrlptr = RInt32(ctrlptr + &H28)
        ctrlptr = RInt32(ctrlptr + &H54)

        WInt32(entityPtr + &H244, ctrlptr * (state And 1))

    End Sub
    Public Sub test_disableai(ByVal state As Byte)
        WBytes(&H13784EE, {state})
    End Sub
    Public Sub test_playerexterminate(ByVal state As Byte)
        WBytes(&H13784D3, {state})
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
    Public Sub forceentitydrawgroup(entityptr As Integer)
        WInt32(entityptr + &H264, -1)
        WInt32(entityptr + &H268, -1)
        WInt32(entityptr + &H26C, -1)
        WInt32(entityptr + &H270, -1)
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
    Public Sub setfreecam(ByVal state As Byte)
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
    Private Sub setcaption(ByVal str As String)
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
    Private Sub setunknownnpcname(ByVal name As String)
        If name.Length > 21 Then name = Strings.Left(name, 21) 'Prevent runover into code
        WUniStr(&H11A784C, name + ChrW(0))
    End Sub

    Public Sub playerhide(ByVal state As Byte)
        WBytes(&H13784E7, {state})
    End Sub
    Public Sub showhud(ByVal state As Byte)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)
        tmpptr = RUInt32(tmpptr + &H2C)

        WBytes(tmpptr + &HD, {state})
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
    Public Sub waittillload()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378700)

        Dim msPlayed As Integer
        Dim loading As Boolean = False

        msPlayed = RInt32(tmpptr + &H68)

        Do While Not loading
            loading = (msPlayed = RInt32(tmpptr + &H68))
            Thread.Sleep(33)
        Loop
    End Sub


    Private Sub setbriefingmsg(ByVal str As String)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H7C)

        WUniStr(tmpptr + &H3B7A, str + ChrW(0))
        Script("RequestOpenBriefingMsg 10010721, 1")

    End Sub
    Private Sub setgendialog(ByVal str As String, type As Integer, Optional btn0 As String = "", Optional btn1 As String = "")
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
        Script("Wait 500")
    End Sub
    Public Sub wait(val As Integer)
        Thread.Sleep(val)
    End Sub

    Public Function waitforbossdeath(ByVal boost As Integer, match As Integer) As Integer
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
            Return 1
        Else
            Return 0
        End If
    End Function


    Public Sub bossasylum()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 16, False") 'Boss Death Flag
            Script("SetEventFlag 11810000, False") 'Tutorial Complete Flag
            Script("SetEventFlag 11815395, True") 'Boss at lower position

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1810998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")
            Script("Warp_Coords 3.15, 198.15, -6.0, 180")
            Script("CamReset 10000, 1")
            Script("SetEventFlag 11815390, True")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")

            If firstTry And rushName = "Normal" Then
                Script("ClearPlayTime")
                firstTry = False
            End If

            If rushMode Then
                bossDead = waitforbossdeath(0, &H8000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossbedofchaos()


        Script("SetEventFlag 10, False") 'Boss 

        Script("SetEventFlag 11410000, False")
        Script("SetEventFlag 11410200, False") 'Center Platform flag
        Script("SetEventFlag 11410291, False") 'Arm flag
        Script("SetEventFlag 11410292, False") 'Arm flag

        'warp before fog gate to set last solid position

        Script("PlayerHide 1")
        Script("ShowHUD False")
        Script("FadeOut")
        Script("SetHp 10000, 1.0")

        Script("WarpNextStage_Bonfire 1410980")

        Script("Wait 1000")

        Script("WaitForLoad")
        Script("BlackScreen")
        Script("PlayerHide 1")
        Script("SetDisableGravity 10000, 1")

        Script("Wait 500")
        Script("Warp 10000, 1412998")
        Script("Wait 250")
        Script("Warp 10000, 1412997")

        Script("Wait 1250")
        Script("FadeIn")
        Script("ShowHUD 1")
        Script("PlayerHide 0")
        Script("SetDisableGravity 10000, 0")

    End Sub
    Public Sub bossbellgargoyles()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")
            Script("SetEventFlag 3, False") 'Boss Death Flag
            Script("SetEventFlag 11010000, False") 'Boss Cinematic Viewed Flag

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")
            Script("WarpNextStage_Bonfire 1010998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")

            Script("SetEventFlag 11015390, True") 'Boss Fog Used
            Script("SetEventFlag 11015393, True") 'Boss Area Entered
            Script("Wait 250")

            'facing 0 degrees
            Script("Warp_Coords 10.8, 48.92, 87.26")
            Script("CamReset 10000, 1")


            Script("Wait 1250")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")

            If rushMode Then
                bossDead = waitforbossdeath(0, &H10000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If
        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossblackdragonkalameet()
        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")


            Script("SetEventFlag 11210004, False")

            Script("SetEventFlag 121, False")
            Script("SetEventFlag 11210539, True")
            Script("SetEventFlag 11210535, True")
            Script("SetEventFlag 11210067, False")
            Script("SetEventFlag 11210066, False")
            Script("SetEventFlag 11210056, True")

            Script("SetEventFlag 1821, True")
            Script("SetEventFlag 11210592, True")


            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1210998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")
            Script("Warp_Coords 876.04, -344.73, 749.75, 240")
            Script("CamReset 10000, 1")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H2300, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosscaprademon()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11010902, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1010998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")
            Script("Wait 500")

            Script("Warp_Coords -73.17, -43.56, -15.17, 321")
            Script("CamReset 10000, 1")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")


            If rushMode Then
                bossDead = waitforbossdeath(&HF70, &H2000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossceaselessdischarge()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11410800, False")
            Script("SetEventFlag 11410801, False")
            Script("SetEventFlag 11410900, False") 'Boss death flag
            Script("SetEventFlag 51410180, True") 'Corpse Loot reset


            Script("SetEventFlag 11415379, False")
            Script("SetEventFlag 11415385, True")
            Script("SetEventFlag 11415378, True")
            Script("SetEventFlag 11415373, True")
            Script("SetEventFlag 11415372, True")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1410998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")

            Script("Warp_Coords 250.53, -283.15, 72.1")
            Script("Wait 250")

            Script("Warp_Coords 402.45, -278.15, 15.5, 30")
            Script("CamReset 10000, 1")




            Script("Wait 1250")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H3C70, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosscentipededemon()
        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")
            Script("SetEventFlag 11410002, False") 'Cinematic flag
            Script("SetEventFlag 11410901, False")


            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1410998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")

            Script("Warp 10000, 1412896")
            Script("SetEventFlag 11415380, True")
            Script("SetEventFlag 11415383, True")
            Script("SetEventFlag 11415382, True")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H3C70, &H4000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosschaoswitchquelaag()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 9, False")
            Script("SetEventFlag 11400000, False") 'Cinematic flag

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1400980")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 17.2, -236.9, 113.6, 75")
            Script("CamReset 10000, 1")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H400000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosscrossbreedpriscilla()


        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")
            Script("SetEventFlag 4, False") 'Boss Death flag
            Script("SetEventFlag 1691, True") 'Priscilla Hostile flag
            Script("SetEventFlag 1692, True") 'Priscilla Dead flag

            Script("SetEventFlag 11100531, False") 'Boss Disabled flag

            Script("SetEventFlag 11100000, False") 'Previous victory flag




            'StandardTransition(1102961, 1102997)

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1102961")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords -22.72, 60.55, 711.86")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossdarksungwyndolin()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11510900, False") 'Boss Death Flag
            Script("SetEventFlag 11510523, False") 'Boss Disabled Flag

            'StandardTransition(1510982, 1512896)
            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1510982")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 435.1, 60.2, 255.0")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H4670, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossdemonfiresage()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11410410, False")
            'StandardTransition(1410998, 1412416)

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1410998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 148.04, -341.04, 95.57")


            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H3C30, &H20)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossfourkings()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 13, False")
            Script("SetEventFlag 1677, True") 'Kaathe Angry/gone

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1600999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            'Script("Warp_Coords 82.24, -163.2, 0.29")
            'Facing 185.98
            Script("Warp_Coords 85.18, -191.99, 4.95, 185")
            Script("CamReset 10000, 1")

            Script("Wait 1500")
            dropitem("Rings", "Covenant of Artorias", 1)
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")

            If rushMode Then
                bossDead = waitforbossdeath(0, &H40000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    'Script("SetTextEffect 16")
                    Script("WaitTillLoad")
                    Script("WaitForLoad")
                End If
            End If
        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossgapingdragon()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 2, False") 'Boss Death Flag
            Script("SetEventFlag 11000853, True") 'Channeler Death Flag
            'StandardTransition(1000999, 1002997)

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1000999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("Wait 500")
            Script("SetEventFlag 11005390, True")
            Script("SetEventFlag 11005392, True")
            Script("SetEventFlag 11005393, True")
            Script("SetEventFlag 11005394, True")
            Script("SetEventFlag 11005397, True")
            Script("SetEventFlag 11000000, False")


            Script("Warp 10000, 1002997")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")

            If rushMode Then
                bossDead = waitforbossdeath(0, &H20000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")

    End Sub
    Public Sub bossgravelordnito()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")


            Script("SetEventFlag 7, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1310998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")
            Script("Wait 500")

            'Script("Warp 10000, 1312110)
            Script("Warp_Coords -126.84, -265.12, -30.78")
            Script("SetEventFlag 11315390, True")
            Script("SetEventFlag 11315393, True")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H1000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")

    End Sub
    Public Sub bossgwyn()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 15, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1800999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 418.15, -115.92, 169.58")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")

            If firstTry And rushName = "Reverse" Then
                Script("ClearPlayTime")
                firstTry = False
            End If

            If rushMode Then
                bossDead = waitforbossdeath(0, &H10000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossirongolem()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11, False") 'Boss Death Flag
            Script("SetEventFlag 11500865, True") 'Bomb-Tossing Giant Death Flag

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1500999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 85.5, 82, 255.1")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H100000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossknightartorias()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")


            Script("SetEventFlag 11210001, False")
            Script("SetEventFlag 11210513, False") 'Ciaran Present

            'Non-standard due to co-ords warp

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1210998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")
            'facing 75.8 degrees
            Script("Warp_Coords 1034.11, -330.0, 810.68")


            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H2300, &H40000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossmanus()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11210002, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1210982")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 857.53, -576.69, 873.38")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H2300, &H20000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossmoonlightbutterfly()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11200900, False")
            Script("SetEventFlag 11205383, False")

            'timing of warp/flags matters

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1200999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")




            Script("Wait 500")
            Script("Warp_Coords 181.39, 7.53, 29.01")
            Thread.Sleep(4000)
            Script("SetEventFlag 11205383, True")

            Script("Warp_Coords 178.82, 8.12, 30.77")



            Thread.Sleep(2000)
            Script("FadeIn")
            Script("ShowHUD 1")

            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H1E70, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")

    End Sub
    Public Sub bossoands()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 12, False")



            'Non-standard due to co-ords warp

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1510998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")

            Script("Wait 500")
            'facing 90 degrees
            Script("Warp_Coords 539.9, 142.6, 254.79")


            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H80000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosspinwheel()
        Dim bossDead As Boolean = False

        Do
            'Pinwheel Entity ID = 1300800
            Script("RequestFullRecover")

            Script("SetEventFlag 6, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1300999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 46.0, -165.8, 152.02, 180")
            Script("CamReset 10000, 1")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H2000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosssanctuaryguardian()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11210000, False")
            Script("SetEventFlag 11210001, False")


            'Non-standard due to co-ords warp

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")
            Script("SetHp 10000, 1.0")

            Script("WarpNextStage_Bonfire 1210998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")


            Script("Wait 500")
            'facing = 45 deg
            Script("Warp_Coords 931.82, -318.63, 472.45")


            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H2300, &H80000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossseath()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 14, False")
            Script("SetEventFlag 11700000, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")


            Script("WarpNextStage_Bonfire 1700999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 109, 134.05, 856.48")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H20000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bosssif()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 5, False")
            Script("SetEventFlag 11200000, False")
            Script("SetEventFlag 11200001, False")
            Script("SetEventFlag 11200002, False")
            Script("SetEventFlag 11205392, False")
            Script("SetEventFlag 11205393, False")
            Script("SetEventFlag 11205394, False")


            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("SetHp 10000, 1.0")

            Script("WarpNextStage_Bonfire 1200999")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("SetDisableGravity 10000, 1")
            Script("Wait 500")
            'Script("Warp_Coords 274, -19.82, -266.43)
            Script("Wait 500")
            'Script("Warp 10000, 1202999)
            Script("Warp_Coords 254.31, -16.02, -320.32")

            Script("Wait 1000")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(0, &H4000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub
    Public Sub bossstraydemon()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")


            Script("SetEventFlag 11810000, True")
            Script("SetEventFlag 11810900, False")


            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1810998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")
            Script("DisableDamage 10000, 1")

            Script("Wait 500")

            Script("Warp 10000, 1812996")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")
            Script("Wait 1000")
            Script("DisableDamage 10000, 0")
            If rushMode Then
                bossDead = waitforbossdeath(&H5A70, &H8000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")


    End Sub
    Public Sub bosstaurusdemon()

        Dim bossDead As Boolean = False

        Do
            Script("RequestFullRecover")

            Script("SetEventFlag 11010901, False")

            Script("PlayerHide 1")
            Script("ShowHUD False")
            Script("FadeOut")

            Script("WarpNextStage_Bonfire 1010998")

            Script("Wait 1000")

            Script("WaitForLoad")
            Script("BlackScreen")
            Script("PlayerHide 1")

            Script("Wait 500")


            Script("Warp_Coords 49.81, 16.9, -118.87")

            Script("Wait 1500")
            Script("FadeIn")
            Script("ShowHUD 1")
            Script("PlayerHide 0")

            If rushMode Then
                bossDead = waitforbossdeath(&HF70, &H4000000)
                If Not bossDead Then
                    Script("AddTrueDeathCount")
                    Script("SetTextEffect 16")
                    Script("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script("Wait 5000")
    End Sub

    Public Sub scenarioartoriasandciaran()


        Script("SetEventFlag 11210001, False") 'Artorias Disabled
        Script("SetEventFlag 11210513, True") 'Ciaran Present


        Script("SetEventFlag 1863, False") 'Ciaran Hostile
        Script("SetEventFlag 1864, False") 'Ciaran Dead

        Script("PlayerHide 1")
        Script("ShowHUD False")
        Script("FadeOut")

        Script("SetHp 10000, 1.0")

        Script("WarpNextStage_Bonfire 1210998")

        Script("Wait 1000")

        Script("WaitForLoad")
        Script("BlackScreen")


        Script("PlayerHide 1")
        Script("SetDisableGravity 10000, 1")

        Script("Wait 500")
        'facing 75.8 degrees
        Script("Warp_Coords 1034.11, -330.0, 810.68")


        Script("Wait 1500")
        Script("FadeIn")
        Script("ShowHUD 1")
        Script("PlayerHide 0")
        Script("SetDisableGravity 10000, 0")

        Script("SetEventFlag 1863, True") 'Ciaran Hostile
        'funccall_old("SetBossGauge", {6740, 1, 10001, 0, 0})
        Script("SetBossGauge 6740, 1, 10001")
        setunknownnpcname("Lord's Blade Ciaran")
    End Sub
    Public Sub scenariotriplesanctuaryguardian()



        Script("SetEventFlag 11210000, False")
        Script("SetEventFlag 11210001, True")


        'Non-standard due to co-ords warp

        Script("PlayerHide 1")
        Script("ShowHUD False")
        Script("FadeOut")
        Script("SetHp 10000, 1.0")

        Script("WarpNextStage_Bonfire 1210998")

        Script("Wait 1000")

        Script("WaitForLoad")
        Script("BlackScreen")
        Script("PlayerHide 1")
        Script("SetDisableGravity 10000, 1")


        Script("Wait 500")
        'facing = 45 deg
        Script("Warp_Coords 931.82, -318.63, 472.45")


        Script("Wait 1500")
        Script("FadeIn")
        Script("ShowHUD 1")
        Script("PlayerHide 0")
        Script("SetDisableGravity 10000, 0")
    End Sub
    Public Sub scenariopinwheeldefense()
        Script("SetEventFlag 6, False")
        Script("WarpNextStage_Bonfire 1300999")
        Script("wait 1000")
        Script("waitforload")
        Script("blackscreen")
        Script("wait 200")
        Script("warp_coords 42.88, -144.56, 236.94")
        Script("camreset 10000, 1")
        Script("setdisable 6550, 0")
        Script("wait 100")
        Script("intvar1 = chrfadein 6550, 0, 0")
        Script("forceentitydrawgroup intvar1")
        Script("warpentity_coords intvar1, 46.0, -165.8, 152.02, 180")
        Script("warp_coords 46.0, -165.8, 152.02")
        Script("camreset 10000, 1")
        Script("enablelogic 6550, 1")
        Script("enablelogic 10000, 0")
        Script("setdrawenable 10000, 0")
        Script("setdrawenable 6550, 1")
        Script("intvar1 = chrfadein 1300800, 0, 0")
        Script("setdisable 10000, 1")
        Script("controlentity intvar1, 1")
        Script("camfocusentity intvar1")
        Script("wait 1000")
        Script("fadein")
    End Sub

    Public Sub beginbossrush()


        Dim msg As String

        Script("ShowHUD False")

        setgendialog("Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not GenDiagResponse = 1 Then
            setgendialog("So much shame...", 2, "I know", "I don't care")
            Script("ShowHUD 1")
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
            Script("Wait 1000")
        Next


        setbriefingmsg("Begin")

        Script("CroseBriefingMsg")
        Script("Wait 1000")


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
        Script("BlackScreen")
        Script("ShowHUD False")
        Script("Wait 10000")
        Script("FadeIn")
        Script("ShowHUD 1")
        Script("CroseBriefingMsg")

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

        Script("ShowHUD False")

        setgendialog("Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not GenDiagResponse = 1 Then
            setgendialog("So much shame...", 2, "I know", "I don't care")
            Script("ShowHUD 1")
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
            Script("Wait 1000")
        Next


        setbriefingmsg("Begin")

        Script("CroseBriefingMsg")
        Script("Wait 1000")


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
        Script("BlackScreen")
        Script("ShowHUD False")
        Script("Wait 10000")
        Script("FadeIn")
        Script("ShowHUD 1")
        Script("CroseBriefingMsg")

        rushTimer.Abort()

    End Sub






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

        If getIsBossRushThreadActive() Then

            Script("ShowHUD 1")
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)

            If rushMode Then rushTimer.Abort()
            rushMode = False
            trd.Abort()
            Console.WriteLine(trd.ThreadState)
        Else
            If (cboxReverseOrder.Checked) Then
                trd = New Thread(AddressOf beginreversebossrush)
            Else
                trd = New Thread(AddressOf beginbossrush)
            End If
            trd.IsBackground = True
            trd.Start()
        End If

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



    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim updateWindow As New UpdateWindow(sender.Tag)
        updateWindow.ShowDialog()
        If updateWindow.WasSuccessful Then
            Process.Start(updateWindow.NewAssembly, """--old-file=" & updateWindow.OldAssembly & """")
            Me.Close()
        End If
    End Sub

    Private Sub btnTestDisableAI_Click(sender As Object, e As EventArgs)
        test_disableai(True)
    End Sub

    Private Sub btnTestEnableAI_Click(sender As Object, e As EventArgs)
        test_disableai(False)
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

    Private Sub btnX_Click(sender As Object, e As EventArgs)

        Script("ShowHUD 1")
        WInt32(RInt32(&H13786D0) + &H154, -1)
        WInt32(RInt32(&H13786D0) + &H158, -1)

        If rushMode Then rushTimer.Abort()
        rushMode = False
        trd.Abort()
        Console.WriteLine(trd.ThreadState)
    End Sub

    Private Sub btnConsoleExecute_Click(sender As Object, e As EventArgs) Handles btnConsoleExecute.Click
        For Each line In txtConsole.Lines
            Try
                Dim result As Integer
                Dim trimmedLine = line.Trim()
                If Not trimmedLine.Any(
                    Function(x As Char) As Boolean
                        Return Not (x = " "c)
                    End Function
                ) Then
                    Continue For
                End If


                result = Script(trimmedLine)

                txtConsoleResult.Text = "Hex: 0x" & Hex(result) & Environment.NewLine &
                                           "Int: " & result & Environment.NewLine &
                                           "Float: " & BitConverter.ToSingle(BitConverter.GetBytes(result), 0)
            Catch ex As Exception
                MsgBox(line & Environment.NewLine & Environment.NewLine & ex.Message)
            End Try

        Next
    End Sub

    Private Function Script(ByVal str As String) As Integer
        Dim action As String
        Dim params() As String = {}

        Dim storedVal As String = ""

        If str.Contains("=") Then
            storedVal = str.Replace(" ", "").Split("=")(0)
            str = str.Split("=")(1).TrimStart(" ")
        End If


        action = str.Split(" ")(0).ToLower
        If clsFuncLocs.Contains(action) Then
            str = "funccall " & action & ", " & str.ToLower.Replace(action, "")
            action = "funccall"
        Else
            str = str.ToLower.Replace(action, "")
        End If

        If str.Contains(" ") Then
            str = str.Replace(action & " ", "")
            params = str.Replace(" ", "").Split(",")
        End If

        For i = 0 To params.Count - 1
            If params(i).ToLower = "true" Then params(i) = "1"
            If params(i).ToLower = "false" Then params(i) = "0"
            If params(i).ToLower = "intvar1" Then params(i) = intvar1.ToString
        Next


        Dim t As Type = Me.GetType
        Dim pt As Type
        Dim method As MethodInfo
        method = t.GetMethod(action)

        For i = 0 To (method.GetParameters.Count - params.Length) - 1
            Array.Resize(params, params.Length + 1)
            params(params.Length - 1) = "0"
        Next

        Dim typedParams() As Object = {}
        For i = 0 To method.GetParameters.Count - 1
            Array.Resize(typedParams, typedParams.Length + 1)

            If method.GetParameters(i).ParameterType.IsByRef Then
                pt = method.GetParameters(i).ParameterType.GetElementType()
            Else
                pt = method.GetParameters(i).ParameterType()
            End If

            'Fix for non-decimal using regions
            If pt.Name = "Single" Then
                typedParams(typedParams.Length - 1) = Convert.ToSingle(params(i), New CultureInfo("en-us"))
            Else
                typedParams(typedParams.Length - 1) = CTypeDynamic(params(i), pt)
            End If
        Next


        Dim result As Integer
        result = method.Invoke(Me, typedParams)

        Select Case storedVal
            Case "intvar1"
                intvar1 = result
        End Select

        Return result

    End Function

    Private Sub btnTest_Click(sender As Object, e As EventArgs)

        'Script("SetEventFlag 16, 0)
        'warp_coords_facing(71.72, 60, 300.56, 1.0)

        Script("intvar1 = ChrFadeIn 1010700, 1.0")
        Script("ControlEntity intvar1, 0")


    End Sub

    Private Sub btnConsoleHelp_Click(sender As Object, e As EventArgs) Handles btnConsoleHelp.Click
        Dim webAddress As String = "https://docs.google.com/spreadsheets/d/1Gff9pSGpYCJeNAXzUamqAInqFUwk4BhC6dC9Qk3_cDI/edit#gid=0"
        Process.Start(webAddress)
    End Sub

    Private Sub btnScenarioPinwheelDefense_Click(sender As Object, e As EventArgs) Handles btnScenarioPinwheelDefense.Click
        trd = New Thread(AddressOf scenariopinwheeldefense)
        trd.IsBackground = True
        trd.Start()
    End Sub

    Private Sub btnTestEnablePlayerExterminate_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnTestDisablePlayerExterminate_Click(sender As Object, e As EventArgs)
        test_playerexterminate(False)
    End Sub

    Private Sub DisableAIToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TestToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Script("SetEventFlag 16, 0)
        'warp_coords_facing(71.72, 60, 300.56, 1.0)

        Script("intvar1 = ChrFadeIn 1010700, 1.0")
        Script("ControlEntity intvar1, 0")
    End Sub

    Private Sub EnablePlayerExterminateToolStripMenuItem_Click(sender As Object, e As EventArgs)
        test_playerexterminate(1)
    End Sub

    Private Sub DisablePlayerExterminateToolStripMenuItem_Click(sender As Object, e As EventArgs)
        test_playerexterminate(0)
    End Sub

    Private Sub SomethingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SomethingToolStripMenuItem.Click
        'Script("SetEventFlag 16, 0)
        'warp_coords_facing(71.72, 60, 300.56, 1.0)

        Script("intvar1 = ChrFadeIn 1010700, 1.0")
        Script("ControlEntity intvar1, 0")
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        test_disableai(True)
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        test_disableai(False)
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        test_playerexterminate(True)
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        test_playerexterminate(False)
    End Sub
End Class
