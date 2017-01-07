using SiliconStudio.Core;
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
