using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignSkin.Source
{
    public class NewsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is Client.ViewModels.IFeedSourceViewModel)
            {
                return element.FindResource("FEED_SOURCE_TEMPLATE") as DataTemplate;
            }
            else if (item is Client.ViewModels.INewsViewModel)
            {
                return element.FindResource("NEWS_ITEM_TEMPLATE") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
