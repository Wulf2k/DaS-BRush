using DaS.ScriptLib.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaS.ScriptLib.QuickRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var luai = new LuaInterface();

            luai.State.DoString(String.Join("\n\n", args));
        }
    }
}
