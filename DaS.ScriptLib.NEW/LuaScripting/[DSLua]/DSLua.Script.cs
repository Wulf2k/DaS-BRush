using System;
using System.Threading;
using Neo.IronLua;
using DaS.ScriptLib.Injection;

namespace DaS.ScriptLib.LuaScripting
{
    public partial class DSLua
    {
        public class Script
        {
            public event OnRunEventHandler OnRun;
            public delegate void OnRunEventHandler();
            internal void RaiseOnRun()
            {
                if (OnRun != null)
                {
                    OnRun();
                }
            }

            public event OnFinishedEventHandler OnFinished;
            public delegate void OnFinishedEventHandler();
            internal void RaiseOnFinished()
            {
                if (OnFinished != null)
                {
                    OnFinished();
                }
                if (OnScriptFinished != null)
                {
                    OnScriptFinished(this);
                }
            }

            private event OnExceptionEventHandler OnException;
            private delegate void OnExceptionEventHandler(Exception e, EventWaitHandle waitHandle);
            private void RaiseOnException(Exception e, EventWaitHandle waitHandle)
            {
                if (OnException != null)
                {
                    OnException(e, waitHandle);
                }
            }

            private Action<Exception, EventWaitHandle> RunSyncAcknowledgeExceptionAction = null;

            public readonly string Text;
            public readonly string Name;
            public readonly bool IsDebug;
            internal readonly System.Threading.Thread Thread;

            internal readonly Guid UUID;
            public EventWaitHandle FinishTrigger { get; }

            private Script(string scriptText, string scriptName, bool dbg)
            {
                if (!Hook.DARKSOULS.Attached)
                {
                    Hook.DARKSOULS.TryAttachToDarkSouls(true);
                }

                IsDebug = dbg;

                Text = scriptText;
                Name = scriptName;
                Thread = new System.Threading.Thread(DoExecute)
                {
                    Name = "DSLuaScript:" + Name,
                    IsBackground = false
                };
                UUID = Guid.NewGuid();
                DSLua.ScriptAdd(this);
            }

            public void Run()
            {
                FinishTrigger.Reset();
                Thread.Start(this);
            }

            private void DoRunSyncOptionalAcknowledgeException(Exception e, EventWaitHandle h)
            {
                OnException -= DoRunSyncOptionalAcknowledgeException;
                RunSyncAcknowledgeExceptionAction?.Invoke(e, h);
            }

            public void RunSync(Action<Exception, EventWaitHandle> acknowledgeException = null)
            {
                RunSyncAcknowledgeExceptionAction = acknowledgeException;
                OnException += DoRunSyncOptionalAcknowledgeException;
                Run();
                AwaitTermination();
            }

            public void Abort()
            {
                Thread.Abort();
            }


            static internal void DoExecute(object scriptObj)
            {
                var s = (Script)scriptObj;

                s.RaiseOnRun();

                EventWaitHandle exceptionAknowledgedTrigger = new EventWaitHandle(false, EventResetMode.ManualReset);
                bool exceptionAknowledged = false;

                try
                {
                    var chunk = DSLua.L.CompileChunk(s.Text, s.Name, new LuaCompileOptions { DebugEngine = s.IsDebug ? LuaStackTraceDebugger.Default : null });
                    DSLua.G.DoChunk(chunk);

                    exceptionAknowledgedTrigger.Set();
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine($"DSLuaScript \"{s.Name}\" aborted. [UUID: {s.UUID.ToString()}]");
                    exceptionAknowledgedTrigger.Set();
                }
                catch (Exception e)
                {
                    s.RaiseOnException(e, exceptionAknowledgedTrigger);
                }
                finally
                {
                    do
                    {
                        exceptionAknowledged = exceptionAknowledgedTrigger.WaitOne(5000);
                    } while (!(exceptionAknowledged));

                    s.RaiseOnFinished();
                    s.FinishTrigger.Set();
                    DSLua.ScriptRemove(s);
                }

            }

            public void AwaitTermination()
            {
                bool ended = false;
                do
                {
                    ended = FinishTrigger.WaitOne(5000);
                } while (!(ended));
            }

            public static Script FromFile(string luaFilePath, bool dbg = false)
            {
                return new Script(System.IO.File.ReadAllText(luaFilePath), new System.IO.FileInfo(luaFilePath).Name, dbg);
            }

            public static Script FromString(string luaStr, string displayName, bool dbg = false)
            {
                return new Script(luaStr, displayName, dbg);
            }
        }
    }
}