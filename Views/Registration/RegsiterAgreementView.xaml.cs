using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for RegsiterAgreementView.xaml
    /// </summary>
    [Export()]
    public partial class RegsiterAgreementView : UserControl, IView
    {
        public RegsiterAgreementView()
        {
            InitializeComponent();
        }
    }
}
