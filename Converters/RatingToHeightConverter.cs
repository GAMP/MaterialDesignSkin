using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignSkin.Converters
{
    public class RatingToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return 322;
            }

            return 400;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
