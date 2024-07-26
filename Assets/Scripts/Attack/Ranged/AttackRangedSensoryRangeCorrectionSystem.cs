using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;

public partial struct AttackRangedSensoryRangeCorrectionSystem : ISystem, ISystemStartStop
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<TargetingDataComponent> ();
        state.RequireForUpdate<AttackRangedDataComponent> ();

    }

    [BurstCompile]
    public void OnStartRunning (ref SystemState state)
    {

        JobHandle handle = new AttackRangedSensoryRangeCorrectionJob
        {

        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public void OnStopRunning (ref SystemState state)
    {

        return;

    }

    [BurstCompile]
    public partial struct AttackRangedSensoryRangeCorrectionJob : IJobEntity
    {

        [BurstCompile]
        public void Execute (ref TargetingDataComponent data, in AttackRangedDataComponent attackData)
        {

            data.sensoryRange = attackData.attackRange;

        }

    }

}
