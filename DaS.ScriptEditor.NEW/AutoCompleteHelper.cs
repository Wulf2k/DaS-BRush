using DaS.ScriptLib.LuaScripting;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DaS.ScriptEditor.NEW
{
    public class AutoCompleteHelper
    {
        public static class Vals
        {
            public static readonly Type[] BasicTypes = { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
                typeof(long), typeof(ulong), typeof(bool), typeof(float), typeof(double), typeof(string) };

            public static IReadOnlyDictionary<Type, string> TypeDisplayNames = new Dictionary<Type, string>()
            {
                { typeof(Byte), "byte" },
                { typeof(SByte), "sbyte" },
                { typeof(Int16), "short" },
                { typeof(UInt16), "ushort" },
                { typeof(Int32), "int" },
                { typeof(UInt32), "uint" },
                { typeof(Int64), "long" },
                { typeof(UInt64), "ulong" },
                { typeof(Boolean), "bool" },
                { typeof(String), "string" },
                { typeof(Single), "float" },
                { typeof(Double), "double" },
                { typeof(LuaTable), "#table" },
            };
        }

        public static string GetFancyTypeName(Type t)
        {
            if (!t.IsGenericType)
                return Vals.TypeDisplayNames.ContainsKey(t) ? Vals.TypeDisplayNames[t] : t.Name;

            var sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.LastIndexOf('`')));
            sb.Append(t.GetGenericArguments().Aggregate("<", (string agg, Type typ) =>
                {
                    return agg + (agg == "<" ? "" : ",") + GetFancyTypeName(typ);
                }));
            sb.Append(">");

            return sb.ToString();
        }

        public static IEnumerable<KeyValuePair<string, object>> GetAllBasicVarsInTable(LuaTable t)
        {
            return t.Select(x => new KeyValuePair<string, object>(x.Key as string, x.Value)).Where(x => x.Key != null);
        }

        public static bool CheckIfTypeIsDelegate(Type t)
        {
            //<TemporaryGhettofication>
            var tFancyName = GetFancyTypeName(t);
            return (tFancyName.StartsWith("Delegate") || tFancyName.StartsWith("Action<") || tFancyName.StartsWith("Func<"));
            //</TemporaryGhettofication>

            //TODO: THIS METHOD (VERY IMPORTANT)
        }

        public static void EnumerateItemsInTable(MainWindow main, 
            ref List<SeAutoCompleteEntry> entries, 
            LuaTable t, 
            ref List<LuaTable> enumeratedTables, 
            ref List<object> enumeratedClrObjects, 
            string memberPrefix)
        {
            var basicVars = GetAllBasicVarsInTable(t);
            foreach (var v in basicVars)
            {
                var vType = v.Value.GetType();
                var name = (!string.IsNullOrWhiteSpace(memberPrefix) ? memberPrefix + "." : "") + v.Key;

                SeAcType icon = SeAcType.Field;

                if (vType == typeof(LuaTable))
                {
                    icon = SeAcType.Type;
                    var vTable = v.Value as LuaTable;
                    if (!enumeratedTables.Contains(vTable))
                    {
                        EnumerateItemsInTable(main, ref entries, vTable, ref enumeratedTables, ref enumeratedClrObjects, name);
                    }
                }
                else if (Vals.BasicTypes.Contains(vType))
                {
                    icon = SeAcType.Field;
                }
                else if (CheckIfTypeIsDelegate(vType))
                {
                    icon = SeAcType.Method;
                    //TODO: THIS IF-BLOCK (VERY IMPORTANT)
                }
                else
                {
                    icon = SeAcType.Type;
                    if (!enumeratedClrObjects.Contains(v.Value))
                    {
                        var vClrTable = ScriptLib.LuaScripting.Structures.Utils.GlobalInstance.GetClrObjMembers(v.Value);
                        EnumerateItemsInTable(main, ref entries, vClrTable, ref enumeratedTables, ref enumeratedClrObjects, name);
                        enumeratedClrObjects.Add(v.Value);
                    }
                }

                var dispName = GetFancyTypeName(vType) + " " + name;
                entries.Add(new SeAutoCompleteEntry(main, icon, name, dispName, "Value obtained directly from Lua engine's environment and therefore has no description."));
            }

            enumeratedTables.Add(t);
        }

        public static void EnumerateLuaGlobals(MainWindow main, ref List<SeAutoCompleteEntry> entries)
        {
            var enumeratedTables = new List<LuaTable>();
            var enumeratedClrObjects = new List<object>();
            EnumerateItemsInTable(main, ref entries, DSLua.G, ref enumeratedTables, ref enumeratedClrObjects, null);
        }
    }
}
