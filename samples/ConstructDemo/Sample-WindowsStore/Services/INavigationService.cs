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
    /// Navigation service interface
    /// </summary>
    public interface INavigationService
    {
         bool CanGoBack { get; }

        void GoBack();

        void Navigate<TDestinationViewModel>(object parameter = null);
    }
}
