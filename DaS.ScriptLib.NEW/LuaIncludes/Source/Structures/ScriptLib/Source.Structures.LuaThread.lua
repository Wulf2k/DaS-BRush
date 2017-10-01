
---
--Creates and returns a new LuaThread object, which can run the specified Lua function in a background thread.
--
--Note: if you wish to both create *and start executing* a LuaThread at the same time, use the `runthread` function.
--@param #NLua.LuaFunction luaFunction The Lua function which this LuaThread will be able to call.
--@return #LuaThread the LuaThread created by this function call.
function thread(luaFunction)
    return LuaThread(luaFunction);
end

---
--Creates a new LuaThread object, **starts executing it immediately**, and returns the aformentioned LuaThread object.
--@param #NLua.LuaFunction luaFunction The Lua function which this LuaThread will call.
--@param ... Any parameter(s) (if applicable) to pass upon calling the function passed as luaFunction.
--@return #LuaThread the LuaThread created by this function call.
function runthread(luaFunction, ...)
    newLuaThread = LuaThread(luaFunction);
    newLuaThread:Start({...});
    return newLuaThread;
end
















