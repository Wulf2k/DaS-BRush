using System;
using System.Collections.Generic;
using System.Threading;
using Neo.IronLua;
using DaS.ScriptLib.Injection;
using DaS.ScriptLib.LuaScripting.Structures;

namespace DaS.ScriptLib.LuaScripting
{
    public partial class DSLua
    {
        private static Lua L;

        public static LuaGlobal G;
        public static dynamic DG
        {
            get { return G; }
        }

        private static DSAsmCaller AsmCaller = new DSAsmCaller();

        private static DateTime StartTime;
        public static double Clock()
        {
            return 1.0 * DateTime.Now.Subtract(StartTime).Ticks / TimeSpan.TicksPerSecond;
        }

        public static LuaResult Expr<T>(string exp)
        {
            return DoString("return (" + exp + ");");
        }

        public static LuaResult Expr(string exp)
        {
            return DoString("return (" + exp + ");");
        }

        private static LuaTable EmptyLuaTable { get; }

        public static object CallIngameFunc_FromLua(int returnType, int funcAddress, LuaTable args)
        {
            return AsmCaller.CallIngameFunc_FromLua((FuncReturnType)returnType, funcAddress, args, EmptyLuaTable);
        }

        public static object CallIngameFuncREG_FromLua(int returnType, int funcAddress, LuaTable args, LuaTable specialRegisters)
        {
            return AsmCaller.CallIngameFunc_FromLua((FuncReturnType)returnType, funcAddress, args, specialRegisters);
        }

        public static object CallIngameFunc(int returnType, int funcAddress, params object[] args)
        {
            return AsmCaller.CallIngameFunc((FuncReturnType)returnType, funcAddress, args, null);
        }

        public static object CallIngameFuncREG(int returnType, int funcAddress, Dictionary<string, object> specialRegisters, params object[] args)
        {
            return AsmCaller.CallIngameFunc((FuncReturnType)returnType, funcAddress, args, specialRegisters);
        }

        public static LuaResult DoString(string luaStr, params KeyValuePair<string, object>[] args)
        {
            return G.DoChunk(luaStr, "DSLua.DoString()", args);
        }

        //Private Const LOOP_INTERVAL = 5000

        public static EventWaitHandle CleanExitTrigger = new EventWaitHandle(false, EventResetMode.ManualReset);
        //Private Shared NextEvent As DSLuaEvent = DSLuaEvent.None


        private static Thread EventThread;
        //Private Shared ____scriptQueueLock As New Object()

        //Private Shared __scriptQueue As New Queue(Of DSLuaScript)

        public static event OnScriptFinishedEventHandler OnScriptFinished;
        public delegate void OnScriptFinishedEventHandler(Script script);

        //Private Shared ReadOnly Property ScriptQueue As Queue(Of DSLuaScript)
        //    Get
        //        Dim result As Queue(Of DSLuaScript) = Nothing
        //        SyncLock ____scriptQueueLock
        //            result = __scriptQueue
        //        End SyncLock
        //        Return result
        //    End Get
        //End Property

        //Private Shared ScriptQueueCallback As New EventWaitHandle(False, EventResetMode.ManualReset)

        //Private Shared __scriptThreadsLock = New Object()
        //Private Shared __scriptThreads As New Dictionary(Of DSLuaScript, Thread)

        //Friend Shared ReadOnly Property ScriptThreads As Dictionary(Of DSLuaScript, Thread)
        //    Get
        //        Dim result As Dictionary(Of DSLuaScript, Thread) = Nothing
        //        SyncLock __scriptThreadsLock
        //            result = __scriptThreads
        //        End SyncLock
        //        Return result
        //    End Get
        //End Property

        //Public Shared Function RegisterScript(script As DSLuaScript) As Thread
        //    ScriptQueue.Enqueue(script)
        //    ScriptQueueCallback.Reset()
        //    ProcEvent(DSLuaEvent.ExecuteScriptSingle)
        //    Dim scriptQueued As Boolean = False
        //    Do
        //        scriptQueued = ScriptQueueCallback.WaitOne(1000)
        //    Loop Until scriptQueued
        //    Return ScriptThreads(script)
        //End Function

        //Friend Shared Function ProcEvent(e As DSLuaEvent) As Boolean
        //    NextEvent = e
        //    Return EventTrigger.Set()
        //End Function


        private static Dictionary<Guid, Script> ScriptList = new Dictionary<Guid, Script>();

        private static object ____scriptLock = new object();
        private static void ScriptAdd(Script script)
        {
            lock (____scriptLock)
            {
                ScriptList.Add(script.UUID, script);
            }
        }

        private static void ScriptRemove(Script script)
        {
            lock (____scriptLock)
            {
                ScriptList.Remove(script.UUID);
            }
        }

        static internal void ForceInitCleanExitWaitThread()
        {
            if (EventThread?.IsAlive ?? false)
            {
                EventThread.Abort();
            }
            EventThread = new Thread(DoCleanExitWait)
            {
                Name = "DSLua.CleanExitWait_" + Guid.NewGuid().ToString(),
                IsBackground = true
            };
            EventThread.Start();
        }

        static internal void ForceStopAllScripts()
        {
            foreach (Script script in ScriptList.Values)
            {
                script.Abort();
            }
        }

        public static void Init()
        {
            if (!Hook.DARKSOULS.Attached)
            {
                Hook.DARKSOULS.TryAttachToDarkSouls(true);
            }
            Environment.Init();
            ForceInitCleanExitWaitThread();
        }

        private static void PerformCleanExit()
        {
            ForceStopAllScripts();
            Hook.DARKSOULS.Close();
            AsmCaller.Dispose();
        }

        private static void DoCleanExitWait()
        {
            bool doCleanExit = false;

            try
            {
                do
                {
                    doCleanExit = CleanExitTrigger.WaitOne(5000);
                } while (!(doCleanExit));

            }
            catch
            {
            }
            finally
            {
                PerformCleanExit();
            }
        }
    }
}