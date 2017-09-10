﻿Imports System.Threading
Imports NLua.Event
Imports DaS.ScriptLib.Injection
Imports System.Collections.ObjectModel
Imports DaS.ScriptLib.Lua.Structures

Namespace Lua
    Public Class LuaInterface
        Implements IDisposable

        Const INGAME_FUNC_ADDR_FILE = "IngameFunctions.txt"

        Private Shared ReadOnly Property _ingameFuncAddresses As ReadOnlyDictionary(Of String, Integer)
        Private Shared ReadOnly Property _ingameFuncNames As ReadOnlyDictionary(Of Integer, String)

        Public ReadOnly Property AsmCaller As DSAsmCaller
        Public ReadOnly Property State As New NLua.Lua
        Private LoadedDarkSoulsLuaFunctions As New List(Of String)

        Private GC_Counter As Int32 = 0
        Private Const GC_Interval As Int32 = 1000

        Private Shared ReadOnly Property __thisProcess As Process
        Public Shared ReadOnly Property ThisProcess As Process
            Get
                If __thisProcess Is Nothing Then
                    ___thisProcess = Process.GetCurrentProcess()
                End If
                Return __thisProcess
            End Get
        End Property


        Public Shared ReadOnly Property IngameFuncAddresses As ObjectModel.ReadOnlyDictionary(Of String, Int32)
            Get
                Return _ingameFuncAddresses
            End Get
        End Property

        Public Shared ReadOnly Property IngameFuncNames As ObjectModel.ReadOnlyDictionary(Of Integer, String)
            Get
                Return _ingameFuncNames
            End Get
        End Property

        Public Sub New()
            DoWhatSharedSubNewWouldDoIfVbWasntFuckingGlitchyAssGarbage()
            Init()
        End Sub

        Shared Sub New()
            DoWhatSharedSubNewWouldDoIfVbWasntFuckingGlitchyAssGarbage()
        End Sub

        Shared Sub DoWhatSharedSubNewWouldDoIfVbWasntFuckingGlitchyAssGarbage()
            Dim ingameFuncTxt = ScriptLibResources.GetEmbeddedTextResource(INGAME_FUNC_ADDR_FILE)

            Dim initIngameFuncDict_BeforeReadOnly = New Dictionary(Of String, Int32)
            Dim initIngameFuncDict_Name_BeforeReadOnly = New Dictionary(Of Int32, String)

            For Each line In ingameFuncTxt.Split(vbCrLf)
                Dim lineSplit = line.Split("|")
                initIngameFuncDict_BeforeReadOnly.Add(lineSplit(1), Integer.Parse(lineSplit(0)))
                initIngameFuncDict_Name_BeforeReadOnly.Add(Integer.Parse(lineSplit(0)), lineSplit(1))
            Next

            __ingameFuncAddresses = New ReadOnlyDictionary(Of String, Integer)(initIngameFuncDict_BeforeReadOnly)
            __ingameFuncNames = New ReadOnlyDictionary(Of Integer, String)(initIngameFuncDict_Name_BeforeReadOnly)
        End Sub

        Private Shared ____inst As New LuaInterface()
        Public Shared ReadOnly Property Instance As LuaInterface
            Get
                Return ____inst
            End Get
        End Property

        'Public Shared ReadOnly LuaProxyAttributeFuncs As Dictionary(Of String, Reflection.MethodInfo)

        'Shared Sub New()

        '    LuaProxyAttributeFuncs = New Dictionary(Of String, Reflection.MethodInfo)

        '    For Each scriptLibType In Reflection.Assembly.GetExecutingAssembly().GetTypes()
        '        For Each func In scriptLibType.GetMethods(Reflection.BindingFlags.Public)
        '            For Each custAttr In func.CustomAttributes
        '                If custAttr.AttributeType = GetType(LuaFuncAttribute) Then
        '                    Dim funcNameInLua = DirectCast(custAttr.ConstructorArguments.First().Value, String)
        '                    If Not LuaProxyAttributeFuncs.ContainsKey(funcNameInLua) Then
        '                        LuaProxyAttributeFuncs.Add(funcNameInLua, func)
        '                    Else
        '                        Throw New ArgumentException($"Could not bind VB.net function '{func.Name}' to the Lua function name '{funcNameInLua}'; Another function ('{LuaProxyAttributeFuncs(funcNameInLua).Name}') is already bound to it.")
        '                    End If
        '                End If
        '            Next
        '        Next
        '    Next

        'End Sub



        Public ReadOnly ImportedNamespaces As String() = {
            "DaS.ScriptLib.Game.Data",
            "DaS.ScriptLib.Game.Mem"
        }

        Private Sub Init()
            _AsmCaller = New DSAsmCaller()

            State.LoadCLRPackage()
            State.DoString("import ('System', 'System') ")

            For Each ns In ImportedNamespaces
                State.DoString($"import ('Dark Souls Scripting Library', '{ns}') ")
            Next

            State.DoString("import = function () end", "SANDBOX")

            'State("FUNC") = State.RegisterFunction("FUNC", GetType(LuaInterface).GetMethod("CallIngameFunc"))


            'Now that's what I call redundant LUL
            Dim retTypEnumVals = [Enum].GetValues(GetType(FuncReturnType)).Cast(Of FuncReturnType)().ToArray()

            For Each retTyp In retTypEnumVals
                State(retTyp.ToString()) = CType(retTyp, Integer)
            Next

            'For Each kvp In LuaProxyAttributeFuncs
            '    State.RegisterFunction(kvp.Key, kvp.Value)
            'Next

            NLua.LuaRegistrationHelper.TaggedInstanceMethods(State, Me)

            For Each kvp In IngameFuncAddresses
                State.DoString($"function {kvp.Key}(...) return FUNC(INT, {kvp.Value}, {{...}}); end")
            Next

            NLua.LuaRegistrationHelper.TaggedStaticMethods(State, GetType(Funcs))

            For Each typ In GetType(LuaInterface).Assembly.GetTypes().Where(Function(x) ImportedNamespaces.Contains(x.Namespace))
                NLua.LuaRegistrationHelper.TaggedStaticMethods(State, typ)
            Next

            State.DoString(ScriptLibResources.GetEmbeddedTextResource("DarkSoulsFunctions.lua"))

            NLua.LuaRegistrationHelper.TaggedStaticMethods(State, GetType(Hook))
            NLua.LuaRegistrationHelper.TaggedStaticMethods(State, GetType(Dbg))

            DebugLocalsInit()

        End Sub

        Private Sub LuaState_DebugHook(sender As Object, e As DebugHookEventArgs)
            Dbg.Print("line: " & e.LuaDebug.currentline & ", event: " & e.LuaDebug.eventCode)
        End Sub

        <NLua.LuaGlobal(Name:="unpack")>
        Public Function LuaUnpackArgs(args As NLua.LuaTable) As Object()
            Return args.Values.OfType(Of Object).Select(Function(x) E(DirectCast(x, String))).ToArray()
        End Function

        <NLua.LuaGlobal(Name:="int")>
        Public Function LuaQuickValueConvertInt(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToInt32(num))
        End Function

        <NLua.LuaGlobal(Name:="uint")>
        Public Function LuaQuickValueConvertUInt(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToUInt32(num))
        End Function

        <NLua.LuaGlobal(Name:="short")>
        Public Function LuaQuickValueConvertShort(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToInt16(num))
        End Function

        <NLua.LuaGlobal(Name:="ushort")>
        Public Function LuaQuickValueConvertUShort(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToUInt16(num))
        End Function

        <NLua.LuaGlobal(Name:="byte")>
        Public Function LuaQuickValueConvertByte(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToByte(num))
        End Function

        <NLua.LuaGlobal(Name:="sbyte")>
        Public Function LuaQuickValueConvertSByte(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Convert.ToSByte(num))
        End Function

        <NLua.LuaGlobal(Name:="bool")>
        Public Function LuaQuickValueConvertBool(num As Double) As LuaBoxedVal
            Return New LuaBoxedVal(Math.Round(num) > 0)
        End Function

        Public Shared Function DoString(str As String, Optional chunkName As String = "chunk") As Object()
            Return Instance.State.DoString(str, chunkName)
        End Function

        Public Shared Function E(expression As String)
            Return Instance.State.DoString($"return {expression}")(0)
        End Function

        <NLua.LuaGlobal(Name:="FUNC")>
        Public Function CallIngameFunc_FromLua(returnType As Double, funcAddress As Double, args As NLua.LuaTable) As Object
            Return AsmCaller.CallIngameLua(Me, (CType(CType(returnType, Integer), FuncReturnType)), CType(funcAddress, Integer), args)
        End Function

        Public Sub DebugRegisterLocal(path As String, val As Object)

        End Sub

        Public Sub DebugLocalsInit()
            'Register Debug locals table:
            State.NewTable("LUAI")

            'Constants:
            State("LUAI.GC_Interval") = GC_Interval

            'Field initialization for autocomplete
            State("LUAI.ProcessMemoryMB") = 0
        End Sub

        Public Sub DebugLocalsUpdate()
            State("LUAI.GC_Counter") = GC_Counter
            State("LUAI.ProcessMemoryMB") = ThisProcess.PrivateMemorySize64 / 1024 / 1024
        End Sub

        Public Sub DebugUpdate()
            DebugLocalsUpdate()
            If GC_Counter >= GC_Interval Then
                State.DoString("collectgarbage();")
                GC.Collect(0, GCCollectionMode.Forced, False)
                GC_Counter = 0
                ThisProcess.Refresh()
            Else
                GC_Counter += 1
            End If
        End Sub



        Public Sub Dispose() Implements IDisposable.Dispose
            State.DoString("collectgarbage()")
            State.Dispose()
            _State = Nothing

            LoadedDarkSoulsLuaFunctions.Clear()
            LoadedDarkSoulsLuaFunctions = Nothing

            AsmCaller.Dispose()
            _AsmCaller = Nothing
        End Sub
    End Class

End Namespace