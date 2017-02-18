using System.Collections.Generic;
using SiliconStudio.Core.Collections;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Xenko.Physics
{
    public class PhysicsShapeBuilderContext
    {
        internal TrackingCollection<IInlineColliderShapeDesc> PhysicsComponentColliderShapes;
        internal List<IInlineColliderShapeDesc> ColliderShapes;

        public IColliderShapeProvider ColliderShapeProvider { get; internal set; }

        public void AddColliderShape(IInlineColliderShapeDesc colliderShape)
        {
            PhysicsComponentColliderShapes.Add(colliderShape);
            ColliderShapes.Add(colliderShape);
        }
    }
}