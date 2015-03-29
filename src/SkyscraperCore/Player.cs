using System;
using System.Collections.Generic;

namespace SkyscraperCore
{
    public class Player
    {
        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public Point CurrentCard { get; private set; }
        public List<Point> Cards { get; private set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }

        public Player(string diaplyName, string imageUrl, string connectionId, string id)
        {
            Id = id;
            ConnectionId = connectionId;
            DisplayName = diaplyName;
            ImageUrl = imageUrl;
            Cards = new List<Point>();
        }

        public void SetCurrentCard(Point card)
        {
            Cards.Add(card);
            CurrentCard = card;
        }
    }
}