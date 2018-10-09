using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Effects;

namespace MaterialDesignSkin.Source
{
    [MarkupExtensionReturnType(typeof(string))]
    public class SkinShadowExtension : MarkupExtension
    {
        public SkinShadowExtension(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public static bool DisableShadows
        {
            get; set;
        } = false;

        string ResourceKey { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (DisableShadows)
                return default(Effect);

            var staticResourceExtension = new StaticResourceExtension(ResourceKey);

            return staticResourceExtension.ProvideValue(serviceProvider);
        }
    }
}
