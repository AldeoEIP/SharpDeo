using Newtonsoft.Json;

namespace SharpDeo.Model {
    public class ServerDateTime {
        public string Date { get; set; }
        [JsonProperty ("timezone_type")]
        public int TimezoneType { get; set; }
        public string Timezone { get; set; }
    }
}