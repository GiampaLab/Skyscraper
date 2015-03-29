using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyscraperCore
{
    public class Game : IGame
    {
        private IGameFactory _gameFactory;
        private GameInfo _gameInfo;
        private IList<Point> _usedCards;
        private IList<Player> _players; 
        public Game(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            SetUp();
            _players = new List<Player>();
        }

        public void StartGame()
        {
            SetUp();
        }

        public void Init(int symbolsNumber) {
            _gameInfo = _gameFactory.Create(symbolsNumber);
        }

        public void DistributeFirstCard()
        {
            foreach (var player in _players)
            {
                player.SetCurrentCard(GetRandomCard());
            }
        }

        public Point ExtractCard()
        {
            return GetRandomCard();
        }

        public void AddCardToPlayer(string connectionId, Point card)
        {
            var player = _players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null)
                return;
            if (card == null)
                return;
            player.SetCurrentCard(card);
        }

        public Point GetPlayerCurrentCard(string connectionId)
        {
            var player = _players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null)
                return null;
            return player.CurrentCard;
        }

        public GameStats GetGameStats()
        {
            return new GameStats(_players);
        }

        private void SetUp()
        {
            _usedCards = new List<Point>();
        }

        public void AddPlayer(string displayName, string imageUrl, string connectionId, string id)
        {
            if (_players.Any(p => p.Id == id))
                return;
            var player = new Player(displayName, imageUrl, connectionId, id);
            _players.Add(player);
        }
        public IList<Player> GetPlayers()
        {
            return _players;
        }

        private Point GetRandomCard()
        {
            var cards = _gameInfo.Cards.ToArray();
            new Random().Shuffle(cards);
            var card = cards.FirstOrDefault(c => !_usedCards.Contains(c));
            if (card == null)
                return null;
            _usedCards.Add(card);
            var lines = card.Lines.ToArray();
            new Random().Shuffle(lines);
            var randomizedCard = new Point(lines.ToList());
            return randomizedCard;
        }
    }
}