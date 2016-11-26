using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;

namespace TilemapTools.Xenko
{
    //[DataSerializerGlobal(typeof(ReferenceSerializer<TileGrid>), Profile = "Content")]
    //[ContentSerializer(typeof(DataContentSerializer<TileGrid>))]
    [DataContract("TileGrid")]
    public abstract class TileGrid: Grid<Tile,Vector2>
    {
        public const int DefaultCellSize = 1;

        public TileGrid()
        {
            CellSize = new Vector2(DefaultCellSize);
        }

        /// <summary>
        /// The block size of the tile map.
        /// </summary>
        /// <remarks>The minimum blocksize is <see cref="GridBlock.MinimumBlockSize"/></remarks>
        [DataMember(10)]
        [Display("Block Size")]
        [DefaultValue(GridBlock.DefaultBlockSize)]
        public override int BlockSize
        {
            get
            {
                return base.BlockSize;
            }

            set
            {
                base.BlockSize = Math.Max(GridBlock.MinimumBlockSize, value);
            }
        }

        /// <summary>
        /// The size of tiles in world units.
        /// </summary>
        /// <remarks>The minimum value is <see cref="MathUtil.ZeroTolerance"/></remarks>
        [DataMember(20)]
        [Display("Cell Size")]
        public override Vector2 CellSize
        {
            get
            {
                return base.CellSize;
            }
            set
            {
                if (base.CellSize == value)
                    return;

                var cellSize = value;
                cellSize.X = Math.Max(MathUtil.ZeroTolerance, cellSize.X);
                cellSize.Y = Math.Max(MathUtil.ZeroTolerance, cellSize.Y);

                base.CellSize = cellSize;
            }
        }

        protected override IGridBlock<Tile, Vector2> CreateBlock(ShortPoint blockLocation)
        {
            throw new NotImplementedException();
        }

        public ISet<TileGridBlock> FindVisibleGridBlocks(BoundingFrustum localFrustum)
        {
            var result = new HashSet<TileGridBlock>();

            for (int i = 0; i < this.Blocks.Count; i++)
            {
                var block = Blocks[i] as TileGridBlock;

                if (block == null) continue;

                var bounds = block.LocalBounds;

                if (CollisionHelper.FrustumContainsBox(ref localFrustum, ref bounds))
                    result.Add(block);
            }
            return result;
        }

        public ISet<TileGridBlock> FindVisibleGridBlocks(Matrix world, Matrix viewProjection)
        {
            Matrix inverseWorld = world;

            Matrix.Invert(ref world, out inverseWorld);

            var matrix = inverseWorld * viewProjection;
            var frustum = new BoundingFrustum(ref matrix);

            return FindVisibleGridBlocks(frustum);
        }
    }
}
