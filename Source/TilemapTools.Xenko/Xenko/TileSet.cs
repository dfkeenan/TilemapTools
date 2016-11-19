using System.Collections.Generic;
using System.ComponentModel;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// A set (group) of tiles.
    /// </summary>
    [DataContract]
    [DataSerializerGlobal(typeof(ReferenceSerializer<TileSet>), Profile = "Content")]
    [ContentSerializer(typeof(DataContentSerializer<TileSet>))]
    public class TileSet
    {
        public const int DefaultTileSize = 32;

        /// <summary>
        /// The tile width in pixels.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(10)]
        [Display("Tile Width")]
        [DefaultValue(DefaultTileSize)]

        public int TileWidth { get; set; }

        /// <summary>
        /// The tile height in pixels.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(20)]
        [Display("Block Height")]
        [DefaultValue(DefaultTileSize)]
        public int TileHeight { get; set; }

        /// <summary>
        /// The amount of padding around the perimeter of the tile sheet (in pixels).
        /// </summary>
        /// <remarks></remarks>
        [DataMember(30)]
        [Display("Margin")]
        public int Margin { get; set; }

        /// <summary>
        /// The amount of padding between tiles in the tile sheet (in pixels).
        /// </summary>
        /// <remarks></remarks>
        [DataMember(40)]
        [Display("Spacing")]
        public int Spacing { get; set; }

        /// <summary>
        /// The texture of the <see cref="TileSet"/>.
        /// </summary>
        /// <remarks></remarks>
        [DataMember(50)]
        [Display("Tile Sheet")]
        public Texture TileSheet { get; set; }

        /// <summary>
        /// The list of tiles.
        /// </summary>
        //[MemberCollection(NotNullItems = true)] //TODO: 1.9
        public List<Tile> Tiles { get; } = new List<Tile>();

    }
}
