using Unity.Entities;

public struct GameComponent : IComponentData
{

    public bool gameOver;
    public TeamEnum winningTeam;
    
}
