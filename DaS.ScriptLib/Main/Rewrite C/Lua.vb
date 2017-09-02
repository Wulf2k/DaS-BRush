Imports System.Threading
Imports NLua.Event

Public Class Lua

    Private Shared ____inst As New Lua()
    Public Shared ReadOnly Property Instance As Lua
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

    Public ReadOnly Property ThreadCallers As New Dictionary(Of Integer, LuaFunctionCaller)
    Public ReadOnly Property State As New NLua.Lua
    Private LoadedDarkSoulsLuaFunctions As New List(Of String)
    Public ReadOnly Property ModScriptExecuting As Boolean = False

    Private Sub Init()
        State.LoadCLRPackage()
        State.DoString("import ('System', 'System') ")
        State.DoString("import ('Dark Souls Scripting Library', 'DaS.ScriptLib') ")

        State.DoString("import = function () end", "SANDBOX")

        State.DoString($"global VOID = {LuaFunctionCaller.RETURN_TYPE_VOID}")
        State.DoString($"global INT = {LuaFunctionCaller.RETURN_TYPE_INT}")
        State.DoString($"global UINT = {LuaFunctionCaller.RETURN_TYPE_UINT}")
        State.DoString($"global SHORT = {LuaFunctionCaller.RETURN_TYPE_SHORT}")
        State.DoString($"global USHORT = {LuaFunctionCaller.RETURN_TYPE_USHORT}")
        State.DoString($"global BYTE = {LuaFunctionCaller.RETURN_TYPE_BYTE}")
        State.DoString($"global SBYTE = {LuaFunctionCaller.RETURN_TYPE_SBYTE}")
        State.DoString($"global BOOL = {LuaFunctionCaller.RETURN_TYPE_BOOL}")
        State.DoString($"global FLOAT = {LuaFunctionCaller.RETURN_TYPE_FLOAT}")
        State.DoString($"global DOUBLE = {LuaFunctionCaller.RETURN_TYPE_DOUBLE}")
        State.DoString($"global STR = {LuaFunctionCaller.RETURN_TYPE_STR}")
        State.DoString($"global UNISTR = {LuaFunctionCaller.RETURN_TYPE_UNISTR}")

        'For Each kvp In LuaProxyAttributeFuncs
        '    State.RegisterFunction(kvp.Key, kvp.Value)
        'Next

        NLua.LuaRegistrationHelper.TaggedStaticMethods(State, GetType(Lua))

        For Each shittyFunc In ScriptRes.caselessIngameFuncNames.Values.Where(Function(x) Not LoadedDarkSoulsLuaFunctions.Contains(x))
            Dim addr = DirectCast(ScriptRes.autoCompleteFuncInfoByName(shittyFunc), IngameFuncInfo).Address

            State.DoString($"function {shittyFunc} (p1,p2,p3,p4,p5,p6,p7,p8,p9,p10) return FUNC(INT, {addr}, {{p1,p2,p3,p4,p5,p6,p7,p8,p9,p10}}) end")
        Next

        Dim customFuncs_FuncsClass = ScriptRes.autoCompleteFuncInfoByName_FuncsClass.Values.OfType(Of CustomFuncInfo)

        For Each cfi In customFuncs_FuncsClass
            Try
                State.DoString($"{cfi.Name} = function ({String.Join(", ", cfi.ParamList.Select(Function(p) p.Name))}) return end")
                State.RegisterFunction(cfi.Name, cfi.MethodDefinition)

            Catch ex As Exception
                Throw New Exception($"ahh fuck {If(cfi?.Name, "")} {If(cfi?.MethodDefinition, "")}", ex)
            End Try

        Next

        State.SetDebugHook(EventMasks.LUA_MASKALL, 1)

        AddHandler State.DebugHook, AddressOf LuaState_DebugHook

    End Sub

    Private Sub LuaState_DebugHook(sender As Object, e As DebugHookEventArgs)
        Dbg.Print("line: " & e.LuaDebug.currentline & ", event: " & e.LuaDebug.eventCode)
    End Sub

    <NLua.LuaGlobal(Name:="int")>
    Public Shared Function LuaQuickValueConvertInt(num As Double) As Int32
        Return Convert.ToInt32(num)
    End Function

    <NLua.LuaGlobal(Name:="uint")>
    Public Shared Function LuaQuickValueConvertUInt(num As Double) As UInt32
        Return Convert.ToUInt32(num)
    End Function

    <NLua.LuaGlobal(Name:="short")>
    Public Shared Function LuaQuickValueConvertShort(num As Double) As Short
        Return Convert.ToInt16(num)
    End Function

    <NLua.LuaGlobal(Name:="ushort")>
    Public Shared Function LuaQuickValueConvertUShort(num As Double) As UShort
        Return Convert.ToUInt16(num)
    End Function

    <NLua.LuaGlobal(Name:="byte")>
    Public Shared Function LuaQuickValueConvertByte(num As Double) As Byte
        Return Convert.ToByte(num)
    End Function

    <NLua.LuaGlobal(Name:="sbyte")>
    Public Shared Function LuaQuickValueConvertSByte(num As Double) As SByte
        Return Convert.ToSByte(num)
    End Function

    <NLua.LuaGlobal(Name:="bool")>
    Public Shared Function LuaQuickValueConvertBool(num As Double) As Boolean
        Return (Math.Round(num) > 0)
    End Function


    Public Shared Function DoString(str As String, Optional chunkName As String = "chunk") As Object()
        Return Instance.State.DoString(str, chunkName)
    End Function

    Public Shared Function E(expression As String)
        Return Instance.State.DoString($"return {expression}")(0)
    End Function


    <NLua.LuaGlobal(Name:="LoadMod")>
    Public Sub ExecuteModScript(filePath As String)



        Dim newModObj = New Object

        State.Item("Mod") = newModObj



    End Sub


    <NLua.LuaGlobal(Name:="FUNC")>
    Public Function CallIngameFunc(returnType As Int32, funcAddress As Int32, args As NLua.LuaTable) As Object
        If Not ThreadCallers.ContainsKey(Thread.CurrentThread.ManagedThreadId) Then
            ThreadCallers.Add(Thread.CurrentThread.ManagedThreadId, New LuaFunctionCaller(_targetProcessHandle))
        End If

        Return ThreadCallers(Thread.CurrentThread.ManagedThreadId).CallIngameLua(returnType, funcAddress, args)
    End Function
End Class
