using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Infrastructure;

public class Line
{
    public int LineId { get; private set; }
    public IList<int> ConnectionOrders;  

    public Line(int lineId)
    {
        LineId = lineId;
        ConnectionOrders = new List<int>();
    }
}