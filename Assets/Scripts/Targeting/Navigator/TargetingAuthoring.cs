using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class TargetingAuthoring : MonoBehaviour
{

    [SerializeField] float sensoryRange;

    class Baker : Baker<TargetingAuthoring>
    {
        public override void Bake (TargetingAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new TargetingDataComponent
            {
                sensoryRange = authoring.sensoryRange,
            });

            AddComponent (entity, new TargetingInfoComponent
            {
                hasTarget = false,
                targetTeam = TeamEnum.Zombie
            });

            AddComponent (entity, new TargetComponent
            {
                target = new float3 (0, 0, 0),
                accuracyRadius = 3
            });

            AddBuffer<TargetingChoicesBuffer> (entity);

        }
    }

}
