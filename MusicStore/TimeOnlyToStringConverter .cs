using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicStore
{
    public class TimeOnlyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeOnly time)
            {
                // Convert TimeOnly to the desired format (hh:mm:ss)
                return time.ToString("hh\\:mm\\:ss");
            }
            return string.Empty; // Return empty if not a valid TimeOnly
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; // Not needed for this scenario
        }
    }
}

