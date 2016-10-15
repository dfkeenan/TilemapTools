using System.Collections.ObjectModel;

namespace TilemapTools.Tiled
{
    public class TiledObject : ITiledElement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TiledObjectType ObjectType { get; set; }

        public string Type { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Rotation { get; set; }

        public LayerTile? Tile { get; set; }

        public bool Visible { get; set; }

        public Collection<Point> Points { get; set; }

        public PropertyDictionary Properties { get; set; }
    }
}