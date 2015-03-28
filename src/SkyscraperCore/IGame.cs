namespace SkyscraperCore
{
    public interface IGame
    {
        void Init(int symbolsNumber);

        Point DistributeFirstCard(string playerId);
        Point ExtractCard();
        void AddCardToPlayer(string playerId, Point card);
        Point GetPlayerCurrentCard(string playerId);
        GameStats GetGameStats();
        void StartGame();
    }
}