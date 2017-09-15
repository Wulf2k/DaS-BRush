ENTITY = player--Entity.FromName("m12_01_00_00", "c4100_0000")

airAnimId = 700 --1580

if (ENTITY.EventEntityID ~= 10000) then

    SetDisable(10000, true)

end

xinput_dll_offset = Module["XINPUT1_3.DLL"][1]

BTN_A        = 0x1000
BTN_B        = 0x2000
BTN_X        = 0x4000
BTN_Y        = 0x8000
BTN_LB       = 0x0100
BTN_RB       = 0x0200
BTN_HOME     = 0x0400
BTN_BACK     = 0x0020
BTN_LThumb   = 0x0040
BTN_RThumb   = 0x0080
BTN_START    = 0x0010
BTN_UP       = 0x0001
BTN_DOWN     = 0x0002
BTN_LEFT     = 0x0004
BTN_RIGHT    = 0x0008
BTN_CROSS    = 0x1000
BTN_CIRCLE   = 0x2000
BTN_SQUARE   = 0x4000
BTN_TRIANGLE = 0x8000
BTN_L1       = 0x0100
BTN_R1       = 0x0200
BTN_PS       = 0x0400
BTN_SELECT   = 0x0020
BTN_L3       = 0x0040
BTN_R3       = 0x0080

buttonBitmaskValue = 0;
prevButtonBitmaskValue = 0;

L2 = 0
R2 = 0

function CheckButton(maskTest)
    return BitmaskCheck(buttonBitmaskValue, maskTest);
end

function CheckPrevButton(maskTest)
    return BitmaskCheck(prevButtonBitmaskValue, maskTest);
end

function CheckButtonPressed(maskTest)
    cur = CheckButton(maskTest);
    prev = CheckPrevButton(maskTest);
    
    return (cur and (not prev));
end

function CheckButtonReleased(maskTest)
    cur = CheckButton(maskTest);
    prev = CheckPrevButton(maskTest);
    
    return ((not cur) and prev);
end

function ReadController()
    addr = RInt32(RInt32(xinput_dll_offset + 0x10C44)) + 0x0028
    buttonBitmaskValue = RUInt16(addr);
    
    L2 = RByte(addr + 2) / 255.0
    R2 = RByte(addr + 3) / 255.0
end

waitTimeBase = 400
waitTime = 0

fastForwardMin = 1

fastForwardIncrease = 1.1

fastForwardRatio = 1.0

fastForwardMax = 10
--fastForwardMultClimb = 25

prevClock = os.clock()

goUpDownSpeed = 0.25

fastForwardIncreaseTime = 3

fastForwardDecay = 0.9

prevL2 = 0
prevR2 = 0

function SetCamStickToAss(stick)
    camSpeedPtr = RInt32(0x137D6DC)
    camSpeedPtr = RInt32(camSpeedPtr + 0x3c)
    camSpeedPtr = RInt32(camSpeedPtr + 0x60)
    camSpeedPtr = camSpeedPtr + 0x190
    
    if stick then
        WFloat(camSpeedPtr, 1)
    else
        WFloat(camSpeedPtr, 0.1) --default
    end
end

prevIngameTime = 0

while true do 
    
    ingameTime = RInt32(RInt32(0x1378700) + 0x68)
    
    if (ingameTime ~= prevIngameTime) then
        --ChrResetAnimation(10000, true)
        
        ReadController();
        
        SetDeadMode(ENTITY.EventEntityID, true);
        SetDeadMode2(ENTITY.EventEntityID, true);
        
        if (CheckButton(BTN_R3)) then
            deadCamPtr = RInt32(0x137D644) + 0x40;
            WBool(deadCamPtr, false);
        end
        
        if (CheckButtonPressed(BTN_L3)) then
        
            ENTITY.NoUpdate = not ENTITY.NoUpdate
        
        end
        
        ENTITY.NoMove = false
        ENTITY.NoAttack = true
        EnableLogic(int(ENTITY.EventEntityID), true)
        
        SetDisable(ENTITY.EventEntityID, false)
        
        ENTITY:StartControlling()
        ENTITY:View()
        
        ForceEntityDrawGroup(ENTITY.Pointer)
        
        curClock = os.clock()
        
        currentFastForward = fastForwardMin + ((fastForwardMax - fastForwardMin) * fastForwardRatio)
        
        --ForceEntityDrawGroup(GetEntityPtr(10000));
        
        if (CheckButton(BTN_L1) and CheckButton(BTN_R1) and CheckButton(BTN_START) and CheckButton(BTN_L3)) then
            ENTITY.SetDeadMode = false
            SetDeadMode2(ENTITY.EventEntityID, false)
            ENTITY.HP = 0
            Wait(15000)
            WaitForLoadEnd()
        end
        
        if CheckButton(BTN_R1) then
            SetDisableGravity(int(ENTITY.EventEntityID), true)
            SetIgnoreHit(int(ENTITY.EventEntityID), false);
            SetColiEnable(int(ENTITY.EventEntityID), false);
            --SetCamStickToAss(false)
        elseif CheckButton(BTN_L1) then
            SetDisableGravity(int(ENTITY.EventEntityID), false)
            SetIgnoreHit(int(ENTITY.EventEntityID), false);
            SetColiEnable(int(ENTITY.EventEntityID), false);
            --SetCamStickToAss(false)
        else
            SetIgnoreHit(int(ENTITY.EventEntityID), true);
            SetColiEnable(int(ENTITY.EventEntityID), true);
            SetDisableGravity(int(ENTITY.EventEntityID), true)
            --SetCamStickToAss(true)
        end
        
        playerAnimTargetSpeed = currentFastForward
        
        if CheckButton(BTN_A) then
            if CheckButtonPressed(BTN_A) then
                currentFastForward = 1
            end
            
            if fastForwardRatio <= fastForwardMax then
                fastForwardRatio = fastForwardRatio + ((curClock - prevClock) / fastForwardIncreaseTime)
            else
                fastForwardRatio = fastForwardMax
            end
        else
            fastForwardRatio = fastForwardRatio * fastForwardDecay
            
        end
        
        if L2 > 0 then 
            ForcePlayAnimation(ENTITY.EventEntityID, airAnimId)
            
            ENTITY.PosY = ENTITY.PosY - (L2 * goUpDownSpeed * currentFastForward)
            
            Entity.PlayerStablePosY = player.PosY
        elseif L2 == 0 and prevL2 > 0 then
            ForcePlayAnimation(ENTITY.EventEntityID, -1)
        end
        
        if R2 > 0 then
            ForcePlayAnimation(ENTITY.EventEntityID, airAnimId)
            
            ENTITY.PosY = ENTITY.PosY + (R2 * goUpDownSpeed * currentFastForward)
            
            Entity.PlayerStablePosY = player.PosY
        elseif R2 == 0 and prevR2 > 0 then
            ForcePlayAnimation(ENTITY.EventEntityID, -1)
            
        end
        
        
        prevButtonBitmaskValue = buttonBitmaskValue
        
        prevClock = curClock
        
        player.AnimationSpeed = playerAnimTargetSpeed
        
        prevL2 = L2
        prevR2 = R2
    end
    
    prevIngameTime = ingameTime
end