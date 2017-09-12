using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaS.ScriptEditor.NEW
{
    public class LuaTabEventArgs : EventArgs
    {
        public readonly LuaScriptTab Script;

        public LuaTabEventArgs(LuaScriptTab script)
        {
            Script = script;
        }
    }
}
