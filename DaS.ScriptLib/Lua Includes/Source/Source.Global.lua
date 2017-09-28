---
--@param #table tbl The table to dump data from
--@param #string indentStr use ""
--@param #table seen use {}
function dump(tbl,indentStr,seen)
  local s={}
  local n=0
  for k in pairs(tbl) do
    n=n+1 s[n]=k
  end
  table.sort(s)
  for k,v in ipairs(s) do
    val=tbl[v]
    print(indentStr,v,"=",val)
    
    if type(val) == "table" then
      if not seen[val] then
        seen[val] = true
        dump(val,indentStr.."\t",seen)
      end
      
    elseif type(val) == "userdata" then
      udataMembers = Utils:GetClrObjMembers(val)
      clrTbl = {}
      if type(udataMembers) == "table" then
        clrTbl = udataMembers
      else
        clrTbl = {udataMembers}
      end
      
      if not seen[clrTbl] then
        seen[clrTbl] = true
        dump(clrTbl,indentStr.."\t",seen);
      end
      
    end
  end
end

function PrintAllGlobals()
  dump(_G,"")
end