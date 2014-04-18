using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecordsViewer.Portable.Entities
{
    public class WSRecordsResponse 
    {
        [JsonProperty("result")]
        public List<WSRecord> WSRecords { get; set; }
    }
}
