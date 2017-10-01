using System;

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
