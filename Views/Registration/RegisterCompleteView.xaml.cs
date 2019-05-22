using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegisterCompleteView.xaml
    /// </summary>
    [Export()]
    public partial class RegisterCompleteView : UserControl,IView
    {
        public RegisterCompleteView()
        {
            InitializeComponent();
        }
    }
}
