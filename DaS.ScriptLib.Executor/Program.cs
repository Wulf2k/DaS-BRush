using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        static void Main(string[] args)
        {
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

            Console.WriteLine("Attempting to attach to Dark Souls process...");
            Injection.Hook.Init();

            if (!Injection.Hook.DARKSOULS.Attached)
            {
                string infoMsg = "Unable to attach to Dark Souls process. If Dark Souls is indeed running and you receive this error," +
                    "please try right-clicking on the \"Dark Souls Script Editor.exe\" file and ticking the \"Run as Admin\" checkbox." +
                    "If the problem still persists, try restarting Dark Souls, Steam, and/or Windows." +
                    "If the problem STILL persists, contact one of the developers." +
                    "\nDark Souls Script Executor will now exit.";
                Console.Error.WriteLine(infoMsg);
                MessageBox.Show(infoMsg, "Unable to attach to process");
                return;
            }
            else
            {
                Console.WriteLine("Attached to process successfully.");
            }

            Lua.LuaInterface luai;

            Console.WriteLine("Creating Lua interface...");

            try
            {
                luai = new Lua.LuaInterface();
            }
            catch(Exception e)
            {
                if (ShowErrorAndCheckUserBreak(e, "creating Lua interface"))
                {
                    throw e;
                }
                return;
            }

            Console.WriteLine("Lua interface created successfully.");
            Console.WriteLine("Executing Lua script and redirecting all output to console...");
            Console.WriteLine();
            Console.WriteLine("#### LUA SCRIPT OUTPUT BEGIN ###########");
            Console.WriteLine();

            try
            {
                luai.State.DoString(File.ReadAllText(args[0]));

                while (luai.State.IsExecuting)
                { 
                    /* 
                     * 
                     * I don't even think this is necessary since I'm pretty sure DoString runs the actual Lua script
                     * in the same thread from which it is called, but whatever #FuckTheSystem 
                     * 
                     */
                }

                Console.WriteLine();
                Console.WriteLine("#### LUA SCRIPT OUTPUT END #############");

                Console.WriteLine("Lua script finished executing successfully.");
                Console.WriteLine("Dark Souls Script Executor will now exit.");
                return;
            }
            catch(NLua.Exceptions.LuaScriptException e)
            {
                Console.Error.WriteLine(e.Message);
                //MessageBox.Show(e.Message, "Lua Script Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("#### LUA SCRIPT OUTPUT END #############");
                Console.WriteLine();

                if (ShowErrorAndCheckUserBreak(e, "executing the Lua script"))
                {
                    throw e;
                }
            }
            finally
            {
                luai.Dispose();
                Injection.Hook.DARKSOULS.Close();
            }
        }
    }
}
