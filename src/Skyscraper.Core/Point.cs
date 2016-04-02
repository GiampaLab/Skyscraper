using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyscraperCore
{
    public class Card : IEquatable<Card>
    {
        public IList<Line> Lines { get; private set; }

        public Card(IList<Line> lines)
        {
            Lines = lines;
        }

        public bool Equals(Card other)
        {
            return Lines.All(l => other.Lines.Contains(l));
        }
    }
}