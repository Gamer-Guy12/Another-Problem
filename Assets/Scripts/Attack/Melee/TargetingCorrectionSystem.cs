using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

public partial struct TargetingCorrectionSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<TargetComponent> ();
        state.RequireForUpdate<AttackMeleeDataComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {
        
        JobHandle handle = new TargetingCorrectionJob
        {

        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct TargetingCorrectionJob : IJobEntity
    {

        [BurstCompile]
        public void Execute (ref TargetComponent targeting, in AttackMeleeDataComponent data)
        {

            targeting.attackRangeModifier = data.attackRange;

        }

    }

}
