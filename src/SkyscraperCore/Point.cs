using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyscraperCore
{
    public class Point : IEquatable<Point>
    {
        public IList<Line> Lines { get; private set; }

        public Point(IList<Line> lines)
        {
            Lines = lines;
        }

        public bool Equals(Point other)
        {
            return Lines.All(l => other.Lines.Contains(l));
        }
    }
}