using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Test10App
{
    namespace Converters
    {
        public class DateFormatConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;

                DateTime dt = DateTime.Parse(value.ToString());
                return dt.ToString("dd-M-yyyy");
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotSupportedException();
            }
        }
    }
}
