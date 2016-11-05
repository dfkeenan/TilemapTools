namespace TilemapTools
{
    public struct CellLocationPair<TCell>
        where TCell : class
    {
        public CellLocationPair(TCell content, int x, int y) : this()
        {
            Content = content;
            X = x;
            Y = y;
        }

        public TCell Content { get; }

        public int X { get; }

        public int Y { get; }
    }
}