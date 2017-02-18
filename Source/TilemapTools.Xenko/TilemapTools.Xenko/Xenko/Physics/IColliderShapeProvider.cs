using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Xenko.Physics
{
    public interface IColliderShapeProvider
    {
        IInlineColliderShapeDesc CalculateColliderShape(ref Rectangle cellSelection, TileGridBlock block);
    }
}
