using BenchmarkDotNet.Attributes;
using Inno.Scene;

namespace Inno.Benchmark.Scene;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class SceneChurnPhaseBenchmarks
{
    private SceneHarness m_harness = null!;
    private GameScene m_scene = null!;

    [Params(250, 1_000, 4_000)]
    public int objectCount;

    [Params("object", "data", "behavior")]
    public string composition = string.Empty;

    [GlobalSetup]
    public void Setup() => m_harness = new SceneHarness();

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();

    [IterationSetup(Target = nameof(Create))]
    public void SetupCreate() => m_scene = m_harness.world.LoadNewScene("Create");

    [IterationCleanup(Target = nameof(Create))]
    public void CleanupCreate() => m_harness.world.UnloadAllScenes();

    [Benchmark(Description = "Create populated scene objects")]
    public void Create() => Populate(m_scene, objectCount);

    [IterationSetup(Target = nameof(Unload))]
    public void SetupUnload() => Populate(m_harness.world.LoadNewScene("Unload"), objectCount);

    [Benchmark(Description = "Unload populated scene")]
    public void Unload() => m_harness.world.UnloadAllScenes();

    private void Populate(
        GameScene scene,
        int count
    ) {
        for (int index = 0; index < count; index++)
        {
            GameObject gameObject = scene.CreateObject("Object");
            if (composition == "behavior") gameObject.AddComponent<BenchmarkBehavior>();
            if (composition == "data") gameObject.AddComponent<BenchmarkData>();
        }
    }
}
