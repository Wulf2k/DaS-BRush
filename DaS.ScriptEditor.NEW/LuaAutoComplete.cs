using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DaS.ScriptEditor.NEW
{
    public class LuaAutoComplete
    {
        //Properties
        public CompletionWindow CW { get; private set; }
        public bool IsOpen { get; private set; } = false;
        //Fields
        public readonly TextEditor Editor;
        public List<SeAutoCompleteEntry> Entries = new List<SeAutoCompleteEntry>();
        public InsightWindow Insight { get; private set; }

        SeAutoCompleteEntry PrevEntry;

        public LuaAutoComplete(ref TextEditor editor)
        {
            Editor = editor;
            Editor.TextArea.TextEntered += TextArea_TextEntered;
            Editor.TextArea.TextEntering += TextArea_TextEntering;
            Editor.TextArea.PreviewKeyDown += TextArea_KeyDown;

            var ctrlSpace = new RoutedCommand();
            ctrlSpace.InputGestures.Add(new KeyGesture(Key.Space, ModifierKeys.Control));
            var cb = new CommandBinding(ctrlSpace, OnCtrlSpaceCommand);

            Editor.CommandBindings.Add(cb);
        }

        private void OnCtrlSpaceCommand(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (CW == null || !IsOpen)
            {
                InitCW();
            }
            
        }

        private void TextArea_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = (CW != null && IsOpen);
                    break;
            }
        }

        public void InitDefaultEntries(MainWindow mw)
        {
            Entries.Clear();

            var luai = new ScriptLib.Lua.LuaInterface();

            foreach (var g in luai.State.Globals)
            {
                Entries.Add(new SeAutoCompleteEntry(mw, SeAcType.Method, g.Replace("(", ""), "?Description?"));
            }

            foreach (var g in ScriptLib.Lua.LuaInterface.IngameFuncAddresses.Keys)
            {
                Entries.Add(new SeAutoCompleteEntry(mw, SeAcType.Estus, g, "?Description?"));
            }
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (char.IsLetterOrDigit(e.Text[0]) || e.Text[0] == '.' || e.Text[0] == ':')
            {
                if (!IsOpen || CW == null)
                {
                    FirstOpen(sender as TextArea, e);
                }
                else
                {
                    StillOpened(sender as TextArea, e);
                }

                PrevEntry = (CW.CompletionList.SelectedItem as SeAutoCompleteEntry) ?? null;
                IsOpen = true;
            }
            else if (e.Text[0] == '(')
            {
                InitInsight();
            }
            else
            {
                PrevEntry = null;
                if (CW != null)
                {
                    CW.Close();
                }
                IsOpen = false;
            }


        }

        private void FirstOpen(TextArea ta, TextCompositionEventArgs e)
        {
            InitCW();
        }

        private void StillOpened(TextArea ta, TextCompositionEventArgs e)
        {
            if (CW != null)
            {
                //CW.EndOffset++;
            }
            else
            {
                IsOpen = false;
            }
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            //TODO
        }

        private void InitInsight()
        {
            if (PrevEntry != null)
            {
                try
                {
                    Insight = new InsightWindow(Editor.TextArea);
                    Insight.Closed += (o, e2) => Insight = null;
                    Insight.CloseAutomatically = true;
                    Insight.Icon = PrevEntry.Image;
                    Insight.Content = "InsightWindowTest(arg1, arg2, arg3)";
                    Insight.Show();
                }
                catch
                {

                }
            }
            
        }

        private void InitCW()
        {
            CW = new CompletionWindow(Editor.TextArea);
            CW.CompletionList.KeyDown += CompletionList_KeyDown;
            CW.Closed += CW_Closed;

            //CW.StartOffset = ta.Caret.Offset;
            //CW.EndOffset = CW.StartOffset;
            CW.CompletionList.IsFiltering = true;

            foreach (var thing in Entries)
            {
                CW.CompletionList.CompletionData.Add(thing);
            }

            CW.Show();
        }

        private void CW_Closed(object sender, EventArgs e)
        {


            CW.CompletionList.KeyDown -= CompletionList_KeyDown;
            CW.Closed -= CW_Closed;
            Insight?.Close();
            CW = null;
        }

        private void CompletionList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Home:
                case Key.End:
                    e.Handled = true;
                    CW.Close();
                    break;
                //case Key.Enter:
                //case Key.Tab:
                //    //e.Handled = true;
                //    //var selItem = CW.CompletionList.SelectedItem.Text;
                //    //Editor.Select(CW.StartOffset, Editor.CaretOffset);
                //    //Editor.SelectedText = selItem;
                //    //Editor.SelectionLength = 0;
                //    ////CW.EndOffset = wordEnd;
                //    ////Editor.CaretOffset = CW.EndOffset;

                    
                //    break;
            }
        }
    }
}
