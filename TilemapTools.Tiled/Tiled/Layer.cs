using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    public class Layer : LayerBase, ILayer
    {
        public Collection<LayerTile> Tiles { get; set; }
    }
}