--TODO: Documentation

function GetNgPlusText(ngLevel) return ""; end
function MsgBoxOK(text) end
function MsgBoxBtn(text, btnText) end
function MsgBoxChoice(text, choice1, choice2) end
function Warp_Coords(x, y, z, rotx --[[nullable, default = null]]) end
function WarpEntity_Coords(entityPtr, x, y, z, rotx) end
function BlackScreen() end
function CamFocusEntity(entityptr) end
function ClearPlayTime() end
function ControlEntity(entityPtr, state) end
function DisableAI(state) end
function PlayerExterminate(state) end
function FadeIn() end
function FadeOut() end
function ForceEntityDrawGroup(entityptr) end
function SetCamPos(xpos, ypos, zpos, xrot, yrot) end
function SetFreeCam(state) end
function SetClearCount(clearCount) end
function SetSaveEnable(state) end
function SetSaveSlot(slot) end
function SetUnknownNpcName(name) end
function GetClosestEntityToEntity(entityPtr) return 0; end
function GetEntityPtrList() return {}; end
--Fills the provided table with all of the currently loaded Entity Objects. WIP.
function GetAllApparentEntities(table) return {}; end
function GetEntityVec3(entityPtr) end
function MoveEntityLaterallyTowardEntity(entityFromPtr, entityToPtr, speed) end
function GetAngleBetweenEntities(entityPtrA, entityPtrB) end
function GetDistanceSqrdBetweenEntities(entityPtrA, entityPtrB) end
function GetDistanceBetweenEntities(entityPtrA, entityPtrB) end
function MoveEntityLaterally(entityPtr, angle, speed) end
function MoveEntityAtSpeed(entityPtr, speedX, speedY, speedZ, speedRot --[[default = 0]]) end
function GetEntityPosX(entityPtr) return 0; end
function GetEntityPosY(entityPtr) return 0; end
function GetEntityPosZ(entityPtr) return 0; end
function GetEntityRotation(entityPtr) return 0; end
function SetEntityPosX(entityPtr, posX) end
function SetEntityPosY(entityPtr, posY) end
function SetEntityPosZ(entityPtr, posZ) end
function SetEntityRotation(entityPtr, angle) end
function SetEntityCoordsDirectly(entityPtr, posX, posY, posZ, angle --[[nullable]]) end
function GetInGameTimeInMs() return 0; end
function SetEntityLoc(entityPtr, location) end
function PlayerHide(state) end
function ShowHUD(state) end
function WaitForLoadEnd() end
function WaitForLoadStart() end
function WarpEntity_Player(entityptr) end
function WarpPlayer_Entity(entityptr) end
function WarpEntity_Entity(entityptrSrc, entityptrDest) end
function GetEntityPtrByName(mapName, entName) return 0; end
function SetBriefingMsg(str) end
function SetGenDialog(str, type, btn0 --[[default = ""]], btn1 --[[default = ""]]) return { Response = 0, Val = 0 }; end
function Wait(val) end
function DropItem(cat, item, num) end
function SetKeyGuideText(text) end
function SetLineHelpText(text) end
function SetKeyGuideTextPos(x, y) end
function SetLineHelpTextPos(x, y) end
function SetKeyGuideTextClear() end
function SetLineHelpTextClear() end
function ForcePlayerStableFootPos() end
function GetEntityPtr(entityId) end