using System;
using System.IO;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[CollectionDefinition(NAME)]
public sealed class SceneTestsCollection : ICollectionFixture<SceneTestsFixture>
{
    public const string NAME = nameof(SceneTestsCollection);
}

public sealed class SceneTestsFixture : IDisposable
{
    private readonly string m_cacheDirectory = Path.Combine(
        Path.GetTempPath(),
        "InnoSceneTests",
        Guid.NewGuid().ToString("N")
    );

    public SceneTestsFixture()
    {
        _ = typeof(SceneAsset);
        this.modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(SceneTestsCollection).Assembly)
        });
        this.types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        this.serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        this.world = new SceneWorld(identities, types);
        this.assets = new RejectingAssetReferenceResolver();
        this.serializationContext = AssetSerializationContext.Create(assets);
    }

    public ModuleHost modules { get; }
    public TypeCatalog types { get; }
    public SerializationRegistry serialization { get; }
    public IAssetReferenceResolver assets { get; }
    public IdentityAllocator identities { get; } = new();
    public DiagnosticHub diagnostics { get; } = new();
    public LogRouter logs { get; } = new();
    public SceneWorld world { get; }
    public SerializationContext serializationContext { get; }

    public void Dispose()
    {
        world.Dispose();
        serialization.Dispose();
        types.Dispose();
        modules.Dispose();
        logs.Dispose();
        if (Directory.Exists(m_cacheDirectory))
        {
            Directory.Delete(m_cacheDirectory, recursive: true);
        }
    }

    private sealed class RejectingAssetReferenceResolver : IAssetReferenceResolver
    {
        public AssetObject Resolve(
            Guid persistentId,
            Guid stableTypeId,
            string lastKnownPath,
            Type expectedType,
            string propertyPath
        ) => throw new InvalidOperationException($"Unexpected asset reference '{persistentId:D}' at '{propertyPath}'.");
    }
}
