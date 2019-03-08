using Client;
using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace MaterialDesignSkin.Modules
{
    [ModuleDescription("SECTION_MODULE_SHOP", "SECTION_MODULE_SHOP_DESCRIPTION", IsLocalized = true)]
    [ModuleIcon("SHOPPING")]
    [ModuleDisplayOrder(2)]
    [ModuleGuid("3468B227-4A51-4CEF-A213-43A1CC55E607")]
    [ModuleType(typeof(ShopSectionModule))]
    [Export(typeof(IClientSectionModule))]
    public class ShopSectionModule : ClientSectionModuleBase,
        IShopModule,
        IPartImportsSatisfiedNotification
    {
        #region IMPORTS     

        [Import(typeof(Views.ShopView))]
        public override IView View { get => base.View; protected set => base.View = value; }

        [Import(AllowDefault = true, AllowRecomposition = false)]
        private IClientShopViewModel ViewModel
        {
            get; set;
        }

        #endregion

        #region OVERRIDES

        public override Task SwitchInAsync(CancellationToken ct)
        {
            return base.SwitchInAsync(ct);
        }

        public override Task SwitchOutAsync(CancellationToken ct)
        {
            return base.SwitchOutAsync(ct);
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
