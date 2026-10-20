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
    public void IdenticalSelectionPreservesTheImportTimestampAndChangedSelectionReplacesIt()
    {
        string output = Path.Combine(m_root, "BindingSelection.props");
        var generations = new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            ["Inno.Native.Text.csproj"] = SelectedGeneration("selected")
        };
        var linkage = new Dictionary<string, NativeLibraryKind> { ["Inno.Native.Text.csproj"] = NativeLibraryKind.Shared };
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
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
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
        Assert.True(BuildArtifactManifest.IsComplete(Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath))!,
            first.fingerprint, ["Native", "Generated"]));

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
    public async Task UnselectedTargetMappingDoesNotInvalidateTheSelectedGeneration()
    {
        string config = CreateConfig("int sample(int value);");
        var owner = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
        var request = new NativeBindingGenerationRequest([owner], "fixture-target", NativeBindingOutputMode.TargetArtifacts);
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        string definitionPath = Path.Combine(m_root, "bindings.props");
        XDocument definition = XDocument.Load(definitionPath);
        definition.Root!.Element("ItemGroup")!.Add(new XElement("InnoBindingTarget",
            new XAttribute("Include", "unselected-target"), new XElement("Config", "unselected.json")));
        definition.Save(definitionPath);

        var unchangedContext = new NativeBuildContext(m_root, "release");
        using (NativeBindingGenerationResult unchanged = await new NativeBindingGenerator()
            .GenerateAsync(unchangedContext, request, CancellationToken.None))
        {
            Assert.Equal(first.fingerprint, unchanged[owner.nativeProject].fingerprint);
            Assert.Equal(0, unchangedContext.statistics.bindingGenerations);
        }

        File.WriteAllText(Path.Combine(m_root, "selected.json"),
            File.ReadAllText(config).Replace("Fixture.Native", "Fixture.Selected", StringComparison.Ordinal));
        definition.Root.Element("ItemGroup")!.Elements("InnoBindingTarget")
            .Single(target => target.Attribute("Include")!.Value == "fixture-target")
            .Element("Config")!.Value = "selected.json";
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
        File.WriteAllText(facade, "class Counter { public: explicit Counter(int value) : value_(value) {} int Add(int value) { return value_ + value; } private: int value_; };");
        WriteCppConfig(config);
        NativeBindingGenerationDescriptor first = await GenerateAsync();
        NativeBindingCompilation compilation = NativeBindingCompilation.Load(first.nativeDirectory);
        Assert.Contains(compilation.files, file => file.compile && file.rootId == "generated");
        Assert.Contains(compilation.files, file => !file.compile && file.rootId == "generated" && file.relativePath.EndsWith(".h", StringComparison.Ordinal));
        Assert.Equal(Path.GetDirectoryName(first.nativeDirectory),
            Path.GetDirectoryName(Path.GetDirectoryName(first.bindingsPath)));
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
        string nativeHeader = Path.Combine(initial.nativeDirectory, compilation.files.First(file =>
            file.rootId == "generated" && file.relativePath.EndsWith(".h", StringComparison.Ordinal)).relativePath);
        string header = File.ReadAllText(nativeHeader);
        string nativeSource = Path.Combine(initial.nativeDirectory, compilation.files.First(file =>
            file.rootId == "generated" && file.compile).relativePath);
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
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => GenerateAsync(cancellation: cancellation.Token));
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
            Directory.Delete(m_root, recursive: true);
    }

    private static NativeBindingGenerationDescriptor SelectedGeneration(string fingerprint)
        => new()
        {
            fingerprint = fingerprint,
            artifactDirectory = Path.Combine(Path.GetTempPath(), "SelectedGeneration", fingerprint),
            bindingsPath = Path.Combine(Path.GetTempPath(), "SelectedGeneration", fingerprint, "Generated", "Bindings.cs"),
            nativeDirectory = string.Empty
        };

    private string CreateConfig(string header)
    {
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
        File.WriteAllText(Path.Combine(m_root, "Source", "api.h"), header);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "Fixture.csproj"), "<Project />");
        WriteDefinition();
        string config = Path.Combine(m_root, "bindgen.json");
        File.WriteAllText(config, JsonSerializer.Serialize(new {
            schemaVersion = 1,
            library = new { id = "fixture", nativeName = "fixture" },
            source = new { language = "c", headers = new[] { "Source/api.h" } },
            api = new { @namespace = "Fixture.Native", name = "Api" },
            output = new { root = "Generated", layout = "singleFile" }
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
        using var result = await new NativeBindingGenerator().GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, cancellation);
        return result[owner.nativeProject];
    }

    private void WriteDefinition()
    {
        new XDocument(new XElement("Project",
            new XElement("PropertyGroup", new XElement("InnoNativeComponentId", "fixture"), new XElement("InnoBindingHostConfig", "bindgen.json")),
            new XElement("ItemGroup", new XElement("InnoBindingTarget", new XAttribute("Include", "fixture-target"),
                new XElement("Config", "bindgen.json")))))
            .Save(Path.Combine(m_root, "bindings.props"));
    }

    private static void WriteCppConfig(string path)
    {
        File.WriteAllText(path, JsonSerializer.Serialize(new
        {
            schemaVersion = 1,
            library = new { id = "fixture", nativeName = "fixture" },
            source = new { language = "cpp", languageStandard = "c++20", headers = new[] { "Source/facade.hpp" } },
            api = new { @namespace = "Fixture.Native", name = "Api" },
            output = new { root = "Generated", layout = "singleFile" }
        }));
    }

}
