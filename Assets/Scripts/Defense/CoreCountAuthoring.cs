using UnityEngine;
using Unity.Entities;

public class CoreCountAuthoring : MonoBehaviour
{

    class Baker : Baker<CoreCountAuthoring>
    {
        public override void Bake (CoreCountAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.None);

            AddComponent (entity, new CoreCountComponent
            {
                count = 0
            });

        }
    }

}
