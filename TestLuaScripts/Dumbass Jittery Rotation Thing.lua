jitterIntensity = 25
useAcceleration = true 
angleSmoothingPercent = 95

accVelTameMult = 0.5 --helps keep the velocity from veering off in one direction for way too long


math.randomseed(os.time()) 

rotationalVelocity = 0
oldRotationalVelocity = 0

baseChance = 0.25

minWait = 1
maxWait = 1

minSpeedChangeWait = 333
maxSpeedChangeWait = 2333

minAnimSpeed = 0.73
maxAnimSpeed = 1.07

--Forces the animation to play, even if you're in the middle of attacking etc
forceRollAnimations = true

anim_estus = { 7586 }

anim_stumble = { 1560, 1570, 560, 1800, 2083, 2033, 7052 }

anim_cast = { 6200, 6201, 6202, 6203, 6204, 6205, 6206, 6207, 6208, 6209, 6210, 6211, 6212, 6213, 6214, 6215, 6216, 6217, 6218, 6219, 6220, 6221, 6222, 6224, 6225, 6226, 6299, 6300, 6302, 6303, 6304, 6305, 6306, 6307, 6308, 6309, 6310, 6311, 6312, 6313, 6314, 6315, 6316, 6317, 6318, 6319, 6320, 6322, 6324, 6325, 6326, 6399 }

anim_slowrecovery = { 1770, 1780, 1790, 2017, 2027, 2040, 7500 }

anim_extremelyslow = { 2087, 2088, 2045, 7061, 7063 }

function CheckRoll (chance, animIds)
    for i,anim in ipairs(animIds) do 
        if (math.random() * 100) < chance then
            if forceRollAnimations then
                ForcePlayAnimation(10000, anim)
            else
                PlayAnimation(10000, anim)
            end
            return true
        end
    end
    
    return false
end

function RandWaitThing(minDur, maxDur)
    Wait(math.floor(minDur + ((maxDur - minDur) * math.random())))
end

function CheckAllRolls ()
    if CheckRoll(baseChance / 2, anim_estus) then return end
    if CheckRoll(baseChance / 4, anim_stumble) then return end
    if CheckRoll(baseChance / 8, anim_slowrecovery)  then return end
    if CheckRoll(baseChance / 16, anim_extremelyslow)  then return end
    if CheckRoll(baseChance / 256, anim_cast)  then return end
end


function isLinearInterpolationReallyThisEasyToProgram(a, b, t)
    return(a + (b - a) * t)
end

function RotateItBrororo ()
    if useAcceleration == true then
      rotationalVelocity = (rotationalVelocity + ((math.random() - 0.5) * jitterIntensity)) * accVelTameMult
    else
      rotationalVelocity = ((math.random() - 0.5) * jitterIntensity)
    end
    
    smoothRotVel = isLinearInterpolationReallyThisEasyToProgram(rotationalVelocity, oldRotationalVelocity, angleSmoothingPercent / 100.0)
    
    
    SetEntityRotation(playerPtr, GetEntityRotation(playerPtr) + smoothRotVel)
    
    oldRotationalVelocity = smoothRotVel
end

prevOsClock = (os.clock() * 1000)
curOsClock = (os.clock() * 1000)

animChangeTimer = 0
animChangeTimerMax = 0

animSpeedChangeTimer = 0
animSpeedChangeTimerMax = 0

while true do
    playerPtr = ChrFadeIn(10000)
    RotateItBrororo()
    
    curOsClock = (os.clock() * 1000)
    
    deltaTime = (curOsClock - prevOsClock)
    
    animChangeTimer = animChangeTimer + deltaTime
    
    if (animChangeTimer >= animChangeTimerMax) then
        CheckAllRolls()
        animChangeTimer = 0
        animChangeTimerMax = (minWait + ((maxWait - minWait) * math.random()))
    end
    
    animSpeedChangeTimer = animSpeedChangeTimer + deltaTime
    
    if (animSpeedChangeTimer >= animSpeedChangeTimerMax) then
        Player.Anim.CurrentSpeedMult.Value = (minAnimSpeed + ((maxAnimSpeed - minAnimSpeed) * math.random()))
        animSpeedChangeTimer = 0
        animSpeedChangeTimerMax = (minSpeedChangeWait + ((maxSpeedChangeWait - minSpeedChangeWait) * math.random()))
    end
    
    prevOsClock = curOsClock
    
    Wait(16);
end