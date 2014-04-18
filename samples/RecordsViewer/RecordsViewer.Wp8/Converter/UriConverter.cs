using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RecordsViewer.Converter
{
    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return value;
            if(parameter==null) return value;

            string id = (string)value;
            string pagePath = (string)parameter;
            
            string relativeUri= string.Format("{0}?id={1}", pagePath, id);

            return new Uri(relativeUri, UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
