xinput_dll_offset = GetIngameDllAddress("XInput1_3.dll")

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

function ReadController()
    addr = RInt32(RInt32(xinput_dll_offset + 0x10C44)) + 0x0028
    buttonBitmaskValue = RUInt16(addr);
    
    L2 = RByte(addr + 3) / 255.0
    R2 = RByte(addr + 2) / 255.0
end

waitTimeBase = 400
waitTime = 0

fastForwardMin = 1

fastForwardIncrease = 1.1

fastForwardRatio = 1.0

fastForwardMax = 10
--fastForwardMultClimb = 25

prevClock = os.clock()

goUpDownSpeed = 1

fastForwardIncreaseTime = 3

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



while true do 
    
    ChrResetAnimation(10000, true)
    
    SetDeadMode(10000, true);
    SetDeadMode2(10000, true);
    
    if (CheckButton(BTN_R3)) then
        deadCamPtr = RInt32(0x137D644) + 0x40;
        WBool(deadCamPtr, false);
    end
    
    curClock = os.clock()
    
    
    
    currentFastForward = fastForwardMin + ((fastForwardMax - fastForwardMin) * fastForwardRatio)
    
    Player.Anim.DebugOverallSpeedMult.Value = currentFastForward
    
    ForceEntityDrawGroup(GetEntityPtr(10000));
    
    if CheckButton(BTN_R1) then
        SetIgnoreHit(int(10000), false);
        SetDisableGravity(int(10000), true);
        SetCamStickToAss(false)
    elseif CheckButton(BTN_L1) then
        SetIgnoreHit(int(10000), false);
        SetDisableGravity(int(10000), false);
        SetCamStickToAss(false)
    else
        SetIgnoreHit(int(10000), true);
        SetDisableGravity(int(10000), true);
        SetCamStickToAss(true)
    end

    ReadController();
    
    if CheckButton(BTN_A) then
        if CheckButtonPressed(BTN_A) then
            currentFastForward = 1
        end
        
        if fastForwardRatio <= fastForwardMax then
            fastForwardRatio = fastForwardRatio + ((curClock - prevClock) / fastForwardIncreaseTime)
        else
            fastForwardRatio = 1
        end
        
    else
        Player.Anim.DebugOverallSpeedMult.Value = 1
        fastForwardRatio = 0
    end
    
    playerPtr = GetEntityPtr(10000);
    playerHeading = GetEntityRotation(playerPtr);
    
    if L2 > 0 then 
        WarpEntity_Coords(playerPtr, Player.PosX.Value, Player.PosY.Value + (L2 * goUpDownSpeed * currentFastForward), Player.PosZ.Value, playerHeading)
    end
    
    if R2 > 0 then
        WarpEntity_Coords(playerPtr, Player.PosX.Value, Player.PosY.Value - (R2 * goUpDownSpeed * currentFastForward), Player.PosZ.Value, playerHeading)
    end
    
    
    prevButtonBitmaskValue = buttonBitmaskValue
    
    prevClock = curClock
end