using DaS.ScriptLib.LuaScripting;
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

        private void CheckAddNewEntry(SeAutoCompleteEntry seace)
        {
            if (!Entries.Any(x => x.Text == seace.Text))
            {
                Entries.Add(seace);
            }
        }

        public void InitDefaultEntries(MainWindow mw)
        {
            Entries.Clear();

            foreach (var kvp in DSLua.G)
            {
                CheckAddNewEntry(new SeAutoCompleteEntry(mw, SeAcType.Method, kvp.Key.ToString().Replace("(", ""), kvp.Key.ToString().Replace("(", ""), "?Description?"));
            }

            foreach (var e in ScriptLib.LuaScripting.helpers.AutoCompleteHelper.IngameFunctionsFancy)
            {
                CheckAddNewEntry(new SeAutoCompleteEntry(mw, SeAcType.Estus, e.CompletionText, e.ListDisplayText, e.Description));
            }

            foreach (var e in ScriptLib.LuaScripting.helpers.AutoCompleteHelper.IngameFunctionsUnmarked)
            {
                CheckAddNewEntry(new SeAutoCompleteEntry(mw, SeAcType.Estus, e.CompletionText, e.ListDisplayText, e.Description));
            }

            foreach (var e in ScriptLib.LuaScripting.helpers.AutoCompleteHelper.LuaHelperEntries)
            {
                CheckAddNewEntry(new SeAutoCompleteEntry(mw, SeAcType.Estus, e.CompletionText, e.ListDisplayText, e.Description));
            }

            Entries = Entries.OrderBy(x => x.Text).ToList();
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

            var next = ICSharpCode.AvalonEdit.Document.TextUtilities.GetNextCaretPosition(Editor.Document, Editor.CaretOffset, System.Windows.Documents.LogicalDirection.Backward, ICSharpCode.AvalonEdit.Document.CaretPositioningMode.WordStart);

            if (next >= 0 && next < Editor.Text.Length)
            {
                CW.StartOffset = next;
            }
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



            CW.Width = 320;

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
