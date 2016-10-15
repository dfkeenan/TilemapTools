namespace TilemapTools.Tiled
{
    public class ObjectGroup : LayerBase, ILayer
    {
        public ObjectGroupDrawOrder DrawOrder { get; set; }

        public TiledElementList<TiledObject> Objects { get; set; }
    }
}