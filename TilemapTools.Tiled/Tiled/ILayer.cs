namespace TilemapTools.Tiled
{
    public interface ILayer : ITiledElement
    {
        bool Visible { get; }

        double Opacity { get; }

        double OffsetX { get; }

        double OffsetY { get; }
    }
}