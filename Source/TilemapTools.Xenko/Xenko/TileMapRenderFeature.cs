using System;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    public class TileMapRenderFeature : RootRenderFeature
    {
        public override Type SupportedRenderObjectType => typeof(RenderTileMap);
    }
}