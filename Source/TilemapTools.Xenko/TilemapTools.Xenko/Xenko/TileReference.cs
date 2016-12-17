using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools.Xenko
{
    [Flags]
    public enum TileDisplay: byte
    {
        None,
        Visible = 1,
        FlipHorizontal = 2,
        FlipVertical = 4,
    }

    [DebuggerDisplay("Display: {Display}; TileSet: {TileSet}; Tile: {Tile}")]
    [StructLayout(LayoutKind.Sequential)]
    public struct TileReference:IEquatable<TileReference>
    {
        public TileDisplay Display;

        public byte TileSet;

        public short Tile;

        public TileReference(byte tileSet, short tile, TileDisplay display = TileDisplay.Visible)
        {
            Display = display;
            TileSet = tileSet;
            Tile = tile;
        }

        public bool IsEmpty => Display == TileDisplay.None;

        public bool Equals(TileReference other)
        {
            return this.Display == other.Display && this.TileSet == other.TileSet && this.Tile == other.Tile;
        }

        public override int GetHashCode()
        {
            return (int)this;
        }

        public override bool Equals(object obj)
        {
            if(obj is TileReference)
            {
                return this.Equals((TileReference)obj);
            }

            return false;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="TileReference"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(TileReference value)
        {
            int result = (int)value.Display << 24;
            result |= value.TileSet << 16;
            result |= value.Tile;

            return result;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="int"/> to <see cref="TileReference"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator TileReference(int value)
        {
            TileReference result = default(TileReference);
            result.Display = (TileDisplay)(value >> 24);
            result.TileSet = (byte)(value >> 16);
            result.Tile = (short)value;
            return result;
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(TileReference left, TileReference right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(TileReference left, TileReference right)
        {
            return !left.Equals(right);
        }

    }
}
