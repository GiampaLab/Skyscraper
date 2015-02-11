using Newtonsoft.Json;

namespace Skyscraper
{
    public class GameData
    {
        [JsonProperty(PropertyName = "symbolsForEachCard")]
        public int SymbolsForEachCard { get; set; }
    }
}