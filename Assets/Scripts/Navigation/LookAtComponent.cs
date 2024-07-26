using Unity.Entities;
using Unity.Mathematics;

public struct LookAtComponent : IComponentData
{

    public float rotateSpeed;
    public float acceptableError;

}
