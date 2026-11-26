using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Benchmark.Scene;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class SceneBenchmarks
{
    private readonly List<GameObject> m_objects = [];
    private SceneHarness m_harness = null!;
    private GameScene m_scene = null!;

    [Params(1_000, 10_000)]
    public int objectCount;

    [GlobalSetup]
    public void Setup()
    {
        m_harness = new SceneHarness();
        m_scene = m_harness.world.LoadNewScene("Benchmark");
        for (int index = 0; index < objectCount; index++)
        {
            GameObject gameObject = m_scene.CreateObject("Object");
            _ = gameObject.AddComponent<BenchmarkBehavior>();
            _ = gameObject.AddComponent<BenchmarkData>();
            m_objects.Add(gameObject);
        }
        m_harness.world.Update(0.016f);
    }

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();

    [Benchmark(Description = "World.Update N behaviors")]
    public void UpdateBehaviors() => m_harness.world.Update(0.016f);

    [Benchmark(Description = "GetComponent<T> over N objects")]
    public float GetComponentAll()
    {
        float sum = 0;
        for (int index = 0; index < m_objects.Count; index++)
        {
            sum += m_objects[index].GetComponent<BenchmarkData>().value;
        }
        return sum;
    }

    [Benchmark(Description = "Transform.localPosition write over N objects")]
    public void WriteTransforms()
    {
        for (int index = 0; index < m_objects.Count; index++)
        {
            m_objects[index].transform.localPosition = new System.Numerics.Vector3(index, 0, 0);
        }
    }
}
