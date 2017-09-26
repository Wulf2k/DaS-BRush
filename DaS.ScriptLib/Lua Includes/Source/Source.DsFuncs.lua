--[[

    Each Dark Souls function should only exist inside one of these Lua files at a time:
        DarkSoulsFunctions.lua (fully mapped functions which can be used as naturally as one would use any other types of Lua functions)
        DarkSoulsFunctions_Unmapped.lua (unmapped functions; no parameter lists or type specifications)
        DarkSoulsFunctions_This.lua (unmapped functions that are confirmed to be __thiscall-type functions and which use a helper class to call them for testing purposes)
        
    When you map a function in DarkSoulsFunctions_Unmapped.lua, please cut & paste the function into DarkSoulsFunctions.lua
    
    When you confirm that a function in DarkSoulsFunctions_Unmapped.lua is a __thiscall-type function, please cut & paste the function into DarkSoulsFunctions_This.lua

]]

---
--@return #boolean
function ActionEnd(chrId)
    return FUNC(BOOL, 0x00D616D0, {int(chrId)});
end

---
--@return #boolean
function AddActionCount(chrId, intB)
    return FUNC(BOOL, 0x00D5FC20, {int(chrId), int(intB)});
end

---
--@return #number unknown*
function AddBlockClearBonus()
    return FUNC(INT_PTR, 0x00D5E480, {});
end

---
--@return #number char*
function AddClearCount()
    FUNC(INT, 0x00D5EC20, {});
end

---
--@return #number
function AddCorpseEvent(intA, intB)
    return FUNC(INT, 0x00D60930, {int(intA), int(intB)});
end

---
--@return #number struct_a1_2*
function AddCustomRoutePoint(chrId, intB)
    return FUNC(INT, 0x00D645C0, {int(chrId), int(intB)});
end

---
--@return #number InfoAboutSummons*
function AddDeathCount()
    return FUNC(INT, 0x00D5DE20, {});
end

---
--@return #boolean
function AddEventSimpleTalk(intA, intB)
    return FUNC(BOOL, 0x00D62860, {int(intA), int(intB)});
end

---
--@return #boolean
function AddEventSimpleTalkTimer(intA, intB, floatC)
    return FUNC(BOOL, 0x00D62820, {int(intA), int(intB), floatC});
end

---
--@return #number
function AddFieldInsFilter(intA)
    return FUNC(INT, 0x00D62790, {int(intA)});
end

---
--@return #number
function AddGeneEvent(intA, intB)
    return FUNC(INT, 0x00D622B0, {int(intA), int(intB)});
end

---
--@return #number
function AddHitMaskByBit(chrId, byteB)
    return FUNC(BYTE, 0x00D64D50, {int(chrId), byte(byteB)});
end

---
--@return #number
function AddInfomationBuffer(probablyABufferOfSomeSort)
    return FUNC(BYTE, 0x00D64720, {int(probablyABufferOfSomeSort)});
end

---
--@return #number
function AddInfomationBufferTag(intA, intB, intC)
    return FUNC(BYTE, 0x00D64730, {int(intA), int(intB), int(intC)});
end

---
--@return #number
function AddInfomationList(intA, intB, intC)
    return FUNC(BYTE, 0x00D62370, {int(intA), int(intB), int(intC)});
end

---
--@return #number
---
--NOTE: intC and intD are (seemingly) unused by game, and intB/intC/intD 
--belong to 3 contiguous bytes of a struct.
--@return #boolean
function BeginAction(chrId, intB, intC, intD)
    return FUNC(BOOL, 0x00D5FCF0, {int(chrId), int(intB), int(intC), int(intD)});
end

---
--@return #number unknown*
function BeginLoopCheck(chrId)
    return FUNC(INT, 0x00D5FB90, {int(chrId)});
end

---
--@return #number
function CalcExcuteMultiBonus(intA, floatB, intC)
    return FUNC(INT, 0x00D5F4C0, {int(intA), floatB, int(floatC)});
end

---
--@return #boolean
function CamReset(localPlrChrId, boolDoReset)
    return FUNC(BOOL, 0x00D5FB00, {int(localPlrChrId), boolDoReset});
end

---
--TODO: Check if returns bool.
--Note: calls CastTargetSpellPlus with -1 for last arg
function CastTargetSpell(chrId, intB, intC, intD, intE)
    return FUNC(BYTE, 0x00D65E90, {int(chrId), int(intB), int(intC), int(intD), int(intE)});
end

---
--TODO: Check if returns bool.
--@return #number
function CastTargetSpellPlus(chrId, intB, intC, intD, intE, intF)
    return FUNC(BYTE, 0x00D65E50, {int(chrId), int(intB), int(intC), int(intD), int(intE), int(intF)});
end

---
--@return #number struct_a1_2*
function ChangeGreyGhost()
    return FUNC(INT, 0x00D663D0, {});
end

---
--Note: intA is not chrId
--@return #boolean
function ChangeModel(intA, intB)
    return FUNC(BOOL, 0x00D64C70, {int(intA), int(intB)});
end

---
--@return #boolean
function ChangeThink(chrId, think)
    return FUNC(BOOL, 0x00D658A0, {int(chrId), int(think)});
end

---
--Note: intA is not chrId
--@return #boolean
function ChangeWander(intA)
    return FUNC(BOOL, 0x00D61000, {int(intA)});
end

---
--@return #boolean
function CharacterAllAttachSys(chrId)
    return FUNC(BOOL, 0x00D60860, {int(chrId)});
end

---
--NOTE: intB is (seemingly) unused by game
--@return #boolean
function CharactorCopyPosAng(chrIdA, intB, chrIdB)
    return FUNC(BOOL, 0x00D64190, {int(chrIdA), int(intB), int(chrIdB)});
end

---
--@return #boolean
function CheckChrHit_Obj(intA, intB)
    return FUNC(BOOL, 0x00D5EA00, {int(intA), int(intB)});
end

---
--@return #boolean
function CheckChrHit_Wall(intA, intB)
    return FUNC(BOOL, 0x00D5EA30, {int(intA), int(intB)});
end

function CheckEventBody(intA)
    return FUNC(VOID, 0x00D5EC60, {int(intA)});
end

---
--@return #boolean
function CheckEventChr_Proxy(intA, intB)
    return FUNC(BOOL, 0x00D5E9E0, {int(intA), int(intB)});
end

---
--@return #number
function ChrDisableUpDate(chrId, intB)
    return FUNC(INT, 0x00D61460, {int(chrId), int(intB)});
end

---
--@return #number
function ChrFadeIn(chrId, dur, opacity)
    return FUNC(INT, 0x00D607E0, {int(chrId), dur, opacity});
end

---
--@return #number
function ChrFadeOut(chrId, dur, opacity)
    return FUNC(INT, 0x00D60770, {int(chrId), dur, opacity});
end

---
--@return #number
function ChrResetAnimation(chrId)
    return FUNC(INT, 0x00D60670, {int(chrId)});
end

---
--@return #number
function ChrResetRequest(chrId)
    return FUNC(INT, 0x00D606A0, {int(chrId)});
end

---
--@return #nil
function ClearBossGauge()
    return FUNC(VOID, 0x00D5DD80, {});
end

---
--@return #number &(struct_a1_2*)
function ClearSosSign()
    return FUNC(INT, 0x00D5EED0, {});
end

---
--@return #boolean
function ClearTarget(chrId)
    return FUNC(BOOL, 0x00D613C0, {int(chrId)});
end

---
--@return #number
function DisableDamage(chrId, state)
    return FUNC(INT, 0x00D60DD0, {int(chrId), state});
end

---
--@return #number
function DisableMapHit(chrId, state)
    return FUNC(INT, 0x00D61790, {int(chrId), state});
end

---
--@return #number
function DisableMove(chrId, state)
    return FUNC(INT, 0x00D617D0, {int(chrId), state});
end

function DisableCollection(...)
    return FUNC(INT, 0x00D60A40, {...});
end

function DisableHpGauge(...)
    return FUNC(INT, 0x00D60A00, {...});
end

function DisableInterupt(...)
    return FUNC(INT, 0x00D612B0, {...});
end

function DivideRest(...)
    return FUNC(INT, 0x00490520, {...});
end

function EnableAction(...)
    return FUNC(INT, 0x00D5FD60, {...});
end

function EnableGeneratorSystem(...)
    return FUNC(INT, 0x00D5EF20, {...});
end

function EnableHide(...)
    return FUNC(INT, 0x00D60E00, {...});
end

function EnableInvincible(...)
    return FUNC(INT, 0x00D60D90, {...});
end

function EnableLogic(...)
    return FUNC(INT, 0x00D5FD90, {...});
end

function EnableObjTreasure(...)
    return FUNC(INT, 0x00D661D0, {...});
end

---
--@return #number
function ForcePlayAnimation(chrId, animId)
    return FUNC(INT, 0x00D61CF0, {int(chrId), int(animId)});
end

---
--@return #number
function ForcePlayAnimationStayCancel(chrId, animId)
    return FUNC(INT, 0x00D61CA0, {int(chrId), int(animId)});
end

---
--@return #number
function ForcePlayLoopAnimation(chrId, animId)
    return FUNC(INT, 0x00D61C50, {int(chrId), int(animId)});
end

---
--@return #number
function ForceSetOmissionLevel()
    return FUNC(INT, 0x00D60CB0, {});
end

---
--@param #number intA ???
--@return #number
function GetCurrentMapAreaNo(intA)
    return FUNC(INT, 0x00D5F650, {int(intA)});
end

---
--@param #number intA ???
--@return #number
function GetCurrentMapBlockNo(intA)
    return FUNC(INT, 0x00D5F620, {int(intA)});
end

---
--@return #boolean
function IsAction(chrId, byteB)
    return FUNC(BOOL, 0x00D5FAB0, {int(chrId), byte(byteB)});
end

---
--@return #boolean
function IsAlive(chrId)
    return FUNC(BOOL, 0x00D615E0, {int(chrId)});
end

---
--@return #boolean
function IsAliveMotion()
    return FUNC(BOOL, 0x00D5E6A0, {});
end

---
--@return #boolean
function IsAngle(chrIdA, chrIdB, angle)
    return FUNC(BOOL, 0x00D61B30, {int(chrIdA), int(chrIdB), angle});
end

---
--@return #boolean
function IsAnglePlus(chrIdA, chrIdB, angleDWord, intD)
    return FUNC(BOOL, 0x00D61AF0, {int(chrIdA), int(chrIdB), int(angleDWord), int(intD)});
end

---
--@return #boolean
function IsAppearancePlayer(chrId)
    return FUNC(BOOL, 0x00D5F170, {int(chrId)});
end

---
--@return #boolean
function IsBlackGhost()
    return FUNC(BOOL, 0x00D60EF0, {});
end

---
--Note: intA is not chrId
--@return #boolean
function IsBlackGhost_NetPlayer(intA)
    return FUNC(BOOL, 0x00D5E8F0, {int(intA)});
end

---
--@return #boolean
function IsClearItem()
    return FUNC(BOOL, 0x00D5DCF0, {});
end

---
--@return #boolean
function IsClient()
    return FUNC(BOOL, 0x00D5E2E0, {});
end

---
--@return #boolean
function IsColiseumGhost()
    return FUNC(BOOL, 0x00D60EB0, {});
end

---
--@return #boolean
function IsCompleteEvent(eventId)
    return FUNC(BOOL, 0x00D60170, {int(eventId)});
end

---
--@return #boolean
function IsCompleteEventValue(eventId)
    return FUNC(BOOL, 0x00D60150, {int(eventId)});
end

---
--@return #boolean
function IsDead_NextGreyGhost()
    return FUNC(BOOL, 0x00D5E160, {});
end

---
--@return #boolean
function IsDeathPenaltySkip()
    return FUNC(BOOL, 0x00D5D7D0, {});
end

---
--@return #boolean
function IsDestroyed(intA)
    return FUNC(BOOL, 0x00D5F0E0, {int(intA)});
end

---
--@return #boolean
function IsDisable(chrId)
    return FUNC(BOOL, 0x00D60F70, {int(chrId)});
end

---
--@return #boolean
function IsDistance(chrIdA, chrIdB, distance)
    return FUNC(BOOL, 0x00D61BA0, {int(chrIdA), int(chrIdB), distance});
end

---
--@return #boolean
function IsDropCheck_Only(chrId, intB, intC, intD)
    return FUNC(BOOL, 0x00D60C00, {int(chrId), int(intB), int(intC), int(intD)});
end

---
--@return #boolean
function IsEquip(intA, intB)
    return FUNC(BOOL, 0x00D5F4F0, {int(intA), int(intB)});
end

---
--@return #boolean
function IsEventAnim(chrId, intB)
    return FUNC(BOOL, 0x00D61C30, {int(chrId), int(intB)});
end

---
--@return #boolean
function IsFireDead(chrId)
    return FUNC(BOOL, 0x00D60400, {int(chrId)});
end

---
--@return #boolean
function IsForceSummoned()
    return FUNC(BOOL, 0x00D5EC00, {});
end

---
--@return #boolean
function IsGameClient()
    return FUNC(BOOL, 0x00D5EAF0, {});
end

---
--@return #boolean
function IsGreyGhost()
    return FUNC(BOOL, 0x00D60F30, {});
end

---
--@return #boolean
function IsGreyGhost_NetPlayer(netPlayerPtr)
    return FUNC(BOOL, 0x00D5E970, {int(netPlayerPtr)});
end

---
--@return #boolean
function IsHost()
    return FUNC(BOOL, 0x00D5E300, {});
end

---
--@return #boolean
function IsInParty()
    return FUNC(BOOL, 0x00D5E1F0, {});
end

---
--@return #boolean
function IsInParty_EnemyMember()
    return FUNC(BOOL, 0x00D5EEC0, {});
end

---
--@return #boolean
function IsInParty_FriendMember()
    return FUNC(BOOL, 0x00D5E440, {});
end

---
--@return #boolean
function IsIntruder()
    return FUNC(BOOL, 0x00D60ED0, {});
end

---
--@return #boolean
function IsInventoryEquip(intA, intB)
    return FUNC(BOOL, 0x00D5EF40, {int(intA), int(intB)});
end

---
--@return #boolean
function IsJobType(intA)
    return FUNC(BOOL, 0x00D5E040, {int(intA)});
end

---
--@return #boolean
function IsLand(chrId)
    return FUNC(BOOL, 0x00D60450, {int(chrId)});
end

---
--@return #boolean
function IsLiveNetPlayer(intA)
    return FUNC(BOOL, 0x00D5E9B0, {int(intA)});
end

---
--@return #boolean
function IsLivePlayer()
    return FUNC(BOOL, 0x00D60F50, {});
end

---
--@return #boolean
function IsOnlineMode()
    return FUNC(BOOL, 0x00D5DD50, {});
end

---
--@return #boolean
function IsPlayerAssessMenu_Tutorial()
    return FUNC(BOOL, 0x00D5E010, {});
end

---
--@return #boolean
function IsPlayerStay(chrId)
    return FUNC(BOOL, 0x00D61610, {int(chrId)});
end

---
--@return #boolean
function IsPlayMovie()
    return FUNC(BOOL, 0x00D60AF0, {});
end

---
--@return #boolean
function IsPrevGreyGhost()
    return FUNC(BOOL, 0x00D5E140, {});
end

---
--@return #boolean
function IsProcessEventGoal(chrId)
    return FUNC(BOOL, 0x00D61340, {int(chrId)});
end

---
--@return #boolean
function IsReady_Obj(intA)
    return FUNC(BOOL, 0x00D5F370, {int(intA)});
end

---
--@return #boolean
function IsRegionDrop(chrId, intB, intC, intD, intE)
    return FUNC(BOOL, 0x00D60C30, {int(chrId), int(intB), int(intC), int(intD), int(intE)});
end

---
--@return #boolean
function IsRegionIn(chrId, intB)
    return FUNC(BOOL, 0x00D61020, {int(chrId), int(intB)});
end

---
--@return #boolean
function IsRevengeRequested()
    return FUNC(BOOL, 0x00D5E2A0, {});
end

---
--@return #boolean
function IsReviveWait()
    return FUNC(BOOL, 0x00D5E660, {});
end

---
--@return #boolean
function IsShow_CampMenu()
    return FUNC(BOOL, 0x00D5DA20, {});
end

---
--@return #boolean
function IsShowMenu()
    return FUNC(BOOL, 0x00D5EA90, {});
end

---
--@return #boolean
function IsShowMenu_BriefingMsg()
    return FUNC(BOOL, 0x00D5DD70, {});
end

---
--@return #boolean
function IsShowMenu_GenDialog()
    return FUNC(BOOL, 0x00D5EA80, {});
end

---
--@return #boolean
function IsShowMenu_InfoMenu()
    return FUNC(BOOL, 0x00D5DB80, {});
end

---
--@return #boolean
function IsShowSosMsg_Tutorial()
    return FUNC(BOOL, 0x00D5E000, {});
end

---
--@return #boolean
function IsSuccessQWC(intA)
    return FUNC(BOOL, 0x00D5F7D0, {int(intA)});
end

---
--@return #boolean
function IsTryJoinSession()
    return FUNC(BOOL, 0x00D5E2B0, {});
end

---
--@return #boolean
function IsValidInstance(chrId, intEnumB)
    return FUNC(BOOL, 0x00D5F510, {int(chrId), int(intEnumB)});
end

---
--@return #boolean
function IsValidTalk(intA)
    return FUNC(BOOL, 0x00D5E5F0, {int(intA)});
end

---
--@return #boolean

function IsWhiteGhost()
    return FUNC(BOOL, 0x00D60F10, {});
end

---
--@return #boolean
function IsWhiteGhost_NetPlayer(ptr)
    return FUNC(BOOL, 0x00D5E930, {int(ptr)});
end

---
--NOTE: ARG IS DOUBLE, LOADED INTO XMM0. MIGHT NOT WORK
--@return #number struct_a1_2*
function MultiDoping_AllEventBody(dopeRate)
    return FUNC(INT, 0x00D61D60, {dopeRate});
end

---
--@return #number
function PlayAnimation(chrId, animId)
    return FUNC(INT, 0x00D61D10, {int(chrId), int(animId)});
end

---
--@return #number
function PlayAnimationStayCancel(chrId, animId)
    return FUNC(INT, 0x00D61CC0, {int(chrId), int(animId)});
end

---
--@return #number
function PlayLoopAnimation(chrId, animId)
    return FUNC(INT, 0x00D61C70, {int(chrId), int(animId)});
end

---
--TODO: Check return type
function RequestEnding()
    return FUNC(VOID, 0x00D5DDD0, {});
end

---
--@return #number
function SetAlive(chrId, floatB)
    return FUNC(INT, 0x00D664A0, {int(chrId), floatB});
end

---
--@return #number
function SetBossGauge(chrId, intB, intC)
    return FUNC(INT, 0x00D60700, {int(chrId), int(intB), int(intC)});
end

---
--@return #number
function SetChrType(chrId, type)
    return FUNC(VOID, 0x00D60E70, {int(chrId), int(type)});
end

---
--@return #number
function SetCompletelyNoMove(chrId, flag)
    return FUNC(INT, 0x00D604E0, {int(chrId), flag});
end

---
--@return #number
function SetDeadMode(chrId, flag)
    return FUNC(INT, 0x00D61430, {int(chrId), flag});
end

---
--@return #number
function SetDeadMode2(chrId, flag)
    return FUNC(INT, 0x00D613F0, {int(chrId), flag});
end

---
--NOTE: DUMMY
--@return #boolean
function SetDefaultAnimation(chrId)
    return FUNC(BOOL, 0x00D5F110, {int(chrId)});
end

---
--Note: deductive reasoning on the arg name
--@return #number
function SetDefaultMapUid(mapUid)
    return FUNC(INT, 0x00D5F680, {int(mapUid)});
end

---
--@return #boolean True if chr with chrId exists
function SetDisable(chrId, flag)
    return FUNC(BOOL, 0x00D60FC0, {int(chrId), flag});
end

---
--@return #number
function SetEventFlag(flagId, state)
    return FUNC(INT, 0x00D60190, {int(flagId), state});
end

---
--@return #boolean
function SetInsideBattleArea(chrId, intB)
    return FUNC(BOOL, 0x00D61050, {int(chrId), int(intB)});
end

---
--Note: Could likely be the "cannot move before loading screen just before leaving online world" function
--@return #number FRPGNET*
function StopPlayer()
    return FUNC(INT, 0x00D60950, {});
end

---
--@return #boolean
function SubActionCount(chrId, intB)
    return FUNC(BOOL, 0x00D5FBC0, {int(chrId), int(intB)});
end

---
--@return #boolean
function TalkNextPage(intA)
    return FUNC(BOOL, 0x00D5E640, {int(intA)});
end

---
--@return #number struct_a2_2*
function Tutorial_begin()
    return FUNC(INT, 0x00D5E030, {});
end

---
--@return #number struct_a2_2*
function Tutorial_end()
    return FUNC(INT, 0x00D5E020, {});
end

---
--@return #number
function Util_RequestLevelUp(intA)
    return FUNC(INT, 0x00D5D830, {int(intA)});
end

---
--@return #number
function Util_RequestLevelUpFirst(intA)
    return FUNC(INT, 0x00D5D850, {int(intA)});
end

---
--@return #number
function Util_RequestRegene(intA)
    return FUNC(INT, 0x00D5D820, {int(intA)});
end

---
--@return #number
function Util_RequestRespawn(intA)
    return FUNC(INT, 0x00D5D810, {int(intA)});
end

---
--@return #number
function VariableExpand_211_param1(uintA)
    return FUNC(UINT, 0x00D5D8C0, {uint(uintA)});
end

---
--@return #number
function VariableExpand_211_param2(uintA)
    return FUNC(UINT, 0x00D5D8B0, {uint(uintA)});
end

---
--DUMMY
--@return #number
function VariableExpand_211_param3(byteA)
    return FUNC(INT, 0x00D5D8A0, {byte(byteA)});
end

---
--@return #number
function VariableExpand_22_param1(uintA)
    return FUNC(UINT, 0x00D5D8E0, {uint(uintA)});
end

---
--@return #number
function VariableExpand_22_param2(shortA)
    return FUNC(SHORT, 0x00D5D8D0, {short(shortA)});
end

---
--@return #number
function Warp(chrId, warpId)
    return FUNC(INT, 0x00D61D40, {int(chrId), int(warpId)});
end

---
--@return #number
function WarpNextStage(world, area, intC, intD, intE)
    return FUNC(INT, 0x00D62D00, {int(world), int(area), int(intC), int(intD), int(intE)});
end