ShowBossNames = true
TimeBetweenBosses = 3.0
RandomizeBossNG = false

InfiniteLives = true

--Note that this setting is literally completely pointless if InfiniteLives is enabled.
RefillHpEachFight = true

--Order types: "Standard", "Reverse", "Random", "Custom"
BossRushOrder = "Custom"

--Only applicable if OrderType is set to "Custom"
BossRushCustomOrder = ""

--Has NO EFFECT if OrderType is set to "Custom"
BossRushExcludeBed = true 

--"Prompt": Has in-game prompt asking to select NG.
--"Random": Does not prompt player. Chooses a random NG-level each boss
--"Choose": Does not prompt player. Uses the value of "BossRushNgLevel" below
BossRushNgMode = "Prompt"
BossRushNgLevel = 0

------------------------------------------------------------------------------------------ 
--                                User settings end here                                --
------------------------------------------------------------------------------------------ 

function SetPlayerIsVegetable(prot)
    DisableMove(10000, prot) --plr no move if prot
    SetColiEnable(10000, (not prot)) --plr no take dmg if prot
    SetDisableGravity(10000, prot) --plr no fall if prot
    DisableMapHit(10000, prot) --prevent activating col triggers if prot
end

function CountdownString(padTime, time, str, endStr)
    countdown = time + padTime
    currentFrameClock = os.clock()
    prevFrameClock = os.clock()

    repeat 
        currentFrameClock = os.clock()
        
        deltaTime = currentFrameClock - prevFrameClock

        --CroseBriefingMsg() --This is the only way to get it back up if player presses circle. Even then, it's pretty difficult :/

        if countdown > (time + (padTime / 2)) then 
            SetBriefingMsg(str)
        elseif countdown > (padTime / 2) then
            SetBriefingMsg(str..string.format("%.3f", math.max(math.min(countdown - (padTime / 2), time), 0)))
        else
            SetBriefingMsg(str..endStr)
        end

        countdown = countdown - deltaTime

        prevFrameClock = currentFrameClock
    until countdown <= 0
    
    CroseBriefingMsg()
    return true
end
    
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
        SetClearCount(BossRushNgLevel)
    end
    
    if RefillHpEachFight or isFirstBoss then
        RequestFullRecover()
    end
end

function CancelOutOfCurPlrAnim()
    --Force idle anim to start looping
    --ForcePlayLoopAnimation(10000, Data.PlayerAnim.Idle)
    --Cancel out of the idle anim loop so that the player can move again
    --StopLoopAnimation(10000, Data.PlayerAnim.Idle)
	ChrResetAnimation(10000)
end

function SpawnPlayerAtBoss(bossFlag)
    boss = Misc.GetBossFight(bossFlag)
    EventFlagHelper.ApplyAll(boss.AdditionalFlags)

    if bossFlag >= 0 then SetEventFlag(bossFlag, false) end

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    --if boss.BonfireID >= 0 and (not boss.PlayerWarp.IsZero) then 
        --WarpNextStage_Bonfire(boss.BonfireID)
    --else
        --WarpNextStage_Bonfire(Game.Player.BonfireID.Value)
    --end

    SetDeadMode(10000, true)
	WarpNextStage(boss.World, boss.Area, 0, 0, -1)
    CancelOutOfCurPlrAnim()
    
    Wait(2000)
    WaitForLoadEnd()
    BlackScreen()

    --PLAYER INVINCIBLE AND NON-INTERACTIVE
    SetPlayerIsVegetable(true)
    --Bosses won't aggro on player before the screen fades in
    PlayerHide(true)

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    --if boss.BonfireID >= 0 and (not boss.PlayerWarp.IsZero) then
        --Move player directly to warp point instantly without doing a "warp crouch"
        --SetEntityLocation(GetEntityPtr(Data.PlayerID), boss.PlayerWarp)
    --end

	if boss.WarpID > -1 then
        Warp(10000, boss.WarpID)
        CancelOutOfCurPlrAnim()
	end
    
    --Activate current location's map load trigger so it can load during FadeIn
    DisableMapHit(10000, false)
    
    SetDisable(10000, true)
    
    if not boss.PlayerWarp.IsZero then
        --if not (boss.WarpID == -1) then Wait(2000) end
        Warp_Coords(boss.PlayerWarp.Pos.X, boss.PlayerWarp.Pos.Y, boss.PlayerWarp.Pos.Z, boss.PlayerWarp.Rot)
        CancelOutOfCurPlrAnim()
    end
    
    DisableDamage(10000, true)
    

    --Begin resetting camera before FadeIn since the camera reset is smoothed out and takes a long time.
    CamReset(10000, true)
    SetPlayerIsVegetable(false)
    Wait(500)
    PlayerHide(false)
    FadeIn()
    
    
    ForcePlayAnimation(10000, boss.PlayerAnim)
    
    player.Opacity = 0
    SetDisable(10000, false)
    ChrFadeIn(10000, 0.5, 0)
    
    if not boss.PlayerWarp.IsZero then
        --if not (boss.WarpID == -1) then Wait(2000) end
        Warp_Coords(boss.PlayerWarp.Pos.X, boss.PlayerWarp.Pos.Y, boss.PlayerWarp.Pos.Z, boss.PlayerWarp.Rot)
    end
    
    ForcePlayAnimation(10000, boss.PlayerAnim)
    
    --ChrFadeIn(10000, 0, 0)
    DisableDamage(10000, false)
    SetDeadMode(10000, false)

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    if string.len(boss.EntranceLua) > 0 then
        --LUACEPTION
        Lua.Run(boss.EntranceLua)
    end
end

function FightBoss(bossFlag, isFirstBoss)
    bossName = Misc.GetBossFight(bossFlag).Name

    --PrintInfo("Fighting boss: "..bossName)
    
    visibleBossName = bossName
    if ShowBossNames == false then
        visibleBossName = "another boss fight"
    end
    if TimeBetweenBosses > 0 then
        finishedCountdown = CountdownString(1, TimeBetweenBosses, "Up next: "..visibleBossName.."!".."\n","Good luck!")
    end
    
    bossDead = false
    
    repeat
        PrepareForNextBoss(isFirstBoss)
        SpawnPlayerAtBoss(bossFlag)

        if isFirstBoss then
            BossRushHelper.StartNewBossRushTimer() 
        end

        bossDead = BossRushHelper.WaitForBossDeathByEventFlag(bossFlag)
        if bossDead then
            PrintInfo("Boss "..bossName.." dead")
        else 
            PrintInfo("Player dead")
        end
        
        if not bossDead then 
            AddTrueDeathCount()
            SetTextEffect(16)
            CountdownString(0.25, 3.0, "Respawning in:\n", "Now!")
            if not InfiniteLives then --you dun goofed, kiddo
                isGoingToContinue = FailBossRush() --asks user if they wanna retry
                if isGoingToContinue then
                    BeginBossRush()
                end
                return false --even if they retry we have to return after calling BeginBossRush recursively
            end
        end
    until bossDead

    return true
end

function ShowSaveWarning()
    MsgBoxOK("Saving is still disabled.\nIt is recommended that you return to the main menu, then exit the game there.")
end

function BeginBossRush()
    SetSaveEnable(false)

    InitRand()
    diagRes = 0

    ShowHUD(true)

    wussOutChoice = 0

    --PLAYER INVINCIBLE AND NON-INTERACTIVE
    SetPlayerIsVegetable(true)

    CroseBriefingMsg()
    Wait(200) --Not sure if needed but FUCK dude BriefingMsg is finnicky...
    MsgBoxOK("You are invincible while the boss rush mod asks you questions and between bosses.")

    if BossRushNgMode == "Prompt" then
        diagRes = SetGenDialog("Choose your NG+ level wisely. Values above 6 are ignored.\n(0 = NG, 1 = NG+, 2 = NG++, etc)", 3, "Begin", "Wuss Out") 
    else
        diagRes = SetGenDialog("Begin boss rush?", 2, "Begin", "Wuss Out") 
    end

    wussOutForSure = false

    if diagRes.Response == 2 then
        wussOutForSure = true
        diagRes = SetGenDialog("So much shame...", 2, "I know", "I don't care")

        if diagRes.Response == 1 then
            diagRes = SetGenDialog("I guess some people just don't have it in them.", 2, "On second thought...", "Guess not")

            if diagRes.Response == 1 then
                wussOutForSure = false
            end
        end 
    end

    if wussOutForSure then
        ShowHUD(true)
        WInt32(RInt32(0x13786D0) + 0x154, -1) 
        WInt32(RInt32(0x13786D0) + 0x158, -1) 
        SetBriefingMsg("You are no longer invincible.\n \n \n ")
        SetPlayerIsVegetable(false)
        return
    else
        if BossRushNgMode == "Random" then
            BossRushNgLevel = -1
        elseif BossRushNgMode == "Prompt" then
            BossRushNgLevel = math.min(diagRes.Val, 6)
        end
    end
    
    MsgBoxOK("Welcome to the Boss Rush.\nSaving has been disabled.")

	trace("Before GetBossRushOrder()")

    bossOrder = BossRushHelper.GetBossRushOrder(BossRushOrder, BossRushExcludeBed, BossRushCustomOrder)
    
	trace("After GetBossRushOrder()")

    isFirstBoss = true

	trace("Before bossinfo for loop")

    for i=0,bossOrder.Length-1 do
	    trace("Before boss index "..i)
		nextBoss = bossOrder[i]
		trace("Before fightboss "..i)
        continueRush = FightBoss(nextBoss, isFirstBoss)
		trace("After fightboss "..i)
        isFirstBoss = false

        if not continueRush then 
            MsgBoxOK("Better luck next time...")
            ShowSaveWarning()
            return 
        end
    end

    timerStr = BossRushHelper.StopBossRushTimer()

    --TODO: Show all the different settings the user chose for the boss rush in this results screen.
    MsgBoxOK("Congratulations.\nCompleted in "..timerStr..".")
    ShowSaveWarning()
end