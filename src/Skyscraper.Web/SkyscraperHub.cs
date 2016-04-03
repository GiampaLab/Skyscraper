﻿using Microsoft.AspNet.SignalR;
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
        public SkyscraperHub(IGame game)
        {
            _game = game;
        }

        public void StartGame()
        {
            _game.StartGame();
        }

        public void InitGame(int symbols)
        {
            _game.Init(symbols);
            _game.DistributeFirstCard();
            foreach(var player in _game.GetPlayers())
            {
                Clients.Client(player.ConnectionId).start(_game.GetPlayerCurrentCard(player.Id).Lines.Select(l => l.Id));
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
            _game.AddCardToPlayer(id, new Card(symbols.Select(s => new Line(s)).ToList()));
            ExtractCard();
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

        public void ResetGame()
        {
            _game.ResetGame();
        }

        private IEnumerable<PlayerViewModel> GetPlayers()
        {
            return _game.GetPlayers().Select(p => new PlayerViewModel { displayName = p.DisplayName, imageUrl = p.ImageUrl, points = p.Cards.Count, id = p.Id });
        }
    }
}