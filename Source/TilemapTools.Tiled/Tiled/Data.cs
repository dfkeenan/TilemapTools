using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// Reresents embedded data.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        public DataEncoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        public DataCompression Compression { get; set; }

        /// <summary>
        /// Gets or sets the raw content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Opens a <see cref="Stream"/> of the embedded data. Decompressing if necessary.
        /// </summary>
        /// <returns>A decodded and decompressed <see cref="Stream"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="Encoding"/> was not <see cref="DataEncoding.Base64"/>.</exception>
        /// <remarks>
        /// Use <see cref="ReadTiles(int, int)"/> to get layer tiles. 
        /// </remarks>
        public Stream OpenDecodedStream()
        {
            if (Encoding != DataEncoding.Base64)
                throw new InvalidOperationException($"{nameof(Data.Encoding)} must be Base64");

            var rawData = Convert.FromBase64String(Content);

            Stream stream = new MemoryStream(rawData);

            var compression = Compression;

            if (compression == DataCompression.None)
                return stream;

            if (compression == DataCompression.zLib)
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            else if (compression == DataCompression.GZip)
                stream = new GZipStream(stream, CompressionMode.Decompress);

            return stream;
        }

        /// <summary>
        /// Reads tiles from layer data.
        /// </summary>
        /// <param name="width">Width of the layer/map in tiles.</param>
        /// <param name="height">Height of the layer/map in tiles</param>
        /// <returns>Tiles from layer.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="Encoding"/> was not a <see cref="DataEncoding"/> value.</exception>
        /// <exception cref="NotImplementedException">Currently the only <see cref="DataEncoding"/> values supported are <see cref="DataEncoding.CSV"/> and <see cref="DataEncoding.Base64"/>.</exception>
        public IEnumerable<LayerTile> ReadTiles(int width, int height)
        {
            switch (Encoding)
            {
                case DataEncoding.None:
                    throw new NotImplementedException($"Currently {nameof(Data.Encoding)} must be Base64 or CSV");
                case DataEncoding.Base64:
                    using (var stream = OpenDecodedStream())
                    using (var reader = new BinaryReader(stream))
                    {
                        var count = width * height;
                        var tiles = new List<LayerTile>();

                        for (int i = 0; i < count; i++)
                        {
                            var gid = reader.ReadUInt32();
                            tiles.Add(new LayerTile(gid));
                        }

                        return tiles;
                    }
                case DataEncoding.CSV:
                    {
                        var tiles = new List<LayerTile>();
                        foreach (var s in Content.Split(','))
                        {
                            var gid = uint.Parse(s.Trim());
                            tiles.Add(new LayerTile(gid));
                        }
                        return tiles;
                    }
                default:
                    throw new InvalidOperationException($"Unknown {nameof(Data.Encoding)}.");
            }
        }
    }
}