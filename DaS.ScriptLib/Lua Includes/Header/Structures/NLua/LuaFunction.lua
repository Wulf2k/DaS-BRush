
---
--The CLR languages' shared representation of ***all Lua functions*** (e.g. `function() end`)
--@type NLua.LuaFunction
NLua_LuaFunction = {
    
}

---
--Calls this Lua function.
--@param self
--@param #clr.Array args A CLR-declared array of arguments to pass to the function
--@return #clr.Array
function NLua_LuaFunction:Call(args) return {}; end