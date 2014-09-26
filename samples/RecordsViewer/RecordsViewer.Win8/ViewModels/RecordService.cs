using RecordsViewer.Portable;
using RecordsViewer.Portable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accela.WindowsStoreSDK;
using Newtonsoft.Json;

namespace RecordsViewer.ViewModels
{   /// <summary>
    /// Provide get a specific Record or Records list function.
    /// </summary>
    public class RecordService : IRecordService
    {   /// <summary>
        /// Get record by id
        /// </summary>
        /// <param name="recordId">ID of record</param>
        /// <returns>An asynchronnous operation based on record</returns>
        public Task<WSRecord> GetRecordAsync(string recordId)
        {
            return Task.Factory.StartNew(() =>
            {
                return App.RecordsViewModel.Records.FirstOrDefault(c => c.Id == recordId);
            });
        }

        /// <summary>
        /// Get records list
        /// </summary>
        /// <param name="servicePath">Record uri path for API</param>
        /// <param name="@params">parameters for uri</param>
        /// <returns>An asynchronnous operation based on record list</returns>
        public Task<List<WSRecord>> GetRecordsAsync(string servicePath, IDictionary<string, object> @params)
        {
            return Task.Run(() =>
            {
                List<WSRecord> list = new List<WSRecord>();
                try{
                    JsonObject jsonObj = App.SharedSDK.PostAsync(servicePath, @params).Result;
                    list = Accela.WindowsStoreSDK.HttpWebResponseWrapper.customizeResponse<List<WSRecord>>(jsonObj);
                }
                catch (Accela.WindowsStoreSDK.AccelaApiException ex)
                {
                    if (ex != null)
                        throw ex;
                }
                return list;
            });
        }
    }
}
