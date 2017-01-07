using System.Collections.Generic;
using System.ComponentModel;

using SiliconStudio.Assets;
using SiliconStudio.Assets.Compiler;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Core.Yaml;
using SiliconStudio.Xenko.Assets.Sprite;
using SiliconStudio.Xenko.Assets.Textures;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Assets
{
    /// <summary>
    /// This asset represents a sheet (group) of sprites.
    /// </summary>
    [DataContract("TileSet")]
    [CategoryOrder(10, "Parameters")]
    [CategoryOrder(50, "Atlas Packing")]
    [CategoryOrder(150, "Tiles")]
    [AssetFormatVersion(TilemapToolsConfig.PackageName, "0.1.0")]
    [AssetDescription(FileExtension)]
    [AssetContentType(typeof(TileSet))]
    [AssetCompiler(typeof(TileSetAssetCompiler))]
    [Display(1600, "Tile Set")]
    public class TileSetAsset : Asset
    {
        /// <summary>
        /// The default file extension used by the <see cref="TileSetAsset"/>.
        /// </summary>
        public const string FileExtension = ".xktileset";

        /// <summary>
        /// Gets or sets the color key used when color keying for a texture is enabled. When color keying, all pixels of a specified color are replaced with transparent black.
        /// </summary>
        /// <value>The color key.</value>
        /// <userdoc>
        /// The color that should be made transparent in all images of the set.
        /// </userdoc>
        [DataMember(20)]
        [Display(category: "Parameters")]
        public Color ColorKeyColor { get; set; } = new Color(255, 0, 255);

        /// <summary>
        /// Gets or sets a value indicating whether to enable color key. Default is false.
        /// </summary>
        /// <value><c>true</c> to enable color key; otherwise, <c>false</c>.</value>
        /// <userdoc>
        /// If checked, the color specified by 'ColorKeyColor' is made transparent in all images of the set during the asset build.
        /// </userdoc>
        [DataMember(30)]
        [DefaultValue(false)]
        [Display(category: "Parameters")]
        public bool ColorKeyEnabled { get; set; }

        /// <summary>
        /// Gets or sets the texture format.
        /// </summary>
        /// <value>The texture format.</value>
        /// <userdoc>
        /// The texture format in which all the images of the set should be converted to.
        /// </userdoc>
        [DataMember(40)]
        [DefaultValue(TextureFormat.Compressed)]
        [Display(category: "Parameters")]
        public TextureFormat Format { get; set; } = TextureFormat.Compressed;

        /// <summary>
        /// Gets or sets the value indicating whether the output texture is encoded into the standard RGB color space.
        /// </summary>
        /// <userdoc>
        /// If checked, the input image is considered as an sRGB image. This should be default for colored texture
        /// with a HDR/gamma correct rendering.
        /// </userdoc>
        [DataMember(45)]
        [DefaultValue(TextureColorSpace.Auto)]
        [Display("ColorSpace", "Parameters")]
        public TextureColorSpace ColorSpace { get; set; } = TextureColorSpace.Auto;

        /// <summary>
        /// Gets or sets the alpha format.
        /// </summary>
        /// <value>The alpha format.</value>
        /// <userdoc>
        /// The texture alpha format in which all the images of the set should be converted to.
        /// </userdoc>
        [DataMember(50)]
        [DefaultValue(AlphaFormat.Auto)]
        [Display(category: "Parameters")]
        public AlphaFormat Alpha { get; set; } = AlphaFormat.Auto;

        /// <summary>
        /// Gets or sets a value indicating whether [generate mipmaps].
        /// </summary>
        /// <value><c>true</c> if [generate mipmaps]; otherwise, <c>false</c>.</value>
        /// <userdoc>
        /// If checked, mipmaps are generated for all the images of the set.
        /// </userdoc>
        [DataMember(60)]
        [DefaultValue(false)]
        [Display(category: "Parameters")]
        public bool GenerateMipmaps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to convert the texture in pre-multiply alpha.
        /// </summary>
        /// <value><c>true</c> to convert the texture in pre-multiply alpha.; otherwise, <c>false</c>.</value>
        /// <userdoc>
        /// If checked, pre-multiply all color components of the images by their alpha-component.
        /// Use this when elements are rendered with standard blending (and not transitive blending).
        /// </userdoc>
        [DataMember(70)]
        [DefaultValue(true)]
        [Display(category: "Parameters")]
        public bool PremultiplyAlpha { get; set; } = true;

        /// <summary>
        /// Gets or sets the packing attributes of the set.
        /// </summary>
        /// <userdoc>
        /// The parameters used to pack the tiles into atlas.
        /// </userdoc>
        [NotNull]
        [DataMember(100)]
       // [Category("Atlas Packing")]
        public PackingAttributes Packing { get; set; } = new PackingAttributes();

        /// <summary>
        /// Gets or sets the tiles of the set.
        /// </summary>
        /// <userdoc>
        /// The list of tiles composing the set.
        /// </userdoc>
        [DataMember(150)]
       // [Category]
        [MemberCollection(NotNullItems = true)]
        public List<SpriteInfo> Sprites { get; set; } = new List<SpriteInfo>();
    }
}
