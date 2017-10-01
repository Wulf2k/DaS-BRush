---
--@type MapEntry
MapEntry = {
    Pointer = 0,
    PointerToBlockAndArea = 0,
    Block = 0,
    Area = 0,
    EntityCount = 0,
    StartOfEntityStruct = 0,
}

---
--@return #list <#EntityHeader>
function MapEntry:GetEntityHeaders() return {}; end

---
--@return #list <#Entity>
function MapEntry:GetEntities() return {}; end

---
--@return #list <#EntityLocation>
function MapEntry:GetEntityLocations() return {}; end