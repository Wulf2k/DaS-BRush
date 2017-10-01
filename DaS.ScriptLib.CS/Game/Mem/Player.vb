Imports DaS.ScriptLib.Injection.Structures

Namespace Game.Mem

    Public Class Player

        Public Shared PosDataPtr As New LivePtrVar(Of Int32)(Function() Ptr.CharMapDataPtr.Value + &H1C)

        Public Shared Heading As New LivePtrVar(Of Single)(Function() PosDataPtr.Value + &H4)
        Public Shared PosX As New LivePtrVar(Of Single)(Function() PosDataPtr.Value + &H10)
        Public Shared PosY As New LivePtrVar(Of Single)(Function() PosDataPtr.Value + &H14)
        Public Shared PosZ As New LivePtrVar(Of Single)(Function() PosDataPtr.Value + &H18)

        Public Shared StablePosPtr As New LivePtrVar(Of Int32)(Function() &H13784A0)
        Public Shared StablePosX As New LivePtrVar(Of Single)(Function() StablePosPtr.Value + &HB70)
        Public Shared StablePosY As New LivePtrVar(Of Single)(Function() StablePosPtr.Value + &HB74)
        Public Shared StablePosZ As New LivePtrVar(Of Single)(Function() StablePosPtr.Value + &HB78)

        Public Shared BonfirePtr As New LivePtrVar(Of Int32)(Function() &H13784A0)
        Public Shared BonfireID As New LivePtrVar(Of Int32)(Function() BonfirePtr.Value + &HB04)

        'TODO: Add support for uni strings [ it wasn't working before I refactored everything either :^) ]
        'Public Shared ModelName As New LivePtrVar(Of UnicodeStr)(Function() Ptr.CharDataPtr1.Value + &H38)
        Public Shared NPCParamID As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H64)
        Public Shared NPCID As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H68)
        Public Shared NPCID2 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H6C)
        Public Shared ChrType As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H70)
        Public Shared TeamType As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H74)
        Public Shared TargetLock As New LivePtrVar(Of Byte)(Function() Ptr.CharDataPtr1.Value + &H128)
        Public Shared DeathStructPointer As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H170)

        Public Class DeathStruct
            Public Shared IsDead As New LivePtrVar(Of Byte)(Function() DeathStructPointer.Value + &H18)
        End Class

        Public Shared PoiseCurrent As New LivePtrVar(Of Single)(Function() Ptr.CharDataPtr1.Value + &H1C0)
        Public Shared PoiseMax As New LivePtrVar(Of Single)(Function() Ptr.CharDataPtr1.Value + &H1C4)
        Public Shared PoiseRecoverTimer As New LivePtrVar(Of Single)(Function() Ptr.CharDataPtr1.Value + &H1CC)
        Public Shared OptionsBitmask As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H1FC) 'TODODODODODO
        Public Shared OptionsBitmask2 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H200) 'TODODODODODO
        Public Shared EventEntityID As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H208)
        Public Shared Opacity As New LivePtrVar(Of Single)(Function() Ptr.CharDataPtr1.Value + &H258)
        Public Shared DrawGroup1 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H264)
        Public Shared DrawGroup2 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H268)
        Public Shared DrawGroup3 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H26C)
        Public Shared DrawGroup4 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H270)
        Public Shared DispGroup1 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H274)
        Public Shared DispGroup2 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H278)
        Public Shared DispGroup3 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H27C)
        Public Shared DispGroup4 As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H280)
        Public Shared MultiplayerZone As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H284)
        Public Shared Material_Floor As New LivePtrVar(Of Short)(Function() Ptr.CharDataPtr1.Value + &H288)
        Public Shared Material_ArmorSE As New LivePtrVar(Of Short)(Function() Ptr.CharDataPtr1.Value + &H28A)
        Public Shared Material_ArmorSFX As New LivePtrVar(Of Short)(Function() Ptr.CharDataPtr1.Value + &H28C)
        Public Shared HP As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H2D4)
        Public Shared MaxHP As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H2D8)
        Public Shared Stamina As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H2E4)
        Public Shared MaxStamina As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H2E8)
        Public Shared ResistancePoisonCurrent As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H300)
        Public Shared ResistanceToxicCurrent As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H304)
        Public Shared ResistanceBleedCurrent As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H308)
        Public Shared ResistanceCurseCurrent As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H30C)
        Public Shared ResistancePoisonMax As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H310)
        Public Shared ResistanceToxicMax As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H314)
        Public Shared ResistanceBleedMax As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H318)
        Public Shared ResistanceCurseMax As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H31C)
        Public Shared Unknown1Ptr As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H330)

        Public Class Unknown1
            Public Shared NoHit As New LivePtrVar(Of Boolean)(Function() Unknown1Ptr.Value + &H10)
        End Class

        Public Shared TalkID As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H348)
        Public Shared DebugOptionsBitMask As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H3C4) 'TODO: TODODODODODODODODODODO
        Public Shared StatsPtr As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H414)

        Public Class Stats
            Public Shared MaxHP As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H14)
            Public Shared MaxStamina As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H30)
            Public Shared VIT As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H38)
            Public Shared ATN As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H40)
            Public Shared ENDurance As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H48) ' fuck vb
            Public Shared STR As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H50)
            Public Shared DEX As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H58)
            Public Shared INT As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H60)
            Public Shared FTH As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H68)
            Public Shared RES As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H80)
            Public Shared Humanity As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &H7C)
            Public Shared ExternalGenitals As New LivePtrVar(Of Int32)(Function() StatsPtr.Value + &HC2)
        End Class

        Public Shared AI_ID As New LivePtrVar(Of Int32)(Function() Ptr.CharDataPtr1.Value + &H41C)
        Public Shared DebugDrawWeaponDammyPolygon As New LivePtrVar(Of Boolean)(Function() Ptr.CharDataPtr1.Value + &H6C7)
        Public Shared DebugDrawBloodMarkpos As New LivePtrVar(Of Boolean)(Function() Ptr.CharDataPtr1.Value + &H6C8)

        Public Class Anim
            Public Shared SecondsPlaying As New LivePtrVar(Of Single)(Function() Ptr.EntityAnimInstancePtr.Value + &H8)
            Public Shared CurrentSpeedMult As New LivePtrVar(Of Single)(Function() Ptr.EntityAnimInstancePtr.Value + &H40)
            Public Shared TimesLooped As New LivePtrVar(Of Int32)(Function() Ptr.EntityAnimInstancePtr.Value + &H44)

            Public Shared DebugOverallSpeedMult As New LivePtrVar(Of Single)(Function() Ptr.EntityAnimPtr.Value + &H64)
            Public Shared DebugDrawSkeleton As New LivePtrVar(Of Boolean)(Function() Ptr.EntityAnimPtr.Value + &H68)
            Public Shared DebugDrawBoneName As New LivePtrVar(Of Boolean)(Function() Ptr.EntityAnimPtr.Value + &H69)
            Public Shared DebugDrawExtractMotion As New LivePtrVar(Of Boolean)(Function() Ptr.EntityAnimPtr.Value + &H81)
            Public Shared DebugAnimSlotLog As New LivePtrVar(Of Boolean)(Function() Ptr.EntityAnimPtr.Value + &H82)
        End Class

        Public Class Controller
            Public Shared MoveX As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H10)
            Public Shared MoveY As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H18)

            Public Shared CamRotSpeedH As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H50)
            Public Shared CamRotSpeedV As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H54)

            Public Shared CamRotH As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H60)
            Public Shared CamRotV As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H64)

            Public Shared R1Held_Sometimes As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H84)
            Public Shared L2OrR1Held_Sometimes As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H85)
            Public Shared RMouseHeld As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H89)
            Public Shared R1Held As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H8B)
            Public Shared XHeld As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H92)
            Public Shared L1Held As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H97)
            Public Shared L2OrTabHeld As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &H98)
            Public Shared L1Held2 As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HB7)
            Public Shared R1HeldOrCircleTapped As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HB9)
            Public Shared RMouseOrR2Held As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HBE)
            Public Shared R1OrLMouseHeld As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HC0)
            Public Shared L2Held As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HC2)
            Public Shared CircleTapped As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HC2) 'TODODODODODODODO
            Public Shared XHeld2 As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HC7)
            Public Shared L1Held3 As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HCC)
            Public Shared L2Held2 As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HCD)
            Public Shared L1Held4 As New LivePtrVar(Of Boolean)(Function() Ptr.EntityControllerPtr.Value + &HEC)
            Public Shared SecondsR1Held As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &HF0)
            Public Shared SecondsGuarding As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &HF4)
            Public Shared SecondsR2Held As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H104)
            Public Shared SecondsR1Held2 As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H10C)
            Public Shared SecondsL1Held As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H13C)
            Public Shared SecondsGuarding2 As New LivePtrVar(Of Single)(Function() Ptr.EntityControllerPtr.Value + &H144)
        End Class

    End Class

End Namespace
