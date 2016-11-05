using System;
using System.Globalization;

namespace TilemapTools
{
    public struct ShortPoint : IEquatable<ShortPoint>
    {
        public ShortPoint(short x, short y)
        {
            X = x;
            Y = y;
        }

        public short X { get; }

        public short Y { get; }

        public bool Equals(ShortPoint other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ShortPoint))
                return false;

            var strongValue = (ShortPoint)obj;
            return Equals(strongValue);
        }

        public override int GetHashCode()
        {
            int hash = (int)(X << 16);
            hash |= (int)Y;
                        
            return hash;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X, Y);
        }
    }
}