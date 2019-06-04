using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterFailureView.xaml
    /// </summary>
    [Export()]
    public partial class RegisterFailureView : UserControl, IView
    {
        public RegisterFailureView()
        {
            InitializeComponent();
        }
    }
}
