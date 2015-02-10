using Newtonsoft.Json;

public class GameData
{
    [JsonProperty(PropertyName = "symbolsForEachCard")]
    public int SymbolsForEachCard { get; set; }
}