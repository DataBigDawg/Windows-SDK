using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP
namespace Accela.WindowsPhone.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{   /// <summary>
    /// Storage service interface
    /// </summary>
    public interface IStorageService
    {
        Task<T> LoadAsync<T>(string fileName);

        Task SaveAsync(string fileName, object data);
    }
}
