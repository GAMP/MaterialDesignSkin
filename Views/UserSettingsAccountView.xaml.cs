using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for UserSettingsAccountView.xaml
    /// </summary>
    [Export()]
    public partial class UserSettingsAccountView : UserControl,IView
    {
        public UserSettingsAccountView()
        {
            InitializeComponent();
        }
    }
}
