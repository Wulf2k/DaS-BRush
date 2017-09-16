function RInt8(addr) return 0; end
function RInt16(addr) return 0; end
function RInt32(addr) return 0; end
function RInt464(addr) return 0; end

function RUInt8(addr) return 0; end
function RUInt16(addr) return 0; end
function RUInt32(addr) return 0; end
function RUInt64(addr) return 0; end

function RFloat(addr) return 0.0; end
function RDouble(addr) return 0.0; end
function RIntPtr(addr) return 0; end

function RBytes(addr) return {}; end
function RByte(addr) return 0; end

function RBool(addr) return false; end

function RAsciiStr(addr) return ""; end
function RUnicodeStr(addr) return ""; end







function WInt8(addr, val) end
function WInt16(addr, val) end
function WInt32(addr, val) end
function WInt464(addr, val) end

function WUInt8(addr, val) end
function WUInt16(addr, val) end
function WUInt32(addr, val) end
function WUInt64(addr, val) end

function WFloat(addr, val) end
function WDouble(addr, val) end
function WIntPtr(addr, val) end

function WBytes(addr, val) end
function WByte(addr, val)end

function WBool(addr, val) end

function WAsciiStr(addr, val) end
function WUnicodeStr(addr, val) end