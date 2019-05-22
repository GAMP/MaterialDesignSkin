using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterProfileView.xaml
    /// </summary>
    [Export()]
    public partial class RegisterProfileView : UserControl,IView
    {
        public RegisterProfileView()
        {
            InitializeComponent();
        }
    }
}
