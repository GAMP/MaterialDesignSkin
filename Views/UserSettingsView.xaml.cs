using System.ComponentModel.Composition;
using System.Windows.Controls;
using Client.Views;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for UserSettingsView.xaml
    /// </summary>
    [Export(typeof(IUserSettingsView))]
    public partial class UserSettingsView : UserControl, IUserSettingsView
    {
        public UserSettingsView()
        {
            InitializeComponent();
        }
    }
}
