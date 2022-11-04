using System.Collections;
using System.Collections.Generic;

public struct Location
{
    public int X;
    public int Y;

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Location((int, int) xy)
    {
        X = xy.Item1;
        Y = xy.Item2;
    }

    public override bool Equals(object obj)
    {
        if (obj is Location)
        {
            Location location = (Location)obj;
            return location.X == X && location.Y == Y;
        }
        else
        {
            if (obj is (int, int))
            {
                Location location = (Location)obj;
                return (location.X == X && location.Y == Y);
            }
            return false;
        }
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }

    public static Location operator +(Location a) => a;
    public static Location operator -(Location a) => new Location(-a.X, -a.Y);
    public static Location operator +(Location a, Location b) => new Location(a.X + b.X, a.Y + b.Y);
    public static Location operator -(Location a, Location b) => new Location(a.X - b.X, a.Y - b.Y);
    public static Location operator *(Location a, Location b) => new Location(a.X * b.X, a.Y * b.Y);
    public static Location operator *(Location a, int b) => new Location(a.X * b, a.Y * b);
    public static implicit operator Location((int, int) t) => new Location(t.Item1, t.Item2);
}
