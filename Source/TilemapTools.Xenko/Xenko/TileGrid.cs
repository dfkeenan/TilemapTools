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
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    //[DataSerializerGlobal(typeof(ReferenceSerializer<TileGrid>), Profile = "Content")]
    //[ContentSerializer(typeof(DataContentSerializer<TileGrid>))]
    [DataContract("TileGrid")]
    public abstract class TileGrid: Grid<TileReference, Vector2, TileGridBlock>, ITileDefinitionSource    
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

        /// <summary>
        /// The tile sets used by this tile grid.
        /// </summary>
        [DataMember(30)]
        [Display("Tile Sets")]
        public IList<TileSet> TileSets { get; } = new List<TileSet>();


        Tile ITileDefinitionSource.GetTile(ref TileReference reference)
        {
            if (reference.TileSet >= TileSets.Count)
                return null;

            var tileSet = TileSets[reference.TileSet];

            if (reference.Tile >= tileSet.Tiles.Count)
                return null;

            return tileSet.Tiles[reference.Tile];
        }

        protected override TileGridBlock CreateBlock(ShortPoint blockLocation)
        {
            var block = new TileGridBlock(BlockSize, blockLocation, CellEqualityComparer);

            CalculateBounds(ref blockLocation, out block.LocalBounds, out block.Origin);

            return block;
                            
        }
        protected override void OnCellSizeChanged()
        {
            for (int i = 0; i < this.Blocks.Count; i++)
            {
                var block = Blocks[i];
                var location = block.Location;
                CalculateBounds(ref location, out block.LocalBounds, out block.Origin);               
            }
        }

        public abstract ITileMeshDrawBuilder CreateMeshDrawBuilder();

        public void FindVisibleGridBlocks(ref BoundingFrustum localFrustum, IList<TileGridBlock> result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            for (int i = 0; i < this.Blocks.Count; i++)
            {
                var block = Blocks[i];

                var bounds = block.LocalBounds;

                if (CollisionHelper.FrustumContainsBox(ref localFrustum, ref bounds))
                    result.Add(block);
            }
        }

        public void FindVisibleGridBlocks(ref Matrix world, ref Matrix viewProjection, IList<TileGridBlock> result)
        {
            Matrix inverseWorld;

            Matrix.Invert(ref world, out inverseWorld);

            var matrix = inverseWorld * viewProjection;
            var frustum = new BoundingFrustum(ref matrix);

            FindVisibleGridBlocks(ref frustum, result);
        }

        protected virtual void CalculateBounds(ref ShortPoint location, out BoundingBoxExt bounds, out Vector2 origin)
        {        
            var blockSize = BlockSize;
            int left, top, right, bottom;

            GridBlock.CalculateEdges(ref blockSize, ref location, out left, out top, out right, out bottom);

            var size = CellSize;
            var topLeft = new Vector3(left * size.X, top * size.Y, 0);
            var bottomRight = new Vector3(right * size.X, bottom * size.Y, 0);
            Vector3 min, max;

            Vector3.Min(ref topLeft, ref bottomRight, out min);
            Vector3.Max(ref topLeft, ref bottomRight, out max);

            bounds = new BoundingBoxExt(min, max);
            origin = topLeft.XY();

        }
   
    }
}
