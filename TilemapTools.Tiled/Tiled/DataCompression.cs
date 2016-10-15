namespace TilemapTools.Tiled
{
    /// <summary>
    /// The compression of the data in <see cref="Data.Content"/>. 
    /// </summary>
    public enum DataCompression
    {
        /// <summary>
        /// The data is uncompressed.
        /// </summary>
        None,

        /// <summary>
        /// The data is compressed in gZip format.
        /// </summary>
        GZip,

        /// <summary>
        /// The data is compressed in zLib.
        /// </summary>
        zLib
    }
}