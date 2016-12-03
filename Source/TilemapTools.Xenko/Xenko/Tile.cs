using System;
using System.Collections.Generic;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// A tile.
    /// </summary>
    [DataContract]
    [ContentSerializer(typeof(DataContentSerializer<Tile>))]
    [DataSerializerGlobal(typeof(ReferenceSerializer<Tile>), Profile = "Content")]
    public class Tile
    {
        /// <summary>
        /// Create an instance of <see cref="Tile"/> with a unique random name.
        /// </summary>
        public Tile()
            : this(Guid.NewGuid().ToString(), null)
        {
        }

        /// <summary>
        /// Creates an empty <see cref="Tile"/> having the provided name.
        /// </summary>
        /// <param name="tileName">Name of the tile.</param>
        public Tile(string tileName)
            :this(tileName, null)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="Tile"/> from the provided <see cref="Texture"/>.
        /// A unique Id is set as name.
        /// </summary>
        /// <param name="texture">The texture to use as texture.</param>
        /// <param name="region">The region of the texture to use. Defaults with size of the whole.</param>
        public Tile(Texture texture, Rectangle? region = null)
            : this(Guid.NewGuid().ToString(), texture, region)
        {
        }


        /// <summary>
        /// Creates a <see cref="Tile"/> having the provided texture, name and optionally region.
        /// </summary>
        /// <param name="tileName">The name of the tile.</param>
        /// <param name="texture">The texture to use as texture.</param>
        /// <param name="region">The region of the texture to use. Defaults with size of the whole.</param>
        public Tile(string tileName, Texture texture, Rectangle? region = null)
        {
            Name = tileName;

            if (texture != null)
            {
                Frames.Add(new TileFrame
                {
                    TextureRegion = region.HasValue ? region.Value : new Rectangle(0, 0, texture.ViewWidth, texture.ViewHeight),
                });
            }
        }

        /// <summary>
        /// Gets or sets the name of the tile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of frames.
        /// </summary>
        [MemberCollection(NotNullItems = true)]
        public List<TileFrame> Frames { get; } = new List<TileFrame>();

        /// <summary>
        /// Gets the value indicating if the tile is animated (has more than 1 frame).
        /// </summary>
        public bool IsAnimated => Frames.Count > 1;

        /// <summary>
        /// Gets the value indicating if the tile can be rendered using a cached mesh (has 1 frame).
        /// </summary>
        public bool CanCacheTileMesh => Frames.Count == 1;

        /// <summary>
        /// Gets the tile frame of the set at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The tile index</param>
        /// <returns>The tile frame</returns>
        public TileFrame this[int index] => Frames[index];
    }
}