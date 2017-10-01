---
--@type LuaThreadStatus
LuaThreadStatus = {
    Init = 0,
    Running = 1,
    Aborted = 2,
    Finished = 3,
}

---
--@type LuaThread
--@field [parent=#LuaThread] #LuaThreadStatus Status
LuaThread = {
    Status = LuaThreadStatus,
    IsRunning = false,
}


---
--Creates a new LuaThread with the specified function value.
--@callof #LuaThread
--@param #NLua.LuaFunction luaFunction
--@return #LuaThread The LuaThread created.
function LuaThread.__call(luaFunction) return LuaThread; end


---
--Awaits the completion of this LuaThread's Function, returning the inner function's return value(s) if applicable 
--Returns the function's last result immediately if it finished previously and *isn't currently running* 
--(if the function starts running a second time before you had a chance to get the return value, **it will instead begin waiting for the *second* return value!**).
--@param self
--@return #nil, #clr.Array The result of the function, if applicable (**as an array declared in CLR**; ***see the clr.Array type's documentation for more info***); 
--
--Returns `nil` (or possibly a clr.Array of size 0, not 100% sure) if the function does not return any values. 
function LuaThread:Await() return {}; end


---
--Starts this LuaThread's execution. Will silently fail if this LuaThread has already been started previously and is currently running.
--@param self
--@param params The parameter(s) to pass.
--
--A single table will be passed as ***MULTIPLE ARGUMENTS***, rather than a table!
-- 
--If you must pass a whole table as one of the function's parameters, put it inside of a surrounding table (e.g. `{{table,Items,Here}}`).
--
--If you must pass NO arguments, pass one empty table: `{}`
--(and of course, if you need to pass an empty table as one of your arguments, 
--put it inside of a surrounding table.
--
--@return #boolean True if this call actually started the LuaThread. False if it silently failed due to the LuaThread still running from a previous call.
function LuaThread:Start(params) return false; end


---
--Abort's this LuaThread's execution, if applicable; If the LuaThread instance is not currently executing, this function silently fails.
--@param self
--@return #boolean True if this call actually aborted the LuaThread. False if it silently failed due to the LuaThread not running currently.
function LuaThread:Abort() return false; end








