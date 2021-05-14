using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoulsOrganizer.Converters
{
    public class ConverterNullToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = !ReferenceEquals(value, null);
            if (string.Equals("NOT", parameter?.ToString(), StringComparison.InvariantCultureIgnoreCase))
                visible = !visible;
            return  visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
