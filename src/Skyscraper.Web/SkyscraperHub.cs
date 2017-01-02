using Microsoft.AspNet.SignalR;
using Skyscraper.Core;
using System.Collections.Generic;
using System.Linq;

namespace Skyscraper.Web
{
    public class SkyscraperHub : Hub
    {
        // Is set via the constructor on each creation
        private readonly IGame _game;
        private GameInfo _gameInfo;
        private IList<Card> _usedCards;
        private static bool tooLate;
        private ISymbolsProvider _symbolsProvider;

        public SkyscraperHub(IGame game, ISymbolsProvider symbolsProvider)
        {
            _game = game;
            _symbolsProvider = symbolsProvider;
            _symbolsProvider.Init(System.Web.Hosting.HostingEnvironment.MapPath("~/content/icons"));
        }

        public void StartGame(int symbols, PlayerViewModel playerViewModel)
        {
            _game.Init(symbols);
            AddPlayer(playerViewModel);
            _game.DistributeFirstCard();
            var card = _game.ExtractCard();
            var extractedCardsymbols = GetSymbols(card);
            foreach (var player in _game.GetPlayers())
            {
                Clients.Client(player.ConnectionId).start(GetSymbols(_game.GetPlayerCurrentCard(player.Id)), extractedCardsymbols, GetPlayers());
            }
        }

        public void ExtractCard()
        {
            var card = _game.ExtractCard();
            if (card == null)
            {
                var stats = _game.GetGameStats();
                Clients.All.gameOver(stats);
            }
            else
            {
                var symbols = GetSymbols(card);
                Clients.All.setExtractedCard(symbols, GetPlayers());
            }
        }

        public void CardMatched(IEnumerable<int> symbols, string id)
        {
            if (tooLate)
                return;
            tooLate = true;
            _game.AddCardToPlayer(id, new Card(symbols.Select(s => new Line(s)).ToList()));
            ExtractCard();
            tooLate = false;
        }

        public void AddPlayer(PlayerViewModel player)
        {
            _game.AddPlayer(player.displayName, player.imageUrl, Context.ConnectionId, player.id);
            //var card = _game.CurrentlyExtractedCard();
            //var symbols = GetSymbols(card);
            //Clients.Client(Context.ConnectionId).joinGame(symbols, GetSymbols(_game.GetPlayerCurrentCard(player.id)), GetPlayers().First(p  => p.id == player.id));
            //foreach (var p in _game.GetPlayers())
            //{
            //    Clients.Client(p.ConnectionId).setPlayers(GetPlayers());
            //}
        }

        public void ResetGame()
        {
            _game.ResetGame();
        }

        private IQueryable<PlayerViewModel> GetPlayers()
        {
            return _game.GetPlayers().Select(p => new PlayerViewModel { displayName = p.DisplayName, imageUrl = p.ImageUrl, points = p.Cards.Count - 1, id = p.Id }).AsQueryable();
        }

        private IEnumerable<Symbol> GetSymbols(Card card)
        {
            return card.Lines.Select(l => _symbolsProvider.GetSymbol(l.Id));
        }
    }
}