using Newtonsoft.Json;

namespace Skyscraper
{
    public class Symbol
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; internal set; }
        [JsonProperty(PropertyName = "path")]
        public string Path { get; internal set; }
    }
}