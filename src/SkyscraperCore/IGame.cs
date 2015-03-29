using System.Collections.Generic;

namespace SkyscraperCore
{
    public interface IGame
    {
        void Init(int symbolsNumber);
        void DistributeFirstCard();
        Point ExtractCard();
        void AddCardToPlayer(string playerId, Point card);
        Point GetPlayerCurrentCard(string playerId);
        GameStats GetGameStats();
        void StartGame();
        void AddPlayer(string displayName, string imageUrl, string connectionId, string id);
        IList<Player> GetPlayers();
    }
}