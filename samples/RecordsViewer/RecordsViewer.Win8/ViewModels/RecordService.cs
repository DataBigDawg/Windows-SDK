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
            return Task.Run(async () =>
            {
                List<WSRecord> records = null;
                try
                {
                    RecordSearchCondition condition = new RecordSearchCondition();
                    condition.RecordId = "14RES-00000-00006";
                    string jsonString = JsonConvert.SerializeObject(condition);
                    var response = await App.SharedSDK.PostAsync(servicePath, jsonString);

                    var result = JsonConvert.DeserializeObject<WSRecordsResponse>(response.ToString());
                    records = result.WSRecords;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                }

                return records;
            });
        }

        public class RecordSearchCondition
        {
            [JsonProperty(PropertyName = "id")]
            public string RecordId { get; set; }
        }
    }
}
