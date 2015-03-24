using System.Linq;
using Microsoft.AspNet.Mvc;
using SkyscraperCore;

namespace Skyscraper
{
    [Route("api/[controller]")]
    public class GameInfoController : Controller
    {
        private readonly IGameFactory _gameFactory;

        public GameInfoController(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        [HttpPost]
        public GameInfo Create([FromBody]GameData gameData)
        {
            var gameInfo = _gameFactory.Create(gameData.SymbolsForEachCard);
            var isCorrect = true;
            foreach (var card in gameInfo.Cards)
            {
                var card1 = card;
                foreach (var secondCard in gameInfo.Cards.Where(c => c != card1))
                {
                    var secondCard1 = secondCard;
                    var commonSymbol = card1.Lines.Where(l => secondCard1.Lines.Contains(l));
                    if (commonSymbol.Count() != 1)
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }
            return gameInfo;
        }
    }
}