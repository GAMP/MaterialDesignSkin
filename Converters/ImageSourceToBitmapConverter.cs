using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MaterialDesignSkin.Converters
{
    /// <summary>
    /// Converts image source file to a bitmap with main purpose to leave source file open.     
    /// </summary>
    public class ImageSourceToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //check if value is an string and not null or empty
            if (value is string path && !string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    if (File.Exists(path))
                    {
                        var uri = new Uri(path);
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = uri;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        return bitmap;
                    }
                }
                catch
                {
                    //we dont want to throw excpetions here
                    //the exception might be related to invalid file name or invalid file
                    //in all that cases we will just return binding unset value
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
