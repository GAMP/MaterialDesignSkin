using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterConfirmationView.xaml
    /// </summary>
    [Export()]
    public partial class RegisterConfirmationView : UserControl, IView
    {
        public RegisterConfirmationView()
        {
            InitializeComponent();
        }
    }
}
