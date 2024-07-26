using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;

public partial struct AttackRangedSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<AttackRangedDataComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.TempJob);

        JobHandle handle = new AttackRangedJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecb.AsParallelWriter ()
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

        state.Dependency.Complete ();

        ecb.Playback (state.EntityManager);
        ecb.Dispose ();

    }

    [BurstCompile]
    public partial struct AttackRangedJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb;

        [BurstCompile]
        public void Execute (ref AttackRangedDataComponent data, in TargetComponent target, in TargetingInfoComponent targetInfo, in LocalTransform transform, [EntityIndexInQuery] int index)
        {

            data.attackCooldownTimer += deltaTime;

            if (data.attackCooldownTimer < data.attackCooldown || !targetInfo.hasTarget) return;

            data.attackCooldownTimer = 0f;

            Entity projectile = ecb.Instantiate (index, data.projectile);

            LocalTransform projectileTransform = transform;
            projectileTransform.Position += data.shooterOffset;
            ecb.SetComponent (index, projectile, projectileTransform);

            MoveToComponent projectileMoveTo = new MoveToComponent
            {
                forwardPercent = 0f,
                moveSpeed = data.projectileSpeed,
                directional = true
            };
            ecb.SetComponent (index, projectile, projectileMoveTo);

            ecb.SetComponent (index, projectile, target);

            ProjectileDataComponent projectileData = new ProjectileDataComponent
            {
                attackStrength = data.attackStrength,
            };
            ecb.SetComponent (index, projectile, projectileData);

        }

    }

}
