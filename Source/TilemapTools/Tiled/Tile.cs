using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Gets or sets the local tile-id of this tile that represents the terrain visually.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Terrain edges.
        /// </summary>
        public Collection<Terrain> TerrainEdges { get; set; }

        /// <summary>
        /// Gets or sets the probability that this tile is chosen when it competes with others while editing with the terrain tool.
        /// </summary>
        public double Probability { get; set; }

        /// <summary>
        /// Gets or sets the properties for this tile.
        /// </summary>
        public PropertyDictionary Properties { get; set; }

        /// <summary>
        /// Gets or sets the image for this tile.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the object groups of this tile.
        /// </summary>
        public TiledElementList<ObjectGroup> ObjectGroups { get; set; }

        /// <summary>
        /// Gets or sets the animation frames of this tile.
        /// </summary>
        public Collection<AnimationFrame> AnimationFrames { get; set; }

        /// <summary>
        /// Gets the top left terain.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="null"/> if ther is no terrain.
        /// </remarks>
        public Terrain TopLeft => GetTerrain(0);

        /// <summary>
        /// Gets the top right terain or <see langword="null"/> if there is no terrain.
        /// </summary>
        public Terrain TopRight => GetTerrain(1);

        /// <summary>
        /// Gets the bottom left terain or <see langword="null"/> if there is no terrain.
        /// </summary>
        public Terrain BottomLeft => GetTerrain(2);

        /// <summary>
        /// Gets the top bottom terain or <see langword="null"/> if there is no terrain.
        /// </summary>
        public Terrain BottomRight => GetTerrain(3);

        private  Terrain GetTerrain(int i) => TerrainEdges != null && i < TerrainEdges.Count ? TerrainEdges[i] : null;
    }
}