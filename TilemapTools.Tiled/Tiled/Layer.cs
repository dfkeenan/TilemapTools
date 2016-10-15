using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a tile layer.
    /// </summary>
    public class Layer : LayerBase, ILayer
    {
        /// <summary>
        /// Get or set the layer tiles.
        /// </summary>
        public Collection<LayerTile> Tiles { get; set; }
    }
}