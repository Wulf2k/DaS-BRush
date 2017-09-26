---
--@type NLua.Lua
NLua_Lua = {
    IsExecuting = false,
    Globals = {},
}

--[[
---
--This will instantly close the Lua state object and prepare it for disposal, instantly crashing DaS.ScriptLib.Executor. Please do not call this.
function NLua_Lua:Close() end
]]

---
--@return #NLua.LuaFunction
function NLua_Lua:LoadString(chunk, name) end
---
--@return #NLua.LuaFunction
function NLua_Lua:LoadFile(name) end
function NLua_Lua:DoString(chunk, chunkName) end
function NLua_Lua:DoFile(fileName) end
---
--@return #number
function NLua_Lua:GetNumber(fullPath) end
---
--@return #string
function NLua_Lua:GetString(fullPath) end
---
--@return #table
function NLua_Lua:GetTable(fullPath) end
---
--@return #NLua.LuaFunction
function NLua_Lua:GetFunction(fullPath) end
function NLua_Lua:RegisterLuaDelegateType(delegateType, luaDelegateType) end
function NLua_Lua:RegisterLuaClassType(klass, luaClass) end

--[[
---
--Please don't call this; the CLR package will already be loaded at runtime and will never need to be loaded again.
function NLua_Lua:LoadCLRPackage() end
]]

--function NLua_Lua:GetFunction(delegateType, fullPath) end
function NLua_Lua:NewTable(fullPath) end
function NLua_Lua:GetTableDict(table) end
function NLua_Lua:SetDebugHook(mask, count) end
function NLua_Lua:RemoveDebugHook() end
function NLua_Lua:GetHookMask() end
function NLua_Lua:GetHookCount() end
function NLua_Lua:GetLocal(luaDebug, n) end
function NLua_Lua:SetLocal(luaDebug, n) end
function NLua_Lua:GetStack(level, ar) end
function NLua_Lua:GetInfo(what, ar) end
function NLua_Lua:GetUpValue(funcindex, n) end
function NLua_Lua:SetUpValue(funcindex, n) end
function NLua_Lua:Pop() end
function NLua_Lua:Push(value) end

--Not sure how to even handle this overload in Lua but whatever lol
--function NLua_Lua:RegisterFunction (path, clrFunction --[[name in NLua: "function"]]) end

function NLua_Lua:RegisterFunction (path, target, clrFunction --[[name in NLua: "function"]]) end


--[[

---
--This will dispose of the Lua state object and instantly crash DaS.ScriptLib.Executor. Please, just don't.
function NLua_Lua:Dispose() end

]]





