using System;
using System.Collections;
using System.IO;

namespace TilemapTools.Tiled.Serialization
{
    public class TiledSerializerOptions
    {
        private readonly Func<string, Stream> openStream;

        public TiledSerializerOptions(Func<string, Stream> openStream)
        {
            this.openStream = openStream;
        }

        public static TiledSerializerOptions Default { get; internal set; } = new TiledSerializerOptions(null);

        public bool DefaultEmptyCollections { get; set; } = true;

        public bool PreLoadTileSet { get; set; } = true;

        public TiledSerializerOptions Clone() => (TiledSerializerOptions)this.MemberwiseClone();

        public Stream OpenStream(string source)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(source), $"{nameof(source)} is null or empty");

            if (openStream == null)
                throw new InvalidOperationException($"No {nameof(TiledSerializerOptions.openStream)} delegate define");

            return openStream(source);
        }

        internal TCollection GetCollectionOrDefault<TCollection>(TCollection collection) where TCollection : class, ICollection, new()
        {
            if (collection != null && collection.Count > 0)
                return collection;

            if (DefaultEmptyCollections)
                return new TCollection();

            return null;
        }
    }
}