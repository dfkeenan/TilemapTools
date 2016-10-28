namespace TilemapTools.Tiled
{
    public interface ITiledElement
    {
        string Name { get; }

        PropertyDictionary Properties { get; }
    }
}