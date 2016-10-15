using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools.Tiled
{
    /// <summary>
    /// An abstract base class for <see cref="TileMap"/> and related objects that have a source.
    /// </summary>
    public abstract class TiledDocument
    {
        /// <summary>
        /// Get or set the source of the document. Used to reference other files.
        /// </summary>
        /// <remarks>
        /// In TMX and TSX files the sources are relative to the containing document. 
        /// However the loaded documents the source is relative to the originally loaded Document (TMX/TSX file).
        /// </remarks>
        public string Source { get; set; }

        internal string SourcePath => Source == null ? "" : Path.GetDirectoryName(Source);
                
    }   
}
