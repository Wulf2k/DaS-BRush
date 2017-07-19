Imports System.Runtime.InteropServices
Imports System.Threading


Public Class frmForm1

    'TODO:
    'Check equipment durability

    'Set tails to be pre-cut to avoid drops


    'Bed of Chaos, reset platform-collapsing

    'butterfly

    'Grant ring for 4K fight

    '50001550 = Stop Rite of Kindling dropping?


    'Reported 3x Sanctuary Guardians
    'Reported Kaathe Present for 4K
    'Gwyn can open with a different attack

    Private WithEvents refTimer As New System.Windows.Forms.Timer()
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Short

    Private trd As Thread
    Private soulTimer As Thread


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
    Dim dropPtr As UInteger


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
                dropAlloc()
                funcAlloc()
            End If
        End If


    End Sub

    Private Sub funcAlloc()
        Dim TargetBufferSize = 1024
        funcPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_READWRITE)
    End Sub
    Private Sub dropAlloc()
        Dim TargetBufferSize = 1024
        dropPtr = VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_READWRITE)
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



        If Not trd Is Nothing Then

            If trd.ThreadState = &H10 Then
                gbBosses.Visible = True
                btnBeginBossRush.Enabled = True
                btnBeginReverseRush.Enabled = True
            Else
                gbBosses.Visible = False
                btnBeginBossRush.Enabled = False
                btnBeginReverseRush.Enabled = False
            End If
        Else
            gbBosses.Visible = True
            btnBeginBossRush.Enabled = True
            btnBeginReverseRush.Enabled = True
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




        Select Case tabs.SelectedIndex
            Case 1
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





    Private Sub DropItem(ByVal cat As String, item As String, num As Integer)
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
    Private Sub funcCall(ByRef func As String, LUAparams() As String)
        Dim bytes() As Byte
        Dim bytes2() As Byte

        Dim bytParams = New Integer() {&H1D, &H17, &H11, &HB, &H5}
        Dim bytJmp As Integer = &H23

        bytes = {&H55, &H8B, &HEC, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HB8, 0, 0, 0, 0, &H50, &HE8, 0, 0, 0, 0, &H58, &H58, &H58, &H58, &H58, &H58, &H8B, &HE5, &H5D, &HC3}

        For i As Integer = 4 To 0 Step -1
            If LUAparams(i) = "False" Then LUAparams(i) = 0
            If LUAparams(i) = "True" Then LUAparams(i) = 1
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
        Thread.Sleep(5)
    End Sub



    Private Sub SetEventFlag(ByVal flag As Integer, state As Boolean)
        funcCall("SetEventFlag", {flag, state, 0, 0, 0})
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
    Private Sub Warp_Coords(ByVal x As Single, y As Single, z As Single)
        WFloat(charmapdataptr + &HD0, x)
        WFloat(charmapdataptr + &HD4, y)
        WFloat(charmapdataptr + &HD8, z)
        WBytes(charmapdataptr + &HC8, {1})
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
            val = val - 0.03
            val = val - 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next
    End Sub
    Private Sub FlashRed(ByVal ms As Integer)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 1.0

        WFloat(tmpptr + &H270, 5.0)
        WFloat(tmpptr + &H274, 1.0)
        WFloat(tmpptr + &H278, 1.0)
        Thread.Sleep(ms)


        WBytes(tmpptr + &H26D, {0})

    End Sub
    Private Sub HealSelf()
        funcCall("SetHp", {10000, "1.0", 0, 0, 0})
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
    Private Sub PlayerSwoll(ByVal val As Single)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378700)
        tmpptr = RInt32(tmpptr + 8)

        WFloat(tmpptr + &H2B0, val / 3)
        WFloat(tmpptr + &H2B4, val / 3)
        WFloat(tmpptr + &H2B8, val * 1.3)
        WFloat(tmpptr + &H2BC, val)

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

        HealSelf()

        WarpNextStage_Bonfire(bonfireID)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        'funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        Thread.Sleep(500)

        Warp(10000, warpID)

        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        'funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})

    End Sub


    Private Sub BossAsylum()
        SetEventFlag(16, False) 'Boss Death Flag
        SetEventFlag(11810000, False) 'Tutorial Complete Flag
        SetEventFlag(11815395, True) 'Boss at lower position



        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1810998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        'facing 180 degrees
        Warp_Coords(3.15, 198.15, -6)
        SetEventFlag(11815390, True)

        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossBedOfChaos()
        SetEventFlag(10, False) 'Boss 

        SetEventFlag(11410000, False)
        SetEventFlag(11410200, False) 'Center Platform flag
        SetEventFlag(11410291, False) 'Arm flag
        SetEventFlag(11410292, False) 'Arm flag

        'non-standard transition to allow quit-out
        'warp before fog gate to set last solid position

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1410980)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        Warp(10000, 1412998)
        Thread.Sleep(250)
        Warp(10000, 1412997)

        Thread.Sleep(1250)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})

    End Sub
    Private Sub BossBellGargoyles()
        SetEventFlag(3, False) 'Boss Death Flag
        SetEventFlag(11010000, False) 'Boss Cinematic Viewed Flag



        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1010998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)

        SetEventFlag(11015390, True) 'Boss Fog Used
        SetEventFlag(11015393, True) 'Boss Area Entered
        Thread.Sleep(250)

        'facing 0 degrees
        Warp_Coords(10.8, 48.92, 87.26)


        Thread.Sleep(1250)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossBlackDragonKalameet()
        SetEventFlag(11210004, False)

        SetEventFlag(121, False)
        SetEventFlag(11210539, True)
        SetEventFlag(11210535, True)
        SetEventFlag(11210067, False)
        SetEventFlag(11210066, False)
        SetEventFlag(11210056, True)

        SetEventFlag(1821, True)
        SetEventFlag(11210592, True)


        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1210998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        'facing 107 degrees
        Warp_Coords(876.04, -344.73, 749.75)


        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossCapraDemon()
        SetEventFlag(11010902, False)


        'Non-standard due to random deaths
        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        HealSelf()

        WarpNextStage_Bonfire(1010998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        Thread.Sleep(500)
        'facing 238 degrees
        Warp_Coords(-73.17, -43.56, -15.17)
        'Warp(10000, 1012887)

        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossCeaselessDischarge()
        SetEventFlag(11410900, False) 'Boss death flag
        SetEventFlag(51410180, True) 'Corpse Loot reset

        SetEventFlag(11415385, True)
        SetEventFlag(11415378, True)
        SetEventFlag(11415373, True)
        SetEventFlag(11415372, True)

        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1410998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)

        Warp_Coords(250.53, -283.15, 72.1)
        Thread.Sleep(250)
        'facing 30 degrees
        Warp_Coords(402.45, -278.15, 15.5)




        Thread.Sleep(1250)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossCentipedeDemon()
        SetEventFlag(11410901, False)
        StandardTransition(1410998, 1412896)
    End Sub
    Private Sub BossChaosWitchQuelaag()
        SetEventFlag(9, False)
        StandardTransition(1400980, 1402997)
    End Sub
    Private Sub BossCrossbreedPriscilla()
        SetEventFlag(4, False) 'Boss Death flag
        SetEventFlag(1691, True) 'Boss Hostile flag
        SetEventFlag(11100531, False) 'Boss Disabled flag
        StandardTransition(1102961, 1102997)
    End Sub
    Private Sub BossDarkSunGwyndolin()
        SetEventFlag(11510900, False) 'Boss Death Flag
        SetEventFlag(11510523, False) 'Boss Disabled Flag

        StandardTransition(1510982, 1512896)
    End Sub
    Private Sub BossDemonFiresage()
        SetEventFlag(11410410, False)
        StandardTransition(1410998, 1412416)
    End Sub
    Private Sub BossFourKings()
        SetEventFlag(13, False)
        StandardTransition(1600999, 1602996)
    End Sub
    Private Sub BossGapingDragon()
        SetEventFlag(2, False) 'Boss Death Flag
        SetEventFlag(11000853, True) 'Channeler Death Flag
        'StandardTransition(1000999, 1002997)

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        HealSelf()

        WarpNextStage_Bonfire(1000999)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        'funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        Thread.Sleep(500)
        SetEventFlag(11005390, True)
        SetEventFlag(11005392, True)
        SetEventFlag(11005393, True)
        SetEventFlag(11005394, True)
        SetEventFlag(11005397, True)
        SetEventFlag(11000000, False)


        Warp(10000, 1002997)

        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        'funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})



    End Sub
    Private Sub BossGravelordNito()
        SetEventFlag(7, False)
        'StandardTransition(1310998, 1312110)

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        HealSelf()

        WarpNextStage_Bonfire(1310998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        Thread.Sleep(500)

        'Warp(10000, 1312110)
        Warp_Coords(-126.84, -265.12, -30.78)
        SetEventFlag(11315390, True)
        SetEventFlag(11315393, True)

        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})

    End Sub
    Private Sub BossGwyn()
        SetEventFlag(15, False)
        StandardTransition(1800999, 1802996)
    End Sub
    Private Sub BossIronGolem()
        SetEventFlag(11, False) 'Boss Death Flag
        SetEventFlag(11500865, True) 'Bomb-Tossing Giant Death Flag
        StandardTransition(1500999, 1502997)
    End Sub
    Private Sub BossKnightArtorias()
        SetEventFlag(11210001, False)
        SetEventFlag(1864, True) 'Ciarin Dead

        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1210998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        'facing 75.8 degrees
        Warp_Coords(1034.11, -330.0, 810.68)


        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossManus()
        SetEventFlag(11210002, False)
        StandardTransition(1210982, 1212997)
    End Sub
    Private Sub BossMoonlightButterfly()
        SetEventFlag(11200900, False)
        SetEventFlag(11205383, False)
        'StandardTransition(1200999, 1202245)

        'Non-standard due to flags
        'timing of warp/flags matters

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1200999)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})




        Thread.Sleep(500)
        Warp_Coords(181.39, 7.53, 29.01)
        Thread.Sleep(4000)
        SetEventFlag(11205383, True)

        Warp_Coords(178.82, 8.12, 30.77)



        Thread.Sleep(2000)
        FadeIn()
        ShowHUD(True)

        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})

    End Sub
    Private Sub BossOAndS()
        SetEventFlag(12, False)



        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1510998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})

        Thread.Sleep(500)
        'facing 90 degrees
        Warp_Coords(539.9, 142.6, 254.79)


        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossPinwheel()
        SetEventFlag(6, False)
        StandardTransition(1300999, 1302999)
    End Sub
    Private Sub BossSanctuaryGuardian()
        SetEventFlag(11210000, False)


        'Non-standard due to co-ords warp

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()
        HealSelf()

        WarpNextStage_Bonfire(1210998)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})


        Thread.Sleep(500)
        'facing = 45 deg
        Warp_Coords(931.82, -318.63, 472.45)


        Thread.Sleep(1500)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossSeath()
        SetEventFlag(14, False)
        SetEventFlag(11700000, False)

        StandardTransition(1700999, 1702997)
    End Sub
    Private Sub BossSif()
        SetEventFlag(5, False)
        SetEventFlag(11200000, False)
        SetEventFlag(11200001, False)
        SetEventFlag(11200002, False)
        SetEventFlag(11205392, False)
        SetEventFlag(11205393, False)
        SetEventFlag(11205394, False)
        'StandardTransition(1200999, 1202999)

        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        HealSelf()

        WarpNextStage_Bonfire(1200999)

        Thread.Sleep(1000)

        WaitForLoad()
        BlackScreen()
        PlayerHide(True)
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        Thread.Sleep(500)
        'Warp_Coords(274, -19.82, -266.43)
        Thread.Sleep(500)
        'Warp(10000, 1202999)
        Warp_Coords(254.31, -16.02, -320.32)

        Thread.Sleep(1000)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})
    End Sub
    Private Sub BossStrayDemon()
        SetEventFlag(11810000, True)
        SetEventFlag(11810900, False)
        StandardTransition(1810998, 1812996)
    End Sub
    Private Sub BossTaurusDemon()
        SetEventFlag(11010901, False)
        StandardTransition(1010998, 1012897)
    End Sub

    Private Sub BeginBossRush()

        soulTimer = New Thread(AddressOf BeginSoulTimer)
        soulTimer.IsBackground = True


        DropItem("Goods", "Dung Pie", 99)

        DropItem("Rings", "Covenant of Artorias", 1)



        PlayerSwoll(-4)



        FlashRed(3000)
        Thread.Sleep(3000)

        FlashRed(2000)
        Thread.Sleep(2000)

        FlashRed(1000)
        Thread.Sleep(1000)


        For i = 0 To 30
            FlashRed(33)
            Thread.Sleep(33)
        Next




        soulTimer.Start()
        ClearPlaytime()

        BossAsylum()
        WaitForBossDeath(0, &H8000)
        Thread.Sleep(5000)

        PlayerSwoll(-3.66)

        BossTaurusDemon()
        WaitForBossDeath(&HF70, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(-3.33)

        BossBellGargoyles()
        WaitForBossDeath(0, &H10000000)
        Thread.Sleep(5000)

        PlayerSwoll(-3)

        BossCapraDemon()
        WaitForBossDeath(&HF70, &H2000000)
        Thread.Sleep(5000)

        PlayerSwoll(-2.66)

        BossGapingDragon()
        WaitForBossDeath(0, &H20000000)
        Thread.Sleep(5000)

        PlayerSwoll(-2.33)

        BossMoonlightButterfly()
        WaitForBossDeath(&H1E70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-2)

        BossSif()
        WaitForBossDeath(0, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(-1.66)

        BossChaosWitchQuelaag()
        WaitForBossDeath(0, &H400000)
        Thread.Sleep(5000)

        PlayerSwoll(-1.33)

        BossStrayDemon()
        WaitForBossDeath(&H5A70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-1)

        BossIronGolem()
        WaitForBossDeath(0, &H100000)
        Thread.Sleep(5000)

        PlayerSwoll(-0.66)

        BossOAndS()
        WaitForBossDeath(0, &H80000)
        Thread.Sleep(5000)

        PlayerSwoll(-0.33)

        BossPinwheel()
        WaitForBossDeath(0, &H2000000)
        Thread.Sleep(5000)

        PlayerSwoll(0)

        BossGravelordNito()
        WaitForBossDeath(0, &H1000000)
        Thread.Sleep(5000)

        PlayerSwoll(0.33)

        BossSanctuaryGuardian()
        WaitForBossDeath(&H2300, &H80000000)
        Thread.Sleep(5000)

        PlayerSwoll(0.66)

        BossKnightArtorias()
        WaitForBossDeath(&H2300, &H40000000)
        Thread.Sleep(5000)

        PlayerSwoll(1)

        BossManus()
        WaitForBossDeath(&H2300, &H20000000)
        Thread.Sleep(5000)

        PlayerSwoll(1.66)

        BossCeaselessDischarge()
        WaitForBossDeath(&H3C70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(2)

        BossDemonFiresage()
        WaitForBossDeath(&H3C30, &H20)
        Thread.Sleep(5000)

        PlayerSwoll(2.33)

        BossCentipedeDemon()
        WaitForBossDeath(&H3C70, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(2.66)

        BossBlackDragonKalameet()
        WaitForBossDeath(&H2300, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(3)

        BossSeath()
        WaitForBossDeath(0, &H20000)
        Thread.Sleep(5000)

        PlayerSwoll(3.33)

        BossFourKings()
        WaitForBossDeath(0, &H40000)
        Thread.Sleep(5000)

        PlayerSwoll(3.66)

        BossCrossbreedPriscilla()
        WaitForBossDeath(0, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(4)

        BossDarkSunGwyndolin()
        WaitForBossDeath(&H4670, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(4.33)

        BossGwyn()
        WaitForBossDeath(0, &H10000)
        Thread.Sleep(5000)

        PlayerSwoll(6)


        For i = 0 To 30
            FlashRed(33)
            Thread.Sleep(33)
        Next

        soulTimer.Abort()
    End Sub
    Private Sub BeginReverseBossRush()
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



        soulTimer = New Thread(AddressOf BeginSoulTimer)
        soulTimer.IsBackground = True

        DropItem("Goods", "Dung Pie", 99)

        DropItem("Rings", "Covenant of Artorias", 1)



        PlayerSwoll(-4)



        FlashRed(3000)
        Thread.Sleep(3000)

        FlashRed(2000)
        Thread.Sleep(2000)

        FlashRed(1000)
        Thread.Sleep(1000)


        For i = 0 To 30
            FlashRed(33)
            Thread.Sleep(33)
        Next


        BossGwyn()
        ClearPlaytime()


        soulTimer.Start()
        WaitForBossDeath(0, &H10000)
        Thread.Sleep(5000)

        PlayerSwoll(-3.66)

        BossDarkSunGwyndolin()
        WaitForBossDeath(&H4670, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-3.33)

        BossCrossbreedPriscilla()
        WaitForBossDeath(0, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-3)

        BossFourKings()
        WaitForBossDeath(0, &H40000)
        Thread.Sleep(5000)

        PlayerSwoll(-2.66)

        BossSeath()
        WaitForBossDeath(0, &H20000)
        Thread.Sleep(5000)

        PlayerSwoll(-2.33)

        BossBlackDragonKalameet()
        WaitForBossDeath(&H2300, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-2)

        BossCentipedeDemon()
        WaitForBossDeath(&H3C70, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(-1.66)

        BossDemonFiresage()
        WaitForBossDeath(&H3C30, &H20)
        Thread.Sleep(5000)

        PlayerSwoll(-1.33)

        BossCeaselessDischarge()
        WaitForBossDeath(&H3C70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(-1)

        BossManus()
        WaitForBossDeath(&H2300, &H20000000)
        Thread.Sleep(5000)

        PlayerSwoll(-0.66)

        BossKnightArtorias()
        WaitForBossDeath(&H2300, &H40000000)
        Thread.Sleep(5000)

        PlayerSwoll(-0.33)

        BossSanctuaryGuardian()
        WaitForBossDeath(&H2300, &H80000000)
        Thread.Sleep(5000)

        PlayerSwoll(0)

        BossGravelordNito()
        WaitForBossDeath(0, &H1000000)
        Thread.Sleep(5000)

        PlayerSwoll(0.33)

        BossPinwheel()
        WaitForBossDeath(0, &H2000000)
        Thread.Sleep(5000)

        PlayerSwoll(0.66)

        BossOAndS()
        WaitForBossDeath(0, &H80000)
        Thread.Sleep(5000)

        PlayerSwoll(1)

        BossIronGolem()
        WaitForBossDeath(0, &H100000)
        Thread.Sleep(5000)

        PlayerSwoll(1.66)

        BossStrayDemon()
        WaitForBossDeath(&H5A70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(2)

        BossChaosWitchQuelaag()
        WaitForBossDeath(0, &H400000)
        Thread.Sleep(5000)

        PlayerSwoll(2.33)

        BossSif()
        WaitForBossDeath(0, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(2.66)

        BossMoonlightButterfly()
        WaitForBossDeath(&H1E70, &H8000000)
        Thread.Sleep(5000)

        PlayerSwoll(3)

        BossGapingDragon()
        WaitForBossDeath(0, &H20000000)
        Thread.Sleep(5000)

        PlayerSwoll(3.33)

        BossCapraDemon()
        WaitForBossDeath(&HF70, &H2000000)
        Thread.Sleep(5000)

        PlayerSwoll(3.66)

        BossBellGargoyles()
        WaitForBossDeath(0, &H10000000)
        Thread.Sleep(5000)

        PlayerSwoll(4)

        BossTaurusDemon()
        WaitForBossDeath(&HF70, &H4000000)
        Thread.Sleep(5000)

        PlayerSwoll(4.33)

        BossAsylum()
        WaitForBossDeath(0, &H8000)
        Thread.Sleep(5000)

        For i = 0 To 30
            FlashRed(33)
            Thread.Sleep(33)
        Next

        soulTimer.Abort()

        PlayerSwoll(6)

    End Sub

    Private Sub WaitForAsylumDeath()
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
            bossdead = (RInt32(eventPtr) And &H8000)
            selfdead = (RInt32(hpPtr) = 0)

            Thread.Sleep(33)
        End While

        If bossdead Then

        End If
        If selfdead Then
            trd.Abort()
        End If

    End Sub
    Private Sub WaitForBossDeath(ByVal boost As Integer, match As Integer)
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

        End If
        If selfdead Then
            soulTimer.Abort()
            trd.Abort()
        End If

    End Sub




    Private Sub btnBossAsylumDemon_Click(sender As Object, e As EventArgs) Handles btnBossAsylumDemon.Click, Button1.Click
        trd = New Thread(AddressOf BossAsylum)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBedOfChaos_Click(sender As Object, e As EventArgs) Handles btnBossBedOfChaos.Click, Button6.Click
        trd = New Thread(AddressOf BossBedOfChaos)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBellGargoyles_Click(sender As Object, e As EventArgs) Handles btnBossBellGargoyles.Click, Button4.Click
        trd = New Thread(AddressOf BossBellGargoyles)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossBlackDragonKalameet_Click(sender As Object, e As EventArgs) Handles btnBossBlackDragonKalameet.Click, Button26.Click
        trd = New Thread(AddressOf BossBlackDragonKalameet)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCapraDemon_Click(sender As Object, e As EventArgs) Handles btnBossCapraDemon.Click, Button3.Click
        trd = New Thread(AddressOf BossCapraDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCeaselessDischarge_Click(sender As Object, e As EventArgs) Handles btnBossCeaselessDischarge.Click, Button5.Click
        trd = New Thread(AddressOf BossCeaselessDischarge)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCentipedeDemon_Click(sender As Object, e As EventArgs) Handles btnBossCentipedeDemon.Click, Button7.Click
        trd = New Thread(AddressOf BossCentipedeDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossChaosWitchQuelaag_Click(sender As Object, e As EventArgs) Handles btnBossChaosWitchQuelaag.Click, Button8.Click
        trd = New Thread(AddressOf BossChaosWitchQuelaag)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossCrossbreedPriscilla_Click(sender As Object, e As EventArgs) Handles btnBossCrossbreedPriscilla.Click, Button9.Click
        trd = New Thread(AddressOf BossCrossbreedPriscilla)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDarkSunGwyndolin_Click(sender As Object, e As EventArgs) Handles btnBossDarkSunGwyndolin.Click, Button10.Click
        trd = New Thread(AddressOf BossDarkSunGwyndolin)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossDemonFiresage_Click(sender As Object, e As EventArgs) Handles btnBossDemonFiresage.Click, Button11.Click
        trd = New Thread(AddressOf BossDemonFiresage)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossFourKings_Click(sender As Object, e As EventArgs) Handles btnBossFourKings.Click, Button13.Click
        trd = New Thread(AddressOf BossFourKings)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGapingDragon_Click(sender As Object, e As EventArgs) Handles btnBossGapingDragon.Click, Button14.Click
        trd = New Thread(AddressOf BossGapingDragon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGravelordNito_Click(sender As Object, e As EventArgs) Handles btnBossGravelordNito.Click, Button15.Click
        trd = New Thread(AddressOf BossGravelordNito)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossGwyn_Click(sender As Object, e As EventArgs) Handles btnBossGwyn.Click, Button16.Click
        trd = New Thread(AddressOf BossGwyn)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossIronGolem_Click(sender As Object, e As EventArgs) Handles btnBossIronGolem.Click, Button17.Click
        trd = New Thread(AddressOf BossIronGolem)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossKnightArtorias_Click(sender As Object, e As EventArgs) Handles btnBossKnightArtorias.Click, Button18.Click
        trd = New Thread(AddressOf BossKnightArtorias)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossManus_Click(sender As Object, e As EventArgs) Handles btnBossManus.Click, Button19.Click
        trd = New Thread(AddressOf BossManus)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossMoonlightButterfly_Click(sender As Object, e As EventArgs) Handles btnBossMoonlightButterfly.Click, Button20.Click
        trd = New Thread(AddressOf BossMoonlightButterfly)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossOandS_Click(sender As Object, e As EventArgs) Handles btnBossOandS.Click, Button12.Click
        trd = New Thread(AddressOf BossOAndS)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossPinwheel_Click(sender As Object, e As EventArgs) Handles btnBossPinwheel.Click, Button2.Click
        trd = New Thread(AddressOf BossPinwheel)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSanctuaryGuardian_Click(sender As Object, e As EventArgs) Handles btnBossSanctuaryGuardian.Click, Button21.Click
        trd = New Thread(AddressOf BossSanctuaryGuardian)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSeath_Click(sender As Object, e As EventArgs) Handles btnBossSeath.Click, Button22.Click
        trd = New Thread(AddressOf BossSeath)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossSif_Click(sender As Object, e As EventArgs) Handles btnBossSif.Click, Button24.Click
        trd = New Thread(AddressOf BossSif)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossStrayDemon_Click(sender As Object, e As EventArgs) Handles btnBossStrayDemon.Click, Button23.Click
        trd = New Thread(AddressOf BossStrayDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBossTaurusDemon_Click(sender As Object, e As EventArgs) Handles btnBossTaurusDemon.Click, Button25.Click
        trd = New Thread(AddressOf BossTaurusDemon)
        trd.IsBackground = True
        trd.Start()
    End Sub


    Private Sub btnBeginBossRush_Click(sender As Object, e As EventArgs) Handles btnBeginBossRush.Click
        trd = New Thread(AddressOf BeginBossRush)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub btnBeginReverseRush_Click(sender As Object, e As EventArgs) Handles btnBeginReverseRush.Click
        trd = New Thread(AddressOf BeginReverseBossRush)
        trd.IsBackground = True
        trd.Start()
    End Sub
    Private Sub BeginSoulTimer()

        gamestatsptr = RUInt32(&H1378700)

        Dim SoulTimer As TimeSpan
        Dim Souls As Integer
        Dim msPlayed As Integer
        'souls = charptr2 + &H8C

        Do
            msPlayed = RInt32(gamestatsptr + &H68)
            SoulTimer = TimeSpan.FromMilliseconds(msPlayed)
            Souls = SoulTimer.Days * 1000000 + SoulTimer.Hours * 10000 + SoulTimer.Minutes * 100 + SoulTimer.Seconds

            WInt32(charptr2 + &H8C, Souls)
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
            End If
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click

        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})






    End Sub

    Private Sub btnTestTheAppleMan_Click(sender As Object, e As EventArgs) Handles btnTestTheAppleMan.Click
        SetEventFlag(3, False) 'Boss Death Flag
        SetEventFlag(11010000, False) 'Boss Cinematic Viewed Flag

        FadeOut()
        Warp_Coords(10.8, 48.92, 87.26)



        FadeIn()
        ShowHUD(True)
        PlayerHide(False)
    End Sub

    Private Sub btnTestSomeRedYeti_Click(sender As Object, e As EventArgs) Handles btnTestSomeRedYeti.Click
        SetEventFlag(11010902, False)
        'StandardTransition(1010998, 1012887)



        PlayerHide(True)
        ShowHUD(False)
        FadeOut()

        HealSelf()

        WarpNextStage_Bonfire(1010998)

        Thread.Sleep(1000)

        WaitForLoad()
        funcCall("SetDisableGravity", {10000, 1, 0, 0, 0})
        BlackScreen()
        PlayerHide(True)

        Warp(10000, 1012887)
        funcCall("SetDisableGravity", {10000, 0, 0, 0, 0})

        Thread.Sleep(2000)
        FadeIn()
        ShowHUD(True)
        PlayerHide(False)




    End Sub
End Class
