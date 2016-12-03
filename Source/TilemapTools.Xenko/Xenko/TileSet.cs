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
        /// <summary>
        /// The list of tiles.
        /// </summary>
        [MemberCollection(NotNullItems = true)]
        public List<Tile> Tiles { get; } = new List<Tile>();

        /// <summary>
        /// Find the index of a tile in the set using its name.
        /// </summary>
        /// <param name="name">The name of the tile</param>
        /// <returns>The index value</returns>
        /// <remarks>If two tiles have the provided name then the first tile found is returned</remarks>
        /// <exception cref="KeyNotFoundException">No tile in the set have the given name</exception>
        public int FindImageIndex(string name)
        {
            if (Tiles != null)
            {
                for (int i = 0; i < Tiles.Count; i++)
                {
                    if (Tiles[i].Name == name)
                        return i;
                }
            }

            throw new KeyNotFoundException(name);
        }

        /// <summary>
        /// Gets or sets the tile of the set at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The tile index</param>
        /// <returns>The image</returns>
        public Tile this[int index]
        {
            get { return Tiles[index]; }
            set { Tiles[index] = value; }
        }

        /// <summary>
        /// Gets or sets the tile of the set having the provided <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the tile</param>
        /// <returns>The image</returns>
        public Tile this[string name]
        {
            get { return Tiles[FindImageIndex(name)]; }
            set { Tiles[FindImageIndex(name)] = value; }
        }
    }
}
