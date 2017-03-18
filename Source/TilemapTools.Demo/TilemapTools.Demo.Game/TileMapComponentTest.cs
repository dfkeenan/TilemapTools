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
using System;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Demo
{
    public class TileMapComponentTest: SyncScript
    {
        public string MapName { get; set; }

        private List<TileMapComponent> layers = new List<TileMapComponent>();

        private CameraComponent camera;
        private DebugInfo Debug;

        public override void Start()
        {
            camera = SceneSystem.SceneInstance.Select(e => e.Get<CameraComponent>()).Where(c => c != null).FirstOrDefault();

            Debug = SceneSystem.SceneInstance.Select(e => e.Get<DebugInfo>()).Where(c => c != null).FirstOrDefault();

            LoadTileMap();

        }

        private void LoadTileMap()
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

                            tileMapComponent.Grid[x, y] = new TileReference((byte)lookup.FindTileSetIndex(layerTile.GlobalId), (short)(layerTile.GlobalId - firstGlobalId));
                        }

                        x++;
                        if (x == 0) x++;
                    }
                    x = -map.Width / 2;
                    y--;
                    if (y == 0) y--;
                }

                tileMapComponent.Color = new Color4(1, 1, 1, 1) * (float)layer.Opacity; //PreMulitplied Alpha


                var layerEntity = new Entity()
                {
                    tileMapComponent,
                };

                layerEntity.Transform.Position = new Vector3(0, 0, layerDepth);

                //TODO: Improve properties usability
                if (layer.Properties.ContainsKey("CollisionEnabled"))
                {
                    var physicsShapeBuilder = new MinimumPhysicsShapeBuilder();

                    var staticColliderComponent = new StaticColliderComponent();
                    staticColliderComponent.Restitution = 0;
                    physicsShapeBuilder.Update(tileMapComponent.Grid, staticColliderComponent);
                    layerEntity.Add(staticColliderComponent);
                }

                layers.Add(tileMapComponent);

                Entity.AddChild(layerEntity);
                layerDepth += 1f;
            }
        }

        private readonly Keys[] numberKeys = Enumerable.Range((int)Keys.D1, 9).Cast<Keys>().Concat(new[] { Keys.D0 }).ToArray();

        public override void Update()
        {
            for (int i = 0; i < Math.Min(numberKeys.Length,layers.Count); i++)
            {
                if(Input.IsKeyPressed(numberKeys[i]))
                {
                    layers[i].Enabled = !layers[i].Enabled;
                }
            }

            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];

                    var world = layer.Entity.Transform.WorldMatrix;
                    var ray = GetRay(Input.MousePosition);
                    var pick = layer.Grid.GetCell(ref world, ref ray);

                    if (!pick.HasValue) continue;

                   
                    Debug?.WriteMessage($"Layer {i + 1} -> X: {pick.Value.X}; Y:{pick.Value.Y}");

                    if ( i == 0 && pick.Value.Content.IsEmpty)
                    {
                        layer.Grid[pick.Value.X, pick.Value.Y] = new TileReference(0, 1);
                    }
                    //else
                    //{


                    //    var x = pick.X;
                    //    var y = pick.Y;
                    //    var bs = layer.Grid.BlockSize;
                    //    ShortPoint block;
                    //    int cx, cy;

                    //    GridBlock.GetBlockCellLocation(ref x, ref y, ref bs, out block, out cx, out cy);
                    //}
                }

            }
            else
            {
                Input.UnlockMousePosition();
            }
        }

        private Ray GetRay(Vector2 screenPos)
        {
            var backBuffer = GraphicsDevice.Presenter.BackBuffer;
            screenPos.X *= backBuffer.Width;
            screenPos.Y *= backBuffer.Height;

            var viewport = new Viewport(0, 0, backBuffer.Width, backBuffer.Height);

            var unprojectedNear =
                viewport.Unproject(
                    new Vector3(screenPos, 0.0f),
                    camera.ProjectionMatrix,
                    camera.ViewMatrix,
                    Matrix.Identity);

            var unprojectedFar =
                viewport.Unproject(
                    new Vector3(screenPos, 1.0f),
                    camera.ProjectionMatrix,
                    camera.ViewMatrix,
                    Matrix.Identity);

            var direction = unprojectedFar - unprojectedNear;
            direction.Normalize();
            return new Ray(unprojectedNear, direction);
        }
    }
}
