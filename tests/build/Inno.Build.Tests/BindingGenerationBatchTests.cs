using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BindingGenerationBatchTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingBatch", Guid.NewGuid().ToString("N"));

    public BindingGenerationBatchTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "shared.h"), "int sample(int value);");
    }

    [Fact]
    public async Task SourceInputIdentityDetectsSelectedInputsWithoutGeneratingOrReadingUnselectedTargets()
    {
        NativeComponentDescriptor component = CreateComponent("planned");
        string directory = Path.Combine(m_root, "planned");
        string config = Path.Combine(directory, "bindgen.json");
        File.WriteAllText(Path.Combine(directory, "common.json"), "{\"apiName\":\"UnusedDefault\"}");
        string text = File.ReadAllText(config);
        File.WriteAllText(config, text.Replace("{\"preset\"", "{\"baseConfig\":{\"url\":\"file://common.json\"},\"preset\"", StringComparison.Ordinal));
        string unselected = Path.Combine(directory, "bindgen.unselected.json");
        File.WriteAllText(unselected, "{}");
        var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);
        var generator = new NativeBindingGenerator("dotnet");
        NativeBuildContext initial = new(m_root, "release");
        string identity = await generator.ComputeInputFingerprintAsync(initial, request, CancellationToken.None);
        File.WriteAllText(unselected, "{\"changed\":true}");
        Assert.Equal(identity, await generator.ComputeInputFingerprintAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None));
        string shared = Path.Combine(m_root, "shared.h");
        DateTime timestamp = File.GetLastWriteTimeUtc(shared);
        File.WriteAllText(shared, File.ReadAllText(shared).Replace("sample", "change", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(shared, timestamp);
        Assert.NotEqual(identity, await generator.ComputeInputFingerprintAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None));
        Assert.Equal(0, initial.statistics.bindingGenerations);
        Assert.Equal(0, initial.statistics.bindingBatches);
        Assert.False(Directory.Exists(Path.Combine(directory, "obj")));
        Assert.False(Directory.Exists(Path.Combine(directory, "Generated")));
    }

    [Fact]
    public async Task OneBatchReusesSharedReadsOnlyWithinEachFreshPhase()
    {
        var components = new[] { CreateComponent("first"), CreateComponent("second") };
        var context = new NativeBuildContext(m_root, "release");
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest(components, "fixture", NativeBindingOutputMode.TargetArtifacts);
        using var result = await generator.GenerateAsync(context, request, CancellationToken.None);
        Assert.Equal(2, result.Count);
        Assert.Equal(1, context.statistics.bindingBatches);
        Assert.Equal(2, context.statistics.bindingGenerations);
        Assert.Contains(context.statistics.phases, phase => phase.phase == "binding-locks" && phase.reusedReads > 0);
        Assert.Contains(context.statistics.phases, phase => phase.phase == "binding-generation" && phase.reusedReads > 0);
        long expectedBytes = File.ReadAllBytes(Path.Combine(m_root, "shared.h")).Length;
        Assert.All(context.statistics.phases, phase => Assert.True(phase.bytes >= expectedBytes));
        var hot = new NativeBuildContext(m_root, "release");
        using var hotResult = await generator.GenerateAsync(hot, request, CancellationToken.None);
        Assert.Equal(0, hot.statistics.bindingGenerations);
        Assert.Equal(1, hot.statistics.bindingBatches);
        string header = Path.Combine(m_root, "shared.h");
        DateTime timestamp = File.GetLastWriteTimeUtc(header);
        File.WriteAllText(header, "int change(int value);");
        File.SetLastWriteTimeUtc(header, timestamp);
        using var changed = await generator.GenerateAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        Assert.All(result, entry => Assert.NotEqual(entry.Value.fingerprint, changed[entry.Key].fingerprint));
    }

    [Fact]
    public async Task ActiveConsumersBlockReplacementAndReleaseAllowsRepair()
    {
        var component = CreateComponent("reader");
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);
        var context = new NativeBuildContext(m_root, "release");
        using NativeBindingGenerationResult first = await generator.GenerateAsync(context, request, CancellationToken.None);
        NativeBindingGenerationDescriptor descriptor = first[component.nativeProject];
        string original = File.ReadAllText(descriptor.bindingsPath);

        using var waiting = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            using BuildArtifactWriteLease mutation = await BuildArtifactWriteLease.AcquireAsync(
                descriptor.artifactDirectory, waiting.Token);
        });
        using NativeBindingGenerationResult concurrent = await generator.GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        Assert.Equal(descriptor.fingerprint, concurrent[component.nativeProject].fingerprint);

        first.Dispose();
        concurrent.Dispose();
        Assert.Throws<ObjectDisposedException>(() => first[component.nativeProject]);
        using (BuildArtifactWriteLease mutation = await BuildArtifactWriteLease.AcquireAsync(descriptor.artifactDirectory))
            File.WriteAllText(descriptor.bindingsPath, "damaged");
        using NativeBindingGenerationResult repaired = await generator.GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        Assert.Equal(original, File.ReadAllText(repaired[component.nativeProject].bindingsPath));
    }

    [Fact]
    public async Task AFailedBatchPublishesNoPartialClosureAndPreservesEarlierOutputs()
    {
        var first = CreateComponent("first");
        var second = CreateComponent("second");
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest([first, second], "fixture", NativeBindingOutputMode.TargetArtifacts);
        using var completed = await generator.GenerateAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        string original = File.ReadAllText(completed[first.nativeProject].bindingsPath);
        File.WriteAllText(Path.Combine(m_root, "shared.h"), "int different(int value);");
        File.WriteAllText(Path.Combine(m_root, "second", "broken.h"), "invalid C grammar;");
        string config = Path.Combine(m_root, "second", "bindgen.json");
        File.WriteAllText(config, File.ReadAllText(config).Replace("../shared.h", "broken.h", StringComparison.Ordinal));
        await Assert.ThrowsAnyAsync<Exception>(async () => await generator.GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, CancellationToken.None));
        Assert.Equal(original, File.ReadAllText(completed[first.nativeProject].bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "artifacts", "bindings", "first", "fixture")));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await generator.GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, cancellation.Token));
    }

    [Fact]
    public async Task FrozenSdkPathsSelectIndependentHeadersWithoutChangingTheProcessEnvironment()
    {
        string variable = "INNO_BINDING_SDK_" + Guid.NewGuid().ToString("N");
        var component = CreateSdkComponent(variable);
        string firstSdk = CreateSdk("sdk-first", "int");
        string secondSdk = CreateSdk("sdk-second", "long long");
        string? original = Environment.GetEnvironmentVariable(variable);
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);

        using var first = await generator.GenerateAsync(SdkContext(variable, firstSdk), request, CancellationToken.None);
        using var second = await generator.GenerateAsync(SdkContext(variable, secondSdk), request, CancellationToken.None);

        Assert.NotEqual(first[component.nativeProject].fingerprint, second[component.nativeProject].fingerprint);
        Assert.Contains("int Sample(int value)", File.ReadAllText(first[component.nativeProject].bindingsPath));
        Assert.Contains("long Sample(long value)", File.ReadAllText(second[component.nativeProject].bindingsPath));
        Assert.Equal(original, Environment.GetEnvironmentVariable(variable));
    }

    [Fact]
    public async Task MissingFrozenSdkValueFailsBeforeCreatingCandidatesAndPreservesPublishedBindings()
    {
        string variable = "INNO_BINDING_SDK_" + Guid.NewGuid().ToString("N");
        var component = CreateSdkComponent(variable);
        string sdk = CreateSdk("sdk", "int");
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);
        using var result = await generator.GenerateAsync(SdkContext(variable, sdk), request, CancellationToken.None);
        string source = File.ReadAllText(result[component.nativeProject].bindingsPath);

        var failure = await Assert.ThrowsAsync<InvalidOperationException>(async () => await generator.GenerateAsync(
            new NativeBuildContext(m_root, "release"), request, CancellationToken.None));

        Assert.Contains(variable, failure.Message);
        Assert.Equal(source, File.ReadAllText(result[component.nativeProject].bindingsPath));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "artifacts", "bindings", "sdk-component", "fixture")));
        Assert.Empty(Directory.GetDirectories(m_root, ".bgcs-staging-*", SearchOption.AllDirectories));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private NativeComponentDescriptor CreateSdkComponent(string variable)
    {
        var component = CreateComponent("sdk-component");
        File.WriteAllText(Path.Combine(m_root, "shared.h"), "#include <owned.h>\nOWNED_SDK_TYPE sample(OWNED_SDK_TYPE value);");
        string config = Path.Combine(m_root, "sdk-component", "bindgen.json");
        File.WriteAllText(config, File.ReadAllText(config).Replace("\"entryFiles\":",
            "\"systemIncludeFolders\":[\"%" + variable + "%/include\"],\"entryFiles\":", StringComparison.Ordinal));
        return component;
    }

    private string CreateSdk(
        string name,
        string nativeType
    ) {
        string sdk = Path.Combine(m_root, name);
        Directory.CreateDirectory(Path.Combine(sdk, "include"));
        File.WriteAllText(Path.Combine(sdk, "include", "owned.h"), "#define OWNED_SDK_TYPE " + nativeType);
        return sdk;
    }

    private NativeBuildContext SdkContext(
        string variable,
        string sdk
    ) {
        string executable = Path.Combine(m_root, "sdk-tool.exe");
        File.WriteAllText(executable, "Selected SDK tool identity; not executed by this C parser fixture.");
        var selection = new NativeToolchainSelection("fixture", new BuildHostDescriptor("Windows", "x64"),
            new Dictionary<string, string> { ["fixture"] = executable },
            new Dictionary<string, string> { [variable] = sdk }, [sdk], [], ".dll", false);
        return new NativeBuildContext(m_root, "release").WithToolchain(selection);
    }

    private NativeComponentDescriptor CreateComponent(string name)
    {
        string owner = Path.Combine(m_root, name);
        Directory.CreateDirectory(owner);
        File.WriteAllText(Path.Combine(owner, name + ".csproj"), "<Project />");
        File.WriteAllText(Path.Combine(owner, "bindings.props"), $"""
            <Project><PropertyGroup><InnoNativeComponentId>{name}</InnoNativeComponentId><InnoBindingHostConfig>bindgen.json</InnoBindingHostConfig></PropertyGroup>
            <ItemGroup><InnoBindingTarget Include="fixture"><Config>bindgen.json</Config></InnoBindingTarget></ItemGroup></Project>
            """);
        File.WriteAllText(Path.Combine(owner, "bindgen.json"), """
            {"preset":"c-library","namespace":"Fixture.Native","apiName":"Api","libName":"fixture",
             "targetId":"linux-x64-gnu","entryFiles":["../shared.h"],"allowedHeaders":["../shared.h"],
             "generateRuntimeSource":false,"mergeGeneratedFilesToSingleFile":true,"singleFileOutputName":"Bindings.cs"}
            """);
        return new NativeComponentDescriptor(name, name + "/" + name + ".csproj", name + "/" + name + ".csproj",
            bindingDefinition: name + "/bindings.props");
    }
}
