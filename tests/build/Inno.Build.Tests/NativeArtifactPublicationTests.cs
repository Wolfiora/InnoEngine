using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeArtifactPublicationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativePublication", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task SdkDirectoryAliasesContributeContentAndDirectoryCyclesFailBeforeProducingOutput()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "SdkInputs");
        string headers = Path.Combine(m_root, "SdkHeaders");
        Directory.CreateDirectory(source);
        Directory.CreateDirectory(headers);
        string header = Path.Combine(headers, "api.h");
        File.WriteAllText(header, "first declaration");
        string alias = Path.Combine(source, "include");
        await CreateDirectoryAlias(alias, headers);
        string cycle = Path.Combine(source, "cycle");
        try
        {
            Task Produce(
                NativeBuildContext scoped,
                string output,
                CancellationToken cancellation
            ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
            Task<NativeBuildProduct> Publish() => NativeArtifactPublisher.PublishAsync(context,
                typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [], Produce);
            NativeBuildProduct first = await Publish();
            File.WriteAllText(header, "changed declaration");
            NativeBuildProduct changed = await Publish();
            Assert.NotEqual(first.fingerprint, changed.fingerprint);

            await CreateDirectoryAlias(cycle, source);
            IOException failure = await Assert.ThrowsAsync<IOException>(() => Publish());

            Assert.Contains("cycle", failure.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("complete", File.ReadAllText(first.files[0]));
            Assert.Equal("complete", File.ReadAllText(changed.files[0]));
            Assert.Empty(Directory.GetDirectories(Path.Combine(m_root, "artifacts"), "*.staging-*", SearchOption.AllDirectories));
        }
        finally
        {
            if (Directory.Exists(cycle))
                Directory.Delete(cycle, recursive: false);
            Directory.Delete(alias, recursive: false);
        }
    }

    [Fact]
    public async Task ConcurrentRequestsShareOneCompleteProductAndTamperingIsRepaired()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "api.h"), "source");
        int builds = 0;
        string? toolDirectory = null;
        async Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            Interlocked.Increment(ref builds);
            string intermediate = scoped.GetNativeBuildRoot(typeof(NativeArtifactPublisher).Assembly);
            string physical = new DirectoryInfo(intermediate).LinkTarget ?? intermediate;
            Assert.Contains(Path.Combine("obj", "native", "fixture-x64"), physical);
            toolDirectory = intermediate;
            if (OperatingSystem.IsWindows())
                Assert.Throws<IOException>(() => PathBoundary.EnumerateFiles(intermediate).ToArray());
            await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
        }
        Task<NativeBuildProduct> Publish() => NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], ["sdk"], Produce);

        NativeBuildProduct[] products = await Task.WhenAll(Enumerable.Range(0, 4).Select(_ => Publish()));
        Assert.Equal(1, builds);
        if (OperatingSystem.IsWindows())
            Assert.False(Directory.Exists(toolDirectory));
        Assert.All(products, product => Assert.Equal(products[0].directory, product.directory));
        Assert.Throws<NotSupportedException>(() => ((IList<string>)products[0].files).Clear());
        File.WriteAllText(products[0].files[0], "tampered");

        NativeBuildProduct repaired = await Publish();
        Assert.Equal(2, builds);
        Assert.Equal(products[0].fingerprint, repaired.fingerprint);
        Assert.Equal("complete", File.ReadAllText(repaired.files[0]));
        Assert.Empty(Directory.GetDirectories(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task ChangedClosureAndFailedPreparationPreserveEarlierProducts()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "api.h"), "source");
        Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
        NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [], Produce);

        File.WriteAllText(Path.Combine(source, "added.h"), "new closure");
        await Assert.ThrowsAsync<IOException>(() => NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [],
            (
                scoped,
                output,
                cancellation
            ) => throw new IOException("Compiler failed.")));
        Assert.Equal("complete", File.ReadAllText(first.files[0]));

        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [], async (
                scoped,
                output,
                cancellation
            ) => {
                await Produce(scoped, output, cancellation);
                File.WriteAllText(Path.Combine(source, "changed-during-build.h"), "changed");
            }));
        Assert.Equal("complete", File.ReadAllText(first.files[0]));
        Assert.Empty(Directory.GetDirectories(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task DeploymentUsesExactProductsAndPreservesThePriorTreeOnCancellationOrTampering()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "source");
        NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [], async (
                scoped,
                output,
                cancellation
            ) => {
                await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
                string link = Path.Combine(output, "Link");
                Directory.CreateDirectory(link);
                await File.WriteAllTextAsync(Path.Combine(link, "api.lib"), "build-only", cancellation);
            });
        string application = Path.Combine(m_root, "Application");
        await Task.WhenAll(Enumerable.Range(0, 3)
            .Select(_ => HostNativeDeployment.InstallAsync([product], application).AsTask()));
        string deployed = Path.Combine(application, "native", "fixture", "fixture-x64", "api.native");
        Assert.Equal("complete", File.ReadAllText(deployed));
        Assert.Single(Directory.EnumerateFiles(Path.Combine(application, "native"), "*", SearchOption.AllDirectories));

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            HostNativeDeployment.InstallAsync([product], application, cancellation.Token).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));

        File.WriteAllText(product.files.Single(file => file.EndsWith("api.native", StringComparison.Ordinal)), "tampered");
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            HostNativeDeployment.InstallAsync([product], application).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));
        Assert.Empty(Directory.EnumerateDirectories(application, "*.staging-*"));
    }

    [Fact]
    public async Task WindowsColdCMakeBuildUsesTheOwnedToolAliasAndPublishesItsCompiledOutput()
    {
        if (!OperatingSystem.IsWindows())
            return;
        NativeBuildContext context = await HostNativeToolchain.ResolveAsync(CreateContext());
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "CMakeLists.txt"),
            "cmake_minimum_required(VERSION 3.20)\nproject(ColdFixture C)\nadd_library(fixture SHARED api.c)\n");
        File.WriteAllText(Path.Combine(source, "api.c"), "__declspec(dllexport) int value(void) { return 42; }\n");

        NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "windows-x64", [source], [], async (
                scoped,
                output,
                cancellation
            ) => {
                string directory = scoped.GetNativeBuildRoot(typeof(NativeArtifactPublisher).Assembly);
                await ToolchainEnvironment.RunAsync(scoped, "cmake",
                    ["-S", source, "-B", directory, "-G", "Visual Studio 17 2022", "-A", "x64"],
                    source, cancellation);
                await ToolchainEnvironment.RunAsync(scoped, "cmake",
                    ["--build", directory, "--config", "Release", "--target", "fixture"],
                    source, cancellation);
                File.Copy(Path.Combine(directory, "Release", "fixture.dll"), Path.Combine(output, "fixture.dll"));
            });

        Assert.Single(product.files);
        Assert.True(new FileInfo(product.files[0]).Length > 0);
        Assert.Empty(Directory.EnumerateDirectories(Path.Combine(m_root, "artifacts"), "*.staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task CanceledProducerCannotPublishItsCandidate()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "source");
        using var cancellation = new CancellationTokenSource();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => NativeArtifactPublisher.PublishAsync(context,
            typeof(NativeArtifactPublisher).Assembly, "fixture", "fixture-x64", [source], [], async (
                scoped,
                output,
                token
            ) => {
                await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "candidate", token);
                cancellation.Cancel();
            }, cancellation.Token));
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(m_root, "artifacts"), "api.native", SearchOption.AllDirectories));
        Assert.Empty(Directory.GetDirectories(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private static async Task CreateDirectoryAlias(
        string path,
        string target
    ) {
        if (!OperatingSystem.IsWindows())
        {
            Directory.CreateSymbolicLink(path, target);
            return;
        }
        string escapedPath = path.Replace("'", "''", StringComparison.Ordinal);
        string escapedTarget = target.Replace("'", "''", StringComparison.Ordinal);
        await ToolchainEnvironment.RunAsync("powershell.exe",
            ["-NoProfile", "-NonInteractive", "-Command",
                $"$ErrorActionPreference='Stop'; New-Item -ItemType Junction -Path '{escapedPath}' -Target '{escapedTarget}' | Out-Null"],
            Path.GetDirectoryName(path)!, CancellationToken.None);
    }

    private NativeBuildContext CreateContext()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        string projectName = typeof(NativeArtifactPublisher).Assembly.GetName().Name!;
        string owner = Path.Combine(m_root, "build", "toolchains", projectName);
        Directory.CreateDirectory(owner);
        File.WriteAllText(Path.Combine(owner, projectName + ".csproj"), "<Project />");
        return new(m_root, "release");
    }
}
