using System;
using System.Collections.Generic;

namespace TilemapTools
{
    public interface IGridBlock : IDisposable
    {
        int BlockSize { get; }

        int CellCount { get; }

        bool IsEmpty { get; }

        ShortPoint Location { get; }
    }

    public interface IGridBlock<TCell, TCellSize> : IGridBlock, IEnumerable<CellLocationPair<TCell>>
        where TCellSize : struct, IEquatable<TCellSize>
    {
        TCell this[int x, int y] { get; set; }
    }
}