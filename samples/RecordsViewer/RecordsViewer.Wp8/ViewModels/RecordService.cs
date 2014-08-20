using Newtonsoft.Json;
using RecordsViewer.Portable;
using RecordsViewer.Portable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.ViewModels
{
    public class RecordService : IRecordService
    {
        public Task<WSRecord> GetRecordAsync(string recordId)
        {
            return Task.Factory.StartNew(() =>
            {
                return App.RecordsViewModel.Records.FirstOrDefault(c => c.Id == recordId);
            });
        }

        public Task<List<WSRecord>> GetRecordsAsync(string servicePath, IDictionary<string, object> @params)
        {
            return Task.Run(() =>
            {
                List<WSRecord> records = null;
                try
                {
                    var response = App.SharedSDK.GetAsync(servicePath, @params).Result;
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
    }
}
