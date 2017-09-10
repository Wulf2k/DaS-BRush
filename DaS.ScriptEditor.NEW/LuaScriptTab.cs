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
        private bool IsModified
        {
            get
            {
                return __isModified;
            }
            set
            {
                
                __isModified = value;
            }
        }
        private Thread ExecThread;

        private void UpdateTabText()
        {
            Title = (IsRunning ? "[" : "") + SeScriptShortName + (IsModified ? "*" : "") + (IsRunning ? "]" : "");
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

        public LuaScriptTab(string scriptFileName = null) : base()
        {
            CanFloat = false;

            SeScriptFilePath = scriptFileName;

            if (scriptFileName != null)
            {
                INSTANT_LoadDocumentFromDisk();
            }
        }


        protected override void OnParentChanging(ILayoutContainer oldValue, ILayoutContainer newValue)
        {
            
        }

        public void InitEvents()
        {
            ParentLuaContainer.NewTabSelected += ParentLuaContainer_NewTabSelected;
            ParentLuaContainer.OldTabDeselected += ParentLuaContainer_OldTabDeselected;
        }




        private void OtherThread_RunScript(string scriptText)
        {
            var luai = new ScriptLib.Lua.LuaInterface();
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
                ParentLuaContainer.RaiseScriptStop(new LuaTabSwitchEventArgs(this));
                luai.Dispose();
                luai = null;
            }
            
        }


        public void StartExecution()
        {
            StopExecution();

            ExecThread = new Thread(new ParameterizedThreadStart((script) => OtherThread_RunScript(script as string)))
            {
                IsBackground = true,
                Name = "LuaScript:" + SeScriptShortName
            };

            ExecThread.Start(EditorDocument.Text);

            ParentLuaContainer.RaiseScriptStart(new LuaTabSwitchEventArgs(this));
        }

        public void StopExecution()
        {
            if (IsRunning)
            {
                ExecThread.Abort();
            }

            ExecThread = null;
        }


        protected override void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            base.OnIsSelectedChanged(oldValue, newValue);

            if (newValue)
            {
                ParentLuaContainer.SelectedLuaScript = this;
            }
        }

        private void ParentLuaContainer_OldTabDeselected(object sender, LuaTabSwitchEventArgs e)
        {
            if (e.Script == this)
            {
                EditorDocument = ParentLuaContainer?.LuaEditor.Document ?? EditorDocument;
                EditorCaretOffset = ParentLuaContainer?.LuaEditor.CaretOffset ?? EditorCaretOffset;
                EditorVerticalOffset = ParentLuaContainer?.LuaEditor.VerticalOffset ?? EditorVerticalOffset;
                Content = null;
            }
        }

        private void ParentLuaContainer_NewTabSelected(object sender, LuaTabSwitchEventArgs e)
        {
            if (e.Script == this)
            {
                ParentLuaContainer.LuaEditor.Document = EditorDocument;
                ParentLuaContainer.LuaEditor.ScrollToVerticalOffset(EditorVerticalOffset);
                ParentLuaContainer.LuaEditor.CaretOffset = EditorCaretOffset;
                Content = ParentLuaContainer.LuaEditor;

                if (IsRunning)
                {
                    ParentLuaContainer.RaiseScriptStart(new LuaTabSwitchEventArgs(this));
                }
                else
                {
                    ParentLuaContainer.RaiseScriptStop(new LuaTabSwitchEventArgs(this));
                }
            }
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            base.OnClosing(args);

            if (!SeCheckIfCanClose())
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
        public bool SeSave()
        {
            return DoCheckSaveOrSaveAs();
        }

        /// <summary>
        /// Shows "Save As..." dialog and saves the file to disk if the user chooses to.
        /// </summary>
        /// <returns>True if user ends up saving file. False if user clicks "Cancel".</returns>
        public bool SeSaveAs()
        {
            return ShowSaveAsDialog();
        }

        /// <summary>
        /// Checks if this tab can close ("Save unsaved changes?" etc)
        /// </summary>
        /// <returns>True if you can close. False if you can NOT close.</returns>
        public bool SeCheckIfCanClose()
        {
            if (SeScriptFileExists && !IsModified)
            {
                return true;
            }

            var dlgResult = MessageBox.Show($"Save file \"{SeScriptShortName}\"?", "Save First?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (dlgResult == MessageBoxResult.Yes)
            {
                //User decides to save. We let the default save button logic determine the result for us ;)
                return SeSave();
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

        private bool DoCheckSaveOrSaveAs()
        {
            if (SeScriptFileExists)
            {
                INSTANT_SaveDocumentToDisk();
                return true;
            }
            else
            {
                return ShowSaveAsDialog();
            }
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Kappa
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_SaveDocumentToDisk()
        {
            //TODO: Make more "efficient" Kappa
            File.WriteAllText(SeScriptFilePath, EditorDocument.Text);
        }

        /// <summary>
        /// You are expected to perform any checks yourself before calling this or you will piss people off Keepo
        /// </summary>
        /// <param name="doc"></param>
        internal void INSTANT_LoadDocumentFromDisk()
        {
            //TODO: Make more "efficient" Keepo
            EditorDocument.Text = File.ReadAllText(SeScriptFilePath);
        }

        private bool ShowSaveAsDialog()
        {
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
                SeScriptFilePath = dlg.FileName;
                INSTANT_SaveDocumentToDisk();
                return true;
            }

            return false;
        }

        /// <summary>
        /// The open file dialog will be in the directory of this tab's script if applicable.
        /// </summary>
        public bool SeOpenFile()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                AddExtension = false,
                DefaultExt = ".lua",
                FileName = "",
                Filter = "Lua Scripts|*.lua",
                InitialDirectory = new FileInfo(typeof(LuaScriptTab).Assembly.Location).Directory.FullName, //TODO: store last user save/load dir and load on startup
                Title = "Open 1 or more Lua Script(s)",
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true
            };

            if (SeScriptFileExists)
            {
                var cfi = new FileInfo(SeScriptFilePath);
                dlg.InitialDirectory = cfi.Directory.FullName;
                dlg.FileName = cfi.Name;
            }

            if (dlg.ShowDialog() ?? false)
            {
                foreach (var f in dlg.FileNames)
                {
                    var newTab = ParentLuaContainer.AddNewTab();
                    newTab.SeScriptFilePath = f;
                    newTab.INSTANT_LoadDocumentFromDisk();
                    return true;
                }
            }

            return false;
        }
    }
}
