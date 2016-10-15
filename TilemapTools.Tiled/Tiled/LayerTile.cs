namespace TilemapTools.Tiled
{
    public struct LayerTile
    {
        public LayerTile(uint rawGlobalId)
        {
            TileFlip flip;
            TileFlipExtensions.Convert(ref rawGlobalId, out flip);
            GlobalId = rawGlobalId;
            Flip = flip;
        }

        public uint GlobalId;

        public TileFlip Flip;
    }
}