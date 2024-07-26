using Unity.Entities;

public struct AttackMeleeDataComponent : IComponentData
{

    public float attackStrength;
    public float attackCooldown;
    public float attackRange;

    public float attackCooldownTimer;

}
