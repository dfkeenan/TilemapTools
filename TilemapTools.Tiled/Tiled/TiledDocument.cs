using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools.Tiled
{
    public abstract class TiledDocument
    {
        public string Source { get; set; }

        internal string SourcePath => Source == null ? "" : Path.GetDirectoryName(Source);
                
    }   
}
