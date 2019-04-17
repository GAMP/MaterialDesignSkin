using System.ComponentModel.Composition;
using Client;
using Client.ViewModels;
using SharedLib.Views;

namespace MaterialDesignSkin.Modules
{
    [ModuleDescription("SECTION_MODULE_APPLICATIONS_TITLE", "SECTION_MODULE_APP_DESCRIPTION",IsLocalized =true)]
    [ModuleIcon("APPS")]
    [ModuleDisplayOrder(1)]
    [ModuleGuid("A4A98C3F-F944-46EF-9C56-359F5F896B53")]
    [ModuleType(typeof(AppSectionModule))]
    [Export(typeof(IClientSectionModule))]
    public class AppSectionModule : ClientSectionModuleBase
    {
        #region IMPORTS

        [Import(typeof(Views.AppView))]
        public override IView View { get => base.View; protected set => base.View = value; }

        [Import()]
        private IShellViewModel ViewModel
        {
            get; set;
        }

        #endregion

        #region IPartImportsSatisfiedNotification

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();

            View.DataContext = ViewModel;
        } 

        #endregion
    }
}
