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
        public Game(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _usedCards = new List<Point>();
        }

        public void Init(int symbolsNumber) {
            _gameInfo = _gameFactory.Create(symbolsNumber);
        }

        public Point GetFirstCard()
        {
            var cards = _gameInfo.Cards.ToArray();
            new Random().Shuffle(cards);
            var card = cards.First(c => !_usedCards.Contains(c));
            _usedCards.Add(card);
            var lines = card.Lines.ToArray();
            new Random().Shuffle(lines);
            return new Point(lines.ToList());
        }
    }
}