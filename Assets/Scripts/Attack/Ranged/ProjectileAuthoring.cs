using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour
{

    class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake (ProjectileAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new ProjectileDataComponent
            {
                attackStrength = 0
            });

            AddComponent (entity, new MoveToComponent
            {
                forwardPercent = 0,
                moveSpeed = 0,
                directional = true
            });

            AddComponent (entity, new TargetComponent
            {
                accuracyRadius = 0.1f,
                attackRangeModifier = 0,
            });

            AddComponent (entity, new TargetingInfoComponent
            {
                hasTarget = false,
                targetTeam = TeamEnum.Zombie
            });

        }
    }

}
