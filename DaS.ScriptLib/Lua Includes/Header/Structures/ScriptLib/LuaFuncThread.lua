---
--@type LuaFuncThread
LuaFuncThread = {
    HasReturned = false,
    Running = false,
}

function LuaFuncThread:StartThread(params) end

function LuaFuncThread:AbortThread() end