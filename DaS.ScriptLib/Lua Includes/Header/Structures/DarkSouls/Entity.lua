---
-- @type EntityFlagsA
EntityFlagsA = {
    None = 0,
    SetDeadMode = 0x2000000,
    DisableDamage = 0x4000000,
    EnableInvincible = 0x8000000,
    FirstPersonView = 0x100000,
    SetDrawEnable = 0x800000,
    SetSuperArmor = 0x10000,
    SetDisableGravity = 0x4000
}

---
-- @type EntityFlagsB
EntityFlagsB = {
    None = 0,
    DisableHpGauge = 0x8
}

---
-- @type EntityDebugFlags
EntityDebugFlags = {
    NoGoodsConsume = 0x1000000,
    DisableDrawingA = 0x100000,
    DrawCounter = 0x200000,
    DisableDrawingB = 0x80000,
    DrawDirection = 0x4000,
    NoUpdate = 0x8000,
    NoAttack = 0x100,
    NoMove = 0x200,
    NoStamConsume = 0x400,
    NoMPConsume = 0x800,
    NoDead = 0x20,
    NoDamage = 0x40,
    NoHit = 0x80,
    DrawHit = 0x4,
}

---
-- @type EntityMapFlagsA
EntityMapFlagsA = {
    None = 0,
    EnableLogic = 0x1
}

---
-- @type EntityMapFlagsB
EntityMapFlagsA = {
    None = 0,
    DisableCollision = 0x40
}

---
-- @type Covenant
Covenant = {
    None = 0,
    WayOfWhite = 1,
    PrincessGuard = 2,
    WarriorOfSunlight = 3,
    Darkwraith = 4,
    PathOfTheDragon = 5,
    GravelordServant = 6,
    ForestHunter = 7,
    DarkmoonBlade = 8,
    ChaosServant = 9
}

---
-- @type Entity
-- @field #EntityFlagsA FlagsA
-- @field #EntityFlagsB FlagsB
-- @field #EntityDebugFlags DebugFlags
-- @field #EntityMapFlagsA MapFlagsA
-- @field #EntityMapFlagsB MapFlagsB
-- @field [parent=#Entity] #EntityController Controller
-- @field #EntityController DebugController
Entity = {
    HeaderPtr = 0,
    Header = EntityHeader,
    DisableEventBackreadState = false,
    DebugControllerPtr = 0,
    DebugController = EntityController,
    Covenant = 0,
    Pointer = 0,
    ModelName = "",
    NPCID = 0,
    NPCID2 = 0,
    ChrType = 0,
    TeamType = 0,
    IsTargetLocked = false,
    DeathStructPointer = 0,
    IsDead = false,
    PoiseCurrent = 0,
    PoiseMax = 0,
    PoiseRecoverTimer = 0,
    FlagsA = EntityFlagsA.None,
    FlagsB = EntityFlagsB.None,
    EventEntityID = -1,
    Opacity = 0,
    DrawGroup1 = 0,
    DrawGroup2 = 0,
    DrawGroup3 = 0,
    DrawGroup4 = 0,
    DispGroup1 = 0,
    DispGroup2 = 0,
    DispGroup3 = 0,
    DispGroup4 = 0,
    MultiplayerZone = 0,
    Material_Floor = 0,
    Material_ArmorSE = 0,
    Material_ArmorSFX = 0,
    HP = 0,
    MaxHP = 0,
    Stamina = 0,
    MaxStamina = 0,
    ResistancePoisonCurrent = 0,
    ResistanceToxicCurrent = 0,
    ResistanceBleedCurrent = 0,
    ResistanceCurseCurrent = 0,
    ResistancePoisonMax = 0,
    ResistanceToxicMax = 0,
    ResistanceBleedMax = 0,
    ResistanceCurseMax = 0,
    Unknown1Ptr = 0,
    CharMapDataPtr = 0,
    MapFlagsA = 0,
    MapFlagsB = 0,
    ControllerPtr = 0,
    Controller = EntityController,
    AnimationPtr = 0,
    AnimationInstancePtr = 0,
    AnimInstanceTime = 0,
    AnimInstanceSpeed = 0,
    AnimInstanceLoopCount = 0,
    AnimationSpeed = 0,
    AnimDbgDrawSkeleton = 0,
    AnimDbgDrawBoneName = 0,
    AnimDbgDrawExtractMotion = 0,
    AnimDbgSlotLog = 0,
    LocationPtr = 0,
    Location = EntityLocation,
    StatsPtr = 0,
    StatHP = 0,
    StatMaxHPBase = 0,
    StatMaxHP = 0,
    StatMP = 0,
    StatMaxMPBase = 0,
    StatMaxMP = 0,
    StatStamina = 0,
    StatMaxStaminaBase = 0,
    StatMaxStamina = 0,
    StatVIT = 0,
    StatATN = 0,
    StatEND = 0,
    StatSTR = 0,
    StatDEX = 0,
    StatINT = 0,
    StatFTH = 0,
    StatRES = 0,
    StatHumanity = 0,
    StatGender = 0, --I DID THE THING REEEEEE
    StatDebugShopLevel = 0,
    StatStartingClass = 0,
    StatPhysique = 0,
    StatStartingGift = 0,
    StatMultiplayCount = 0,
    StatCoOpSuccessCount = 0,
    StatThiefInvadePlaySuccessCount = 0,
    StatPlayerRankS = 0,
    StatPlayerRankA = 0,
    StatPlayerRankB = 0,
    StatPlayerRankC = 0,
    StatDevotionWarriorOfSunlight = 0,
    StatDevotionDarkwraith = 0,
    StatDevotionDragon = 0,
    StatDevotionGravelord = 0,
    StatDevotionForest = 0,
    StatDevotionDarkmoon = 0,
    StatDevotionChaos = 0,
    StatIndictments = 0,
    StatDebugBlockClearBonus = 0,
    StatEggSouls = 0,
    StatPoisonResist = 0,
    StatBleedResist = 0,
    StatToxicResist = 0,
    StatCurseResist = 0,
    StatDebugClearItem = 0,
    StatDebugResvSoulSteam = 0,
    StatDebugResvSoulPenalty = 0,
    StatCovenant = Covenant.None,
    StatAppearanceFaceType = 0,
    StatAppearanceHairType = 0,
    StatAppearanceHairAndEyesColor = 0,
    StatCurseLevel = 0,
    StatInvadeType = 0,
    StatEquipRightHand2 = 0,
    StatEquipHelmet = 0,
	StatEquipChest = 0,
	StatEquipGloves = 0,
	StatEquipPants = 0,
    StatAppearanceScaleHead = 0,
    StatAppearanceScaleChest = 0,
    StatAppearanceScaleWaist = 0,
    StatAppearanceScaleArms = 0,
    StatAppearanceScaleLegs = 0,
    StatAppearanceHairColorR = 0,
    StatAppearanceHairColorG = 0,
    StatAppearanceHairColorB = 0,
    StatAppearanceHairColorA = 0,
    StatAppearanceEyeColorR = 0,
    StatAppearanceEyeColorG = 0,
    StatAppearanceEyeColorB = 0,
    StatAppearanceEyeColorA = 0,
    StatAppearanceFaceData = {},
    StatMagicDefense = 0,
    StatMaxItemBurden = 0,
    StatPoisonBuildup = 0,
    StatToxicBuildup = 0,
    StatBleedBuildup = 0,
    StatCurseBuildup = 0,
    StatPoise = 0,
    StatSoulLevel = 0,
    StatSouls = 0,
    StatPointTotal = 0,
    StatName = "",
    DebugFlags = 0,
    NoGoodsConsume = false,
    DisableDrawingA = false,
    DrawCounter = false,
    DisableDrawingB = false,
    DrawDirection = false,
    NoUpdate = false,
    NoAttack = false,
    NoMove = false,
    NoStamConsume = false,
    NoMPConsume = false,
    NoDead = false,
    NoDamage = false,
    DrawHit = false,
    DisableDamage = false,
    EnableInvincible = false,
    FirstPersonView = false,
    SetDeadMode = false,
    SetDisableGravity = false,
    SetDrawEnable = false,
    SetSuperArmor = false,
    DisableCollision = false,
    EnableLogic = false,
}

---
-- Gets a flag in @{Entity.DebugFlags}
-- @param #Entity self This entity instance.
-- @param #EntityDebugFlags flg The flag to get the state of within the @{Entity.DebugFlags} bitmask.
-- @return #boolean The bit state (on or off)
function Entity.GetDebugFlag(self, flg) return false; end

---
-- Sets a flag in @{Entity.DebugFlags}
-- @param #Entity self This entity instance.
-- @param #EntityDebugFlags flg The flag to modify in the @{Entity.DebugFlags} bitmask.
-- @param #boolean state Bit state (on or off)
function Entity.SetDebugFlag(self, flg, state) end

---
-- Gets a flag in @{Entity.FlagsA}
-- @param #Entity self This entity instance.
-- @param #EntityFlagsA flg The flag to get the state of within the @{Entity.FlagsA} bitmask.
-- @return #boolean The bit state (on or off)
function Entity.GetFlagA(self, flg) return false; end

---
-- Sets a flag in @{Entity.FlagsA}
-- @param #Entity self This entity instance.
-- @param #EntityFlagsA flg The flag to modify in the @{Entity.FlagsA} bitmask.
-- @param #boolean state Bit state (on or off)
function Entity.SetFlagA(self, flg, state) end

---
-- Makes the camera follow this entity.
function Entity:View() end

---
-- Warps this entity to the given entity.
-- @function [parent=#Entity]
-- @param #Entity entity The entity to warp to.
function Entity:WarpToEntity(entity) end

---
-- Warps this entity to the entity with the given pointer.
-- @function [parent=#Entity]
-- @param #number entityPtr The pointer of the entity to warp to.
function Entity.WarpToEntityPtr(entityPtr) end

---
-- Warps this entity to the entity with the given ID.
-- @function [parent=#Entity]
-- @param #number entityId The EventEntityID of the entity to warp to.
function Entity:WarpToEventEntityID(entityId) end


--Static

Entity.PlayerStablePosPtr = 0
Entity.PlayerStablePosX = 0
Entity.PlayerStablePosY = 0
Entity.PlayerStablePosZ = 0

---
-- Gets the local player.
-- @return #Entity The player character.
Entity.GetPlayer = function() return Entity; end

---
-- Gets an entity from the given EventEntityID.
-- @return #Entity The entity.
Entity.FromID = function(id) return Entity; end

---
-- Gets an entity from the given map/entity name pair.
-- @param #string mapName The name of the map e.g. "m12_01_00_00"
-- @param #string entityName The name of the entity e.g. "c4100_0000"
-- @return #Entity The entity.
Entity.FromName = function(mapName, entityName) return Entity; end