namespace TilemapTools.Tiled
{
    public abstract class LayerBase : ILayer
    {
        public string Name { get; set; }

        public double Opacity { get; set; } = 1;

        public bool Visible { get; set; } = true;

        public double OffsetX { get; set; }

        public double OffsetY { get; set; }

        public PropertyDictionary Properties { get; set; }
    }
}