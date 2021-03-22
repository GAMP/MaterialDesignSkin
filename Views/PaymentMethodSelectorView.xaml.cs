using Client.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for PaymentMethodSelectorView.xaml
    /// </summary>
    [Export(typeof(IPaymentMethodSelectorView))]
    public partial class PaymentMethodSelectorView : UserControl, IPaymentMethodSelectorView
    {
        public PaymentMethodSelectorView()
        {
            InitializeComponent();
        }
    }
}
