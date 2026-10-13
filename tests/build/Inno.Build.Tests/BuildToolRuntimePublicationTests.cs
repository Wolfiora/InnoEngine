using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class BuildToolRuntimePublicationTests
{
    [Fact]
    public async Task PreparedToolLoadsAnImmutableDeploymentAndLeavesSdkOutputWritable()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string owner = Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces", Guid.NewGuid().ToString("N"));
        try
        {
            string output = Path.Combine(owner, "Runtime");
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(15));
            await ToolchainEnvironment.RunAsync(host,
                ["build", Path.Combine(root, "build", "cli", "Inno.Build.Cli", "Inno.Build.Cli.csproj"),
                    "-p:BuildProjectReferences=false", "-p:InnoBuildCliOutputPath=" + output + Path.DirectorySeparatorChar,
                    "-nodeReuse:false", "-v:quiet"],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            string source = Path.Combine(output, "net9.0", "Inno.Build.Cli.dll");
            string prepared = await BuildApplication.PublishRuntimeAsync(root, "Debug", host, source, cancellation.Token);
            string publication = Directory.GetParent(Path.GetDirectoryName(prepared)!)!.FullName;
            using FileLease activeReader = await FileLease.AcquireSharedAsync(publication + ".read.lock",
                TimeSpan.FromSeconds(10), cancellation.Token);
            string repeated = await BuildApplication.PublishRuntimeAsync(root, "Debug", host, source, cancellation.Token);
            Assert.Equal(prepared, repeated);
            Assert.NotEqual(source, prepared);
            Assert.Contains(Path.Combine("cli", "hosts"), prepared);
            Assert.Equal(AssemblyName.GetAssemblyName(source).FullName, AssemblyName.GetAssemblyName(prepared).FullName);
            WeakReference monitor = LoadAndVerifySourceWritable(prepared, source);
            for (int attempt = 0; attempt < 10 && monitor.IsAlive; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Thread.Sleep(10);
            }
            Assert.False(monitor.IsAlive);
        }
        finally
        {
            if (Directory.Exists(owner))
                Directory.Delete(PathBoundary.RequireUnlinkedPath(Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces"), owner), recursive: true);
        }
    }

    [Fact]
    public async Task PreCanceledPublicationDoesNotDiscoverSdkOrCreateState()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BuildApplication.PublishRuntimeAsync(
            "unavailable-checkout", "Debug", "unavailable-sdk", "unavailable-assembly", cancellation.Token));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadAndVerifySourceWritable(
        string publication,
        string source
    ) {
        var load = new AssemblyLoadContext("Build-tool publication acceptance", isCollectible: true);
        var monitor = new WeakReference(load);
        try
        {
            _ = load.LoadFromAssemblyPath(publication);
            using FileStream writing = File.Open(source, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            Assert.True(writing.CanWrite);
            return monitor;
        }
        finally
        {
            load.Unload();
        }
    }
}
