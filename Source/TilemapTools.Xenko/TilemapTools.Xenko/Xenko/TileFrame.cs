using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// A tile frame represents a single frame a tile animation.
    /// </summary>
    [DataContract]
    [ContentSerializer(typeof(DataContentSerializer<TileFrame>))]
    [DataSerializerGlobal(typeof(ReferenceSerializer<TileFrame>), Profile = "Content")]
    public class TileFrame
    {
        /// <summary>
        /// The rectangle specifying the region of the texture to use for that frame.
        /// </summary>
        public Rectangle TextureRegion;

        /// <summary>
        /// The texture in which the image is contained
        /// </summary>
        public Texture Texture { get; set; }
    }
}
