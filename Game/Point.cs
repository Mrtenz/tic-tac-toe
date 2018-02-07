using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Point))
            {
                return false;
            }

            Point point = (Point) obj;
            return point.X == X && point.Y == Y;
        }

        public override int GetHashCode()
        {
            return (int) Math.Sqrt(Math.Pow(X, 2) * Math.Pow(Y, 2));
        }
    }
}
