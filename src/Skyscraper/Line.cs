using System;
using System.Collections.Generic;

namespace Skyscraper
{
    public class Line : IEquatable<Line>
    {
        public int Id { get; private set; }
        public IList<int> ConnectionOrders;  

        public Line(int id)
        {
            Id = id;
            ConnectionOrders = new List<int>();
        }

        public bool Equals(Line other)
        {
            return Id == other.Id;
        }
    }
}