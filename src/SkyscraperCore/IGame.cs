namespace SkyscraperCore
{
    public interface IGame
    {
        void Init(int symbolsNumber);

        Point GetFirstCard();
    }
}