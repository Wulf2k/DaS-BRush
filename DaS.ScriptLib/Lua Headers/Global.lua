--The local player character. Feel free to access at any time without fear of crashing the game, but nothing will happen if the player isn't loaded ;)
player = Entity

--[[
    A list of all modules loaded by Dark Souls. 
    Module["DARKSOULS.EXE"][1] + 0x1337 is the same as "DARKSOULS.EXE+0x1337" in Cheat Engine, for example.
]]
Module = {["ModuleName"] = {0x00000000, 0xFFFFFFFF --[[etc]]}}







VOID = 0
BYTE = 1
SBYTE = 2
SHORT = 3
USHORT = 4
INT = 5
UINT = 6
LONG = 7
ULONG = 8
FLOAT = 9
DOUBLE = 10
BOOL = 11
STR_ANSI = 13
STR_UNI = 14

--Runs an in-game Lua function with the specified address and args. E.g. "FUNC(INT, 0x1234, {int(5), int(7), false});"
function FUNC(returnType, funcAddress, argTable) end

--Runs an in-game Lua function with the specified address, args, and x86 register values. E.g. "FUNC(INT, 0x1234, {int(5), int(7), false}, {"EAX" = 7, "ECX" = 0x12345678});"
function FUNC_REG(returnType, funcAddress, argTable) end

function trace(txt) end
--function unpack(table) end

--Forces a #number to be a signed 32-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function int() end

--Forces a #number to be an unsigned 32-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function uint() end

--Forces a #number to be a signed 16-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function short() end

--Forces a #number to be an usigned 16-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function ushort() end

--Forces a #number to be an usigned 8-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function byte() end

--Forces a #number to be a signed 8-bit integer. Only works for arguments passed to FUNC/FUNC_REG.
function sbyte() end

--Forces a #number to be a boolean value (1 or 0 input gives true or false, respectively). Only works for arguments passed to FUNC/FUNC_REG.
function bool() end

--Forces a #string to be an ANSI string stored in Dark Souls's working memory. Only works for arguments passed to FUNC/FUNC_REG.
function str_ansi() end

--Forces a #string to be a Unicode string stored in Dark Souls's working memory. Only works for arguments passed to FUNC/FUNC_REG.
function str_uni() end

































