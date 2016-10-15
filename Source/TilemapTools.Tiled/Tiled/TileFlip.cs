namespace TilemapTools.Tiled
{
    /// <summary>
    /// The filipping of a tile.
    /// </summary>
    /// <remarks>
    /// When rendering a tile, the order of operation matters. 
    /// The diagonal flip (x/y axis swap) is done first, followed by the horizontal and vertical flips.
    /// </remarks>
    public enum TileFlip
    {
        /// <summary>
        /// Tile is not flipped.
        /// </summary>
        None = 0,

        /// <summary>
        /// Tile is flipped horizontally.
        /// </summary>
        Horizontally = 4,

        /// <summary>
        /// Tile is flipped vertically.
        /// </summary>
        Vertically = 2,

        /// <summary>
        /// Tile is flipped diagonally.
        /// </summary>
        Diagonally = 1
    }

    internal static class TileFlipExtensions
    {
        private const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        private const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        private const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        private const uint FLIIPED = FLIPPED_DIAGONALLY_FLAG | FLIPPED_HORIZONTALLY_FLAG | FLIPPED_DIAGONALLY_FLAG;

        public static void Convert(ref uint globalId, out TileFlip flip)
        {
            flip = (TileFlip)((globalId & FLIIPED) >> 29);

            globalId &= ~FLIIPED;
        }
    }
}