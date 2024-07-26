using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public partial struct MoveToSystem : ISystem
{

    [BurstCompile]
    public void OnStart (ref SystemState state)
    {

        state.RequireForUpdate<MoveToComponent> ();
        state.RequireForUpdate<TargetComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new MoveToJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel (state.Dependency);

        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct MoveToJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref LocalTransform transform, in MoveToComponent data, in TargetComponent target, ref TargetingInfoComponent targetInfo)
        {

            float3 flatTar = new float3 (target.target.x, transform.Position.y, target.target.z);

            if (data.directional) flatTar = target.target;

            if (math.distance (flatTar, transform.Position) < target.accuracyRadius + target.attackRangeModifier)
            {

                targetInfo.atTarget = true;

                return;

            }

            float3 dir = math.normalize (flatTar - transform.Position);
            float3 dirChange = dir * data.moveSpeed * deltaTime;

            float3 forwardChange = transform.Forward () * data.moveSpeed * deltaTime;

            float3 change = forwardChange * data.forwardPercent + dirChange * (1 - data.forwardPercent);

            transform = transform.Translate (change);

        }

    }

}
