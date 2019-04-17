using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for AcceptDialogViewEx.xaml
    /// </summary>
    [Export()]
    public partial class AcceptDialogViewEx : UserControl,IView
    {
        public AcceptDialogViewEx()
        {
            InitializeComponent();
        }
    }
}
