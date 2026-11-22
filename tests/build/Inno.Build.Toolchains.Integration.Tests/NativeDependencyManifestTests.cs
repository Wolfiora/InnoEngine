using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Integration.Tests;

public sealed class NativeDependencyManifestTests : IDisposable
{
    private readonly string m_root = Path.Combine(
        new PhysicalPathResolver().Resolve(Path.GetTempPath()),
        "InnoDependencyTests",
        Guid.NewGuid().ToString("N")
    );

    private readonly NativeComponentDescriptor m_component = new("fixture", "component/Native.csproj", "component/Toolchain.csproj");
    private int m_productions;

    [Fact]
    public async Task ExactPublishedPayloadImportsWithoutAConsumerCompilerAndIsAdoptedByItsOwner()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        using ProductNativeBuildResult result = await NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        );
        Assert.Equal("payload", File.ReadAllText(Assert.Single(result).files.Single()));
        Assert.Equal(1, m_productions);
        Assert.Single(Directory.GetFiles(Path.Combine(fixture.consumer, "artifacts", "native"), "*.cache.json"));
        using ProductNativeBuildResult warm = await NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        );
        Assert.Equal(result[0].fingerprint, warm[0].fingerprint);
        Assert.Equal(1, m_productions);
    }

    [Fact]
    public async Task AggregateProductsRetainTheirExplicitLayoutWithoutPlatformBranches()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync(aggregate: true);
        using ProductNativeBuildResult imported = await NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        );
        Assert.Equal(Path.Combine(imported[0].directory, "Outputs"), imported.aggregateDirectory);
        Assert.Equal("payload", File.ReadAllText(Assert.Single(imported).files.Single()));
        Assert.Empty(imported.bindingGenerations);
        Assert.Equal(1, m_productions);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task SelectionIsReadOnlyAndRequiresNoArchive(bool aggregate)
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync(aggregate);
        string downloads = Path.Combine(fixture.consumer, "artifacts", "fixture-target", "native");
        foreach (string archive in Directory.GetFiles(downloads))
        {
            File.Delete(archive);
        }
        IReadOnlyList<NativeDependencyAsset> selected = await NativeDependencyManifest.SelectAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        );
        Assert.Single(selected);
        Assert.False(Directory.Exists(Path.Combine(fixture.consumer, "artifacts", "native")));
        Assert.Equal(1, m_productions);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{invalid")]
    [InlineData("{\"assets\": [null], \"aggregates\": []}")]
    public async Task MalformedSelectionFailsBeforeTouchingArtifacts(string json)
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        File.WriteAllText(fixture.selection, json);
        await Assert.ThrowsAsync<InvalidDataException>(
            () => NativeDependencyManifest.SelectAsync(fixture.consumer, fixture.selection, "fixture-target", "release", fixture.plan)
        );
        Assert.False(Directory.Exists(Path.Combine(fixture.consumer, "artifacts", "native")));
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("archive")]
    [InlineData("target")]
    [InlineData("options")]
    [InlineData("escape")]
    [InlineData("source")]
    [InlineData("added-recipe")]
    public async Task MissingOrChangedDependencyCannotInvokeTheProducer(string damage)
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        JsonObject selection = JsonNode.Parse(File.ReadAllText(fixture.selection))!.AsObject();
        JsonObject asset = selection["assets"]![0]!.AsObject();
        string archive = Path.Combine(
            fixture.consumer,
            "artifacts",
            "fixture-target",
            "native",
            asset["sha256"]!.GetValue<string>() + ".zip"
        );
        switch (damage)
        {
            case "missing":
                File.Delete(archive);
                break;
            case "archive":
                File.WriteAllText(archive, "damaged");
                break;
            case "target":
                asset["target"] = "foreign-target";
                break;
            case "options":
                asset["options"] = "{}";
                break;
            case "escape":
                asset["directory"] = "../outside";
                break;
            case "source":
                File.AppendAllText(Path.Combine(fixture.consumer, "component", "Native.csproj"), "changed");
                break;
            case "added-recipe":
                File.WriteAllText(Path.Combine(fixture.consumer, "component", "Source", "NewRecipe.cmake"), "new input");
                break;
        }

        File.WriteAllText(fixture.selection, selection.ToJsonString());
        Exception? failure = await Record.ExceptionAsync(
            () => NativeDependencyManifest.AcquireAsync(fixture.consumer, fixture.selection, "fixture-target", "release", fixture.plan)
        );
        if (damage == "missing")
        {
            Assert.IsType<FileNotFoundException>(failure);
        }
        else
        {
            Assert.IsType<InvalidDataException>(failure);
        }
        Assert.Equal(1, m_productions);
    }

    [Fact]
    public async Task ModifiedInstalledArtifactFailsWithoutRepairOrProduction()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        string output;
        using (ProductNativeBuildResult imported = await NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        )) {
            output = imported[0].files.Single();
        }
        File.WriteAllText(output, "changed");
        await Assert.ThrowsAsync<InvalidDataException>(
            () => NativeDependencyManifest.AcquireAsync(fixture.consumer, fixture.selection, "fixture-target", "release", fixture.plan)
        );
        Assert.Equal("changed", File.ReadAllText(output));
        Assert.Equal(1, m_productions);
    }

    [Fact]
    public async Task ForeignDependencyClosureIsRejectedBeforeDownloadOrInstallation()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        JsonObject selection = JsonNode.Parse(File.ReadAllText(fixture.selection))!.AsObject();
        selection["assets"]![0]!["dependencies"] = new JsonObject { ["foreign"] = new string('a', 64) };
        File.WriteAllText(fixture.selection, selection.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeDependencyManifest.SelectAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        ));
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        ));
        Assert.False(Directory.Exists(Path.Combine(fixture.consumer, "artifacts", "native")));
        Assert.Equal(1, m_productions);
    }

    [Fact]
    public async Task IndexFlagsCannotHideChangedConsumerSourceBytes()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync();
        const string C_RELATIVE = "component/Source/source.h";
        await GitAsync(fixture.consumer, "update-index", "--assume-unchanged", C_RELATIVE);
        string source = Path.Combine(fixture.consumer, C_RELATIVE);
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(source, "native other");
        File.SetLastWriteTimeUtc(source, timestamp);
        Assert.Equal(string.Empty, (await GitAsync(fixture.consumer, "status", "--porcelain")).Trim());
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeDependencyManifest.SelectAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        ));
        Assert.False(Directory.Exists(Path.Combine(fixture.consumer, "artifacts", "native")));
        Assert.Equal(1, m_productions);
    }

    [Fact]
    public async Task PinnedSubmoduleNeedsNoConsumerCheckoutButMutableCheckoutCannotHideChanges()
    {
        (string consumer, string selection, ProductNativeBuildPlan plan) fixture = await PrepareAsync(submodule: true);
        using (ProductNativeBuildResult imported = await NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        )) {
            Assert.Equal("payload", File.ReadAllText(imported[0].files.Single()));
        }
        Assert.False(File.Exists(Path.Combine(fixture.consumer, "component", "Source", "source.h")));
        await GitAsync(fixture.consumer, "-c", "protocol.file.allow=always", "submodule", "update", "--init");
        string sourceRoot = Path.Combine(fixture.consumer, "component", "Source");
        await GitAsync(sourceRoot, "update-index", "--assume-unchanged", "source.h");
        File.WriteAllText(Path.Combine(sourceRoot, "source.h"), "hidden changed input");
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeDependencyManifest.AcquireAsync(
            fixture.consumer,
            fixture.selection,
            "fixture-target",
            "release",
            fixture.plan
        ));
        Assert.Equal(1, m_productions);
    }

    public void Dispose()
    {
        if (!Directory.Exists(m_root))
        {
            return;
        }
        foreach (string file in Directory.EnumerateFiles(m_root, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        Directory.Delete(m_root, recursive: true);
    }

    private async Task<(string consumer, string selection, ProductNativeBuildPlan plan)> PrepareAsync(
        bool aggregate = false,
        bool submodule = false
    ) {
        string producer = Path.Combine(m_root, "producer");
        string consumer = Path.Combine(m_root, "consumer");
        Directory.CreateDirectory(Path.Combine(producer, "component", "Source"));
        File.WriteAllText(Path.Combine(producer, "component", "Source", "source.h"), "native input");
        File.WriteAllText(Path.Combine(producer, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(producer, ".gitignore"), "artifacts/\n**/obj/\n");
        File.WriteAllText(Path.Combine(producer, "component", "Native.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(producer, "component", "Toolchain.csproj"), "<Project />");
        await GitAsync(producer, "init");
        if (submodule)
        {
            string source = Path.Combine(producer, "component", "Source");
            await GitAsync(source, "init");
            await GitAsync(source, "add", ".");
            await GitAsync(
                source,
                "-c",
                "user.name=Fixture",
                "-c",
                "user.email=fixture@example.invalid",
                "commit",
                "-m",
                "Native submodule"
            );
            await GitAsync(producer, "-c", "protocol.file.allow=always", "submodule", "add", source.Replace('\\', '/'), "component/Source");
        }
        await GitAsync(producer, "add", ".");
        await GitAsync(producer, "-c", "user.name=Fixture", "-c", "user.email=fixture@example.invalid", "commit", "-m", "Native source");
        await GitAsync(m_root, "clone", "--no-hardlinks", producer, consumer);
        string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        var tools = new NativeToolchainSelection(
            "fixture-target",
            new BuildHostDescriptor("fixture-host", "fixture-arch"),
            new Dictionary<string, string> { ["sdk"] = sdk },
            new Dictionary<string, string>(),
            [],
            [],
            new NativeLinkPlan(staticLibraryExtension: ".data"),
            false
        );
        NativeBuildContext context = new NativeBuildContext(producer, "release").WithToolchain(tools);
        var plan = new ProductNativeBuildPlan("fixture", [new ProductNativeBuildStep(
            "fixture",
            m_component,
            new NativeComponentBuildOptions(NativeLibraryKind.Static),
            [],
            ProduceAsync,
            Recipe,
            static (
                relative,
                _
            ) => relative
        )]);
        NativeBuildProduct? aggregateProduct = aggregate
            ? await ProduceAsync(context, new Dictionary<string, NativeBuildProduct>(), CancellationToken.None)
            : null;
        using ProductNativeBuildResult products = aggregateProduct is not null
            ? new ProductNativeBuildResult(
                [aggregateProduct],
                new NativeBindingGenerationResult(new Dictionary<string, NativeBindingGenerationDescriptor>(), []),
                Path.Combine(aggregateProduct.directory, "Outputs"),
                Recipe(context)
            )
            : await new ComponentNativeProductExecutor().BuildAsync(context, plan, CancellationToken.None);
        string selection = await NativeDependencyManifest.ExportAsync(
            context,
            plan,
            products,
            Path.Combine(producer, "artifacts", "export")
        );
        JsonNode asset = JsonNode.Parse(File.ReadAllText(selection))![aggregate ? "aggregates" : "assets"]![0]!;
        string downloads = Path.Combine(consumer, "artifacts", "fixture-target", "native");
        Directory.CreateDirectory(downloads);
        File.Copy(
            Path.Combine(producer, "artifacts", "export", asset["asset"]!.GetValue<string>()),
            Path.Combine(downloads, asset["sha256"]!.GetValue<string>() + ".zip")
        );
        string consumerSelection = Path.Combine(consumer, "artifacts", "selection.json");
        File.Copy(selection, consumerSelection);
        return (consumer, consumerSelection, plan);
    }

    private NativeBuildRecipe Recipe(NativeBuildContext context)
        => new(
            m_component,
            "fixture",
            "fixture-target",
            [NativeBuildInput.FromPath(context.engineRoot, Path.Combine(context.engineRoot, "component", "Source"))],
            ["fixture"]
        );

    private Task<NativeBuildProduct> ProduceAsync(
        NativeBuildContext context,
        IReadOnlyDictionary<string, NativeBuildProduct> dependencies,
        CancellationToken cancellationToken
    ) => NativeArtifactPublisher.PublishAsync(
        context,
        Recipe(context),
        (
            _,
            output,
            token
        ) =>
        {
            m_productions++;
            return File.WriteAllTextAsync(Path.Combine(output, "output.data"), "payload", token);
        },
        describeOutputs: static (
            _,
            output
        ) => [new NativeArtifact("fixture", Path.Combine(output, "output.data"), NativeArtifactRole.StaticArchive)],
        cancellationToken: cancellationToken
    );

    private static Task<string> GitAsync(
        string root,
        params string[] arguments
    ) => ToolchainEnvironment.CaptureOutputAsync("git", arguments, root, cancellationToken: CancellationToken.None);
}
