using System.Collections.Generic;

namespace Skyscraper
{
    public class Line
    {
        public int Id { get; private set; }
        public IList<int> ConnectionOrders;  

        public Line(int id)
        {
            Id = id;
            ConnectionOrders = new List<int>();
        }
    }
}