using Client.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MaterialDesignSkin.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    [Export(typeof(IUserProfileEditView))]
    public partial class UserProfileEditView : UserControl, IUserProfileEditView
    {
        public UserProfileEditView()
        {
            InitializeComponent();
        }
    }
}
