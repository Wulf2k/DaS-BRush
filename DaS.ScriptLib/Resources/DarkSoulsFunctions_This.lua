--[[MultiplayerStruct *__thiscall sub_D600D0(void *this)
{
  MultiplayerStruct *result; // eax@1
  int v2; // esi@2
  int v3; // edi@3
  unsigned __int8 v4; // al@3
  char v5; // al@3
  int v6; // edi@3
  unsigned __int8 v7; // al@3
  void *v8; // [sp+4h] [bp-4h]@1

  v8 = this;
  result = MainMultiplayerObject;
  if ( MainMultiplayerObject )
  {
    v2 = *&MainMultiplayerObject->gap2cD[4];
    if ( v2 )
    {
      v3 = *v2;
      v4 = (*(*v2 + 740))(v2);
      v5 = (*(v3 + 748))(v2, v4);
      v6 = *v2;
      LOBYTE(v8) = v5 + 1;
      v7 = (*(*v2 + 740))(v2, v8);
      result = (*(v6 + 752))(v2, v7);
    }
  }
  return result;
}]]
function AddCurrentVowRankPoint()
    return FUNC(INT, 0x00D600D0, {});
end


function IsLoadWait()
    return FUNC(INT, 0x00D5D930, {});
end

function IsOnline()
    return FUNC(INT, 0x00D5E2D0, {});
end

--[[
function LuaCall()
    return FUNC(INT, 0x00D62C60, {});
end
---]]

--[[
function LuaCallStart()
    return FUNC(INT, 0x00D66290, {});
end
---]]

--[[
function LuaCallStartPlus()
    return FUNC(INT, 0x00D66260, {});
end
---]]