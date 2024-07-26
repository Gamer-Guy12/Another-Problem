using UnityEngine;
using Unity.Entities;

public class CoreTagAuthoring : MonoBehaviour
{

    class Baker : Baker<CoreTagAuthoring>
    {
        public override void Bake (CoreTagAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Renderable);

            AddComponent (entity, new CoreTag { });

        }
    }

}

public struct CoreTag : IComponentData { }
