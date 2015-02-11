using System;
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

        private IEnumerable<Point> GetCards(int symbolsForEachCard)
        {
            var cards = new List<Point>();
            var symbols = new List<Line>();
            var allSymbols = new List<Line>();
            var currentSymbolId = 1;
            for (var i = 0; i < symbolsForEachCard; i++)
            {
                var line = new Line(currentSymbolId);
                //line.ConnectionOrders.Add(0);
                allSymbols.Add(line);
                symbols.Add(line);
                currentSymbolId++;
            }
            cards.Add(new Point(symbols, 0));
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
            var connectionsMaxOrder = Math.Floor((decimal)pointsToConnect/iteration);
            var connectionsMinOrder = pointsToConnect % iteration;
            var linesToAddCount = numberOfLines - connectionsMaxOrder - connectionsMinOrder;
            if (linesToAddCount < 0)
                return points;

            var lines = new List<Line>();
            var conjunctionLine = new List<Line>();
            var addedPoints = new List<Point>();
            foreach (var pointToConnect in points.Where(point => !addedPoints.Contains(point) && point.Lines.Any(l => l.ConnectionOrders.Count == (iteration - 1))))
            {
                var line = pointToConnect.Lines.Last(l => l.ConnectionOrders.Count == (iteration - 1));
                conjunctionLine.Add(line);
                line.ConnectionOrders.Add(iteration);
                addedPoints.AddRange(points.Where(p => p.Lines.Contains(line)));
                if (conjunctionLine.Count == connectionsMaxOrder)
                    break;
            }
            foreach (var pointToConnect in points.Where(point => !addedPoints.Contains(point) && point.Lines.Any(l => l.ConnectionOrders.Count == (iteration - 2))))
            {
                var line = pointToConnect.Lines.Last(l => l.ConnectionOrders.Count == (iteration - 2));
                conjunctionLine.Add(line);
                line.ConnectionOrders.Add(iteration);
                addedPoints.AddRange(points.Where(p => p.Lines.Contains(line)));
                if (conjunctionLine.Count == connectionsMaxOrder)
                    break;
            }
            lines.AddRange(conjunctionLine);
            var lineId = allLines.Max(l => l.Id);
            for(var i = 0; i< linesToAddCount; i++)
            {
                var newLine = new Line(++lineId);
                lines.Add(newLine);
                allLines.Add(newLine);
            }
            points.Add(new Point(lines, iteration));
            return CreateNewPoint(points, numberOfLines, iteration, allLines);
        }
        private static int GetOrderOfTheGeometry(int symbolsForEachCard)
        {
            return symbolsForEachCard - 1;
        }

        private static int GetTotalNumberOfSymbols(int symbolsForEachCard)
        {
            return (symbolsForEachCard * GetOrderOfTheGeometry(symbolsForEachCard)) + 1;
        }
    }
}