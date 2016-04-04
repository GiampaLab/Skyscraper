namespace Skyscraper.Web
{
    public interface ISymbolsProvider
    {
        Symbol GetSymbol(int id);
        void Init(string iconsPath);
    }
}