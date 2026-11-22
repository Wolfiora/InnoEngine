using BenchmarkDotNet.Attributes;
using Inno.Scene;

namespace Inno.Benchmarks;

[MemoryDiagnoser]
public class SceneChurnPhaseBenchmarks
{
    private SceneHarness m_harness = null!;
    private GameScene m_scene = null!;

    [Params(250, 1_000, 4_000)]
    public int objectCount;

    [GlobalSetup]
    public void Setup() => m_harness = new SceneHarness();

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();

    [IterationSetup(Target = nameof(Create))]
    public void SetupCreate() => m_scene = m_harness.world.LoadNewScene("Create");

    [IterationCleanup(Target = nameof(Create))]
    public void CleanupCreate() => m_harness.world.UnloadAllScenes();

    [Benchmark(Description = "Create N objects (+2 components) only")]
    public void Create() => Populate(m_scene, objectCount);

    [IterationSetup(Target = nameof(Unload))]
    public void SetupUnload() => Populate(m_harness.world.LoadNewScene("Unload"), objectCount);

    [Benchmark(Description = "Unload scene with N objects (+2 components) only")]
    public void Unload() => m_harness.world.UnloadAllScenes();

    private static void Populate(GameScene scene, int count)
    {
        for (int index = 0; index < count; index++)
        {
            GameObject gameObject = scene.CreateObject("Object");
            _ = gameObject.AddComponent<BenchmarkBehavior>();
            _ = gameObject.AddComponent<BenchmarkData>();
        }
    }
}
