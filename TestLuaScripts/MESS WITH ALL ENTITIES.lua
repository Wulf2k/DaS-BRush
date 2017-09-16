minEntitySpeed = 0.05
maxEntitySpeed = 5

flipThreshLow = 0.25
flipThreshHigh = 1.75

entityRandScale = 5 --0.5

prevIngameTime = 0

cycles = 0

--Makes camera follow the correct distance behind player rather than trailing you closer on higher framerates.
applyCameraMovementFixForFramerate = true

--Only relevant if applyCameraMovementFixForFramerate == true
cameraMovementFixTargetFramerate = 60

--This makes enemies unkillable just fyi ;)
DEBUG_VIEW = false

--[[
local Rand_A1, Rand_A2 = 727595, 798405  -- 5^17=D20*A1+A2
local Rand_D20, Rand_D40 = 1048576, 1099511627776  -- 2^20, 2^40
local Rand_X1, Rand_X2 = 0, 1

function PopSomeRandos()
    for i = 2, math.random(2, 3 + math.floor(20 * math.random())) do 
        math.random()
    end
end

function RandShiftUpOrDown1(val)
    PopSomeRandos()
    if math.random() >= 0.5 then
        return val + 1
    else
        return val - 1
    end
end

function SEED_RAND()
    --0 <= X1 <= 2^20-1, 1 <= X2 <= 2^20-1 (must be odd!)
    
    math.randomseed(os.time())
    
    PopSomeRandos()
    Rand_X1 = math.random(0, (2^20) - 1)
    
    PopSomeRandos()
    Rand_X2 = math.random(0, (2^20) - 1)
    
    if Rand_X1 % 2 == 0 then Rand_X1 = RandShiftUpOrDown1(Rand_X1); end
    if Rand_X2 % 2 == 0 then Rand_X2 = RandShiftUpOrDown1(Rand_X2); end
end

function RAND()
    local U = Rand_X2*Rand_A2
    local V = (Rand_X1*Rand_A2 + Rand_X2*Rand_A1) % Rand_D20
    V = (V*Rand_D20 + U) % Rand_D40
    Rand_X1 = math.floor(V/Rand_D20)
    Rand_X2 = V - Rand_X1*Rand_D20
    return (((V/Rand_D40) - 0.5) * 2)
end
--]]


function InitEntityTable()
    entityTable = nil
    entityTable = {}
    GetAllApparentEntities(entityTable)
end

InitEntityTable()

function RAND()
    --math.randomseed(os.time() + os.clock() + math.random())
    return ((math.random() - 0.5) * 2)
    
end

--SEED_RAND()
math.randomseed(os.time())
--prevIngameTimeInner = 0

--[[
function WAIT_FOR_GAME()
    ingameTimeStopped = true
    repeat
        ingameTime = RInt32(RInt32(0x1378700) + 0x68)
        ingameTimeStopped = (ingameTime == prevIngameTime)
        prevIngameTime = ingameTime
    until (not ingameTimeStopped)
end
--]]

function FixCameraMovementForCustomFramerate(fps)
    --WFloat(RInt32(RInt32(RInt32(0x137D6DC) + 0x3c) + 0x60) + 0x190, 0.1 / (fps / 30.0))
    WFloat(RInt32(RInt32(RInt32(0x137D6DC) + 0x3c) + 0x60) + 0x190, math.min(math.max(0.1 * (1 + (RAND() * 0.05)), 0.1), 1))
end

function DoCameraMovementFix()
    FixCameraMovementForCustomFramerate(math.max(cameraMovementFixTargetFramerate or 1, 1))
end

ingameTime = 0

function RandomizeAllEntitySpeeds()
    for i,j in ipairs(entityTable) do
        --[[
        if Utils:WaitForGameAndMeasureDuration() > 0.1 then
            Wait(5000)
            WaitForLoadEnd();
            OnLoadingScreenEnd();
        end
        --]]
        if i > #entityTable then
            break
        end
        entityTable[i].AnimationSpeed = math.max(math.min(entityTable[i].AnimationSpeed * (1 + (RAND() * entityRandScale)), maxEntitySpeed), minEntitySpeed)
        if DEBUG_VIEW then 
            SetKeyGuideText(string.format("Randomizing entity %.4d of %.4d (Overall cycles: %.8d, IGT: %.4f)", i, #entityTable, cycles, ingameTime)); 
            entityTable[i].NoDamage = true
            entityTable[i].NoDead = true
            entityTable[i].HP = 1 + ((entityTable[i].AnimationSpeed - minEntitySpeed) / (maxEntitySpeed - minEntitySpeed)) * (entityTable[i].MaxHP - 1);
        end
        
        --if applyCameraMovementFixForFramerate then DoCameraMovementFix() end
    end
end

function OnLoadingScreenEnd()
    cycles = 0
    InitEntityTable();
    SetKeyGuideTextPos(220, 150);
end

--Checks if the Player exists by checking whether the static CharData1 pointer has a value > 0
function PlayerExists()
    return RInt32(RInt32(0x137DC70) + 0x4) ~= 0
end

if playere

function MainLoop()
    InitEntityTable();
    if PlayerExists() then
        RandomizeAllEntitySpeeds()
        
        if DEBUG_VIEW then 
            SetDeadMode2(10000, true)
            player.SetDeadMode = true
            player.EnableInvincible = true
            player.NoDead = true
            player.NoDamage = true
            player.HP = player.MaxHP
        end
    else
        while not PlayerExists() do end
        Wait(33)
        Utils:WaitForGame()
        WaitForLoadEnd();
        OnLoadingScreenEnd();
    end
    
    if applyCameraMovementFixForFramerate then DoCameraMovementFix() end
    
    cycles = cycles + 1
end

--[[
if DEBUG_VIEW then
    origRandomizeIndividualEntitySpeed = RandomizeIndividualEntitySpeed
    RandomizeIndividualEntitySpeed = function(i)
        origRandomizeIndividualEntitySpeed(i);
        
        --entityTable[i].NoDamage = true;
        --entityTable[i].NoDead = true;
        
        --entityTable[i].HP = 1 + ((entityTable[i].AnimationSpeed - minEntitySpeed) / (maxEntitySpeed - minEntitySpeed)) * (entityTable[i].MaxHP - 1);
        
        SetKeyGuideText(string.format("Randomizing entity %.4d of %.4d (Overall cycles: %.8d)", i, #entityTable, cycles));
    end
    
    origMainLoop = MainLoop
    MainLoop = function() origMainLoop(); player.NoDead = true; player.NoDamage = true; end
end
--]]
while true do MainLoop() end


