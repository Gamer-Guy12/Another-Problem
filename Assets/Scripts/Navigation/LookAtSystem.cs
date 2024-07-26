using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;

public partial struct LookAtSystem : ISystem
{

    [BurstCompile]
    public void OnStart (ref SystemState state)
    {

        state.RequireForUpdate<LookAtComponent> ();
        state.RequireForUpdate<TargetComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new LookAtJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct LookAtJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref LocalTransform transform, in LookAtComponent data, in TargetComponent target)
        {

            float3 flatTar = new float3 (target.target.x, transform.Position.y, target.target.z);
            float3 dir = math.normalize (flatTar - transform.Position);

            if (math.dot (dir, transform.Forward ()) >= 1 - data.acceptableError) return;

            float oldAngle = math.angle (quaternion.LookRotation (dir, math.up ()), quaternion.LookRotation (transform.Forward (), math.up ()));
            LocalTransform temp = transform.RotateY (data.rotateSpeed * deltaTime);

            if (math.angle (quaternion.LookRotation (dir, math.up ()), quaternion.LookRotation (temp.Forward (), math.up ())) < oldAngle) transform = transform.RotateY (data.rotateSpeed * deltaTime);
            else transform = transform.RotateY (-data.rotateSpeed * deltaTime);

        }

    }

}
