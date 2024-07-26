using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using static UnityEngine.GraphicsBuffer;

public partial struct TargetingSystem : ISystem
{

    EntityQuery query;

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<TargetingDataComponent> ();
        state.RequireForUpdate<TargetComponent> ();
        state.RequireForUpdate<TeamComponentData> ();

        query = new EntityQueryBuilder (Allocator.Temp)
            .WithAll<LocalTransform, TargetDataComponent, TeamComponentData> ()
            .Build (ref state);

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        NativeArray<TargetDataComponent> targets = query.ToComponentDataArray<TargetDataComponent> (Allocator.TempJob);
        NativeArray<LocalTransform> transforms = query.ToComponentDataArray<LocalTransform> (Allocator.TempJob);
        NativeArray<TeamComponentData> teams = query.ToComponentDataArray<TeamComponentData> (Allocator.TempJob);
        NativeArray<Entity> entities = query.ToEntityArray (Allocator.TempJob);

        JobHandle handle = new TargetingJob
        {

            targets = targets,
            transforms = transforms,
            teams = teams,
            entities = entities

        }.ScheduleParallel (state.Dependency);
        JobHandle disposeHandle = targets.Dispose (handle);
        JobHandle disposeHandle2 = transforms.Dispose (disposeHandle);
        JobHandle disposeHandle3 = teams.Dispose (disposeHandle2);
        state.Dependency = entities.Dispose (disposeHandle3);

    }

    [BurstCompile]
    public partial struct TargetingJob : IJobEntity
    {

        [ReadOnly] public NativeArray<TargetDataComponent> targets;
        [ReadOnly] public NativeArray<LocalTransform> transforms;
        [ReadOnly] public NativeArray<TeamComponentData> teams;
        [ReadOnly] public NativeArray<Entity> entities;

        [BurstCompile]
        public void Execute (in LocalTransform transform, in TargetingDataComponent data, ref DynamicBuffer<TargetingChoicesBuffer> choices, ref TargetComponent target, in TeamComponentData team, ref TargetingInfoComponent info)
        {

            if (targets.Length <= 0) return;

            choices.Clear ();

            for (int i = 0; i < targets.Length; i++)
            {

                float distance = math.distance (transforms[i].Position, transform.Position);

                choices.Add (new TargetingChoicesBuffer
                {
                    distance = distance,
                });

            }

            int best = -1;

            for (int i = 0; i < choices.Length; i++)
            {

                if (best == -1 && team.team != teams[i].team && choices[i].distance > 0)
                {

                    best = i;
                    continue;

                }

                if (best == -1) continue;

                if (choices[i].distance < choices[best].distance && team.team != teams[i].team && choices[i].distance > 0)
                {

                    best = i;

                }

            }

            if (best == -1)
            {

                info.hasTarget = false;

                return;

            }

            if (math.distance (transforms[best].Position, transform.Position) <= data.sensoryRange)
            {

                target.target = transforms[best].Position;
                target.accuracyRadius = targets[best].radius;
                target.targetEntity = entities[best];

                info.atTarget = false;
                info.hasTarget = true;
                info.targetTeam = teams[best].team;

            }
            else
            {

                best = -1;

                for (int i = 0; i < choices.Length; i++)
                {

                    if (best == -1 && team.team != teams[i].team && choices[i].distance > 0 && targets[i].infiniteRange)
                    {

                        best = i;
                        continue;

                    }

                    if (best == -1) continue;

                    if (choices[i].distance < choices[best].distance && team.team != teams[i].team && choices[i].distance > 0 && targets[i].infiniteRange)
                    {

                        best = i;

                    }

                }

                if (best == -1)
                {

                    info.hasTarget = false;

                    return;

                }

                target.target = transforms[best].Position;
                target.accuracyRadius = targets[best].radius;
                target.targetEntity = entities[best];

                info.atTarget = false;
                info.hasTarget = true;
                info.targetTeam = teams[best].team;

            }

        }

    }

}
