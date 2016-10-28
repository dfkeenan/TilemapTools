using TilemapTools.Mathematics;

namespace TilemapTools.Tiled.Serialization
{
    public class TileInfo<TTExture>
    {
        public TileInfo(TTExture texture, Rectangle sourceRectangle, Tile tile, TileSet tileset)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Tile = tile;
            TileSet = tileset;
        }

        public TTExture Texture { get; }

        public Rectangle SourceRectangle { get; }

        public Tile Tile { get; }

        public TileSet TileSet { get; }
    }
}