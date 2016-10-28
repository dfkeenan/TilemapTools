namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a terrain.
    /// </summary>
    public class Terrain : ITiledElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the local tile-id of the tile that represents the terrain visually.
        /// </summary>
        public int Tile { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public PropertyDictionary Properties { get; set; }
    }
}