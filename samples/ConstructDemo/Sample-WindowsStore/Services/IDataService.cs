using Accela.WindowsStoreSDK;
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
    /// Provide web API call interfaces
    /// </summary>
    public interface IDataService
    {
        Task<JsonObject> GetRecords(AccelaSDK sdk);
        Task<JsonObject> GetRecord(AccelaSDK sdk, string[] ids);
        Task<JsonObject> CreateRecord(AccelaSDK sdk, string recordType);
        Task<JsonObject> SearchRecords(AccelaSDK sdk);
        Task<JsonObject> GetRecordDocumentList(AccelaSDK sdk, string recordId);
        Task<JsonObject> CreateRecordDocument(AccelaSDK sdk, string recordId, IDictionary<string, object> postData);

        Task<JsonObject> GetAppSettings(AccelaSDK sdk);

        Task<byte[]> DownloadDocument(AccelaSDK sdk, string documentId);
    }
}
