using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using System.ComponentModel;
using System.Threading;

namespace DaS.ScriptEditor.NEW
{
    public class LuaScriptTab : LayoutDocument
    {
        //Stuff:

        public const string DefaultScriptName = "New Lua Script (Untitled).lua";

        private bool __isModified = false;
        public bool IsModified
        {
            get
            {
                return __isModified;
            }
            set
            {
                __isModified = value;

                if (ParentLuaContainer.SelectedLuaScript == this)
                {
                    ParentLuaContainer.RaiseCurrentTabIsModifiedChanged(this);
                }

                UpdateTabText();
            }
        }
        private Thread ExecThread;

        private void UpdateTabText()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Title = (IsRunning ? "[" : "") + SeScriptShortName + (IsModified ? "*" : "") + (IsRunning ? "]" : "");
            });
        }

        public bool IsRunning
        {
            get
            {
                return ExecThread != null && ExecThread.IsAlive;
            }
        }

        //LuaEditor storage:

        public LuaScriptTabContainer ParentLuaContainer
        {
            get
            {
                return Parent as LuaScriptTabContainer;
            }
        }



        public TextDocument EditorDocument { get; private set; } = new TextDocument();
        private double EditorVerticalOffset = 0;
        private int EditorCaretOffset = 0;



        //Constructor:

        public LuaScriptTab(LuaScriptTabContainer parent, Action<bool> loading, string scriptFileName = null) : base()
        {
            CanFloat = false;

            SeScriptFilePath = scriptFileName;

            if (scriptFileName != null)
            {
                INSTANT_LoadDocumentFromDisk(loading);
            }

            parent.ScriptStart += ParentLuaContainer_ScriptStart;
            parent.ScriptStop += ParentLuaContainer_ScriptStop;
        }

        private void ParentLuaContainer_ScriptStop(object sender, LuaTabEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateTabText();
            });
        }

        private void ParentLuaContainer_ScriptStart(object sender, LuaTabEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateTabText();
            });
        }

        //TODO: see if this blank override was for a reason lol
        protected override void OnParentChanging(ILayoutContainer oldValue, ILayoutContainer newValue)
        {
            
        }

        public void InitEvents()
        {
            ParentLuaContainer.NewTabSelected += ParentLuaContainer_NewTabSelected;
            ParentLuaContainer.OldTabDeselected += ParentLuaContainer_OldTabDeselected;
        }




        private void OtherThread_RunScript(Action<bool> loading, string scriptText)
        {
            ScriptLib.Lua.LuaInterface luai = null;

            Application.Current.Dispatcher.Invoke(() =>
            {
                loading(true);
                luai = new ScriptLib.Lua.LuaInterface();
                loading(false);
            });
            
            try
            {
                luai.State.DoString(scriptText);
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception e)
            {
                if (ScriptLib.Lua.Dbg.PopupErrQue(e.Message) ?? false)
                {
                    throw e;
                }
            }
            finally
            {
                ParentLuaContainer.RaiseScriptStop(this);
                luai.Dispose();
                luai = null;
            }
            
        }


        public void StartExecution(Action<bool> loading)
        {
            StopExecution();

            ExecThread = new Thread(new ParameterizedThreadStart((script) => OtherThread_RunScript(loading, script as string)))
            {
                IsBackground = true,
                Name = "LuaScript:" + SeScriptShortName
            };

            ExecThread.Start(EditorDocument.Text);

            ParentLuaContainer.RaiseScriptStart(this);
        }

        public void StopExecution()
        {
            if (IsRunning)
            {
                ExecThread.Abort();
            }

            ExecThread = null;

            ParentLuaContainer.RaiseScriptStop(this);
        }


        protected override void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedChanged(oldValue, newValue);

            if (newValue)
            {
                ParentLuaContainer.SelectedLuaScript = this;
            }
        }

        private void ParentLuaContainer_OldTabDeselected(object sender, LuaTabEventArgs e)
        {
            if (e.Script == this)
            {
                EditorDocument = ParentLuaContainer?.LuaEditor.Document ?? EditorDocument;
                EditorCaretOffset = ParentLuaContainer?.LuaEditor.CaretOffset ?? EditorCaretOffset;
                EditorVerticalOffset = ParentLuaContainer?.LuaEditor.VerticalOffset ?? EditorVerticalOffset;
                Content = null;
            }
        }

        private void ParentLuaContainer_NewTabSelected(object sender, LuaTabEventArgs e)
        {
            if (e.Script == this)
            {
                ParentLuaContainer.LuaEditor.Document = EditorDocument;
                ParentLuaContainer.LuaEditor.ScrollToVerticalOffset(EditorVerticalOffset);
                ParentLuaContainer.LuaEditor.CaretOffset = EditorCaretOffset;
                Content = ParentLuaContainer.LuaEditor;

                if (IsRunning)
                {
                    ParentLuaContainer.RaiseScriptStart(this);
                }
                else
                {
                    ParentLuaContainer.RaiseScriptStop(this);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            base.OnClosing(args);

            if (!SeCheckIfCanClose((dummy) => { }))
            {
                args.Cancel = true;
            }
        }

        public bool SeScriptFileExists
        {
            get
            {
                return SeScriptFilePath != null && File.Exists(SeScriptFilePath);
            }
        }

        public string SeScriptFilePath
        {
            get
            {
                return EditorDocument.FileName;
            }
            internal set
            {
                EditorDocument.FileName = value;
                Title = SeScriptShortName;
            }
        }

        public string SeScriptShortName
        {
            get
            {
                return SeScriptFilePath != null ? new FileInfo(SeScriptFilePath).Name : DefaultScriptName;
            }
        }

        /// <summary>
        /// Saves this tab's script like you're hitting the save button. Will show a "Save As..." dialog if it is the first time saving, etc.
        /// </summary>
        /// <returns>True if the file was saved to disk in any way. False if user clicks "Cancel" to "Save As..." dialog (if applicable).</returns>
        public bool SeSave(Action<bool> loading)
        {
            return DoCheckSaveOrSaveAs(loading);
        }

        /// <summary>
        /// Shows "Save As..." dialog and saves the file to disk if the user chooses to.
        /// </summary>
        /// <returns>True if user ends up saving file. False if user clicks "Cancel".</returns>
        public bool SeSaveAs(Action<bool> loading)
        {
            return ShowSaveAsDialog(loading);
        }

        /// <summary>
        /// Checks if this tab can close ("Save unsaved changes?" etc)
        /// </summary>
        /// <returns>True if you can close. False if you can NOT close.</returns>
        public bool SeCheckIfCanClose(Action<bool> loading)
        {
            if (SeScriptFileExists && !IsModified)
            {
                return true;
            }

            var dlgResult = MessageBox.Show($"Save file \"{SeScriptShortName}\"?", "Save First?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (dlgResult == MessageBoxResult.Yes)
            {
                //User decides to save. We let the default save button logic determine the result for us ;)
                return SeSave(loading);
            }
            else if (dlgResult == MessageBoxResult.No)
            {
                //User decides not to save, so we return true.
                return true;
            }
            else
            {
                //User decides not to close tab after all.
                return false;
            }
        }

        private bool DoCheckSaveOrSaveAs(Action<bool> loading)
        {
            if (SeScriptFileExists)
            {
                INSTANT_SaveDocumentToDisk(loading);
                return true;
            }
            else
            {
                return ShowSaveAsDialog(loading);
            }
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Kappa
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_SaveDocumentToDisk(Action<bool> loading)
        {
            loading(true);

            Application.Current.Dispatcher.Invoke(() =>
            {
                //TODO: Make more "efficient" Kappa
                File.WriteAllText(SeScriptFilePath, EditorDocument.Text);

                IsModified = false;
            });
            
            loading(false);
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Keepo
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_LoadDocumentFromDisk(Action<bool> loading)
        {
            loading(true);

            Application.Current.Dispatcher.Invoke(() =>
            {
                //TODO: Make more "efficient" Keepo
                EditorDocument.Text = File.ReadAllText(SeScriptFilePath);

                IsModified = false;
            });

            loading(false);
        }

        private bool ShowSaveAsDialog(Action<bool> loading)
        {
            loading(false);

            var dlg = new Microsoft.Win32.SaveFileDialog()
            {
                CheckFileExists = false,
                AddExtension = true,
                DefaultExt = ".lua",
                FileName = DefaultScriptName,
                Filter = "Lua Scripts|*.lua",
                InitialDirectory = new FileInfo(typeof(LuaScriptTab).Assembly.Location).Directory.FullName, //TODO: store last user save/load dir and load on startup
                CreatePrompt = false,
                OverwritePrompt = true,
                Title = "Save Lua Script"
            };

            if (SeScriptFileExists)
            {
                var cfi = new FileInfo(SeScriptFilePath);
                dlg.InitialDirectory = cfi.Directory.FullName;
                dlg.FileName = cfi.Name;
            }

            bool doSave = dlg.ShowDialog() ?? false;

            if (doSave)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SeScriptFilePath = dlg.FileName;
                    INSTANT_SaveDocumentToDisk(loading);
                });
                return true;
            }

            return false;
        }

        /// <summary>
        /// The open file dialog will be in the directory of this tab's script if applicable.
        /// </summary>
        public bool SeOpenFile(Action<bool> loading)
        {
            return ParentLuaContainer.SeOpenFile(loading, EditorDocument.FileName != null ? new FileInfo(EditorDocument.FileName).DirectoryName : null);
        }
    }
}
