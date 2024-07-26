using Unity.Entities;
using Unity.Mathematics;

public struct AttackRangedDataComponent : IComponentData
{

    public float attackStrength;
    public float attackCooldown;
    public float attackRange;

    public Entity projectile;
    public float projectileSpeed;

    public float3 shooterOffset;

    public float attackCooldownTimer;

}
