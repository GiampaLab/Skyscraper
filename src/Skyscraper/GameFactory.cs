using System.Collections.Generic;
using System.Linq;

namespace Skyscraper
{
    public class GameFactory : IGameFactory
    {
        public GameInfo Create(int symbolsForEachCard)
        {
            var totalNumberOfSymbols = GetTotalNumberOfSymbols(symbolsForEachCard);

            return new GameInfo
            {
                SymbolsForEachCard = symbolsForEachCard,
                TotalNumberOfSymbols = totalNumberOfSymbols,
                TotalNumberOfCards = totalNumberOfSymbols,
                Cards = GetCards(symbolsForEachCard)
            };
        }

        private static int GetOrderOfTheGeometry(int symbolsForEachCard)
        {
            return symbolsForEachCard - 1;
        }

        private static int GetTotalNumberOfSymbols(int symbolsForEachCard)
        {
            return (symbolsForEachCard * GetOrderOfTheGeometry(symbolsForEachCard)) + 1;
        }

        private IEnumerable<Point> GetCards(int symbolsForEachCard)
        {
            var cards = new List<Point>();
            var symbols = new List<Line>();
            var allSymbols = new List<Line>();
            var currentSymbolId = 1;
            for (var i = 0; i < symbolsForEachCard; i++)
            {
                var line = new Line(currentSymbolId);
                line.ConnectionOrders.Add(0);
                allSymbols.Add(line);
                symbols.Add(line);
                currentSymbolId++;
            }
            cards.Add(new Point(symbols));
            var geometryOrder = GetOrderOfTheGeometry(symbolsForEachCard);
            for (var i = 0; i < geometryOrder; i++)
            {
                CreateNewPoint(cards,symbolsForEachCard, i + 1, allSymbols);
            }
            return cards;
        }

        private static IEnumerable<Point> CreateNewPoint(IList<Point> points, int numberOfLines, int iteration, IList<Line> allLines)
        {
            var pointsToConnect = points.Count();
            var linesToAddCount = numberOfLines * iteration - pointsToConnect;
            

            var lines = new List<Line>();
            var conjunctionLine = new List<Line>();
            foreach (var line in points.Select(point => point.Lines.FirstOrDefault(l => !l.ConnectionOrders.Contains(iteration))))
            {
                if (line != null)
                {
                    conjunctionLine.Add(line);
                    line.ConnectionOrders.Add(iteration);
                }
            }
            lines.AddRange(conjunctionLine);
            var lineId = allLines.Max(l => l.Id);
            for(var i = 0; i< linesToAddCount; i++)
            {
                var newLine = new Line(++lineId);
                newLine.ConnectionOrders.Add(iteration -1);
                lines.Add(newLine);
                allLines.Add(newLine);
            }
            points.Add(new Point(lines));
            return linesToAddCount == 0 ? points : CreateNewPoint(points, numberOfLines, iteration, allLines);
        }
    }
}