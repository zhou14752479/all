using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace fiddler
{
    class UrlCaptureConfiguration
    {
        [XmlIgnore]
        [JsonIgnore]
        public int ProcessId { get; set; }

        public bool IgnoreResources { get; set; }
        public string CaptureDomain { get; set; }
        public List<string> UrlFilterExclusions { get; set; }
        public List<string> ExtensionFilterExclusions { get; set; }

        public UrlCaptureConfiguration()
        {
            UrlFilterExclusions = new List<string>();
            ExtensionFilterExclusions = new List<string>();
        }
    }
}
