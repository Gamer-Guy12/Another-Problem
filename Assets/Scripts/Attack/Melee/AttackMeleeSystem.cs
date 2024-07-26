using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct AttackMeleeSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<AttackMeleeDataComponent> ();
        state.RequireForUpdate<TargetComponent> ();
        state.RequireForUpdate<TargetingInfoComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        ComponentLookup<HealthComponent> healthLookup = SystemAPI.GetComponentLookup<HealthComponent> (false);
        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.TempJob);

        JobHandle handle = new AttackJob
        {
            healthLookup = healthLookup,
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecb.AsParallelWriter()
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

        handle.Complete ();

        ecb.Playback (state.EntityManager);
        ecb.Dispose ();

    }

    [BurstCompile]
    public partial struct AttackJob : IJobEntity
    {

        [ReadOnly] public ComponentLookup<HealthComponent> healthLookup;
        [ReadOnly] public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;

        [BurstCompile]
        public void Execute (ref AttackMeleeDataComponent data, in TargetComponent target, in TargetingInfoComponent info, in LocalTransform transform, [ChunkIndexInQuery] int index)
        {

            if (!info.hasTarget) return;
            if (math.distance (transform.Position, new float3(target.target.x, transform.Position.y, target.target.z)) > target.accuracyRadius + target.attackRangeModifier) return;

            data.attackCooldownTimer += deltaTime;

            if (data.attackCooldownTimer < data.attackCooldown) return;
            data.attackCooldownTimer = data.attackCooldown;

            HealthComponent targetHealth = healthLookup[target.targetEntity];

            targetHealth.health -= data.attackStrength;
            ecb.SetComponent (index, target.targetEntity, targetHealth);

        }

    }

}
