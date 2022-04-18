using Client;
using MaterialDesignSkin.Source;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MaterialDesignSkin.Modules
{
    /// <summary>
    /// Material skin bootstrapper.
    /// </summary>
    [Export(typeof(ISkinBootStrapper))]
    public class BootStrapper : ISkinBootStrapper
    {
        #region IMPORTS

        [Import()]
        private IClient Client
        {
            get; set;
        }

        #endregion

        #region ISkinBootStrapper
        public async Task InitializeAsync(string configFileName)
        {
            try
            {
                if (File.Exists(configFileName))
                {
                    //deserialize configuration
                    var materialConfig = Client.JsonDeserializeConfig<MaterialConfig>(configFileName);

                    //set render mode
                    RenderOptions.ProcessRenderMode = materialConfig.SoftwareRendering ? System.Windows.Interop.RenderMode.SoftwareOnly : System.Windows.Interop.RenderMode.Default;

                    //disable shadows if configured
                    SkinShadowExtension.DisableShadows = materialConfig.DisableShadows;

                    //disable points if configured
                    SettingsHelper.HidePoints = materialConfig.HidePoints;

                    //hide balance if configured
                    SettingsHelper.HideBalance = materialConfig.HideBalance;

                    //hide active applications if enabled
                    SettingsHelper.HideActiveApplications = materialConfig.HideActiveApplications;

                    //hide application rating if enabled
                    SettingsHelper.HideAppRating = materialConfig.HideAppRating;

                    //hide application filters if enabled
                    SettingsHelper.HideAppFilters = materialConfig.HideAppFilters;

                    //hide application info if enabled
                    SettingsHelper.HideAppInfo = materialConfig.HideAppInfo;

                    //hide user login component if configured
                    SettingsHelper.HideUserLoginComponent = materialConfig.HideUserLoginComponent;

                    //hide display language if configured
                    SettingsHelper.HideDisplayLanguage = materialConfig.HideDisplayLanguage;

                    //hide input language if configured
                    SettingsHelper.HideInputLanguage = materialConfig.HideInputLanguage;
                }
            }
            catch(Exception ex)
            {
                //trace exception
                Client.LogAddError("Skin bootstrapper failed.", ex, SharedLib.LogCategories.Generic);
            }

            var wpfApp = Application.Current;

            //include your resources here
            await wpfApp.Dispatcher.InvokeAsync(() =>
            {
                wpfApp.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml", UriKind.Absolute) });
                wpfApp.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml", UriKind.Absolute) });
                wpfApp.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Grey.xaml", UriKind.Absolute) });
                wpfApp.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(@"/MaterialDesignSkin;component/Resources/Resources.xaml", UriKind.RelativeOrAbsolute) });
            });          
        }
        #endregion        
    }
}
