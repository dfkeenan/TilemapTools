using System;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public interface ITileMeshDrawBuilder:IDisposable
    {
        TileMeshDraw Build(TileGridBlock block, ITileDefinitionSource tileDefinitionSource,GraphicsDevice graphicsDevice, ref Vector2 cellSize);
        void Clear();
    }
}