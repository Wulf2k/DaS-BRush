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
  dump(_G,"",{})
end

function inspect(item)
    local inspectForm : System.Windows.Forms.Form = clr.System.Windows.Forms.Form()
    inspectForm.MinimumSize = clr.System.Drawing.Size(256, 224) --SNES resolution because why not
    inspectForm.MinimizeBox = false
    inspectForm.ShowIcon = false
    inspectForm.ShowInTaskbar = false
    inspectForm.StartPosition = clr.System.Windows.Forms.FormStartPosition.CenterScreen
    inspectForm.Text = "Inspecting "..item:ToString()
    inspectForm.Size = clr.System.Drawing.Size(384, 672)
    
    local ctrlType : System.Type = clr.System.Windows.Forms.Control:GetType()
    local boolType : System.Type = clr.System.Boolean:GetType()
    local bndFlgs : System.Reflection.BindingFlags = bit32.bor(clr.System.Reflection.BindingFlags.NonPublic, clr.System.Reflection.BindingFlags.Instance)
    
    local prop : System.Reflection.PropertyInfo = ctrlType:GetProperty("DoubleBuffered", bndFlgs, nil, boolType, clr.System.Type[0], nil)
    prop:SetValue(inspectForm, true, clr.System.Reflection.BindingFlags.GetProperty, nil, nil, nil)
    
    local inspectPropertyGrid : System.Windows.Forms.PropertyGrid = clr.System.Windows.Forms.PropertyGrid()
    inspectPropertyGrid.Dock = clr.System.Windows.Forms.DockStyle.Fill
    inspectPropertyGrid.PropertySort = clr.System.Windows.Forms.PropertySort.NoSort
    
    inspectPropertyGrid.SelectedObject = item
    
    
    inspectForm.Controls:Add(inspectPropertyGrid)
    
    inspectForm:ShowDialog()
end