using System.Collections.ObjectModel;
using System.Linq;
using TilemapTools.Mathematics;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a Tiled tileset stored in the TSX (Tile Set XML) format. 
    /// </summary>
    public class TileSet : TiledDocument, ITiledElement
    {
        /// <summary>
        /// Gets or sets the first global id. The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
        /// </summary>
        public int FirstGlobalId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the (maximum) width of the tiles in this tileset. 
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// Gets or sets the (maximum) height of the tiles in this tileset.
        /// </summary>
        public int TileHeight { get; set; }


        public Tile GetTile(uint globalId)
        {
            var localId = globalId - FirstGlobalId;

            return Tiles?.FirstOrDefault(t => t.Id == localId);
        }

        public Rectangle CalculateSourceRectangle(uint globalId)
        {
            var localId = globalId - FirstGlobalId;

            var columns = Columns.GetValueOrDefault(1);
            var x = localId % columns;
            var y = localId / columns;

            var left = x * (TileWidth + Spacing) + Margin;
            var top = y * (TileHeight + Spacing) + Margin;

            return new Rectangle((int)left, (int)top, TileWidth, TileHeight);
        }

        /// <summary>
        /// Gets or sets the spacing in pixels between the tiles in this tileset (applies to the tileset <see cref="TileSet.Image"/>).
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Gets or sets the margin around the tiles in this tileset (applies to the tileset image)..
        /// </summary>
        public int Margin { get; set; }

        /// <summary>
        /// Gets or sets the number of tile columns in the tileset. For image collection tilesets it is editable and is used when displaying the tileset..
        /// </summary>
        public int? Columns { get; set; }

        /// <summary>
        /// Gets or sets the number of tiles in this tileset.
        /// </summary>
        public int? TileCount { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public PropertyDictionary Properties { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        public Collection<Tile> Tiles { get; set; }

        /// <summary>
        /// Gets or sets the tile offset in pixels. To be applied when drawing a tile from the related tileset.
        /// </summary>
        public TileOffset TileOffset { get; set; }

        /// <summary>
        /// Gets or sets the image for this tileset.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the terrains for this tileset.
        /// </summary>
        public TiledElementList<Terrain> Terrains { get; set; }
    }
}