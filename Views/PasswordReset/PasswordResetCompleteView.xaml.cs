using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PasswordResetCompleteView.xaml
    /// </summary>
    [Export()]
    public partial class PasswordResetCompleteView : UserControl,IView
    {
        public PasswordResetCompleteView()
        {
            InitializeComponent();
        }
    }
}
