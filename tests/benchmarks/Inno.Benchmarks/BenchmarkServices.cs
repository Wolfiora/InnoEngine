using System;
using System.IO;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Graph;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Plugins.Authoring;

namespace Inno.Benchmarks;

/// <summary>
/// Owns one real module, type, serialization, identity, logging and diagnostic service scope.
/// </summary>
public sealed class BenchmarkServices : IDisposable
{
    private readonly IDisposable m_identityScope;

    public BenchmarkServices()
    {
        _ = typeof(AssetPipeline);
        _ = typeof(TextAsset);
        _ = typeof(PluginSourceService);
        _ = typeof(GraphEditorModule);
        this.identities = new IdentityAllocator();
        m_identityScope = identities.EnterScope();
        this.modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(BenchmarkServices).Assembly) }
        );
        this.types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        this.serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
    }

    public ModuleHost modules { get; }
    public TypeCatalog types { get; }
    public SerializationRegistry serialization { get; }
    public IdentityAllocator identities { get; }
    public LogRouter logs { get; } = new();
    public DiagnosticHub diagnostics { get; } = new();

    public void Dispose()
    {
        serialization.Dispose();
        types.Dispose();
        modules.Dispose();
        logs.Dispose();
        m_identityScope.Dispose();
    }
}
