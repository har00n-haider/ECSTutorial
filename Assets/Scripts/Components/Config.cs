using Unity.Entities;

// NOTE:
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
// basically is there a threadsafe Debug.Log that can be used?

// IDEA:
// Would be good to expand on this:
// "Creating our own shaders (via shadergraph) and mapping custom ECS components to their inputs
// is out of scope for this tutorial" using the starting point that the given in this tutorial.

// QUESTION: How to get the intellisense in VS working for the types used in the ECS packages
// (is there DLL + XML somewhere that is not reference by the project)

// PROPS: Explicitly namespacing the non ECS types is very hand in differentiating the packages being used

// NOTE:
// There appear to be a alot of "should nots" that should be "cannots" if certain workflows are to be enforced 
// (runtime error at least)
// e.g. Creating local querie members before Update()

// QUESTION:
// What is live baking? What other types of baking are there?

struct Config : IComponentData
{
    public Entity TankPrefab;
    public int TankCount;
    public float SafeZoneRadius;
}