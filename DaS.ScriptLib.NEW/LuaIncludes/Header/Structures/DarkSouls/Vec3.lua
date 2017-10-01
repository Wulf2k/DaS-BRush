--TODO: Documentation
Vec3 = {
    X = 0, 
    Y = 0, 
    Z = 0,
    GetLateralUnit = function(angle) end
}
function Vec3:Plus(v) end
function Vec3:Minus(v) end
function Vec3:Times(v) end
function Vec3:DividedBy(v) end
function Vec3:MagnitudeSquared() end
function Vec3:Magnitude() end
function Vec3:DistanceSquaredTo(v) end
function Vec3:DistanceTo(v) end
function Vec3:GetLateralAngleTo(v) end
function Vec3:GetLateralUnit() end