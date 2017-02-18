using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Physics;
using TilemapTools.Xenko.Physics;

namespace TilemapTools.Xenko
{
    public class TileMapProcessor : EntityProcessor<TileMapComponent, TileMapProcessor.TileMapInfo>
    {
        protected override TileMapInfo GenerateComponentData(Entity entity, TileMapComponent component)
        {
            var physicsComponent = entity.Get<PhysicsTriggerComponentBase>();
            var isValidPhysics = physicsComponent is StaticColliderComponent || physicsComponent is RigidbodyComponent;

            var info = new TileMapInfo()
            {
                TileMapComponent = component,
                TileGrid = component.Grid,
                PhysicsComponent = isValidPhysics ? physicsComponent : null,
                PhysicsShapeBuilder = component.PhysicsShapeBuilder,
            };

            return info;
        }

        public override void Update(GameTime time)
        {
            foreach (var tileMapDataPair in ComponentDatas)
            {
                var tileMapInfo = tileMapDataPair.Value;

                if (tileMapInfo.PhysicsComponent == null || tileMapInfo.PhysicsShapeBuilder == null || tileMapInfo.TileGrid == null)
                    continue;

                tileMapInfo.PhysicsShapeBuilder?.Update(tileMapInfo.TileGrid,tileMapInfo.PhysicsComponent);

            }
        }

        protected override bool IsAssociatedDataValid(Entity entity, TileMapComponent component, TileMapInfo associatedData)
        {
            return associatedData.TileMapComponent == component &&
                   associatedData.TileGrid == component.Grid &&
                   associatedData.PhysicsShapeBuilder == component.PhysicsShapeBuilder &&
                   associatedData.PhysicsComponent == entity.Get<PhysicsTriggerComponentBase>();
        }
        
        protected override void OnEntityComponentRemoved(Entity entity, TileMapComponent component, TileMapInfo data)
        {
            base.OnEntityComponentRemoved(entity, component, data);

            data.PhysicsShapeBuilder?.RemoveAssociatedColliderShapes(data.PhysicsComponent);
        }

        public class TileMapInfo
        {
            public PhysicsTriggerComponentBase PhysicsComponent;
            public TileMapComponent TileMapComponent;
            public PhysicsShapeBuilder PhysicsShapeBuilder;
            public TileGrid TileGrid;
        }
    }
}