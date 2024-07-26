using Unity.Entities;

public struct TargetDataComponent : IComponentData
{

    public float radius;
    // Can be targeted from an infinite distance away
    public bool infiniteRange;

}
