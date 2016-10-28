namespace TilemapTools.Tiled
{
    /// <summary>
    /// A group of <see cref="TiledObject"/>.
    /// </summary>
    public class ObjectGroup : LayerBase, ILayer
    {
        /// <summary>
        /// Gets or sets the draw order.
        /// </summary>
        public ObjectGroupDrawOrder DrawOrder { get; set; }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        public TiledElementList<TiledObject> Objects { get; set; }
    }
}