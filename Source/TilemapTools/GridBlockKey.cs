using System;

namespace TilemapTools
{
    internal struct GridBlockKey:IEquatable<GridBlockKey>
    {
        public short X { get; }
        public short Y { get; }

        public GridBlockKey(short x, short y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(GridBlockKey other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            int hash = (int)(X << 16);
            hash |= (int)Y;

            return hash;

        }

        public override bool Equals(object value)
        {
            if (!(value is GridBlockKey))
                return false;

            var strongValue = (GridBlockKey)value;
            return Equals(strongValue);
        }
    }
}