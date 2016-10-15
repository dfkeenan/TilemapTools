namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents the global id of the tile and its flip
    /// </summary>
    public struct LayerTile
    {
        public LayerTile(uint rawGlobalId)
        {
            TileFlip flip;
            TileFlipExtensions.Convert(ref rawGlobalId, out flip);
            GlobalId = rawGlobalId;
            Flip = flip;
        }

        /// <summary>
        /// The global id of the tile.
        /// </summary>
        public uint GlobalId;

        /// <summary>
        /// The flip of the tile.
        /// </summary>
        public TileFlip Flip;
    }
}