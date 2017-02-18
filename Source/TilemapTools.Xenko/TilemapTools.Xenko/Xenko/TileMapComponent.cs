using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;
using TilemapTools.Xenko.Rendering;
using TilemapTools.Xenko.Physics;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// Add a Tilemap to an <see cref="Entity"/>.
    /// </summary>
    [DataContract("TileMapComponent")]
    [Display("Tile Map", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(TileMapProcessor))]
    [DefaultEntityComponentRenderer(typeof(TileMapRendererProcessor))]
    [ComponentOrder(50000)]
    public sealed class TileMapComponent: ActivableEntityComponent
    {      
        /// <summary>
        /// The color to apply on the tile map.
        /// </summary>
        [DataMember(10)]
        [Display("Color")]
        public Color4 Color = Color4.White;

        /// <summary>
        /// The grid of the tile map.
        /// </summary>
        [DataMember(20)]
        [Display("Grid")]
        [NotNull]
        [DataMemberCustomSerializer]
        public TileGrid Grid { get; set; } = new OrthogonalTileGrid();


        /// <summary>
        /// The physics shape builder of the tile map.
        /// </summary>
        [DataMember(30)]
        [Display("Physics Shape Builder")]
        [DataMemberCustomSerializer]
        public PhysicsShapeBuilder PhysicsShapeBuilder { get; set; }
    }
}
