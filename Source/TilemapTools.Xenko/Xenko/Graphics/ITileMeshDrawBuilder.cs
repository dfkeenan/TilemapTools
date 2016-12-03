using System;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public interface ITileMeshDrawBuilder:IDisposable
    {
        int IndiciesPerTile { get; }

        void Add(Texture texture, ref Rectangle source, ref RectangleF destination);
        TileMeshDraw Build(GraphicsDevice graphicsDevice);
        void Clear();
    }
}