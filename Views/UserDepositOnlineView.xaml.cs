using Client.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for UserDepositOnlineView.xaml
    /// </summary>
    [Export(typeof(IUserDepositOnlineView))]
    public partial class UserDepositOnlineView : UserControl, IUserDepositOnlineView
    {
        public UserDepositOnlineView()
        {
            InitializeComponent();
        }
    }
}
