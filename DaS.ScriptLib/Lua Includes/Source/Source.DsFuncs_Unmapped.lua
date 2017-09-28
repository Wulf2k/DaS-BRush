

function AddEventGoal(...) : int
    return FUNC(INT, 0x00D66000, {...});
end

--[[
.text:00D60D30 loc_D60D30:                             ; DATA XREF: .text:0108B98Ao
.text:00D60D30 push    edi
.text:00D60D31 mov     edi, DebgEvent_Global_obj
.text:00D60D37 test    edi, edi
.text:00D60D39 jz      short loc_D60D67
.text:00D60D3B mov     eax, [esp+8]
.text:00D60D3F push    esi
.text:00D60D40 push    eax
.text:00D60D41 call    sub_D69F30
.text:00D60D46 mov     esi, eax
.text:00D60D48 test    esi, esi
.text:00D60D4A jz      short loc_D60D66
.text:00D60D4C mov     eax, [esp+10h]
.text:00D60D50 call    sub_D6C360
.text:00D60D55 test    eax, eax
.text:00D60D57 jz      short loc_D60D66
.text:00D60D59 mov     ecx, [esp+14h]
.text:00D60D5D push    0
.text:00D60D5F push    ecx
.text:00D60D60 push    esi
.text:00D60D61 call    sub_D40B70
]]
function AddEventParts(...) : int
    return FUNC(INT, 0x00D60D30, {...});
end

--[[
.text:00D60CF0 loc_D60CF0:                             ; DATA XREF: .text:0108B9BAo
.text:00D60CF0 push    edi
.text:00D60CF1 mov     edi, DebgEvent_Global_obj
.text:00D60CF7 test    edi, edi
.text:00D60CF9 jz      short loc_D60D27
.text:00D60CFB mov     eax, [esp+8]
.text:00D60CFF push    esi
.text:00D60D00 push    eax
.text:00D60D01 call    sub_D69F30
.text:00D60D06 mov     esi, eax
.text:00D60D08 test    esi, esi
.text:00D60D0A jz      short loc_D60D26
.text:00D60D0C mov     eax, [esp+10h]
.text:00D60D10 call    sub_D6C360
.text:00D60D15 test    eax, eax
.text:00D60D17 jz      short loc_D60D26
.text:00D60D19 mov     ecx, [esp+14h]
.text:00D60D1D push    1
.text:00D60D1F push    ecx
.text:00D60D20 push    esi
.text:00D60D21 call    sub_D40B70
.text:00D60D26
.text:00D60D26 loc_D60D26:                             ; CODE XREF: .text:00D60D0Aj
.text:00D60D26                                         ; .text:00D60D17j
.text:00D60D26 pop     esi
.text:00D60D27
.text:00D60D27 loc_D60D27:                             ; CODE XREF: .text:00D60CF9j
.text:00D60D27 pop     edi
.text:00D60D28 retn    0Ch
]]
function AddEventParts_Ignore(...) : int
    return FUNC(INT, 0x00D60CF0, {...});
end

--[[
.text:00D5DB50 loc_D5DB50:                             ; DATA XREF: .text:01089A5Ao
.text:00D5DB50 mov     ecx, info_about_summons
.text:00D5DB56 test    ecx, ecx
.text:00D5DB58 jz      short locret_D5DB6E
.text:00D5DB5A mov     eax, [ecx+4Ch]
.text:00D5DB5D cmp     eax, 0FFFFFFFEh
.text:00D5DB60 jbe     short loc_D5DB6A
.text:00D5DB62 mov     dword ptr [ecx+4Ch], 0FFFFFFFFh
.text:00D5DB69 retn
]]
function AddHelpWhiteGhost(...) : int
    return FUNC(INT, 0x00D5DB50, {...});
end

function AddInfomationListItem(...) : int
    return FUNC(INT, 0x00D62350, {...});
end

function AddInfomationTimeMsgTag(...) : int
    return FUNC(INT, 0x00D646F0, {...});
end

function AddInfomationTosBuffer(...) : int
    return FUNC(INT, 0x00D646D0, {...});
end

function AddInfomationTosBufferPlus(...) : int
    return FUNC(INT, 0x00D646B0, {...});
end

function AddInventoryItem(...) : int
    return FUNC(INT, 0x00D664C0, {...});
end

function AddKillBlackGhost(...) : int
    return FUNC(INT, 0x00D5DB30, {...});
end

function AddQWC(...) : int
    return FUNC(INT, 0x00D5F810, {...});
end

function AddRumble(...) : int
    return FUNC(INT, 0x00D640D0, {...});
end

function AddTreasureEvent(...) : int
    return FUNC(INT, 0x00D5F3B0, {...});
end

function AddTrueDeathCount(...) : int
    return FUNC(INT, 0x00D5DDF0, {...});
end

function CalcGetCurrentMapEntityId(...) : int
    return FUNC(INT, 0x00D5F230, {...});
end

function CalcGetMultiWallEntityId(...) : int
    return FUNC(INT, 0x00D5E360, {...});
end

function CastPointSpell(...) : int
    return FUNC(INT, 0x00D65FB0, {...});
end

function CastPointSpell_Horming(...) : int
    return FUNC(INT, 0x00D65F10, {...});
end

function CastPointSpellPlus(...) : int
    return FUNC(INT, 0x00D65F60, {...});
end

function CastPointSpellPlus_Horming(...) : int
    return FUNC(INT, 0x00D65EC0, {...});
end

--[[
.text:00D60D70 loc_D60D70:                             ; DATA XREF: .text:0108B88Ao
.text:00D60D70 cmp     DebgEvent_Global_obj, 0
.text:00D60D77 jz      short locret_D60D87
.text:00D60D79 mov     eax, [esp+8]
.text:00D60D7D push    eax
.text:00D60D7E mov     eax, [esp+8]
.text:00D60D82 call    sub_D6D7E0
.text:00D60D87
.text:00D60D87 locret_D60D87:                          ; CODE XREF: .text:00D60D77j
.text:00D60D87 retn    8
]]
function ChangeInitPosAng(...) : int
    return FUNC(INT, 0x00D60D70, {...});
end

--[[
.text:00D61DA0 loc_D61DA0:                             ; DATA XREF: .text:0108A25Ao
.text:00D61DA0 cmp     DebgEvent_Global_obj, 0
.text:00D61DA7 jnz     short loc_D61DAE
.text:00D61DA9 xor     al, al
.text:00D61DAB retn    8
.text:00D61DAE ; ---------------------------------------------------------------------------
.text:00D61DAE
.text:00D61DAE loc_D61DAE:                             ; CODE XREF: .text:00D61DA7j
.text:00D61DAE mov     eax, [esp+4]
.text:00D61DB2 push    ebx
.text:00D61DB3 push    edi
.text:00D61DB4 call    sub_D6C360
.text:00D61DB9 mov     edi, eax
.text:00D61DBB xor     bl, bl
.text:00D61DBD test    edi, edi
.text:00D61DBF jz      short loc_D61DEB
.text:00D61DC1 mov     eax, [esp+10h]
.text:00D61DC5 test    eax, eax
.text:00D61DC7 jle     short loc_D61DE2
.text:00D61DC9 call    sub_D6C360
.text:00D61DCE test    eax, eax
.text:00D61DD0 jz      short loc_D61DEB
.text:00D61DD2 push    edi
.text:00D61DD3 call    sub_CDF150
.text:00D61DD8 add     esp, 4
.text:00D61DDB pop     edi
.text:00D61DDC mov     bl, al
.text:00D61DDE pop     ebx
.text:00D61DDF retn    8
]]
function ChangeTarget(...) : int
    return FUNC(INT, 0x00D61DA0, {...});
end

--[[
.text:00D5EE50 loc_D5EE50:                             ; DATA XREF: .text:0108E0BAo
.text:00D5EE50 jmp     loc_D6B240
.text:00D5EE50 ; ---------------------------------------------------------------------------
.text:00D5EE55 align 10h
.text:00D5EE60
.text:00D5EE60 loc_D5EE60:                             ; DATA XREF: .text:0108D87Ao
.text:00D5EE60 push    edi
.text:00D5EE61 mov     edi, host_info_global
.text:00D5EE67 call    sub_D2A970
.text:00D5EE6C pop     edi
.text:00D5EE6D retn
]]
function CheckPenalty(...) : int
    return FUNC(INT, 0x00D5EE50, {...});
end

function ChrDisableUpdate(...) : int
    return FUNC(INT, 0x00D61460, {...});
end

--[[
.text:00D5EDD0 loc_D5EDD0:                             ; DATA XREF: .text:0108E95Ao
.text:00D5EDD0 mov     eax, session_info
.text:00D5EDD5 push    esi
.text:00D5EDD6 mov     esi, [eax+0BD0h]
.text:00D5EDDC push    edi
.text:00D5EDDD lea     edi, [eax+0BD0h]
.text:00D5EDE3 mov     dword ptr [eax+0BCCh], 0
.text:00D5EDED test    esi, esi
.text:00D5EDEF jz      short loc_D5EE0F
.text:00D5EDF1 push    esi
.text:00D5EDF2 call    sub_562C30
.text:00D5EDF7 mov     edx, [eax]
.text:00D5EDF9 add     esp, 4
.text:00D5EDFC mov     ecx, eax
.text:00D5EDFE mov     eax, [edx+34h]
.text:00D5EE01 push    esi
.text:00D5EE02 call    eax
.text:00D5EE04 mov     eax, session_info
.text:00D5EE09 mov     dword ptr [edi], 0
.text:00D5EE0F
]]
function ClearMyWorldState(...) : int
    return FUNC(INT, 0x00D5EDD0, {...});
end

function CloseGenDialog(...) : int
    return FUNC(INT, 0x00D5E500, {...});
end

function CloseMenu(...) : int
    return FUNC(INT, 0x00D5ED00, {...});
end

function CloseRankingDialog(...) : int
    return FUNC(INT, 0x00D5D950, {...});
end

function CloseTalk(...) : int
    return FUNC(INT, 0x00D5E610, {...});
end

function CompleteEvent(...) : int
    return FUNC(INT, 0x00D660A0, {...});
end

function CreateCamSfx(...) : int
    return FUNC(INT, 0x00D620B0, {...});
end

function CreateDamage_NoCollision(...) : int
    return FUNC(INT, 0x00D65DC0, {...});
end

function CreateEventBody(...) : int
    return FUNC(INT, 0x00D65830, {...});
end

function CreateEventBodyPlus(...) : int
    return FUNC(INT, 0x00D657C0, {...});
end

function CreateHeroBloodStain(...) : int
    return FUNC(INT, 0x00D62640, {...});
end

function CreateSfx(...) : int
    return FUNC(INT, 0x00D5F000, {...});
end

function CreateSfx_DummyPoly(...) : int
    return FUNC(INT, 0x00D64140, {...});
end

function CroseBriefingMsg(...) : int
    return FUNC(INT, 0x00D5E400, {...});
end

function CustomLuaCall(...) : int
    return FUNC(INT, 0x00D62C30, {...});
end

function CustomLuaCallStart(...) : int
    return FUNC(INT, 0x00D66240, {...});
end

function CustomLuaCallStartPlus(...) : int
    return FUNC(INT, 0x00D66210, {...});
end

function DeleteCamSfx(...) : int
    return FUNC(INT, 0x00D5E8A0, {...});
end

function DeleteEvent(...) : int
    return FUNC(INT, 0x00D5EAA0, {...});
end

function DeleteObjSfxAll(...) : int
    return FUNC(INT, 0x00D5E7B0, {...});
end

function DeleteObjSfxDmyPlyID(...) : int
    return FUNC(INT, 0x00D5E7F0, {...});
end

function DeleteObjSfxEventID(...) : int
    return FUNC(INT, 0x00D5E7D0, {...});
end

function DisableCollection(...) : int
    return FUNC(INT, 0x00D60A40, {...});
end

function DisableHpGauge(...) : int
    return FUNC(INT, 0x00D60A00, {...});
end

function DisableInterupt(...) : int
    return FUNC(INT, 0x00D612B0, {...});
end

function DivideRest(...) : int
    return FUNC(INT, 0x00490520, {...});
end

function EnableAction(...) : int
    return FUNC(INT, 0x00D5FD60, {...});
end

function EnableGeneratorSystem(...) : int
    return FUNC(INT, 0x00D5EF20, {...});
end

function EnableHide(...) : int
    return FUNC(INT, 0x00D60E00, {...});
end

function EnableInvincible(...) : int
    return FUNC(INT, 0x00D60D90, {...});
end

function EnableLogic(...) : int
    return FUNC(INT, 0x00D5FD90, {...});
end

function EnableObjTreasure(...) : int
    return FUNC(INT, 0x00D661D0, {...});
end

function EndAnimation(...) : int
    return FUNC(INT, 0x00D61BF0, {...});
end

function EraseEventSpecialEffect(...) : int
    return FUNC(INT, 0x00D61210, {...});
end

function EraseEventSpecialEffect_2(...) : int
    return FUNC(INT, 0x00D61170, {...});
end

function EventTagInsertString_forPlayerNo(...) : int
    return FUNC(INT, 0x00D5E820, {...});
end

function ExcutePenalty(...) : int
    return FUNC(INT, 0x00D5EBE0, {...});
end

function ForceChangeTarget(...) : int
    return FUNC(INT, 0x00D61E00, {...});
end

function ForceDead(...) : int
    return FUNC(INT, 0x00D66690, {...});
end

function ForceSetOmissionLevel(...) : int
    return FUNC(INT, 0x00D60CB0, {...});
end

function ForceUpdateNextFrame(...) : int
    return FUNC(INT, 0x00D60C90, {...});
end

function GetBountyRankPoint(...) : int
    return FUNC(INT, 0x00D60130, {...});
end

function GetClearBonus(...) : int
    return FUNC(INT, 0x00D5DEB0, {...});
end

function GetClearCount(...) : int
    return FUNC(INT, 0x00D5E5A0, {...});
end

function GetClearState(...) : int
    return FUNC(INT, 0x00D5E580, {...});
end

function GetDeathState(...) : int
    return FUNC(INT, 0x00D5D800, {...});
end

function GetDistance(...) : int
    return FUNC(INT, 0x00D61B70, {...});
end

function GetEnemyPlayerId_Random(...) : int
    return FUNC(INT, 0x00D5ED10, {...});
end

function GetEventFlagValue(...) : int
    return FUNC(INT, 0x00D60340, {...});
end

function GetEventGoalState(...) : int
    return FUNC(INT, 0x00D612F0, {...});
end

function GetEventMode(...) : int
    return FUNC(INT, 0x00D61690, {...});
end

function GetEventRequest(...) : int
    return FUNC(INT, 0x00D610D0, {...});
end

function GetFloorMaterial(...) : int
    return FUNC(INT, 0x00D60480, {...});
end

function GetGlobalQWC(...) : int
    return FUNC(INT, 0x00D5F850, {...});
end

function GetHeroPoint(...) : int
    return FUNC(INT, 0x00D5DC50, {...});
end

function GetHostPlayerNo(...) : int
    return FUNC(INT, 0x00D5E260, {...});
end

function GetHp(...) : int
    return FUNC(INT, 0x00D5FA80, {...});
end

function GetHpRate(...) : int
    return FUNC(INT, 0x00D5FA30, {...});
end

function GetItem(...) : int
    return FUNC(INT, 0x00D5E240, {...});
end

function GetLadderCount(...) : int
    return FUNC(INT, 0x00D64580, {...});
end

function GetLastBlockId(...) : int
    return FUNC(INT, 0x00D5EFC0, {...});
end

function GetLocalPlayerChrType(...) : int
    return FUNC(INT, 0x00D5FB70, {...});
end

function GetLocalPlayerId(...) : int
    return FUNC(INT, 0x00D5E270, {...});
end

function GetLocalPlayerInvadeType(...) : int
    return FUNC(INT, 0x00D5FB50, {...});
end

function GetLocalPlayerSoulLv(...) : int
    return FUNC(INT, 0x00D5EB30, {...});
end

function GetLocalPlayerVowType(...) : int
    return FUNC(INT, 0x00D5FB20, {...});
end

function GetLocalQWC(...) : int
    return FUNC(INT, 0x00D5F870, {...});
end

function GetMultiWallNum(...) : int
    return FUNC(INT, 0x00D5DC40, {...});
end

function GetNetPlayerChrType(...) : int
    return FUNC(INT, 0x00D5E8C0, {...});
end

function GetObjHp(...) : int
    return FUNC(INT, 0x00D5F320, {...});
end

function GetParam(...) : int
    return FUNC(INT, 0x00CD9FF0, {...});
end

function GetParam1(...) : int
    return FUNC(INT, 0x00403B00, {...});
end

function GetParam2(...) : int
    return FUNC(INT, 0x00B1CD50, {...});
end

function GetParam3(...) : int
    return FUNC(INT, 0x00788EE0, {...});
end

function GetPartyMemberNum_InvadeType(...) : int
    return FUNC(INT, 0x00D5EBA0, {...});
end

function GetPartyMemberNum_VowType(...) : int
    return FUNC(INT, 0x00D5F3A0, {...});
end

function GetPlayerId_Random(...) : int
    return FUNC(INT, 0x00D5ED30, {...});
end

function GetPlayerNo_LotNitoMultiItem(...) : int
    return FUNC(INT, 0x00D64100, {...});
end

function GetPlayID(...) : int
    return FUNC(INT, 0x00B0E260, {...});
end

function GetQWC(...) : int
    return FUNC(INT, 0x00D5F8C0, {...});
end

function GetRandom(...) : int
    return FUNC(INT, 0x00D5E0F0, {...});
end

function GetRateItem(...) : int
    return FUNC(INT, 0x00D665B0, {...});
end

function GetRateItem_IgnoreMultiPlay(...) : int
    return FUNC(INT, 0x00D66590, {...});
end

function GetReturnState(...) : int
    return FUNC(INT, 0x00D5E230, {...});
end

function GetRightCurrentWeaponId(...) : int
    return FUNC(INT, 0x00D5EF80, {...});
end

function GetSoloClearBonus(...) : int
    return FUNC(INT, 0x00D5DE60, {...});
end

function GetSummonAnimId(...) : int
    return FUNC(INT, 0x00D60890, {...});
end

function GetSummonBlackResult(...) : int
    return FUNC(INT, 0x00D5E530, {...});
end

function GetTargetChrID(...) : int
    return FUNC(INT, 0x00D64160, {...});
end

function GetTempSummonParam(...) : int
    return FUNC(INT, 0x00D5DDE0, {...});
end

function GetTravelItemParamId(...) : int
    return FUNC(INT, 0x00D60440, {...});
end

function GetWhiteGhostCount(...) : int
    return FUNC(INT, 0x00D5E3D0, {...});
end

function HasSuppleItem(...) : int
    return FUNC(INT, 0x00D60430, {...});
end

function HavePartyMember(...) : int
    return FUNC(INT, 0x00D5E1D0, {...});
end

function HoverMoveVal(...) : int
    return FUNC(INT, 0x00D62C00, {...});
end

function HoverMoveValDmy(...) : int
    return FUNC(INT, 0x00D648E0, {...});
end

function IncrementCoopPlaySuccessCount(...) : int
    return FUNC(INT, 0x00D5F1D0, {...});
end

function IncrementThiefInvadePlaySuccessCount(...) : int
    return FUNC(INT, 0x00D5F1A0, {...});
end

function InfomationMenu(...) : int
    return FUNC(INT, 0x00D64110, {...});
end

function InitDeathState(...) : int
    return FUNC(INT, 0x00D5D7E0, {...});
end

function InvalidMyBloodMarkInfo(...) : int
    return FUNC(INT, 0x00D60E50, {...});
end

function InvalidMyBloodMarkInfo_Tutorial(...) : int
    return FUNC(INT, 0x00D60E30, {...});
end

function InvalidPointLight(...) : int
    return FUNC(INT, 0x00D5F060, {...});
end

function InvalidSfx(...) : int
    return FUNC(INT, 0x00D5F780, {...});
end

---
--IDA crashes when I try to convert it to a function. meh.
function IsMatchingMultiPlay(...) : int
    return FUNC(INT, 0x00D5ED80, {...});
end

function LeaveSession(...) : int
    return FUNC(INT, 0x00D62900, {...});
end

function LockSession(...) : int
    return FUNC(INT, 0x00D5EE60, {...});
end

function NoAnimeTurnCharactor(...) : int
    return FUNC(INT, 0x00D61480, {...});
end

function NotNetMessage_begin(...) : int
    return FUNC(INT, 0x00D5DBE0, {...});
end

function NotNetMessage_end(...) : int
    return FUNC(INT, 0x00D5DBD0, {...});
end

function ObjRootMtxMove(...) : int
    return FUNC(INT, 0x00D647A0, {...});
end

function ObjRootMtxMoveByChrDmyPoly(...) : int
    return FUNC(INT, 0x00D64760, {...});
end

function ObjRootMtxMoveDmyPoly(...) : int
    return FUNC(INT, 0x00D64780, {...});
end

function OnActionCheckKey(...) : int
    return FUNC(INT, 0x00D63B70, {...});
end

function OnActionEventRegion(...) : int
    return FUNC(INT, 0x00D63C70, {...});
end

function OnActionEventRegionAttribute(...) : int
    return FUNC(INT, 0x00D63BE0, {...});
end

function OnBallista(...) : int
    return FUNC(INT, 0x00D5E290, {...});
end

function OnBloodMenuClose(...) : int
    return FUNC(INT, 0x00D62FF0, {...});
end

function OnBonfireEvent(...) : int
    return FUNC(INT, 0x00D64B40, {...});
end

function OnCharacterAnimEnd(...) : int
    return FUNC(INT, 0x00D63860, {...});
end

function OnCharacterDead(...) : int
    return FUNC(INT, 0x00D63AB0, {...});
end

function OnCharacterHP(...) : int
    return FUNC(INT, 0x00D63A50, {...});
end

function OnCharacterHP_CheckAttacker(...) : int
    return FUNC(INT, 0x00D639E0, {...});
end

function OnCharacterHpRate(...) : int
    return FUNC(INT, 0x00D63980, {...});
end

function OnCharacterTotalDamage(...) : int
    return FUNC(INT, 0x00D63750, {...});
end

function OnCharacterTotalRateDamage(...) : int
    return FUNC(INT, 0x00D636C0, {...});
end

function OnCheckEzStateMessage(...) : int
    return FUNC(INT, 0x00D63B10, {...});
end

function OnChrAnimEnd(...) : int
    return FUNC(INT, 0x00D64570, {...});
end

function OnChrAnimEndPlus(...) : int
    return FUNC(INT, 0x00D637D0, {...});
end

function OnDistanceAction(...) : int
    return FUNC(INT, 0x00D65240, {...});
end

function OnDistanceActionAttribute(...) : int
    return FUNC(INT, 0x00D65190, {...});
end

function OnDistanceActionDmyPoly(...) : int
    return FUNC(INT, 0x00D64F70, {...});
end

function OnDistanceActionPlus(...) : int
    return FUNC(INT, 0x00D650D0, {...});
end

function OnDistanceActionPlusAttribute(...) : int
    return FUNC(INT, 0x00D65010, {...});
end

function OnDistanceJustIn(...) : int
    return FUNC(INT, 0x00D64E50, {...});
end

function OnEndFlow(...) : int
    return FUNC(INT, 0x00D62990, {...});
end

function OnFireDamage(...) : int
    return FUNC(INT, 0x00D634B0, {...});
end

function OnKeyTime2(...) : int
    return FUNC(INT, 0x00D635D0, {...});
end

function OnNetDistanceIn(...) : int
    return FUNC(INT, 0x00D62D40, {...});
end

function OnNetRegion(...) : int
    return FUNC(INT, 0x00D625D0, {...});
end

function OnNetRegionAttr(...) : int
    return FUNC(INT, 0x00D625A0, {...});
end

function OnNetRegionAttrPlus(...) : int
    return FUNC(INT, 0x00D62DA0, {...});
end

function OnNetRegionPlus(...) : int
    return FUNC(INT, 0x00D62E00, {...});
end

function OnObjAnimEnd(...) : int
    return FUNC(INT, 0x00D64560, {...});
end

function OnObjAnimEndPlus(...) : int
    return FUNC(INT, 0x00D64550, {...});
end

function OnObjDestroy(...) : int
    return FUNC(INT, 0x00D63900, {...});
end

function OnObjectDamageHit(...) : int
    return FUNC(INT, 0x00D659A0, {...});
end

function OnObjectDamageHit_NoCall(...) : int
    return FUNC(INT, 0x00D65940, {...});
end

function OnObjectDamageHit_NoCallPlus(...) : int
    return FUNC(INT, 0x00D658E0, {...});
end

function OnPlayerActionInRegion(...) : int
    return FUNC(INT, 0x00D63EB0, {...});
end

function OnPlayerActionInRegionAngle(...) : int
    return FUNC(INT, 0x00D63D90, {...});
end

function OnPlayerActionInRegionAngleAttribute(...) : int
    return FUNC(INT, 0x00D63D00, {...});
end

function OnPlayerActionInRegionAttribute(...) : int
    return FUNC(INT, 0x00D63E20, {...});
end

function OnPlayerAssessMenu(...) : int
    return FUNC(INT, 0x00D629E0, {...});
end

function OnPlayerDistanceAngleInTarget(...) : int
    return FUNC(INT, 0x00D652F0, {...});
end

function OnPlayerDistanceInTarget(...) : int
    return FUNC(INT, 0x00D65380, {...});
end

function OnPlayerDistanceOut(...) : int
    return FUNC(INT, 0x00D64EE0, {...});
end

function OnPlayerKill(...) : int
    return FUNC(INT, 0x00D63430, {...});
end

function OnRegionIn(...) : int
    return FUNC(INT, 0x00D64050, {...});
end

function OnRegionJustIn(...) : int
    return FUNC(INT, 0x00D63FC0, {...});
end

function OnRegionJustOut(...) : int
    return FUNC(INT, 0x00D63F40, {...});
end

function OnRegistFunc(...) : int
    return FUNC(INT, 0x00D63050, {...});
end

function OnRequestMenuEnd(...) : int
    return FUNC(INT, 0x00D62670, {...});
end

function OnRevengeMenuClose(...) : int
    return FUNC(INT, 0x00D64AC0, {...});
end

function OnSelectMenu(...) : int
    return FUNC(INT, 0x00D62EF0, {...});
end

function OnSelfBloodMark(...) : int
    return FUNC(INT, 0x00D633B0, {...});
end

function OnSelfHeroBloodMark(...) : int
    return FUNC(INT, 0x00D63330, {...});
end

function OnSessionIn(...) : int
    return FUNC(INT, 0x00D63230, {...});
end

function OnSessionInfo(...) : int
    return FUNC(INT, 0x00D630E0, {...});
end

function OnSessionJustIn(...) : int
    return FUNC(INT, 0x00D632B0, {...});
end

function OnSessionJustOut(...) : int
    return FUNC(INT, 0x00D631B0, {...});
end

function OnSessionOut(...) : int
    return FUNC(INT, 0x00D63130, {...});
end

function OnSimpleDamage(...) : int
    return FUNC(INT, 0x00D63540, {...});
end

function OnTalkEvent(...) : int
    return FUNC(INT, 0x00D65BB0, {...});
end

function OnTalkEventAngleOut(...) : int
    return FUNC(INT, 0x00D65A20, {...});
end

function OnTalkEventDistIn(...) : int
    return FUNC(INT, 0x00D65B30, {...});
end

function OnTalkEventDistOut(...) : int
    return FUNC(INT, 0x00D65AB0, {...});
end

function OnTestEffectEndPlus(...) : int
    return FUNC(INT, 0x00D626C0, {...});
end

function OnTextEffectEnd(...) : int
    return FUNC(INT, 0x00D62720, {...});
end

function OnTurnCharactorEnd(...) : int
    return FUNC(INT, 0x00D62A30, {...});
end

function OnWanderFade(...) : int
    return FUNC(INT, 0x00D622D0, {...});
end

function OnWanderingDemon(...) : int
    return FUNC(INT, 0x00D62240, {...});
end

function OnWarpMenuClose(...) : int
    return FUNC(INT, 0x00D62F90, {...});
end

function OnYesNoDialog(...) : int
    return FUNC(INT, 0x00D62E60, {...});
end

function OpenCampMenu(...) : int
    return FUNC(INT, 0x00D5DA80, {...});
end

function OpeningDead(...) : int
    return FUNC(INT, 0x00D66600, {...});
end

function OpeningDeadPlus(...) : int
    return FUNC(INT, 0x00D665D0, {...});
end

function OpenSOSMsg_Tutorial(...) : int
    return FUNC(INT, 0x00D5E750, {...});
end

function ParamInitialize(...) : int
    return FUNC(INT, 0x00D66360, {...});
end

function PauseTutorial(...) : int
    return FUNC(INT, 0x00D5E0B0, {...});
end

function PlayerChrResetAnimation_RemoOnly(...) : int
    return FUNC(INT, 0x00D60630, {...});
end

function PlayObjectSE(...) : int
    return FUNC(INT, 0x00D61660, {...});
end

function PlayPointSE(...) : int
    return FUNC(INT, 0x00D61F00, {...});
end

function PlayTypeSE(...) : int
    return FUNC(INT, 0x00D61630, {...});
end

function RecallMenuEvent(...) : int
    return FUNC(INT, 0x00D62780, {...});
end

function ReconstructBreak(...) : int
    return FUNC(INT, 0x00D66070, {...});
end

function RecoveryHeroin(...) : int
    return FUNC(INT, 0x00D60740, {...});
end

function RegistObjact(...) : int
    return FUNC(INT, 0x00D5D870, {...});
end

function RegistSimpleTalk(...) : int
    return FUNC(INT, 0x00D628A0, {...});
end

function RemoveInventoryEquip(...) : int
    return FUNC(INT, 0x00D60C70, {...});
end

function RepeatMessage_begin(...) : int
    return FUNC(INT, 0x00D5DBC0, {...});
end

function RepeatMessage_end(...) : int
    return FUNC(INT, 0x00D5DBB0, {...});
end

function RequestForceUpdateNetwork(...) : int
    return FUNC(INT, 0x00D60AA0, {...});
end

function RequestFullRecover(...) : int
    return FUNC(INT, 0x00D5DD40, {...});
end

function RequestGenerate(...) : int
    return FUNC(INT, 0x00D5F290, {...});
end

function RequestNormalUpdateNetwork(...) : int
    return FUNC(INT, 0x00D60A70, {...});
end

function RequestOpenBriefingMsg(...) : int
    return FUNC(INT, 0x00D5EB50, {...});
end

function RequestOpenBriefingMsgPlus(...) : int
    return FUNC(INT, 0x00D62260, {...});
end

function RequestPlayMovie(...) : int
    return FUNC(INT, 0x00D65750, {...});
end

function RequestPlayMoviePlus(...) : int
    return FUNC(INT, 0x00D656D0, {...});
end

function RequestRemo(...) : int
    return FUNC(INT, 0x00D66440, {...});
end

function RequestRemoPlus(...) : int
    return FUNC(INT, 0x00D663E0, {...});
end

function RequestUnlockTrophy(...) : int
    return FUNC(INT, 0x00D5EBB0, {...});
end

function ReqularLeavePlayer(...) : int
    return FUNC(INT, 0x00D5E3E0, {...});
end

function ResetCamAngle(...) : int
    return FUNC(INT, 0x00D5EB20, {...});
end

function ResetEventQwcSpEffect(...) : int
    return FUNC(INT, 0x00D61ED0, {...});
end

function ResetSummonParam(...) : int
    return FUNC(INT, 0x00D5EB80, {...});
end

function ResetSyncRideObjInfo(...) : int
    return FUNC(INT, 0x00D60500, {...});
end

function ResetThink(...) : int
    return FUNC(INT, 0x00D61070, {...});
end

function RestorePiece(...) : int
    return FUNC(INT, 0x00D64C40, {...});
end

function ReturnMapSelect(...) : int
    return FUNC(INT, 0x00D5E1C0, {...});
end

function RevivePlayer(...) : int
    return FUNC(INT, 0x00D645E0, {...});
end

function RevivePlayerNext(...) : int
    return FUNC(INT, 0x00D5E0A0, {...});
end

function SaveRequest(...) : int
    return FUNC(INT, 0x00D5EE90, {...});
end

function SaveRequest_Profile(...) : int
    return FUNC(INT, 0x00D5EE70, {...});
end

function SendEventRequest(...) : int
    return FUNC(INT, 0x00D61130, {...});
end

function SetAliveMotion(...) : int
    return FUNC(INT, 0x00D5E6C0, {...});
end

function SetAlwaysDrawForEvent(...) : int
    return FUNC(INT, 0x00D60570, {...});
end

function SetAlwaysEnableBackread_forEvent(...) : int
    return FUNC(INT, 0x00D60750, {...});
end

function SetAngleFoward(...) : int
    return FUNC(INT, 0x00D61A80, {...});
end

function SetAreaStartMapUid(...) : int
    return FUNC(INT, 0x00D5F720, {...});
end

function SetBossUnitJrHit(...) : int
    return FUNC(INT, 0x00D61710, {...});
end

function SetBountyRankPoint(...) : int
    return FUNC(INT, 0x00D64E00, {...});
end

function SetBrokenPiece(...) : int
    return FUNC(INT, 0x00D5F7A0, {...});
end

function SetCamModeParamTargetId(...) : int
    return FUNC(INT, 0x00D5DF00, {...});
end

function SetCamModeParamTargetIdForBossLock(...) : int
    return FUNC(INT, 0x00D5DEF0, {...});
end

function SetChrTypeDataGrey(...) : int
    return FUNC(INT, 0x00D5DFB0, {...});
end

function SetChrTypeDataGreyNext(...) : int
    return FUNC(INT, 0x00D5DF50, {...});
end

function SetClearBonus(...) : int
    return FUNC(INT, 0x00D5F3D0, {...});
end

function SetClearItem(...) : int
    return FUNC(INT, 0x00D5DCD0, {...});
end

function SetClearSesiionCount(...) : int
    return FUNC(INT, 0x00D5E4C0, {...});
end

function SetClearState(...) : int
    return FUNC(INT, 0x00D5E560, {...});
end

function SetColiEnable(...) : int
    return FUNC(INT, 0x00D60090, {...});
end

function SetColiEnableArray(...) : int
    return FUNC(INT, 0x00D64AA0, {...});
end

function SetDefaultRoutePoint(...) : int
    return FUNC(INT, 0x00D645A0, {...});
end

function SetDisableBackread_forEvent(...) : int
    return FUNC(INT, 0x00D60AD0, {...});
end

function SetDisableDamage(...) : int
    return FUNC(INT, 0x00D5EC30, {...});
end

function SetDisableGravity(...) : int
    return FUNC(INT, 0x00D610A0, {...});
end

function SetDisableWeakDamageAnim(...) : int
    return FUNC(INT, 0x00D603D0, {...});
end

function SetDisableWeakDamageAnim_light(...) : int
    return FUNC(INT, 0x00D60390, {...});
end

function SetDispMask(...) : int
    return FUNC(INT, 0x00D61800, {...});
end

function SetDrawEnable(...) : int
    return FUNC(INT, 0x00D600B0, {...});
end

function SetDrawEnableArray(...) : int
    return FUNC(INT, 0x00D64530, {...});
end

function SetDrawGroup(...) : int
    return FUNC(INT, 0x00D61A00, {...});
end

function SetEnableEventPad(...) : int
    return FUNC(INT, 0x00D5E420, {...});
end

function SetEventBodyBulletCorrect(...) : int
    return FUNC(INT, 0x00D5E6E0, {...});
end

function SetEventBodyMaterialSeAndSfx(...) : int
    return FUNC(INT, 0x00D5E710, {...});
end

function SetEventBodyMaxHp(...) : int
    return FUNC(INT, 0x00D61D70, {...});
end

function SetEventCommand(...) : int
    return FUNC(INT, 0x00D5FCB0, {...});
end

function SetEventCommandIndex(...) : int
    return FUNC(INT, 0x00D5FC70, {...});
end

function SetEventFlagValue(...) : int
    return FUNC(INT, 0x00D60360, {...});
end

function SetEventGenerate(...) : int
    return FUNC(INT, 0x00D604B0, {...});
end

function SetEventMovePointType(...) : int
    return FUNC(INT, 0x00D60BB0, {...});
end

function SetEventSimpleTalk(...) : int
    return FUNC(INT, 0x00D64BD0, {...});
end

function SetEventSpecialEffect(...) : int
    return FUNC(INT, 0x00D61280, {...});
end

function SetEventSpecialEffect_2(...) : int
    return FUNC(INT, 0x00D611C0, {...});
end

function SetEventSpecialEffectOwner(...) : int
    return FUNC(INT, 0x00D61240, {...});
end

function SetEventSpecialEffectOwner_2(...) : int
    return FUNC(INT, 0x00D61190, {...});
end

function SetEventTarget(...) : int
    return FUNC(INT, 0x00D61AA0, {...});
end

function SetExVelocity(...) : int
    return FUNC(INT, 0x00D61F30, {...});
end

function SetFirstSpeed(...) : int
    return FUNC(INT, 0x00D61E90, {...});
end

function SetFirstSpeedPlus(...) : int
    return FUNC(INT, 0x00D61E50, {...});
end

function SetFlagInitState(...) : int
    return FUNC(INT, 0x00D5DBF0, {...});
end

function SetFootIKInterpolateType(...) : int
    return FUNC(INT, 0x00D60310, {...});
end

function SetForceJoinBlackRequest(...) : int
    return FUNC(INT, 0x00D65DF0, {...});
end

function SetHitInfo(...) : int
    return FUNC(INT, 0x00D61990, {...});
end

function SetHitMask(...) : int
    return FUNC(INT, 0x00D64920, {...});
end

function SetHp(...) : int
    return FUNC(INT, 0x00D5F9A0, {...});
end

function SetIgnoreHit(...) : int
    return FUNC(INT, 0x00D61750, {...});
end

function SetInfomationPriority(...) : int
    return FUNC(INT, 0x00D5DB10, {...});
end

function SetIsAnimPauseOnRemoPlayForEvent(...) : int
    return FUNC(INT, 0x00D5F300, {...});
end

function SetKeepCommandIndex(...) : int
    return FUNC(INT, 0x00D624F0, {...});
end

function SetLoadWait(...) : int
    return FUNC(INT, 0x00D5D940, {...});
end

function SetLockActPntInvalidateMask(...) : int
    return FUNC(INT, 0x00D608F0, {...});
end

function SetMapUid(...) : int
    return FUNC(INT, 0x00D64610, {...});
end

function SetMaxHp(...) : int
    return FUNC(INT, 0x00D5F910, {...});
end

function SetMenuBrake(...) : int
    return FUNC(INT, 0x00D5DB70, {...});
end

function SetMiniBlockIndex(...) : int
    return FUNC(INT, 0x00D5F5F0, {...});
end

function SetMovePoint(...) : int
    return FUNC(INT, 0x00D61390, {...});
end

function SetMultiWallMapUid(...) : int
    return FUNC(INT, 0x00D5F200, {...});
end

function SetNoNetSync(...) : int
    return FUNC(INT, 0x00D606D0, {...});
end

function SetObjDeactivate(...) : int
    return FUNC(INT, 0x00D5F120, {...});
end

function SetObjDisableBreak(...) : int
    return FUNC(INT, 0x00D5F340, {...});
end

function SetObjEventCollisionFill(...) : int
    return FUNC(INT, 0x00D5FF80, {...});
end

function SetObjSfx(...) : int
    return FUNC(INT, 0x00D62320, {...});
end

function SetReturnPointEntityId(...) : int
    return FUNC(INT, 0x00D5DC70, {...});
end

function SetReviveWait(...) : int
    return FUNC(INT, 0x00D5E680, {...});
end

function SetSelfBloodMapUid(...) : int
    return FUNC(INT, 0x00D5FFF0, {...});
end

function SetSosSignPos(...) : int
    return FUNC(INT, 0x00D5F570, {...});
end

function SetSosSignWarp(...) : int
    return FUNC(INT, 0x00D5E070, {...});
end

function SetSpStayAndDamageAnimId(...) : int
    return FUNC(INT, 0x00D609C0, {...});
end

function SetSpStayAndDamageAnimIdPlus(...) : int
    return FUNC(INT, 0x00D60980, {...});
end

function SetSubMenuBrake(...) : int
    return FUNC(INT, 0x00D5D880, {...});
end

function SetSummonedPos(...) : int
    return FUNC(INT, 0x00D5E090, {...});
end

function SetSyncRideObjInfo(...) : int
    return FUNC(INT, 0x00D60520, {...});
end

function SetSystemIgnore(...) : int
    return FUNC(INT, 0x00D605A0, {...});
end

function SetTalkMsg(...) : int
    return FUNC(INT, 0x00D5E190, {...});
end

function SetTeamType(...) : int
    return FUNC(INT, 0x00D60B50, {...});
end

function SetTeamTypeDefault(...) : int
    return FUNC(INT, 0x00D60B00, {...});
end

function SetTeamTypePlus(...) : int
    return FUNC(INT, 0x00D60B30, {...});
end

function SetTextEffect(...) : int
    return FUNC(INT, 0x00D5E430, {...});
end

function SetTutorialSummonedPos(...) : int
    return FUNC(INT, 0x00D5E080, {...});
end

function SetValidTalk(...) : int
    return FUNC(INT, 0x00D5E5C0, {...});
end

function ShowGenDialog(...) : int
    return FUNC(INT, 0x00D5EEF0, {...});
end

function ShowRankingDialog(...) : int
    return FUNC(INT, 0x00D5D960, {...});
end

function SOSMsgGetResult_Tutorial(...) : int
    return FUNC(INT, 0x00D5DA50, {...});
end

function StopLoopAnimation(...) : int
    return FUNC(INT, 0x00D5FFC0, {...});
end

function StopPointSE(...) : int
    return FUNC(INT, 0x00D5EA60, {...});
end

function SubDispMaskByBit(...) : int
    return FUNC(INT, 0x00D5FEA0, {...});
end

function SubHitMask(...) : int
    return FUNC(INT, 0x00D64CA0, {...});
end

function SubHitMaskByBit(...) : int
    return FUNC(INT, 0x00D64CF0, {...});
end

function SummonBlackRequest(...) : int
    return FUNC(INT, 0x00D627D0, {...});
end

function SummonedMapReload(...) : int
    return FUNC(INT, 0x00D60850, {...});
end

function SummonSuccess(...) : int
    return FUNC(INT, 0x00D64BB0, {...});
end

function SwitchDispMask(...) : int
    return FUNC(INT, 0x00D5FF20, {...});
end

function SwitchHitMask(...) : int
    return FUNC(INT, 0x00D64DB0, {...});
end

function TreasureDispModeChange(...) : int
    return FUNC(INT, 0x00D64860, {...});
end

function TreasureDispModeChange2(...) : int
    return FUNC(INT, 0x00D647C0, {...});
end

function TurnCharactor(...) : int
    return FUNC(INT, 0x00D623A0, {...});
end

function UnLockSession(...) : int
    return FUNC(INT, 0x00D5E3F0, {...});
end

function UpDateBloodMark(...) : int
    return FUNC(INT, 0x00D661F0, {...});
end

function ValidPointLight(...) : int
    return FUNC(INT, 0x00D5F020, {...});
end

function ValidSfx(...) : int
    return FUNC(INT, 0x00D5F0A0, {...});
end

function VariableExpand_211_Param1(...) : int
    return FUNC(INT, 0x00D5D8C0, {...});
end

function VariableOrder_211(...) : int
    return FUNC(INT, 0x00D5D8F0, {...});
end

function VariableOrder_22(...) : int
    return FUNC(INT, 0x00D5D910, {...});
end

function WARN(...) : int
    return FUNC(INT, 0x00D62050, {...});
end

function WarpDmy(...) : int
    return FUNC(INT, 0x00D64A70, {...});
end

function WarpNextStage_Bonfire(...) : int
    return FUNC(INT, 0x00D62CA0, {...});
end

function WarpNextStageKick(...) : int
    return FUNC(INT, 0x00D62930, {...});
end

function WarpRestart(...) : int
    return FUNC(INT, 0x00D62580, {...});
end

function WarpRestartNoGrey(...) : int
    return FUNC(INT, 0x00D62550, {...});
end

function WarpSelfBloodMark(...) : int
    return FUNC(INT, 0x00D649D0, {...});
end