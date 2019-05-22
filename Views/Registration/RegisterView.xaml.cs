using Client.Views;
using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    [Export()]
    [Export(typeof(IRegisterView))]
    public partial class RegisterView : UserControl, IView, IRegisterView
    {
        public RegisterView()
        {
            InitializeComponent();
        }
    }
}
