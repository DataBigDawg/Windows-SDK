using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Entities
{
    public class RecordSearchCondition
    {
        [JsonProperty(PropertyName = "id")]
        public string RecordId { get; set; }
    }
}
