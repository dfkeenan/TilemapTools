using System;

namespace TilemapTools
{
    public struct GridBlockKey : IEquatable<GridBlockKey>
    {
        public GridBlockKey(short x, short y)
        {
            X = x;
            Y = y;
        }

        public short X { get; }

        public short Y { get; }

        public bool Equals(GridBlockKey other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GridBlockKey))
                return false;

            var strongValue = (GridBlockKey)obj;
            return Equals(strongValue);
        }

        public override int GetHashCode()
        {
            int hash = (int)(X << 16);
            hash |= (int)Y;

            return hash;
        }
    }
}