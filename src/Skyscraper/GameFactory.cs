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
            var cards = GetCards(symbolsForEachCard);
            return new GameInfo
            {
                SymbolsForEachCard = symbolsForEachCard,
                TotalNumberOfSymbols = totalNumberOfSymbols,
                TotalNumberOfCards = cards.Count(),
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
                allSymbols.Add(line);
                symbols.Add(line);
                currentSymbolId++;
            }
            cards.Add(new Point(symbols));
            for (int i = 1; i <= symbolsForEachCard; i++)
            {
                var currentLine = cards[0].Lines.First(l => l.Id == i);
                
                CreateNewPoint(cards, symbolsForEachCard, currentLine);
            }
            return cards;
        }

        private static IEnumerable<Point> CreateNewPoint(IList<Point> points, int numberOfLines, Line currentLine)
        {
            if (points.Count == currentLine.Id * (numberOfLines - 1) + 1)
                return points;

            var point = new Point(new List<Line> { currentLine });
            var conjunctionLines = new List<Line>();
            var addedPoints = points.Where(p => p.Lines.Contains(currentLine)).ToList();
            var pointsToConnect = points.Except(points.Where(p => p.Lines.Contains(currentLine)));
            var pointsToConnectCount = pointsToConnect.Count();
            //var index = (points.Count - 1)%(numberOfLines - 1);
            //var connectionsMaxOrderCount = (int)Math.Floor((decimal) (pointsToConnectCount / (numberOfLines - 1)));

            foreach (var pointToConnect in pointsToConnect)
            {
                foreach (var line in pointToConnect.Lines)
                {
                    var addedLines = addedPoints.SelectMany(p => p.Lines).Distinct().ToList();
                    var pointsConnectedByLine = points.Where(p => p.Lines.Contains(line)).ToList();
                    var linesConnectedByLine = pointsConnectedByLine.SelectMany(p => p.Lines).Distinct().ToList();
                    addedLines.AddRange(linesConnectedByLine);
                    if (pointsConnectedByLine.Any(p => addedPoints.Contains(p)) || addedPoints.SelectMany(p => p.Lines).Contains(line) ||
                        (conjunctionLines.Count < numberOfLines - 2 && addedLines.Distinct().ToList().Count == GetTotalNumberOfSymbols(numberOfLines)))
                        continue;
                    conjunctionLines.Add(line);
                    addedPoints.AddRange(pointsConnectedByLine);
                    break;
                }
            }

            ((List<Line>)point.Lines).AddRange(conjunctionLines);
            var lineId = points.SelectMany(p => p.Lines).Max(l => l.Id);
            var linesToAddCount = (numberOfLines - 1) - conjunctionLines.Count;

            for (var i = 0; i< linesToAddCount; i++)
            {
                var newLine = new Line(++lineId);
                point.Lines.Add(newLine);
            }
            points.Add(point);
            return CreateNewPoint(points, numberOfLines, currentLine);
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