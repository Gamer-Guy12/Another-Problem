using Unity.Entities;
using Unity.Mathematics;

public struct MoveToComponent : IComponentData
{

    public float moveSpeed;
    // What percent of movement is forward, 1 - forwardPercent is how much is toward the target
    public float forwardPercent;
    // Whether it should move flat or move up and down
    public bool directional;

}
