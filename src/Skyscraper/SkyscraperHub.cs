using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SkyscraperCore;
using System.Collections.Generic;
using System.Linq;

namespace Skyscraper
{
    public class SkyscraperHub : Hub
    {
        // Is set via the constructor on each creation
        private readonly IHubContext _hubContext;
        private BasicShapeModel _model;
        private readonly IGame _game;
        private GameInfo _gameInfo;
        private IList<Card> _usedCards;
        private static bool tooLate;

        public SkyscraperHub(IConnectionManager connectionManager, IGame game)
        {
            _hubContext = connectionManager.GetHubContext<SkyscraperHub>();
            _game = game;
        }

        public void StartGame(int symbols)
        {
            _game.StartGame();
            _game.Init(symbols);
            _game.DistributeFirstCard();
            var card = _game.ExtractCard();
            var extractedCardsymbols = card.Lines.Select(l => l.Id);
            foreach (var player in _game.GetPlayers())
            {
                Clients.Client(player.ConnectionId).start(_game.GetPlayerCurrentCard(player.Id).Lines.Select(l => l.Id), extractedCardsymbols, GetPlayers());
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
                var symbols = card.Lines.Select(l => l.Id);
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
            Clients.All.setPlayers(GetPlayers());
            if (_game.GameStarted())
            {
                //joining the current game
                var card = _game.CurrentlyExtractedCard();
                var symbols = card.Lines.Select(l => l.Id);
                Clients.Client(Context.ConnectionId).joinGame(symbols, _game.GetPlayerCurrentCard(player.id).Lines.Select(l => l.Id));
            }
        }

        private IEnumerable<PlayerViewModel> GetPlayers()
        {
            return _game.GetPlayers().Select(p => new PlayerViewModel { displayName = p.DisplayName, imageUrl = p.ImageUrl, points = p.Cards.Count, id = p.Id });
        }
    }
}