namespace TilemapTools.Tiled
{
    public class Image : TiledDocument
    {
        public string Format { get; set; }

        //trans
        public Color? TransparentColor { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public Data Data { get; internal set; }
    }
}