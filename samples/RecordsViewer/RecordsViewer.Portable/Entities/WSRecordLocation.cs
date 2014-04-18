using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecordsViewer.Portable.Entities
{
    public class WSRecordLocation
    {
        [JsonProperty("geometryPoint")]
        public WSGeometryPoint GeometryPoint { get; set; }
    }

    public class WSGeometryPoint
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("X")]
        public string X { get; set; }
        [JsonProperty("Y")]
        public string Y { get; set; }
    }
}
