using Unity.Entities;

public struct TargetingInfoComponent : IComponentData
{

    public bool hasTarget;
    public TeamEnum targetTeam;
    public bool atTarget;
    
}
