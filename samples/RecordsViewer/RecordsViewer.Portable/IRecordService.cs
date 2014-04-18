using RecordsViewer.Portable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{
    public interface IRecordService
    {
        Task<List<WSRecord>> GetRecordsAsync(string servicePath, IDictionary<string, object> @params);

        Task<WSRecord> GetRecordAsync(string recordId);

        Task<WSRecordLocation> GetRecordLocation(string recordId);
    }
}
