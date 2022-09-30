using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetSelectUserView.xaml
    /// </summary>
    [Export()]
    public partial class PasswordResetSelectUserView : UserControl, IView
    {
        public PasswordResetSelectUserView()
        {
            InitializeComponent();
        }
    }
}
