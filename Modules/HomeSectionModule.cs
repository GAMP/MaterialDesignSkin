using System.ComponentModel.Composition;
using Client;
using Client.ViewModels;
using SharedLib.Views;

namespace MaterialDesignSkin.Modules
{
    [ModuleDescription("SECTION_MODULE_HOME_TITLE", "SECTION_MODULE_DESCRIPTION",IsLocalized =true)]
    [ModuleIcon("HOME")]
    [ModuleDisplayOrder(0)]
    [ModuleGuid("1732883C-CEED-4322-B038-E9EC0A46A129")]
    [ModuleType(typeof(HomeSectionModule))]
    [Export(typeof(IClientSectionModule))]
    public class HomeSectionModule : ClientSectionModuleBase
    {
        [Import(typeof(Views.HomeView))]
        public override IView View { get => base.View; protected set => base.View = value; }

        [Import()]
        private IShellViewModel ViewModel
        {
            get; set;
        }

        public override void OnImportsSatisfied()
        {
            base.OnImportsSatisfied();

            View.DataContext = ViewModel;
        }
    }
}
