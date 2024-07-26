using UnityEngine;
using Unity.Entities;

public class TeamAuthoring : MonoBehaviour
{

    [SerializeField] TeamEnum team;

    class Baker : Baker<TeamAuthoring>
    {
        public override void Bake (TeamAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.None);

            AddComponent (entity, new TeamComponentData
            {
                team = authoring.team
            });

        }
    }

}
