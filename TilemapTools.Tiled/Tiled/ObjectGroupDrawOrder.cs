namespace TilemapTools.Tiled
{
    /// <summary>
    /// The draw order of <see cref="TiledObject"/>s in the <see cref="ObjectGroup"/>.
    /// </summary>
    public enum ObjectGroupDrawOrder
    {
        /// <summary>
        /// Unkown draw order
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Top down draw order. Sorted by there y-coordinate.
        /// </summary>
        TopDown,

        /// <summary>
        /// Index draw order. Sorted by order of appearance.
        /// </summary>
        Index
    }
}