using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable.Entities
{
    public class WSRecord : WSIdentifierBase
    {
        [JsonProperty("status")]
        public WSStatus Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("openedDate")]
        public string OpenedDate { get; set; }

        [JsonProperty("type")]
        public WSType Type { get; set; }

        [JsonProperty("addresses")]
        public List<WSAddress> Addresses { get; set; }
    }
}
