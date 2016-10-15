namespace TilemapTools.Tiled
{
    public struct TileOffset
    {
        public TileOffset(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }
}