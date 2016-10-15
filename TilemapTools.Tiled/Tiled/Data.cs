using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace TilemapTools.Tiled
{
    public class Data
    {
        public DataEncoding Encoding { get; set; }

        public DataCompression Compression { get; set; }

        public string Content { get; set; }

        public Stream OpenStream()
        {
            if (Encoding != DataEncoding.Base64)
                throw new InvalidOperationException($"{nameof(Data.Encoding)} must be Base64");

            var rawData = Convert.FromBase64String(Content);

            Stream stream = new MemoryStream(rawData);

            var compression = Compression;

            if (compression == DataCompression.None)
                throw new InvalidOperationException($"{nameof(Data.Encoding)} must be GZip or zLib");

            if (compression == DataCompression.zLib)
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            else if (compression == DataCompression.GZip)
                stream = new GZipStream(stream, CompressionMode.Decompress);

            return stream;
        }

        public IEnumerable<LayerTile> ReadTiles(int width, int height)
        {
            switch (Encoding)
            {
                case DataEncoding.None:
                    throw new NotImplementedException($"Currently {nameof(Data.Encoding)} must be Base64 or CSV");
                case DataEncoding.Base64:
                    using (var stream = OpenStream())
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