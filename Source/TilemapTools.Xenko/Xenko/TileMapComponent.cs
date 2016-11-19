using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// Add a Tilemap to an <see cref="Entity"/>.
    /// </summary>
    [DataContract("TileMapComponent")]
    [Display("Tile Map", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(TileMapProcessor))]
    [DefaultEntityComponentRenderer(typeof(TileMapRendererProcessor))]
    [ComponentOrder(20000)]
    public sealed class TileMapComponent: ActivableEntityComponent
    {
        /// <summary>
        /// The block size of the tile map.
        /// </summary>
        [DataMember(10)]
        [Display("Block Size")]
        [DefaultValue(GridBlock.DefaultBlockSize)]
        public int BlockSize = GridBlock.DefaultBlockSize;

        /// <summary>
        /// The size of tiles in world units.
        /// </summary>
        [DataMember(20)]
        [Display("Cell Size")]
        public Vector2 CellSize = new Vector2(1,1);

        /// <summary>
        /// The color to apply on the tile map.
        /// </summary>
        [DataMember(30)]
        [Display("Color")]
        public Color4 Color = Color4.White;
    }
}
