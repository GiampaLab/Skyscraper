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
        }

        public void StartGame()
        {
            SetUp();
        }

        public void Init(int symbolsNumber) {
            _gameInfo = _gameFactory.Create(symbolsNumber);
        }

        public Point DistributeFirstCard(string playerId)
        {
            var player = new Player(playerId);
            _players.Add(player);
            var card = GetRandomCard();
            player.SetCurrentCard(card);
            return card;
        }

        public Point ExtractCard()
        {
            return GetRandomCard();
        }

        public void AddCardToPlayer(string playerId, Point card)
        {
            var player = _players.FirstOrDefault(p => p.PlayerId == playerId);
            if (player == null)
                return;
            var cardToAdd = _usedCards.FirstOrDefault(c => c == card);
            if (cardToAdd == null)
                return;
            player.SetCurrentCard(cardToAdd);
        }

        public Point GetPlayerCurrentCard(string playerId)
        {
            var player = _players.FirstOrDefault(p => p.PlayerId == playerId);
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
            _players = new List<Player>();
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