using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
// IJobEntity relies on source generation to implicitly define a query from the signature of the Execute function.
partial struct CannonBallJob : IJobEntity
{
    // A regular EntityCommandBuffer cannot be used in parallel, a ParallelWriter has to be explicitly used.
    public EntityCommandBuffer.ParallelWriter ECB;
    // Time cannot be directly accessed from a job, so DeltaTime has to be passed in as a parameter.
    public float DeltaTime;

    // The ChunkIndexInQuery attributes maps the chunk index to an int parameter.
    // Each chunk can only be processed by a single thread, so those indices are unique to each thread.
    // They are also fully deterministic, regardless of the amounts of parallel processing happening.
    // So those indices are used as a sorting key when recording commands in the EntityCommandBuffer,
    // this way we ensure that the playback of commands is always deterministic.


    // QUESTION: What is a chunk and why do we need its index? A diagram of the above description 
    // would help.

    // ANSWER:
    // A chunk contains a number of entities of a given archtype


    // QUESTION: Is the chunk index the index of the chunk a given entity belongs to? OR
    // is the the chunk index the index of the entity within the chunk?


    void Execute([ChunkIndexInQuery] int chunkIndex, ref CannonBallAspect cannonBall)
    {
        var gravity = new float3(0.0f, -9.82f, 0.0f);
        var invertY = new float3(1.0f, -1.0f, 1.0f);

        cannonBall.Position += cannonBall.Speed * DeltaTime;
        if (cannonBall.Position.y < 0.0f)
        {
            cannonBall.Position *= invertY;
            cannonBall.Speed *= invertY * 0.8f;
        }

        cannonBall.Speed += gravity * DeltaTime;

        var speed = math.lengthsq(cannonBall.Speed);

        // Condition to kill the cannonball
        if (speed < 0.1f) ECB.DestroyEntity(chunkIndex, cannonBall.Self); 
    }
}

/// <summary>
/// Used to move the cannon ball. The logic is done in a seperately defined job that runs in parallel.
/// 
/// QUESTION: How can we see that these jobs are running in parrallel? Via the profiler?
/// </summary>
[BurstCompile]
partial struct CannonBallSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // QUESTION: Are ECB systems always singletons? Or maybe all systems are singletons?
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var cannonBallJob = new CannonBallJob
        {
            // Note the function call required to get a parallel writer for an EntityCommandBuffer.
            ECB = ecb.AsParallelWriter(),
            // Time cannot be directly accessed from a job, so DeltaTime has to be passed in as a parameter.
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        cannonBallJob.ScheduleParallel();
    }
}