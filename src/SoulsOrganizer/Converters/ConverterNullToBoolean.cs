using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoulsOrganizer.Converters
{
    public class ConverterNullToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = !ReferenceEquals(value, null);
            if (string.Equals("NOT", parameter?.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = !result;
            return  result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
