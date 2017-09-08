﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaS.ScriptEditor.NEW
{
    public class LuaTabSwitchEventArgs : EventArgs
    {
        public readonly LuaScriptTab Script;

        public LuaTabSwitchEventArgs(LuaScriptTab script)
        {
            Script = script;
        }
    }
}
