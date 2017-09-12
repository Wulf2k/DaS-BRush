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

        public event EventHandler<SeButtonEventArgs> SeButtonEnabledChanged;

        protected virtual void RaiseSeButtonClicked(SeButton b)
        {
            SeButtonClicked?.Invoke(this, new SeButtonEventArgs(b));
        }

        protected virtual void RaiseSeButtonEnabledChanged(SeButton b)
        {
            SeButtonEnabledChanged?.Invoke(this, new SeButtonEventArgs(b));
        }

        public SeToolbar()
        {
            InitializeComponent();

            ScriptLib.Injection.Hook.Init();
        }

        public Button this[SeButton b]
        {
            get
            {
                switch(b)
                {
                    case SeButton.NewDoc: return ButtonNewDoc;
                    case SeButton.OpenFile: return ButtonOpenFile;
                    case SeButton.SaveAllFiles: return ButtonSaveAllFiles;
                    case SeButton.SaveFile: return ButtonSaveFile;
                    case SeButton.Start: return ButtonStart;
                    case SeButton.Stop: return ButtonStop;
                    case SeButton.Refresh: return ButtonRefresh;
                    default: return null;
                }
            }
        }

        public SeButton this[Button b]
        {
            get
            {
                if (b == ButtonNewDoc) return SeButton.NewDoc;
                else if (b == ButtonOpenFile) return SeButton.OpenFile;
                else if (b == ButtonSaveAllFiles) return SeButton.SaveAllFiles;
                else if (b == ButtonSaveFile) return SeButton.SaveFile;
                else if (b == ButtonStart) return SeButton.Start;
                else if (b == ButtonStop) return SeButton.Stop;
                else if (b == ButtonRefresh) return SeButton.Refresh;
                return SeButton.None;
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseSeButtonClicked(this[sender as Button]);
        }

        public void UpdateDarkSoulsVersionText(string newText)
        {
            DarkSoulsVersionLabel.Content = newText;
        }

        public void SetButtonEnabled(SeButton b, bool enabled)
        {
            this[b]?.SeSetEnabled(enabled);
            RaiseSeButtonEnabledChanged(b);
        }

        public bool GetButtonEnabled(SeButton b)
        {
            return this[b]?.IsHitTestVisible ?? false;
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
