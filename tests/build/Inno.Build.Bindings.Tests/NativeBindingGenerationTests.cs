using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using BGCS.Configuration;
using BGCS.Core;
using Xunit;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Bindings.Tests;

public sealed class NativeBindingGenerationTests : IDisposable
{
    private readonly string m_root = Path.Combine(
        new PhysicalPathResolver().Resolve(Path.GetTempPath()),
        "InnoNativeBindingTests",
        Guid.NewGuid().ToString("N")
    );

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task DependencyPackageRetainsBindingsWithoutConsumerGeneration()
    {
        CreateConfig("int sample(int value);");
        File.WriteAllText(Path.Combine(m_root, ".gitignore"), "artifacts/\nobj/\nconsumer/\n");
        await GitAsync(m_root, "init");
        await GitAsync(m_root, "add", ".");
        await GitAsync(m_root, "-c", "user.name=Fixture", "-c", "user.email=fixture@example.invalid", "commit", "-m", "Binding source");
        string consumer = Path.Combine(m_root, "consumer");
        await GitAsync(m_root, "clone", "--no-hardlinks", m_root, consumer);
        var component = new NativeComponentDescriptor(
            "fixture",
            "Fixture.csproj",
            "Fixture.csproj",
            bindingDefinition: "bindings.props"
        );
        string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        var tools = new NativeToolchainSelection(
            "fixture-target",
            new BuildHostDescriptor("fixture-host", "fixture-arch"),
            new Dictionary<string, string> { ["sdk"] = sdk },
            new Dictionary<string, string>(),
            [],
            [],
            new NativeLinkPlan(sharedLibraryExtension: ".data"),
            false
        );
        NativeBuildContext context = new NativeBuildContext(m_root, "release").WithToolchain(tools)
            .WithBindingGenerator(BindingTestProfiles.Generator("fixture-target"));
        int productions = 0;
        NativeBuildRecipe Recipe(NativeBuildContext current)
            => new(
                component,
                "fixture",
                "fixture-target",
                [NativeBuildInput.FromPath(current.engineRoot, Path.Combine(current.engineRoot, "Source"))],
                ["fixture"]
            );
        var plan = new ProductNativeBuildPlan("fixture", [new ProductNativeBuildStep(
            "fixture",
            component,
            new NativeComponentBuildOptions(NativeLibraryKind.Shared),
            [],
            (
                current,
                _,
                cancellation
            ) => NativeArtifactPublisher.PublishAsync(
                current,
                Recipe(current),
                (
                    _,
                    output,
                    token
                ) =>
                {
                    productions++;
                    return File.WriteAllTextAsync(Path.Combine(output, "fixture.data"), "native payload", token);
                },
                cancellation,
                describeOutputs: static (
                    _,
                    output
                ) =>
                    [new NativeArtifact("fixture", Path.Combine(output, "fixture.data"), NativeArtifactRole.SharedLibrary)]
            ),
            Recipe,
            static (
                relative,
                _
            ) => relative
        )]);
        using ProductNativeBuildResult produced = await new ComponentNativeProductExecutor().BuildAsync(
            context,
            plan,
            CancellationToken.None
        );
        string selection = await NativeDependencyManifest.ExportAsync(context, plan, produced, Path.Combine(m_root, "artifacts", "export"));
        using JsonDocument manifest = JsonDocument.Parse(File.ReadAllText(selection));
        JsonElement asset = manifest.RootElement.GetProperty("assets")[0];
        string downloads = Path.Combine(consumer, "artifacts", "fixture-target", "native");
        Directory.CreateDirectory(downloads);
        File.Copy(
            Path.Combine(m_root, "artifacts", "export", asset.GetProperty("asset").GetString()!),
            Path.Combine(downloads, asset.GetProperty("sha256").GetString() + ".zip")
        );
        string selected = Path.Combine(consumer, "artifacts", "selection.json");
        File.Copy(selection, selected);
        string managed = File.ReadAllText(produced[0].bindingGeneration!.bindingsPath);
        using (ProductNativeBuildResult imported = await NativeDependencyManifest.AcquireAsync(
            consumer,
            selected,
            "fixture-target",
            "release",
            plan
        )) {
            NativeBindingGenerationDescriptor binding = imported.bindingGenerations[component.nativeProject];
            Assert.Equal(produced[0].bindingGeneration!.fingerprint, binding.fingerprint);
            Assert.Equal(managed, File.ReadAllText(binding.bindingsPath));
            Assert.StartsWith(consumer + Path.DirectorySeparatorChar, binding.bindingsPath);
            Assert.Equal(binding, imported[0].bindingGeneration);
            Assert.NotEmpty(NativeBindingCompilation.Load(binding.nativeDirectory).roots);
            Assert.Equal(1, productions);
        }
        using ProductNativeBuildResult warm = await NativeDependencyManifest.AcquireAsync(
            consumer,
            selected,
            "fixture-target",
            "release",
            plan
        );
        Assert.Equal(managed, File.ReadAllText(warm[0].bindingGeneration!.bindingsPath));
        Assert.Equal(1, productions);

        static Task<string> GitAsync(
            string root,
            params string[] arguments
        ) => ToolchainEnvironment.CaptureOutputAsync("git", arguments, root, CancellationToken.None);
    }

    [Fact]
    public void ManagedSelectionPreservesExactNativeIdentitiesInStableProjectOrder()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        NativeBindingGenerationDescriptor.WriteSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj"] = SelectedGeneration("text-fingerprint"),
                ["backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj"] = SelectedGeneration("bgfx-fingerprint")
            },
            new Dictionary<string, NativeLibraryKind>
            {
                ["backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj"] = NativeLibraryKind.Static,
                ["backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj"] = NativeLibraryKind.Shared
            }
        );

        XElement[] groups = XDocument.Load(output).Root!.Elements("PropertyGroup").ToArray();
        Assert.Equal(2, groups.Length);
        Assert.Equal("'$(MSBuildProjectName)' == 'Inno.Native.Bgfx'", groups[0].Attribute("Condition")!.Value);
        Assert.Equal("bgfx-fingerprint", groups[0].Element("BindGenExpectedFingerprint")!.Value);
        Assert.Equal(
            SelectedGeneration("bgfx-fingerprint").artifactOwnerDirectory,
            groups[0].Element("BindGenArtifactOwnerDirectory")!.Value
        );
        Assert.Equal(
            Path.GetDirectoryName(SelectedGeneration("bgfx-fingerprint").bindingsPath),
            groups[0].Element("BindGenOutputDirectory")!.Value
        );
        Assert.Equal("text-fingerprint", groups[1].Element("BindGenExpectedFingerprint")!.Value);
        Assert.Equal("Shared", groups[0].Element("InnoNativeLibraryKind")!.Value);
        Assert.Equal("Static", groups[1].Element("InnoNativeLibraryKind")!.Value);
    }

    [Theory]
    [InlineData(NativeLibraryKind.Static, false)]
    [InlineData(NativeLibraryKind.Static, true)]
    [InlineData(NativeLibraryKind.Shared, false)]
    [InlineData(NativeLibraryKind.Shared, true)]
    public void ManagedSelectionPreservesImportInitializationIndependentlyOfLinkage(
        NativeLibraryKind linkage,
        bool runtimeImports
    ) {
        string output = Path.Combine(m_root, "ImportSelection.props");
        var generations = new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            ["Inno.Native.Text.csproj"] = SelectedGeneration("selected", runtimeImports)
        };
        var links = new Dictionary<string, NativeLibraryKind> { ["Inno.Native.Text.csproj"] = linkage };
        NativeBindingGenerationDescriptor.WriteSelection(output, generations, links);
        XElement group = XDocument.Load(output).Root!.Element("PropertyGroup")!;
        Assert.Equal(runtimeImports ? "true" : "false", group.Element("InnoNativeRuntimeImports")!.Value);
        Assert.Equal(linkage.ToString(), group.Element("InnoNativeLibraryKind")!.Value);
        NativeBindingGenerationDescriptor.ValidateSelection(output, generations, links);
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["Inno.Native.Text.csproj"] = SelectedGeneration("selected", !runtimeImports)
            },
            links
        ));
    }

    [Fact]
    public void IdenticalSelectionPreservesTheImportTimestampAndChangedSelectionReplacesIt()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        var generations = new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            ["Inno.Native.Text.csproj"] = SelectedGeneration("selected")
        };
        var linkage = new Dictionary<string, NativeLibraryKind>
        {
            ["Inno.Native.Text.csproj"] = NativeLibraryKind.Shared
        };
        NativeBindingGenerationDescriptor.WriteSelection(output, generations, linkage);
        DateTime originalTime = DateTime.UtcNow.AddMinutes(-1);
        File.SetLastWriteTimeUtc(output, originalTime);
        DateTime actualTime = File.GetLastWriteTimeUtc(output);
        NativeBindingGenerationDescriptor.WriteSelection(output, generations, linkage);
        Assert.Equal(actualTime, File.GetLastWriteTimeUtc(output));
        NativeBindingGenerationDescriptor.ValidateSelection(output, generations, linkage);

        linkage["Inno.Native.Text.csproj"] = NativeLibraryKind.Static;
        NativeBindingGenerationDescriptor.WriteSelection(output, generations, linkage);
        Assert.NotEqual(actualTime, File.GetLastWriteTimeUtc(output));
        NativeBindingGenerationDescriptor.ValidateSelection(output, generations, linkage);
    }

    [Fact]
    public void PublicationRejectsChangedOrIncompleteManagedSelection()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        var original = new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            ["Inno.Native.Text.csproj"] = SelectedGeneration("original")
        };
        var linkage = new Dictionary<string, NativeLibraryKind>
        {
            ["Inno.Native.Text.csproj"] = NativeLibraryKind.Shared
        };
        NativeBindingGenerationDescriptor.WriteSelection(output, original, linkage);
        NativeBindingGenerationDescriptor.ValidateSelection(output, original, linkage);
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["Inno.Native.Text.csproj"] = SelectedGeneration("changed")
            },
            linkage
        ));
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>(),
            new Dictionary<string, NativeLibraryKind>()
        ));
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(
            output,
            original,
            new Dictionary<string, NativeLibraryKind> { ["Inno.Native.Text.csproj"] = NativeLibraryKind.Static }
        ));
        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(
            output,
            original,
            new Dictionary<string, NativeLibraryKind>()
        ));
        Assert.Equal(
            "original",
            XDocument.Load(output).Root!.Element("PropertyGroup")!
                .Element("BindGenExpectedFingerprint")!.Value
        );
    }

    [Fact]
    public void CollidingManagedSelectionDoesNotReplaceAnExistingCompleteSelection()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
        File.WriteAllText(output, "previous complete selection");

        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["first/Inno.Native.Text.csproj"] = SelectedGeneration("first"),
                ["second/inno.native.text.csproj"] = SelectedGeneration("second")
            },
            new Dictionary<string, NativeLibraryKind>
            {
                ["first/Inno.Native.Text.csproj"] = NativeLibraryKind.Shared,
                ["second/inno.native.text.csproj"] = NativeLibraryKind.Shared
            }
        ));
        Assert.Equal("previous complete selection", File.ReadAllText(output));
    }

    [Fact]
    public void UnsafeManagedProjectNameIsRejectedBeforeCreatingSelectionOutput()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(
            output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["Inno'Injected.csproj"] = SelectedGeneration("selected")
            },
            new Dictionary<string, NativeLibraryKind> { ["Inno'Injected.csproj"] = NativeLibraryKind.Shared }
        ));
        Assert.False(Directory.Exists(m_root));
    }

    [Theory]
    [InlineData(BindingImportMode.LibraryImport)]
    [InlineData(BindingImportMode.DllImport)]
    [Trait("ExternalDependency", "BGCS")]
    public async Task PlatformImportChoiceChangesOfficialGenerationAndFrozenSelection(BindingImportMode importMode)
    {
        CreateConfig("unsigned long long sample(void* value);");
        NativeBindingGenerationDescriptor provider = await GenerateAsync();
        var component = new NativeComponentDescriptor(
            "fixture",
            "Fixture.csproj",
            "Fixture.csproj",
            bindingDefinition: "bindings.props"
        );
        var request = new NativeBindingGenerationRequest(
            [component],
            "fixture-target",
            NativeBindingOutputMode.TargetArtifacts
        );
        var profile = new NativeBindingProfile(
            "fixture-target",
            new TargetConfiguration { id = "windows-x64-msvc" },
            importMode: importMode
        );
        using NativeBindingGenerationResult result = await new NativeBindingGenerator(profile).GenerateAsync(
            new NativeBuildContext(m_root, "release"),
            request,
            CancellationToken.None
        );
        NativeBindingGenerationDescriptor imports = result[component.nativeProject];
        Assert.False(provider.usesRuntimeImports);
        Assert.True(imports.usesRuntimeImports);
        Assert.NotEqual(provider.fingerprint, imports.fingerprint);
        string source = File.ReadAllText(imports.bindingsPath);
        Assert.Contains(importMode + "(", source);
        Assert.DoesNotContain("delegate* unmanaged", source);
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task SourceChangesSelectIndependentGenerationsAndTamperingIsRepaired()
    {
        CreateConfig("int sample(int value);");
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string original = File.ReadAllText(first.bindingsPath);
        Assert.Contains(Path.Combine(first.fingerprint, "Generated", "Bindings.cs"), first.bindingsPath);
        string descriptorPath = Path.Combine(m_root, "requests", "descriptor.json");
        first.Write(descriptorPath);
        Assert.Equal(first.bindingsPath, NativeBindingGenerationDescriptor.Load(descriptorPath).bindingsPath);
        Assert.True(
            BuildArtifactManifest.IsComplete(
                Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath))!,
                first.fingerprint,
                ["Native", "Generated"]
            )
        );

        File.WriteAllText(first.bindingsPath, "tampered");
        NativeBindingGenerationDescriptor repaired = await GenerateAsync();
        Assert.Equal(first.fingerprint, repaired.fingerprint);
        Assert.Equal(original, File.ReadAllText(repaired.bindingsPath));

        File.WriteAllText(Path.Combine(m_root, "Source", "api.h"), "long long sample(long long value);");
        await Assert.ThrowsAsync<InvalidOperationException>(() => GenerateAsync(expected: first.fingerprint));
        NativeBindingGenerationDescriptor changed = await GenerateAsync();
        Assert.NotEqual(first.fingerprint, changed.fingerprint);
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.NotEqual(original, File.ReadAllText(changed.bindingsPath));
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task UnselectedPlatformDoesNotInvalidateTheSelectedGeneration()
    {
        string config = CreateConfig("int sample(int value);");
        var owner = new NativeComponentDescriptor(
            "fixture",
            "Fixture.csproj",
            "Fixture.csproj",
            bindingDefinition: "bindings.props"
        );
        var request = new NativeBindingGenerationRequest(
            [owner],
            "fixture-target",
            NativeBindingOutputMode.TargetArtifacts
        );
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string definitionPath = Path.Combine(m_root, "bindings.props");
        XDocument definition = XDocument.Load(definitionPath);
        File.WriteAllText(Path.Combine(m_root, "UnselectedPlatform.cs"), "unrelated target policy");

        var unchangedContext = new NativeBuildContext(m_root, "release");
        using (NativeBindingGenerationResult unchanged = await BindingTestProfiles.Generator("fixture-target")
            .GenerateAsync(unchangedContext, request, CancellationToken.None)
        ) {
            Assert.Equal(first.fingerprint, unchanged[owner.nativeProject].fingerprint);
            Assert.Equal(0, unchangedContext.statistics.bindingGenerations);
        }

        File.WriteAllText(
            Path.Combine(m_root, "selected.json"),
            File.ReadAllText(config).Replace("Fixture.Native", "Fixture.Selected", StringComparison.Ordinal)
        );
        definition.Root!.Element("PropertyGroup")!.Element("InnoBindingConfig")!.Value = "selected.json";
        definition.Save(definitionPath);
        NativeBindingGenerationDescriptor changed = await GenerateAsync();
        Assert.NotEqual(first.fingerprint, changed.fingerprint);
        Assert.Contains("Fixture.Selected", File.ReadAllText(changed.bindingsPath));
        Assert.Contains("Fixture.Native", File.ReadAllText(first.bindingsPath));
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task CppBridgeAndManagedBindingsShareOneCompletedGeneration()
    {
        string config = CreateConfig("int sample(int value);");
        string facade = Path.Combine(m_root, "Source", "facade.hpp");
        File.WriteAllText(
            facade,
            "class Counter { public: explicit Counter(int value) : value_(value) {} int Add(int value) { return value_ + value; } private: int value_; };"
        );
        WriteCppConfig(config);
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        NativeBindingCompilation compilation = NativeBindingCompilation.Load(first.nativeDirectory);
        Assert.Contains(compilation.files, file => file.compile && file.rootId == "generated");
        Assert.Contains(
            compilation.files,
            file => !file.compile && file.rootId == "generated" && file.relativePath.EndsWith(".h", StringComparison.Ordinal)
        );
        Assert.Equal(
            Path.GetDirectoryName(first.nativeDirectory),
            Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath))
        );
        Assert.False(Directory.Exists(Path.Combine(m_root, "declared")));
        string original = File.ReadAllText(first.bindingsPath);
        File.WriteAllText(facade, "class Counter { invalid C++; };");
        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync());
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "artifacts", "bindings", "fixture", "fixture-target")));
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task HostBindingFailurePreservesBothPublishedOutputTrees()
    {
        string config = CreateConfig("int sample(int value);");
        string facade = Path.Combine(m_root, "Source", "facade.hpp");
        File.WriteAllText(facade, "class Counter { public: int Value() { return 7; } };");
        WriteCppConfig(config);
        NativeBindingGenerationDescriptor initial = await GenerateAsync(mode: NativeBindingOutputMode.HostSource);
        string source = File.ReadAllText(initial.bindingsPath);
        NativeBindingCompilation compilation = NativeBindingCompilation.Load(initial.nativeDirectory);
        string nativeHeader = Path.Combine(
            initial.nativeDirectory,
            compilation.files.First(file => (
                file.rootId == "generated" && file.relativePath.EndsWith(".h", StringComparison.Ordinal)
            )).relativePath
        );
        string header = File.ReadAllText(nativeHeader);
        string nativeSource = Path.Combine(
            initial.nativeDirectory,
            compilation.files.First(file => (file.rootId == "generated" && file.compile)).relativePath
        );
        string implementation = File.ReadAllText(nativeSource);
        File.WriteAllText(facade, "class Counter { public: int Changed() { return 9; } };");
        File.WriteAllText(config, File.ReadAllText(config).Replace("facade.hpp", "missing.hpp", StringComparison.Ordinal));

        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync(mode: NativeBindingOutputMode.HostSource));

        Assert.Equal(source, File.ReadAllText(initial.bindingsPath));
        Assert.Equal(header, File.ReadAllText(nativeHeader));
        Assert.Equal(implementation, File.ReadAllText(nativeSource));
        Assert.Empty(Directory.GetDirectories(m_root, ".bgcs-staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task FailedOrCanceledGenerationKeepsTheLastCompleteBundle()
    {
        CreateConfig("int sample(int value);");
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string original = File.ReadAllText(first.bindingsPath);
        File.WriteAllText(Path.Combine(m_root, "Source", "api.h"), "this is not valid C;");
        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync());
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => GenerateAsync(cancellationToken: cancellation.Token));
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "artifacts", "bindings", "fixture", "fixture-target")));
    }

    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task CheckOnlyDetectsModifiedOutputsWithoutPublishing()
    {
        CreateConfig("int sample(int value);");
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        await GenerateAsync(checkOnly: true);
        File.WriteAllText(first.bindingsPath, "tampered");
        await Assert.ThrowsAsync<InvalidDataException>(() => GenerateAsync(checkOnly: true));
        Assert.Equal("tampered", File.ReadAllText(first.bindingsPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            foreach (string file in Directory.EnumerateFiles(m_root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }
            Directory.Delete(m_root, recursive: true);
        }
    }

    private string CreateConfig(string header)
    {
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
        File.WriteAllText(Path.Combine(m_root, "Source", "api.h"), header);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "Fixture.csproj"), "<Project />");
        WriteDefinition();
        string config = Path.Combine(m_root, "bindgen.json");
        File.WriteAllText(
            config,
            JsonSerializer.Serialize(new
            {
                schemaVersion = 2,
                library = new { id = "fixture", nativeName = "fixture" },
                source = new { language = "c", headers = new[] { "Source/api.h" } },
                api = new { @namespace = "Fixture.Native", name = "Api" },
                output = new { root = "Generated", layout = "singleFile" }
            })
        );
        return config;
    }

    private async Task<NativeBindingGenerationDescriptor> GenerateAsync(
        NativeBindingOutputMode mode = NativeBindingOutputMode.TargetArtifacts,
        bool checkOnly = false,
        string? expected = null,
        CancellationToken cancellationToken = default
    ) {
        var owner = new NativeComponentDescriptor(
            "fixture",
            "Fixture.csproj",
            "Fixture.csproj",
            bindingDefinition: "bindings.props"
        );
        Dictionary<string, string>? fingerprints = expected is null
                                                   ? null
                                                   : new Dictionary<string, string> { [owner.nativeProject] = expected };
        var request = new NativeBindingGenerationRequest([owner], "fixture-target", mode, checkOnly, fingerprints);
        using NativeBindingGenerationResult result = await BindingTestProfiles.Generator("fixture-target").GenerateAsync(
            new NativeBuildContext(m_root, "release"),
            request,
            cancellationToken
        );
        return result[owner.nativeProject];
    }

    private void WriteDefinition()
        => new XDocument(
            new XElement(
                "Project",
                new XElement(
                    "PropertyGroup",
                    new XElement("InnoNativeComponentId", "fixture"),
                    new XElement("InnoBindingConfig", "bindgen.json")
                )
            )
        )
            .Save(Path.Combine(m_root, "bindings.props"));

    private static NativeBindingGenerationDescriptor SelectedGeneration(
        string fingerprint,
        bool usesRuntimeImports = false
    ) => new()
    {
        fingerprint = fingerprint,
        artifactDirectory = Path.Combine(Path.GetTempPath(), "SelectedGeneration", fingerprint),
        artifactOwnerDirectory = Path.Combine(Path.GetTempPath(), "SelectedGeneration"),
        bindingsPath = Path.Combine(Path.GetTempPath(), "SelectedGeneration", fingerprint, "Generated", "Bindings.cs"),
        usesRuntimeImports = usesRuntimeImports,
        nativeDirectory = string.Empty
    };

    private static void WriteCppConfig(string path)
        => File.WriteAllText(
            path,
            JsonSerializer.Serialize(new
            {
                schemaVersion = 2,
                library = new { id = "fixture", nativeName = "fixture" },
                source = new { language = "cpp", languageStandard = "c++20", headers = new[] { "Source/facade.hpp" } },
                api = new { @namespace = "Fixture.Native", name = "Api" },
                output = new { root = "Generated", layout = "singleFile" }
            })
        );
}
