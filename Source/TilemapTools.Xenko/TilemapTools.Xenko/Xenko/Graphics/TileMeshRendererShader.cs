using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Shaders;

namespace TilemapTools.Xenko.Graphics
{
    public partial class TileMeshRendererShader
    {
        private static EffectBytecode bytecode;

        public static EffectBytecode Bytecode
        {
            get
            {
                return bytecode ?? (bytecode = EffectBytecode.FromBytesSafe(binaryBytecode));
            }
        }
    }
}
