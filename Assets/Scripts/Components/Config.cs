using Unity.Entities;


// NOTES:

// Need to have some approved mechanism of
// - logging
// - debugging
// to understand the system better.

// Debugging:
// When working with a multithreaded system, knowing what the threads look like 
// in real time is always helpful. Can get this visualisation to some degree from 
// the profiler window, but knowing how to connect to the unity instance
// to view threads, and the code currently executing in them would be good

// Logging:
// Its pretty old school but undeniably usefull, especially when you don't want
// to stop a given thread on a breakpoint while other threads continue executing



// IDEA:
// Would be good to expand on this:
// "Creating our own shaders (via shadergraph) and mapping custom ECS components to their inputs
// is out of scope for this tutorial" using the starting point that the given in this tutorial.


// QUESTION: Get the intellisense in VS working for the types used in the ECS packages (do they not have a DLL + XML somewhere)


struct Config : IComponentData
{
    public Entity TankPrefab;
    public int TankCount;
    public float SafeZoneRadius;
}