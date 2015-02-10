using Microsoft.AspNet.Mvc;

[Route("api/[controller]")]
public class GameInfoController
{
    private readonly IGameFactory _gameFactory;

    public GameInfoController(IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
    }

    [HttpPost]
    public GameInfo Create([FromBody]GameData gameData)
    {
        return _gameFactory.Create(gameData.SymbolsForEachCard);
    }
}