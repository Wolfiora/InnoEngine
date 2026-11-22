using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeArtifactPublicationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativePublication", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task RepairWaitsForReadersAndCancellationPreservesThePinnedDirectory()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "repair.h");
        File.WriteAllText(source, "input");
        var owner = new NativeComponentDescriptor(
            "fixture",
            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
        );
        NativeBuildRecipe recipe = CreateRecipe(context, owner, "fixture", "fixture-x64", [source], []);
        var prepared = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        int calls = 0;
        async Task ProduceAsync(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellationToken
        ) {
            await File.WriteAllTextAsync(Path.Combine(output, "result.native"), "complete", cancellationToken);
            if (Interlocked.Increment(ref calls) > 1)
            {
                prepared.TrySetResult();
            }
        }
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(context, recipe, ProduceAsync);
        File.WriteAllText(first.files[0], "damaged");
        using var cancellation = new CancellationTokenSource();
        Task<NativeBuildProduct> waiting = NativeArtifactPublisher.PublishAsync(
            CreateContext(),
            recipe,
            ProduceAsync,
            cancellationToken: cancellation.Token
        );
        await prepared.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await Task.Delay(80);
        Assert.False(waiting.IsCompleted);
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => waiting);
        Assert.Equal("damaged", File.ReadAllText(first.files[0]));
        first.Dispose();
        using NativeBuildProduct repaired = await NativeArtifactPublisher.PublishAsync(CreateContext(), recipe, ProduceAsync);
        Assert.Equal("complete", File.ReadAllText(repaired.files[0]));
    }

    [Fact]
    public async Task MaterializationPreservesIdenticalTimestampsAndRejectsChangesAfterTheSourceSnapshot()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "int first;");
        string intermediate = Path.Combine(m_root, "Intermediate", "input.h");
        using NativeBuildProduct result1 = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                cancellation
            ) =>
            {
                Assert.True(await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation));
                DateTime original = File.GetLastWriteTimeUtc(intermediate);
                Assert.False(await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation));
                Assert.Equal(original, File.GetLastWriteTimeUtc(intermediate));
                File.WriteAllText(Path.Combine(output, "result.native"), "complete");
            }
        );

        context = CreateContext();
        File.WriteAllText(source, "int newer;");
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                cancellation
            ) =>
            {
                File.WriteAllText(source, "int wrong;");
                await NativeInputMaterializer.CopyAsync(scoped, source, intermediate, cancellation);
            }
        ));
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
        using NativeBuildProduct result2 = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                cancellation
            ) =>
            {
                using var canceled = new CancellationTokenSource();
                canceled.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                    NativeInputMaterializer.CopyAsync(scoped, source, intermediate, canceled.Token).AsTask());
                File.WriteAllText(Path.Combine(output, "result.native"), "complete");
            }
        );
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
        await CreateDirectoryAliasAsync(alias, headers);
        string cycle = Path.Combine(source, "cycle");
        try
        {
            Task ProduceAsync(
                NativeBuildContext scoped,
                string output,
                CancellationToken cancellationToken
            ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellationToken);
            Task<NativeBuildProduct> PublishAsync()
            {
                NativeBuildContext operation = CreateContext();
                return NativeArtifactPublisher.PublishAsync(
                    operation,
                    CreateRecipe(
                        operation,
                        new NativeComponentDescriptor(
                            "fixture",
                            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                            "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                        ),
                        "fixture",
                        "fixture-x64",
                        [source],
                        []
                    ),
                    ProduceAsync
                );
            }
            using NativeBuildProduct first = await PublishAsync();
            File.WriteAllText(header, "changed declaration");
            using NativeBuildProduct changed = await PublishAsync();
            Assert.NotEqual(first.fingerprint, changed.fingerprint);

            await CreateDirectoryAliasAsync(cycle, source);
            IOException failure = await Assert.ThrowsAsync<IOException>(() => PublishAsync());

            Assert.Contains("cycle", failure.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("complete", File.ReadAllText(first.files[0]));
            Assert.Equal("complete", File.ReadAllText(changed.files[0]));
            Assert.Empty(Directory.GetDirectories(Path.Combine(m_root, "artifacts"), "*.staging-*", SearchOption.AllDirectories));
        }
        finally
        {
            if (Directory.Exists(cycle))
            {
                Directory.Delete(cycle, recursive: false);
            }
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
        async Task ProduceAsync(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellationToken
        ) {
            Interlocked.Increment(ref builds);
            string intermediate = scoped.GetNativeBuildRoot(
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                )
            );
            string physical = new DirectoryInfo(intermediate).LinkTarget ?? intermediate;
            Assert.Contains(Path.Combine("obj", "native", "fixture-x64"), physical);
            toolDirectory = intermediate;
            Assert.True(Directory.Exists(intermediate));
            await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellationToken);
        }
        Task<NativeBuildProduct> PublishAsync()
            => NativeArtifactPublisher.PublishAsync(
                context,
                CreateRecipe(
                    context,
                    new NativeComponentDescriptor(
                        "fixture",
                        "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                        "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                    ),
                    "fixture",
                    "fixture-x64",
                    [source],
                    ["sdk"]
                ),
                ProduceAsync
            );

        NativeBuildProduct[] products = await Task.WhenAll(Enumerable.Range(0, 4).Select(_ => PublishAsync()));
        Assert.Equal(1, builds);
        Assert.True(Directory.Exists(toolDirectory));
        Assert.All(products, product => Assert.Equal(products[0].directory, product.directory));
        Assert.Throws<NotSupportedException>(() => ((IList<string>)products[0].files).Clear());
        File.WriteAllText(products[0].files[0], "tampered");

        foreach (NativeBuildProduct product in products)
        {
            product.Dispose();
        }
        using NativeBuildProduct repaired = await PublishAsync();
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
        Task ProduceAsync(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellationToken
        ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellationToken);
        using NativeBuildProduct result3 = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "first",
                "fixture-x64",
                [source],
                []
            ),
            ProduceAsync
        );
        using NativeBuildProduct result4 = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "second",
                "fixture-x64",
                [source],
                []
            ),
            ProduceAsync
        );
        Assert.Equal(5, context.statistics.hashedFiles);
        Assert.Equal(30, context.statistics.hashedBytes);

        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(source, "unsafe");
        File.SetLastWriteTimeUtc(source, timestamp);
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "third",
                "fixture-x64",
                [source],
                []
            ),
            ProduceAsync
        ));
    }

    [Fact]
    public async Task ChangedClosureAndFailedPreparationPreserveEarlierProducts()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "api.h"), "source");
        Task ProduceAsync(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellationToken
        ) => File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellationToken);
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            ProduceAsync
        );

        File.WriteAllText(Path.Combine(source, "added.h"), "new closure");
        context = CreateContext();
        await Assert.ThrowsAsync<IOException>(() => NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            (
                @scoped,
                output,
                cancellation
            ) => throw new IOException("Compiler failed.")
        ));
        Assert.Equal("complete", File.ReadAllText(first.files[0]));

        context = CreateContext();
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                cancellation
            ) =>
            {
                await ProduceAsync(scoped, output, cancellation);
                File.WriteAllText(Path.Combine(source, "changed-during-build.h"), "changed");
            }
        ));
        Assert.Equal("complete", File.ReadAllText(first.files[0]));
        Assert.Empty(Directory.GetDirectories(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task DeploymentUsesExactProductsAndPreservesThePriorTreeOnCancellationOrTampering()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "source");
        using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                cancellation
            ) =>
            {
                await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "complete", cancellation);
                string link = Path.Combine(output, "Link");
                Directory.CreateDirectory(link);
                await File.WriteAllTextAsync(Path.Combine(link, "api.lib"), "build-only", cancellation);
            }
        );
        var plan = new ProductNativeBuildPlan("fixture", [new ProductNativeBuildStep(
            "fixture",
            new NativeComponentDescriptor(
                "fixture",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
            ),
            new NativeComponentBuildOptions(NativeLibraryKind.Shared),
            [],
            (
                context,
                dependencies,
                token
            ) => Task.FromResult(product),
            selected => CreateRecipe(selected, ProductOwner(), "fixture", "fixture-x64", [source], []),
            (
                relative,
                target
            ) => (
                relative.Split(Path.DirectorySeparatorChar)[0] == "Link"
                ? null
                : Path.Combine("fixture", target, relative)
            )
        )]);
        string application = Path.Combine(m_root, "Application");
        await Task.WhenAll(
            Enumerable.Range(0, 3)
                .Select(_ => ProductNativeDeployment.InstallAsync([product], plan, application).AsTask())
        );
        string deployed = Path.Combine(application, "native", "fixture", "fixture-x64", "api.native");
        Assert.Equal("complete", File.ReadAllText(deployed));
        Assert.Single(Directory.EnumerateFiles(Path.Combine(application, "native"), "*", SearchOption.AllDirectories));

        foreach (string name in new[] { "FirstCandidate", "SecondCandidate" })
        {
            string candidate = Path.Combine(m_root, name);
            Directory.CreateDirectory(candidate);
            await ProductNativeDeployment.StageAsync([product], plan, candidate);
            Assert.Equal(new[] { "native" }, Directory.EnumerateDirectories(candidate).Select(Path.GetFileName));
            Assert.Empty(Directory.EnumerateFiles(candidate));
            Assert.Equal("complete", File.ReadAllText(Path.Combine(candidate, "native", "fixture", "fixture-x64", "api.native")));
            Assert.Single(Directory.EnumerateFiles(candidate, "*", SearchOption.AllDirectories));
            await Assert.ThrowsAsync<IOException>(() => ProductNativeDeployment.StageAsync([product], plan, candidate).AsTask());
        }

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            ProductNativeDeployment.InstallAsync([product], plan, application, cancellation.Token).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));
        string canceledCandidate = Path.Combine(m_root, "CanceledCandidate");
        Directory.CreateDirectory(canceledCandidate);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            ProductNativeDeployment.StageAsync([product], plan, canceledCandidate, cancellation.Token).AsTask());
        Assert.Empty(Directory.EnumerateFileSystemEntries(canceledCandidate));

        File.WriteAllText(product.files.Single(file => file.EndsWith("api.native", StringComparison.Ordinal)), "tampered");
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            ProductNativeDeployment.InstallAsync([product], plan, application).AsTask());
        Assert.Equal("complete", File.ReadAllText(deployed));
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            ProductNativeDeployment.StageAsync([product], plan, canceledCandidate).AsTask());
        Assert.Empty(Directory.EnumerateFileSystemEntries(canceledCandidate));
        Assert.Empty(Directory.EnumerateDirectories(application, "*.staging-*"));

        static NativeComponentDescriptor ProductOwner()
            => new(
                "fixture",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
            );
    }

    [Fact]
    public async Task CanceledProducerCannotPublishItsCandidate()
    {
        NativeBuildContext context = CreateContext();
        string source = Path.Combine(m_root, "source.h");
        File.WriteAllText(source, "source");
        using var cancellation = new CancellationTokenSource();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => NativeArtifactPublisher.PublishAsync(
            context,
            CreateRecipe(
                context,
                new NativeComponentDescriptor(
                    "fixture",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj",
                    "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj"
                ),
                "fixture",
                "fixture-x64",
                [source],
                []
            ),
            async (
                @scoped,
                output,
                token
            ) =>
            {
                await File.WriteAllTextAsync(Path.Combine(output, "api.native"), "candidate", token);
                cancellation.Cancel();
            },
            cancellationToken: cancellation.Token
        ));
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(m_root, "artifacts"), "api.native", SearchOption.AllDirectories));
        Assert.Empty(Directory.GetDirectories(m_root, "*.staging-*", SearchOption.AllDirectories));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
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

    private static NativeBuildRecipe CreateRecipe(
        NativeBuildContext context,
        NativeComponentDescriptor owner,
        string component,
        string targetID,
        IReadOnlyList<string> inputPaths,
        IReadOnlyList<string> declarations
    ) => new(
        owner,
        component,
        targetID,
        inputPaths.Concat(context.toolchain?.inputPaths ?? [])
            .Select(path => NativeBuildInput.FromPath(context.engineRoot, path)),
        declarations.Concat(context.toolchain?.declarations ?? [])
    );

    private static async Task CreateDirectoryAliasAsync(
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
        await ToolchainEnvironment.RunAsync(
            "powershell.exe",
            [
                "-NoProfile",
                "-NonInteractive",
                "-Command",
                $"$ErrorActionPreference='Stop'; New-Item -ItemType Junction -Path '{escapedPath}' -Target '{escapedTarget}' | Out-Null"
            ],
            Path.GetDirectoryName(path)!,
            CancellationToken.None
        );
    }
}
