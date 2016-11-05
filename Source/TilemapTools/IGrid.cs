using System;

namespace TilemapTools
{
    public interface IGrid<TCell, TCellSize> : IDisposable
        where TCell : class
        where TCellSize : struct, IEquatable<TCellSize>
    {
        int BlockSize { get; set; }

        TCellSize CellSize { get; set; }

        TCell this[int x, int y] { get; set; }

    }
}