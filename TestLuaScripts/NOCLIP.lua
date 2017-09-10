xinput_dll_offset = 0x1740000;

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
end

waitTimeBase = 400
waitTime = 0

fastForwardMult = 20
fastForwardMultClimb = 8

while true do 
    
    SetDeadMode(int(10000), true);
    SetDeadMode2(int(10000), true);
    
    
    
    
    if CheckButton(BTN_A) and CheckButton(BTN_R1) then
        SetIgnoreHit(int(10000), false);
        SetDisableGravity(int(10000), true);
    elseif CheckButton(BTN_A) and CheckButton(BTN_L1) then
        SetIgnoreHit(int(10000), false);
        SetDisableGravity(int(10000), false);
    else
        SetIgnoreHit(int(10000), true);
        SetDisableGravity(int(10000), true);
    end

    ReadController();
    
    if CheckButton(BTN_L3) then
        Player.Anim.DebugOverallSpeedMult.Value = fastForwardMult
        waitTime = waitTimeBase / fastForwardMult
    else
        Player.Anim.DebugOverallSpeedMult.Value = 1
        waitTime = waitTimeBase
    end
    
    if CheckButton(BTN_A) then
    
        if CheckButton(BTN_UP) then 
            Player.Anim.DebugOverallSpeedMult.Value = fastForwardMultClimb
        waitTime = waitTimeBase / fastForwardMultClimb
            ForcePlayAnimation(10000, 7011);
            Wait(waitTime);
            ForcePlayAnimation(10000, 7012);
            Wait(waitTime);
        elseif CheckButton(BTN_DOWN) then
            Player.Anim.DebugOverallSpeedMult.Value = fastForwardMultClimb
        waitTime = waitTimeBase / fastForwardMultClimb
            ForcePlayAnimation(10000, 7021);
            Wait(waitTime);
            ForcePlayAnimation(10000, 7022);
            Wait(waitTime);
        end
    
    end
    
    
    prevButtonBitmaskValue = buttonBitmaskValue
    
end