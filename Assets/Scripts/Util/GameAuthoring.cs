using UnityEngine;
using Unity.Entities;

public class GameAuthoring : MonoBehaviour
{

    class Baker : Baker<GameAuthoring>
    {
        public override void Bake (GameAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.None);

            AddComponent (entity, new GameComponent
            {
                gameOver = false,
                winningTeam = TeamEnum.Zombie
            });

        }
    }

}
