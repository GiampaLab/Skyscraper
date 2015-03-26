using System;
using System.Collections.Generic;

namespace SkyscraperCore
{
    public class Player
    {
        public string PlayerId { get; private set; }
        public Point CurrentCard { get; private set; }

        private List<Point> cards;
        public Player(string playerId)
        {
            this.PlayerId = playerId;
            cards = new List<Point>();
        }

        public void SetCurrentCard(Point card)
        {
            cards.Add(card);
            CurrentCard = card;
        }
    }
}