using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecordsViewer.Portable.Entities
{
    [JsonObject]
    public class WSAddress : WSIdentifierBase
    {
        [JsonProperty("xCoordinate")]
        public string XCoordinate { get; set; }
        [JsonProperty("yCoordinate")]
        public string YCoordinate { get; set; }
    }
}
