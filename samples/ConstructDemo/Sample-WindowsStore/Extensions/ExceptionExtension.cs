using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP 

namespace Accela.WindowsPhone.Sample.Extensions
#else
namespace Accela.WindowsStore.Sample.Extensions
#endif
{   /// <summary>
    /// Utility class to handle exception message
    /// </summary>
    public static class ExceptionExtension
    {   /// <summary>
        /// Get the message of the exception
        /// </summary>
        public static string GetUnderExceptionMessage(this Exception exception)
        {
            if (exception == null)
                return null;
            Exception underException = exception;
            while (underException.InnerException != null)
            {
                underException = underException.InnerException;
            }
            return underException.Message;
        }
    }
}
