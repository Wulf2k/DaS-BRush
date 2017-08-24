ShowBossNames = true
TimeBetweenBosses = 3.0
RandomizeBossNG = false

InfiniteLives = true

--Note that this setting is literally completely pointless if InfiniteLives is enabled.
RefillHpEachFight = true

--Order types: "Standard", "Reverse", "Random", "Custom"
BossRushOrder = "Custom"

--Only applicable if OrderType is set to "Custom"
BossRushCustomOrder = "Gravelord Nito; Bed of Chaos; Bed of Chaos"

--Has NO EFFECT if OrderType is set to "Custom"
BossRushExcludeBed = true 


------------------------------------------------------------------------------------------ 
--                                User settings end here                                --
------------------------------------------------------------------------------------------ 

function CountdownString(padTime, time, str, endStr)
    countdown = time + padTime
    currentFrameClock = os.clock()
    prevFrameClock = os.clock()
	
    repeat 
        currentFrameClock = os.clock()
        
        deltaTime = currentFrameClock - prevFrameClock
        
        SetBriefingMsg(str..string.format("%.3f", math.max(math.min(countdown, time), 0))) 
        countdown = countdown - deltaTime
         
        prevFrameClock = currentFrameClock
    until countdown <= 0
    
    SetBriefingMsg(str..endStr)
end

--Overall boss rush variables: 
bossRushNgLevel = 0
    
function FailBossRush()
    diagRes = SetGenDialog("No-death boss rush failed. Would you like to retry?", 1, "Yes", "No") 
	
	if diagRes.Response == 1 then
	    return true
    else
	    return false
	end
end
    
function InitRand()
    math.randomseed(os.time())
    rando = math.random(0, 16)
    for i=0,rando do
        math.random()
    end
end

function PrepareForNextBoss(isFirstBoss)
    if RandomizeBossNG then
        SetClearCount(math.random(0, 6))
    else
        SetClearCount(bossRushNgLevel)
    end
    
    if RefillHpEachFight or isFirstBoss then
        RequestFullRecover()
    end
end

function FightBoss(bossName, isFirstBoss)
    visibleBossName = bossName
    if ShowBossNames == false then
        visibleBossName = "The Next Boss™"
    end
    CountdownString(1, TimeBetweenBosses, "Up next: "..visibleBossName.."!".."\n","Good luck!")
	
	bossDead = false
	
	repeat
	    PrepareForNextBoss(isFirstBoss)
	    BossRushHelper.SpawnPlayerAtBoss(bossName)
		if isFirstBoss then
            StartNewBossRushTimer() 
	    end
		
		bossDead = BossRushHelper.WaitForBossDeathByName(bossName)
		
		if not bossDead and not InfiniteLives then
		    isGoingToContinue = FailBossRush()
			if isGoingToContinue then
			    BeginBossRush()
			end
			return false
		end
	until bossDead

	return true
end

function ShowSaveWarning()
    MsgBoxOK("Saving is still disabled.\nIt is recommended that you return to the main menu, then exit the game there.")
end

function BeginBossRush()
    SetSaveEnable(false)

    Game.UpdateHook()
    InitRand()
    msg = ""
    diagRes = 0
    
	CroseBriefingMsg()
    ShowHUD(true)

    if RandomizeBossNG then
        diagRes = SetGenDialog("Boss NG level randomization is enabled in script. Ready to start?", 1, "Begin", "Wuss Out") 
    else
        diagRes = SetGenDialog("Choose your NG+ level wisely. Values above 6 are ignored.\n(0 = NG, 1 = NG+, 2 = NG++, etc)", 3, "Begin", "Wuss Out") 
    end
    
    
    if diagRes.Response ~= 1 then
        SetGenDialog("So much shame...", 2, "I know", "I don't care")
        ShowHUD(true)
        Proc.WInt32(Proc.RInt32(0x13786D0) + 0x154, -1) 
        Proc.WInt32(Proc.RInt32(0x13786D0) + 0x158, -1) 
        return
    end
    
    
    if RandomizeBossNG then
        bossRushNgLevel = -1
    else
        bossRushNgLevel = math.min(diagRes.Val, 6)
    end
    
    SetGenDialog("Welcome to the Boss Rush.\nSaving has been disabled.\nYou have "..string.format("%d", TimeBetweenBosses).." seconds to mentally prepare before each loading screen.", 1, "OK")   
    
    bossOrder = BossRushHelper.GetBossRushOrder(BossRushOrder, BossRushExcludeBed, BossRushCustomOrder)
    
    for i=0,bossOrder.Length-1 do
        continueRush = FightBoss(bossOrder[i]) 
        if not continueRush then return end
        isFirstBoss = false
    end

    timerStr = StopBossRushTimer()

	--TODO: Show all the different settings the user chose for the boss rush in this results screen.
    MsgBoxOK("Congratulations.\nCompleted in "..timerStr..".")
    ShowSaveWarning()
end