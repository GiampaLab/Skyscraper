using System.Collections.Generic;

public class Point
{
    public IEnumerable<Line> Lines { get; private set; }

    public Point(IEnumerable<Line> lines)
    {
        Lines = lines;
    }
}