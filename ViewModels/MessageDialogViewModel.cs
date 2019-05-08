using Client;
using Client.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace MaterialDesignSkin.ViewModels
{
    [Export(), PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(IMessageDialogViewModel))]
    public class MessageDialogViewModel : SharedLib.PropertyChangedBase, IMessageDialogViewModel
    {
        #region FIELDS
        private string message;
        private MessageDialogButtons buttons = MessageDialogButtons.Accept;
        private ICommand acceptCommand, cancelCommand;
        #endregion

        #region PROPERTIES

        public ICommand AcceptCommand
        {
            get { return acceptCommand; }
            set { SetProperty(ref acceptCommand, value); }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { SetProperty(ref cancelCommand, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public MessageDialogButtons Buttons
        {
            get { return buttons; }
            set
            {
                SetProperty(ref buttons, value);
                RaisePropertyChanged(nameof(ShowAccept));
                RaisePropertyChanged(nameof(ShowCancel));
            }
        }

        public bool ShowAccept
        {
            get { return Buttons.HasFlag(MessageDialogButtons.Accept); }
        }

        public bool ShowCancel
        {
            get { return Buttons.HasFlag(MessageDialogButtons.Cancel); }
        }

        #endregion
    }
}
