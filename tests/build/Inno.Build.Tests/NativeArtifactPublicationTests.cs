using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Build.Windows;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeArtifactPublicationTests : IDisposable
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadNativeValue();

    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativePublication", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task MaterializationPreservesIdenticalTimestampsAndRejectsChangesAfterTheSourceSnapshot()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "int first;");
        string intermediate = Path.Combine(m_root, "Intermediate", "input.h");
        await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
                scoped,
                output,
                cancellation
            ) => {
                Assert.True(await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation));
                DateTime original = File.GetLastWriteTimeUtc(intermediate);
                Assert.False(await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation));
                Assert.Equal(original, File.GetLastWriteTimeUtc(intermediate));
                File.WriteAllText(Path.Combine(output, "result.native"), "complete");
            });

        context = CreateContext();
        File.WriteAllText(source, "int newer;");
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
                scoped,
                output,
                cancellation
            ) => {
                File.WriteAllText(source, "int wrong;");
                await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation);
            }));
        Assert.Equal("int first;", File.ReadAllText(intermediate));
        Assert.Empty(Directory.EnumerateFiles(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task CanceledMaterializationPreservesTheOwnedIntermediate()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        string intermediate = Path.Combine(m_root, "input.h");
        File.WriteAllText(source, "source");
        File.WriteAllText(intermediate, "previous");
        await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
                scoped,
                output,
                cancellation
            ) => {
                using var canceled = new CancellationTokenSource();
                canceled.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                    NativeInputMaterializer.CopyAsync(scoped, source, intermediate, canceled.Token).AsTask());
                File.WriteAllText(Path.Combine(output, "result.native"), "complete");
            });
        Assert.Equal("previous", File.ReadAllText(intermediate));
    }

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
            Task<NativeBuildProduct> Publish()
            {
                NativeBuildContext operation = CreateContext();
                return NativeArtifactPublisher.PublishAsync(operation,
                    CreateRecipe(operation, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), Produce);
            }
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
            string intermediate = scoped.GetNativeBuildRoot(new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"));
            string physical = new DirectoryInfo(intermediate).LinkTarget ?? intermediate;
            Assert.Contains(Path.Combine("obj", "native", "fixture-x64"), physical);
            toolDirectory = intermediate;
            if (OperatingSystem.IsWindows())
                Assert.Throws<IOException>(() => PathBoundary.EnumerateFiles(intermediate).ToArray());
            await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
        }
        Task<NativeBuildProduct> Publish() => NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], ["sdk"]), Produce);

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
    public async Task SharedInitialInputsAreHashedOnceButEveryPublicationRechecksTheirBytes()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "shared.h");
        File.WriteAllText(source, "shared");
        Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
        await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "first", "fixture-x64", [source], []), Produce);
        await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "second", "fixture-x64", [source], []), Produce);
        Assert.Equal(5, context.statistics.hashedFiles);
        Assert.Equal(30, context.statistics.hashedBytes);

        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(source, "unsafe");
        File.SetLastWriteTimeUtc(source, timestamp);
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "third", "fixture-x64", [source], []), Produce));
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
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), Produce);

        File.WriteAllText(Path.Combine(source, "added.h"), "new closure");
        context = CreateContext();
        await Assert.ThrowsAsync<IOException>(() => NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []),
            (
                scoped,
                output,
                cancellation
            ) => throw new IOException("Compiler failed.")));
        Assert.Equal("complete", File.ReadAllText(first.files[0]));

        context = CreateContext();
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
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
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
                scoped,
                output,
                cancellation
            ) => {
                await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
                string link = Path.Combine(output, "Link");
                Directory.CreateDirectory(link);
                await File.WriteAllTextAsync(Path.Combine(link, "api.lib"), "build-only", cancellation);
            });
        var plan = new ProductNativeBuildPlan("fixture", [new ProductNativeBuildStep(
            "fixture", new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), new(NativeLibraryKind.Shared), [],
            (
                context,
                dependencies,
                token
            ) => Task.FromResult(product),
            (
                relative,
                target
            ) => relative.Split(Path.DirectorySeparatorChar)[0] == "Link"
                ? null : Path.Combine("fixture", target, relative))]);
        string application = Path.Combine(m_root, "Application");
        await Task.WhenAll(Enumerable.Range(0, 3)
            .Select(_ => ProductNativeDeployment.InstallAsync([product], plan, application).AsTask()));
        string deployed = Path.Combine(application, "native", "fixture", "fixture-x64", "api.native");
        Assert.Equal("complete", File.ReadAllText(deployed));
        Assert.Single(Directory.EnumerateFiles(Path.Combine(application, "native"), "*", SearchOption.AllDirectories));

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            ProductNativeDeployment.InstallAsync([product], plan, application, cancellation.Token).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));

        File.WriteAllText(product.files.Single(file => file.EndsWith("api.native", StringComparison.Ordinal)), "tampered");
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            ProductNativeDeployment.InstallAsync([product], plan, application).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));
        Assert.Empty(Directory.EnumerateDirectories(application, "*.staging-*"));
    }

    [Fact]
    public async Task WindowsColdCMakeBuildCompilesCopiedInputsEvenWhenOriginalBytesAreTemporarilyChanged()
    {
        if (!OperatingSystem.IsWindows())
            return;
        NativeBuildContext unresolved = CreateContext();
        NativeToolchainSelection tools = await new WindowsNativeToolchainProvider(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet")).ResolveAsync(
            unresolved, new BuildHostDescriptor("Windows", "x64"), "windows-x64", CancellationToken.None);
        NativeBuildContext context = unresolved.WithToolchain(tools).WithComponentOptions(
            new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"),
            new NativeComponentBuildOptions(NativeLibraryKind.Shared));
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "CMakeLists.txt"),
            "cmake_minimum_required(VERSION 3.20)\nproject(ColdFixture C)\n"
            + "configure_file(\"${INNO_FIXTURE_INPUT}\" \"${CMAKE_CURRENT_BINARY_DIR}/answer.h\" COPYONLY)\n"
            + "add_library(fixture SHARED api.c)\n"
            + "target_include_directories(fixture PRIVATE \"${CMAKE_CURRENT_BINARY_DIR}\")\n");
        File.WriteAllText(Path.Combine(source, "api.c"), "#include \"answer.h\"\n__declspec(dllexport) int value(void) { return ANSWER; }\n");
        string header = Path.Combine(source, "answer.h");
        File.WriteAllText(header, "#define ANSWER 42\n");
        DateTime timestamp = File.GetLastWriteTimeUtc(header);

        NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context,
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "windows-x64", [source], []), async (
                scoped,
                output,
                cancellation
            ) => {
                var owner = new NativeComponentDescriptor("fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj");
                NativeCMakeSource copied = await NativeCMakeExecutor.PrepareSourceAsync(scoped, owner, cancellation);
                Assert.Same(copied, await NativeCMakeExecutor.PrepareSourceAsync(scoped.WithToolchain(tools), owner, cancellation));
                Assert.NotEqual(source, copied.ResolvePath(source));
                Assert.Throws<InvalidOperationException>(() => copied.ResolvePath(Path.Combine(source, "undeclared.h")));
                Assert.Equal("-DCMAKE_INSTALL_PREFIX=" + output, copied.ResolveDefinition("-DCMAKE_INSTALL_PREFIX=" + output));
                File.WriteAllText(header, "#define ANSWER 17\n");
                File.SetLastWriteTimeUtc(header, timestamp);
                try
                {
                    Assert.Equal("#define ANSWER 42\n", File.ReadAllText(copied.ResolvePath(header)));
                    string directory = await NativeCMakeExecutor.BuildAsync(scoped, owner, source,
                        "fixture", ["-DINNO_FIXTURE_INPUT=" + header], cancellation);
                    File.Copy(NativeCMakeExecutor.FindOutput(scoped, directory, "fixture.dll"), Path.Combine(output, "fixture.dll"));
                }
                finally
                {
                    File.WriteAllText(header, "#define ANSWER 42\n");
                    File.SetLastWriteTimeUtc(header, timestamp);
                }
            });

        Assert.Single(product.files);
        Assert.True(new FileInfo(product.files[0]).Length > 0);
        nint library = NativeLibrary.Load(product.files[0]);
        try
        {
            ReadNativeValue read = Marshal.GetDelegateForFunctionPointer<ReadNativeValue>(NativeLibrary.GetExport(library, "value"));
            Assert.Equal(42, read());
        }
        finally
        {
            NativeLibrary.Free(library);
        }
        Assert.True(context.statistics.materializedBytes > 0);
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
            CreateRecipe(context, new NativeComponentDescriptor("fixture", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"), "fixture", "fixture-x64", [source], []), async (
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

    private static NativeBuildRecipe CreateRecipe(
        NativeBuildContext context,
        NativeComponentDescriptor owner,
        string component,
        string targetId,
        IReadOnlyList<string> inputPaths,
        IReadOnlyList<string> declarations
    ) => new(owner, component, targetId,
        inputPaths.Concat(context.toolchain?.inputPaths ?? [])
            .Select(path => NativeBuildInput.FromPath(context.engineRoot, path)),
        declarations.Concat(context.toolchain?.declarations ?? []));

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
        string projectName = "Inno.Build.Toolchains";
        string owner = Path.Combine(m_root, "build", "toolchains", projectName);
        Directory.CreateDirectory(owner);
        File.WriteAllText(Path.Combine(owner, projectName + ".csproj"), "<Project />");
        return new(m_root, "release");
    }
}
