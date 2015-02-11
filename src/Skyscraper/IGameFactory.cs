namespace Skyscraper
{
    public interface IGameFactory
    {
        GameInfo Create(int symbolsForEachCard);
    }
}