using System.Linq;
using SiliconStudio.Core.IO;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;
using TilemapTools.Xenko;
using TilemapTools.Xenko.Tiled;
using SiliconStudio.Xenko.Physics;
using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;
using TilemapTools.Xenko.Physics;

namespace TilemapTools.Demo
{
    public class TileMapComponentTest: StartupScript
    {
        public string MapName { get; set; }

        public override void Start()
        {
            if (string.IsNullOrEmpty(MapName) || !Content.Exists(MapName))
                return;

            

            var serial = new TiledSerializer(new TiledSerializerOptions((s) => Content.OpenAsStream(s, StreamFlags.None), VirtualFileSystem.Combine));
            var map = serial.LoadTileMap(MapName);

            var lookup = new XenkoTileLookup(map, Content);

            var tileSets = new List<Xenko.TileSet>();


            foreach (var tileSet in map.TileSets)
            {                
                var xenkoTileSet = new TilemapTools.Xenko.TileSet();

                var texture = lookup.GetTexture(tileSet.Image);
                var columns = tileSet.Columns.GetValueOrDefault(1);


                for (int localId = 0; localId < tileSet.TileCount; localId++)
                {
                    var x = localId % columns;
                    var y = localId / columns;

                    var left = x * (tileSet.TileWidth + tileSet.Spacing) + tileSet.Margin;
                    var top = y * (tileSet.TileHeight + tileSet.Spacing) + tileSet.Margin;

                    var rect = new SiliconStudio.Core.Mathematics.Rectangle();
                    rect.X = left;
                    rect.Y = top;
                    rect.Width = tileSet.TileWidth;
                    rect.Height = tileSet.TileHeight;

                    xenkoTileSet.Tiles.Add(new TilemapTools.Xenko.Tile(texture, rect));
                }

                tileSets.Add(xenkoTileSet);
                
            }


            var firstGlobalId = map.TileSets[0].FirstGlobalId;

            float layerDepth = 0;


            foreach (var layer in map.Layers.OfType<Layer>())
            {
                var tileMapComponent = new TileMapComponent();
                tileMapComponent.Grid.CellSize = new Vector2(1.5f);
                tileSets.ForEach(ts => tileMapComponent.Grid.TileSets.Add(ts));                

                int x = -map.Width / 2;
                int y = map.Height / 2;

                for (int my = 0; my < map.Height; my++)
                {
                    for (int mx = 0; mx < map.Width; mx++)
                    {
                        var layerTile = layer.Tiles[map.Width * my + mx];

                        if (!layerTile.IsEmpty)
                        {                    
                            //assuming first tileset for testing purposes        
                            tileMapComponent.Grid[x, y] = new TileReference(0,(short)(layerTile.GlobalId - firstGlobalId));
                        }
                        
                        x++;
                        if (x == 0) x++;
                    }
                    x = -map.Width / 2;
                    y--;
                    if (y == 0) y--;
                }

                var layerEntity = new Entity()
                {
                    tileMapComponent,
                };

                layerEntity.Transform.Position = new Vector3(0, 0, layerDepth);

                //TODO: Improve properties usability
                if (layer.Properties.ContainsKey("CollisionEnabled"))
                {
                    var physicsShapeBuilder = new ColliderShapePerTilePhysicsShapeBuilder();

                    var staticColliderComponent = new StaticColliderComponent();
                    physicsShapeBuilder.Update(tileMapComponent.Grid, staticColliderComponent);
                    layerEntity.Add(staticColliderComponent);
                }

                Entity.AddChild(layerEntity);
                layerDepth += 1f;
            }

        }
    }
}
