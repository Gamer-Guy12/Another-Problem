using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct ZombieGroupSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<ZombieSpawnDataBuffer> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        EntityManager manager = state.EntityManager;
        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.Temp);

        DynamicBuffer<ZombieSpawnDataBuffer> buffer = SystemAPI.GetSingletonBuffer<ZombieSpawnDataBuffer> ();
        ZombieSpawnDataComponent data = SystemAPI.GetSingleton<ZombieSpawnDataComponent> ();
        Random random = data.random;

        if (data.isLinear) return;

        foreach (ZombieSpawnDataBuffer item in buffer)
        {

            // Spawn Zombies
            Entity entity = ecb.Instantiate (data.prefab);
            LocalTransform transform = manager.GetComponentData<LocalTransform> (data.prefab);
            transform.Position = item.pos;
            transform.Rotation = item.rot;
            ecb.SetComponent (entity, transform);

            // Get Random Data
            RandomZombieData zombieData = RandomZombieData.CalculateRandomData (ref random, data.difficulty);

            MoveToComponent moveTo = manager.GetComponentData<MoveToComponent> (data.prefab);
            moveTo.moveSpeed = zombieData.moveSpeed;
            ecb.SetComponent (entity, moveTo);

            LookAtComponent lookAt = manager.GetComponentData<LookAtComponent> (data.prefab);
            lookAt.rotateSpeed = zombieData.rotationSpeed;
            ecb.SetComponent (entity, lookAt);

            AttackMeleeDataComponent attackData = manager.GetComponentData<AttackMeleeDataComponent> (data.prefab);
            attackData.attackStrength = zombieData.attackStrength;
            attackData.attackCooldown = zombieData.attackCooldown;
            attackData.attackRange = zombieData.attackRange;
            ecb.SetComponent (entity, attackData);

            HealthComponent health = manager.GetComponentData<HealthComponent> (data.prefab);
            health.health = zombieData.health;
            health.regenRate = zombieData.regenRate;
            ecb.SetComponent (entity, health);

            TargetingDataComponent targetingData = manager.GetComponentData<TargetingDataComponent> (data.prefab);
            targetingData.sensoryRange = zombieData.attackSensoryRange;
            ecb.SetComponent (entity, targetingData);

        }

        // Clear Data
        EntityQuery query = new EntityQueryBuilder (Allocator.Temp).WithAll<ZombieSpawnDataBuffer> ().Build (state.EntityManager);
        NativeArray<Entity> entities = query.ToEntityArray (Allocator.Temp);

        foreach (Entity entity in entities)
        {

            manager.RemoveComponent (entity, ComponentType.ReadWrite<ZombieSpawnDataBuffer> ());

        }

        ecb.Playback (manager);

        ecb.Dispose ();
        query.Dispose ();
        entities.Dispose ();

    }

}
