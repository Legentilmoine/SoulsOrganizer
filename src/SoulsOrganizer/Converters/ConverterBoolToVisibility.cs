using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoulsOrganizer.Converters
{
    public class ConverterBoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool || value is Nullable<bool>) || value == null)
                return Visibility.Visible;

            bool booleanValue = false;
            if (value is bool)
                booleanValue = (bool)value;
            if (value is Nullable<bool>)
                booleanValue = ((Nullable<bool>)value).Value;

            if (string.Equals("NOT", $"{parameter}", StringComparison.InvariantCultureIgnoreCase))
                booleanValue = !booleanValue;
            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
