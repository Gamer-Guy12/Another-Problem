using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup (typeof (FixedStepSimulationSystemGroup))]
[UpdateAfter (typeof (PhysicsSimulationGroup))]
public partial struct ProjectileSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<ProjectileDataComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new ProjectileJob
        {

        }.Schedule (SystemAPI.GetSingleton<SimulationSingleton> (), state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct ProjectileJob : ITriggerEventsJob
    {

        public void Execute (TriggerEvent triggerEvent)
        {

            UnityEngine.Debug.Log ("Hit");

        }

    }

}
