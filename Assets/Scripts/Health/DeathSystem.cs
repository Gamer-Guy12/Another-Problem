using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;

public partial struct DeathSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<HealthComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        BufferLookup<Child> childrenLookup = SystemAPI.GetBufferLookup<Child> (true);
        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.TempJob);

        foreach ((RefRO<HealthComponent> data, Entity entity) in SystemAPI.Query<RefRO<HealthComponent>> ().WithEntityAccess ())
        {

            if (data.ValueRO.health > 0) continue;

            DestroyEntity (entity, childrenLookup, ecb);

        }

        ecb.Playback (state.EntityManager);
        ecb.Dispose ();

    }

    [BurstCompile]
    public void DestroyEntity (Entity entity, BufferLookup<Child> childrenLookup, EntityCommandBuffer ecb)
    {

        if (!childrenLookup.HasBuffer (entity))
        {

            ecb.DestroyEntity (entity);
            return;

        }
        else if (childrenLookup[entity].Length <= 0)
        {

            ecb.DestroyEntity (entity);
            return;

        }

        foreach (Child child in childrenLookup[entity])
        {

            DestroyEntity (child.Value, childrenLookup, ecb);

        }

    }

}
