using Unity.Entities;

public struct HealthComponent : IComponentData
{

    public float maxHealth;
    public float health;
    public float regenRate;
    
}
