using System;
using System.Collections.Generic;

namespace SkyscraperCore
{
    public class Line : IEquatable<Line>
    {
        public int Id { get; private set; }

        public Line(int id)
        {
            Id = id;
        }

        public bool Equals(Line other)
        {
            return Id == other.Id;
        }
    }
}