using System;

namespace LevelModel.Models.Components
{
    public struct Point
    {


        public int X { get; set; }

        public int Y { get; set; }

        public static readonly Point Empty = new Point(0, 0);


        public Point(int x, int y) {
            X = x;
            Y = y;
        }


        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

        public static Point operator *(Point p1, int value) => new Point(p1.X * value, p1.Y * value);

        public static Point operator *(int value, Point p1) => p1 * value;

        public static bool operator ==(Point p1, Point p2) => (p1.X == p2.X && p1.Y == p2.Y);

        public static bool operator !=(Point p1, Point p2) => !(p1 == p2);

        public override int GetHashCode() => (X + Y).GetHashCode();

        public override bool Equals(Object obj) {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            return this == ((Point)obj);
        }


    }
}
