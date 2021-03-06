﻿using Client;
using Client.ViewModels;
using MaterialDesignSkin.Views;
using SharedLib.Views;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace MaterialDesignSkin.Modules
{
    [ModuleDescription("USER_SETTINGS_MODULE_ACCOUNT_TITLE", IsLocalized = true)]
    [ModuleGuid("EDADC6BC-4D07-47DF-86B8-3D0945C48A15")]
    [ModuleIcon("Account")]
    [ModuleDisplayOrder(0)]
    [Export(typeof(IClientUserSettingsModule))]
    public class UserSettingsAccountModule : ClientUserSettingsModuleBase
    {
        [Import(typeof(UserSettingsAccountView))]
        public override IView View { get => base.View; protected set => base.View = value; }

        #region IMPORTS
        [Import()]
        private IClinetCompositionService CompositionService
        {
            get; set;
        } 
        #endregion

        public override Task SwitchInAsync(CancellationToken ct)
        {
            var model = this.CompositionService.GetExportedValue<IUserProfileEditViewModel>();
            this.View.DataContext = model;
            return base.SwitchInAsync(ct);
        }
    }
}
