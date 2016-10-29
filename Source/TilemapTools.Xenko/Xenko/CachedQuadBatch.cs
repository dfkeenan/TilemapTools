using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core;

namespace TilemapTools.Xenko
{
    public class CachedQuadBatch<TVertex> : IDisposable
        where TVertex : struct, IVertex
    {
        internal Buffer<TVertex> VertexBuffer;
        internal Buffer<short> IndexBuffer;
        internal List<DrawRange> Ranges;

        internal CachedQuadBatch()
        {

        }

        public void Dispose()
        {
            Utilities.Dispose(ref VertexBuffer);
            Utilities.Dispose(ref IndexBuffer);
            Ranges?.Clear();
        }

        internal class DrawRange
        {
            public Texture Texture;
            public int StartIndex;
            public int IndexCount;
        }
    }
}
