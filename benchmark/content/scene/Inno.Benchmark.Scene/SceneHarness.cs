using System;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Benchmark.Scene;

public sealed class SceneHarness : IDisposable
{
    private readonly IDisposable m_scope;

    public SceneHarness()
    {
        this.modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(SceneHarness).Assembly)
        });
        this.types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        this.serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        this.world = new SceneWorld(identities, types);
        m_scope = world.EnterScope();
        this.serializationContext = AssetSerializationContext.Create(new RejectingAssetReferenceResolver());
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
