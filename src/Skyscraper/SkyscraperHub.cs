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
        private IList<Point> _usedCards;
        public SkyscraperHub(IConnectionManager connectionManager, IGame game)
        {
            _hubContext = connectionManager.GetHubContext<SkyscraperHub>();
            _game = game;
        }
        public void UpdateModel(BasicShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            _model = clientModel;
            _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
        }

        public void StartGame()
        {
            _game.StartGame();
        }

        public void InitGame(int symbols)
        {
            _game.Init(symbols);
            var card = _game.DistributeFirstCard(Context.ConnectionId);
            Clients.Client(Context.ConnectionId).start(card.Lines.Select(l => l.Id));
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
                Clients.All.setExtractedCard(symbols);
            }
        }

        public void CardMatched(IEnumerable<int> symbols)
        {
            _game.AddCardToPlayer(Context.ConnectionId, new Point(symbols.Select(s => new Line(s)).ToList()));
        }

        public void AddBox()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateBox();
        }
        public void AddCircle()
        {
            //mouseConstraint.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(Context.ConnectionId).updateCircle();
        }
    }
}