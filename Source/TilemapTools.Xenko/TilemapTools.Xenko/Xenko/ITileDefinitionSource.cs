namespace TilemapTools.Xenko
{
    public interface ITileDefinitionSource
    {
        Tile GetTile(ref TileReference reference);
    }
}
