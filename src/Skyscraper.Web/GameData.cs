using Newtonsoft.Json;

namespace Skyscraper.Web
{
    public class GameData
    {
        [JsonProperty(PropertyName = "symbolsForEachCard")]
        public int SymbolsForEachCard { get; set; }
    }
}