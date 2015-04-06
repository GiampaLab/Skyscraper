namespace Skyscraper
{
    public interface ISymbolsProvider
    {
        Symbol GetSymbol(int id);
        void Init(string basePath, string iconsPath);
    }
}