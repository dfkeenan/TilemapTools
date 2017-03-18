namespace TilemapTools.Tiled
{
    public interface ITiledElement
    {
        string Name { get; }

        PropertyDictionary Properties { get; }
    }

    public static class ITiledElementExtensions
    {
        public static T Property<T>(this ITiledElement tiledElement, string propertyName) 
            => tiledElement.Properties.GetOrDefault<T>(propertyName);

        public static string RawProperty(this ITiledElement tiledElement, string propertyName)
            => tiledElement.Properties[propertyName].RawValue;
    }
}