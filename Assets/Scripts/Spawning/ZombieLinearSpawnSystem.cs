using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using static UnityEditor.Progress;

public partial struct ZombieLinearSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<ZombieSpawnDataBuffer> ();
        state.RequireForUpdate<ZombieSpawnDataComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        EntityManager manager = state.EntityManager;
        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.Temp);

        DynamicBuffer<ZombieSpawnDataBuffer> buffer = SystemAPI.GetSingletonBuffer<ZombieSpawnDataBuffer> ();
        ZombieSpawnDataComponent data = SystemAPI.GetSingleton<ZombieSpawnDataComponent> ();
        Random random = data.random;

        if (!data.isLinear) return;

        // Timer
        foreach (RefRW<ZombieSpawnDataComponent> timer in SystemAPI.Query<RefRW<ZombieSpawnDataComponent>> ())
        {

            timer.ValueRW.spawnCooldownTimer = timer.ValueRO.spawnCooldownTimer + SystemAPI.Time.DeltaTime;

            if (timer.ValueRO.spawnCooldownTimer < timer.ValueRO.spawnCooldown) return;

            timer.ValueRW.spawnCooldownTimer = 0f;

        }

        ZombieSpawnDataBuffer item = buffer[random.NextInt (0, buffer.Length - 1)];

        // Spawn Zombies
        LocalTransform transform = manager.GetComponentData<LocalTransform> (data.prefab);
        transform.Position = item.pos;
        transform.Rotation = item.rot;
        ecb.SetComponent (data.prefab, transform);
        Entity entity = ecb.Instantiate (data.prefab);

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

        TargetingDataComponent targetingData = manager.GetComponentData<TargetingDataComponent> (data.prefab);
        targetingData.sensoryRange = zombieData.attackSensoryRange;
        ecb.SetComponent (entity, targetingData);

        // Set Random
        foreach (RefRW<ZombieSpawnDataComponent> setRandom in SystemAPI.Query<RefRW<ZombieSpawnDataComponent>> ())
        {

            setRandom.ValueRW.random = random;

        }

        ecb.Playback (manager);

        ecb.Dispose ();

    }

}
