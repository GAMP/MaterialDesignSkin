using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignSkin.Converters
{
    public class AppExePopupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return false;

            if (values.Any(v => v == DependencyProperty.UnsetValue))
                return false;

            if (values.Count() < 3)
                return false;

            bool mouseOver1 = (bool)values[0];
            bool mouseOver2 = (bool)values[1];
            bool overlayShowing = (bool)values[2];

            return (mouseOver1 || mouseOver2) && !overlayShowing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
