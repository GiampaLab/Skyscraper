using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            allSymbols.Add(line);
            symbols.Add(line);
            currentSymbolId++;
        }
        cards.Add(new Point(symbols));
        var totalNumberOfSymbols = GetTotalNumberOfSymbols(symbolsForEachCard);
        var geometryOrder = GetOrderOfTheGeometry(symbolsForEachCard);
        for (var i = 0; i < geometryOrder; i++)
        {
            cards.AddRange(CreateNewPoint(cards,symbolsForEachCard, i + 1, allSymbols));
        }
        return cards;
    }

    private IEnumerable<Point> CreateNewPoint(IList<Point> points, int numberOfLines, int iteration, IList<Line> allLines)
    {
        var lines = new List<Line>();
        var pointsToConnect = points.Count();
        var linesToAddCount = numberOfLines - pointsToConnect;
        var conjunctionLine = points.Last().Lines.Last();
        lines.Add(conjunctionLine);
        var lineId = conjunctionLine.LineId;
        for(var i = 1; i< numberOfLines; i++)
        {
            var newLine = new Line(++lineId);
            lines.Add(newLine);
            allLines.Add(newLine);
        }
        points.Add(new Point(lines));
        return CreateNewPoint(points, numberOfLines, iteration, allLines);
    }
}