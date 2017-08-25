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

--"Prompt": Has in-game prompt asking to select NG.
--"Random": Does not prompt player. Chooses a random NG-level each boss
--"Choose": Does not prompt player. Uses the value of "BossRushNgLevel" below
BossRushNgMode = "Prompt"
BossRushNgLevel = 0


------------------------------------------------------------------------------------------ 
--                                User settings end here                                --
------------------------------------------------------------------------------------------ 

function SetPlayerIsVegetable(prot)
    DisableMove(Data.PlayerID, prot) --plr no move if prot
    SetColiEnable(Data.PlayerID, (not prot)) --plr no take dmg if prot
    SetDisableGravity(Data.PlayerID, prot) --plr no fall if prot
    DisableMapHit(Data.PlayerID, prot) --prevent activating col triggers if prot
end

function CountdownString(padTime, time, str, endStr)
    countdown = time + padTime
    currentFrameClock = os.clock()
    prevFrameClock = os.clock()

    repeat 
        currentFrameClock = os.clock()
        
        deltaTime = currentFrameClock - prevFrameClock

        CroseBriefingMsg() --This is the only way to get it back up if player presses circle. Even then, it's pretty difficult :/

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
    ForcePlayLoopAnimation(Data.PlayerID, Data.PlayerAnim.Idle)
    --Cancel out of the idle anim loop so that the player can move again
    StopLoopAnimation(Data.PlayerID, Data.PlayerAnim.Idle)
end

function SpawnPlayerAtBoss(bossName)
    boss = Data.BossFights[bossName]
    EventFlag.ApplyAll(boss.AdditionalFlags)

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    if boss.EventFlag >= 0 then SetEventFlag(boss.EventFlag, false) end
    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    if boss.BonfireID >= 0 and (not boss.PlayerWarp.IsZero) then 
        WarpNextStage_Bonfire(boss.BonfireID)
    else
        WarpNextStage_Bonfire(Game.Player.BonfireID)
    end

    Wait(2000)
    WaitForLoadEnd()
    BlackScreen()

    --PLAYER INVINCIBLE AND NON-INTERACTIVE
    SetPlayerIsVegetable(true)
    --Bosses won't aggro on player before the screen fades in
    PlayerHide(true)

    --Cancel out of the "I just warped and I'm so tired" animation
    CancelOutOfCurPlrAnim()

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    if boss.BonfireID >= 0 and (not boss.PlayerWarp.IsZero) then
        --Move player directly to warp point instantly without doing a "warp crouch"
        SetEntityLocation(GetEntityPtr(Data.PlayerID), boss.PlayerWarp)
    end

    --Activate current location's map load trigger so it can load during FadeIn
    DisableMapHit(Data.PlayerID, false)
    --Begin resetting camera before FadeIn since the camera reset is smoothed out and takes a long time.
    CamReset(Data.PlayerID, 1)

    --Just in case I guess.
    ShowHUD(true)

    --Start playing the walking through fogwall anim before it begins to fade in:
    ForcePlayAnimation(Data.PlayerID, Data.PlayerAnim.FogWalk)
    FadeIn()
    --Wait(1100)
    SetPlayerIsVegetable(false)
    PlayerHide(false)

    --Wait for player to finish walking through fog (roughly)
    Wait(2000)

    --Almost fail-proof check since we do not have all the boss rush data filled in yet.
    if boss.EntranceLua.Trim().Length > 0 then
        --LUACEPTION
        Lua.Run(boss.EntranceLua)
    end
end

function FightBoss(bossName, isFirstBoss)
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
        BossRushHelper.SpawnPlayerAtBoss(bossName)

        if isFirstBoss then
            BossRushHelper.StartNewBossRushTimer() 
        end

        bossDead = BossRushHelper.WaitForBossDeathByName(bossName)
        
        if not bossDead then 
            AddTrueDeathCount()
            SetTextEffect(16)
            if not InfiniteLives then --you dun goofed, kiddo
                --Wait for die and respawn
                WaitForLoadStart()
                WaitForLoadEnd()
                FadeIn()
                isGoingToContinue = FailBossRush() --asks user if they wanna retry
                if isGoingToContinue then
                    BeginBossRush()
                end
                return false --even if they retry we have to return after calling BeginBossRush recursively
            else
                Wait(5000)
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
    SetBriefingMsg("You are invincible while the boss rush mod asks you questions and between bosses.")

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
        Proc.WInt32(Proc.RInt32(0x13786D0) + 0x154, -1) 
        Proc.WInt32(Proc.RInt32(0x13786D0) + 0x158, -1) 
        SetBriefingMsg("! NO LONGER INVINCIBLE !")
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

    bossOrder = BossRushHelper.GetBossRushOrder(BossRushOrder, BossRushExcludeBed, BossRushCustomOrder)
    
    for i=0,bossOrder.Length-1 do
        continueRush = FightBoss(bossOrder[i], isFirstBoss)
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