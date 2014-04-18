using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.Extensions
#else
namespace Accela.WindowsStore.Sample.Extensions
#endif
{
    public static class ExceptionExtension
    {
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
