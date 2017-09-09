using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko.Rendering
{
    //public class TileMapPipelinePlugin : PipelinePlugin<TileMapRenderFeature>
    //{
    //    protected override TileMapRenderFeature CreateRenderFeature(PipelinePluginContext context)
    //    {
    //        // Mandatory render stages
    //        var transparentRenderStage = context.RenderSystem.GetOrCreateRenderStage("Transparent", "Main", new RenderOutputDescription(context.RenderContext.GraphicsDevice.Presenter.BackBuffer.ViewFormat, context.RenderContext.GraphicsDevice.Presenter.DepthStencilBuffer.ViewFormat));

    //        var tileMapRenderFeature = new TileMapRenderFeature();
    //        tileMapRenderFeature.RenderStageSelectors.Add(new SimpleGroupToRenderStageSelector
    //        {
    //            EffectName = "Test",
    //            RenderStage = transparentRenderStage
    //        });

    //        return tileMapRenderFeature;
    //    }
    //}
}