using Client.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for UserPasswordEditView.xaml
    /// </summary>
    [Export(typeof(IUserPasswordEditView))]
    public partial class UserPasswordEditView : UserControl , IUserPasswordEditView
    {
        public UserPasswordEditView()
        {
            InitializeComponent();
        }
    }
}
