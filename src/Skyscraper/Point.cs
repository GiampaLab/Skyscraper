using System.Collections.Generic;

namespace Skyscraper
{
    public class Point
    {
        public IEnumerable<Line> Lines { get; private set; }

        public Point(IEnumerable<Line> lines)
        {
            Lines = lines;
        }
    }
}