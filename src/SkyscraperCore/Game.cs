using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyscraperCore
{
    public class Game : IGame
    {
        private IGameFactory _gameFactory;
        private GameInfo _gameInfo;
        private IList<Card> _cardsOnTable;
        private IList<Player> _players;
        public bool _gameStarted;

        public Game(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _players = new List<Player>();
            SetUp();
        }

        public void StartGame()
        {
            SetUp();
        }

        public void Init(int symbolsNumber) {
            _gameStarted = true;
            _gameInfo = _gameFactory.Create(symbolsNumber);
        }

        public void DistributeFirstCard()
        {
            foreach (var player in _players)
            {
                player.SetCurrentCard(GetRandomCard());
            }
        }

        public Card ExtractCard()
        {
            var card = GetRandomCard();
            _cardsOnTable.Add(card);
            return card;
        }

        public void AddCardToPlayer(string id, Card card)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            if (player == null)
                return;
            if (card == null)
                return;
            player.SetCurrentCard(card);
        }

        public Card GetPlayerCurrentCard(string id)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            if (player == null)
                return null;
            return player.Cards.Last();
        }

        public GameStats GetGameStats()
        {
            return new GameStats(_players);
        }

        private void SetUp()
        {
            foreach (var player in _players)
            {
                player.ResetCards();
            }
            _cardsOnTable = new List<Card>();
        }

        public void AddPlayer(string displayName, string imageUrl, string connectionId, string id)
        {
            if (_players.Any(p => p.Id == id))
                return;
            var player = new Player(displayName, imageUrl, connectionId, id);
            if (_gameStarted)
                player.SetCurrentCard(GetRandomCard());
            _players.Add(player);
        }

        public Card CurrentlyExtractedCard()
        {
            return _cardsOnTable.Last();
        }

        public IList<Player> GetPlayers()
        {
            return _players;
        }
        public bool GameStarted()
        {
            return _gameStarted;
        }

        private Card GetRandomCard()
        {
            var cards = _gameInfo.Cards.ToArray();
            new Random().Shuffle(cards);
            var card = cards.FirstOrDefault();
            if (card == null)
            {
                _gameStarted = false;
                return null;
            }
            _gameInfo.Cards = cards.Where(c => c != card).ToList();
            var lines = card.Lines.ToArray();
            new Random().Shuffle(lines);
            var randomizedCard = new Card(lines.ToList());
            return randomizedCard;
        }
    }
}