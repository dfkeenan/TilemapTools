using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    public class Tile
    {
        public int Id { get; set; }

        public Collection<Terrain> TerrainEdges { get; set; }

        public double Probability { get; set; }

        public PropertyDictionary Properties { get; set; }

        public Image Image { get; set; }

        public TiledElementList<ObjectGroup> ObjectGroups { get; set; }

        public Collection<AnimationFrame> AnimationFrames { get; set; }

        public Terrain TopLeft => GetTerrain(0);

        public Terrain TopRight => GetTerrain(1);

        public Terrain BottomLeft => GetTerrain(2);

        public Terrain BottomRight => GetTerrain(3);

        private  Terrain GetTerrain(int i) => TerrainEdges != null && i < TerrainEdges.Count ? TerrainEdges[i] : null;
    }
}