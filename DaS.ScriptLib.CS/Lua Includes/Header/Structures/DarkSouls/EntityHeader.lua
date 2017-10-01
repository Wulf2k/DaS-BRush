---
--@type EntityHeaderFlagsA
EntityHeaderFlagsA = {
    None = 0x0,
    Disabled = 0x1,
    PlayerHide = 0x4
}

---
--@type EntityHeader
--@field #EntityHeaderFlagsA FlagsA
EntityHeader = {
    Size = 0x20,
    Pointer = 0,
    CloneValue = 0,
    EntityPtr = 0,
    Entity = Entity(),
    UnknownPtr1 = 0,
    FlagsA = EntityHeaderFlagsA.None,
    LocationPtr = 0,
    Location = EntityLocation(),
    UnknownPtr2 = 0,
    DeadFlag = false,
    --Flags
    PlayerHide = false,
    Disabled = false,
}

---
--@param #Entityheader other
function EntityHeader:CopyFrom(other) end