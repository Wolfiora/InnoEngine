using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Testing.Content;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public abstract class AssetLoaderFixture : IDisposable
{
    protected readonly string m_moduleArtifacts = Path.Combine(
        Path.GetTempPath(),
        "AssetImporterModules-" + Guid.NewGuid().ToString("N")
    );

    protected readonly ModuleHost m_modules;
    protected readonly TypeCatalog m_types;
    protected readonly SerializationRegistry m_serialization;
    protected readonly LogRouter m_logs = new();
    protected readonly IdentityAllocator m_identities = new();
    protected readonly DiagnosticHub m_diagnostics = new();
    private readonly IDisposable m_diagnosticScope;
    private readonly IDisposable m_identityScope;

    protected AssetLoaderFixture()
    {
        _ = typeof(PrivateConstructorAssetImporter);
        _ = typeof(DeferredAssetImporter);
        _ = typeof(TestBuildProcessor);
        DeferredAssetImporter.isAvailable = true;
        ExtensionDependentImporter.available = false;
        ExtensionDependentImporter.attempts = 0;
        m_identityScope = m_identities.EnterScope();
        m_diagnosticScope = m_diagnostics.EnterScope();
        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(AssetLoaderFixture).Assembly)
        });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
        SlowAssetImporter.Reset();
        ImporterConflictProbe.duplicateExtension = false;
        MutableAssetImporter.attempts = 0;
    }

    public void Dispose()
    {
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        m_logs.Dispose();
        m_diagnosticScope.Dispose();
        m_identityScope.Dispose();
        m_modules.generations.Wait();
        if (Directory.Exists(m_moduleArtifacts))
        {
            Directory.Delete(m_moduleArtifacts, recursive: true);
        }
    }

    protected sealed class MountedSourceMetadata : ISerializable
    {
        [SerializableProperty]
        public Guid persistentId { get; set; }

        [SerializableProperty]
        public int sourceKind { get; set; }

        [SerializableProperty]
        public string importerId { get; set; } = string.Empty;

        [SerializableProperty]
        public byte[] importerSettingsBytes { get; set; } = [];
    }

    protected sealed class TestWorkspace : IDisposable
    {
        private readonly string m_root = Path.Combine(
            Path.GetTempPath(),
            "InnoAssetLoaderTests",
            Guid.NewGuid().ToString("N")
        );

        internal TestWorkspace()
        {
            Directory.CreateDirectory(assetRoot);
            Directory.CreateDirectory(libraryRoot);
        }

        public void Dispose()
        {
            SlowAssetImporter.ALLOW_IMPORT.Set();
            if (Directory.Exists(m_root))
            {
                Directory.Delete(m_root, recursive: true);
            }
        }

        internal string assetRoot => Path.Combine(m_root, "Assets");
        internal string libraryRoot => Path.Combine(m_root, "Library");

        internal AssetLoader CreateLoader(
            TypeCatalog types,
            SerializationRegistry serialization,
            IdentityAllocator identities,
            DiagnosticHub diagnostics,
            LogRouter logs
        ) => new(types, serialization, identities, diagnostics, logs, assetRoot, libraryRoot);

        internal string SourcePath(string relativePath) => Path.Combine(assetRoot, relativePath);

        internal string FindArtifactFile(AssetArtifactInfo artifact)
        {
            string key = artifact.key.value;
            string directory = Path.Combine(
                libraryRoot,
                "Artifacts",
                key[..2].ToLowerInvariant(),
                key[2..4].ToLowerInvariant(),
                key,
                "outputs"
            );
            return Directory.EnumerateFiles(directory).Single(path => (
                new FileInfo(path).Length == artifact.length
                && Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))) == artifact.contentHash
            ));
        }

        internal void WriteText(
            string relativePath,
            string content
        ) {
            string path = SourcePath(relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content, new UTF8Encoding(false));
        }

        internal string ReadText(string relativePath) => File.ReadAllText(SourcePath(relativePath));

        internal void Move(
            string oldRelativePath,
            string newRelativePath
        ) {
            string target = SourcePath(newRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Move(SourcePath(oldRelativePath), target);
        }

        internal void DeleteSource(string relativePath)
        {
            string path = SourcePath(relativePath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
