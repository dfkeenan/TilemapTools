using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools.Xenko
{
    public interface ITileDefinitionSource
    {
        Tile GetTile(ref TileReference reference);
    }
}
