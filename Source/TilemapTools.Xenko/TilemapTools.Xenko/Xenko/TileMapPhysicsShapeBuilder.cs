using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Xenko
{
    public abstract class TileMapPhysicsShapeBuilder
    {
        private Dictionary<ShortPoint, List<IInlineColliderShapeDesc>> tileBlockColliderShapes = new Dictionary<ShortPoint, List<IInlineColliderShapeDesc>>();

        private TrackingCollection<IInlineColliderShapeDesc> currentPhysicsComponentColliderShapes;
        private List<IInlineColliderShapeDesc> currentColliderShapes;

        public virtual void RemoveAssociatedColliderShapes(PhysicsTriggerComponentBase physicsComponent)
        {
            if (physicsComponent == null) return;

            foreach (var pair in tileBlockColliderShapes)
            {
                var colliderShapes = pair.Value;
                RemoveAssociatedColliderShapes(physicsComponent, colliderShapes);
            }

        }

        public void Update(TileGrid tileGrid, PhysicsTriggerComponentBase physicsComponent)
        {
            var blocks = tileGrid.InternalBlocks;
            var cellSize = tileGrid.CellSize;

            var changed = false;

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                if(block.PhysicsInvalidated)
                {
                    List<IInlineColliderShapeDesc> colliderShapes = null;

                    if(tileBlockColliderShapes.TryGetValue(block.Location, out colliderShapes))
                    {
                        changed = changed || RemoveAssociatedColliderShapes(physicsComponent, colliderShapes);
                    }
                    else if(!block.IsEmpty)
                    {
                        tileBlockColliderShapes[block.Location] = colliderShapes = new List<IInlineColliderShapeDesc>();

                        currentPhysicsComponentColliderShapes = physicsComponent.ColliderShapes;
                        currentColliderShapes = colliderShapes;

                        Update(block, ref cellSize);

                        currentPhysicsComponentColliderShapes = null;
                        currentColliderShapes = null;
                    }
                    changed = changed || true;
                    block.PhysicsInvalidated = false;
                }
            }

        }

        protected abstract void Update(TileGridBlock block, ref Vector2 cellSize);

        protected void AddTileColliderShape(IInlineColliderShapeDesc colliderShape)
        {
            currentPhysicsComponentColliderShapes.Add(colliderShape);
            currentColliderShapes.Add(colliderShape);
        }

        private static bool RemoveAssociatedColliderShapes(PhysicsTriggerComponentBase physicsComponent, List<IInlineColliderShapeDesc> colliderShapes)
        {
            var changed = false;
            for (int i = 0; i < colliderShapes.Count; i++)
            {
                changed = changed || physicsComponent.ColliderShapes.Remove(colliderShapes[i]);
            }

            colliderShapes.Clear();

            return changed;
        }
    }
}
