using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

/// <summary>
/// Verifies binding identities through independent SDK test hosts rather than a shared dictionary hash seed.
/// </summary>
[Trait("ExternalDependency", "BGCS")]
public sealed class BindingGenerationProcessTests
{
    private const string C_ROOT = "INNO_TEST_BINDING_IDENTITY_ROOT";
    private const string C_OUTPUT = "INNO_TEST_BINDING_IDENTITY_OUTPUT";

    /// <summary>
    /// Keeps unchanged mapping identities stable across processes and rejects a real mapping change.
    /// </summary>
    /// <returns>A task completing after all independent hosts have exited and their identities agree.</returns>
    [Fact]
    public async Task DictionaryMappingsRemainStableAcrossIndependentProcesses()
    {
        string? inherited = Environment.GetEnvironmentVariable(C_ROOT);
        if (inherited is not null)
        {
            var component = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
            var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);
            string identity = await BuildTestProducts.Generator().ComputeInputFingerprintAsync(
                new NativeBuildContext(inherited, "release"), request, CancellationToken.None);
            File.WriteAllText(Environment.GetEnvironmentVariable(C_OUTPUT)!, identity);
            return;
        }

        string root = Path.Combine(Path.GetTempPath(), "InnoBindingIdentity", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            File.WriteAllText(Path.Combine(root, "Fixture.csproj"), "<Project />");
            File.WriteAllText(Path.Combine(root, "bindings.props"), "<Project><PropertyGroup><InnoNativeComponentId>fixture</InnoNativeComponentId><InnoBindingConfig>bindgen.json</InnoBindingConfig></PropertyGroup></Project>");
            Directory.CreateDirectory(Path.Combine(root, "Native"));
            File.WriteAllLines(Path.Combine(root, "Native", "fixture.h"), Enumerable.Range(0, 64).Select(static index => $"int sample_{index}(int value);"));
            var names = Enumerable.Range(0, 64).ToDictionary(static index => $"sample_{index}", static index => $"Sample{index}", StringComparer.Ordinal);
            WriteConfiguration(root, names);
            string first = await RunHost(root, "first");
            string second = await RunHost(root, "second");
            Assert.Equal(first, second);
            names["sample_0"] = "Changed";
            WriteConfiguration(root, names);
            Assert.NotEqual(first, await RunHost(root, "changed"));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static void WriteConfiguration(
        string root,
        Dictionary<string, string> names
    ) => File.WriteAllText(Path.Combine(root, "bindgen.json"), JsonSerializer.Serialize(new
    {
        schemaVersion = 1,
        library = new { id = "fixture", nativeName = "fixture" },
        source = new { language = "c", headers = new[] { "Native/fixture.h" }, includeDirectories = new[] { "Native" } },
        api = new { @namespace = "Fixture.Native", name = "Api", names },
        output = new { layout = "singleFile", singleFileName = "Bindings.cs", root = "Generated" }
    }));

    private static async Task<string> RunHost(
        string root,
        string name
    ) {
        string dotnet = ToolchainEnvironment.ResolveExecutable(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet");
        string output = Path.Combine(root, name + ".identity");
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        await ToolchainEnvironment.RunAsync(dotnet,
            ["test", typeof(BindingGenerationProcessTests).Assembly.Location,
                "--filter", "FullyQualifiedName=" + typeof(BindingGenerationProcessTests).FullName + "." + nameof(DictionaryMappingsRemainStableAcrossIndependentProcesses),
                "--results-directory", Path.Combine(root, "results"),
                "--logger", "trx;LogFileName=" + name + ".trx",
                "--environment", C_ROOT + "=" + root, "--environment", C_OUTPUT + "=" + output],
            root, timeout.Token, DotNetSdkEnvironment.Create(dotnet));
        return File.ReadAllText(output);
    }
}
