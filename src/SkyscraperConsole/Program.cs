using SkyscraperCore;
using System;

namespace SkyscraperConsole
{
    public class Program
    {
        public void Main(string[] args)
        {
            var symbols = int.Parse(args[0]);
            var gameFactory = new GameFactory();
            var result = gameFactory.Create(symbols);
            Console.WriteLine("Total number of cards" + result.TotalNumberOfCards);
            Console.WriteLine("Total number of symbols" + result.TotalNumberOfSymbols);
        }
    }
}
