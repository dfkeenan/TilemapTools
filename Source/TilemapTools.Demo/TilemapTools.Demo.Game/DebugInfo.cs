using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Composers;

namespace TilemapTools.Demo
{
    public class DebugInfo : SyncScript
    {

        public Keys ToggleKey = Keys.F12;

        public bool DebuggingEnabled = false;

        public TimeSpan MessageTimeOut = TimeSpan.FromSeconds(2);

        public Entity Camera;

        private ScriptComponent oldCameraController;
        private DebugCameraController debugCameraController;

        private readonly Queue<string> messages = new Queue<string>();

        private TimeSpan elapsedTime = TimeSpan.Zero;
        private SceneDelegateRenderer delegateRenderer;
        private SpriteBatch spriteBatch;

        public SpriteFont Font { get; set; }

        public override void Start()
        {
            base.Start();

            var virtualResolution = new Vector3(GraphicsDevice.Presenter.BackBuffer.Width, GraphicsDevice.Presenter.BackBuffer.Height, 1);
            spriteBatch = new SpriteBatch(GraphicsDevice) { VirtualResolution = virtualResolution };

            var scene = SceneSystem.SceneInstance.Scene;
            var compositor = ((SceneGraphicsCompositorLayers)scene.Settings.GraphicsCompositor);
            compositor.Master.Renderers.Add(delegateRenderer = new SceneDelegateRenderer(Draw));
        }        

        public override void Update()
        {
            if(Input.IsKeyPressed(ToggleKey))
            {
                DebuggingEnabled = !DebuggingEnabled;

                if (DebuggingEnabled)
                {
                    EnableDebugInfo();
                }
                else
                {
                    DisableDebugInfo();
                }                
            }

            elapsedTime += Game.UpdateTime.Elapsed;

            if(elapsedTime >= MessageTimeOut)
            {
                if(messages.Count > 0)
                    messages.Dequeue();

                elapsedTime -= MessageTimeOut;
            }
            
        }

        public void WriteMessage(string message) => messages.Enqueue(message);


        private void Draw(RenderDrawContext renderContext, RenderFrame frame)
        {
            if(DebuggingEnabled)
            {
                if (Font != null)
                {
                    spriteBatch.Begin(renderContext.GraphicsContext, depthStencilState: DepthStencilStates.None);
                    DrawMessages(renderContext);

                    var size = spriteBatch.MeasureString(Font, $"FPS: {Game.UpdateTime.FramePerSecond:0.00}");
                    var position = new Vector2(spriteBatch.VirtualResolution.Value.X - size.X, 0);


                    spriteBatch.DrawString(Font, $"FPS: {Game.UpdateTime.FramePerSecond:0.00}", position, Color4.White);

                    spriteBatch.End();
                }
                    
            }
        }

        private void DrawMessages(RenderDrawContext renderContext)
        {
            var position = new Vector2();

            foreach (var message in messages)
            {
                spriteBatch.DrawString(Font, message, position, Color4.White);
                position.Y += spriteBatch.MeasureString(Font, message).Y;
            }            
        }

        public override void Cancel()
        {
            // Remove the delegate renderer from the pipeline
            var scene = SceneSystem.SceneInstance.Scene;
            var compositor = ((SceneGraphicsCompositorLayers)scene.Settings.GraphicsCompositor);
            compositor.Master.Renderers.Remove(delegateRenderer);

            // destroy graphic objects
            spriteBatch.Dispose();
        }

        private void DisableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if (simulation != null)
            {
                simulation.ColliderShapesRendering = false;
            }

            if (Camera != null)
            {
                debugCameraController?.Cancel();
                Camera.Remove<DebugCameraController>();

                if (oldCameraController != null)
                    Camera.Add(oldCameraController);
            }
        }

        private void EnableDebugInfo()
        {
            var simulation = this.GetSimulation();
            if(simulation != null)
            {
                simulation.ColliderShapesRendering = true;
            }
            if (Camera != null)
            {
                oldCameraController = Camera.Get<FollowCamera>();
                oldCameraController.Cancel();
                Camera.Remove<FollowCamera>();
                Camera.Add(debugCameraController = debugCameraController ?? new Demo.DebugCameraController());
            }            
        }
    }
}
