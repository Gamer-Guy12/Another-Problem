using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;

public partial struct RegenSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<HealthComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new RegenJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct RegenJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref HealthComponent data)
        {

            data.health += data.regenRate * deltaTime;

            if (data.health > data.maxHealth) data.health = data.maxHealth;

        }

    }

}
