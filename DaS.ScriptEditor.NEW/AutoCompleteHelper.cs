using DaS.ScriptLib.LuaScripting;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
                { typeof(Object), "object" },
                { typeof(void), "void" }
            };
        }

        public class Config
        {
            public static bool UseTextBlockFormatting = true;
            public static bool UseMarkupSafeAngleBrackets = true;
            public static bool PutSpacesBetweenGenericParameters = true;
            public static bool PutSpacesBetweenMethodParameters = true;
            public static bool UseNeoLuaColonTypeSpecifierFormat = true;
        }

        public static string FormatHighlightBlue(string s)
        {
            return $@"<Run Foreground='DodgerBlue' FontWeight='SemiBold'>{s}</Run>";
        }

        public static string GetFancyTypeName(Type t)
        {
            if (!t.IsGenericType)
            {
                if (t.IsArray)
                {
                    var bracketStart = t.Name.IndexOf("[");

                    var beforeBrackets = t.Name.Substring(0, bracketStart);
                    var atBrackets = t.Name.Substring(bracketStart);

                    var elementType = t.GetElementType();

                    if (Vals.TypeDisplayNames.ContainsKey(elementType))
                        beforeBrackets = Vals.TypeDisplayNames[elementType];

                    return FormatHighlightBlue(beforeBrackets) + atBrackets;
                }
                    

                return FormatHighlightBlue(Vals.TypeDisplayNames.ContainsKey(t) ? Vals.TypeDisplayNames[t] : t.Name);
            }
                

            var openBracket = Config.UseMarkupSafeAngleBrackets ? "&lt;" : "<";
            var closeBracket = Config.UseMarkupSafeAngleBrackets ? "&gt;" : ">";

            var sb = new StringBuilder();
            sb.Append(FormatHighlightBlue(t.Name.Substring(0, t.Name.LastIndexOf('`'))));
            sb.Append(t.GetGenericArguments().Aggregate(openBracket, (string agg, Type typ) =>
                {
                    return agg + (agg == openBracket ? "" : (Config.PutSpacesBetweenGenericParameters ? ", " : ",")) + GetFancyTypeName(typ);
                }));
            sb.Append(closeBracket);

            return sb.ToString();
        }

        public static IEnumerable<KeyValuePair<string, object>> GetAllBasicVarsInTable(LuaTable t)
        {
            return t.Select(x => new KeyValuePair<string, object>(x.Key as string, x.Value)).Where(x => x.Key != null);
        }

        public static string GetTypedMember(string memberDisplayText, string typeDisplayText)
        {
            if (string.IsNullOrWhiteSpace(memberDisplayText?.Trim()))
                return typeDisplayText;

            if (Config.UseNeoLuaColonTypeSpecifierFormat)
            {
                if (typeDisplayText.Contains("LuaResult"))
                    return memberDisplayText;

                return memberDisplayText + " : " + typeDisplayText;
            }
            else
            {
                if (typeDisplayText.Contains("LuaResult"))
                    return "function " + memberDisplayText;

                return typeDisplayText + " " + memberDisplayText;
            }
        }

        public static string GetFancyParameterString(ParameterInfo p)
        {
            var sb = new StringBuilder();

            if (p.IsOptional)
                sb.Append("[");
            if (p.IsOut)
                sb.Append("out ");
            
            sb.Append(GetTypedMember(p.Name, GetFancyTypeName(p.ParameterType)));

            if (p.HasDefaultValue)
            {
                sb.Append(" = ");
                if (p.DefaultValue == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(p.DefaultValue.ToString());
                }
            }

            if (p.IsOptional)
                sb.Append("]");

            return sb.ToString();
        }

        public static string GetFancyMethodString(MethodInfo m, string methodName = null)
        {
            var paramStr = string.Join(Config.PutSpacesBetweenMethodParameters ? ", " : ",", 
                m.GetParameters()
                .Where(p => p.ParameterType != typeof(System.Runtime.CompilerServices.Closure))
                .Select(p => GetFancyParameterString(p)));
            return GetTypedMember($"{methodName ?? m.Name}({paramStr})", GetFancyTypeName(m.ReturnType));
        }

        public static void EnumerateItemsInTable(MainWindow main, 
            ref List<SeAutoCompleteEntry> entries, 
            LuaTable t, 
            ref List<LuaTable> enumeratedTables, 
            ref List<object> enumeratedClrObjects, 
            string memberPrefix,
            bool doRecursion)
        {
            var basicVars = GetAllBasicVarsInTable(t);
            foreach (var v in basicVars)
            {
                var vType = v.Value.GetType();
                var name = (!string.IsNullOrWhiteSpace(memberPrefix) ? memberPrefix + "." : "") + v.Key;
                string dispName = null;
                string desc = null;

                SeAcType icon = SeAcType.Field;
                if (vType == typeof(LuaTable))
                {
                    icon = SeAcType.Field;
                    var vTable = v.Value as LuaTable;
                    if (doRecursion && !enumeratedTables.Contains(vTable))
                    {
                        EnumerateItemsInTable(main, ref entries, vTable, ref enumeratedTables, ref enumeratedClrObjects, name, doRecursion);
                    }
                }
                else if (vType == typeof(LuaType))
                {
                    var vClrType = (v.Value as LuaType).Type;

                    string typeType = "type"; //I'm hilarious and original.

                    if (vClrType.IsClass) typeType = "class";
                    else if (vClrType.IsEnum) typeType = "enum";
                    else if (vClrType.IsInterface) typeType = "interface";

                    icon = SeAcType.Type;
                    dispName = GetTypedMember($"<Bold>{name}</Bold>", FormatHighlightBlue(typeType));
                }
                else if (Vals.BasicTypes.Contains(vType))
                {
                    icon = SeAcType.Field;
                }
                else if (typeof(Delegate).IsAssignableFrom(vType))
                {
                    icon = SeAcType.Method;
                    dispName = GetFancyMethodString((v.Value as Delegate).Method, v.Key);
                }
                else if (vType == typeof(LuaMethod))
                {
                    icon = SeAcType.Method;
                    dispName = GetFancyMethodString((v.Value as LuaMethod).Delegate.Method, v.Key);
                }
                else if (vType == typeof(LuaFilePackage) || vType == typeof(LuaLibraryPackage))
                {
                    icon = SeAcType.Lua;
                }
                else
                {
                    icon = SeAcType.Field;
                    if (doRecursion && !enumeratedClrObjects.Contains(v.Value))
                    {
                        var vClrTable = ScriptLib.LuaScripting.Module.Utils.GetClrObjMembers(v.Value);
                        EnumerateItemsInTable(main, ref entries, vClrTable, ref enumeratedTables, ref enumeratedClrObjects, name, doRecursion);
                        enumeratedClrObjects.Add(v.Value);
                    }
                }

                entries.Add(new SeAutoCompleteEntry(main, icon, name, dispName ?? GetTypedMember(name, GetFancyTypeName(vType)), 
                    desc ?? "Value obtained directly from Lua engine's environment and therefore has no description.", Config.UseTextBlockFormatting));
            }

            enumeratedTables.Add(t);
        }

        public static void EnumerateLuaGlobals(MainWindow main, ref List<SeAutoCompleteEntry> entries, bool doRecursion)
        {
            var enumeratedTables = new List<LuaTable>();
            var enumeratedClrObjects = new List<object>();
            EnumerateItemsInTable(main, ref entries, DSLua.G, ref enumeratedTables, ref enumeratedClrObjects, null, doRecursion);
        }
    }
}
