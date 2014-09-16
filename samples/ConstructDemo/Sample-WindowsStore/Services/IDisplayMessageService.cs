using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{   /// <summary>
    /// Show API response message interface
    /// </summary>
    public interface IDisplayMessageService
    {
        void Show(string title, string message);
        void Show(string message);
    }
}
