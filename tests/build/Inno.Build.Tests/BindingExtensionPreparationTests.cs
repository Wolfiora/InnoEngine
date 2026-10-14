using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BindingExtensionPreparationTests
{
    private const string C_FIXTURE_ROOT = "INNO_TEST_BINDING_EXTENSION_ROOT";

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public async Task ExtensionRestoreCompletesBeforeItsInputInventoryIsFrozen()
    {
        string engine = ToolchainEnvironment.FindRepoRoot();
        string? root = Environment.GetEnvironmentVariable(C_FIXTURE_ROOT);
        if (root is null)
        {
            using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(
                Path.Combine(engine, "artifacts", "acceptance", "binding-extension-fixtures"));
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            var environment = new Dictionary<string, string?>(DotNetSdkEnvironment.Create(host))
            {
                [C_FIXTURE_ROOT] = owner.directory
            };
            await ToolchainEnvironment.RunAsync(host,
                ["test", typeof(BindingExtensionPreparationTests).Assembly.Location, "--filter",
                    "FullyQualifiedName=Inno.Build.Tests.BindingExtensionPreparationTests.ExtensionRestoreCompletesBeforeItsInputInventoryIsFrozen"],
                engine, timeout.Token, environment);
            return;
        }
        ProcessOwnedDirectory.RegisterCurrentProcess(Path.GetDirectoryName(root)!, root);
        File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(root, "shared.h"), "int sample(int value);");
        string componentRoot = Path.Combine(root, "extension");
        Directory.CreateDirectory(componentRoot);
        File.WriteAllText(Path.Combine(componentRoot, "extension.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(componentRoot, "bindings.props"), """
            <Project><PropertyGroup><InnoNativeComponentId>extension</InnoNativeComponentId><InnoBindingHostConfig>bindgen.json</InnoBindingHostConfig></PropertyGroup>
            <ItemGroup><InnoBindingTarget Include="fixture"><Config>bindgen.json</Config></InnoBindingTarget></ItemGroup></Project>
            """);
        File.WriteAllText(Path.Combine(componentRoot, "bindgen.json"), """
            {"preset":"c-library","namespace":"Fixture.Native","apiName":"Api","libName":"fixture",
             "targetId":"linux-x64-gnu","entryFiles":["../shared.h"],"allowedHeaders":["../shared.h"],
             "generateRuntimeSource":false,"mergeGeneratedFilesToSingleFile":true,"singleFileOutputName":"Bindings.cs"}
            """);
        var component = new NativeComponentDescriptor("extension", "extension/extension.csproj", "extension/extension.csproj",
            bindingDefinition: "extension/bindings.props");
        string directory = Path.Combine(root, "extension");
        string extension = Path.Combine(directory, "Extension");
        Directory.CreateDirectory(extension);
        string project = Path.Combine(extension, "Fixture.Extension.csproj");
        string packages = Path.Combine(ToolchainEnvironment.FindRepoRoot(), "build", "msbuild", "Inno.Packages.props");
        string version = XDocument.Load(packages).Root!.Element("PropertyGroup")!.Element("InnoBgcsPackageVersion")!.Value;
        File.WriteAllText(project, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup>"
            + "<TargetFramework>net9.0</TargetFramework><RestorePackagesWithLockFile>true</RestorePackagesWithLockFile>"
            + "</PropertyGroup><ItemGroup><PackageReference Include=\"BGCS.Core\" Version=\"[" + version
            + "]\" /></ItemGroup></Project>");
        File.WriteAllText(Path.Combine(extension, "FixturePlugin.cs"), """
            using BGCS.Core.Extensibility;
            public sealed class FixturePlugin : IBindingPlugin
            {
                public string id => "fixture.extension";
                public string version => "1.0.0";
                public int contractVersion => BindingPluginContract.C_CURRENT_VERSION;
                public void Configure(IBindingPluginHost host) {}
            }
            """);
        string definition = Path.Combine(directory, "bindings.props");
        File.WriteAllText(definition, File.ReadAllText(definition).Replace("</PropertyGroup>",
            "<InnoBindingExtensionProject>Extension/Fixture.Extension.csproj</InnoBindingExtensionProject></PropertyGroup>", StringComparison.Ordinal));
        string packageLock = Path.Combine(extension, "packages.lock.json");
        Assert.False(File.Exists(packageLock));
        var generator = new NativeBindingGenerator("dotnet");
        var request = new NativeBindingGenerationRequest([component], "fixture", NativeBindingOutputMode.TargetArtifacts);
        using NativeBindingGenerationResult result = await generator.GenerateAsync(
            new NativeBuildContext(root, "release"), request, CancellationToken.None);
        Assert.True(File.Exists(packageLock));
        Assert.True(File.Exists(result[component.nativeProject].bindingsPath));
        using NativeBindingGenerationResult hot = await generator.GenerateAsync(
            new NativeBuildContext(root, "release"), request, CancellationToken.None);
        Assert.Equal(result[component.nativeProject].fingerprint, hot[component.nativeProject].fingerprint);
    }

}
