namespace TilemapTools.Tiled
{
    public enum TileFlip
    {
        None = 0,
        Horizontally = 4,
        Vertacally = 2,
        Diagonally = 1
    }

    internal static class TileFlipExtensions
    {
        private const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        private const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        private const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        private const uint FLIIPED = FLIPPED_DIAGONALLY_FLAG | FLIPPED_HORIZONTALLY_FLAG | FLIPPED_DIAGONALLY_FLAG;

        public static void Convert(ref uint globalId, out TileFlip flip)
        {
            flip = (TileFlip)((globalId & FLIIPED) >> 29);

            globalId &= ~FLIIPED;
        }
    }
}