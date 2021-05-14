using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace SoulsOrganizer.Converters
{
    public class MultiConvecterBoolean : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var param = $"{parameter}".ToUpper();
            bool or = param.Contains("OR");
            bool not = param.Contains("NOT");
            bool result = true;
            var booleanLists = values.OfType<bool>();
            if (values != null && booleanLists.Any())
                result = or ? booleanLists.Aggregate((x, y) => x | y) : booleanLists.Aggregate((x, y) => x & y);
            else
                result = false;
            return not ? !result : result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
