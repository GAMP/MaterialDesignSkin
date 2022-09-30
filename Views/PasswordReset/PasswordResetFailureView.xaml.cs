using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetFailureView.xaml
    /// </summary>
    [Export()]
    public partial class PasswordResetFailureView : UserControl, IView
    {
        public PasswordResetFailureView()
        {
            InitializeComponent();
        }
    }
}
