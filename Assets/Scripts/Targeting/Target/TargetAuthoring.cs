using UnityEngine;
using Unity.Entities;

public class TargetAuthoring : MonoBehaviour
{

    [Header ("Targeting")]

    [SerializeField] float radius;
    [SerializeField] bool infiniteRange;

    [Header ("Health")]

    [SerializeField] float maxHealth;
    [SerializeField] float regenRate;

    [Header ("Team")]

    [SerializeField] TeamEnum team;

    class Baker : Baker<TargetAuthoring>
    {
        public override void Bake (TargetAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new TargetDataComponent
            {
                radius = authoring.radius,
                infiniteRange = authoring.infiniteRange
            });

            AddComponent (entity, new HealthComponent
            {
                maxHealth = authoring.maxHealth,
                health = authoring.maxHealth,
                regenRate = authoring.regenRate
            });

            AddComponent (entity, new TeamComponentData
            {
                team = authoring.team
            });

        }
    }

}
