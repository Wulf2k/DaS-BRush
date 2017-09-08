using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media.Imaging;

namespace DaS.ScriptEditor.NEW
{
    public enum SeAcType
    {
        Field,
        Lua,
        Method,
        Type,
        Estus
    }
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    public class SeAutoCompleteEntry : ICompletionData
    {
        public SeAcType AcType { get; private set; }
        public readonly MainWindow Main;

        public SeAutoCompleteEntry(MainWindow main, SeAcType acType, string text, string desc)
        {
            this.Main = main;
            this.AcType = acType;
            this.Text = text;
            this.Description = desc;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return Main.Resources[("AutoComplete" + AcType.ToString())] as BitmapImage; }
        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get { return this.Text; }
        }

        public object Description
        {
            get; private set;
        }

        public double Priority => 1.0;

        //"depending on how the insertion was triggered, it is an instance 
        //of TextCompositionEventArgs, KeyEventArgs, or MouseEventArgs."
        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
