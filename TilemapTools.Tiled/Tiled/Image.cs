namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents an image.
    /// </summary>
    public class Image : TiledDocument
    {
        /// <summary>
        /// Gets or sets the format. Used for embedded images, in combination with the <see cref="Image.Data"/> . Valid values are file extensions like png, gif, jpg, bmp, etc.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the transparent color. Defines a specific color that is treated as transparent (example value: "#FF00FF" for magenta).
        /// </summary>
        public Color? TransparentColor { get; set; }

        /// <summary>
        /// Gets or sets the image width in pixels (optional, used for tile index correction when the image changes).
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the image height in pixels (optional).
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the data. Used for embedded images.
        /// </summary>
        public Data Data { get; internal set; }
    }
}