﻿using System.Collections.Generic;

namespace Skyscraper
{
    public class GameInfo
    {
        public int SymbolsForEachCard { get; set; }
        public int TotalNumberOfSymbols { get; set; }
        public int TotalNumberOfCards { get; set; }
        public IEnumerable<Point> Cards { get; set; }
    }
}