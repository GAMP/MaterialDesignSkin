using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetSetNewPasswordView.xaml
    /// </summary>
    [Export()]
    public partial class PasswordResetSetNewPasswordView : UserControl, IView
    {
        public PasswordResetSetNewPasswordView()
        {
            InitializeComponent();
        }
    }
}
