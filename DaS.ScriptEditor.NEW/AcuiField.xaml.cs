using System.Windows;
using System.Windows.Controls;

namespace DaS.ScriptEditor.NEW
{
    /// <summary>
    /// Interaction logic for AcuiField.xaml
    /// </summary>
    public partial class AcuiField : UserControl
    {
        public AcuiField()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsTypeProperty =
             DependencyProperty.Register("IsType", typeof(bool), typeof(AcuiField));
        public bool IsType
        {
            get { return (bool)GetValue(IsTypeProperty); }
            set { SetValue(IsTypeProperty, value); }
        }
    }
}
