numCalls = 0
timer = 0
prevTimer = 0
prevOsClock = os.clock()
math.randomseed(os.time())

while true do
    --SetChrType(int(10000), int(math.random() * 1))
    --ChrResetAnimation(int(10000), false)
    val = ForceChangeTarget(int(10000), int(10000))
    SetLineHelpText(""..val);
    --ForcePlayLoopAnimation(10000, rand * 10000)
    
    numCalls = numCalls + 1
    curOsClock = os.clock()
    deltaTime = (curOsClock - prevOsClock)
    timer = timer + deltaTime
    
    gc = LUAI.GC_Counter
    gcMax = LUAI.GC_Interval
    
    debugStr = ""..string.format("%.8d", numCalls).."["..string.format("%.0f", numCalls / timer).."Hz][GC Time:"..(string.format("%.3d", gc)).."/"..(gcMax).."][Mem:"..string.format("%.2f", LUAI.ProcessMemoryMB).."MB]"
    
    SetKeyGuideText(debugStr);
    SetKeyGuideTextPos(256, 160);
    
    prevOsClock = curOsClock
    prevTimer = timer
    
end