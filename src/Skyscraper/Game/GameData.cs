using Newtonsoft.Json;

namespace Skyscraper.Game
{
    public class GameData
    {
        [JsonProperty(PropertyName = "symbolsForEachCard")]
        public int SymbolsForEachCard { get; set; }
    }
}