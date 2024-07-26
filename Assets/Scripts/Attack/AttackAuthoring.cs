using UnityEngine;
using Unity.Entities;

public class AttackAuthoring : MonoBehaviour
{

    [SerializeField] bool ranged;

    [Header ("General")]
    [SerializeField] float attackCooldown;
    [SerializeField] float attackStrength;
    [SerializeField] float attackRange;

    [Header ("Ranged")]
    [SerializeField] Transform shooter;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;

    class Baker : Baker<AttackAuthoring>
    {
        public override void Bake (AttackAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            if (!authoring.ranged)
            {

                AddComponent (entity, new AttackMeleeDataComponent
                {
                    attackCooldown = authoring.attackCooldown,
                    attackStrength = authoring.attackStrength,
                    attackRange = authoring.attackRange
                });

            }
            else
            {

                Entity entityProjectile = GetEntity (authoring.projectile, TransformUsageFlags.Dynamic);

                AddComponent (entity, new AttackRangedDataComponent
                {
                    attackCooldown = authoring.attackCooldown,
                    attackRange = authoring.attackRange,
                    attackStrength = authoring.attackStrength,
                    shooterOffset = authoring.shooter.localPosition,
                    projectile = entityProjectile,
                    projectileSpeed = authoring.projectileSpeed
                });

            }

        }
    }

}
