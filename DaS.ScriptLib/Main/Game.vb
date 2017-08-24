Imports System.Threading

Public Class Game

    Public Shared LuaFunctionPointer As Integer
    Public Shared NodeDumpPointer As Integer = 0
    Public Shared CharPointer1 As Integer
    Public Shared CharMapDataPointer As Integer
    Public Shared CharPosDataPointer As Integer
    Public Shared CharPointer2 As Integer
    Public Shared CharPointer3 As Integer
    Public Shared EnemyPointer As Integer
    Public Shared EnemyPointer2 As Integer
    Public Shared EnemyPointer3 As Integer
    Public Shared EnemyPointer4 As Integer
    Public Shared TendPointer As Integer
    Public Shared GameStatsPointer As Integer
    Public Shared BonfirePointer As Integer
    Public Shared DropPointer As Integer
    Public Shared PlayerStableFootPosPointer As Integer
    Public Shared MenuPointer As Integer
    Public Shared LinePointer As Integer
    Public Shared KeyPointer As Integer
    Public Shared EntityControllerPointer As Integer
    Public Shared EntityAnimPointer As Integer
    Public Shared EntityAnimInstancePointer As Integer

    Public Shared GenDiagResponse As Integer
    Public Shared GenDiagVal As Integer

    Public Shared rushTimer As Thread

    Public Shared ReadOnly Property IsHooked As Boolean
    Public Shared ReadOnly Property DetectedDarkSoulsVersion As String

    Private Shared Function AllocNewPtr() As Integer
        Dim TargetBufferSize = 1024
        Return VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Function

    Private Shared Sub AllocPointers()
        DropPointer = AllocNewPtr()
        LuaFunctionPointer = AllocNewPtr()
    End Sub

    Friend Shared Function CheckHook() As Boolean
        If (RUInt32(&H400080) = &HFC293654&) Then
            _DetectedDarkSoulsVersion = "Dark Souls (Latest Release Ver.)"

            Dim tmpProtect As Integer
            VirtualProtectEx(_targetProcessHandle, &H10CC000, &H1DE000, 4, tmpProtect)

            WBytes(&HBE73FE, {&H20})
            WBytes(&HBE719F, {&H20})
            WBytes(&HBE722B, {&H20})

            Funcs.SetSaveEnable(0) 'TODO: is this right lol

            Return True
        Else
            If (RUInt32(&H400080) = &HE91B11E2&) Then
                _DetectedDarkSoulsVersion = "Dark Souls (Invalid Beta)"
            Else
                _DetectedDarkSoulsVersion = "None"
            End If

            Return False
        End If
    End Function

    Public Shared Function InitHook() As Boolean
        If ScanForProcess("DARK SOULS", True) Then
            'Check if this process is even Dark Souls
            _IsHooked = CheckHook()

            If IsHooked Then
                AllocPointers()
                Return True
            End If
        End If

        Return False
    End Function

    Public Shared Sub UpdateHook()
        If Not Game.CheckHook() Then
            Return
        End If

        BonfirePointer = RUInt32(&H13784A0)
        CharPointer1 = RInt32(&H137DC70)
        CharPointer1 = RInt32(CharPointer1 + &H4)
        CharPointer1 = RInt32(CharPointer1)
        GameStatsPointer = RUInt32(&H1378700)
        CharPointer2 = RUInt32(GameStatsPointer + &H8)
        CharMapDataPointer = RInt32(CharPointer1 + &H28)
        CharPosDataPointer = RInt32(CharMapDataPointer + &H1C)
        PlayerStableFootPosPointer = RInt32(&H13784A0)
        MenuPointer = RInt32(&H13786D0)
        LinePointer = RInt32(&H1378388)
        KeyPointer = RInt32(&H1378640)
        EntityControllerPointer = RInt32(CharMapDataPointer + &H54)
        EntityAnimPointer = RInt32(CharMapDataPointer + &H14)
        EntityAnimInstancePointer = RInt32(EntityAnimPointer + &HC)
        EntityAnimInstancePointer = RInt32(EntityAnimInstancePointer + &H10)
        EntityAnimInstancePointer = RInt32(EntityAnimInstancePointer)


    End Sub

    Public Class GameStats

        Public Shared ClearCount As New LivePtrVarInt(Function() GameStatsPointer + &H3C)
        Public Shared TrueDeathCount As New LivePtrVarInt(Function() GameStatsPointer + &H58)
        'TODO: rename Hook.GameStats.TimerThing to the actual name...?
        Public Shared TotalPlayTime As New LivePtrVarInt(Function() GameStatsPointer + &H68)

    End Class

    Public Class Player

        Public Shared ModelName As New LivePtrVarUnicodeStr(Function() CharPointer1 + &H38)
        Public Shared NPCParamID As New LivePtrVarInt(Function() CharPointer1 + &H64)
        Public Shared NPCID As New LivePtrVarInt(Function() CharPointer1 + &H68)
        Public Shared NPCID2 As New LivePtrVarInt(Function() CharPointer1 + &H6C)
        Public Shared ChrType As New LivePtrVarInt(Function() CharPointer1 + &H70)
        Public Shared TeamType As New LivePtrVarInt(Function() CharPointer1 + &H74)
        Public Shared TargetLock As New LivePtrVarByte(Function() CharPointer1 + &H128)
        Public Shared DeathStructPointer As New LivePtrVarInt(Function() CharPointer1 + &H170)
        Public Class DeathStruct
            Public Shared IsDead As New LivePtrVarByte(Function() DeathStructPointer.Value + &H18)
        End Class
        Public Shared PoiseCurrent As New LivePtrVarFloat(Function() CharPointer1 + &H1C0)
        Public Shared PoiseMax As New LivePtrVarFloat(Function() CharPointer1 + &H1C4)
        Public Shared PoiseRecoverTimer As New LivePtrVarFloat(Function() CharPointer1 + &H1CC)
        Public Shared OptionsBitmask As New LivePtrVarInt(Function() CharPointer1 + &H1FC) 'TODODODODODO
        Public Shared OptionsBitmask2 As New LivePtrVarInt(Function() CharPointer1 + &H200) 'TODODODODODO
        Public Shared EventEntityID As New LivePtrVarInt(Function() CharPointer1 + &H208)
        Public Shared Opacity As New LivePtrVarFloat(Function() CharPointer1 + &H258)
        Public Shared DrawGroup1 As New LivePtrVarInt(Function() CharPointer1 + &H264)
        Public Shared DrawGroup2 As New LivePtrVarInt(Function() CharPointer1 + &H268)
        Public Shared DrawGroup3 As New LivePtrVarInt(Function() CharPointer1 + &H26C)
        Public Shared DrawGroup4 As New LivePtrVarInt(Function() CharPointer1 + &H270)
        Public Shared DispGroup1 As New LivePtrVarInt(Function() CharPointer1 + &H274)
        Public Shared DispGroup2 As New LivePtrVarInt(Function() CharPointer1 + &H278)
        Public Shared DispGroup3 As New LivePtrVarInt(Function() CharPointer1 + &H27C)
        Public Shared DispGroup4 As New LivePtrVarInt(Function() CharPointer1 + &H280)
        Public Shared MultiplayerZone As New LivePtrVarInt(Function() CharPointer1 + &H284)
        Public Shared Material_Floor As New LivePtrVarShort(Function() CharPointer1 + &H288)
        Public Shared Material_ArmorSE As New LivePtrVarShort(Function() CharPointer1 + &H28A)
        Public Shared Material_ArmorSFX As New LivePtrVarShort(Function() CharPointer1 + &H28C)
        Public Shared HP As New LivePtrVarInt(Function() CharPointer1 + &H2D4)
        Public Shared MaxHP As New LivePtrVarInt(Function() CharPointer1 + &H2D8)
        Public Shared Stamina As New LivePtrVarInt(Function() CharPointer1 + &H2E4)
        Public Shared MaxStamina As New LivePtrVarInt(Function() CharPointer1 + &H2E8)
        Public Shared ResistancePoisonCurrent As New LivePtrVarInt(Function() CharPointer1 + &H300)
        Public Shared ResistanceToxicCurrent As New LivePtrVarInt(Function() CharPointer1 + &H304)
        Public Shared ResistanceBleedCurrent As New LivePtrVarInt(Function() CharPointer1 + &H308)
        Public Shared ResistanceCurseCurrent As New LivePtrVarInt(Function() CharPointer1 + &H30C)
        Public Shared ResistancePoisonMax As New LivePtrVarInt(Function() CharPointer1 + &H310)
        Public Shared ResistanceToxicMax As New LivePtrVarInt(Function() CharPointer1 + &H314)
        Public Shared ResistanceBleedMax As New LivePtrVarInt(Function() CharPointer1 + &H318)
        Public Shared ResistanceCurseMax As New LivePtrVarInt(Function() CharPointer1 + &H31C)
        Public Shared Unknown1Ptr As New LivePtrVarInt(Function() CharPointer1 + &H330)
        Public Class Unknown1
            Public Shared NoHit As New LivePtrVarBool(Function() Unknown1Ptr.Value + &H10)
        End Class
        Public Shared TalkID As New LivePtrVarInt(Function() CharPointer1 + &H348)
        Public Shared DebugOptionsBitMask As New LivePtrVarInt(Function() CharPointer1 + &H3C4) 'TODODODODODODODODODODODO
        Public Shared StatsPtr As New LivePtrVarInt(Function() CharPointer1 + &H414)

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

        Public Shared AI_ID As New LivePtrVarInt(Function() CharPointer1 + &H41C)
        Public Shared DebugDrawWeaponDammyPolygon As New LivePtrVarBool(Function() CharPointer1 + &H6C7)
        Public Shared DebugDrawBloodMarkpos As New LivePtrVarBool(Function() CharPointer1 + &H6C8)

        Public Class Anim
            Public Shared SecondsPlaying As New LivePtrVarFloat(Function() EntityAnimInstancePointer + &H8)
            Public Shared CurrentSpeedMult As New LivePtrVarFloat(Function() EntityAnimInstancePointer + &H40)
            Public Shared TimesLooped As New LivePtrVarInt(Function() EntityAnimInstancePointer + &H44)

            Public Shared DebugOverallSpeedMult As New LivePtrVarFloat(Function() EntityAnimPointer + &H64)
            Public Shared DebugDrawSkeleton As New LivePtrVarBool(Function() EntityAnimPointer + &H68)
            Public Shared DebugDrawBoneName As New LivePtrVarBool(Function() EntityAnimPointer + &H69)
            Public Shared DebugDrawExtractMotion As New LivePtrVarBool(Function() EntityAnimPointer + &H81)
            Public Shared DebugAnimSlotLog As New LivePtrVarBool(Function() EntityAnimPointer + &H82)
        End Class

        Public Class Controller
            Public Shared MoveX As New LivePtrVarFloat(Function() EntityControllerPointer + &H10)
            Public Shared MoveY As New LivePtrVarFloat(Function() EntityControllerPointer + &H18)

            Public Shared CamRotSpeedH As New LivePtrVarFloat(Function() EntityControllerPointer + &H50)
            Public Shared CamRotSpeedV As New LivePtrVarFloat(Function() EntityControllerPointer + &H54)

            Public Shared CamRotH As New LivePtrVarFloat(Function() EntityControllerPointer + &H60)
            Public Shared CamRotV As New LivePtrVarFloat(Function() EntityControllerPointer + &H64)

            Public Shared R1Held_Sometimes As New LivePtrVarBool(Function() EntityControllerPointer + &H84)
            Public Shared L2OrR1Held_Sometimes As New LivePtrVarBool(Function() EntityControllerPointer + &H85)
            Public Shared RMouseHeld As New LivePtrVarBool(Function() EntityControllerPointer + &H89)
            Public Shared R1Held As New LivePtrVarBool(Function() EntityControllerPointer + &H8B)
            Public Shared XHeld As New LivePtrVarBool(Function() EntityControllerPointer + &H92)
            Public Shared L1Held As New LivePtrVarBool(Function() EntityControllerPointer + &H97)
            Public Shared L2OrTabHeld As New LivePtrVarBool(Function() EntityControllerPointer + &H98)
            Public Shared L1Held2 As New LivePtrVarBool(Function() EntityControllerPointer + &HB7)
            Public Shared R1HeldOrCircleTapped As New LivePtrVarBool(Function() EntityControllerPointer + &HB9)
            Public Shared RMouseOrR2Held As New LivePtrVarBool(Function() EntityControllerPointer + &HBE)
            Public Shared R1OrLMouseHeld As New LivePtrVarBool(Function() EntityControllerPointer + &HC0)
            Public Shared L2Held As New LivePtrVarBool(Function() EntityControllerPointer + &HC2)
            Public Shared CircleTapped As New LivePtrVarBool(Function() EntityControllerPointer + &HC2) 'TODODODODODODODO
            Public Shared XHeld2 As New LivePtrVarBool(Function() EntityControllerPointer + &HC7)
            Public Shared L1Held3 As New LivePtrVarBool(Function() EntityControllerPointer + &HCC)
            Public Shared L2Held2 As New LivePtrVarBool(Function() EntityControllerPointer + &HCD)
            Public Shared L1Held4 As New LivePtrVarBool(Function() EntityControllerPointer + &HEC)
            Public Shared SecondsR1Held As New LivePtrVarFloat(Function() EntityControllerPointer + &HF0)
            Public Shared SecondsGuarding As New LivePtrVarFloat(Function() EntityControllerPointer + &HF4)
            Public Shared SecondsR2Held As New LivePtrVarFloat(Function() EntityControllerPointer + &H104)
            Public Shared SecondsR1Held2 As New LivePtrVarFloat(Function() EntityControllerPointer + &H10C)
            Public Shared SecondsL1Held As New LivePtrVarFloat(Function() EntityControllerPointer + &H13C)
            Public Shared SecondsGuarding2 As New LivePtrVarFloat(Function() EntityControllerPointer + &H144)
        End Class

        Public Shared Heading As New LivePtrVarFloat(Function() CharPosDataPointer + &H4)
        Public Shared PosX As New LivePtrVarFloat(Function() CharPosDataPointer + &H10)
        Public Shared PosY As New LivePtrVarFloat(Function() CharPosDataPointer + &H14)
        Public Shared PosZ As New LivePtrVarFloat(Function() CharPosDataPointer + &H18)
        Public Shared StablePosX As New LivePtrVarFloat(Function() PlayerStableFootPosPointer + &HB70)
        Public Shared StablePosY As New LivePtrVarFloat(Function() PlayerStableFootPosPointer + &HB74)
        Public Shared StablePosZ As New LivePtrVarFloat(Function() PlayerStableFootPosPointer + &HB78)

        Public Shared BonfireID As New LivePtrVarInt(Function() BonfirePointer + &HB04)

    End Class

End Class
