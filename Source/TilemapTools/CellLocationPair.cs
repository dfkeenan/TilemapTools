using System.Diagnostics;

namespace TilemapTools
{
    [DebuggerDisplay("Content = {Content}; X = {X}; Y = {Y}")]
    public struct CellLocationPair<TCell>
    {
        public CellLocationPair(TCell content, int x, int y) : this()
        {
            Content = content;
            X = x;
            Y = y;
        }

        public TCell Content { get; }

        public int X { get; }

        public int Y { get; }
    }
}