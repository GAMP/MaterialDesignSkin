using Client.Views;
using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetView.xaml
    /// </summary>
    [Export()]
    [Export(typeof(IPasswordResetView))]
    public partial class PasswordResetView : UserControl, IView, IPasswordResetView
    {
        public PasswordResetView()
        {
            InitializeComponent();
        }
    }
}
