using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RecordsViewer.Converter
{    /// <summary>
    /// Uri Converter
    /// </summary>
    public class UriConverter : IValueConverter
    {   /// <summary>
        /// Convert uri for searching records API
        /// </summary>
        /// <param name="value">Id value, used to search to a specific id record.</param>
        /// <param name="value">Parameter object, used for the path of the uri.</param>
        /// <param name="value">Id value, used to search to a specific id record.</param>
        /// <returns>The objet of the new created uri.</returns>
        public object Convert(object value, object parameter)
        {
            if (value == null) return value;
            if(parameter==null) return value;

            string id = (string)value;
            string pagePath = (string)parameter;
            
            string relativeUri= string.Format("{0}?id={1}", pagePath, id);

            return new Uri(relativeUri, UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
