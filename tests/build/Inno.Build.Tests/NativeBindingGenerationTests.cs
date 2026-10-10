using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBindingGenerationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeBindingTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void ManagedSelectionPreservesExactNativeIdentitiesInStableProjectOrder()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        NativeBindingGenerationDescriptor.WriteSelection(output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj"] = SelectedGeneration("text-fingerprint"),
                ["backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj"] = SelectedGeneration("bgfx-fingerprint")
            }, new Dictionary<string, NativeLibraryKind>
            {
                ["backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj"] = NativeLibraryKind.Static,
                ["backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj"] = NativeLibraryKind.Shared
            });

        XElement[] groups = XDocument.Load(output).Root!.Elements("PropertyGroup").ToArray();
        Assert.Equal(2, groups.Length);
        Assert.Equal("'$(MSBuildProjectName)' == 'Inno.Native.Bgfx'", groups[0].Attribute("Condition")!.Value);
        Assert.Equal("bgfx-fingerprint", groups[0].Element("BindGenExpectedFingerprint")!.Value);
        Assert.Equal(Path.GetDirectoryName(SelectedGeneration("bgfx-fingerprint").bindingsPath),
            groups[0].Element("BindGenOutputDirectory")!.Value);
        Assert.Equal("text-fingerprint", groups[1].Element("BindGenExpectedFingerprint")!.Value);
        Assert.Equal("Shared", groups[0].Element("InnoNativeLibraryKind")!.Value);
        Assert.Equal("Static", groups[1].Element("InnoNativeLibraryKind")!.Value);
    }

    [Fact]
    public void PublicationRejectsChangedOrIncompleteManagedSelection()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        var original = new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            ["Inno.Native.Text.csproj"] = SelectedGeneration("original")
        };
        var linkage = new Dictionary<string, NativeLibraryKind> { ["Inno.Native.Text.csproj"] = NativeLibraryKind.Shared };
        NativeBindingGenerationDescriptor.WriteSelection(output, original, linkage);
        NativeBindingGenerationDescriptor.ValidateSelection(output, original, linkage);
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["Inno.Native.Text.csproj"] = SelectedGeneration("changed")
            }, linkage));
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(output,
            new Dictionary<string, NativeBindingGenerationDescriptor>(), new Dictionary<string, NativeLibraryKind>()));
        Assert.Throws<InvalidDataException>(() => NativeBindingGenerationDescriptor.ValidateSelection(output, original,
            new Dictionary<string, NativeLibraryKind> { ["Inno.Native.Text.csproj"] = NativeLibraryKind.Static }));
        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(output, original,
            new Dictionary<string, NativeLibraryKind>()));
        Assert.Equal("original", XDocument.Load(output).Root!.Element("PropertyGroup")!
            .Element("BindGenExpectedFingerprint")!.Value);
    }

    [Fact]
    public void CollidingManagedSelectionDoesNotReplaceAnExistingCompleteSelection()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        Directory.CreateDirectory(m_root);
        File.WriteAllText(output, "previous complete selection");

        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["first/Inno.Native.Text.csproj"] = SelectedGeneration("first"),
                ["second/inno.native.text.csproj"] = SelectedGeneration("second")
            }, new Dictionary<string, NativeLibraryKind>
            {
                ["first/Inno.Native.Text.csproj"] = NativeLibraryKind.Shared,
                ["second/inno.native.text.csproj"] = NativeLibraryKind.Shared
            }));
        Assert.Equal("previous complete selection", File.ReadAllText(output));
    }

    [Fact]
    public void UnsafeManagedProjectNameIsRejectedBeforeCreatingSelectionOutput()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        Assert.Throws<ArgumentException>(() => NativeBindingGenerationDescriptor.WriteSelection(output,
            new Dictionary<string, NativeBindingGenerationDescriptor>
            {
                ["Inno'Injected.csproj"] = SelectedGeneration("selected")
            }, new Dictionary<string, NativeLibraryKind> { ["Inno'Injected.csproj"] = NativeLibraryKind.Shared }));
        Assert.False(Directory.Exists(m_root));
    }

    [Fact]
    public async Task SourceChangesSelectIndependentGenerationsAndTamperingIsRepaired()
    {
        CreateConfig("int sample(int value);");
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string original = File.ReadAllText(first.bindingsPath);
        Assert.Contains(Path.Combine(first.fingerprint, "Generated", "Bindings.cs"), first.bindingsPath);
        string descriptorPath = Path.Combine(m_root, "requests", "descriptor.json");
        first.Write(descriptorPath);
        Assert.Equal(first.bindingsPath, NativeBindingGenerationDescriptor.Load(descriptorPath).bindingsPath);
        Assert.True(BuildArtifactManifest.IsComplete(Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath))!,
            first.fingerprint, ["Native", "Generated"]));

        File.WriteAllText(first.bindingsPath, "tampered");
        NativeBindingGenerationDescriptor repaired = await GenerateAsync();
        Assert.Equal(first.fingerprint, repaired.fingerprint);
        Assert.Equal(original, File.ReadAllText(repaired.bindingsPath));

        File.WriteAllText(Path.Combine(m_root, "api.h"), "long sample(long value);");
        await Assert.ThrowsAsync<InvalidOperationException>(() => GenerateAsync(expected: first.fingerprint));
        NativeBindingGenerationDescriptor changed = await GenerateAsync();
        Assert.NotEqual(first.fingerprint, changed.fingerprint);
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.NotEqual(original, File.ReadAllText(changed.bindingsPath));
    }

    [Fact]
    public async Task CppBridgeAndManagedBindingsShareOneCompletedGeneration()
    {
        string config = CreateConfig("int sample(int value);");
        string facade = Path.Combine(m_root, "facade.hpp");
        File.WriteAllText(facade, "class Counter { public: explicit Counter(int value) : value_(value) {} int Add(int value) { return value_ + value; } private: int value_; };");
        string bridge = Path.Combine(m_root, "bridge.json");
        File.WriteAllText(bridge, JsonSerializer.Serialize(new {
            EntryFiles = new[] { "facade.hpp" }, AllowedHeaders = new[] { "facade.hpp" },
            IncludeFolders = new[] { m_root }, OutputPath = "declared", NamePrefix = "fixture_",
            LanguageStandard = "c++20", GenerateBuildManifest = false, GenerateCSharpBindings = false,
            ParseSystemIncludes = false
        }));
        File.WriteAllText(config, JsonSerializer.Serialize(new {
            Preset = "c-library", Namespace = "Fixture.Native", ApiName = "Api", LibName = "fixture",
            EntryFiles = new[] { "declared/include/Classes.h" }, AllowedHeaders = new[] { "declared/include/Classes.h" },
            IncludeFolders = new[] { "declared/include" }, GenerateRuntimeSource = false,
            MergeGeneratedFilesToSingleFile = true, SingleFileOutputName = "Bindings.cs"
        }));
        WriteDefinition(bridge);
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        Assert.True(File.Exists(Path.Combine(first.bridgeDirectory, "include", "Classes.h")));
        Assert.True(File.Exists(Path.Combine(first.bridgeDirectory, "src", "Classes.cpp")));
        Assert.Equal(Path.GetDirectoryName(first.bridgeDirectory),
            Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath)));
        Assert.False(Directory.Exists(Path.Combine(m_root, "declared")));
        string original = File.ReadAllText(first.bindingsPath);
        File.WriteAllText(facade, "class Counter { invalid C++; };");
        WriteDefinition(bridge);
        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync());
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "obj", "fixture-target")));
    }

    [Fact]
    public async Task HostBindingFailurePreservesBothPublishedOutputTrees()
    {
        string config = CreateConfig("int sample(int value);");
        string facade = Path.Combine(m_root, "facade.hpp");
        File.WriteAllText(facade, "class Counter { public: int Value() { return 7; } };");
        string bridge = Path.Combine(m_root, "bridge.json");
        File.WriteAllText(bridge, """
            {"entryFiles":["facade.hpp"],"allowedHeaders":["facade.hpp"],"outputPath":"Native/Generated",
             "languageStandard":"c++20","generateBuildManifest":false,"generateCSharpBindings":false}
            """);
        File.WriteAllText(config, """
            {"preset":"c-library","namespace":"Fixture.Native","apiName":"Api","libName":"fixture","outputPath":"Generated",
             "entryFiles":["Native/Generated/include/Classes.h"],"allowedHeaders":["Native/Generated/include/Classes.h"],
             "includeFolders":["Native/Generated/include"],"generateRuntimeSource":false,
             "mergeGeneratedFilesToSingleFile":true,"singleFileOutputName":"Bindings.cs"}
            """);
        WriteDefinition(bridge);
        NativeBindingGenerationDescriptor initial = await GenerateAsync(mode: NativeBindingOutputMode.HostSource);
        string source = File.ReadAllText(initial.bindingsPath);
        string nativeHeader = Path.Combine(initial.bridgeDirectory, "include", "Classes.h");
        string header = File.ReadAllText(nativeHeader);
        string nativeSource = Path.Combine(initial.bridgeDirectory, "src", "Classes.cpp");
        string implementation = File.ReadAllText(nativeSource);
        File.WriteAllText(facade, "class Counter { public: int Changed() { return 9; } };");
        File.WriteAllText(config, File.ReadAllText(config).Replace("Classes.h", "missing.h", StringComparison.Ordinal));

        WriteDefinition(bridge);
        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync(mode: NativeBindingOutputMode.HostSource));

        Assert.Equal(source, File.ReadAllText(initial.bindingsPath));
        Assert.Equal(header, File.ReadAllText(nativeHeader));
        Assert.Equal(implementation, File.ReadAllText(nativeSource));
        Assert.Empty(Directory.GetDirectories(m_root, ".bgcs-staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task FailedOrCanceledGenerationKeepsTheLastCompleteBundle()
    {
        CreateConfig("int sample(int value);");
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string original = File.ReadAllText(first.bindingsPath);
        File.WriteAllText(Path.Combine(m_root, "api.h"), "this is not valid C;");
        await Assert.ThrowsAnyAsync<Exception>(() => GenerateAsync());
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => GenerateAsync(cancellation: cancellation.Token));
        Assert.Equal(original, File.ReadAllText(first.bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "obj", "fixture-target")));
    }

    [Fact]
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
            Directory.Delete(m_root, recursive: true);
    }

    private static NativeBindingGenerationDescriptor SelectedGeneration(string fingerprint)
        => new()
        {
            fingerprint = fingerprint,
            bindingsPath = Path.Combine(Path.GetTempPath(), "SelectedGeneration", fingerprint, "Generated", "Bindings.cs"),
            bridgeDirectory = string.Empty
        };

    private string CreateConfig(string header)
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "api.h"), header);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "Fixture.csproj"), "<Project />");
        WriteDefinition();
        string config = Path.Combine(m_root, "bindgen.json");
        File.WriteAllText(config, JsonSerializer.Serialize(new {
            Preset = "c-library", Namespace = "Fixture.Native", ApiName = "Api", LibName = "fixture",
            TargetId = "linux-x64-gnu", EntryFiles = new[] { "api.h" }, AllowedHeaders = new[] { "api.h" },
            GenerateRuntimeSource = false, MergeGeneratedFilesToSingleFile = true, SingleFileOutputName = "Bindings.cs"
        }));
        return config;
    }

    private async Task<NativeBindingGenerationDescriptor> GenerateAsync(
        NativeBindingOutputMode mode = NativeBindingOutputMode.TargetArtifacts,
        bool checkOnly = false,
        string? expected = null,
        CancellationToken cancellation = default
    ) {
        var owner = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
        var fingerprints = expected is null ? null : new Dictionary<string, string> { [owner.nativeProject] = expected };
        var request = new NativeBindingGenerationRequest([owner], "fixture-target", mode, checkOnly, fingerprints);
        var result = await new NativeBindingGenerator("dotnet").GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, cancellation);
        return result[owner.nativeProject];
    }

    private void WriteDefinition(string? bridge = null)
    {
        new XDocument(new XElement("Project",
            new XElement("PropertyGroup", new XElement("InnoBindingHostConfig", "bindgen.json"),
                bridge is null ? null : new XElement("InnoBindingHostBridge", Path.GetFileName(bridge))),
            new XElement("ItemGroup", new XElement("InnoBindingTarget", new XAttribute("Include", "fixture-target"),
                new XElement("Config", "bindgen.json"),
                bridge is null ? null : new XElement("Bridge", Path.GetFileName(bridge))))))
            .Save(Path.Combine(m_root, "bindings.props"));
    }

}
