using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyscraper
{
    public class Point : IEquatable<Point>
    {
        public IEnumerable<Line> Lines { get; private set; }
        public int Iteration { get; private set; }

        public Point(IEnumerable<Line> lines, int iteration)
        {
            Lines = lines;
            Iteration = iteration;
        }

        public bool Equals(Point other)
        {
            return Lines.All(l => other.Lines.Contains(l));
        }
    }
}