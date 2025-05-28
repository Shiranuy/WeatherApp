using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherApp.Converters
{
    public class BoolToFavoriteTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "★ Удалить из избранного" : "☆ Добавить в избранное";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}