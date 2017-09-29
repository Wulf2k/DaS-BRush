using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

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
        public const string Xmlns = @"xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'";
        public const string XmlnsX = @"xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'";

        //Note: <InlineUIContainer> can straight up just have controls inside it like <Button/>

        public static TextBlock GetTextBlockElement(string inputText)
        {
            return System.Windows.Markup.XamlReader.Parse($@"<TextBlock {Xmlns} {XmlnsX}>{inputText}</TextBlock>") as TextBlock;
        }

        public SeAcType AcType { get; private set; }
        public readonly MainWindow Main;

        public SeAutoCompleteEntry(MainWindow main, SeAcType acType, string luaCompletionText, string dispText, string desc, bool useTextBlockFormatting)
        {
            Main = main;
            AcType = acType;
            Text = luaCompletionText;
            DescriptionText = desc;
            DispText = dispText;
            UseTextBlockFormatting = useTextBlockFormatting;
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return Main.Resources[("AutoComplete" + AcType.ToString())] as BitmapImage; }
        }

        public bool UseTextBlockFormatting { get; private set; }

        public string Text { get; private set; }

        public string DispText { get; private set; }

        // Use this property if you want to show a fancy UIElement in the list.
        public object Content
        {
            get
            {
                if (UseTextBlockFormatting)
                {
                    return GetTextBlockElement(DispText);
                }
                else
                {
                    return DispText;
                }
            }
        }

        public string DescriptionText { get; private set; }

        public object Description
        {
            get
            {
                if (UseTextBlockFormatting)
                {
                    return GetTextBlockElement(DescriptionText);
                }
                else
                {
                    return DescriptionText;
                }
            }
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
