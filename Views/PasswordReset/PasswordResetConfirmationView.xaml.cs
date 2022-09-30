using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetConfirmationView.xaml
    /// </summary>
    [Export()]
    public partial class PasswordResetConfirmationView : UserControl, IView
    {
        public PasswordResetConfirmationView()
        {
            InitializeComponent();
        }
    }
}
