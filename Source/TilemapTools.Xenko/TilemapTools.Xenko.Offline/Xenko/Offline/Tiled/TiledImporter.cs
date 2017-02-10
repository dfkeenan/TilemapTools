using System;
using System.Collections.Generic;
using SiliconStudio.Assets;
using SiliconStudio.Core.IO;

namespace TilemapTools.Xenko.Offline.Tiled
{
    public class TiledImporter : AssetImporterBase
    {
        private const string FileExtensions = ".tmx;.tms";

        public override IEnumerable<Type> RootAssetTypes
        {
            get
            {
                yield return typeof(TileSetAsset);
            }
        }

        public override IEnumerable<AssetItem> Import(UFile rawAssetPath, AssetImporterParameters importParameters)
        {
            var outputAssets = new List<AssetItem>();

          

            return outputAssets;
        }

        public override Guid Id { get; } = new Guid("8FCFA3FF-7057-475E-BB07-5183A727B017");
        public override string Description { get; } = "Tiled Tile Map And Tilest Importer";

        public override string SupportedFileExtensions => FileExtensions;
    }
}
