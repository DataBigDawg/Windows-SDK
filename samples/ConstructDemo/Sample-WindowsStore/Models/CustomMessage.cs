using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.Models
#else
namespace Accela.WindowsStore.Sample.Models
#endif
{
    public class CustomMessage
    {
        public CustomMessage(string message) :
            this(string.Empty, message)
        {

        }

        public CustomMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }
    }
}
