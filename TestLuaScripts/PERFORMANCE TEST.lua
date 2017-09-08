numCalls = 0
timer = 0
prevOsClock = os.clock()
math.randomseed(os.time())

while true do
    SetChrType(int(10000), int(math.random() * 5))
    --ForcePlayLoopAnimation(10000, rand * 10000)
    
    numCalls = numCalls + 1
    curOsClock = os.clock()
    timer = timer + (curOsClock - prevOsClock)
    debugStr = "Loops: "..string.format("%.8d", numCalls).." ("..string.format("%.2f", numCalls / timer).." Hz)"
    --SetBriefingMsg(debugStr)
    
    --Print(debugStr)
    
    prevOsClock = curOsClock
    
    
end