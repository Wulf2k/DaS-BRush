Imports System.Threading

Public Class Game

    Public Class Injected
        Public Shared ItemDropPtr As New IngameAllocatedPtr()
    End Class

    Private Shared hookCheckThread As Thread

    ';)
    <HideFromScripting>
    Public Shared Property IsHooked As Boolean

    ';)
    <HideFromScripting>
    Public Shared ReadOnly Property DetectedDarkSoulsVersion As String = "Nothing"

    ';)
    <HideFromScripting>
    Public Shared Event OnCheckHook(ByVal hooked As Boolean)

    ';)
    <HideFromScripting>
    Public Shared ReadOnly HookCheckInterval As Integer = 33

    Private Shared Sub _checkHook()
        If (RUInt32(&H400080) = &HFC293654&) Then
            _DetectedDarkSoulsVersion = "Dark Souls (Latest Release Ver.)"

            Dim tmpProtect As Integer
            VirtualProtectEx(_targetProcessHandle, &H10CC000, &H1DE000, 4, tmpProtect)

            WBytes(&HBE73FE, {&H20})
            WBytes(&HBE719F, {&H20})
            WBytes(&HBE722B, {&H20})

            Funcs.SetSaveEnable(False)

            _IsHooked = True
        Else
            If (RUInt32(&H400080) = &HE91B11E2&) Then
                _DetectedDarkSoulsVersion = "Dark Souls (Invalid Beta)"
            Else
                _DetectedDarkSoulsVersion = "None"
            End If

            _IsHooked = False
        End If

        RaiseEvent OnCheckHook(_IsHooked)

        If IsHooked Then
            If hookCheckThread Is Nothing OrElse Not hookCheckThread.IsAlive Then
                hookCheckThread = New Thread(AddressOf _updateHook) With {.IsBackground = True, .Priority = ThreadPriority.BelowNormal, .Name = "DaS.ScriptLib.HookCheckLoop"}
                hookCheckThread.Start()
            End If
        Else
            If hookCheckThread IsNot Nothing AndAlso hookCheckThread.IsAlive Then
                hookCheckThread.Abort()
            End If
        End If
    End Sub

    ';)
    <HideFromScripting>
    Public Shared Sub Hook()
        _IsHooked = False

        If ScanForProcess("DARK SOULS", True) Then
            _checkHook()
        End If
    End Sub

    ';)
    <HideFromScripting>
    Public Shared Sub Unhook()
        DetachFromProcess()
    End Sub

    Private Shared Sub _updateHook()
        _checkHook()
        If Not IsHooked Then
            hookCheckThread.Abort()
        End If
        Thread.Sleep(HookCheckInterval)
    End Sub

    Public Shared PtrToPtrToCharDataPtr1 As New LivePtrVarInt(Function() &H137DC70)
    Public Shared PtrToCharDataPtr1 As New LivePtrVarInt(Function() PtrToPtrToCharDataPtr1.Value + &H4)
    Public Shared CharDataPtr1 As New LivePtrVarInt(Function() PtrToCharDataPtr1.Value)

    Public Shared GameStatsPtr As New LivePtrVarInt(Function() &H1378700)
    Public Shared CharDataPtr2 As New LivePtrVarInt(Function() GameStatsPtr.Value + &H8)

    Public Shared CharMapDataPtr As New LivePtrVarInt(Function() CharDataPtr1.Value + &H28)

    Public Shared MenuPtr As New LivePtrVarInt(Function() &H13786D0)
    Public Shared LinePtr As New LivePtrVarInt(Function() &H1378388)
    Public Shared KeyPtr As New LivePtrVarInt(Function() &H1378640)

    Public Shared EntityControllerPtr As New LivePtrVarInt(Function() CharMapDataPtr.Value + &H54)
    Public Shared EntityAnimPtr As New LivePtrVarInt(Function() CharMapDataPtr.Value + &H14)
    Public Shared PtrToPtrToEntityAnimInstancePtr As New LivePtrVarInt(Function() EntityAnimPtr.Value + &HC)
    Public Shared PtrToEntityAnimInstancePtr As New LivePtrVarInt(Function() PtrToPtrToEntityAnimInstancePtr.Value + &H10)
    Public Shared EntityAnimInstancePtr As New LivePtrVarInt(Function() PtrToEntityAnimInstancePtr)

    Public Class GameStats

        Public Shared ClearCount As New LivePtrVarInt(Function() GameStatsPtr.Value + &H3C)
        Public Shared TrueDeathCount As New LivePtrVarInt(Function() GameStatsPtr.Value + &H58)
        Public Shared TotalPlayTime As New LivePtrVarInt(Function() GameStatsPtr.Value + &H68)

    End Class

    Public Class Player

        Public Shared PosDataPtr As New LivePtrVarInt(Function() CharMapDataPtr.Value + &H1C)

        Public Shared Heading As New LivePtrVarFloat(Function() PosDataPtr.Value + &H4)
        Public Shared PosX As New LivePtrVarFloat(Function() PosDataPtr.Value + &H10)
        Public Shared PosY As New LivePtrVarFloat(Function() PosDataPtr.Value + &H14)
        Public Shared PosZ As New LivePtrVarFloat(Function() PosDataPtr.Value + &H18)

        Public Shared StablePosPtr As New LivePtrVarInt(Function() &H13784A0)
        Public Shared StablePosX As New LivePtrVarFloat(Function() StablePosPtr.Value + &HB70)
        Public Shared StablePosY As New LivePtrVarFloat(Function() StablePosPtr.Value + &HB74)
        Public Shared StablePosZ As New LivePtrVarFloat(Function() StablePosPtr.Value + &HB78)

        Public Shared BonfirePtr As New LivePtrVarInt(Function() &H13784A0)
        Public Shared BonfireID As New LivePtrVarInt(Function() BonfirePtr.Value + &HB04)

        Public Shared ModelName As New LivePtrVarUnicodeStr(Function() CharDataPtr1.Value + &H38)
        Public Shared NPCParamID As New LivePtrVarInt(Function() CharDataPtr1.Value + &H64)
        Public Shared NPCID As New LivePtrVarInt(Function() CharDataPtr1.Value + &H68)
        Public Shared NPCID2 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H6C)
        Public Shared ChrType As New LivePtrVarInt(Function() CharDataPtr1.Value + &H70)
        Public Shared TeamType As New LivePtrVarInt(Function() CharDataPtr1.Value + &H74)
        Public Shared TargetLock As New LivePtrVarByte(Function() CharDataPtr1.Value + &H128)
        Public Shared DeathStructPointer As New LivePtrVarInt(Function() CharDataPtr1.Value + &H170)

        Public Class DeathStruct
            Public Shared IsDead As New LivePtrVarByte(Function() DeathStructPointer.Value + &H18)
        End Class

        Public Shared PoiseCurrent As New LivePtrVarFloat(Function() CharDataPtr1.Value + &H1C0)
        Public Shared PoiseMax As New LivePtrVarFloat(Function() CharDataPtr1.Value + &H1C4)
        Public Shared PoiseRecoverTimer As New LivePtrVarFloat(Function() CharDataPtr1.Value + &H1CC)
        Public Shared OptionsBitmask As New LivePtrVarInt(Function() CharDataPtr1.Value + &H1FC) 'TODODODODODO
        Public Shared OptionsBitmask2 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H200) 'TODODODODODO
        Public Shared EventEntityID As New LivePtrVarInt(Function() CharDataPtr1.Value + &H208)
        Public Shared Opacity As New LivePtrVarFloat(Function() CharDataPtr1.Value + &H258)
        Public Shared DrawGroup1 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H264)
        Public Shared DrawGroup2 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H268)
        Public Shared DrawGroup3 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H26C)
        Public Shared DrawGroup4 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H270)
        Public Shared DispGroup1 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H274)
        Public Shared DispGroup2 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H278)
        Public Shared DispGroup3 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H27C)
        Public Shared DispGroup4 As New LivePtrVarInt(Function() CharDataPtr1.Value + &H280)
        Public Shared MultiplayerZone As New LivePtrVarInt(Function() CharDataPtr1.Value + &H284)
        Public Shared Material_Floor As New LivePtrVarShort(Function() CharDataPtr1.Value + &H288)
        Public Shared Material_ArmorSE As New LivePtrVarShort(Function() CharDataPtr1.Value + &H28A)
        Public Shared Material_ArmorSFX As New LivePtrVarShort(Function() CharDataPtr1.Value + &H28C)
        Public Shared HP As New LivePtrVarInt(Function() CharDataPtr1.Value + &H2D4)
        Public Shared MaxHP As New LivePtrVarInt(Function() CharDataPtr1.Value + &H2D8)
        Public Shared Stamina As New LivePtrVarInt(Function() CharDataPtr1.Value + &H2E4)
        Public Shared MaxStamina As New LivePtrVarInt(Function() CharDataPtr1.Value + &H2E8)
        Public Shared ResistancePoisonCurrent As New LivePtrVarInt(Function() CharDataPtr1.Value + &H300)
        Public Shared ResistanceToxicCurrent As New LivePtrVarInt(Function() CharDataPtr1.Value + &H304)
        Public Shared ResistanceBleedCurrent As New LivePtrVarInt(Function() CharDataPtr1.Value + &H308)
        Public Shared ResistanceCurseCurrent As New LivePtrVarInt(Function() CharDataPtr1.Value + &H30C)
        Public Shared ResistancePoisonMax As New LivePtrVarInt(Function() CharDataPtr1.Value + &H310)
        Public Shared ResistanceToxicMax As New LivePtrVarInt(Function() CharDataPtr1.Value + &H314)
        Public Shared ResistanceBleedMax As New LivePtrVarInt(Function() CharDataPtr1.Value + &H318)
        Public Shared ResistanceCurseMax As New LivePtrVarInt(Function() CharDataPtr1.Value + &H31C)
        Public Shared Unknown1Ptr As New LivePtrVarInt(Function() CharDataPtr1.Value + &H330)

        Public Class Unknown1
            Public Shared NoHit As New LivePtrVarBool(Function() Unknown1Ptr.Value + &H10)
        End Class

        Public Shared TalkID As New LivePtrVarInt(Function() CharDataPtr1.Value + &H348)
        Public Shared DebugOptionsBitMask As New LivePtrVarInt(Function() CharDataPtr1.Value + &H3C4) 'TODODODODODODODODODODODO
        Public Shared StatsPtr As New LivePtrVarInt(Function() CharDataPtr1.Value + &H414)

        Public Class Stats
            Public Shared MaxHP As New LivePtrVarInt(Function() StatsPtr.Value + &H14)
            Public Shared MaxStamina As New LivePtrVarInt(Function() StatsPtr.Value + &H30)
            Public Shared VIT As New LivePtrVarInt(Function() StatsPtr.Value + &H38)
            Public Shared ATN As New LivePtrVarInt(Function() StatsPtr.Value + &H40)
            Public Shared ENDurance As New LivePtrVarInt(Function() StatsPtr.Value + &H48) ' fuck vb
            Public Shared STR As New LivePtrVarInt(Function() StatsPtr.Value + &H50)
            Public Shared DEX As New LivePtrVarInt(Function() StatsPtr.Value + &H58)
            Public Shared INT As New LivePtrVarInt(Function() StatsPtr.Value + &H60)
            Public Shared FTH As New LivePtrVarInt(Function() StatsPtr.Value + &H68)
            Public Shared RES As New LivePtrVarInt(Function() StatsPtr.Value + &H80)
            Public Shared Humanity As New LivePtrVarInt(Function() StatsPtr.Value + &H7C)
            Public Shared ExternalGenitals As New LivePtrVarInt(Function() StatsPtr.Value + &HC2)
        End Class

        Public Shared AI_ID As New LivePtrVarInt(Function() CharDataPtr1.Value + &H41C)
        Public Shared DebugDrawWeaponDammyPolygon As New LivePtrVarBool(Function() CharDataPtr1.Value + &H6C7)
        Public Shared DebugDrawBloodMarkpos As New LivePtrVarBool(Function() CharDataPtr1.Value + &H6C8)

        Public Class Anim
            Public Shared SecondsPlaying As New LivePtrVarFloat(Function() EntityAnimInstancePtr.Value + &H8)
            Public Shared CurrentSpeedMult As New LivePtrVarFloat(Function() EntityAnimInstancePtr.Value + &H40)
            Public Shared TimesLooped As New LivePtrVarInt(Function() EntityAnimInstancePtr.Value + &H44)

            Public Shared DebugOverallSpeedMult As New LivePtrVarFloat(Function() EntityAnimPtr.Value + &H64)
            Public Shared DebugDrawSkeleton As New LivePtrVarBool(Function() EntityAnimPtr.Value + &H68)
            Public Shared DebugDrawBoneName As New LivePtrVarBool(Function() EntityAnimPtr.Value + &H69)
            Public Shared DebugDrawExtractMotion As New LivePtrVarBool(Function() EntityAnimPtr.Value + &H81)
            Public Shared DebugAnimSlotLog As New LivePtrVarBool(Function() EntityAnimPtr.Value + &H82)
        End Class

        Public Class Controller
            Public Shared MoveX As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H10)
            Public Shared MoveY As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H18)

            Public Shared CamRotSpeedH As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H50)
            Public Shared CamRotSpeedV As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H54)

            Public Shared CamRotH As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H60)
            Public Shared CamRotV As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H64)

            Public Shared R1Held_Sometimes As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H84)
            Public Shared L2OrR1Held_Sometimes As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H85)
            Public Shared RMouseHeld As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H89)
            Public Shared R1Held As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H8B)
            Public Shared XHeld As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H92)
            Public Shared L1Held As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H97)
            Public Shared L2OrTabHeld As New LivePtrVarBool(Function() EntityControllerPtr.Value + &H98)
            Public Shared L1Held2 As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HB7)
            Public Shared R1HeldOrCircleTapped As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HB9)
            Public Shared RMouseOrR2Held As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HBE)
            Public Shared R1OrLMouseHeld As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HC0)
            Public Shared L2Held As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HC2)
            Public Shared CircleTapped As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HC2) 'TODODODODODODODO
            Public Shared XHeld2 As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HC7)
            Public Shared L1Held3 As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HCC)
            Public Shared L2Held2 As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HCD)
            Public Shared L1Held4 As New LivePtrVarBool(Function() EntityControllerPtr.Value + &HEC)
            Public Shared SecondsR1Held As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &HF0)
            Public Shared SecondsGuarding As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &HF4)
            Public Shared SecondsR2Held As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H104)
            Public Shared SecondsR1Held2 As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H10C)
            Public Shared SecondsL1Held As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H13C)
            Public Shared SecondsGuarding2 As New LivePtrVarFloat(Function() EntityControllerPtr.Value + &H144)
        End Class

    End Class

End Class