using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecordsViewer.Portable.Entities
{
    public abstract class WSIdentifierBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("display")]
        public string Display { get; set; }
    }
}
