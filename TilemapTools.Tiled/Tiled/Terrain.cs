namespace TilemapTools.Tiled
{
    public class Terrain : ITiledElement
    {
        public string Name { get; set; }

        public int Tile { get; set; }

        public PropertyDictionary Properties { get; set; }
    }
}