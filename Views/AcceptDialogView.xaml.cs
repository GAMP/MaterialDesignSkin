using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for AcceptDialogView.xaml
    /// </summary>
    [Export()]
    public partial class AcceptDialogView : UserControl , IView
    {
        public AcceptDialogView()
        {
            InitializeComponent();
        }
    }
}
