Imports System.Runtime.InteropServices
Imports DaS.ScriptLib.Injection.Structures
Imports DaS.ScriptLib.Lua
Imports DaS.ScriptLib.Injection

Namespace Game.Data.Structures

    <Flags>
    Public Enum EntityFlagsA
        SetDeadMode = &H2000000
        DisableDamage = &H4000000
        EnableInvincible = &H8000000
        FirstPersonView = &H100000
        SetDrawEnable = &H800000
        SetSuperArmor = &H10000
        SetDisableGravity = &H4000
    End Enum

    <Flags>
    Public Enum EntityFlagsB
        DisableHpGauge = &H8
    End Enum

    <Flags>
    Public Enum EntityDebugFlags
        NoGoodsConsume = &H1000000
        DisableDrawingA = &H100000
        DrawCounter = &H200000
        DisableDrawingB = &H80000
        DrawDirection = &H4000
        NoUpdate = &H8000
        NoAttack = &H100
        NoMove = &H200
        NoStamConsume = &H400
        NoMPConsume = &H800
        NoDead = &H20
        NoDamage = &H40
        NoHit = &H80
        DrawHit = &H4
    End Enum

    <Flags>
    Public Enum EntityMapFlagsA As Byte
        None = 0
        EnableLogic = &H1
    End Enum

    <Flags>
    Public Enum EntityMapFlagsB As Byte
        None = 0
        DisableCollision = &H40
    End Enum


    Public Enum Covenant
        WayOfWhite = 1
        PrincessGuard = 2
        WarriorOfSunlight = 3
        Darkwraith = 4
        PathOfTheDragon = 5
        GravelordServant = 6
        ForestHunter = 7
        DarkmoonBlade = 8
        ChaosServant = 9
    End Enum

    <StructLayout(LayoutKind.Explicit)>
    Public Class Entity

        Public Const MAX_STATNAME_LENGTH = 14

        Public ReadOnly Property Pointer As Integer
            Get
                Return _getOffset()
            End Get
        End Property

        Private _getOffset As Func(Of Integer)

        Public Sub New(getOffsetFunc As Func(Of Integer))
            _getOffset = getOffsetFunc
        End Sub

        Public Sub New(constantOffsetValue As Integer)
            Dim offsetValueCopy As Integer = constantOffsetValue

            _getOffset = Function() offsetValueCopy
        End Sub

        Public Property HeaderPtr As Integer
            Get
                Return RInt32(Pointer + &HC)
            End Get
            Set(value As Integer)
                WInt32(Pointer + &HC, value)
            End Set
        End Property

        Public Property Header As EntityHeader
            Get
                Return New EntityHeader(HeaderPtr)
            End Get
            Set(value As EntityHeader)
                Header.CopyFrom(value)
            End Set
        End Property

        Public Property DisableEventBackreadState As Boolean
            Get
                Return RBool(Pointer + &H14)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H14, value)
            End Set
        End Property



        Public Property DebugControllerPtr As Integer
            Get
                Return RInt32(CharMapDataPtr + &H244)
            End Get
            Set(value As Integer)
                WInt32(CharMapDataPtr + &H244, value)
            End Set
        End Property

        Public Property DebugController As EntityController
            Get
                Return New EntityController(Function() DebugControllerPtr)
            End Get
            Set(value As EntityController)
                DebugControllerPtr = value.Pointer
            End Set
        End Property

        Public Shared Function GetPlayer() As Entity
            Return FromID(10000)
        End Function

        Public Shared Function FromID(id As Integer) As Entity
            Return New Entity(Function() Funcs.GetEntityPtr(id))
        End Function

        Public Shared Function FromName(mapName As String, entityName As String) As Entity
            Return New Entity(Function() Funcs.GetEntityPtrByName(mapName, entityName))
        End Function

        Public Property ModelName As String
            Get
                Return RUnicodeStr(Pointer + &H38, 10)
            End Get
            Set(value As String)
                WUnicodeStr(Pointer + &H38, value.Substring(0, Math.Min(value.Length, 10)))
            End Set
        End Property

        Public Property NPCID As Int32
            Get
                Return RInt32(Pointer + &H68)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H68, value)
            End Set
        End Property

        Public Property NPCID2 As Int32
            Get
                Return RInt32(Pointer + &H6C)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H6C, value)
            End Set
        End Property

        Public Property ChrType As Int32
            Get
                Return RInt32(Pointer + &H70)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H70, value)
            End Set
        End Property

        Public Property TeamType As Int32
            Get
                Return RInt32(Pointer + &H74)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H74, value)
            End Set
        End Property

        Public Property IsTargetLocked As Boolean
            Get
                Return RBool(Pointer + &H128)
            End Get
            Set(value As Boolean)
                WBool(Pointer + &H128, value)
            End Set
        End Property

        Public ReadOnly Property DeathStructPointer As Int32
            Get
                Return RInt32(Pointer + &H170)
            End Get
        End Property

        Public ReadOnly Property IsDead As Boolean
            Get
                Return RBool(DeathStructPointer + &H18)
            End Get
            'Writing value crashed game...
            'Set(value As Boolean)
            '    WBool(DeathStructPointer + &H18, value)
            'End Set
        End Property

        Public Property PoiseCurrent As Single
            Get
                Return RFloat(Pointer + &H1C0)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1C0, value)
            End Set
        End Property

        Public Property PoiseMax As Single
            Get
                Return RFloat(Pointer + &H1C4)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1C4, value)
            End Set
        End Property

        Public Property PoiseRecoverTimer As Single
            Get
                Return RFloat(Pointer + &H1CC)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H1CC, value)
            End Set
        End Property

        Public Property FlagsA As EntityFlagsA
            Get
                Return CType(RInt32(Pointer + &H1FC), EntityFlagsA)
            End Get
            Set(value As EntityFlagsA)
                WInt32(Pointer + &H1FC, CType(value, Integer))
            End Set
        End Property

        Public Property FlagsB As EntityFlagsB
            Get
                Return CType(RInt32(Pointer + &H200), EntityFlagsB)
            End Get
            Set(value As EntityFlagsB)
                WInt32(Pointer + &H200, CType(value, Integer))
            End Set
        End Property

        Public Property EventEntityID As Int32
            Get
                Return RInt32(Pointer + &H208)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H208, value)
            End Set
        End Property

        Public Property Opacity As Single
            Get
                Return RFloat(Pointer + &H258)
            End Get
            Set(value As Single)
                WFloat(Pointer + &H258, value)
            End Set
        End Property

        Public Property DrawGroup1 As Int32
            Get
                Return RInt32(Pointer + &H264)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H264, value)
            End Set
        End Property

        Public Property DrawGroup2 As Int32
            Get
                Return RInt32(Pointer + &H268)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H268, value)
            End Set
        End Property

        Public Property DrawGroup3 As Int32
            Get
                Return RInt32(Pointer + &H26C)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H26C, value)
            End Set
        End Property

        Public Property DrawGroup4 As Int32
            Get
                Return RInt32(Pointer + &H270)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H270, value)
            End Set
        End Property

        Public Property DispGroup1 As Int32
            Get
                Return RInt32(Pointer + &H274)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H274, value)
            End Set
        End Property

        Public Property DispGroup2 As Int32
            Get
                Return RInt32(Pointer + &H278)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H278, value)
            End Set
        End Property

        Public Property DispGroup3 As Int32
            Get
                Return RInt32(Pointer + &H27C)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H27C, value)
            End Set
        End Property

        Public Property DispGroup4 As Int32
            Get
                Return RInt32(Pointer + &H280)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H280, value)
            End Set
        End Property

        Public Property MultiplayerZone As Int32
            Get
                Return RInt32(Pointer + &H284)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H284, value)
            End Set
        End Property

        Public Property Material_Floor As Short
            Get
                Return RInt16(Pointer + &H288)
            End Get
            Set(value As Short)
                WInt16(Pointer + &H288, value)
            End Set
        End Property

        Public Property Material_ArmorSE As Short
            Get
                Return RInt16(Pointer + &H28A)
            End Get
            Set(value As Short)
                WInt16(Pointer + &H28A, value)
            End Set
        End Property

        Public Property Material_ArmorSFX As Short
            Get
                Return RInt16(Pointer + &H28C)
            End Get
            Set(value As Short)
                WInt16(Pointer + &H28C, value)
            End Set
        End Property

        Public Property HP As Int32
            Get
                Return RInt32(Pointer + &H2D4)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H2D4, value)
            End Set
        End Property

        Public Property MaxHP As Int32
            Get
                Return RInt32(Pointer + &H2D8)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H2D8, value)
            End Set
        End Property

        Public Property Stamina As Int32
            Get
                Return RInt32(Pointer + &H2E4)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H2E4, value)
            End Set
        End Property

        Public Property MaxStamina As Int32
            Get
                Return RInt32(Pointer + &H2E8)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H2E8, value)
            End Set
        End Property

        Public Property ResistancePoisonCurrent As Int32
            Get
                Return RInt32(Pointer + &H300)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H300, value)
            End Set
        End Property

        Public Property ResistanceToxicCurrent As Int32
            Get
                Return RInt32(Pointer + &H304)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H304, value)
            End Set
        End Property

        Public Property ResistanceBleedCurrent As Int32
            Get
                Return RInt32(Pointer + &H308)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H308, value)
            End Set
        End Property

        Public Property ResistanceCurseCurrent As Int32
            Get
                Return RInt32(Pointer + &H30C)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H30C, value)
            End Set
        End Property

        Public Property ResistancePoisonMax As Int32
            Get
                Return RInt32(Pointer + &H310)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H310, value)
            End Set
        End Property

        Public Property ResistanceToxicMax As Int32
            Get
                Return RInt32(Pointer + &H314)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H314, value)
            End Set
        End Property

        Public Property ResistanceBleedMax As Int32
            Get
                Return RInt32(Pointer + &H318)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H318, value)
            End Set
        End Property

        Public Property ResistanceCurseMax As Int32
            Get
                Return RInt32(Pointer + &H31C)
            End Get
            Set(value As Int32)
                WInt32(Pointer + &H31C, value)
            End Set
        End Property

        Public ReadOnly Property Unknown1Ptr As Int32
            Get
                Return RInt32(Pointer + &H330)
            End Get
        End Property


        Public ReadOnly Property CharMapDataPtr As Integer
            Get
                Return RInt32(Pointer + &H28)
            End Get
        End Property

        Public Property MapFlagsA As EntityMapFlagsA
            Get
                Return CType(RByte(CharMapDataPtr + &HC0), EntityMapFlagsB)
            End Get
            Set(value As EntityMapFlagsA)
                WByte(CharMapDataPtr + &HC0, CType(value, Byte))
            End Set
        End Property

        Public Property MapFlagsB As EntityMapFlagsB
            Get
                Return CType(RByte(CharMapDataPtr + &HC6), EntityMapFlagsB)
            End Get
            Set(value As EntityMapFlagsB)
                WByte(CharMapDataPtr + &HC6, CType(value, Byte))
            End Set
        End Property

        Public Property ControllerPtr As Integer
            Get
                Return RInt32(CharMapDataPtr + &H54)
            End Get
            Set(value As Integer)
                WInt32(CharMapDataPtr + &H54, value)
            End Set
        End Property

        Public Property Controller As EntityController
            Get
                Return New EntityController(Function() ControllerPtr)
            End Get
            Set(value As EntityController)
                ControllerPtr = value.Pointer
            End Set
        End Property

        'TODO: CONTROLLER
        'Formatlike "ControllerMoveX", "ControllerR1Held", etc

        Public ReadOnly Property AnimationPtr As Integer
            Get
                Return RInt32(CharMapDataPtr + &H14)
            End Get
        End Property

        Public ReadOnly Property AnimationInstancePtr As Integer
            Get
                Return RInt32(RInt32(RInt32(AnimationPtr + &HC) + &H10))
            End Get
        End Property

        Public Property AnimInstanceTime As Single
            Get
                Return RFloat(AnimationInstancePtr + &H8)
            End Get
            Set(value As Single)
                WFloat(AnimationInstancePtr + &H8, value)
            End Set
        End Property

        Public Property AnimInstanceSpeed As Single
            Get
                Return RFloat(AnimationInstancePtr + &H40)
            End Get
            Set(value As Single)
                WFloat(AnimationInstancePtr + &H40, value)
            End Set
        End Property

        Public Property AnimInstanceLoopCount As Single
            Get
                Return RFloat(AnimationInstancePtr + &H44)
            End Get
            Set(value As Single)
                WFloat(AnimationInstancePtr + &H44, value)
            End Set
        End Property

        Public Property AnimationSpeed As Single
            Get
                Return RFloat(AnimationPtr + &H64)
            End Get
            Set(value As Single)
                WFloat(AnimationPtr + &H64, value)
            End Set
        End Property

        Public Property AnimDbgDrawSkeleton As Boolean
            Get
                Return RBool(AnimationPtr + &H68)
            End Get
            Set(value As Boolean)
                WBool(AnimationPtr + &H68, value)
            End Set
        End Property

        Public Property AnimDbgDrawBoneName As Boolean
            Get
                Return RBool(AnimationPtr + &H69)
            End Get
            Set(value As Boolean)
                WBool(AnimationPtr + &H69, value)
            End Set
        End Property

        Public Property AnimDbgDrawExtractMotion As Boolean
            Get
                Return RBool(AnimationPtr + &H81)
            End Get
            Set(value As Boolean)
                WBool(AnimationPtr + &H81, value)
            End Set
        End Property

        Public Property AnimDbgSlotLog As Boolean
            Get
                Return RBool(AnimationPtr + &H82)
            End Get
            Set(value As Boolean)
                WBool(AnimationPtr + &H82, value)
            End Set
        End Property

        Public Property LocationPtr As Integer
            Get
                Return RInt32(CharMapDataPtr + &H1C)
            End Get
            Set(value As Integer)
                WInt32(CharMapDataPtr + &H1C, value)
            End Set
        End Property

        Public Property Location As EntityLocation
            Get
                Return New EntityLocation(LocationPtr)
            End Get
            Set(value As EntityLocation)
                Dim fuckVb = New EntityLocation(LocationPtr)
                fuckVb.CopyFrom(value)
            End Set
        End Property

#Region "STATS"

        Public ReadOnly Property StatsPtr As Integer
            Get
                Return RInt32(Pointer + &H414)
            End Get
        End Property

        Public Property StatHP As Integer
            Get
                Return RInt32(StatsPtr + &HC)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HC, value)
            End Set
        End Property

        Public Property StatMaxHPBase As Integer
            Get
                Return RInt32(StatsPtr + &H10)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H10, value)
            End Set
        End Property

        Public Property StatMaxHP As Integer
            Get
                Return RInt32(StatsPtr + &H14)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H14, value)
            End Set
        End Property

        Public Property StatMP As Integer
            Get
                Return RInt32(StatsPtr + &H18)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H18, value)
            End Set
        End Property

        Public Property StatMaxMPBase As Integer
            Get
                Return RInt32(StatsPtr + &H1C)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H1C, value)
            End Set
        End Property

        Public Property StatMaxMP As Integer
            Get
                Return RInt32(StatsPtr + &H20)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H20, value)
            End Set
        End Property

        Public Property StatStamina As Integer
            Get
                Return RInt32(StatsPtr + &H24)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H24, value)
            End Set
        End Property

        Public Property StatMaxStaminaBase As Integer
            Get
                Return RInt32(StatsPtr + &H28)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H28, value)
            End Set
        End Property

        Public Property StatMaxStamina As Integer
            Get
                Return RInt32(StatsPtr + &H30)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H30, value)
            End Set
        End Property

        Public Property StatVIT As Integer
            Get
                Return RInt32(StatsPtr + &H38)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H38, value)
            End Set
        End Property

        Public Property StatATN As Integer
            Get
                Return RInt32(StatsPtr + &H40)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H40, value)
            End Set
        End Property

        Public Property StatEND As Integer
            Get
                Return RInt32(StatsPtr + &H48)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H48, value)
            End Set
        End Property

        Public Property StatSTR As Integer
            Get
                Return RInt32(StatsPtr + &H50)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H50, value)
            End Set
        End Property

        Public Property StatDEX As Integer
            Get
                Return RInt32(StatsPtr + &H58)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H58, value)
            End Set
        End Property

        Public Property StatINT As Integer
            Get
                Return RInt32(StatsPtr + &H60)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H60, value)
            End Set
        End Property

        Public Property StatFTH As Integer
            Get
                Return RInt32(StatsPtr + &H68)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H68, value)
            End Set
        End Property

        Public Property StatRES As Integer
            Get
                Return RInt32(StatsPtr + &H80)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H80, value)
            End Set
        End Property

        Public Property StatHumanity As Integer
            Get
                Return RInt32(StatsPtr + &H7C)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H7C, value)
            End Set
        End Property

        Public Property StatGender As Short 'oh no i did the thing REEEEEEEEEEEEEEEE
            Get
                Return RInt16(StatsPtr + &HC2)
            End Get
            Set(value As Short)
                WInt16(StatsPtr + &HC2, value)
            End Set
        End Property

        Public Property StatDebugShopLevel As Short
            Get
                Return RInt16(StatsPtr + &HC4)
            End Get
            Set(value As Short)
                WInt16(StatsPtr + &HC4, value)
            End Set
        End Property

        Public Property StatStartingClass As Byte
            Get
                Return RByte(StatsPtr + &HC6)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HC6, value)
            End Set
        End Property

        Public Property StatPhysique As Byte
            Get
                Return RByte(StatsPtr + &HC7)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HC7, value)
            End Set
        End Property

        Public Property StatStartingGift As Byte
            Get
                Return RByte(StatsPtr + &HC7)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HC7, value)
            End Set
        End Property

        Public Property StatMultiplayCount As Integer
            Get
                Return RInt32(StatsPtr + &HCC)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HCC, value)
            End Set
        End Property

        Public Property StatCoOpSuccessCount As Integer
            Get
                Return RInt32(StatsPtr + &HD0)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HD0, value)
            End Set
        End Property

        Public Property StatThiefInvadePlaySuccessCount As Integer
            Get
                Return RInt32(StatsPtr + &HD4)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HD4, value)
            End Set
        End Property

        Public Property StatPlayerRankS As Integer
            Get
                Return RInt32(StatsPtr + &HD8)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HD8, value)
            End Set
        End Property

        Public Property StatPlayerRankA As Integer
            Get
                Return RInt32(StatsPtr + &HDC)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HDC, value)
            End Set
        End Property

        Public Property StatPlayerRankB As Integer
            Get
                Return RInt32(StatsPtr + &HE0)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HE0, value)
            End Set
        End Property

        Public Property StatPlayerRankC As Integer
            Get
                Return RInt32(StatsPtr + &HE4)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HE4, value)
            End Set
        End Property

        Public Property StatDevotionWarriorOfSunlight As Byte
            Get
                Return RByte(StatsPtr + &HE5)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HE5, value)
            End Set
        End Property

        Public Property StatDevotionDarkwraith As Byte
            Get
                Return RByte(StatsPtr + &HE6)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HE6, value)
            End Set
        End Property

        Public Property StatDevotionDragon As Byte
            Get
                Return RByte(StatsPtr + &HE7)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HE7, value)
            End Set
        End Property

        Public Property StatDevotionGravelord As Byte
            Get
                Return RByte(StatsPtr + &HE8)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HE8, value)
            End Set
        End Property

        Public Property StatDevotionForest As Byte
            Get
                Return RByte(StatsPtr + &HE9)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HE9, value)
            End Set
        End Property

        Public Property StatDevotionDarkmoon As Byte
            Get
                Return RByte(StatsPtr + &HEA)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HEA, value)
            End Set
        End Property

        Public Property StatDevotionChaos As Byte
            Get
                Return RByte(StatsPtr + &HEB)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &HEB, value)
            End Set
        End Property

        Public Property StatIndictments As Integer
            Get
                Return RInt32(StatsPtr + &HEC)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HEC, value)
            End Set
        End Property

        Public Property StatDebugBlockClearBonus As Single
            Get
                Return RFloat(StatsPtr + &HF0)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &HF0, value)
            End Set
        End Property

        Public Property StatEggSouls As Integer
            Get
                Return RInt32(StatsPtr + &HF4)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HF4, value)
            End Set
        End Property

        Public Property StatPoisonResist As Integer
            Get
                Return RInt32(StatsPtr + &HF8)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HF8, value)
            End Set
        End Property

        Public Property StatBleedResist As Integer
            Get
                Return RInt32(StatsPtr + &HFC)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &HFC, value)
            End Set
        End Property

        Public Property StatToxicResist As Integer
            Get
                Return RInt32(StatsPtr + &H100)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H100, value)
            End Set
        End Property

        Public Property StatCurseResist As Integer
            Get
                Return RInt32(StatsPtr + &H104)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H104, value)
            End Set
        End Property

        Public Property StatDebugClearItem As Byte
            Get
                Return RByte(StatsPtr + &H108)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H108, value)
            End Set
        End Property

        Public Property StatDebugResvSoulSteam As Byte
            Get
                Return RByte(StatsPtr + &H109)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H109, value)
            End Set
        End Property

        Public Property StatDebugResvSoulPenalty As Byte
            Get
                Return RByte(StatsPtr + &H10A)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H10A, value)
            End Set
        End Property

        Public Property StatCovenant As Covenant
            Get
                Return CType(RByte(StatsPtr + &H10B), Covenant)
            End Get
            Set(value As Covenant)
                WInt32(StatsPtr + &H10B, CType(value, Byte))
            End Set
        End Property

        Public Property StatAppearanceFaceType As Byte
            Get
                Return RByte(StatsPtr + &H10C)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H10C, value)
            End Set
        End Property

        Public Property StatAppearanceHairType As Byte
            Get
                Return RByte(StatsPtr + &H10D)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H10D, value)
            End Set
        End Property

        Public Property StatAppearanceHairAndEyesColor As Byte
            Get
                Return RByte(StatsPtr + &H10E)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H10E, value)
            End Set
        End Property

        Public Property StatCurseLevel As Byte
            Get
                Return RByte(StatsPtr + &H10F)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H10F, value)
            End Set
        End Property

        Public Property StatInvadeType As Byte
            Get
                Return RByte(StatsPtr + &H110)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H110, value)
            End Set
        End Property

        'TODO: CHECK FOR OTHER EQUIP SLOTS

        Public Property StatEquipRightHand2 As Integer
            Get
                Return RInt32(StatsPtr + &H258)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H258, value)
            End Set
        End Property

        Public Property StatEquipHelmet As Integer
            Get
                Return RInt32(StatsPtr + &H26C)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H26C, value)
            End Set
        End Property

        Public Property StatAppearanceScaleHead As Single
            Get
                Return RFloat(StatsPtr + &H2AC)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H2AC, value)
            End Set
        End Property

        Public Property StatAppearanceScaleChest As Single
            Get
                Return RFloat(StatsPtr + &H2B0)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H2B0, value)
            End Set
        End Property

        Public Property StatAppearanceScaleWaist As Single
            Get
                Return RFloat(StatsPtr + &H2B4)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H2B4, value)
            End Set
        End Property

        Public Property StatAppearanceScaleArms As Single
            Get
                Return RFloat(StatsPtr + &H2B8)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H2B8, value)
            End Set
        End Property

        Public Property StatAppearanceScaleLegs As Single
            Get
                Return RFloat(StatsPtr + &H2BC)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H2BC, value)
            End Set
        End Property

        Public Property StatAppearanceHairColorR As Single
            Get
                Return RFloat(StatsPtr + &H380)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H380, value)
            End Set
        End Property

        Public Property StatAppearanceHairColorG As Single
            Get
                Return RFloat(StatsPtr + &H384)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H384, value)
            End Set
        End Property

        Public Property StatAppearanceHairColorB As Single
            Get
                Return RFloat(StatsPtr + &H388)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H388, value)
            End Set
        End Property

        Public Property StatAppearanceHairColorA As Single
            Get
                Return RFloat(StatsPtr + &H38C)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H38C, value)
            End Set
        End Property

        Public Property StatAppearanceEyeColorR As Single
            Get
                Return RFloat(StatsPtr + &H390)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H390, value)
            End Set
        End Property

        Public Property StatAppearanceEyeColorG As Single
            Get
                Return RFloat(StatsPtr + &H394)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H394, value)
            End Set
        End Property

        Public Property StatAppearanceEyeColorB As Single
            Get
                Return RFloat(StatsPtr + &H398)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H398, value)
            End Set
        End Property

        Public Property StatAppearanceEyeColorA As Single
            Get
                Return RFloat(StatsPtr + &H39C)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H39C, value)
            End Set
        End Property

        Public Property StatAppearanceFaceData(index As Integer) As Byte
            Get
                Return RByte(StatsPtr + &H3A0 + index)
            End Get
            Set(value As Byte)
                WByte(StatsPtr + &H3A0 + index, value)
            End Set
        End Property

        'TODO: CHECK FOR OTHER DEFENSES

        Public Property StatMagicDefense As Integer
            Get
                Return RInt32(StatsPtr + &H43C)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H43C, value)
            End Set
        End Property

        'TODO: IS THIS THE DEMONS SOULS ITEM BURDEN OR WHAT WULF

        Public Property StatMaxItemBurden As Single
            Get
                Return RFloat(StatsPtr + &H44C)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H44C, value)
            End Set
        End Property

        'TODO: CONFIRM IF THESE ARE THE BUILDUPS AND NOT THE STAT SCREEN MAXIMUMS

        Public Property StatPoisonBuildup As Single
            Get
                Return RFloat(StatsPtr + &H49C)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H49C, value)
            End Set
        End Property

        Public Property StatToxicBuildup As Single
            Get
                Return RFloat(StatsPtr + &H4A0)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H4A0, value)
            End Set
        End Property

        Public Property StatBleedBuildup As Single
            Get
                Return RFloat(StatsPtr + &H4A4)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H4A4, value)
            End Set
        End Property

        Public Property StatCurseBuildup As Single
            Get
                Return RFloat(StatsPtr + &H4A8)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H4A8, value)
            End Set
        End Property

        Public Property StatPoise As Single
            Get
                Return RFloat(StatsPtr + &H4AC)
            End Get
            Set(value As Single)
                WFloat(StatsPtr + &H4AC, value)
            End Set
        End Property

        Public Property StatSoulLevel As Integer
            Get
                Return RInt32(StatsPtr + &H88)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H88, value)
            End Set
        End Property

        Public Property StatSouls As Integer
            Get
                Return RInt32(StatsPtr + &H8C)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H8C, value)
            End Set
        End Property

        Public Property StatPointTotal As Integer
            Get
                Return RInt32(StatsPtr + &H98)
            End Get
            Set(value As Integer)
                WInt32(StatsPtr + &H98, value)
            End Set
        End Property

        Public Property StatName As String
            Get
                Return RUnicodeStr(Pointer + &HA0, MAX_STATNAME_LENGTH)
            End Get
            Set(value As String)
                WAsciiStr(Pointer + &HA0, value.Substring(0, Math.Min(value.Length, MAX_STATNAME_LENGTH)))
            End Set
        End Property

#End Region


#Region "Static"
        Public Shared ReadOnly Property PlayerStablePosPtr As Integer = &H13784A0

        Public Shared Property PlayerStablePosX As Single
            Get
                Return RFloat(PlayerStablePosPtr + &HB70)
            End Get
            Set(value As Single)
                WFloat(PlayerStablePosPtr + &HB70, value)
            End Set
        End Property

        Public Shared Property PlayerStablePosY As Single
            Get
                Return RFloat(PlayerStablePosPtr + &HB74)
            End Get
            Set(value As Single)
                WFloat(PlayerStablePosPtr + &HB74, value)
            End Set
        End Property

        Public Shared Property PlayerStablePosZ As Single
            Get
                Return RFloat(PlayerStablePosPtr + &HB78)
            End Get
            Set(value As Single)
                WFloat(PlayerStablePosPtr + &HB78, value)
            End Set
        End Property
#End Region

        Public Property DebugFlags As EntityDebugFlags
            Get
                Return CType(RInt32(Pointer + &H3C4), EntityDebugFlags)
            End Get
            Set(value As EntityDebugFlags)
                WInt32(Pointer + &H3C4, CType(value, Integer))
            End Set
        End Property

        Public Function GetDebugFlag(flg As EntityDebugFlags) As Boolean
            Return DebugFlags.HasFlag(flg)
        End Function

        Public Sub SetDebugFlag(flg As EntityDebugFlags, state As Boolean)
            If state Then
                DebugFlags = DebugFlags Or flg
            Else
                DebugFlags = DebugFlags And (Not flg)
            End If
        End Sub

        Public Function GetFlagA(flg As EntityFlagsA) As Boolean
            Return FlagsA.HasFlag(flg)
        End Function

        Public Sub SetFlagA(flg As EntityFlagsA, state As Boolean)
            If state Then
                FlagsA = FlagsA Or flg
            Else
                FlagsA = FlagsA And (Not flg)
            End If
        End Sub

#Region "DebugFlags"
        Public Property NoGoodsConsume As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoGoodsConsume)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoGoodsConsume, value)
            End Set
        End Property

        Public Property DisableDrawingA As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.DisableDrawingA)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.DisableDrawingA, value)
            End Set
        End Property

        Public Property DrawCounter As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.DrawCounter)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.DrawCounter, value)
            End Set
        End Property

        Public Property DisableDrawingB As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.DisableDrawingB)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.DisableDrawingB, value)
            End Set
        End Property

        Public Property DrawDirection As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.DrawDirection)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.DrawDirection, value)
            End Set
        End Property

        Public Property NoUpdate As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoUpdate)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoUpdate, value)
            End Set
        End Property

        Public Property NoAttack As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoAttack)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoAttack, value)
            End Set
        End Property

        Public Property NoMove As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoMove)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoMove, value)
            End Set
        End Property

        Public Property NoStamConsume As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoStamConsume)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoStamConsume, value)
            End Set
        End Property

        Public Property NoMPConsume As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoMPConsume)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoMPConsume, value)
            End Set
        End Property

        Public Property NoDead As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoDead)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoDead, value)
            End Set
        End Property

        Public Property NoDamage As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.NoDamage)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.NoDamage, value)
            End Set
        End Property

        Public Property DrawHit As Boolean
            Get
                Return GetDebugFlag(EntityDebugFlags.DrawHit)
            End Get
            Set(value As Boolean)
                SetDebugFlag(EntityDebugFlags.DrawHit, value)
            End Set
        End Property
#End Region

#Region "FlagsA"
        Public Property DisableDamage As Boolean
            Get
                Return GetFlagA(EntityFlagsA.DisableDamage)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.DisableDamage, value)
            End Set
        End Property

        Public Property EnableInvincible As Boolean
            Get
                Return GetFlagA(EntityFlagsA.EnableInvincible)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.EnableInvincible, value)
            End Set
        End Property

        Public Property FirstPersonView As Boolean
            Get
                Return GetFlagA(EntityFlagsA.FirstPersonView)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.FirstPersonView, value)
            End Set
        End Property

        Public Property SetDeadMode As Boolean
            Get
                Return GetFlagA(EntityFlagsA.SetDeadMode)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.SetDeadMode, value)
            End Set
        End Property

        Public Property SetDisableGravity As Boolean
            Get
                Return GetFlagA(EntityFlagsA.SetDisableGravity)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.SetDisableGravity, value)
            End Set
        End Property

        Public Property SetDrawEnable As Boolean
            Get
                Return GetFlagA(EntityFlagsA.SetDrawEnable)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.SetDrawEnable, value)
            End Set
        End Property

        Public Property SetSuperArmor As Boolean
            Get
                Return GetFlagA(EntityFlagsA.SetSuperArmor)
            End Get
            Set(value As Boolean)
                SetFlagA(EntityFlagsA.SetSuperArmor, value)
            End Set
        End Property
#End Region

        Public Function GetMapFlagA(flg As EntityMapFlagsA) As Boolean
            Return MapFlagsA.HasFlag(flg)
        End Function

        Public Sub SetMapFlagA(flg As EntityMapFlagsA, state As Boolean)
            If state Then
                MapFlagsA = MapFlagsA Or flg
            Else
                MapFlagsA = MapFlagsA And (Not flg)
            End If
        End Sub

        Public Function GetMapFlagB(flg As EntityMapFlagsB) As Boolean
            Return MapFlagsB.HasFlag(flg)
        End Function

        Public Sub SetMapFlagB(flg As EntityMapFlagsB, state As Boolean)
            If state Then
                MapFlagsB = MapFlagsB Or flg
            Else
                MapFlagsB = MapFlagsB And (Not flg)
            End If
        End Sub

#Region "MapFlagsA and MapFlagsB"

        Public Property EnableLogic As Boolean
            Get
                Return GetMapFlagA(EntityMapFlagsA.EnableLogic)
            End Get
            Set(value As Boolean)
                SetMapFlagA(EntityMapFlagsA.EnableLogic, value)
            End Set
        End Property

        Public Property DisableCollision As Boolean
            Get
                Return GetMapFlagB(EntityMapFlagsB.DisableCollision)
            End Get
            Set(value As Boolean)
                SetMapFlagB(EntityMapFlagsB.DisableCollision, value)
            End Set
        End Property

#End Region

        Public Sub View()
            Funcs.CamFocusEntity(Pointer)
        End Sub

        Public Sub WarpToEntity(dest As Entity)
            Funcs.WarpEntity_Entity(Pointer, dest.Pointer)
        End Sub

        Public Sub WarpToEntityPtr(dest As Integer)
            Funcs.WarpEntity_Entity(Pointer, dest)
        End Sub

        Public Sub WarpToEventEntityID(dest As Integer)
            Funcs.WarpEntity_Entity(Pointer, Funcs.GetEntityPtr(dest))
        End Sub

    End Class
End Namespace
