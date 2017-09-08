--[[

    NOTE: To uncomment a function, change the "--[" on top to "---["
    This uncomments the entire function immediately.

]]

--[[
function ActionEnd()
    return FUNC(INT, 0x00D616D0, {});
end
---]]

--[[
function AddActionCount()
    return FUNC(INT, 0x00D5FC20, {});
end
---]]

--[[
function AddBlockClearBonus()
    return FUNC(INT, 0x00D5E480, {});
end
---]]

---[[
function AddClearCount()
    FUNC(VOID, 0x00D5EC20, {});
end
---]]

--[[
function AddCorpseEvent()
    return FUNC(INT, 0x00D60930, {});
end
---]]

--[[
function AddCurrentVowRankPoint()
    return FUNC(INT, 0x00D600D0, {});
end
---]]

--[[
function AddCustomRoutePoint()
    return FUNC(INT, 0x00D645C0, {});
end
---]]

--[[
function AddDeathCount()
    return FUNC(INT, 0x00D5DE20, {});
end
---]]

--[[
function AddEventParts()
    return FUNC(INT, 0x00D60D30, {});
end
---]]

--[[
function AddEventParts_Ignore()
    return FUNC(INT, 0x00D60CF0, {});
end
---]]

--[[
function AddEventSimpleTalk()
    return FUNC(INT, 0x00D62860, {});
end
---]]

--[[
function AddEventSimpleTalkTimer()
    return FUNC(INT, 0x00D62820, {});
end
---]]

--[[
function AddFieldInsFilter()
    return FUNC(INT, 0x00D62790, {});
end
---]]

--[[
function AddGeneEvent()
    return FUNC(INT, 0x00D622B0, {});
end
---]]

--[[
function AddHelpWhiteGhost()
    return FUNC(INT, 0x00D5DB50, {});
end
---]]

--[[
function AddHitMaskByBit()
    return FUNC(INT, 0x00D64D50, {});
end
---]]

--[[
function AddInfomationBuffer()
    return FUNC(INT, 0x00D64720, {});
end
---]]

--[[
function AddInfomationBufferTag()
    return FUNC(INT, 0x00D64730, {});
end
---]]

--[[
function AddInfomationList()
    return FUNC(INT, 0x00D62370, {});
end
---]]

--[[
function AddInfomationListItem()
    return FUNC(INT, 0x00D62350, {});
end
---]]

--[[
function AddInfomationTimeMsgTag()
    return FUNC(INT, 0x00D646F0, {});
end
---]]

--[[
function AddInfomationTosBuffer()
    return FUNC(INT, 0x00D646D0, {});
end
---]]

--[[
function AddInfomationTosBufferPlus()
    return FUNC(INT, 0x00D646B0, {});
end
---]]

--[[
function AddInventoryItem()
    return FUNC(INT, 0x00D664C0, {});
end
---]]

--[[
function AddKillBlackGhost()
    return FUNC(INT, 0x00D5DB30, {});
end
---]]

--[[
function AddQWC()
    return FUNC(INT, 0x00D5F810, {});
end
---]]

--[[
function AddRumble()
    return FUNC(INT, 0x00D640D0, {});
end
---]]

--[[
function AddTreasureEvent()
    return FUNC(INT, 0x00D5F3B0, {});
end
---]]

--[[
function AddTrueDeathCount()
    return FUNC(INT, 0x00D5DDF0, {});
end
---]]

--[[
function BeginAction()
    return FUNC(INT, 0x00D5FCF0, {});
end
---]]

--[[
function BeginLoopCheck()
    return FUNC(INT, 0x00D5FB90, {});
end
---]]

--[[
function CalcExcuteMultiBonus()
    return FUNC(INT, 0x00D5F4C0, {});
end
---]]

--[[
function CalcGetCurrentMapEntityId()
    return FUNC(INT, 0x00D5F230, {});
end
---]]

--[[
function CalcGetMultiWallEntityId()
    return FUNC(INT, 0x00D5E360, {});
end
---]]

--[[
function CamReset()
    return FUNC(INT, 0x00D5FB00, {});
end
---]]

--[[
function CastPointSpell()
    return FUNC(INT, 0x00D65FB0, {});
end
---]]

--[[
function CastPointSpell_Horming()
    return FUNC(INT, 0x00D65F10, {});
end
---]]

--[[
function CastPointSpellPlus()
    return FUNC(INT, 0x00D65F60, {});
end
---]]

--[[
function CastPointSpellPlus_Horming()
    return FUNC(INT, 0x00D65EC0, {});
end
---]]

--[[
function CastTargetSpell()
    return FUNC(INT, 0x00D65E90, {});
end
---]]

--[[
function CastTargetSpellPlus()
    return FUNC(INT, 0x00D65E50, {});
end
---]]

--[[
function ChangeGreyGhost()
    return FUNC(INT, 0x00D663D0, {});
end
---]]

--[[
function ChangeInitPosAng()
    return FUNC(INT, 0x00D60D70, {});
end
---]]

--[[
function ChangeModel()
    return FUNC(INT, 0x00D64C70, {});
end
---]]

--[[
function ChangeTarget()
    return FUNC(INT, 0x00D61DA0, {});
end
---]]

--[[
function ChangeThink()
    return FUNC(INT, 0x00D658A0, {});
end
---]]

--[[
function ChangeWander()
    return FUNC(INT, 0x00D61000, {});
end
---]]

--[[
function CharacterAllAttachSys()
    return FUNC(INT, 0x00D60860, {});
end
---]]

--[[
function CharactorCopyPosAng()
    return FUNC(INT, 0x00D64190, {});
end
---]]

--[[
function CheckChrHit_Obj()
    return FUNC(INT, 0x00D5EA00, {});
end
---]]

--[[
function CheckChrHit_Wall()
    return FUNC(INT, 0x00D5EA30, {});
end
---]]

--[[
function CheckEventBody()
    return FUNC(INT, 0x00D5EC60, {});
end
---]]

--[[
function CheckEventChr_Proxy()
    return FUNC(INT, 0x00D5E9E0, {});
end
---]]

--[[
function CheckPenalty()
    return FUNC(INT, 0x00D5EE50, {});
end
---]]

--[[
function ChrDisableUpDate()
    return FUNC(INT, 0x00D61460, {});
end
---]]

---[[
function ChrFadeIn(chrId, dur, opacity)
    return FUNC(INT, 0x00D607E0, {int(chrId), dur, opacity});
end
---]]

--[[
function ChrFadeOut()
    return FUNC(INT, 0x00D60770, {});
end
---]]

--[[
function ChrResetAnimation()
    return FUNC(INT, 0x00D60670, {});
end
---]]

--[[
function ChrResetRequest()
    return FUNC(INT, 0x00D606A0, {});
end
---]]

--[[
function ClearBossGauge()
    return FUNC(INT, 0x00D5DD80, {});
end
---]]

--[[
function ClearMyWorldState()
    return FUNC(INT, 0x00D5EDD0, {});
end
---]]

--[[
function ClearSosSign()
    return FUNC(INT, 0x00D5EED0, {});
end
---]]

--[[
function ClearTarget()
    return FUNC(INT, 0x00D613C0, {});
end
---]]

--[[
function CloseGenDialog()
    return FUNC(INT, 0x00D5E500, {});
end
---]]

--[[
function CloseMenu()
    return FUNC(INT, 0x00D5ED00, {});
end
---]]

--[[
function CloseRankingDialog()
    return FUNC(INT, 0x00D5D950, {});
end
---]]

--[[
function CloseTalk()
    return FUNC(INT, 0x00D5E610, {});
end
---]]

--[[
function CompleteEvent()
    return FUNC(INT, 0x00D660A0, {});
end
---]]

--[[
function CreateCamSfx()
    return FUNC(INT, 0x00D620B0, {});
end
---]]

--[[
function CreateDamage_NoCollision()
    return FUNC(INT, 0x00D65DC0, {});
end
---]]

--[[
function CreateEventBody()
    return FUNC(INT, 0x00D65830, {});
end
---]]

--[[
function CreateEventBodyPlus()
    return FUNC(INT, 0x00D657C0, {});
end
---]]

--[[
function CreateHeroBloodStain()
    return FUNC(INT, 0x00D62640, {});
end
---]]

--[[
function CreateSfx()
    return FUNC(INT, 0x00D5F000, {});
end
---]]

--[[
function CreateSfx_DummyPoly()
    return FUNC(INT, 0x00D64140, {});
end
---]]

--[[
function CroseBriefingMsg()
    return FUNC(INT, 0x00D5E400, {});
end
---]]

--[[
function CustomLuaCall()
    return FUNC(INT, 0x00D62C30, {});
end
---]]

--[[
function CustomLuaCallStart()
    return FUNC(INT, 0x00D66240, {});
end
---]]

--[[
function CustomLuaCallStartPlus()
    return FUNC(INT, 0x00D66210, {});
end
---]]

--[[
function DeleteCamSfx()
    return FUNC(INT, 0x00D5E8A0, {});
end
---]]

--[[
function DeleteEvent()
    return FUNC(INT, 0x00D5EAA0, {});
end
---]]

--[[
function DeleteObjSfxAll()
    return FUNC(INT, 0x00D5E7B0, {});
end
---]]

--[[
function DeleteObjSfxDmyPlyID()
    return FUNC(INT, 0x00D5E7F0, {});
end
---]]

--[[
function DeleteObjSfxEventID()
    return FUNC(INT, 0x00D5E7D0, {});
end
---]]

--[[
function DisableCollection()
    return FUNC(INT, 0x00D60A40, {});
end
---]]

--[[
function DisableDamage()
    return FUNC(INT, 0x00D60DD0, {});
end
---]]

--[[
function DisableHpGauge()
    return FUNC(INT, 0x00D60A00, {});
end
---]]

--[[
function DisableInterupt()
    return FUNC(INT, 0x00D612B0, {});
end
---]]

--[[
function DisableMapHit()
    return FUNC(INT, 0x00D61790, {});
end
---]]

--[[
function DisableMove()
    return FUNC(INT, 0x00D617D0, {});
end
---]]

--[[
function DivideRest()
    return FUNC(INT, 0x00490520, {});
end
---]]

--[[
function EnableAction()
    return FUNC(INT, 0x00D5FD60, {});
end
---]]

--[[
function EnableGeneratorSystem()
    return FUNC(INT, 0x00D5EF20, {});
end
---]]

--[[
function EnableHide()
    return FUNC(INT, 0x00D60E00, {});
end
---]]

--[[
function EnableInvincible()
    return FUNC(INT, 0x00D60D90, {});
end
---]]

--[[
function EnableLogic()
    return FUNC(INT, 0x00D5FD90, {});
end
---]]

--[[
function EnableObjTreasure()
    return FUNC(INT, 0x00D661D0, {});
end
---]]

--[[
function EndAnimation()
    return FUNC(INT, 0x00D61BF0, {});
end
---]]

--[[
function EraseEventSpecialEffect()
    return FUNC(INT, 0x00D61210, {});
end
---]]

--[[
function EraseEventSpecialEffect_2()
    return FUNC(INT, 0x00D61170, {});
end
---]]

--[[
function EventTagInsertString_forPlayerNo()
    return FUNC(INT, 0x00D5E820, {});
end
---]]

--[[
function ExcutePenalty()
    return FUNC(INT, 0x00D5EBE0, {});
end
---]]

--[[
function ForceChangeTarget()
    return FUNC(INT, 0x00D61E00, {});
end
---]]

--[[
function ForceDead()
    return FUNC(INT, 0x00D66690, {});
end
---]]

--[[
function ForcePlayAnimation()
    return FUNC(INT, 0x00D61CF0, {});
end
---]]

--[[
function ForcePlayAnimationStayCancel()
    return FUNC(INT, 0x00D61CA0, {});
end
---]]

--[[
function ForcePlayLoopAnimation()
    return FUNC(INT, 0x00D61C50, {});
end
---]]

--[[
function ForceSetOmissionLevel()
    return FUNC(INT, 0x00D60CB0, {});
end
---]]

--[[
function ForceUpdateNextFrame()
    return FUNC(INT, 0x00D60C90, {});
end
---]]

--[[
function GetBountyRankPoint()
    return FUNC(INT, 0x00D60130, {});
end
---]]

--[[
function GetClearBonus()
    return FUNC(INT, 0x00D5DEB0, {});
end
---]]

--[[
function GetClearCount()
    return FUNC(INT, 0x00D5E5A0, {});
end
---]]

--[[
function GetClearState()
    return FUNC(INT, 0x00D5E580, {});
end
---]]

--[[
function GetCurrentMapAreaNo()
    return FUNC(INT, 0x00D5F650, {});
end
---]]

--[[
function GetCurrentMapBlockNo()
    return FUNC(INT, 0x00D5F620, {});
end
---]]

--[[
function GetDeathState()
    return FUNC(INT, 0x00D5D800, {});
end
---]]

--[[
function GetDistance()
    return FUNC(INT, 0x00D61B70, {});
end
---]]

--[[
function GetEnemyPlayerId_Random()
    return FUNC(INT, 0x00D5ED10, {});
end
---]]

--[[
function GetEventFlagValue()
    return FUNC(INT, 0x00D60340, {});
end
---]]

--[[
function GetEventGoalState()
    return FUNC(INT, 0x00D612F0, {});
end
---]]

--[[
function GetEventMode()
    return FUNC(INT, 0x00D61690, {});
end
---]]

--[[
function GetEventRequest()
    return FUNC(INT, 0x00D610D0, {});
end
---]]

--[[
function GetFloorMaterial()
    return FUNC(INT, 0x00D60480, {});
end
---]]

--[[
function GetGlobalQWC()
    return FUNC(INT, 0x00D5F850, {});
end
---]]

--[[
function GetHeroPoint()
    return FUNC(INT, 0x00D5DC50, {});
end
---]]

--[[
function GetHostPlayerNo()
    return FUNC(INT, 0x00D5E260, {});
end
---]]

--[[
function GetHp()
    return FUNC(INT, 0x00D5FA80, {});
end
---]]

--[[
function GetHpRate()
    return FUNC(INT, 0x00D5FA30, {});
end
---]]

--[[
function GetItem()
    return FUNC(INT, 0x00D5E240, {});
end
---]]

--[[
function GetLadderCount()
    return FUNC(INT, 0x00D64580, {});
end
---]]

--[[
function GetLastBlockId()
    return FUNC(INT, 0x00D5EFC0, {});
end
---]]

--[[
function GetLocalPlayerChrType()
    return FUNC(INT, 0x00D5FB70, {});
end
---]]

--[[
function GetLocalPlayerId()
    return FUNC(INT, 0x00D5E270, {});
end
---]]

--[[
function GetLocalPlayerInvadeType()
    return FUNC(INT, 0x00D5FB50, {});
end
---]]

--[[
function GetLocalPlayerSoulLv()
    return FUNC(INT, 0x00D5EB30, {});
end
---]]

--[[
function GetLocalPlayerVowType()
    return FUNC(INT, 0x00D5FB20, {});
end
---]]

--[[
function GetLocalQWC()
    return FUNC(INT, 0x00D5F870, {});
end
---]]

--[[
function GetMultiWallNum()
    return FUNC(INT, 0x00D5DC40, {});
end
---]]

--[[
function GetNetPlayerChrType()
    return FUNC(INT, 0x00D5E8C0, {});
end
---]]

--[[
function GetObjHp()
    return FUNC(INT, 0x00D5F320, {});
end
---]]

--[[
function GetParam()
    return FUNC(INT, 0x00CD9FF0, {});
end
---]]

--[[
function GetParam1()
    return FUNC(INT, 0x00403B00, {});
end
---]]

--[[
function GetParam2()
    return FUNC(INT, 0x00B1CD50, {});
end
---]]

--[[
function GetParam3()
    return FUNC(INT, 0x00788EE0, {});
end
---]]

--[[
function GetPartyMemberNum_InvadeType()
    return FUNC(INT, 0x00D5EBA0, {});
end
---]]

--[[
function GetPartyMemberNum_VowType()
    return FUNC(INT, 0x00D5F3A0, {});
end
---]]

--[[
function GetPlayerId_Random()
    return FUNC(INT, 0x00D5ED30, {});
end
---]]

--[[
function GetPlayerNo_LotNitoMultiItem()
    return FUNC(INT, 0x00D64100, {});
end
---]]

--[[
function GetPlayID()
    return FUNC(INT, 0x00B0E260, {});
end
---]]

--[[
function GetQWC()
    return FUNC(INT, 0x00D5F8C0, {});
end
---]]

--[[
function GetRandom()
    return FUNC(INT, 0x00D5E0F0, {});
end
---]]

--[[
function GetRateItem()
    return FUNC(INT, 0x00D665B0, {});
end
---]]

--[[
function GetRateItem_IgnoreMultiPlay()
    return FUNC(INT, 0x00D66590, {});
end
---]]

--[[
function GetReturnState()
    return FUNC(INT, 0x00D5E230, {});
end
---]]

--[[
function GetRightCurrentWeaponId()
    return FUNC(INT, 0x00D5EF80, {});
end
---]]

--[[
function GetSoloClearBonus()
    return FUNC(INT, 0x00D5DE60, {});
end
---]]

--[[
function GetSummonAnimId()
    return FUNC(INT, 0x00D60890, {});
end
---]]

--[[
function GetSummonBlackResult()
    return FUNC(INT, 0x00D5E530, {});
end
---]]

--[[
function GetTargetChrID()
    return FUNC(INT, 0x00D64160, {});
end
---]]

--[[
function GetTempSummonParam()
    return FUNC(INT, 0x00D5DDE0, {});
end
---]]

--[[
function GetTravelItemParamId()
    return FUNC(INT, 0x00D60440, {});
end
---]]

--[[
function GetWhiteGhostCount()
    return FUNC(INT, 0x00D5E3D0, {});
end
---]]

--[[
function HasSuppleItem()
    return FUNC(INT, 0x00D60430, {});
end
---]]

--[[
function HavePartyMember()
    return FUNC(INT, 0x00D5E1D0, {});
end
---]]

--[[
function HoverMoveVal()
    return FUNC(INT, 0x00D62C00, {});
end
---]]

--[[
function HoverMoveValDmy()
    return FUNC(INT, 0x00D648E0, {});
end
---]]

--[[
function IncrementCoopPlaySuccessCount()
    return FUNC(INT, 0x00D5F1D0, {});
end
---]]

--[[
function IncrementThiefInvadePlaySuccessCount()
    return FUNC(INT, 0x00D5F1A0, {});
end
---]]

--[[
function InfomationMenu()
    return FUNC(INT, 0x00D64110, {});
end
---]]

--[[
function InitDeathState()
    return FUNC(INT, 0x00D5D7E0, {});
end
---]]

--[[
function InvalidMyBloodMarkInfo()
    return FUNC(INT, 0x00D60E50, {});
end
---]]

--[[
function InvalidMyBloodMarkInfo_Tutorial()
    return FUNC(INT, 0x00D60E30, {});
end
---]]

--[[
function InvalidPointLight()
    return FUNC(INT, 0x00D5F060, {});
end
---]]

--[[
function InvalidSfx()
    return FUNC(INT, 0x00D5F780, {});
end
---]]

--[[
function IsAction()
    return FUNC(INT, 0x00D5FAB0, {});
end
---]]

--[[
function IsAlive()
    return FUNC(INT, 0x00D615E0, {});
end
---]]

--[[
function IsAliveMotion()
    return FUNC(INT, 0x00D5E6A0, {});
end
---]]

--[[
function IsAngle()
    return FUNC(INT, 0x00D61B30, {});
end
---]]

--[[
function IsAnglePlus()
    return FUNC(INT, 0x00D61AF0, {});
end
---]]

--[[
function IsAppearancePlayer()
    return FUNC(INT, 0x00D5F170, {});
end
---]]

--[[
function IsBlackGhost()
    return FUNC(INT, 0x00D60EF0, {});
end
---]]

--[[
function IsBlackGhost_NetPlayer()
    return FUNC(INT, 0x00D5E8F0, {});
end
---]]

--[[
function IsClearItem()
    return FUNC(INT, 0x00D5DCF0, {});
end
---]]

--[[
function IsClient()
    return FUNC(INT, 0x00D5E2E0, {});
end
---]]

--[[
function IsColiseumGhost()
    return FUNC(INT, 0x00D60EB0, {});
end
---]]

--[[
function IsCompleteEvent()
    return FUNC(INT, 0x00D60170, {});
end
---]]

--[[
function IsCompleteEventValue()
    return FUNC(INT, 0x00D60150, {});
end
---]]

--[[
function IsDead_NextGreyGhost()
    return FUNC(INT, 0x00D5E160, {});
end
---]]

--[[
function IsDeathPenaltySkip()
    return FUNC(INT, 0x00D5D7D0, {});
end
---]]

--[[
function IsDestroyed()
    return FUNC(INT, 0x00D5F0E0, {});
end
---]]

--[[
function IsDisable()
    return FUNC(INT, 0x00D60F70, {});
end
---]]

--[[
function IsDistance()
    return FUNC(INT, 0x00D61BA0, {});
end
---]]

--[[
function IsDropCheck_Only()
    return FUNC(INT, 0x00D60C00, {});
end
---]]

--[[
function IsEquip()
    return FUNC(INT, 0x00D5F4F0, {});
end
---]]

--[[
function IsEventAnim()
    return FUNC(INT, 0x00D61C30, {});
end
---]]

--[[
function IsFireDead()
    return FUNC(INT, 0x00D60400, {});
end
---]]

--[[
function IsForceSummoned()
    return FUNC(INT, 0x00D5EC00, {});
end
---]]

--[[
function IsGameClient()
    return FUNC(INT, 0x00D5EAF0, {});
end
---]]

--[[
function IsGreyGhost()
    return FUNC(INT, 0x00D60F30, {});
end
---]]

--[[
function IsGreyGhost_NetPlayer()
    return FUNC(INT, 0x00D5E970, {});
end
---]]

--[[
function IsHost()
    return FUNC(INT, 0x00D5E300, {});
end
---]]

--[[
function IsInParty()
    return FUNC(INT, 0x00D5E1F0, {});
end
---]]

--[[
function IsInParty_EnemyMember()
    return FUNC(INT, 0x00D5EEC0, {});
end
---]]

--[[
function IsInParty_FriendMember()
    return FUNC(INT, 0x00D5E440, {});
end
---]]

--[[
function IsIntruder()
    return FUNC(INT, 0x00D60ED0, {});
end
---]]

--[[
function IsInventoryEquip()
    return FUNC(INT, 0x00D5EF40, {});
end
---]]

--[[
function IsJobType()
    return FUNC(INT, 0x00D5E040, {});
end
---]]

--[[
function IsLand()
    return FUNC(INT, 0x00D60450, {});
end
---]]

--[[
function IsLiveNetPlayer()
    return FUNC(INT, 0x00D5E9B0, {});
end
---]]

--[[
function IsLivePlayer()
    return FUNC(INT, 0x00D60F50, {});
end
---]]

--[[
function IsLoadWait()
    return FUNC(INT, 0x00D5D930, {});
end
---]]

--[[
function IsMatchingMultiPlay()
    return FUNC(INT, 0x00D5ED80, {});
end
---]]

--[[
function IsOnline()
    return FUNC(INT, 0x00D5E2D0, {});
end
---]]

--[[
function IsOnlineMode()
    return FUNC(INT, 0x00D5DD50, {});
end
---]]

--[[
function IsPlayerAssessMenu_Tutorial()
    return FUNC(INT, 0x00D5E010, {});
end
---]]

--[[
function IsPlayerStay()
    return FUNC(INT, 0x00D61610, {});
end
---]]

--[[
function IsPlayMovie()
    return FUNC(INT, 0x00D60AF0, {});
end
---]]

--[[
function IsPrevGreyGhost()
    return FUNC(INT, 0x00D5E140, {});
end
---]]

--[[
function IsProcessEventGoal()
    return FUNC(INT, 0x00D61340, {});
end
---]]

--[[
function IsReady_Obj()
    return FUNC(INT, 0x00D5F370, {});
end
---]]

--[[
function IsRegionDrop()
    return FUNC(INT, 0x00D60C30, {});
end
---]]

--[[
function IsRegionIn()
    return FUNC(INT, 0x00D61020, {});
end
---]]

--[[
function IsRevengeRequested()
    return FUNC(INT, 0x00D5E2A0, {});
end
---]]

--[[
function IsReviveWait()
    return FUNC(INT, 0x00D5E660, {});
end
---]]

--[[
function IsShow_CampMenu()
    return FUNC(INT, 0x00D5DA20, {});
end
---]]

--[[
function IsShowMenu()
    return FUNC(INT, 0x00D5EA90, {});
end
---]]

--[[
function IsShowMenu_BriefingMsg()
    return FUNC(INT, 0x00D5DD70, {});
end
---]]

--[[
function IsShowMenu_GenDialog()
    return FUNC(INT, 0x00D5EA80, {});
end
---]]

--[[
function IsShowMenu_InfoMenu()
    return FUNC(INT, 0x00D5DB80, {});
end
---]]

--[[
function IsShowSosMsg_Tutorial()
    return FUNC(INT, 0x00D5E000, {});
end
---]]

--[[
function IsSuccessQWC()
    return FUNC(INT, 0x00D5F7D0, {});
end
---]]

--[[
function IsTryJoinSession()
    return FUNC(INT, 0x00D5E2B0, {});
end
---]]

--[[
function IsValidInstance()
    return FUNC(INT, 0x00D5F510, {});
end
---]]

--[[
function IsValidTalk()
    return FUNC(INT, 0x00D5E5F0, {});
end
---]]

--[[
function IsWhiteGhost()
    return FUNC(INT, 0x00D60F10, {});
end
---]]

--[[
function IsWhiteGhost_NetPlayer()
    return FUNC(INT, 0x00D5E930, {});
end
---]]

--[[
function LeaveSession()
    return FUNC(INT, 0x00D62900, {});
end
---]]

--[[
function LockSession()
    return FUNC(INT, 0x00D5EE60, {});
end
---]]

--[[
function LuaCall()
    return FUNC(INT, 0x00D62C60, {});
end
---]]

--[[
function LuaCallStart()
    return FUNC(INT, 0x00D66290, {});
end
---]]

--[[
function LuaCallStartPlus()
    return FUNC(INT, 0x00D66260, {});
end
---]]

--[[
function MultiDoping_AllEventBody()
    return FUNC(INT, 0x00D61D60, {});
end
---]]

--[[
function NoAnimeTurnCharactor()
    return FUNC(INT, 0x00D61480, {});
end
---]]

--[[
function NotNetMessage_begin()
    return FUNC(INT, 0x00D5DBE0, {});
end
---]]

--[[
function NotNetMessage_end()
    return FUNC(INT, 0x00D5DBD0, {});
end
---]]

--[[
function ObjRootMtxMove()
    return FUNC(INT, 0x00D647A0, {});
end
---]]

--[[
function ObjRootMtxMoveByChrDmyPoly()
    return FUNC(INT, 0x00D64760, {});
end
---]]

--[[
function ObjRootMtxMoveDmyPoly()
    return FUNC(INT, 0x00D64780, {});
end
---]]

--[[
function OnActionCheckKey()
    return FUNC(INT, 0x00D63B70, {});
end
---]]

--[[
function OnActionEventRegion()
    return FUNC(INT, 0x00D63C70, {});
end
---]]

--[[
function OnActionEventRegionAttribute()
    return FUNC(INT, 0x00D63BE0, {});
end
---]]

--[[
function OnBallista()
    return FUNC(INT, 0x00D5E290, {});
end
---]]

--[[
function OnBloodMenuClose()
    return FUNC(INT, 0x00D62FF0, {});
end
---]]

--[[
function OnBonfireEvent()
    return FUNC(INT, 0x00D64B40, {});
end
---]]

--[[
function OnCharacterAnimEnd()
    return FUNC(INT, 0x00D63860, {});
end
---]]

--[[
function OnCharacterDead()
    return FUNC(INT, 0x00D63AB0, {});
end
---]]

--[[
function OnCharacterHP()
    return FUNC(INT, 0x00D63A50, {});
end
---]]

--[[
function OnCharacterHP_CheckAttacker()
    return FUNC(INT, 0x00D639E0, {});
end
---]]

--[[
function OnCharacterHpRate()
    return FUNC(INT, 0x00D63980, {});
end
---]]

--[[
function OnCharacterTotalDamage()
    return FUNC(INT, 0x00D63750, {});
end
---]]

--[[
function OnCharacterTotalRateDamage()
    return FUNC(INT, 0x00D636C0, {});
end
---]]

--[[
function OnCheckEzStateMessage()
    return FUNC(INT, 0x00D63B10, {});
end
---]]

--[[
function OnChrAnimEnd()
    return FUNC(INT, 0x00D64570, {});
end
---]]

--[[
function OnChrAnimEndPlus()
    return FUNC(INT, 0x00D637D0, {});
end
---]]

--[[
function OnDistanceAction()
    return FUNC(INT, 0x00D65240, {});
end
---]]

--[[
function OnDistanceActionAttribute()
    return FUNC(INT, 0x00D65190, {});
end
---]]

--[[
function OnDistanceActionDmyPoly()
    return FUNC(INT, 0x00D64F70, {});
end
---]]

--[[
function OnDistanceActionPlus()
    return FUNC(INT, 0x00D650D0, {});
end
---]]

--[[
function OnDistanceActionPlusAttribute()
    return FUNC(INT, 0x00D65010, {});
end
---]]

--[[
function OnDistanceJustIn()
    return FUNC(INT, 0x00D64E50, {});
end
---]]

--[[
function OnEndFlow()
    return FUNC(INT, 0x00D62990, {});
end
---]]

--[[
function OnFireDamage()
    return FUNC(INT, 0x00D634B0, {});
end
---]]

--[[
function OnKeyTime2()
    return FUNC(INT, 0x00D635D0, {});
end
---]]

--[[
function OnNetDistanceIn()
    return FUNC(INT, 0x00D62D40, {});
end
---]]

--[[
function OnNetRegion()
    return FUNC(INT, 0x00D625D0, {});
end
---]]

--[[
function OnNetRegionAttr()
    return FUNC(INT, 0x00D625A0, {});
end
---]]

--[[
function OnNetRegionAttrPlus()
    return FUNC(INT, 0x00D62DA0, {});
end
---]]

--[[
function OnNetRegionPlus()
    return FUNC(INT, 0x00D62E00, {});
end
---]]

--[[
function OnObjAnimEnd()
    return FUNC(INT, 0x00D64560, {});
end
---]]

--[[
function OnObjAnimEndPlus()
    return FUNC(INT, 0x00D64550, {});
end
---]]

--[[
function OnObjDestroy()
    return FUNC(INT, 0x00D63900, {});
end
---]]

--[[
function OnObjectDamageHit()
    return FUNC(INT, 0x00D659A0, {});
end
---]]

--[[
function OnObjectDamageHit_NoCall()
    return FUNC(INT, 0x00D65940, {});
end
---]]

--[[
function OnObjectDamageHit_NoCallPlus()
    return FUNC(INT, 0x00D658E0, {});
end
---]]

--[[
function OnPlayerActionInRegion()
    return FUNC(INT, 0x00D63EB0, {});
end
---]]

--[[
function OnPlayerActionInRegionAngle()
    return FUNC(INT, 0x00D63D90, {});
end
---]]

--[[
function OnPlayerActionInRegionAngleAttribute()
    return FUNC(INT, 0x00D63D00, {});
end
---]]

--[[
function OnPlayerActionInRegionAttribute()
    return FUNC(INT, 0x00D63E20, {});
end
---]]

--[[
function OnPlayerAssessMenu()
    return FUNC(INT, 0x00D629E0, {});
end
---]]

--[[
function OnPlayerDistanceAngleInTarget()
    return FUNC(INT, 0x00D652F0, {});
end
---]]

--[[
function OnPlayerDistanceInTarget()
    return FUNC(INT, 0x00D65380, {});
end
---]]

--[[
function OnPlayerDistanceOut()
    return FUNC(INT, 0x00D64EE0, {});
end
---]]

--[[
function OnPlayerKill()
    return FUNC(INT, 0x00D63430, {});
end
---]]

--[[
function OnRegionIn()
    return FUNC(INT, 0x00D64050, {});
end
---]]

--[[
function OnRegionJustIn()
    return FUNC(INT, 0x00D63FC0, {});
end
---]]

--[[
function OnRegionJustOut()
    return FUNC(INT, 0x00D63F40, {});
end
---]]

--[[
function OnRegistFunc()
    return FUNC(INT, 0x00D63050, {});
end
---]]

--[[
function OnRequestMenuEnd()
    return FUNC(INT, 0x00D62670, {});
end
---]]

--[[
function OnRevengeMenuClose()
    return FUNC(INT, 0x00D64AC0, {});
end
---]]

--[[
function OnSelectMenu()
    return FUNC(INT, 0x00D62EF0, {});
end
---]]

--[[
function OnSelfBloodMark()
    return FUNC(INT, 0x00D633B0, {});
end
---]]

--[[
function OnSelfHeroBloodMark()
    return FUNC(INT, 0x00D63330, {});
end
---]]

--[[
function OnSessionIn()
    return FUNC(INT, 0x00D63230, {});
end
---]]

--[[
function OnSessionInfo()
    return FUNC(INT, 0x00D630E0, {});
end
---]]

--[[
function OnSessionJustIn()
    return FUNC(INT, 0x00D632B0, {});
end
---]]

--[[
function OnSessionJustOut()
    return FUNC(INT, 0x00D631B0, {});
end
---]]

--[[
function OnSessionOut()
    return FUNC(INT, 0x00D63130, {});
end
---]]

--[[
function OnSimpleDamage()
    return FUNC(INT, 0x00D63540, {});
end
---]]

--[[
function OnTalkEvent()
    return FUNC(INT, 0x00D65BB0, {});
end
---]]

--[[
function OnTalkEventAngleOut()
    return FUNC(INT, 0x00D65A20, {});
end
---]]

--[[
function OnTalkEventDistIn()
    return FUNC(INT, 0x00D65B30, {});
end
---]]

--[[
function OnTalkEventDistOut()
    return FUNC(INT, 0x00D65AB0, {});
end
---]]

--[[
function OnTestEffectEndPlus()
    return FUNC(INT, 0x00D626C0, {});
end
---]]

--[[
function OnTextEffectEnd()
    return FUNC(INT, 0x00D62720, {});
end
---]]

--[[
function OnTurnCharactorEnd()
    return FUNC(INT, 0x00D62A30, {});
end
---]]

--[[
function OnWanderFade()
    return FUNC(INT, 0x00D622D0, {});
end
---]]

--[[
function OnWanderingDemon()
    return FUNC(INT, 0x00D62240, {});
end
---]]

--[[
function OnWarpMenuClose()
    return FUNC(INT, 0x00D62F90, {});
end
---]]

--[[
function OnYesNoDialog()
    return FUNC(INT, 0x00D62E60, {});
end
---]]

--[[
function OpenCampMenu()
    return FUNC(INT, 0x00D5DA80, {});
end
---]]

--[[
function OpeningDead()
    return FUNC(INT, 0x00D66600, {});
end
---]]

--[[
function OpeningDeadPlus()
    return FUNC(INT, 0x00D665D0, {});
end
---]]

--[[
function OpenSOSMsg_Tutorial()
    return FUNC(INT, 0x00D5E750, {});
end
---]]

--[[
function ParamInitialize()
    return FUNC(INT, 0x00D66360, {});
end
---]]

--[[
function PauseTutorial()
    return FUNC(INT, 0x00D5E0B0, {});
end
---]]

--[[
function PlayAnimation()
    return FUNC(INT, 0x00D61D10, {});
end
---]]

--[[
function PlayAnimationStayCancel()
    return FUNC(INT, 0x00D61CC0, {});
end
---]]

--[[
function PlayerChrResetAnimation_RemoOnly()
    return FUNC(INT, 0x00D60630, {});
end
---]]

--[[
function PlayLoopAnimation()
    return FUNC(INT, 0x00D61C70, {});
end
---]]

--[[
function PlayObjectSE()
    return FUNC(INT, 0x00D61660, {});
end
---]]

--[[
function PlayPointSE()
    return FUNC(INT, 0x00D61F00, {});
end
---]]

--[[
function PlayTypeSE()
    return FUNC(INT, 0x00D61630, {});
end
---]]

--[[
function RecallMenuEvent()
    return FUNC(INT, 0x00D62780, {});
end
---]]

--[[
function ReconstructBreak()
    return FUNC(INT, 0x00D66070, {});
end
---]]

--[[
function RecoveryHeroin()
    return FUNC(INT, 0x00D60740, {});
end
---]]

--[[
function RegistObjAct()
    return FUNC(INT, 0x00D5D870, {});
end
---]]

--[[
function RegistSimpleTalk()
    return FUNC(INT, 0x00D628A0, {});
end
---]]

--[[
function RemoveInventoryEquip()
    return FUNC(INT, 0x00D60C70, {});
end
---]]

--[[
function RepeatMessage_begin()
    return FUNC(INT, 0x00D5DBC0, {});
end
---]]

--[[
function RepeatMessage_end()
    return FUNC(INT, 0x00D5DBB0, {});
end
---]]

--[[
function RequestEnding()
    return FUNC(INT, 0x00D5DDD0, {});
end
---]]

--[[
function RequestForceUpdateNetwork()
    return FUNC(INT, 0x00D60AA0, {});
end
---]]

--[[
function RequestFullRecover()
    return FUNC(INT, 0x00D5DD40, {});
end
---]]

--[[
function RequestGenerate()
    return FUNC(INT, 0x00D5F290, {});
end
---]]

--[[
function RequestNormalUpdateNetwork()
    return FUNC(INT, 0x00D60A70, {});
end
---]]

--[[
function RequestOpenBriefingMsg()
    return FUNC(INT, 0x00D5EB50, {});
end
---]]

--[[
function RequestOpenBriefingMsgPlus()
    return FUNC(INT, 0x00D62260, {});
end
---]]

--[[
function RequestPlayMovie()
    return FUNC(INT, 0x00D65750, {});
end
---]]

--[[
function RequestPlayMoviePlus()
    return FUNC(INT, 0x00D656D0, {});
end
---]]

--[[
function RequestRemo()
    return FUNC(INT, 0x00D66440, {});
end
---]]

--[[
function RequestRemoPlus()
    return FUNC(INT, 0x00D663E0, {});
end
---]]

--[[
function RequestUnlockTrophy()
    return FUNC(INT, 0x00D5EBB0, {});
end
---]]

--[[
function ReqularLeavePlayer()
    return FUNC(INT, 0x00D5E3E0, {});
end
---]]

--[[
function ResetCamAngle()
    return FUNC(INT, 0x00D5EB20, {});
end
---]]

--[[
function ResetEventQwcSpEffect()
    return FUNC(INT, 0x00D61ED0, {});
end
---]]

--[[
function ResetSummonParam()
    return FUNC(INT, 0x00D5EB80, {});
end
---]]

--[[
function ResetSyncRideObjInfo()
    return FUNC(INT, 0x00D60500, {});
end
---]]

--[[
function ResetThink()
    return FUNC(INT, 0x00D61070, {});
end
---]]

--[[
function RestorePiece()
    return FUNC(INT, 0x00D64C40, {});
end
---]]

--[[
function ReturnMapSelect()
    return FUNC(INT, 0x00D5E1C0, {});
end
---]]

--[[
function RevivePlayer()
    return FUNC(INT, 0x00D645E0, {});
end
---]]

--[[
function RevivePlayerNext()
    return FUNC(INT, 0x00D5E0A0, {});
end
---]]

--[[
function SaveRequest()
    return FUNC(INT, 0x00D5EE90, {});
end
---]]

--[[
function SaveRequest_Profile()
    return FUNC(INT, 0x00D5EE70, {});
end
---]]

--[[
function SendEventRequest()
    return FUNC(INT, 0x00D61130, {});
end
---]]

--[[
function SetAlive()
    return FUNC(INT, 0x00D664A0, {});
end
---]]

--[[
function SetAliveMotion()
    return FUNC(INT, 0x00D5E6C0, {});
end
---]]

--[[
function SetAlwaysDrawForEvent()
    return FUNC(INT, 0x00D60570, {});
end
---]]

--[[
function SetAlwaysEnableBackread_forEvent()
    return FUNC(INT, 0x00D60750, {});
end
---]]

--[[
function SetAngleFoward()
    return FUNC(INT, 0x00D61A80, {});
end
---]]

--[[
function SetAreaStartMapUid()
    return FUNC(INT, 0x00D5F720, {});
end
---]]

--[[
function SetBossGauge()
    return FUNC(INT, 0x00D60700, {});
end
---]]

--[[
function SetBossUnitJrHit()
    return FUNC(INT, 0x00D61710, {});
end
---]]

--[[
function SetBountyRankPoint()
    return FUNC(INT, 0x00D64E00, {});
end
---]]

--[[
function SetBrokenPiece()
    return FUNC(INT, 0x00D5F7A0, {});
end
---]]

--[[
function SetCamModeParamTargetId()
    return FUNC(INT, 0x00D5DF00, {});
end
---]]

--[[
function SetCamModeParamTargetIdForBossLock()
    return FUNC(INT, 0x00D5DEF0, {});
end
---]]

--[[
function SetChrType()
    return FUNC(INT, 0x00D60E70, {});
end
---]]

--[[
function SetChrTypeDataGrey()
    return FUNC(INT, 0x00D5DFB0, {});
end
---]]

--[[
function SetChrTypeDataGreyNext()
    return FUNC(INT, 0x00D5DF50, {});
end
---]]

--[[
function SetClearBonus()
    return FUNC(INT, 0x00D5F3D0, {});
end
---]]

--[[
function SetClearItem()
    return FUNC(INT, 0x00D5DCD0, {});
end
---]]

--[[
function SetClearSesiionCount()
    return FUNC(INT, 0x00D5E4C0, {});
end
---]]

--[[
function SetClearState()
    return FUNC(INT, 0x00D5E560, {});
end
---]]

--[[
function SetColiEnable()
    return FUNC(INT, 0x00D60090, {});
end
---]]

--[[
function SetColiEnableArray()
    return FUNC(INT, 0x00D64AA0, {});
end
---]]

--[[
function SetCompletelyNoMove()
    return FUNC(INT, 0x00D604E0, {});
end
---]]

--[[
function SetDeadMode()
    return FUNC(INT, 0x00D61430, {});
end
---]]

--[[
function SetDeadMode2()
    return FUNC(INT, 0x00D613F0, {});
end
---]]

--[[
function SetDefaultAnimation()
    return FUNC(INT, 0x00D5F110, {});
end
---]]

--[[
function SetDefaultMapUid()
    return FUNC(INT, 0x00D5F680, {});
end
---]]

--[[
function SetDefaultRoutePoint()
    return FUNC(INT, 0x00D645A0, {});
end
---]]

--[[
function SetDisable()
    return FUNC(INT, 0x00D60FC0, {});
end
---]]

--[[
function SetDisableBackread_forEvent()
    return FUNC(INT, 0x00D60AD0, {});
end
---]]

--[[
function SetDisableDamage()
    return FUNC(INT, 0x00D5EC30, {});
end
---]]

--[[
function SetDisableGravity()
    return FUNC(INT, 0x00D610A0, {});
end
---]]

--[[
function SetDisableWeakDamageAnim()
    return FUNC(INT, 0x00D603D0, {});
end
---]]

--[[
function SetDisableWeakDamageAnim_light()
    return FUNC(INT, 0x00D60390, {});
end
---]]

--[[
function SetDispMask()
    return FUNC(INT, 0x00D61800, {});
end
---]]

--[[
function SetDrawEnable()
    return FUNC(INT, 0x00D600B0, {});
end
---]]

--[[
function SetDrawEnableArray()
    return FUNC(INT, 0x00D64530, {});
end
---]]

--[[
function SetDrawGroup()
    return FUNC(INT, 0x00D61A00, {});
end
---]]

--[[
function SetEnableEventPad()
    return FUNC(INT, 0x00D5E420, {});
end
---]]

--[[
function SetEventBodyBulletCorrect()
    return FUNC(INT, 0x00D5E6E0, {});
end
---]]

--[[
function SetEventBodyMaterialSeAndSfx()
    return FUNC(INT, 0x00D5E710, {});
end
---]]

--[[
function SetEventBodyMaxHp()
    return FUNC(INT, 0x00D61D70, {});
end
---]]

--[[
function SetEventCommand()
    return FUNC(INT, 0x00D5FCB0, {});
end
---]]

--[[
function SetEventCommandIndex()
    return FUNC(INT, 0x00D5FC70, {});
end
---]]

--[[
function SetEventFlag()
    return FUNC(INT, 0x00D60190, {});
end
---]]

--[[
function SetEventFlagValue()
    return FUNC(INT, 0x00D60360, {});
end
---]]

--[[
function SetEventGenerate()
    return FUNC(INT, 0x00D604B0, {});
end
---]]

--[[
function SetEventMovePointType()
    return FUNC(INT, 0x00D60BB0, {});
end
---]]

--[[
function SetEventSimpleTalk()
    return FUNC(INT, 0x00D64BD0, {});
end
---]]

--[[
function SetEventSpecialEffect()
    return FUNC(INT, 0x00D61280, {});
end
---]]

--[[
function SetEventSpecialEffect_2()
    return FUNC(INT, 0x00D611C0, {});
end
---]]

--[[
function SetEventSpecialEffectOwner()
    return FUNC(INT, 0x00D61240, {});
end
---]]

--[[
function SetEventSpecialEffectOwner_2()
    return FUNC(INT, 0x00D61190, {});
end
---]]

--[[
function SetEventTarget()
    return FUNC(INT, 0x00D61AA0, {});
end
---]]

--[[
function SetExVelocity()
    return FUNC(INT, 0x00D61F30, {});
end
---]]

--[[
function SetFirstSpeed()
    return FUNC(INT, 0x00D61E90, {});
end
---]]

--[[
function SetFirstSpeedPlus()
    return FUNC(INT, 0x00D61E50, {});
end
---]]

--[[
function SetFlagInitState()
    return FUNC(INT, 0x00D5DBF0, {});
end
---]]

--[[
function SetFootIKInterpolateType()
    return FUNC(INT, 0x00D60310, {});
end
---]]

--[[
function SetForceJoinBlackRequest()
    return FUNC(INT, 0x00D65DF0, {});
end
---]]

--[[
function SetHitInfo()
    return FUNC(INT, 0x00D61990, {});
end
---]]

--[[
function SetHitMask()
    return FUNC(INT, 0x00D64920, {});
end
---]]

--[[
function SetHp()
    return FUNC(INT, 0x00D5F9A0, {});
end
---]]

--[[
function SetIgnoreHit()
    return FUNC(INT, 0x00D61750, {});
end
---]]

--[[
function SetInfomationPriority()
    return FUNC(INT, 0x00D5DB10, {});
end
---]]

--[[
function SetInsideBattleArea()
    return FUNC(INT, 0x00D61050, {});
end
---]]

--[[
function SetIsAnimPauseOnRemoPlayForEvent()
    return FUNC(INT, 0x00D5F300, {});
end
---]]

--[[
function SetKeepCommandIndex()
    return FUNC(INT, 0x00D624F0, {});
end
---]]

--[[
function SetLoadWait()
    return FUNC(INT, 0x00D5D940, {});
end
---]]

--[[
function SetLockActPntInvalidateMask()
    return FUNC(INT, 0x00D608F0, {});
end
---]]

--[[
function SetMapUid()
    return FUNC(INT, 0x00D64610, {});
end
---]]

--[[
function SetMaxHp()
    return FUNC(INT, 0x00D5F910, {});
end
---]]

--[[
function SetMenuBrake()
    return FUNC(INT, 0x00D5DB70, {});
end
---]]

--[[
function SetMiniBlockIndex()
    return FUNC(INT, 0x00D5F5F0, {});
end
---]]

--[[
function SetMovePoint()
    return FUNC(INT, 0x00D61390, {});
end
---]]

--[[
function SetMultiWallMapUid()
    return FUNC(INT, 0x00D5F200, {});
end
---]]

--[[
function SetNoNetSync()
    return FUNC(INT, 0x00D606D0, {});
end
---]]

--[[
function SetObjDeactivate()
    return FUNC(INT, 0x00D5F120, {});
end
---]]

--[[
function SetObjDisableBreak()
    return FUNC(INT, 0x00D5F340, {});
end
---]]

--[[
function SetObjEventCollisionFill()
    return FUNC(INT, 0x00D5FF80, {});
end
---]]

--[[
function SetObjSfx()
    return FUNC(INT, 0x00D62320, {});
end
---]]

--[[
function SetReturnPointEntityId()
    return FUNC(INT, 0x00D5DC70, {});
end
---]]

--[[
function SetReviveWait()
    return FUNC(INT, 0x00D5E680, {});
end
---]]

--[[
function SetSelfBloodMapUid()
    return FUNC(INT, 0x00D5FFF0, {});
end
---]]

--[[
function SetSosSignPos()
    return FUNC(INT, 0x00D5F570, {});
end
---]]

--[[
function SetSosSignWarp()
    return FUNC(INT, 0x00D5E070, {});
end
---]]

--[[
function SetSpStayAndDamageAnimId()
    return FUNC(INT, 0x00D609C0, {});
end
---]]

--[[
function SetSpStayAndDamageAnimIdPlus()
    return FUNC(INT, 0x00D60980, {});
end
---]]

--[[
function SetSubMenuBrake()
    return FUNC(INT, 0x00D5D880, {});
end
---]]

--[[
function SetSummonedPos()
    return FUNC(INT, 0x00D5E090, {});
end
---]]

--[[
function SetSyncRideObjInfo()
    return FUNC(INT, 0x00D60520, {});
end
---]]

--[[
function SetSystemIgnore()
    return FUNC(INT, 0x00D605A0, {});
end
---]]

--[[
function SetTalkMsg()
    return FUNC(INT, 0x00D5E190, {});
end
---]]

--[[
function SetTeamType()
    return FUNC(INT, 0x00D60B50, {});
end
---]]

--[[
function SetTeamTypeDefault()
    return FUNC(INT, 0x00D60B00, {});
end
---]]

--[[
function SetTeamTypePlus()
    return FUNC(INT, 0x00D60B30, {});
end
---]]

--[[
function SetTextEffect()
    return FUNC(INT, 0x00D5E430, {});
end
---]]

--[[
function SetTutorialSummonedPos()
    return FUNC(INT, 0x00D5E080, {});
end
---]]

--[[
function SetValidTalk()
    return FUNC(INT, 0x00D5E5C0, {});
end
---]]

--[[
function ShowGenDialog()
    return FUNC(INT, 0x00D5EEF0, {});
end
---]]

--[[
function ShowRankingDialog()
    return FUNC(INT, 0x00D5D960, {});
end
---]]

--[[
function SOSMsgGetResult_Tutorial()
    return FUNC(INT, 0x00D5DA50, {});
end
---]]

--[[
function StopLoopAnimation()
    return FUNC(INT, 0x00D5FFC0, {});
end
---]]

--[[
function StopPlayer()
    return FUNC(INT, 0x00D60950, {});
end
---]]

--[[
function StopPointSE()
    return FUNC(INT, 0x00D5EA60, {});
end
---]]

--[[
function SubActionCount()
    return FUNC(INT, 0x00D5FBC0, {});
end
---]]

--[[
function SubDispMaskByBit()
    return FUNC(INT, 0x00D5FEA0, {});
end
---]]

--[[
function SubHitMask()
    return FUNC(INT, 0x00D64CA0, {});
end
---]]

--[[
function SubHitMaskByBit()
    return FUNC(INT, 0x00D64CF0, {});
end
---]]

--[[
function SummonBlackRequest()
    return FUNC(INT, 0x00D627D0, {});
end
---]]

--[[
function SummonedMapReload()
    return FUNC(INT, 0x00D60850, {});
end
---]]

--[[
function SummonSuccess()
    return FUNC(INT, 0x00D64BB0, {});
end
---]]

--[[
function SwitchDispMask()
    return FUNC(INT, 0x00D5FF20, {});
end
---]]

--[[
function SwitchHitMask()
    return FUNC(INT, 0x00D64DB0, {});
end
---]]

--[[
function TalkNextPage()
    return FUNC(INT, 0x00D5E640, {});
end
---]]

--[[
function TreasureDispModeChange()
    return FUNC(INT, 0x00D64860, {});
end
---]]

--[[
function TreasureDispModeChange2()
    return FUNC(INT, 0x00D647C0, {});
end
---]]

--[[
function TurnCharactor()
    return FUNC(INT, 0x00D623A0, {});
end
---]]

--[[
function Tutorial_begin()
    return FUNC(INT, 0x00D5E030, {});
end
---]]

--[[
function Tutorial_end()
    return FUNC(INT, 0x00D5E020, {});
end
---]]

--[[
function UnLockSession()
    return FUNC(INT, 0x00D5E3F0, {});
end
---]]

--[[
function UpDateBloodMark()
    return FUNC(INT, 0x00D661F0, {});
end
---]]

--[[
function Util_RequestLevelUp()
    return FUNC(INT, 0x00D5D830, {});
end
---]]

--[[
function Util_RequestLevelUpFirst()
    return FUNC(INT, 0x00D5D850, {});
end
---]]

--[[
function Util_RequestRegene()
    return FUNC(INT, 0x00D5D820, {});
end
---]]

--[[
function Util_RequestRespawn()
    return FUNC(INT, 0x00D5D810, {});
end
---]]

--[[
function ValidPointLight()
    return FUNC(INT, 0x00D5F020, {});
end
---]]

--[[
function ValidSfx()
    return FUNC(INT, 0x00D5F0A0, {});
end
---]]

--[[
function VariableExpand_211_param1()
    return FUNC(INT, 0x00D5D8C0, {});
end
---]]

--[[
function VariableExpand_211_param2()
    return FUNC(INT, 0x00D5D8B0, {});
end
---]]

--[[
function VariableExpand_211_param3()
    return FUNC(INT, 0x00D5D8A0, {});
end
---]]

--[[
function VariableExpand_22_param1()
    return FUNC(INT, 0x00D5D8E0, {});
end
---]]

--[[
function VariableExpand_22_param2()
    return FUNC(INT, 0x00D5D8D0, {});
end
---]]

--[[
function VariableOrder_211()
    return FUNC(INT, 0x00D5D8F0, {});
end
---]]

--[[
function VariableOrder_22()
    return FUNC(INT, 0x00D5D910, {});
end
---]]

--[[
function WARN()
    return FUNC(INT, 0x00D62050, {});
end
---]]

--[[
function Warp()
    return FUNC(INT, 0x00D61D40, {});
end
---]]

--[[
function WarpDmy()
    return FUNC(INT, 0x00D64A70, {});
end
---]]

--[[
function WarpNextStage()
    return FUNC(INT, 0x00D62D00, {});
end
---]]

--[[
function WarpNextStage_Bonfire()
    return FUNC(INT, 0x00D62CA0, {});
end
---]]

--[[
function WarpNextStageKick()
    return FUNC(INT, 0x00D62930, {});
end
---]]

--[[
function WarpRestart()
    return FUNC(INT, 0x00D62580, {});
end
---]]

--[[
function WarpRestartNoGrey()
    return FUNC(INT, 0x00D62550, {});
end
---]]

--[[
function WarpSelfBloodMark()
    return FUNC(INT, 0x00D649D0, {});
end
---]]

