using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    [Export()]
    public partial class ShopView : UserControl, IView
    {
        public ShopView()
        {
            InitializeComponent();
        }
    }
}
