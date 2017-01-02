namespace Skyscraper.Core
{
    public interface IGameFactory
    {
        GameInfo Create(int symbolsForEachCard);
    }
}