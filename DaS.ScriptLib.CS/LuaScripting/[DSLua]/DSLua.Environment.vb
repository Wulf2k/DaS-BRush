Imports System.Threading
Imports Neo.IronLua
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.LuaScripting.Structures
Imports System.Reflection

Namespace LuaScripting

    Partial Public Class DSLua
        Public Class Environment

            Public Shared ReadOnly ImportedNamespaces As String() = {
                "DaS.ScriptLib.Game.Data",
                "DaS.ScriptLib.Game.Data.Helpers",
                "DaS.ScriptLib.Game.Data.Structures",
                "DaS.ScriptLib.Game.Mem",
                "DaS.ScriptLib.Lua.Structures", 'TODO: Make sure this doesn't give end-user access to something dangerous lol
                "System"
            }

            Private Shared _importedTypes As New List(Of Type)

            Public Shared ReadOnly Property ImportedTypes As IReadOnlyCollection(Of Type)
                Get
                    Return _importedTypes.AsReadOnly()
                End Get
            End Property

            Private Class HelperFuncs

                Public Shared Temp_AllLoadedTypes As List(Of Type)

                Public Shared Sub TryImportType(typ As Type)
                    If Not _importedTypes.Contains(typ) Then
                        'G.DoChunk($"const {typ.Name} typeof {typ.FullName}", "DSLua.Environment.HelperFuncs.ImportType()")
                        G.DoChunk($"{typ.Name} = clr.{typ.FullName}", "DSLua.Environment.HelperFuncs.ImportType()")
                        'LuaType.RegisterTypeAlias(typ.Name, typ) 'TODO: CHECK IF WORKS OR NO
                        _importedTypes.Add(typ)
                    End If
                End Sub

                Public Shared Sub ImportAllTypesInNamespace(ns As String)
                    For Each typ In Temp_AllLoadedTypes
                        'Temporary "work" "around"
                        If typ.IsGenericType OrElse typ.Name.Contains("<"c) OrElse typ.Name.Contains(">"c) OrElse typ.Name.Contains("+"c) OrElse
                            typ.FullName.Contains("<"c) OrElse typ.FullName.Contains(">"c) OrElse typ.FullName.Contains("+"c) Then
                            Continue For
                        End If

                        If typ.Namespace = ns Then
                            TryImportType(typ)
                        End If
                    Next
                End Sub

                Public Shared Function MakeDelegateFromMethodInfo(m As MethodInfo, Optional targetInstance As Object = Nothing) As [Delegate]

                    Dim getTypeDel As Func(Of Type(), Type)
                    Dim isAction = m.ReturnType.Equals(GetType(Void))
                    Dim types = m.GetParameters().Select(Function(p) p.ParameterType)

                    If isAction Then
                        getTypeDel = AddressOf Expressions.Expression.GetActionType
                    Else
                        getTypeDel = AddressOf Expressions.Expression.GetFuncType
                        types = types.Concat(New Type() {m.ReturnType})
                    End If

                    If m.IsStatic Then
                        Return [Delegate].CreateDelegate(getTypeDel(types.ToArray()), m)
                    Else
                        Return [Delegate].CreateDelegate(getTypeDel(types.ToArray()), targetInstance, m.Name)
                    End If

                End Function

                Public Shared Sub GlobalImportAllMethodsInType(typ As Type)

                    For Each m In typ.GetMethods(BindingFlags.Public Or BindingFlags.Static Or BindingFlags.DeclaredOnly)
                        G(m.Name) = MakeDelegateFromMethodInfo(m)
                    Next

                End Sub

            End Class

            Private Shared Sub LoadGlobalInstanceVars()
                HelperFuncs.GlobalImportAllMethodsInType(GetType(Funcs))
                HelperFuncs.GlobalImportAllMethodsInType(GetType(Hook))

                'DG.FUNC = New Action(Of FuncReturnType, Integer, LuaTable)(AddressOf CallIngameFunc_FromLua)
                DG.FUNC = HelperFuncs.MakeDelegateFromMethodInfo(GetType(DSLua).GetMethod("CallIngameFunc_FromLua"))
                'Backup plans in case the above doesn't work:
                'G.DefineMethod("FUNC", Function(r, f, a) CallIngameFunc_FromLua(r, f, a))
                'G.DefineMethod("FUNC", New Action(Of FuncReturnType, Integer, LuaTable)(AddressOf CallIngameFunc_FromLua))

                'DG.FUNC_REG = New Action(Of FuncReturnType, Integer, LuaTable, LuaTable)(AddressOf CallIngameFuncREG_FromLua)
                DG.FUNC_REG = HelperFuncs.MakeDelegateFromMethodInfo(GetType(DSLua).GetMethod("CallIngameFuncREG_FromLua"))
                G.DoChunk("player = Entity.GetPlayer()", "player")
                DG.Utils = New Utils
            End Sub

            Friend Shared Sub Init()
                L = New Lua(LuaIntegerType.Int32, LuaFloatType.Float)
                G = L.CreateEnvironment(Of LuaGlobal)()

                StartTime = DateTime.Now

                HelperFuncs.Temp_AllLoadedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(Function(t) t.GetTypes()).ToList()

                For Each ns In ImportedNamespaces
                    HelperFuncs.ImportAllTypesInNamespace(ns)
                Next

                HelperFuncs.Temp_AllLoadedTypes.Clear()
                HelperFuncs.Temp_AllLoadedTypes = Nothing

                'HelperFuncs is unusable now since its unloaded

                DG.Module = DARKSOULS.ModuleOffsets

                'DG.Module = New LuaTable()
                'If DARKSOULS.ModuleOffsets IsNot Nothing Then
                '    For Each dll In DARKSOULS.ModuleOffsets
                '        DG.Module($"{dll.Key}") = New LuaTable()

                '        For i = 1 To dll.Value.Count
                '            DG.Module($"{dll.Key}").Item(i) = dll.Value(i - 1)
                '        Next
                '    Next
                'End If


                'Now that's what I call redundant LUL
                Dim retTypEnumVals = [Enum].GetValues(GetType(FuncReturnType)).Cast(Of FuncReturnType)().ToArray()

                For Each retTyp In retTypEnumVals
                    G(retTyp.ToString()) = CType(retTyp, Integer)
                Next

                Dim allEmbeddedLuaSourceFiles = ScriptLibResources.EmbeddedResourceNames.Where(
                    Function(x)
                        Dim u = x.ToUpper()
                        Return u.StartsWith("SOURCE.") AndAlso u.EndsWith(".LUA")
                    End Function)

                For Each scriptName In allEmbeddedLuaSourceFiles
                    G.DoChunk(ScriptLibResources.GetEmbeddedTextResource(scriptName), scriptName)
                Next

                LoadGlobalInstanceVars()
            End Sub

        End Class
    End Class

End Namespace
