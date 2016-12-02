using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using TilemapTools.Xenko;
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    [Display("Orthogonal")]
    [DataContract("OrthogonalTileGrid")]
    public class OrthogonalTileGrid : TileGrid
    {
        public override ITileMeshDrawBuilder CreateMeshDrawBuilder()
        {
            return new OrthogonalTileMeshDrawBuilder();
        }
    }
}
