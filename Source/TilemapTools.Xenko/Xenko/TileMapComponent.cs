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
        public const int DefaultCellSize = 1;

        private Vector2 cellSize;

        public TileMapComponent()
        {
            cellSize = new Vector2(DefaultCellSize);
            Grid = new TileGrid();
        }

        /// <summary>
        /// The block size of the tile map.
        /// </summary>
        /// <remarks>The minimum blocksize is <see cref="GridBlock.MinimumBlockSize"/></remarks>
        [DataMember(10)]
        [Display("Block Size")]
        [DefaultValue(GridBlock.DefaultBlockSize)]
        public int BlockSize
        {
            get
            {
                return Grid.BlockSize;
            }

            set
            {
                Grid.BlockSize = Math.Max(GridBlock.MinimumBlockSize, value);
            }
        }

        /// <summary>
        /// The size of tiles in world units.
        /// </summary>
        /// <remarks>The minimum value is <see cref="MathUtil.ZeroTolerance"/></remarks>
        [DataMember(20)]
        [Display("Cell Size")]
        public Vector2 CellSize
        {
            get
            {
                return cellSize;
            }
            set
            {
                if (cellSize == value)
                    return;

                cellSize = value;
                cellSize.X = Math.Max(MathUtil.ZeroTolerance, cellSize.X);
                cellSize.Y = Math.Max(MathUtil.ZeroTolerance, cellSize.Y);
                OnCellSizeChanged();
            }
        }

        /// <summary>
        /// The color to apply on the tile map.
        /// </summary>
        [DataMember(30)]
        [Display("Color")]
        public Color4 Color = Color4.White;


        internal TileGrid Grid { get; set; }


        public Tile this[int x, int y]
        {
            get
            {
                return Grid[x, y];
            }

            set
            {
                Grid[x, y] = value;
            }
        }

        private void OnCellSizeChanged()
        {
            
        }
    }
}
