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
