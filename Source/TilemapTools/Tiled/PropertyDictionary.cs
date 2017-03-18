using System.Collections.Generic;

namespace TilemapTools.Tiled
{
    public class PropertyDictionary : Dictionary<string, Property>
    {
        public T GetOrDefault<T>(string propertyName) => this.ContainsKey(propertyName) ? (T)this[propertyName].Value : default(T);        
    }
}