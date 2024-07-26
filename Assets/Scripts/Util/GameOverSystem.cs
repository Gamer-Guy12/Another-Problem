using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;

public partial struct GameOverSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<CoreCountComponent> ();
        state.RequireForUpdate<GameComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new GameOverJob
        {

        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct GameOverJob : IJobEntity
    {

        [BurstCompile]
        public void Execute (in CoreCountComponent data, ref GameComponent game)
        {

            if (data.count > 0) return;

            game.winningTeam = TeamEnum.Zombie;
            game.gameOver = true;

        }

    }

}
