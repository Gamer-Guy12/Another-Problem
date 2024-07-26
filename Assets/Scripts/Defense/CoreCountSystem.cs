using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

public partial struct CoreCountSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<CoreCountComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        EntityQuery query = new EntityQueryBuilder (Allocator.Temp)
            .WithAll<CoreTag> ()
            .Build (ref state);

        NativeArray<CoreTag> tags = query.ToComponentDataArray<CoreTag> (Allocator.Temp);

        CoreCountComponent coreCount = SystemAPI.GetSingleton<CoreCountComponent> ();
        coreCount.count = (uint) tags.Length;

        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.Temp);

        NativeArray<Entity> entities = query.ToEntityArray (Allocator.Temp);

        if (entities.Length <= 0) return;

        ecb.SetComponent (entities[0], coreCount);

    }

}
