namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a tilesets offset in pixels.
    /// </summary>
    public struct TileOffset
    {
        public TileOffset(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get or set the horizontal offset in pixels.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Get or set the Vertical offset in pixels (positive is down).
        /// </summary>
        public int Y { get; set; }
    }
}