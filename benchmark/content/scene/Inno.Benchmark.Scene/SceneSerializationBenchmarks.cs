using BenchmarkDotNet.Attributes;
using Inno.Scene;

namespace Inno.Benchmark.Scene;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class SceneSerializationBenchmarks
{
    private SceneHarness m_harness = null!;
    private GameScene m_scene = null!;
    private byte[] m_bytes = [];
    private GameScene? m_restored;

    [Params(250, 1000, 4000)]
    public int objectCount;

    [GlobalSetup(Target = nameof(Serialize))]
    public void Setup()
    {
        m_harness = new SceneHarness();
        m_scene = m_harness.world.LoadNewScene("Serialize");
        for (int i = 0; i < objectCount; i++)
        {
            m_scene.CreateObject("Object").AddComponent<BenchmarkData>();
        }
    }

    [GlobalSetup(Target = nameof(Deserialize))]
    public void PrepareBytes()
    {
        Setup();
        m_bytes = Serialize();
        m_harness.world.UnloadAllScenes();
    }

    [Benchmark]
    public byte[] Serialize() => m_harness.serialization.Serialize(m_scene, m_harness.serializationContext);

    [Benchmark]
    public GameScene Deserialize() => m_restored = m_harness.serialization.Deserialize<GameScene>(m_bytes, m_harness.serializationContext);

    [IterationCleanup(Target = nameof(Deserialize))]
    public void ReleaseRestored()
    {
        if (m_restored is null) return;
        m_harness.world.LoadScene(m_restored);
        m_harness.world.UnloadAllScenes();
        m_restored = null;
    }

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();
}
