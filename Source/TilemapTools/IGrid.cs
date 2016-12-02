using System;
using System.Collections.Generic;

namespace TilemapTools
{
    public interface IGrid<TCell, TCellSize> : IEnumerable<CellLocationPair<TCell>>
        where TCellSize : struct, IEquatable<TCellSize>
    {
        int BlockSize { get; set; }

        TCellSize CellSize { get; set; }

        TCell this[int x, int y] { get; set; }

    }
}