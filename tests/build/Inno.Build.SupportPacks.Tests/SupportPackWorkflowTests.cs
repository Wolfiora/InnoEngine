using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Inno.Adapter.Content.FileSystem;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Build.Managed;
using Inno.Content;
using Inno.Core.IO;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.MacOSArm64;
using Inno.Build.WindowsX64;
using Inno.Core.Identity;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;




using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;

using Xunit;
using Inno.Build;
using Inno.Testing.Build;

namespace Inno.Build.SupportPacks.Tests;

public sealed class SupportPackWorkflowTests : BuildPipelineFixture
{

    [Fact]
    public async Task UnknownManagedDeploymentNeverStartsSupportPackPreparation()
    {
        var provisioner = new TestSupportPackProvisioner(_ =>
            throw new InvalidOperationException("Unsupported deployments must not prepare a Pack."));
        BuildPipeline pipeline = CreatePipeline(provisioner);

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            pipeline.EnsurePlayerSupportPackAsync(
                new BuildTargetId("windows-x64"), new ManagedDeploymentId("unregistered")).AsTask());

        Assert.Contains("unregistered", failure.Message, StringComparison.Ordinal);
        Assert.Equal(0, provisioner.callCount);
    }

    [Fact]
    public async Task UnsupportedPlatformDeploymentPairNeverStartsSupportPackPreparation()
    {
        var provisioner = new TestSupportPackProvisioner(_ =>
            throw new InvalidOperationException("Unsupported platforms must not prepare a Pack."));
        var deployments = new ManagedDeploymentCatalog(
            [new FixtureManagedDeploymentCompiler(["browser-wasm"])]);
        BuildPipeline pipeline = CreatePipeline(provisioner, deployments);

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            pipeline.EnsurePlayerSupportPackAsync(
                new BuildTargetId("windows-x64"), ManagedDeploymentId.coreClr).AsTask());

        Assert.Contains("win-x64", failure.Message, StringComparison.Ordinal);
        Assert.Equal(0, provisioner.callCount);
    }

    [Fact]
    public async Task MissingSupportPackIsPreparedBeforeTheOwnerThreadExport()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, new BuildTargetId("macos-arm64").value), recursive: true);
        var provisioner = new TestSupportPackProvisioner(target =>
            CreateSupportPack(target, "Inno.Player.MacOSArm64"));

        BuildPipeline pipeline = CreatePipeline(provisioner);
        using PlayerSupportPackLease prepared = await pipeline.EnsurePlayerSupportPackAsync(new BuildTargetId("macos-arm64"), deployment: null);
        BuildResult result = await pipeline.BuildGameAsync(new GameBuildRequest
        {
            profile = CreateProfile(new BuildTargetId("macos-arm64")),
            outputDirectory = Path.Combine(m_root, "Builds", "Provisioned")
        });

        Assert.True(result.succeeded);
        Assert.Equal(1, provisioner.callCount);
        Assert.True(Directory.Exists(Path.Combine(m_supportPackRoot, new BuildTargetId("macos-arm64").value)));
    }

    [Fact]
    public async Task MissingPackFailsPromptlyWhileAsynchronousPreparationIsPending()
    {
        SaveStartupScene();
        Directory.Delete(Path.Combine(m_supportPackRoot, new BuildTargetId("macos-arm64").value), recursive: true);
        using var cancellation = new CancellationTokenSource();
        var provisioner = new PendingSupportPackProvisioner();
        BuildPipeline pipeline = CreatePipeline(provisioner);
        Task<PlayerSupportPackLease> preparation = pipeline.EnsurePlayerSupportPackAsync(
            new BuildTargetId("macos-arm64"), deployment: null, cancellation.Token).AsTask();
        try
        {
            Assert.False(preparation.IsCompleted);
            Task<BuildResult> build = pipeline.BuildGameAsync(new GameBuildRequest
            {
                profile = CreateProfile(new BuildTargetId("macos-arm64")),
                outputDirectory = Path.Combine(m_root, "Builds", "Pending")
            }).AsTask();

            Assert.True(build.IsCompleted);
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => build);
            m_engine.generations.EnsureReady("verify the rejected snapshot released its read lease");
            Assert.False(Directory.Exists(Path.Combine(m_root, "Builds", "Pending")));
        }
        finally
        {
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => preparation);
        }
    }

    [Fact]
    public async Task InvalidInstalledSupportPackDoesNotTriggerProvisioning()
    {
        SaveStartupScene();
        File.Delete(Path.Combine(GetSupportPackDirectory(new BuildTargetId("macos-arm64")),
            "native", "libminiaudio-release.dylib"));
        var provisioner = new TestSupportPackProvisioner(target =>
            CreateSupportPack(target, "Inno.Player.MacOSArm64"));

        await Assert.ThrowsAsync<InvalidDataException>(() =>
            CreatePipeline(provisioner).BuildGameAsync(new GameBuildRequest
            {
                profile = CreateProfile(new BuildTargetId("macos-arm64")),
                outputDirectory = Path.Combine(m_root, "Builds", "InvalidPack")
            }).AsTask());

        Assert.Equal(0, provisioner.callCount);
    }
}
