namespace TilemapTools.Tiled
{
    /// <summary>
    /// An abstract base class for <see cref="ILayer"/> implementations. 
    /// </summary>
    public abstract class LayerBase : ILayer
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the X offset. Rendering offset for this layer in pixels.
        /// </summary>
        public double OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y offset. Rendering offset for this layer in pixels.
        /// </summary>
        public double OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        public PropertyDictionary Properties { get; set; }
    }
}