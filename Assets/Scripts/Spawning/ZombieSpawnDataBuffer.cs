using Unity.Entities;
using Unity.Mathematics;

public struct ZombieSpawnDataBuffer : IBufferElementData
{

    public float3 pos;
    public quaternion rot;

}
