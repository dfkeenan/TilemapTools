using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;
using TilemapTools.Xenko.Rendering;
using TilemapTools.Xenko.Physics;
using System.ComponentModel;
using System;

namespace TilemapTools.Xenko
{
    /// <summary>
    /// Add a Tilemap to an <see cref="Entity"/>.
    /// </summary>
    [DataContract("TileMapComponent")]
    [Display("Tile Map", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(TileMapProcessor))]
    [DefaultEntityComponentRenderer(typeof(TileMapRendererProcessor))]
    [ComponentOrder(50000)]
    public sealed class TileMapComponent: ActivableEntityComponent
    {

        public enum TileMapSampler
        {
            [Display("Point (Nearest)")]
            PointClamp,

            [Display("Linear")]
            LinearClamp,

            [Display("Anisotropic")]
            AnisotropicClamp,

        }

        /// <summary>
        /// The color to apply on the tile map.
        /// </summary>
        [DataMember(10)]
        [Display("Color")]
        public Color4 Color = Color4.White;

        /// <summary>
        /// Gets or sets a value indicating whether the tile map is a pre-multiplied alpha (default is true).
        /// </summary>
        /// <value><c>true</c> if the texture is pre-multiplied by alpha; otherwise, <c>false</c>.</value>
        [DataMember(20)]
        [DefaultValue(true)]
        public bool PremultipliedAlpha { get; set; } = true;

        /// <summary>
        /// Ignore the depth of other elements of the scene when rendering the tile map by disabling the depth test.
        /// </summary>
        /// <userdoc>Ignore the depth of other elements of the scene when rendering the tile map. When checked, the tile map is always put on top of previous elements.</userdoc>
        [DataMember(30)]
        [DefaultValue(false)]
        [Display("Ignore Depth")]
        public bool IgnoreDepth;


        /// <summary>
        /// Specifies the texture sampling method to be used for this tile map
        /// </summary>
        /// <userdoc>
        /// Specifies the texture sampling method to be used for this tile map
        /// </userdoc>
        [DataMember(40)]
        [DefaultValue(TileMapSampler.PointClamp)]
        [Display("Sampler")]
        public TileMapSampler Sampler { get; set; } = TileMapSampler.PointClamp;


        [DataMember(50)]
        [DefaultValue(0)]
        [Display("Frames Per Second")]
        public int FramesPerSecond
        {
            get
            {
                return framesPerSecond;
            }
            set
            {
                framesPerSecond = value;
                frameTime = framesPerSecond == 0 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(1000 / framesPerSecond);
                isAnimated = Math.Abs(framesPerSecond) > 1;
            }
        }

        /// <summary>
        /// The grid of the tile map.
        /// </summary>
        [DataMember(60)]
        [Display("Grid")]
        [NotNull]
        [DataMemberCustomSerializer]
        public TileGrid Grid { get; set; } = new OrthogonalTileGrid();


        /// <summary>
        /// The physics shape builder of the tile map.
        /// </summary>
        [DataMember(70)]
        [Display("Physics Shape Builder")]
        [DataMemberCustomSerializer]
        public PhysicsShapeBuilder PhysicsShapeBuilder { get; set; }

        [DataMember(90)]
        public RenderGroup RenderGroup { get; set; }

        private int framesPerSecond = 0;
        private TimeSpan frameTime = TimeSpan.Zero;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private bool isAnimated = false;

        internal bool IsAnimated
        {
            get
            {
                return isAnimated;
            }
        }

        internal void UpdateAnimatedTiles(ref TimeSpan elapsedTime)
        {
            if (!isAnimated) return;

            this.elapsedTime += elapsedTime;

            if(this.elapsedTime >= frameTime)
            {
                //Increment tileFrames;


                this.elapsedTime -= frameTime;
            }
        }
    }
}
