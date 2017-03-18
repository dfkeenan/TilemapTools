using System;
using System.Runtime.CompilerServices;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko.Graphics
{
    public class TileMeshRenderer
    {
        private EffectInstance DefaultEffect;

        private EffectInstance Effect => DefaultEffect;
        private ObjectParameterAccessor<Texture>? textureUpdater;
        private ObjectParameterAccessor<SamplerState>? samplerUpdater;

        private MutablePipelineState MutablePipeline;
        private GraphicsDevice GraphicsDevice;
        private GraphicsContext GraphicsContext;
        private Matrix ViewProjectionMatrix;
        private bool hasBegun;

        private BlendStateDescription? BlendState;
        private RasterizerStateDescription? RasterizerState;
        private SamplerState SamplerState;
        private DepthStencilStateDescription? DepthStencilState;

        public ParameterCollection Parameters => Effect.Parameters;

        public TileMeshRenderer(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));

            GraphicsDevice = graphicsDevice;
            MutablePipeline = new MutablePipelineState(graphicsDevice);
            DefaultEffect = new EffectInstance(new Effect(GraphicsDevice, TileMeshRendererShader.Bytecode){ Name = "BatchDefaultEffect"});
        }

        private void PrepareForRendering()
        {
            var localSamplerState = SamplerState ?? GraphicsDevice.SamplerStates.PointClamp;

            // Sets the sampler state of the effect
            if (samplerUpdater.HasValue)
                Parameters.Set(samplerUpdater.Value, localSamplerState);

            Effect.UpdateEffect(GraphicsDevice);

            // Setup states (Blend, DepthStencil, Rasterizer)
            MutablePipeline.State.SetDefaults();
            MutablePipeline.State.RootSignature = Effect.RootSignature;
            MutablePipeline.State.EffectBytecode = Effect.Effect.Bytecode;
            MutablePipeline.State.BlendState = BlendState ?? BlendStates.AlphaBlend;
            MutablePipeline.State.DepthStencilState = DepthStencilState ?? DepthStencilStates.Default;
            MutablePipeline.State.RasterizerState = RasterizerState ?? RasterizerStates.CullBack;
            MutablePipeline.State.InputElements = VertexPositionTexture.Layout.CreateInputElements();
            MutablePipeline.State.PrimitiveType = PrimitiveType.TriangleList;
            MutablePipeline.State.Output.CaptureState(GraphicsContext.CommandList);
            MutablePipeline.Update();

        }

        public void Begin(GraphicsContext graphicsContext, Matrix world,Matrix viewProjection, Color4 color, BlendStateDescription? blendState = null, SamplerState samplerState = null, DepthStencilStateDescription? depthStencilState = null, RasterizerStateDescription? rasterizerState = null)
        {
            CheckEndHasBeenCalled();
            
            GraphicsContext = graphicsContext;
            ViewProjectionMatrix = viewProjection;

            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;


            textureUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Texture0))
                textureUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Texture0);

            samplerUpdater = null;
            if (Effect.Effect.HasParameter(TexturingKeys.Sampler))
                samplerUpdater = Effect.Parameters.GetAccessor(TexturingKeys.Sampler);

            PrepareForRendering();

            MutablePipeline.State.Output.CaptureState(GraphicsContext.CommandList);
            MutablePipeline.Update();

            GraphicsContext.CommandList.SetPipelineState(MutablePipeline.CurrentState);

            
            var wvp = world * ViewProjectionMatrix;
            Parameters.Set(TileMeshBaseKeys.MatrixTransform, wvp);
            Parameters.Set(TileMeshBaseKeys.Color, color);
            hasBegun = true;
        }
        
        public void Draw(TileMeshDraw tileMeshDraw)
        {
            CheckBeginHasBeenCalled();
            GraphicsContext.CommandList.SetVertexBuffer(0, tileMeshDraw.VertexBuffer.Buffer, tileMeshDraw.VertexBuffer.Offset, tileMeshDraw.VertexBuffer.Stride);
            GraphicsContext.CommandList.SetIndexBuffer(tileMeshDraw.IndexBuffer.Buffer, 0, tileMeshDraw.IndexBuffer.Is32Bit);

            foreach (var range in tileMeshDraw.Ranges)
            {
                if (textureUpdater.HasValue)
                    Parameters.Set(textureUpdater.Value, range.Texture);

                Effect.Apply(GraphicsContext);

                GraphicsContext.CommandList.DrawIndexed(range.IndexCount, range.StartIndex);
            }
        }

        public void End()
        {
            CheckBeginHasBeenCalled();
            
            hasBegun = false;
        }

        private void CheckBeginHasBeenCalled([CallerMemberName] string caller = null)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before " + caller);
            }
        }

        private void CheckEndHasBeenCalled([CallerMemberName] string caller = null)
        {
            if (hasBegun)
            {
                throw new InvalidOperationException("End must be called before " + caller);
            }
        }
    }
}
