using System;
using System.Collections.Generic;

namespace Skyscraper.Core
{
    public class Player
    {
        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public List<Card> Cards { get; private set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }

        public Player(string diaplyName, string imageUrl, string connectionId, string id)
        {
            Id = id;
            ConnectionId = connectionId;
            DisplayName = diaplyName;
            ImageUrl = imageUrl;
            Cards = new List<Card>();
        }

        public void SetCurrentCard(Card card)
        {
            Cards.Add(card);
        }

        public void ResetCards()
        {
            Cards = new List<Card>();
        }
    }
}