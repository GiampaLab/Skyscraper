using System.Collections.Generic;

namespace Skyscraper.Core
{
    public interface IGame
    {
        bool GameStarted();
        void Init(int symbolsNumber);
        void DistributeFirstCard();
        Card ExtractCard();
        void AddCardToPlayer(string playerId, Card card);
        Card GetPlayerCurrentCard(string playerId);
        GameStats GetGameStats();
        void AddPlayer(string displayName, string imageUrl, string connectionId, string id);
        IList<Player> GetPlayers();
        void UpdatePlayer(string displayName, string imageUrl, string connectionId, string id);
        Card CurrentlyExtractedCard();
        void ResetGame();
    }
}