using System;
using System.Linq;
using DaS.ScriptLib.Injection;
using Neo.IronLua;

namespace DaS.ScriptLib.LuaScripting.Module
{
    public static class Utils
    {
        public const string TableName = "Utils";
        public const double MinLoadingScreenDur = 3.0;

        public static bool BitmaskCheck(ulong input, ulong mask)
        {
            return ((input & mask) == mask);
        }

        //Function WAIT_FOR_GAME()
        //    ingameTimeStopped = True
        //    repeat
        //    ingameTime = RInt32(RInt32(0x1378700) + 0x68)
        //    ingameTimeStopped = (ingameTime == prevIngameTime)
        //    prevIngameTime = ingameTime
        //    until(Not ingameTimeStopped)
        //    End

        public static void WaitForGame()
        {
            int curIngameTime = 0;
            int prevIngameTime = 0;
            bool ingameTimeStopped = true;
            do
            {
                int ingameTimePointer = Hook.RInt32(0x1378700);
                if (ingameTimePointer == 0)
                    continue;

                curIngameTime = Hook.RInt32(ingameTimePointer + 0x68);
                if (curIngameTime == 0)
                    continue;

                ingameTimeStopped = (prevIngameTime == 0 || prevIngameTime == curIngameTime);

                prevIngameTime = curIngameTime;

                Funcs.Wait(16);
            } while (ingameTimeStopped);
        }

        public static double WaitForGameAndMeasureDuration()
        {
            var startTime = DateTime.Now;
            WaitForGame();

            return (1.0 * DateTime.Now.Subtract(startTime).Ticks / TimeSpan.TicksPerSecond);
        }

        public static void WaitUntilAfterNextLoadingScreen()
        {
            double waitDuration = 0;
            do
            {
                waitDuration = WaitForGameAndMeasureDuration();
            } while (!(waitDuration >= MinLoadingScreenDur));
        }

        public static uint GetIngameDllAddress(string moduleName)
        {
            uint[] modules = new uint[255];
            uint cbNeeded = 0;
            PSAPI.EnumProcessModules(Hook.DARKSOULS.GetHandle(), modules, 4 * 1024, ref cbNeeded);

            var numModules = cbNeeded / IntPtr.Size;

            for (int i = 0; i <= numModules - 1; i++)
            {
                var disModule = new IntPtr(modules[i]);
                System.Text.StringBuilder name = new System.Text.StringBuilder();
                PSAPI.GetModuleBaseName(Hook.DARKSOULS.GetHandle(), disModule, name, 255);

                if ((name.ToString().ToUpper().Equals(moduleName.ToUpper())))
                {
                    return modules[i];
                }
            }

            return 0;
        }

        private static readonly Type[] LuaCompatibleValueTypes = new Type[] {
            typeof(string),
            typeof(LuaMethod),
            typeof(LuaTable),
            typeof(float),
            typeof(double),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(bool)
        };

        public static void BreakViewTable(LuaTable table)
        {
            throw new Exception("BreakViewTable called.");
        }

        public static LuaTable GetClrObjMembers(object obj)
        {
            return GetClrObjMembers_Internal(obj, 0);
        }

        private static LuaTable GetClrObjMembers_Internal(object obj, int recursion)
        {
            var tbl = new LuaTable();

            if (obj == null)
            {
                return tbl;
            }
            else if (recursion > 3)
            {
                tbl[1] = "[REACHED MAX RECURSION]";
                return tbl;
            }

            Type objType = obj.GetType();

            if (obj is Type)
            {
                tbl[1] = "Type: " + ((Type)obj).Name;
                return tbl;
            }

            var fieldArr = objType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.FieldInfo field in fieldArr)
            {
                tbl[field.Name] = LuaCompatibleValueTypes.Contains(field.FieldType) ? field.GetValue(obj) : GetClrObjMembers_Internal(field.GetValue(obj), recursion + 1);
            }

            var propArr = objType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.PropertyInfo prop in propArr)
            {
                if (prop.GetIndexParameters().Length > 0)
                {
                    tbl[prop.Name] = "Indexed Property: " + prop.PropertyType.Name + "[";
                }
                else
                {
                    object propVal = "[ERROR GETTING VALUE]";

                    try
                    {
                        propVal = prop.GetValue(obj);

                    }
                    catch (Exception) { }

                    tbl[prop.Name] = LuaCompatibleValueTypes.Contains(prop.PropertyType) ? propVal : GetClrObjMembers_Internal(propVal, recursion + 1);
                }
            }

            return tbl;
        }
    }
}