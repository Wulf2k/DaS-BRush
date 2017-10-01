using ICSharpCode.AvalonEdit;
using System;
using Xceed.Wpf.AvalonDock.Layout;
using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Windows.Input;
using System.Windows.Media;

namespace DaS.ScriptEditor.NEW
{
    public class LuaScriptTabContainer : LayoutDocumentPane
    {
        public const string IconScriptRunning_Path = "pack://application:,,,/Resources/ScriptRunning.png";
        public const string IconScriptNotRunning_Path = "pack://application:,,,/Resources/ScriptNotRunning.png";

        public TextEditor LuaEditor;
        public LuaAutoComplete AutoComplete;

        public MainWindow Main;

        //public readonly ImageSource IconScriptRunning;
        //public readonly ImageSource IconScriptNotRunning;

        public event EventHandler<LuaTabEventArgs> NewTabSelected;
        public event EventHandler<LuaTabEventArgs> OldTabDeselected;
        public event EventHandler<LuaTabEventArgs> ScriptStart;
        public event EventHandler<LuaTabEventArgs> ScriptStop;
        public event EventHandler<LuaTabEventArgs> CurrentTabIsModifiedChanged;

        private LuaScriptTab __selectedLuaScript;
        public LuaScriptTab SelectedLuaScript
        {
            get
            {
                return SelectedContent as LuaScriptTab;
            }
            set
            {
                OnOldTabDeselected(__selectedLuaScript);
                OnNewTabSelected(value);

                __selectedLuaScript = value;
            }
        }

        public LuaScriptTabContainer() : base()
        {

        }

        private void InitEditor()
        {
            LuaEditor = new TextEditor()
            {
                SyntaxHighlighting = LoadHightLightRule(),
                FontFamily = new FontFamily("Consolas"),
                //lol don't you love Microsoft
                FontSize = 10.0 * (96.0 / 72.0),
                WordWrap = true,
                ShowLineNumbers = true
            };

            LuaEditor.Options.AllowScrollBelowDocument = true;
            LuaEditor.Options.ConvertTabsToSpaces = true;

            LuaEditor.Options.EnableEmailHyperlinks = false;
            LuaEditor.Options.EnableHyperlinks = true;
            LuaEditor.Options.EnableImeSupport = true;
            LuaEditor.Options.EnableRectangularSelection = true;
            LuaEditor.Options.HideCursorWhileTyping = false;
            LuaEditor.Options.IndentationSize = 4;
            LuaEditor.Options.ShowBoxForControlCharacters = false;
            LuaEditor.Options.WordWrapIndentation = 4;

            //I, Meowmaritus, hate these so I'm turning them off by default.
            //They can 'easily' be added as menu items by setting the "is checked" flag to a <Binding/> for LuaEditor.Options.* with the binding's Mode on "TwoWay"
            LuaEditor.Options.CutCopyWholeLine = false;
            LuaEditor.Options.EnableTextDragDrop = false;
            LuaEditor.Options.EnableVirtualSpace = false;
            LuaEditor.Options.HighlightCurrentLine = false;

            LuaEditor.TextArea.TextEntered += TextArea_TextEntered;

            LuaEditor.Style = Main.Resources["DarkAvalonEditStyle"] as Style;

            LuaEditor.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x1E, 0x1E, 0x1E));
            LuaEditor.TextArea.Caret.CaretBrush = Brushes.White;
            LuaEditor.Foreground = Brushes.White;
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            var selSc = SelectedLuaScript;
            if (selSc != null)
                selSc.IsModified = true;
        }

        public IHighlightingDefinition LoadHightLightRule()
        {
            using (Stream s = typeof(LuaScriptTabContainer).Assembly.GetManifestResourceStream("DaS.ScriptEditor.NEW.EmbeddedResources.Lua.xshd"))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    return HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        public LuaScriptTab AddNewTab(Action<bool> loading)
        {
            LuaScriptTab newTab = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (LuaEditor == null)
                {
                    InitEditor();
                    AutoComplete = new LuaAutoComplete(ref LuaEditor, ref Main);
                }

                newTab = new LuaScriptTab(this, loading);

                if (ChildrenCount == 0)
                {
                    newTab.Content = LuaEditor;
                    LuaEditor.Document = newTab.EditorDocument;
                }

                Children.Add(newTab);
                newTab.InitEvents();
            });
            return newTab;
        }

        public void FocusTab(LuaScriptTab tab)
        {
            if (!Children.Contains(tab))
            {
                Children.Add(tab);
            }

            SelectedContentIndex = Children.IndexOf(tab);
        }

        public bool SaveAll(Action<bool> loading)
        {
            foreach(var thing in Children)
            {
                if (!((thing as LuaScriptTab).SeSave(loading)))
                {
                    return false;
                }
            }
            return true;
        }

        public LuaScriptTab this[int i]
        {
            get
            {
                return Children[i] as LuaScriptTab;
            }
        }

        protected virtual void OnNewTabSelected(LuaScriptTab tab)
        {
            NewTabSelected?.Invoke(this, new LuaTabEventArgs(tab));
        }

        protected virtual void OnOldTabDeselected(LuaScriptTab tab)
        {
            OldTabDeselected?.Invoke(this, new LuaTabEventArgs(tab));
        }

        internal void RaiseScriptStart(LuaScriptTab tab)
        {
            if (SelectedLuaScript == tab)
            {
                ScriptStart?.Invoke(this, new LuaTabEventArgs(tab));
            }
        }

        internal void RaiseScriptStop(LuaScriptTab tab)
        {
            if (SelectedLuaScript == tab)
            {
                ScriptStop?.Invoke(this, new LuaTabEventArgs(tab));
            }
        }

        internal void RaiseCurrentTabIsModifiedChanged(LuaScriptTab tab)
        {
            if (SelectedLuaScript == tab)
            {
                CurrentTabIsModifiedChanged?.Invoke(this, new LuaTabEventArgs(tab));
            }
        }

        /// <summary>
        /// The open file dialog will be in the directory of this tab's script if applicable.
        /// </summary>
        public bool SeOpenFile(Action<bool> loading, string startDir = null)
        {
            loading(false);

            var dlg = new Microsoft.Win32.OpenFileDialog()
            {
                CheckFileExists = true,
                AddExtension = false,
                DefaultExt = ".lua",
                FileName = "",
                Filter = "Lua Scripts|*.lua",
                Title = "Open 1 or more Lua Script(s)",
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true
            };

            if (startDir != null && Directory.Exists(startDir))
            {
                dlg.InitialDirectory = startDir;
            }
            else
            {
                //if (Directory.Exists("..\\..\\..\\TestLuaScripts"))
                //{
                //    dlg.InitialDirectory = "..\\..\\..\\TestLuaScripts"; //TODO: store last user save/load dir and load on startup
                //}
            }
            

            if (dlg.ShowDialog() ?? false)
            {
                foreach (var f in dlg.FileNames)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var newTab = AddNewTab(loading);
                        newTab.SeScriptFilePath = f;
                        newTab.INSTANT_LoadDocumentFromDisk(loading);
                    });
                    return true;
                }
            }

            return false;
        }
    }
}
