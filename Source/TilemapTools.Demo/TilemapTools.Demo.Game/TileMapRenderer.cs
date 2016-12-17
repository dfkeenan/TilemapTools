using SiliconStudio.Core.IO;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Composers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilemapTools.Tiled;
using TilemapTools.Tiled.Serialization;

using TilemapTools.Xenko.Tiled;
using TilemapTools.Xenko;
using TilemapTools.Xenko.Graphics;

namespace TiledExperiment
{
    public class TileMapRenderer:StartupScript
    {
        
        private SceneDelegateRenderer delegateRenderer;       
        private TileMeshDraw cachedTiles;
        private TileMeshRenderer cachedQuadRenderer;

        public CameraComponent Camera { get; set; }

        public string MapName { get; set; }


        public override void Start()
        {
            var serial = new TiledSerializer(new TiledSerializerOptions((s) => Content.OpenAsStream(s, StreamFlags.None), VirtualFileSystem.Combine));            
            var map = serial.LoadTileMap(MapName);
            var ffff = 34;

            RegisterRenderer(new Color(map.BackgroundColor.Value.ToRgba()));
            this.cachedQuadRenderer = new TileMeshRenderer(GraphicsDevice);

            var lookup = new XenkoTileLookup(map, Content);

            using (var builder = new OrthogonalTileMeshDrawBuilder())
            {
                const float tileSize = 1.5f;
                var top = map.Height * tileSize;

                foreach (var layer in map.Layers.OfType<Layer>().Skip(1))
                {
                    for (int y = 0; y < map.Height; y++)
                    {
                        for (int x = 0; x < map.Width; x++)
                        {
                            var tile = lookup[layer, x, y];
                            if (tile != null)
                            {

                                var source = new Rectangle(tile.SourceRectangle.X, tile.SourceRectangle.Y, tile.SourceRectangle.Width, tile.SourceRectangle.Height);
                                var tileRect = new RectangleF(x * tileSize, -y * tileSize, 1 * tileSize, 1 * tileSize);
                                                             
                                builder.Add(tile.Texture, ref source, ref tileRect);

                            }
                        }
                    }
                }

                this.cachedTiles = builder.Build(GraphicsDevice);
            }
                        
                    
        }
        
        private void RegisterRenderer(Color color)
        {
            // register the renderer in the pipeline
            var scene = SceneSystem.SceneInstance.Scene;
            var compositor = ((SceneGraphicsCompositorLayers)scene.Settings.GraphicsCompositor);
            compositor.Master.Renderers.Insert(1, delegateRenderer = new SceneDelegateRenderer(DrawTileMap));

            var clearFrame = compositor.Master.Renderers.OfType<ClearRenderFrameRenderer>().FirstOrDefault();

            clearFrame.Color = color;
        }

        public void DrawTileMap(RenderDrawContext context, RenderFrame frame)
        {
            var viewProjection = Camera.ViewProjectionMatrix;
            this.Entity.Transform.UpdateWorldMatrix();
            var world = this.Entity.Transform.WorldMatrix;

            cachedQuadRenderer.Begin(context.GraphicsContext, world,viewProjection);
            cachedQuadRenderer.Draw(cachedTiles);
            cachedQuadRenderer.End();

        }

        public override void Cancel()
        {
            cachedTiles.Dispose();

            // remove the delegate renderer from the pipeline
            var scene = SceneSystem.SceneInstance.Scene;
            var compositor = ((SceneGraphicsCompositorLayers)scene.Settings.GraphicsCompositor);
            compositor.Master.Renderers.Remove(delegateRenderer);

        }
    }


    
}
