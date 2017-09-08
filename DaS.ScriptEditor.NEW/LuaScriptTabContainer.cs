using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;
using System.Windows;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;

namespace DaS.ScriptEditor.NEW
{
    public class LuaScriptTabContainer : LayoutDocumentPane
    {
        public TextEditor LuaEditor;
        public LuaAutoComplete AutoComplete;

        public event EventHandler<LuaTabSwitchEventArgs> NewTabSelected;
        public event EventHandler<LuaTabSwitchEventArgs> OldTabDeselected;
        public event EventHandler<LuaTabSwitchEventArgs> ScriptStart;
        public event EventHandler<LuaTabSwitchEventArgs> ScriptStop;

        private LuaScriptTab __selectedLuaScript;
        public LuaScriptTab SelectedLuaScript
        {
            get
            {
                return SelectedContent as LuaScriptTab;
            }
            set
            {
                OnOldTabDeselected(new LuaTabSwitchEventArgs(__selectedLuaScript));
                OnNewTabSelected(new LuaTabSwitchEventArgs(value));

                __selectedLuaScript = value;
            }
        }

        public LuaScriptTabContainer() : base()
        {
            AddNewTab();
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

        public LuaScriptTab AddNewTab()
        {
            if (LuaEditor == null)
            {
                InitEditor();
                AutoComplete = new LuaAutoComplete(ref LuaEditor);
            }

            var newTab = new LuaScriptTab();

            if (ChildrenCount == 0)
            {
                newTab.Content = LuaEditor;
                LuaEditor.Document = newTab.EditorDocument;
            }

            Children.Add(newTab);
            newTab.InitEvents();
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

        public bool SaveAll()
        {
            foreach(var thing in Children)
            {
                if (!((thing as LuaScriptTab).SeSave()))
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

        protected virtual void OnNewTabSelected(LuaTabSwitchEventArgs e)
        {
            NewTabSelected?.Invoke(this, e);
        }

        protected virtual void OnOldTabDeselected(LuaTabSwitchEventArgs e)
        {
            OldTabDeselected?.Invoke(this, e);
        }

        internal void RaiseScriptStart(LuaTabSwitchEventArgs e)
        {
            if (SelectedLuaScript == e.Script)
            {
                ScriptStart?.Invoke(this, e);
            }
        }

        internal void RaiseScriptStop(LuaTabSwitchEventArgs e)
        {
            if (SelectedLuaScript == e.Script)
            {
                ScriptStop?.Invoke(this, e);
            }
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

            if (dlg.ShowDialog() ?? false)
            {
                foreach (var f in dlg.FileNames)
                {
                    var newTab = AddNewTab();
                    newTab.SeScriptFilePath = f;
                    newTab.INSTANT_LoadDocumentFromDisk();
                    return true;
                }
            }

            return false;
        }
    }
}
