using TilemapTools.Mathematics;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a Tiled map stored in the TMX (Tile Map XML) format. 
    /// </summary>
    public class TileMap : TiledDocument
    {
        /// <summary>
        /// Gets or sets the TMX format version, generally 1.0.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the map width in tiles.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the map height in tiles
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of a tile.
        /// </summary>
        /// <remarks>
        /// The <see cref="TileWidth"/> and <see cref="TileHeight"/> properties determine the general grid size of the map. 
        /// The individual tiles may have different sizes. Larger tiles will extend at the top
        /// and right (anchored to the bottom left).
        /// </remarks>
        public int TileWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the tile.
        /// </summary>
        /// <remarks>
        /// The <see cref="TileWidth"/> and <see cref="TileHeight"/> properties determine the general grid size of the map. 
        /// The individual tiles may have different sizes. Larger tiles will extend at the top
        /// and right (anchored to the bottom left).
        /// </remarks>
        public int TileHeight { get; set; }

        /// <summary>
        /// Gets or sets the hexangle side length of the tile's edge, in pixels. Only for hexagonal maps.
        /// </summary>
        /// <remarks>
        /// Determines the width or height (depending on the <see cref="StaggerAxis"/>) of the tile's edge.
        /// </remarks>
        public int? HexSideLength { get; set; }

        /// <summary>
        /// Gets or sets the map orientation.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the stagger axis. Only for staggered and hexagonal maps.
        /// </summary>
        public StaggerAxis StaggerAxis { get; set; }

        /// <summary>
        /// Gets or sets the stagger index. Only for staggered and hexagonal maps.
        /// </summary>
        /// <remarks>
        /// For staggered and hexagonal maps, determines whether the "even" or "odd" indexes along the staggered axis are shifted.
        /// </remarks>
        public StaggerIndex StaggerIndex { get; set; }

        /// <summary>
        /// Gets or sets the render order. The order in which tiles are rendered.
        /// </summary>
        public RenderOrder RenderOrder { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the next avaliable ID for new objects.
        /// </summary>
        /// <remarks>
        /// This number is stored to prevent reuse of the same ID after objects have been removed.
        /// </remarks>
        public int? NextObjectID { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public PropertyDictionary Properties { get; set; }

        /// <summary>
        /// Gets or sets the tilesets
        /// </summary>
        public TiledElementList<TileSet> TileSets { get; set; }

        /// <summary>
        /// Gets or sets the layers. This includes <see cref="Layer"/>, <see cref="ObjectGroup"/>  and <see cref="ImageLayer"/>
        /// in file order.
        /// </summary>
        public TiledElementList<ILayer> Layers { get; set; }
    }
}