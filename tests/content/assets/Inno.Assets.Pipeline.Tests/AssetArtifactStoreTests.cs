using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Serialization.DotNet;
using Inno.Content;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Testing.Content;
using Inno.Testing.Diagnostics;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetArtifactStoreTests : AssetLoaderFixture
{
    [Fact]
    public void ContentAddressedStoreDeduplicatesEqualImportsAndCollectsUnreachableBundles()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/first.txt", "same");
        workspace.WriteText("Text/second.txt", "same");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/first.txt"), out AssetInfo? first));
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/second.txt"), out AssetInfo? second));
        Assert.Equal(first!.artifactKey, second!.artifactKey);
        Assert.True(loader.TryGetArtifact(first.persistentID, "runtime", out AssetArtifactInfo? oldArtifact));
        Assert.NotNull(oldArtifact);

        workspace.WriteText("Text/first.txt", "changed");
        Assert.True(loader.Import(AssetPath.Project("Text/first.txt")));
        Assert.True(loader.TryGetArtifact(first.persistentID, "runtime", out AssetArtifactInfo? currentArtifact));
        Assert.NotNull(currentArtifact);
        Assert.NotEqual(oldArtifact.key, currentArtifact.key);
        string oldPath = workspace.FindArtifactFile(oldArtifact);
        Assert.True(File.Exists(oldPath));

        Assert.Equal(0, loader.CollectArtifacts(TimeSpan.Zero, maximumSizeBytes: 0));
        Assert.True(File.Exists(oldPath));
        workspace.DeleteSource("Text/second.txt");
        File.Delete(workspace.SourcePath("Text/second.txt.imeta"));
        loader.Rescan();
        Assert.True(loader.CollectArtifacts(TimeSpan.Zero, maximumSizeBytes: 0) >= 1);
        Assert.False(File.Exists(oldPath));
        Assert.True(File.Exists(workspace.FindArtifactFile(currentArtifact)));
    }

    [Fact]
    public void ArtifactLookupRejectsWrongLengthAndExportRejectsSameLengthCorruption()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/value.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/value.txt"), out AssetInfo? info));
        Assert.True(loader.TryGetArtifact(info!.persistentID, "runtime", out AssetArtifactInfo? artifact));
        string path = workspace.FindArtifactFile(artifact!);
        byte[] original = File.ReadAllBytes(path);

        File.WriteAllBytes(path, [1]);
        Assert.Throws<InvalidDataException>(() => loader.TryGetArtifact(info.persistentID, "runtime", out _));
        byte[] changed = (byte[])original.Clone();
        changed[0] ^= 0x01;
        File.WriteAllBytes(path, changed);
        Assert.True(loader.TryGetArtifact(info.persistentID, "runtime", out _));
        Assert.Throws<InvalidDataException>(() => loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "CorruptedRuntime")));
        File.WriteAllBytes(path, original);
        Assert.True(loader.TryGetArtifact(info.persistentID, "runtime", out _));
        loader.ExportRuntimeArtifacts(Path.Combine(workspace.libraryRoot, "ValidRuntime"));
    }

    [Fact]
    public async Task AggregateBuildProcessorProducesStableContentAddressedOutput()
    {
        using var workspace = new TestWorkspace();
        workspace.WriteText("Text/input.txt", "value");
        using AssetLoader loader = workspace.CreateLoader(m_types, m_serialization, m_identities, m_diagnostics, m_logs);
        loader.Rescan();
        Assert.True(loader.TryGetInfo(AssetPath.Project("Text/input.txt"), out AssetInfo? input));
        Assert.NotNull(input);
        var definition = new TestBuildDefinitionAsset { label = "bundle" };

        AssetArtifactKey first = await loader.BuildAsync(definition, [input]);
        AssetArtifactKey repeated = await loader.BuildAsync(definition, [input]);

        Assert.False(first.isEmpty);
        Assert.Equal(first, repeated);
    }
}
