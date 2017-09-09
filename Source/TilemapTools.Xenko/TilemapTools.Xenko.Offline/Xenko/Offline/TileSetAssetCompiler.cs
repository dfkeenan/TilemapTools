using SiliconStudio.Assets.Compiler;
using SiliconStudio.BuildEngine;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Animations;
using SiliconStudio.Xenko.SpriteStudio.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SiliconStudio.Assets;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Assets;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Offline
{
    /// <summary>
    /// The <see cref="TileSetAsset"/> compiler.
    /// </summary>
    public class TileSetAssetCompiler : AssetCompilerBase
    {
        protected override void Prepare(AssetCompilerContext context, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            throw new NotImplementedException();
        }
    }
}
