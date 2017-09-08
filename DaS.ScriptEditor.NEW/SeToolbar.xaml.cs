using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DaS.ScriptEditor.NEW
{
    /// <summary>
    /// Interaction logic for ScriptEditorToolbar.xaml
    /// </summary>
    public partial class SeToolbar : UserControl
    {
        public event EventHandler<SeButtonEventArgs> SeButtonClicked;

        protected virtual void RaiseSeButtonClicked(SeButtonEventArgs e)
        {
            SeButtonClicked?.Invoke(this, e);
        }

        public SeToolbar()
        {
            InitializeComponent();

            ScriptLib.Injection.Hook.Init();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            SeButtonEventArgs args = new SeButtonEventArgs(SeButton.None);
            var btn = sender as Button;

            if (btn == ButtonNewDoc) args.ButtonType = SeButton.NewDoc;
            else if (btn == ButtonOpenFile) args.ButtonType = SeButton.OpenFile;
            else if (btn == ButtonSaveAllFiles) args.ButtonType = SeButton.SaveAllFiles;
            else if (btn == ButtonSaveFile) args.ButtonType = SeButton.SaveFile;
            else if (btn == ButtonStart) args.ButtonType = SeButton.Start;
            else if (btn == ButtonStop) args.ButtonType = SeButton.Stop;
            else if (btn == ButtonRefresh) args.ButtonType = SeButton.Refresh;

            RaiseSeButtonClicked(args);
        }

        public void UpdateDarkSoulsVersionText(string newText)
        {
            DarkSoulsVersionLabel.Content = newText;
        }
    }

    public enum SeButton
    {
        ExitEntireProgram,
        None,
        NewDoc,
        OpenFile,
        Refresh,
        SaveAllFiles,
        SaveFile,
        SaveAs,
        Start,
        Stop
    }

    public class SeButtonEventArgs : EventArgs
    {
        public SeButton ButtonType { get; set; }

        public SeButtonEventArgs(SeButton type)
        {
            ButtonType = type;
        }
    }


}
