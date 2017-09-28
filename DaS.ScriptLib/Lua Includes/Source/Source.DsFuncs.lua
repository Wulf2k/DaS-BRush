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
function ActionEnd(chrId : int) : bool
    return FUNC(BOOL, 0x00D616D0, {chrId});
end

---
--@return #boolean
function AddActionCount(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D5FC20, {chrId, b});
end

---
--@return #number unknown*
function AddBlockClearBonus() : int
    return FUNC(INT, 0x00D5E480, {});
end

---
--@return #number char*
function AddClearCount()
    FUNC(INT, 0x00D5EC20, {});
end

---
--@return #number
function AddCorpseEvent(a : int, b : int) : int
    return FUNC(INT, 0x00D60930, {a, b});
end

---
--@return #number struct_a1_2*
function AddCustomRoutePoint(chrId : int, b : int) : int
    return FUNC(INT, 0x00D645C0, {chrId, b});
end

---
--@return #number InfoAboutSummons*
function AddDeathCount() : int
    return FUNC(INT, 0x00D5DE20, {});
end

---
--@return #number
function AddEventGoal(a : int, b : int, c : float, d : float, e : float, f : float, g : float, h : float, i : float, j: float, k : float) : int
    return FUNC(INT, 0x00D66000, {a, b, c, d, e, f, g, h, i, j, k});
end

---
--@return #boolean
function AddEventSimpleTalk(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D62860, {a, b});
end

---
--@return #boolean
function AddEventSimpleTalkTimer(a : int, b : int, c : float) : bool
    return FUNC(BOOL, 0x00D62820, {a, b, c});
end

---
--@return #number
function AddFieldInsFilter(a : int) : int
    return FUNC(INT, 0x00D62790, {a});
end

---
--@return #number
function AddGeneEvent(a : int, b : int) : int
    return FUNC(INT, 0x00D622B0, {a, b});
end

---
--@return #number
function AddHelpWhiteGhost() : int
    return FUNC(INT, 0x00D5DB50, {});
end

---
--@return #number
function AddHitMaskByBit(chrId : int, b : byte) : byte
    return FUNC(BYTE, 0x00D64D50, {chrId, b});
end

---
--@return #number
function AddInfomationBuffer(probablyABufferOfSomeSort : int) : byte
    return FUNC(BYTE, 0x00D64720, {probablyABufferOfSomeSort});
end

---
--@return #number
function AddInfomationBufferTag(a : int, b : int, c : int) : byte
    return FUNC(BYTE, 0x00D64730, {a, b, c});
end

---
--@return #number
function AddInfomationList(a : int, b : int, c : int) : byte
    return FUNC(BYTE, 0x00D62370, {a, b, c});
end


function AddInfomationListItem(a : int, b : int, c : int) : int
    return FUNC(INT, 0x00D62350, {a, b, c});
end

function AddInventoryItem(itemId : int, category : int, quantity : int)
    return FUNC(INT, 0x00D664C0, {itemId, category, quantity});
end

---
--@return #number
---
--NOTE: c and d are (seemingly) unused by game, and b/c/d
--belong to 3 contiguous bytes of a struct.
--@return #boolean
function BeginAction(chrId : int, b : int, c : int, d : int) : bool
    return FUNC(BOOL, 0x00D5FCF0, {chrId, b, c, d});
end

---
--@return #number unknown*
function BeginLoopCheck(chrId : int) : int
    return FUNC(INT, 0x00D5FB90, {chrId});
end

---
--@return #number
function CalcExcuteMultiBonus(a : int, b : float, c : int) : int
    return FUNC(INT, 0x00D5F4C0, {a, b, c});
end

---
--@return #boolean
function CamReset(localPlrChrId : int, boolDoReset : bool) : bool
    return FUNC(BOOL, 0x00D5FB00, {localPlrChrId, boolDoReset});
end

---
--TODO: Check if returns bool.
--Note: calls CastTargetSpellPlus with -1 for last arg
function CastTargetSpell(chrId : int, b : int, c : int, d : int, e : int) : byte
    return FUNC(BYTE, 0x00D65E90, {chrId, b, c, d, e});
end

---
--TODO: Check if returns bool.
--@return #number
function CastTargetSpellPlus(chrId : int, b : int, c : int, d : int, e : int, f : int) : byte
    return FUNC(BYTE, 0x00D65E50, {chrId, b, c, d, e, f});
end

---
--@return #number struct_a1_2*
function ChangeGreyGhost() : int
    return FUNC(INT, 0x00D663D0, {});
end

---
--Note: a is not chrId
--@return #boolean
function ChangeModel(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D64C70, {a, b});
end

---
--@return #boolean
function ChangeThink(chrId : int, think : int) : bool
    return FUNC(BOOL, 0x00D658A0, {chrId, think});
end

---
--Note: a : int is not chrId : int
--@return #boolean
function ChangeWander(a : int) : bool
    return FUNC(BOOL, 0x00D61000, {a});
end

---
--@return #boolean
function CharacterAllAttachSys(chrId : int) : bool
    return FUNC(BOOL, 0x00D60860, {chrId});
end

---
--NOTE: b : int is (seemingly) unused by game
--@return #boolean
function CharactorCopyPosAng(chrId1 : int, b : int, chrId2 : int) : bool
    return FUNC(BOOL, 0x00D64190, {chrId1, b, chrId2});
end

---
--@return #boolean
function CheckChrHit_Obj(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D5EA00, {a, b});
end

---
--@return #boolean
function CheckChrHit_Wall(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D5EA30, {a, b});
end

function CheckEventBody(a : int)
    FUNC(VOID, 0x00D5EC60, {a});
end

---
--@return #boolean
function CheckEventChr_Proxy(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D5E9E0, {a, b});
end

---
--@return #number
function ChrDisableUpDate(chrId : int, b : int) : int
    return FUNC(INT, 0x00D61460, {chrId, b});
end

---
--@return #number
function ChrFadeIn(chrId : int, dur : float, opacity : float) : int
    return FUNC(INT, 0x00D607E0, {chrId, dur, opacity});
end

---
--@return #number
function ChrFadeOut(chrId : int, dur : float, opacity : float) : int
    return FUNC(INT, 0x00D60770, {chrId, dur, opacity});
end

---
--@return #number
function ChrResetAnimation(chrId : int) : int
    return FUNC(INT, 0x00D60670, {chrId});
end

---
--@return #number
function ChrResetRequest(chrId : int) : int
    return FUNC(INT, 0x00D606A0, {chrId});
end

---
--@return #nil
function ClearBossGauge()
    FUNC(VOID, 0x00D5DD80, {});
end

---
--@return #number &(struct_a1_2*)
function ClearSosSign() : int
    return FUNC(INT, 0x00D5EED0, {});
end

---
--@return #boolean
function ClearTarget(chrId : int) : bool
    return FUNC(BOOL, 0x00D613C0, {chrId});
end


function CloseGenDialog() : int
    return FUNC(INT, 0x00D5E500, {});
end

function CloseMenu()
    return FUNC(INT, 0x00D5ED00, {});
end

function CloseTalk(a : int)
    return FUNC(INT, 0x00D5E610, {a});
end

function CompleteEvent(eventId : int)
    return FUNC(INT, 0x00D660A0, {eventId});
end

---
--@return #number
function DisableDamage(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D60DD0, {chrId, state});
end

---
--@return #number
function DisableMapHit(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D61790, {chrId, state});
end

---
--@return #number
function DisableMove(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D617D0, {chrId, state});
end

---
--@return #number
function ForcePlayAnimation(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61CF0, {chrId, animId});
end

---
--@return #number
function ForcePlayAnimationStayCancel(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61CA0, {chrId, animId});
end

---
--@return #number
function ForcePlayLoopAnimation(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61C50, {chrId, animId});
end

---
--@return #number
function ForceSetOmissionLevel() : int
    return FUNC(INT, 0x00D60CB0, {});
end

---
--@param #number a : int ???
--@return #number
function GetCurrentMapAreaNo(a : int) : int
    return FUNC(INT, 0x00D5F650, {a});
end

---
--@param #number a : int ???
--@return #number
function GetCurrentMapBlockNo(a : int) : int
    return FUNC(INT, 0x00D5F620, {a});
end

function GetItem(a : int, b : int)
    return FUNC(INT, 0x00D5E240, {a, b});
end

---
--@return #boolean
function IsAction(chrId : int, b : byte) : bool
    return FUNC(BOOL, 0x00D5FAB0, {chrId, byte(b)});
end

---
--@return #boolean
function IsAlive(chrId : int) : bool
    return FUNC(BOOL, 0x00D615E0, {chrId});
end

---
--@return #boolean
function IsAliveMotion() : bool
    return FUNC(BOOL, 0x00D5E6A0, {});
end

---
--@return #boolean
function IsAngle(chrId1 : int, chrId2 : int, angle : float) : bool
    return FUNC(BOOL, 0x00D61B30, {chrId1, chrId2, angle});
end

---
--@return #boolean
function IsAnglePlus(chrId1 : int, chrId2 : int, angleDWord : int, d : int) : bool
    return FUNC(BOOL, 0x00D61AF0, {chrId1, chrId2, angleDWord, d});
end

---
--@return #boolean
function IsAppearancePlayer(chrId : int) : bool
    return FUNC(BOOL, 0x00D5F170, {chrId});
end

---
--@return #boolean
function IsBlackGhost() : bool
    return FUNC(BOOL, 0x00D60EF0, {});
end

---
--Note: a : int is not chrId : int
--@return #boolean
function IsBlackGhost_NetPlayer(a : int) : bool
    return FUNC(BOOL, 0x00D5E8F0, {a});
end

---
--@return #boolean
function IsClearItem() : bool
    return FUNC(BOOL, 0x00D5DCF0, {});
end

---
--@return #boolean
function IsClient() : bool
    return FUNC(BOOL, 0x00D5E2E0, {});
end

---
--@return #boolean
function IsColiseumGhost() : bool
    return FUNC(BOOL, 0x00D60EB0, {});
end

---
--@return #boolean
function IsCompleteEvent(eventId : int) : bool
    return FUNC(BOOL, 0x00D60170, {eventId});
end

---
--@return #boolean
function IsCompleteEventValue(eventId : int) : bool
    return FUNC(BOOL, 0x00D60150, {eventId});
end

---
--@return #boolean
function IsDead_NextGreyGhost() : bool
    return FUNC(BOOL, 0x00D5E160, {});
end

---
--@return #boolean
function IsDeathPenaltySkip() : bool
    return FUNC(BOOL, 0x00D5D7D0, {});
end

---
--@return #boolean
function IsDestroyed(a : int) : bool
    return FUNC(BOOL, 0x00D5F0E0, {a});
end

---
--@return #boolean
function IsDisable(chrId : int) : bool
    return FUNC(BOOL, 0x00D60F70, {chrId});
end

---
--@return #boolean
function IsDistance(chrId1 : int, chrId2 : int, distance) : bool
    return FUNC(BOOL, 0x00D61BA0, {chrId1, chrId2, distance});
end

---
--@return #boolean
function IsDropCheck_Only(chrId : int, b : int, c : int, d : int) : bool
    return FUNC(BOOL, 0x00D60C00, {chrId, b, c, d});
end

---
--@return #boolean
function IsEquip(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D5F4F0, {a, b});
end

---
--@return #boolean
function IsEventAnim(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D61C30, {chrId, b});
end

---
--@return #boolean
function IsFireDead(chrId : int) : bool
    return FUNC(BOOL, 0x00D60400, {chrId});
end

---
--@return #boolean
function IsForceSummoned() : bool
    return FUNC(BOOL, 0x00D5EC00, {});
end

---
--@return #boolean
function IsGameClient() : bool
    return FUNC(BOOL, 0x00D5EAF0, {});
end

---
--@return #boolean
function IsGreyGhost() : bool
    return FUNC(BOOL, 0x00D60F30, {});
end

---
--@return #boolean
function IsGreyGhost_NetPlayer(netPlayerPtr : int) : bool
    return FUNC(BOOL, 0x00D5E970, {netPlayerPtr});
end

---
--@return #boolean
function IsHost() : bool
    return FUNC(BOOL, 0x00D5E300, {});
end

---
--@return #boolean
function IsInParty() : bool
    return FUNC(BOOL, 0x00D5E1F0, {});
end

---
--@return #boolean
function IsInParty_EnemyMember() : bool
    return FUNC(BOOL, 0x00D5EEC0, {});
end

---
--@return #boolean
function IsInParty_FriendMember() : bool
    return FUNC(BOOL, 0x00D5E440, {});
end

---
--@return #boolean
function IsIntruder() : bool
    return FUNC(BOOL, 0x00D60ED0, {});
end

---
--@return #boolean
function IsInventoryEquip(a : int, b : int) : bool
    return FUNC(BOOL, 0x00D5EF40, {a, b});
end

---
--@return #boolean
function IsJobType(a : int) : bool
    return FUNC(BOOL, 0x00D5E040, {a});
end

---
--@return #boolean
function IsLand(chrId : int) : bool
    return FUNC(BOOL, 0x00D60450, {chrId});
end

---
--@return #boolean
function IsLiveNetPlayer(a : int) : bool
    return FUNC(BOOL, 0x00D5E9B0, {a});
end

---
--@return #boolean
function IsLivePlayer() : bool
    return FUNC(BOOL, 0x00D60F50, {});
end

---
--@return #boolean
function IsOnlineMode() : bool
    return FUNC(BOOL, 0x00D5DD50, {});
end

---
--@return #boolean
function IsPlayerAssessMenu_Tutorial() : bool
    return FUNC(BOOL, 0x00D5E010, {});
end

---
--@return #boolean
function IsPlayerStay(chrId : int) : bool
    return FUNC(BOOL, 0x00D61610, {chrId});
end

---
--@return #boolean
function IsPlayMovie() : bool
    return FUNC(BOOL, 0x00D60AF0, {});
end

---
--@return #boolean
function IsPrevGreyGhost() : bool
    return FUNC(BOOL, 0x00D5E140, {});
end

---
--@return #boolean
function IsProcessEventGoal(chrId : int) : bool
    return FUNC(BOOL, 0x00D61340, {chrId});
end

---
--@return #boolean
function IsReady_Obj(a : int) : bool
    return FUNC(BOOL, 0x00D5F370, {a});
end

---
--@return #boolean
function IsRegionDrop(chrId : int, b : int, c : int, d : int, e : int) : bool
    return FUNC(BOOL, 0x00D60C30, {chrId, b, c, d, e});
end

---
--@return #boolean
function IsRegionIn(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D61020, {chrId, b});
end

---
--@return #boolean
function IsRevengeRequested() : bool
    return FUNC(BOOL, 0x00D5E2A0, {});
end

---
--@return #boolean
function IsReviveWait() : bool
    return FUNC(BOOL, 0x00D5E660, {});
end

---
--@return #boolean
function IsShow_CampMenu() : bool
    return FUNC(BOOL, 0x00D5DA20, {});
end

---
--@return #boolean
function IsShowMenu() : bool
    return FUNC(BOOL, 0x00D5EA90, {});
end

---
--@return #boolean
function IsShowMenu_BriefingMsg() : bool
    return FUNC(BOOL, 0x00D5DD70, {});
end

---
--@return #boolean
function IsShowMenu_GenDialog() : bool
    return FUNC(BOOL, 0x00D5EA80, {});
end

---
--@return #boolean
function IsShowMenu_InfoMenu() : bool
    return FUNC(BOOL, 0x00D5DB80, {});
end

---
--@return #boolean
function IsShowSosMsg_Tutorial() : bool
    return FUNC(BOOL, 0x00D5E000, {});
end

---
--@return #boolean
function IsSuccessQWC(a : int) : bool
    return FUNC(BOOL, 0x00D5F7D0, {a});
end

---
--@return #boolean
function IsTryJoinSession() : bool
    return FUNC(BOOL, 0x00D5E2B0, {});
end

---
--@return #boolean
function IsValidInstance(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D5F510, {chrId, b});
end

---
--@return #boolean
function IsValidTalk(a : int) : bool
    return FUNC(BOOL, 0x00D5E5F0, {a});
end

---
--@return #boolean

function IsWhiteGhost() : bool
    return FUNC(BOOL, 0x00D60F10, {});
end

---
--@return #boolean
function IsWhiteGhost_NetPlayer(ptr : int) : bool
    return FUNC(BOOL, 0x00D5E930, {ptr});
end

---
--NOTE: ARG IS DOUBLE, LOADED INTO XMM0. MIGHT NOT WORK
--@return #number struct_a1_2*
function MultiDoping_AllEventBody(dopeRate : float) : int
    return FUNC(INT, 0x00D61D60, {dopeRate});
end

---
--@return #number
function PlayAnimation(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61D10, {chrId, animId});
end

---
--@return #number
function PlayAnimationStayCancel(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61CC0, {chrId, animId});
end

---
--@return #number
function PlayLoopAnimation(chrId : int, animId : int) : int
    return FUNC(INT, 0x00D61C70, {chrId, animId});
end

---
--TODO: Check return type
function RequestEnding()
    return FUNC(VOID, 0x00D5DDD0, {});
end

---
--@return #number
function SetAlive(chrId : int, b : float) : int
    return FUNC(INT, 0x00D664A0, {chrId, b});
end

---
--@return #number
function SetBossGauge(chrId : int, b : int, c : int) : int
    return FUNC(INT, 0x00D60700, {chrId, b, c});
end

---
--@return #number
function SetChrType(chrId : int, type : int)
    return FUNC(VOID, 0x00D60E70, {chrId, type});
end

---
--@return #number
function SetCompletelyNoMove(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D604E0, {chrId, state});
end

---
--@return #number
function SetDeadMode(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D61430, {chrId, state});
end

---
--@return #number
function SetDeadMode2(chrId : int, state : bool) : int
    return FUNC(INT, 0x00D613F0, {chrId, state});
end

---
--NOTE: DUMMY
--@return #boolean
function SetDefaultAnimation(chrId : int) : bool
    return FUNC(BOOL, 0x00D5F110, {chrId});
end

---
--Note: deductive reasoning on the arg name
--@return #number
function SetDefaultMapUid(mapUid : int) : int
    return FUNC(INT, 0x00D5F680, {mapUid});
end

---
--@return #boolean True if chr with chrId : int exists
function SetDisable(chrId : int, state : bool) : bool
    return FUNC(BOOL, 0x00D60FC0, {chrId, state});
end

---
--@return #number
function SetEventFlag(flagId : int, state : bool) : int
    return FUNC(INT, 0x00D60190, {flagId, state});
end

---
--@return #boolean
function SetInsideBattleArea(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D61050, {chrId, b});
end

function SetTextEffect(id : int) : int
    return FUNC(INT, 0x00D5E430, {id});
end

---
--Note: Could likely be the "cannot move before loading screen just before leaving online world" function
--@return #number FRPGNET*
function StopPlayer() : int
    return FUNC(INT, 0x00D60950, {});
end

---
--@return #boolean
function SubActionCount(chrId : int, b : int) : bool
    return FUNC(BOOL, 0x00D5FBC0, {chrId, b});
end

---
--@return #boolean
function TalkNextPage(a : int) : bool
    return FUNC(BOOL, 0x00D5E640, {a});
end

---
--@return #number struct_a2_2*
function Tutorial_begin() : int
    return FUNC(INT, 0x00D5E030, {});
end

---
--@return #number struct_a2_2*
function Tutorial_end() : int
    return FUNC(INT, 0x00D5E020, {});
end

---
--@return #number
function Util_RequestLevelUp(a : int) : int
    return FUNC(INT, 0x00D5D830, {a});
end

---
--@return #number
function Util_RequestLevelUpFirst(a : int) : int
    return FUNC(INT, 0x00D5D850, {a});
end

---
--@return #number
function Util_RequestRegene(a : int) : int
    return FUNC(INT, 0x00D5D820, {a});
end

---
--@return #number
function Util_RequestRespawn(a : int) : int
    return FUNC(INT, 0x00D5D810, {a});
end

---
--@return #number
function VariableExpand_211_param1(a : uint) : uint
    return FUNC(UINT, 0x00D5D8C0, {a});
end

---
--@return #number
function VariableExpand_211_param2(a : uint) : uint
    return FUNC(UINT, 0x00D5D8B0, {a});
end

---
--DUMMY
--@return #number
function VariableExpand_211_param3(a : byte) : int
    return FUNC(INT, 0x00D5D8A0, {a});
end

---
--@return #number
function VariableExpand_22_param1(a : uint) : uint
    return FUNC(UINT, 0x00D5D8E0, {a});
end

---
--@return #number
function VariableExpand_22_param2(a : short) : short
    return FUNC(SHORT, 0x00D5D8D0, {a});
end

---
--@return #number
function Warp(chrId : int, warpId : int) : int
    return FUNC(INT, 0x00D61D40, {chrId, warpId});
end

---
--@return #number
function WarpNextStage(world : int, area : int, c : int, d : int, e : int) : int
    return FUNC(INT, 0x00D62D00, {world, area, c, d, e});
end