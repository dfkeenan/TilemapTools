namespace TilemapTools.Tiled
{
    public class TileMap : TiledDocument
    {
        public string Version { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public int? HexSideLength { get; set; }

        public OrientationType Orientation { get; set; }

        public StaggerAxis StaggerAxis { get; set; }

        public StaggerIndex StaggerIndex { get; set; }

        public RenderOrder RenderOrder { get; set; }

        public Color? BackgroundColor { get; set; }

        public int? NextObjectID { get; set; }

        public PropertyDictionary Properties { get; set; }

        public TiledElementList<TileSet> TileSets { get; set; }

        public TiledElementList<ILayer> Layers { get; set; }
    }
}