using System;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public interface ITileMeshDrawBuilder:IDisposable
    {
        int IndiciesPerTile { get; }

        void Add(Texture texture, Rectangle source, RectangleF destination);
        TileMeshDraw Build(GraphicsDevice graphicsDevice);
        void Clear();
    }
}