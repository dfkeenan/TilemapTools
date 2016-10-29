using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Serialization.Assets;
using SiliconStudio.Xenko.Graphics;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;

namespace TilemapTools.Xenko.Tiled
{
    public class XenkoTileLookup : TileLookup<Texture>
    {
        private readonly ContentManager content;
        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public XenkoTileLookup(TileMap map, ContentManager content) : base(map)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            this.content = content;
        }

        protected override Texture GetTexture(TilemapTools.Tiled.Image image)
        {
            if (image?.Source == null) return null;

            Texture texture = null;

            if (textures.TryGetValue(image.Source, out texture))
                return texture;

            var url = GetAssetUrl(image.Source);

            textures[image.Source] = texture = content.Load<Texture>(url);

            return texture;
        }

        private string GetAssetUrl(string source)
        {
            var i = source.LastIndexOf('.');

            if (i < 0) return source;

            return source.Substring(0, i);
        }
    }
}
