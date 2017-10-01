using DaS.ScriptLib.Injection;
using DaS.ScriptLib.LuaScripting.Structures;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DaS.ScriptLib.LuaScripting
{
    public partial class DSLua
    {
        public class Environment
        {
            public static readonly string[] ImportedNamespaces = {
                "DaS.ScriptLib.Game.Data",
                "DaS.ScriptLib.Game.Data.Helpers",
                "DaS.ScriptLib.Game.Data.Structures",
                "DaS.ScriptLib.Game.Mem",
                "DaS.ScriptLib.LuaScripting.Structures",
                "DaS.ScriptLib.LuaScripting.Module",


                "System"
            };

            private static List<Type> _importedTypes = new List<Type>();
            public static IReadOnlyCollection<Type> ImportedTypes
            {
                get { return _importedTypes.AsReadOnly(); }
            }

            private class HelperFuncs
            {
                public static List<Type> Temp_AllLoadedTypes;
                public static void TryImportType(Type typ)
                {
                    if (!_importedTypes.Contains(typ))
                    {
                        try
                        {
                            //G.DoChunk($"const {typ.Name} typeof {typ.FullName}", "DSLua.Environment.HelperFuncs.ImportType()");
                            G.DoChunk($"{typ.Name} = clr.{typ.FullName}", "DSLua.Environment.HelperFuncs.ImportType()");
                        }
                        catch(Exception e)
                        {
                            throw e;
                        }
                       
                        //LuaType.RegisterTypeAlias(typ.Name, typ); //TODO: CHECK IF WORKS OR NO
                        _importedTypes.Add(typ);
                    }
                }

                public static void ImportAllTypesInNamespace(string ns)
                {
                    foreach (Type typ in Temp_AllLoadedTypes)
                    {
                        //Temporary "work" "around"
                        if (typ.IsGenericType || typ.Name.Contains('<') || typ.Name.Contains('>') || typ.Name.Contains('+') || typ.FullName.Contains('<') || typ.FullName.Contains('>') || typ.FullName.Contains('+'))
                        {
                            continue;
                        }

                        if (typ.Namespace == ns)
                        {
                            TryImportType(typ);
                        }
                    }
                }

                public static Delegate MakeDelegateFromMethodInfo(MethodInfo m, object targetInstance = null)
                {

                    Func<Type[], Type> getTypeDel = null;
                    bool isAction = m.ReturnType.Equals(typeof(void));
                    var types = m.GetParameters().Select(p => p.ParameterType);

                    if (isAction)
                    {
                        getTypeDel = System.Linq.Expressions.Expression.GetActionType;
                    }
                    else
                    {
                        getTypeDel = System.Linq.Expressions.Expression.GetFuncType;
                        types = types.Concat(new Type[] { m.ReturnType });
                    }

                    if (m.IsStatic)
                    {
                        return Delegate.CreateDelegate(getTypeDel(types.ToArray()), m);
                    }
                    else
                    {
                        return Delegate.CreateDelegate(getTypeDel(types.ToArray()), targetInstance, m.Name);
                    }

                }

                public static void GlobalImportAllMethodsInType(Type typ)
                {
                    foreach (MethodInfo m in typ.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    {
                        G[m.Name] = MakeDelegateFromMethodInfo(m);
                    }
                }
            }

            private static void LoadGlobalInstanceVars()
            {
                HelperFuncs.GlobalImportAllMethodsInType(typeof(Funcs));
                HelperFuncs.GlobalImportAllMethodsInType(typeof(Hook));

                //DG.FUNC = new Action<FuncReturnType, int, LuaTable>(CallIngameFunc_FromLua);
                DG.FUNC = HelperFuncs.MakeDelegateFromMethodInfo(typeof(DSLua).GetMethod("CallIngameFunc_FromLua"));
                //Backup plans in case the above doesn't work:
                //G.DefineMethod("FUNC", (r, f, a) => CallIngameFunc_FromLua(r, f, a));
                //G.DefineMethod("FUNC", new Action<FuncReturnType, int, LuaTable>(CallIngameFunc_FromLua));

                //DG.FUNC_REG = new Action<FuncReturnType, int, LuaTable, LuaTable>(CallIngameFuncREG_FromLua);
                DG.FUNC_REG = HelperFuncs.MakeDelegateFromMethodInfo(typeof(DSLua).GetMethod("CallIngameFuncREG_FromLua"));
            }

            static internal void Init()
            {
                L = new Lua(LuaIntegerType.Int32, LuaFloatType.Float);
                G = L.CreateEnvironment<LuaGlobal>();

                StartTime = DateTime.Now;

                DG.Clock = (Func<double>)Clock;

                HelperFuncs.Temp_AllLoadedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(t => t.GetTypes()).ToList();

                foreach (string ns in ImportedNamespaces)
                {
                    HelperFuncs.ImportAllTypesInNamespace(ns);
                }

                HelperFuncs.Temp_AllLoadedTypes.Clear();
                HelperFuncs.Temp_AllLoadedTypes = null;

                //HelperFuncs is unusable now since its unloaded

                DG.Module = Hook.DARKSOULS.ModuleOffsets;

                //DG.Module = new LuaTable();
                //if (Hook.DARKSOULS.ModuleOffsets != null)
                //{
                //    foreach (var dll in Hook.DARKSOULS.ModuleOffsets)
                //    {
                //        DG.Module[$"{dll.Key}"] = new LuaTable();
                //        For (int i = 1; i <= dll.Value.Count; i++)
                //        {
                //            DG.Module[$"{dll.Key}"][i] = dll.Value[i - 1];
                //        }
                //    }
                //}


                //Now that's what I call redundant LUL
                var retTypEnumVals = Enum.GetValues(typeof(FuncReturnType)).Cast<FuncReturnType>().ToArray();

                foreach (FuncReturnType retTyp in retTypEnumVals)
                {
                    G[retTyp.ToString()] = (int)retTyp;
                }

                var allEmbeddedLuaSourceFiles = ScriptLibResources.EmbeddedResourceNames.Where(x =>
                {
                    var u = x.ToUpper();
                    return u.StartsWith("LUAINCLUDES.") && u.EndsWith(".LUA");
                });

                foreach (string scriptName in allEmbeddedLuaSourceFiles)
                {
                    G.DoChunk(ScriptLibResources.GetEmbeddedTextResource(scriptName), scriptName);
                }

                LoadGlobalInstanceVars();
            }
        }
    }
}