using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Benchmark.Engine;

[StableTypeID("6f3d0c0e-6a55-4d0b-9a0f-3b8c2c5c1a01")]
public sealed class BenchmarkBehavior : GameBehavior
{
    public int updates;

    protected override void Update() => updates++;
}

[StableTypeID("6f3d0c0e-6a55-4d0b-9a0f-3b8c2c5c1a02")]
public sealed class BenchmarkData : GameComponent
{
    public float value;
}

public sealed class SceneHarness : IDisposable
{
    private readonly IDisposable m_scope;

    public SceneHarness()
    {
        modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(SceneHarness).Assembly)
        });
        types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        world = new SceneWorld(identities, types);
        m_scope = world.EnterScope();
        serializationContext = AssetSerializationContext.Create(new RejectingAssetReferenceResolver());
    }

    public SerializationContext serializationContext { get; }

    public ModuleHost modules { get; }
    public TypeCatalog types { get; }
    public SerializationRegistry serialization { get; }
    public IdentityAllocator identities { get; } = new();
    public SceneWorld world { get; }

    public void Dispose()
    {
        m_scope.Dispose();
        world.Dispose();
        serialization.Dispose();
        types.Dispose();
        modules.Dispose();
    }

    private sealed class RejectingAssetReferenceResolver : IAssetReferenceResolver
    {
        public AssetObject Resolve(
            Guid persistentID,
            Guid stableTypeID,
            string lastKnownPath,
            Type expectedType,
            string propertyPath
        ) => throw new InvalidOperationException($"Unexpected asset reference '{persistentID:D}' at '{propertyPath}'.");
    }
}

[MemoryDiagnoser]
public class SceneBenchmarks
{
    private SceneHarness m_harness = null!;
    private GameScene m_scene = null!;
    private readonly List<GameObject> m_objects = [];

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

[MemoryDiagnoser]
public class SceneLifecycleBenchmarks
{
    private SceneHarness m_harness = null!;

    [Params(250, 1_000, 4_000)]
    public int objectCount;

    [GlobalSetup]
    public void Setup() => m_harness = new SceneHarness();

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();

    [Benchmark(Description = "Create N objects (+2 components), update, unload")]
    public void CreateUpdateUnload()
    {
        GameScene scene = m_harness.world.LoadNewScene("Churn");
        for (int index = 0; index < objectCount; index++)
        {
            GameObject gameObject = scene.CreateObject("Object");
            _ = gameObject.AddComponent<BenchmarkBehavior>();
            _ = gameObject.AddComponent<BenchmarkData>();
        }
        m_harness.world.Update(0.016f);
        m_harness.world.UnloadAllScenes();
    }

    [Benchmark(Description = "Build N-deep hierarchy chains (SetParent)")]
    public void Hierarchy()
    {
        GameScene scene = m_harness.world.LoadNewScene("Hierarchy");
        Transform? parent = null;
        for (int index = 0; index < objectCount; index++)
        {
            Transform child = scene.CreateObject("Node").transform;
            if (index % 10 != 0)
            {
                child.SetParent(parent);
            }
            parent = child;
        }
        m_harness.world.UnloadAllScenes();
    }

    [Benchmark(Description = "Serialize+Deserialize scene with N objects")]
    public int SerializeRoundTrip()
    {
        GameScene scene = m_harness.world.LoadNewScene("Serialize");
        for (int index = 0; index < objectCount; index++)
        {
            _ = scene.CreateObject("Object").AddComponent<BenchmarkData>();
        }
        byte[] bytes = m_harness.serialization.Serialize(scene, m_harness.serializationContext);
        m_harness.world.UnloadAllScenes();
        GameScene restored = m_harness.serialization.Deserialize<GameScene>(bytes, m_harness.serializationContext);
        m_harness.world.LoadScene(restored);
        int restoredCount = restored.GetObjects().Count;
        m_harness.world.UnloadAllScenes();
        return restoredCount;
    }
}
