using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterConfirmErrorView.xaml
    /// </summary>
    [Export()]
    public partial class RegisterConfirmErrorView : UserControl, IView
    {
        public RegisterConfirmErrorView()
        {
            InitializeComponent();
        }
    }
}
