using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko
{
    public abstract class TileGridBlock : GridBlock<Tile, Vector2>
    {
        public TileGridBlock(int blockSize, Vector2 cellSize, ShortPoint location, IEqualityComparer<Tile> cellEqualityComparer) : base(blockSize, cellSize, location, cellEqualityComparer)
        {
            CalculateBounds();
        }

        public BoundingBoxExt LocalBounds { get; private set; }

        protected override void OnCellContentChanged(int x, int y)
        {
            base.OnCellContentChanged(x, y);
        }

        protected override void OnCellSizeChanged()
        {
            CalculateBounds();
        }

        private void CalculateBounds()
        {
            var blockSize = BlockSize;
            var location = Location;

            int left, top, right, bottom;

            if (location.X < 0)
            {
                left = location.X * blockSize;
            }
            else
            {
                left = (location.X - 1) * blockSize;
            }
            right = left + blockSize;


            if (location.Y < 0)
            {
                top = (location.Y + 1) * blockSize;
            }
            else
            {
                top = location.Y * blockSize;
            }
            bottom = top - blockSize;

            var topLeft = new Vector3(left, top, 0);
            var bottomRight = new Vector3(right, bottom, 0);
            Vector3 min, max;

            Vector3.Min(ref topLeft, ref bottomRight, out min);
            Vector3.Max(ref topLeft, ref bottomRight, out max);

            LocalBounds = new BoundingBoxExt(min, max);
        }
    }
}
