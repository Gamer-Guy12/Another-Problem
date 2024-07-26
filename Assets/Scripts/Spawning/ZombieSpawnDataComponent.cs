using Unity.Entities;
using Unity.Mathematics;

public struct ZombieSpawnDataComponent : IComponentData
{

    public Entity prefab;
    public int difficulty;
    public Random random;

    public bool isLinear;

    public float spawnCooldown;
    public float spawnCooldownTimer;
    
}
