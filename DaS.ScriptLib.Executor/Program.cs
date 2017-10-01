using DaS.ScriptLib.LuaScripting;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace DaS.ScriptLib.Executor
{
    class Program
    {
        static string GetExceptionText(Exception e, string indentString = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(indentString + e.Message);
            sb.AppendLine(indentString + e.StackTrace);

            if (e.InnerException != null)
            {
                sb.AppendLine();
                sb.AppendLine(indentString + "Inner Exception:");
                sb.AppendLine(GetExceptionText(e.InnerException, indentString + "\t"));
            }

            return sb.ToString();
        }

        static bool ShowErrorAndCheckUserBreak(Exception e, string actionText)
        {
            string errMsg = $"Dark Souls Script Executor encountered an exception while {actionText.Trim()} (shown below).\n" +
                "----------------------------------------\n" +
                $"{GetExceptionText(e, "\t")}\n" +
                "----------------------------------------";
            Console.Error.WriteLine(errMsg);
            Console.WriteLine("Showing error message box and asking user whether to throw the above exception...");
            bool result = MessageBox.Show(errMsg + "\nWould you like to crash the application for debugging purposes?", "Exception Encountered", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            Console.WriteLine($"User selected \"" + (result ? "Yes" : "No") + "\"."); //String interpolation ($) doesn't support using quotation marks inside the braces >:^(
            return result;
        }

        static void DoException(Exception e, string scriptPath)
        {
            

            if (e is Neo.IronLua.LuaException)
            {
                var le = (e as Neo.IronLua.LuaException);
                Console.Error.WriteLine($"Exception (shown below) thrown at line {le.Line}, column {le.Column} of \"{new FileInfo(scriptPath).Name}\":\n");
                Console.Error.WriteLine(e.Message);
            }
            else
            {
                Console.Error.WriteLine($"Exception (shown below) thrown while executing \"{new FileInfo(scriptPath).Name}\":\n");
                Console.Error.WriteLine(e.Message);
            }

            foreach (var q in Neo.IronLua.LuaExceptionData.GetData(e))
            {
                Console.Error.WriteLine(q.ToString());
            }

            var inner = e.InnerException as Neo.IronLua.LuaException;

            if (e.InnerException != null)
            {
                Console.Error.Write("\n\n\n");
                DoException(e.InnerException, scriptPath);
            }
        }

        static void Main(string[] args)
        {
            DSLua.Init();

            //Console.WriteLine("TESTING:");
            //var TEST = LuaType.Clr.EnumerateMembers<MethodInfo>(LuaMethodEnumerate.Dynamic, m => m.GetType().GetMethods(BindingFlags.Instance));
            //foreach (var test in TEST)
            //{
            //    Console.WriteLine(test.Name);
            //}
            //return;

            var testScript = DSLua.Script.FromString("print(\"Initializing DSLua...\")", "INIT");
            testScript.RunSync((e, h) =>
            {
                DoException(e, "INIT");
                h.Set();
            });

            string invalidArg = null;
            string targetScriptFile = null;
            bool attachDebugger = false;

            foreach (var arg in args)
            {
                var formattedArg = arg.Trim().ToUpper();
                if (formattedArg.StartsWith("-") || formattedArg.StartsWith("/"))
                {
                    var cmd = formattedArg.Substring(1);

                    if (cmd == "D" || cmd == "DEBUG")
                    {
                        attachDebugger = true;
                    }
                    else
                    {
                        invalidArg = arg;
                    }
                }
                else
                {
                    targetScriptFile = arg;
                }
            }

            string usageError = null;

            if (invalidArg != null)
                usageError = $"Invalid argument passed to Dark Souls Script Executor: \"{invalidArg}\"";
            else if (targetScriptFile == null)
                usageError = "No script passed as argument to 'Dark Souls Script Executor.exe' process.";
            else if (!File.Exists(targetScriptFile))
                usageError = $"Specified file (\"{targetScriptFile}\") does not exist.";

            if (usageError != null)
            {
                usageError += "\n\nDark Souls Script Executor will now exit.";
                Console.Error.WriteLine(usageError);
                MessageBox.Show(usageError);
                return;
            }

            if (attachDebugger)
            {
                Console.WriteLine("Debug-enable argument passed to Dark Souls Script Executor.");
                Console.WriteLine("Launching Windows' debugger attach dialogue...");

                if (!System.Diagnostics.Debugger.Launch())
                {
                    var warning = "Debug-enable argument was passed to Dark Souls Script Executor but attempt to launch the debugger failed.";
                    Console.Error.WriteLine(warning);
                    Console.WriteLine("Asking user whether to continue without debugger...");
                    if (MessageBox.Show(warning + "\nWould you like to continue without the debugger present?", 
                        "Continue without debugger?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    {
                        Console.WriteLine("User chose not to continue without debugger.\n\nDark Souls Script Executor will now exit.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("User chose to continue without debugger.");
                    }
                }
            }

            Console.WriteLine("Executing Lua script and redirecting all output to console...");
            Console.WriteLine();

            try
            {
                var s = DSLua.Script.FromFile(args[0], true);
                
                s.RunSync((e, h) =>
                {
                    DoException(e, args[0]);
                    h.Set();
                });

                s.FinishTrigger.WaitOne();
            }
            finally
            {
                DSLua.CleanExitTrigger.Set();
                Injection.Hook.DARKSOULS.Close();
            }
        }
    }
}
