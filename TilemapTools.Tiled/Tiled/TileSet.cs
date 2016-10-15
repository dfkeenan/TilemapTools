using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    public class TileSet : TiledDocument, ITiledElement
    {
        public int FirstGlobalId { get; set; }

        public string Name { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public int Spacing { get; set; }

        public int Margin { get; set; }

        public int? Columns { get; set; }

        public int? TileCount { get; set; }

        public PropertyDictionary Properties { get; set; }

        public Collection<Tile> Tiles { get; set; }

        public TileOffset TileOffset { get; set; }

        public Image Image { get; set; }

        public TiledElementList<Terrain> Terrains { get; set; }
    }
}