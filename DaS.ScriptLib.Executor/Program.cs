using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaS.ScriptLib.Executor
{
    class Program
    {
        static void Main(string[] args)
        {
            Injection.Hook.Init();
            if (File.Exists(args[0]) && Injection.Hook.DARKSOULS.Attached)
            {
                using (var luai = new Lua.LuaInterface())
                {
                    try
                    {
                        string script = File.ReadAllText(args[0]);
                        luai.State.DoString(script);

                        while (luai.State.IsExecuting)
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("Could not attach to Dark Souls.");
            }

        }
    }
}
