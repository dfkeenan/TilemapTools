using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core;

using Buffer = SiliconStudio.Xenko.Graphics.Buffer;

namespace TilemapTools.Xenko
{
    public class CachedQuadBatch : IDisposable
    {
        internal Buffer VertexBuffer;
        internal Buffer IndexBuffer;
        internal List<DrawRange> Ranges;
        internal int VertexSize;

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
