---
-- Location Structure
-- @type Loc
Loc = {
    Pos = Vec3,
    Rot = Heading,
    IsZero = false
}

function Loc.AngleTo(self, other) end