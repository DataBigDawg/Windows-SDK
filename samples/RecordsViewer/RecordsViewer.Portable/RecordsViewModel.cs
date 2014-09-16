using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordsViewer.Portable.Entities;

namespace RecordsViewer.Portable
{
    /// <summary>
    /// Model class with record service and records, integrated with get records function.
    /// </summary>
    public class RecordsViewModel : NotifyPropertyBase
    {
        private IRecordService _recordService;

        public ObservableCollection<WSRecord> Records { get; private set; }

        public RecordsViewModel(IRecordService recordService)
        {
            this._recordService = recordService;
            this.Records = new ObservableCollection<WSRecord>();
        }
        /// <summary>
        /// get the lastest 10 records asynchronously
        /// </summary>
        public Task LoadRecordsAsync()
        {
            return Task.Run(() =>
            {
                var apiPath = "/v4/search/records?expand=addresses";
                                
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("limit", 10);
                    parameters.Add("offset", 0);
                    var records = _recordService.GetRecordsAsync(apiPath, parameters).Result;
                    if (records == null)
                        return;
                    Records = new ObservableCollection<WSRecord>(records.OrderByDescending(c => c.OpenedDate));
                }
                catch (Accela.WindowsStoreSDK.AccelaApiException ex)
                {
                    if (ex != null)
                        throw ex;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                }
            });
        }
    }
}
