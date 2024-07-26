using Unity.Entities;
using Unity.Mathematics;

public struct TargetComponent : IComponentData
{

    public float3 target;
    public float accuracyRadius;
    public float attackRangeModifier;

    public Entity targetEntity;
    
}
