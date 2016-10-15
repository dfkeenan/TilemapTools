namespace TilemapTools.Tiled
{
    /// <summary>
    /// The encoding of the data in <see cref="Data.Content"/>. 
    /// </summary>
    public enum DataEncoding
    {
        /// <summary>
        /// Tiles encoded as xml each tile is &lt;tile&gt;&lt;/tile&gt;. Not supported.
        /// </summary>
        None,
        /// <summary>
        /// Data storesd as base64 encoded <see cref="string"/>.
        /// </summary>
        Base64,
        /// <summary>
        /// Data storesd as comma separated integers encoded <see cref="string"/>.
        /// </summary>
        CSV
    }
}