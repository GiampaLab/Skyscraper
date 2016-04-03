using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Skyscraper.Core
{
    public class GameFactory : IGameFactory
    {
        public GameInfo Create(int symbolsForEachCard)
        {
            var totalNumberOfSymbols = GetTotalNumberOfSymbols(symbolsForEachCard);
            var cards = GetCards2(symbolsForEachCard);
            return new GameInfo
            {
                SymbolsForEachCard = symbolsForEachCard,
                TotalNumberOfSymbols = totalNumberOfSymbols,
                TotalNumberOfCards = cards.Count(),
                Cards = cards
            };
        }

        private IEnumerable<Card> GetCards2(int symbolsForEachCard)
        {
            var cards = new List<Card>();
            var orderOfTheGeometry = symbolsForEachCard - 1;
            Card point = null;
            for (int i = 0; i< orderOfTheGeometry; i++)
            {
                point = new Card(new List<Line>());
                for (int j = 0; j < orderOfTheGeometry; j++)
                {
                    point.Lines.Add(new Line(i * orderOfTheGeometry + j));
                }
                point.Lines.Add(new Line(orderOfTheGeometry * orderOfTheGeometry + 1));
                cards.Add(point);
            }
            for (int i = 0; i < orderOfTheGeometry; i++)
            {
                for (int j = 0; j < orderOfTheGeometry; j++)
                {
                    point = new Card(new List<Line>());
                    for (int k = 0; k < orderOfTheGeometry; k++)
                    {
                        point.Lines.Add(new Line(k * orderOfTheGeometry + (j + i * k) % orderOfTheGeometry));
                    }
                    point.Lines.Add(new Line(orderOfTheGeometry * orderOfTheGeometry + 2 + i));
                    cards.Add(point);
                }
            }
            point = new Card(new List<Line>());
            for (int i = 0; i < orderOfTheGeometry + 1; i++)
                point.Lines.Add(new Line(orderOfTheGeometry * orderOfTheGeometry + 1 + i));
            cards.Add(point);
            return cards;
        }

        private IEnumerable<Card> GetCards(int symbolsForEachCard)
        {
            var cards = new List<Card>();
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
            cards.Add(new Card(symbols));
            for (int i = 1; i <= symbolsForEachCard; i++)
            {
                var currentLine = cards[0].Lines.First(l => l.Id == i);
                CreateNewPoint(cards, symbolsForEachCard, currentLine);
            }
            return cards;
        }

        private static IEnumerable<Card> CreateNewPoint(IList<Card> points, int numberOfLines, Line currentLine)
        {
            if (points.Count == currentLine.Id * (numberOfLines - 1) + 1)
                return points;

            var iteration = (points.Count - 1) % (numberOfLines - 1);
            var point = new Card(new List<Line>());
            var conjunctionLines = new List<Line> { currentLine };
            var addedPoints = points.Where(p => p.Lines.Contains(currentLine)).ToList();
            var pointsToConnect = points.Except(points.Where(p => p.Lines.Contains(currentLine)));
            var pointsToConnectCount = pointsToConnect.Count();

            var connectionsMaxOrderCount = (int)Math.Floor((decimal) (pointsToConnectCount / (numberOfLines - 1)));

            foreach (var pointToConnect in pointsToConnect)
            {
                var i = 0;
                while(conjunctionLines.Count < numberOfLines)
                {
                    var line = pointToConnect.Lines[i];
                    var addedLines = addedPoints.SelectMany(p => p.Lines).Distinct().ToList();
                    var pointsConnectedByLine = points.Where(p => p.Lines.Contains(line)).ToList();
                    var linesConnectedByLine = pointsConnectedByLine.SelectMany(p => p.Lines).Distinct().ToList();
                    var newLines = linesConnectedByLine.Where(l => !addedLines.Contains(l)).ToList();
                    addedLines.AddRange(linesConnectedByLine);
                    var guess = ((numberOfLines - conjunctionLines.Count - 1) * (numberOfLines - iteration) - numberOfLines + 1 + conjunctionLines.Count);
                    if (pointsConnectedByLine.Any(p => addedPoints.Contains(p)) || addedPoints.SelectMany(p => p.Lines).Contains(line) ||
                        pointsConnectedByLine.Count > numberOfLines -1 ||
                        (GetTotalNumberOfSymbols(numberOfLines) - addedLines.Distinct().ToList().Count < (numberOfLines - conjunctionLines.Count - 1) ))
                    {
                        i++;
                        continue;
                    }
                    var totalPoints = pointsConnectedByLine.Count + addedPoints.Count;
                    Debug.WriteLine("Total number of points: " + totalPoints);
                    Debug.WriteLine("Lines left guess: " + (GetTotalNumberOfSymbols(numberOfLines) - ((numberOfLines -1) + (totalPoints - 1)*numberOfLines)));
                    Debug.WriteLine("Lines Available: " + (GetTotalNumberOfSymbols(numberOfLines) - addedLines.Distinct().ToList().Count));
                    Debug.WriteLine("Connection order: " + connectionsMaxOrderCount);
                    Debug.WriteLine("Origin line: " + currentLine.Id);
                    Debug.WriteLine("Line Added: " + line.Id);
                    Debug.WriteLine("");
                    conjunctionLines.Add(line);
                    addedPoints.AddRange(pointsConnectedByLine);
                    break;
                }
            }

            ((List<Line>)point.Lines).AddRange(conjunctionLines);
            var lineId = points.SelectMany(p => p.Lines).Max(l => l.Id);
            var linesToAddCount = numberOfLines - conjunctionLines.Count;

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