using UnityEngine;
using Unity.Transforms;
using Unity.Entities;

public class ParentAuthoring : MonoBehaviour
{

    [SerializeField] GameObject parent;

    class Baker : Baker<ParentAuthoring>
    {
        public override void Bake (ParentAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            Entity parent = GetEntity(authoring.parent, TransformUsageFlags.Dynamic);

            AddComponent (entity, new Parent
            {
                Value = parent
            });

        }
    }

}
