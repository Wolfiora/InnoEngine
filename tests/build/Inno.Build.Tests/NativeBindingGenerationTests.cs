using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Inno.Build.Tasks;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBindingGenerationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeBindingTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public void SourceChangesSelectIndependentGenerationsAndTamperingIsRepaired()
    {
        string config = CreateConfig("int sample(int value);");
        RecordingBuildEngine engine = new();
        GenerateBindingsTask first = CreateTask(config, engine);
        Assert.True(first.Execute(), string.Join(Environment.NewLine, engine.errors));
        string original = File.ReadAllText(first.GeneratedBindings);
        Assert.Contains(Path.Combine(first.GenerationFingerprint, "Generated", "Bindings.cs"), first.GeneratedBindings);
        NativeBindingGenerationDescriptor descriptor = NativeBindingGenerationDescriptor.Load(first.DescriptorOutputPath);
        Assert.Equal(first.GeneratedBindings, descriptor.bindingsPath);
        Assert.True(BuildArtifactManifest.IsComplete(Path.GetDirectoryName(Path.GetDirectoryName(first.GeneratedBindings))!,
            first.GenerationFingerprint, ["Native", "Generated"]));

        File.WriteAllText(first.GeneratedBindings, "tampered");
        GenerateBindingsTask repaired = CreateTask(config, engine);
        Assert.True(repaired.Execute(), string.Join(Environment.NewLine, engine.errors));
        Assert.Equal(first.GenerationFingerprint, repaired.GenerationFingerprint);
        Assert.Equal(original, File.ReadAllText(repaired.GeneratedBindings));

        File.WriteAllText(Path.Combine(m_root, "api.h"), "long sample(long value);");
        GenerateBindingsTask stale = CreateTask(config, engine);
        stale.ExpectedFingerprint = first.GenerationFingerprint;
        Assert.False(stale.Execute());
        Assert.False(File.Exists(stale.DescriptorOutputPath));
        GenerateBindingsTask changed = CreateTask(config, engine);
        Assert.True(changed.Execute(), string.Join(Environment.NewLine, engine.errors));
        Assert.NotEqual(first.GenerationFingerprint, changed.GenerationFingerprint);
        Assert.Equal(original, File.ReadAllText(first.GeneratedBindings));
        Assert.NotEqual(original, File.ReadAllText(changed.GeneratedBindings));
    }

    [Fact]
    public void CppBridgeAndManagedBindingsShareOneCompletedGeneration()
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
        RecordingBuildEngine engine = new();
        GenerateBindingsTask first = CreateTask(config, engine);
        first.BridgeConfigPath = bridge;
        Assert.True(first.Execute(), string.Join(Environment.NewLine, engine.errors));
        Assert.True(File.Exists(Path.Combine(first.NativeBridgeDirectory, "include", "Classes.h")));
        Assert.True(File.Exists(Path.Combine(first.NativeBridgeDirectory, "src", "Classes.cpp")));
        Assert.Equal(Path.GetDirectoryName(first.NativeBridgeDirectory),
            Path.GetDirectoryName(Path.GetDirectoryName(first.GeneratedBindings)));
        Assert.False(Directory.Exists(Path.Combine(m_root, "declared")));
        string original = File.ReadAllText(first.GeneratedBindings);
        File.WriteAllText(facade, "class Counter { invalid C++; };");
        GenerateBindingsTask failed = CreateTask(config, engine);
        failed.BridgeConfigPath = bridge;
        Assert.False(failed.Execute());
        Assert.Equal(original, File.ReadAllText(first.GeneratedBindings));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "obj", "fixture-target")));
    }

    [Fact]
    public void HostBindingFailurePreservesBothPublishedOutputTrees()
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
            {"preset":"c-library","namespace":"Fixture.Native","apiName":"Api","libName":"fixture",
             "entryFiles":["Native/Generated/include/Classes.h"],"allowedHeaders":["Native/Generated/include/Classes.h"],
             "includeFolders":["Native/Generated/include"],"generateRuntimeSource":false,
             "mergeGeneratedFilesToSingleFile":true,"singleFileOutputName":"Bindings.cs"}
            """);
        RecordingBuildEngine engine = new();
        GenerateBindingsTask initial = CreateTask(config, engine);
        initial.TargetOutputRoot = string.Empty;
        initial.BridgeConfigPath = bridge;
        Assert.True(initial.Execute(), string.Join(Environment.NewLine, engine.errors));
        string source = File.ReadAllText(initial.GeneratedBindings);
        string nativeHeader = Path.Combine(initial.NativeBridgeDirectory, "include", "Classes.h");
        string header = File.ReadAllText(nativeHeader);
        string nativeSource = Path.Combine(initial.NativeBridgeDirectory, "src", "Classes.cpp");
        string implementation = File.ReadAllText(nativeSource);
        File.WriteAllText(facade, "class Counter { public: int Changed() { return 9; } };");
        File.WriteAllText(config, File.ReadAllText(config).Replace("Classes.h", "missing.h", StringComparison.Ordinal));

        GenerateBindingsTask failed = CreateTask(config, engine);
        failed.TargetOutputRoot = string.Empty;
        failed.BridgeConfigPath = bridge;
        Assert.False(failed.Execute());

        Assert.Equal(source, File.ReadAllText(initial.GeneratedBindings));
        Assert.Equal(header, File.ReadAllText(nativeHeader));
        Assert.Equal(implementation, File.ReadAllText(nativeSource));
        Assert.False(File.Exists(failed.DescriptorOutputPath));
        Assert.Empty(Directory.GetDirectories(m_root, ".bgcs-staging-*", SearchOption.AllDirectories));
    }

    [Fact]
    public void FailedOrCanceledGenerationKeepsTheLastCompleteBundle()
    {
        string config = CreateConfig("int sample(int value);");
        RecordingBuildEngine engine = new();
        GenerateBindingsTask first = CreateTask(config, engine);
        Assert.True(first.Execute(), string.Join(Environment.NewLine, engine.errors));
        string original = File.ReadAllText(first.GeneratedBindings);
        File.WriteAllText(Path.Combine(m_root, "api.h"), "this is not valid C;");
        GenerateBindingsTask failed = CreateTask(config, engine);
        Assert.False(failed.Execute());
        Assert.NotEmpty(engine.errors);
        Assert.Equal(original, File.ReadAllText(first.GeneratedBindings));
        GenerateBindingsTask canceled = CreateTask(config, engine);
        canceled.Cancel();
        Assert.False(canceled.Execute());
        Assert.Equal(original, File.ReadAllText(first.GeneratedBindings));
        Assert.False(File.Exists(canceled.DescriptorOutputPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "obj", "fixture-target")));
    }

    [Fact]
    public void CheckOnlyDetectsModifiedOutputsWithoutPublishing()
    {
        string config = CreateConfig("int sample(int value);");
        RecordingBuildEngine engine = new();
        GenerateBindingsTask first = CreateTask(config, engine);
        Assert.True(first.Execute(), string.Join(Environment.NewLine, engine.errors));
        GenerateBindingsTask check = CreateTask(config, engine);
        check.CheckOnly = true;
        Assert.True(check.Execute(), string.Join(Environment.NewLine, engine.errors));
        File.WriteAllText(first.GeneratedBindings, "tampered");
        GenerateBindingsTask changed = CreateTask(config, engine);
        changed.CheckOnly = true;
        Assert.False(changed.Execute());
        Assert.Equal("tampered", File.ReadAllText(first.GeneratedBindings));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private string CreateConfig(string header)
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "api.h"), header);
        string config = Path.Combine(m_root, "bindgen.json");
        File.WriteAllText(config, JsonSerializer.Serialize(new {
            Preset = "c-library", Namespace = "Fixture.Native", ApiName = "Api", LibName = "fixture",
            TargetId = "linux-x64-gnu", EntryFiles = new[] { "api.h" }, AllowedHeaders = new[] { "api.h" },
            GenerateRuntimeSource = false, MergeGeneratedFilesToSingleFile = true, SingleFileOutputName = "Bindings.cs"
        }));
        return config;
    }

    private GenerateBindingsTask CreateTask(
        string config,
        RecordingBuildEngine engine
    ) => new() {
        EngineRoot = m_root, ConfigPath = config, OutputDirectory = Path.Combine(m_root, "Generated"),
        TargetOutputRoot = Path.Combine(m_root, "obj", "fixture-target"), BuildEngine = engine,
        DescriptorOutputPath = Path.Combine(m_root, "requests", Guid.NewGuid().ToString("N") + ".json")
    };

    private sealed class RecordingBuildEngine : IBuildEngine
    {
        public List<string> errors { get; } = [];
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => "fixture.proj";

        public void LogErrorEvent(BuildErrorEventArgs e) => errors.Add(e.Message ?? string.Empty);
        public void LogWarningEvent(BuildWarningEventArgs e) { }
        public void LogMessageEvent(BuildMessageEventArgs e) { }
        public void LogCustomEvent(CustomBuildEventArgs e) { }
        public bool BuildProjectFile(
            string projectFileName,
            string[] targetNames,
            IDictionary globalProperties,
            IDictionary targetOutputs
        ) => throw new NotSupportedException();
    }
}
