using UnityEngine;
using Unity.Entities;

[RequireComponent (typeof (TargetAuthoring))]
public class NavigationAuthoring : MonoBehaviour
{

    [Header ("Move To")]
    [SerializeField] float moveSpeed;
    [SerializeField] float forwardPercent;
    [SerializeField] bool directional;

    [Header ("Look At")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float acceptableError;

    class Baker : Baker<NavigationAuthoring>
    {
        public override void Bake (NavigationAuthoring authoring)
        {
            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new MoveToComponent
            {
                moveSpeed = authoring.moveSpeed,
                forwardPercent = authoring.forwardPercent,
                directional = authoring.directional,
            });

            AddComponent (entity, new LookAtComponent
            {
                rotateSpeed = authoring.rotateSpeed,
                acceptableError = authoring.acceptableError
            });
        }
    }

}
