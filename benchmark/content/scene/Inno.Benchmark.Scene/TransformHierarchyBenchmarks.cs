using BenchmarkDotNet.Attributes;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Benchmark.Scene;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class TransformHierarchyBenchmarks
{
    private SceneHarness m_harness = null!;
    private Transform m_root = null!;
    private Transform m_leaf = null!;
    private int m_revision;
    private GameScene m_scene = null!;

    [Params(1, 10)]
    public int depth;

    [GlobalSetup(Targets = [nameof(WriteParent), nameof(ReadAfterWrite)])]
    public void Setup()
    {
        m_harness = new SceneHarness();
        GameScene scene = m_harness.world.LoadNewScene("Hierarchy");
        m_root = m_leaf = scene.CreateObject("Root").transform;
        for (int i = 1; i <= depth; i++)
        {
            Transform child = scene.CreateObject("Child").transform;
            child.SetParent(m_leaf);
            m_leaf = child;
        }
    }

    [IterationSetup(Target = nameof(BuildHierarchy))]
    public void EmptyScene()
    {
        m_harness = new SceneHarness();
        m_scene = m_harness.world.LoadNewScene("Build hierarchy");
    }

    [Benchmark]
    public void BuildHierarchy()
    {
        Transform root = m_scene.CreateObject("Root").transform;
        Transform parent = root;
        for (int i = 0; i < 1000; i++)
        {
            Transform child = m_scene.CreateObject("Child").transform;
            child.SetParent(i % depth == 0 ? root : parent);
            parent = child;
        }
    }

    [IterationCleanup(Target = nameof(BuildHierarchy))]
    public void ReleaseHierarchy() => m_harness.Dispose();

    [Benchmark]
    public void WriteParent() => m_root.localPosition = new(++m_revision, 0, 0);

    [IterationSetup(Target = nameof(ReadAfterWrite))]
    public void Invalidate() => WriteParent();

    [Benchmark]
    public float ReadAfterWrite() => m_leaf.worldPosition.x;

    [GlobalCleanup(Targets = [nameof(WriteParent), nameof(ReadAfterWrite)])]
    public void Cleanup() => m_harness.Dispose();
}
