using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilemapTools.Mathematics;

namespace TilemapTools.Tiled.Serialization
{
    public abstract class TileLookup<TTexture>
    {
        private readonly Dictionary<uint, TileInfo<TTexture>> cachedTiles = new Dictionary<uint, TileInfo<TTexture>>();
        private readonly TileMap map;

        public TileMap Map => map;

        public TileLookup(TileMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            this.map = map;
        }


        public TileInfo<TTexture> this[Layer layer, int x, int y]
        {
            get
            {
                var layerTile = layer.Tiles[map.Width * y + x];

                if (layerTile.IsEmpty) return null;

                return this[layerTile];
            }
        }

        public TileInfo<TTexture> this[LayerTile tile] => this[tile.GlobalId];

        public TileInfo<TTexture> this[uint globalId]
        {
            get
            {


                TileInfo<TTexture> tileInfo = null;

                if (cachedTiles.TryGetValue(globalId, out tileInfo))
                    return tileInfo;

                var tileset = FindTileSet(globalId);

                Tile tile = tileset.GetTile(globalId);

                Rectangle sourceRectangle = Rectangle.Empty;
                TTexture texture = default(TTexture);

                if(tile?.Image != null)
                {
                    sourceRectangle = new Rectangle(0, 0, tile.Image.Width.GetValueOrDefault(), tile.Image.Height.GetValueOrDefault());
                    texture = GetTexture(tile?.Image);
                }
                else
                {
                    sourceRectangle = tileset.CalculateSourceRectangle(globalId);
                    texture = GetTexture(tileset.Image);
                }
                
                cachedTiles[globalId] = tileInfo = new TileInfo<TTexture>(texture, sourceRectangle, tile, tileset);

                return tileInfo;
            }
        }

        public abstract TTexture GetTexture(Image image);


        public int FindTileSetIndex(uint tileGlobalId)
        {
            for (int i = map.TileSets.Count - 1; i >= 0; --i)
            {
                var currentTileSet = map.TileSets[i];
                if (currentTileSet.FirstGlobalId <= tileGlobalId)
                {
                    return i;
                }

            }

            throw new ArgumentOutOfRangeException(nameof(tileGlobalId), "Unable to find TileSet.");
        }


        public TileSet FindTileSet(uint tileGlobalId) => map.TileSets[FindTileSetIndex(tileGlobalId)];
    }
}
