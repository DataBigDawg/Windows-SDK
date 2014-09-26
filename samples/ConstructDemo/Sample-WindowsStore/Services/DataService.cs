using Accela.WindowsStoreSDK;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{   /// <summary>
    /// Provide web API call methods
    /// </summary>
    public class DataService : IDataService
    {
        public Task<JsonObject> GetRecords(AccelaSDK sdk)
        {
            var path = "/v4/records";
            return sdk.GetAsync(path);
        }

        public Task<JsonObject> CreateRecord(AccelaSDK sdk, string recordType)
        {
            var path = "/v4/records";
            var postObject = new
            {
                type = new
                {
                    value = recordType//"ServiceRequest/Trees and Weeds/Tall Grass and Weeds/NA"
                }
            };
            var jsonData = sdk.SerializeJson(postObject);
            return sdk.PostAsync(path, jsonData);
        }

        public Task<JsonObject> SearchRecords(AccelaSDK sdk)
        {
            // search records opened in the last 7 days
            var path = "/v4/search/records";
            var postJson = "{\"openedDateFrom\":\"" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "\"}";
            return sdk.PostAsync(path, postJson);
        }


        public Task<JsonObject> GetRecord(AccelaSDK sdk, string[] ids)
        {
            var path = "/v4/records/{ids}";
            dynamic urlParams = new ExpandoObject();
            urlParams.ids = string.Join(",", ids);
            return sdk.GetAsync(path, urlParams);
        }

        public Task<JsonObject> GetRecordDocumentList(AccelaSDK sdk, string recordId)
        {
            var path = "/v4/records/{recordId}/documents";
            dynamic urlParams = new ExpandoObject();
            urlParams.recordId = recordId;
            return sdk.GetAsync(path, urlParams);
        }

        public Task<JsonObject> CreateRecordDocument(AccelaSDK sdk, string recordId, IDictionary<string, object> postData)
        {
            var path = "/v4/records/{recordId}/documents";
            postData.Add("recordId", recordId);
            return sdk.UploadAttachmentAsync(path, postData);
        }

        public Task<byte[]> DownloadDocument(AccelaSDK sdk, string documentId)
        {
            var path = "/v4/documents/{documentId}/download";
            var postData = new { documentId = documentId };
            return sdk.DownloadAttachmentAsync(path, postData);
        }

        public Task<JsonObject> GetAppSettings(AccelaSDK sdk)
        {
            var path = "/v4/appsettings";
            return sdk.GetAsync(path);
        }
    }
}
