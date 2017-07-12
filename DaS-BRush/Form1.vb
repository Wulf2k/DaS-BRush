Imports System.Runtime.InteropServices
Imports System.Threading


Public Class frmForm1

    'TODO:
    'Check equipment durability
    'Get Artorias WarpID
    'Get Ceaseless WarpID
    'Get Gargoyle's WarpID
    'Get Kalameet's WarpID
    'Get Sanctuary Guardians WarpID
    'Test O&S warp to fight trigger, then immediate warp to solid ground.

    'Test Moonlight Butterfy fight


    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Private trd As Thread


    Private Declare Function OpenProcess Lib "kernel32.dll" (ByVal dwDesiredAcess As UInt32, ByVal bInheritHandle As Boolean, ByVal dwProcessId As Int32) As IntPtr
    Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByRef lpNumberOfBytesRead As Integer) As Boolean
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer() As Byte, ByVal iSize As Integer, ByVal lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Boolean
    Private Declare Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As IntPtr, ByVal flAllocationType As Integer, ByVal flProtect As Integer) As IntPtr
    Private Declare Function CreateRemoteThread Lib "kernel32" (ByVal hProcess As Integer, ByVal lpThreadAttributes As Integer, ByVal dwStackSize As Integer, ByVal lpStartAddress As Integer, ByVal lpParameter As Integer, ByVal dwCreationFlags As Integer, ByRef lpThreadId As Integer) As Integer

    Public Const PROCESS_VM_READ = &H10
    Public Const TH32CS_SNAPPROCESS = &H2
    Public Const MEM_COMMIT = 4096
    Public Const PAGE_READWRITE = 4
    Public Const PROCESS_CREATE_THREAD = (&H2)
    Public Const PROCESS_VM_OPERATION = (&H8)
    Public Const PROCESS_VM_WRITE = (&H20)
    Public Const PROCESS_ALL_ACCESS = &H1F0FFF

    Dim isHooked As Boolean = False



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


    Dim charptr1 As UInteger
    Dim charmapdataptr As UInteger
    Dim charposdataptr As UInteger
    Dim charptr2 As UInteger
    Dim charptr3 As UInteger
    Dim enemyptr As UInteger
    Dim enemyptr2 As UInteger
    Dim enemyptr3 As UInteger
    Dim enemyptr4 As UInteger
    Dim tendptr As UInteger


    Dim gamestatsptr As UInteger
    Dim bonfireptr As UInteger

    Dim funcPtr As UInteger

    Dim delay As Integer = 33


    Dim playerHP As Integer
    Dim playerStam As Integer

    Dim playerMaxHP As Integer
    Dim playerMaxStam As Integer

    Dim playerFacing As Single
    Dim playerXpos As Single
    Dim playerYpos As Single
    Dim playerZpos As Single


    Private _targetProcess As Process = Nothing 'to keep track of it. not used yet.
    Private _targetProcessHandle As IntPtr = IntPtr.Zero 'Used for ReadProcessMemory


    Public Function ScanForProcess(ByVal windowCaption As String, Optional automatic As Boolean = False) As Boolean
        Dim _allProcesses() As Process = Process.GetProcesses
        For Each pp As Process In _allProcesses
            If pp.MainWindowTitle.ToLower.Equals(windowCaption.ToLower) Then
                'found it! proceed.
                Return TryAttachToProcess(pp, automatic)
            End If
        Next
        Return False
    End Function
    Public Function TryAttachToProcess(ByVal proc As Process, Optional automatic As Boolean = False) As Boolean
        If Not (_targetProcessHandle = IntPtr.Zero) Then
            DetachFromProcess()
        End If

        _targetProcess = proc
        _targetProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, False, _targetProcess.Id)
        If _targetProcessHandle = 0 Then
            If Not automatic Then 'Showing 2 message boxes as soon as you start the program is too annoying.
                MessageBox.Show("Failed to attach to process.Please run with administrative privileges.")
            End If

            Return False
        Else
            'if we get here, all connected and ready to use ReadProcessMemory()
            Return True
            'MessageBox.Show("OpenProcess() OK")
        End If

    End Function
    Public Sub DetachFromProcess()
        If Not (_targetProcessHandle = IntPtr.Zero) Then
            _targetProcess = Nothing
            Try
                CloseHandle(_targetProcessHandle)
                _targetProcessHandle = IntPtr.Zero
                'MessageBox.Show("MemReader::Detach() OK")
            Catch ex As Exception
                'MessageBox.Show("Warning: MemoryManager::DetachFromProcess::CloseHandle error " & Environment.NewLine & ex.Message)
            End Try
        End If
    End Sub

    Public Sub initClls()
        Dim nameList As New List(Of String)

        cllItemCats = {clsWeapons, clsArmor, clsRings, clsGoods}
        cllItemCatsIDs = {clsWeaponsIDs, clsArmorIDs, clsRingsIDs, clsGoodsIDs}


        '-----------------------Function names-----------------------
        nameList = ParseItems(clsFuncNames, clsFuncLocs, My.Resources.FuncLocs)
        For Each func In nameList
            cmbFuncName.Items.Add(func)
        Next
        cmbFuncName.SelectedItem = "PlayAnimation"


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


        cmbItemCat.SelectedIndex = 0
    End Sub

    Public Function ParseItems(ByRef cls As Hashtable, ByRef clsIDs As Hashtable, ByRef txt As String) As List(Of String)
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

    Public Function RInt8(ByVal addr As IntPtr) As SByte
        Dim _rtnBytes(0) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 1, vbNull)
        Return _rtnBytes(0)
    End Function
    Public Function RInt16(ByVal addr As IntPtr) As Int16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToInt16(_rtnBytes, 0)
    End Function
    Public Function RInt32(ByVal addr As IntPtr) As Int32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)

        Return BitConverter.ToInt32(_rtnBytes, 0)
    End Function
    Public Function RInt64(ByVal addr As IntPtr) As Int64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToInt64(_rtnBytes, 0)
    End Function
    Public Function RUInt16(ByVal addr As IntPtr) As UInt16
        Dim _rtnBytes(1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 2, vbNull)
        Return BitConverter.ToUInt16(_rtnBytes, 0)
    End Function
    Public Function RUInt32(ByVal addr As IntPtr) As UInt32
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToUInt32(_rtnBytes, 0)
    End Function
    Public Function RUInt64(ByVal addr As IntPtr) As UInt64
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToUInt64(_rtnBytes, 0)
    End Function
    Public Function RFloat(ByVal addr As IntPtr) As Single
        Dim _rtnBytes(3) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 4, vbNull)
        Return BitConverter.ToSingle(_rtnBytes, 0)
    End Function
    Public Function RDouble(ByVal addr As IntPtr) As Double
        Dim _rtnBytes(7) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, 8, vbNull)
        Return BitConverter.ToDouble(_rtnBytes, 0)
    End Function
    Public Function RIntPtr(ByVal addr As IntPtr) As IntPtr
        Dim _rtnBytes(IntPtr.Size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, IntPtr.Size, Nothing)
        If IntPtr.Size = 4 Then
            Return New IntPtr(BitConverter.ToUInt32(_rtnBytes, 0))
        Else
            Return New IntPtr(BitConverter.ToInt64(_rtnBytes, 0))
        End If
    End Function
    Public Function RBytes(ByVal addr As IntPtr, ByVal size As Int32) As Byte()
        Dim _rtnBytes(size - 1) As Byte
        ReadProcessMemory(_targetProcessHandle, addr, _rtnBytes, size, vbNull)
        Return _rtnBytes
    End Function
    Private Function RAsciiStr(ByVal addr As UInteger) As String
        Dim Str As String = ""
        Dim cont As Boolean = True
        Dim loc As Integer = 0

        Dim bytes(&H10) As Byte

        ReadProcessMemory(_targetProcessHandle, addr, bytes, &H10, vbNull)

        While (cont And loc < &H10)
            If bytes(loc) > 0 Then

                Str = Str + Convert.ToChar(bytes(loc))

                loc += 1
            Else
                cont = False
            End If
        End While

        Return Str
    End Function
    Private Function RUnicodeStr(ByVal addr As UInteger) As String
        Dim Str As String = ""
        Dim cont As Boolean = True
        Dim loc As Integer = 0

        Dim bytes(&H20) As Byte


        ReadProcessMemory(_targetProcessHandle, addr, bytes, &H20, vbNull)

        While (cont And loc < &H20)
            If bytes(loc) > 0 Then

                Str = Str + Convert.ToChar(bytes(loc))

                loc += 2
            Else
                cont = False
            End If
        End While

        Return Str
    End Function

    Public Sub WInt32(ByVal addr As IntPtr, val As Int32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WUInt32(ByVal addr As IntPtr, val As UInt32)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WFloat(ByVal addr As IntPtr, val As Single)
        WriteProcessMemory(_targetProcessHandle, addr, BitConverter.GetBytes(val), 4, Nothing)
    End Sub
    Public Sub WBytes(ByVal addr As IntPtr, val As Byte())
        WriteProcessMemory(_targetProcessHandle, addr, val, val.Length, Nothing)
    End Sub
    Public Sub WAsciiStr(addr As UInteger, str As String)
        WriteProcessMemory(_targetProcessHandle, addr, System.Text.Encoding.ASCII.GetBytes(str), str.Length, Nothing)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        initClls()

        refTimer = New System.Windows.Forms.Timer
        refTimer.Interval = delay
        refTimer.Enabled = True

        If ScanForProcess("DARK SOULS", True) Then
            'Check if this process is even Dark Souls
            checkDarkSoulsVersion()
            If isHooked Then

                funcAlloc()
            End If
        End If


    End Sub

    Private Sub funcAlloc()
        Dim TargetBufferSize = 1024
        funcPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_READWRITE)
    End Sub

    Private Sub checkDarkSoulsVersion()

        isHooked = True
        refTimer.Enabled = True


        If (RUInt32(&H400080) = &HFC293654&) Then
            lblRelease.Text = "Dark Souls (Latest Release Ver.)"
        Else
            lblRelease.Text = "None"
            isHooked = False
            refTimer.Enabled = False
        End If
    End Sub

    Private Sub refTimer_Tick() Handles refTimer.Tick

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




        Select Case tabs.SelectedIndex
            Case 0
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


            Case 2
                If Not nmbPhantomType.Focused Then nmbPhantomType.Value = RInt32(charptr1 + &H70)
                If Not nmbTeamType.Focused Then nmbTeamType.Value = RInt32(charptr1 + &H74)

                If Not nmbVitality.Focused Then nmbVitality.Value = RInt32(charptr2 + &H38)
                If Not nmbAttunement.Focused Then nmbAttunement.Value = RInt32(charptr2 + &H40)
                If Not nmbEnd.Focused Then nmbEnd.Value = RInt32(charptr2 + &H48)
                If Not nmbStr.Focused Then nmbStr.Value = RInt32(charptr2 + &H50)
                If Not nmbDex.Focused Then nmbDex.Value = RInt32(charptr2 + &H58)
                If Not nmbIntelligence.Focused Then nmbIntelligence.Value = RInt32(charptr2 + &H60)
                If Not nmbFaith.Focused Then nmbFaith.Value = RInt32(charptr2 + &H68)

                If Not nmbHumanity.Focused Then nmbHumanity.Value = RInt32(charptr2 + &H7C)
                If Not nmbResistance.Focused Then nmbResistance.Value = RInt32(charptr2 + &H80)
                If Not nmbSoulLevel.Focused Then nmbSoulLevel.Value = RInt32(charptr2 + &H88)

                If Not txtSouls.Focused Then txtSouls.Text = RInt32(charptr2 + &H8C)

                If Not nmbIndictments.Focused Then nmbIndictments.Value = RInt32(charptr2 + &HEC)

        End Select


    End Sub






    Private Sub funcCall(ByRef func As String, LUAparams() As String)
        Dim bytes() As Byte
        Dim bytes2() As Byte

        Dim bytParams = New Integer() {&H1D, &H17, &H11, &HB, &H5}
        Dim bytJmp As Integer = &H23

        bytes = {&H55, &H8B, &HEC, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HE8, 0, 0, 0, 0, &H58, &H58, &H58, &H58, &H58, &H58, &H8B, &HE5, &H5D, &HC3}

        For i As Integer = 4 To 0 Step -1
            If LUAparams(i).Contains(".") Then
                bytes2 = BitConverter.GetBytes(Convert.ToSingle(LUAparams(i)))
            Else
                bytes2 = BitConverter.GetBytes(Convert.ToInt32(LUAparams(i)))
            End If

            Array.Copy(bytes2, 0, bytes, bytParams(i), bytes2.Length)
        Next
        bytes2 = BitConverter.GetBytes(CInt(0 - ((funcPtr + bytJmp + 4) - clsFuncLocs(func))))

        Array.Copy(bytes2, 0, bytes, bytJmp, bytes2.Length)
        WriteProcessMemory(_targetProcessHandle, funcPtr, bytes, 1024, 0)
        CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr, 0, 0, 0)
    End Sub

    Private Sub Warp(ByVal entityID As Integer, point As Integer)
        funcCall("Warp", {entityID, point, 0, 0, 0})
    End Sub
    Private Sub WarpNextStage(ByVal world As Integer, block As Integer, area As Integer)
        funcCall("WarpNextStage", {world, block, 0, 0, area})
    End Sub
    Private Sub WarpNextStage_Bonfire(ByVal bonfireID As Integer)
        funcCall("WarpNextStage_Bonfire", {bonfireID, 0, 0, 0, 0})
    End Sub

    Private Sub BlackScreen()
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        WFloat(tmpptr + &H270, 0)
        WFloat(tmpptr + &H274, 0)
        WFloat(tmpptr + &H278, 0)
    End Sub
    Private Sub ClearPlaytime()
        Dim tmpPtr As IntPtr = RIntPtr(&H1378700)
        WUInt32(tmpPtr + &H68, 0)
    End Sub
    Private Sub FadeIn()
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

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
    Private Sub FadeOut()
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 1.0


        For i = 0 To 33
            val = val - 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next
    End Sub
    Private Sub ShowHUD(ByVal state As Boolean)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)
        tmpptr = RUInt32(tmpptr + &H2C)

        WBytes(tmpptr + &HD, {state})
    End Sub
    Private Sub PlayerHide(ByVal state As Boolean)
        WBytes(&H13784E7, {state})
    End Sub
    Private Sub WaitForLoad()
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)

        Dim msPlayed As UInteger
        Dim loading As Boolean = True

        msPlayed = RUInt32(tmpptr + &H68)

        Do While loading
            loading = (msPlayed = RUInt32(tmpptr + &H68))

        Loop
    End Sub


    Private Sub StandardTransition(ByVal bonfireID, warpID)
        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        WarpNextStage_Bonfire(bonfireID)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()

        Warp(10000, warpID)

        Thread.Sleep(2000)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)

    End Sub


    Private Sub BossAsylum()
        StandardTransition(1810998, 1812997)
        ClearPlaytime()
    End Sub
    Private Sub BossBedOfChaos()
        'non-standard transition to allow quit-out
        'warp before fog gate to set last solid position

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        WarpNextStage_Bonfire(1410980)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()

        Warp(10000, 1412998)
        Thread.Sleep(200)
        Warp(10000, 1412997)

        Thread.Sleep(1800)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)

    End Sub
    Private Sub BossBellGargoyles()
        'StandardTransition()
    End Sub
    Private Sub BossCapraDemon()
        StandardTransition(1010998, 1012887)
    End Sub
    Private Sub BossCeaselessDischarge()
        'StandardTransition()
    End Sub
    Private Sub BossCentipedeDemon()
        StandardTransition(1410998, 1412896)
    End Sub
    Private Sub BossChaosWitchQuelaag()
        StandardTransition(1400980, 1402997)
    End Sub
    Private Sub BossCrossbreedPriscilla()
        StandardTransition(1102961, 1102997)
    End Sub
    Private Sub BossDarkSunGwyndolin()
        StandardTransition(1510982, 1512896)
    End Sub
    Private Sub BossDemonFiresage()
        StandardTransition(1410998, 1412416)
    End Sub
    Private Sub BossFourKings()
        StandardTransition(1600999, 1602996)
    End Sub
    Private Sub BossGapingDragon()
        StandardTransition(1000999, 1002997)
    End Sub
    Private Sub BossGravelordNito()
        StandardTransition(1310998, 1312110)
    End Sub
    Private Sub BossGwyn()
        StandardTransition(1800999, 1802996)
    End Sub
    Private Sub BossIronGolem()
        StandardTransition(1500999, 1502997)
    End Sub
    Private Sub BossKnightArtorias()
        'StandardTransition()
    End Sub


    Private Sub BossManus()
        StandardTransition(1210982, 1212997)
    End Sub
    Private Sub BossMoonlightButterfly()
        StandardTransition(1200999, 1202245)
    End Sub
    Private Sub BossOAndS()
        'StandardTransition()
    End Sub
    Private Sub BossPinwheel()
        StandardTransition(1300999, 1302999)
    End Sub
    Private Sub BossSanctuaryGuardian()
        'StandardTransition()
    End Sub
    Private Sub BossSeath()
        StandardTransition(1700999, 1702997)
    End Sub
    Private Sub BossSif()
        StandardTransition(1200999, 1202999)
    End Sub
    Private Sub BossStrayDemon()
        StandardTransition(1810998, 1812996)
    End Sub
    Private Sub BossTaurusDemon()
        StandardTransition(1010998, 1012897)
    End Sub




    Private Sub btnBossAsylumDemon_Click(sender As Object, e As EventArgs) Handles btnBossAsylumDemon.Click
        trd = New Thread(AddressOf BossAsylum)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBedOfChaos_Click(sender As Object, e As EventArgs) Handles btnBossBedOfChaos.Click
        trd = New Thread(AddressOf BossBedOfChaos)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBellGargoyles_Click(sender As Object, e As EventArgs) Handles btnBossBellGargoyles.Click
        trd = New Thread(AddressOf BossBellGargoyles)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCapraDemon_Click(sender As Object, e As EventArgs) Handles btnBossCapraDemon.Click
        trd = New Thread(AddressOf BossCapraDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCeaselessDischarge_Click(sender As Object, e As EventArgs) Handles btnBossCeaselessDischarge.Click
        trd = New Thread(AddressOf BossCeaselessDischarge)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCentipedeDemon_Click(sender As Object, e As EventArgs) Handles btnBossCentipedeDemon.Click
        trd = New Thread(AddressOf BossCentipedeDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossChaosWitchQuelaag_Click(sender As Object, e As EventArgs) Handles btnBossChaosWitchQuelaag.Click
        trd = New Thread(AddressOf BossChaosWitchQuelaag)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCrossbreedPriscilla_Click(sender As Object, e As EventArgs) Handles btnBossCrossbreedPriscilla.Click
        trd = New Thread(AddressOf BossCrossbreedPriscilla)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDarkSunGwyndolin_Click(sender As Object, e As EventArgs) Handles btnBossDarkSunGwyndolin.Click
        trd = New Thread(AddressOf BossDarkSunGwyndolin)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDemonFiresage_Click(sender As Object, e As EventArgs) Handles btnBossDemonFiresage.Click
        trd = New Thread(AddressOf BossDemonFiresage)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossFourKings_Click(sender As Object, e As EventArgs) Handles btnBossFourKings.Click
        trd = New Thread(AddressOf BossFourKings)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGapingDragon_Click(sender As Object, e As EventArgs) Handles btnBossGapingDragon.Click
        trd = New Thread(AddressOf BossGapingDragon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGravelordNito_Click(sender As Object, e As EventArgs) Handles btnBossGravelordNito.Click
        trd = New Thread(AddressOf BossGravelordNito)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGwyn_Click(sender As Object, e As EventArgs) Handles btnBossGwyn.Click
        trd = New Thread(AddressOf BossGwyn)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossIronGolem_Click(sender As Object, e As EventArgs) Handles btnBossIronGolem.Click
        trd = New Thread(AddressOf BossIronGolem)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossKnightArtorias_Click(sender As Object, e As EventArgs) Handles btnBossKnightArtorias.Click
        trd = New Thread(AddressOf BossKnightArtorias)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossManus_Click(sender As Object, e As EventArgs) Handles btnBossManus.Click
        trd = New Thread(AddressOf BossManus)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossMoonlightButterfly_Click(sender As Object, e As EventArgs) Handles btnBossMoonlightButterfly.Click
        trd = New Thread(AddressOf BossMoonlightButterfly)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossOandS_Click(sender As Object, e As EventArgs) Handles btnBossOandS.Click
        trd = New Thread(AddressOf BossOAndS)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossPinwheel_Click(sender As Object, e As EventArgs) Handles btnBossPinwheel.Click
        trd = New Thread(AddressOf BossPinwheel)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSanctuaryGuardian_Click(sender As Object, e As EventArgs) Handles btnBossSanctuaryGuardian.Click
        trd = New Thread(AddressOf BossSanctuaryGuardian)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSeath_Click(sender As Object, e As EventArgs) Handles btnBossSeath.Click
        trd = New Thread(AddressOf BossSeath)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSif_Click(sender As Object, e As EventArgs) Handles btnBossSif.Click
        trd = New Thread(AddressOf BossSif)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossStrayDemon_Click(sender As Object, e As EventArgs) Handles btnBossStrayDemon.Click
        trd = New Thread(AddressOf BossStrayDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossTaurusDemon_Click(sender As Object, e As EventArgs) Handles btnBossTaurusDemon.Click
        trd = New Thread(AddressOf BossTaurusDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
End Class
