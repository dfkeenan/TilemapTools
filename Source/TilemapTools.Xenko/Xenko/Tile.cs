using SiliconStudio.Core;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;

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

    }
}