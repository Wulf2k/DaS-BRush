Utils = {}

function Utils:BitmaskCheck(input, mask) return false; end
function Utils:WaitForGame() end
function Utils:WaitForGameAndMeasureDuration() return 0; end
function Utils:WaitUntilAfterNextLoadingScreen() end
function Utils:GetIngameDllAddress(moduleName) return 0; end
---
--@param #LUA lua
function Utils:GetClrObjMembers(lua, obj) return {}; end
function Utils:BreakViewTable(tbl) end