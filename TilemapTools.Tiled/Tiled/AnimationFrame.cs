namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a tile animation frame
    /// </summary>
    public struct AnimationFrame
    {
        /// <summary>
        /// Creates a new <see cref="AnimationFrame"/>
        /// </summary>
        /// <param name="id">The local tile id with in the parent.</param>
        /// <param name="duration">The duration of the frame in milliseconds.</param>
        public AnimationFrame(int id, int duration)
        {
            Id = id;
            Duration = duration;
        }

        /// <summary>
        /// Gets or sets the local tile id with in the parent.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the duration of the frame in milliseconds.
        /// </summary>
        public int Duration { get; set; }
    }
}