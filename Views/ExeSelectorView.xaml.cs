using Client.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for ExeSelectorView.xaml
    /// </summary>
    [Export(typeof(IExeSelectorView))]
    public partial class ExeSelectorView : UserControl, IExeSelectorView
    {
        public ExeSelectorView()
        {
            InitializeComponent();
        }
    }
}
