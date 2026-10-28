using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Testing.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ManagedOutputOwnershipTests
{
    [Theory]
    [InlineData("Debug", false)]
    [InlineData("Release", false)]
    [InlineData("Debug", true)]
    [InlineData("Release", true)]
    public async Task SdkSeparatesNeutralAndProductOutputsBeforeRestore(
        string configuration,
        bool artifacts
    ) {
        string root = TestRepository.GetRoot(typeof(ManagedOutputOwnershipTests).Assembly);
        string owner = Path.Combine(Path.GetTempPath(), "InnoManagedOutputTests");
        string fixture = PathBoundary.Resolve(owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        try
        {
            File.Copy(Path.Combine(root, "global.json"), Path.Combine(fixture, "global.json"));
            await File.WriteAllTextAsync(Path.Combine(fixture, "Directory.Build.props"), $"""
                <Project>
                  <Import Project="{SecurityElement.Escape(Path.Combine(root, "Directory.Build.props"))}" />
                </Project>
                """);
            string project = Path.Combine(fixture, "Shared.csproj");
            await File.WriteAllTextAsync(project, """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                </Project>
                """);
            await File.WriteAllTextAsync(Path.Combine(fixture, "Shared.cs"), "public sealed class Shared { }");
            string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
                ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            var common = new List<string> { $"-p:Configuration={configuration}" };
            if (artifacts)
                common.Add($"-p:ArtifactsPath={Path.Combine(fixture, "outputs")}");
            Dictionary<string, string> neutral = await EvaluateAsync(host, project, common);
            var productArguments = new List<string>(common) { "-p:InnoNativeTarget=test-target" };
            Dictionary<string, string> product = await EvaluateAsync(host, project, productArguments);
            foreach (string property in new[] { "ProjectAssetsFile", "IntermediateOutputPath", "TargetPath" })
                Assert.NotEqual(neutral[property], product[property]);

            var compileArguments = new List<string>(productArguments)
            {
                "-p:BuildProjectReferences=true", "-p:BuildingInsideVisualStudio=false"
            };
            Dictionary<string, string> compile = await EvaluateAsync(host, project, compileArguments);
            Assert.Equal(product["ProjectAssetsFile"], compile["ProjectAssetsFile"]);
            Assert.Equal(product["TargetPath"], compile["TargetPath"]);

            if (artifacts)
            {
                foreach (string input in new[] { "InnoToolTarget", "InnoNativeBuildFingerprint" })
                {
                    var changedArguments = new List<string>(productArguments) { $"-p:{input}=different-input" };
                    Dictionary<string, string> changed = await EvaluateAsync(host, project, changedArguments);
                    Assert.NotEqual(product["ProjectAssetsFile"], changed["ProjectAssetsFile"]);
                    Assert.NotEqual(product["TargetPath"], changed["TargetPath"]);
                }
                string productProperties = Path.Combine(fixture, "Product.props");
                await File.WriteAllTextAsync(productProperties, "<Project />");
                var changedProductArguments = new List<string>(productArguments)
                {
                    $"-p:InnoProductBuildProperties={productProperties}"
                };
                Dictionary<string, string> changedProduct = await EvaluateAsync(host, project, changedProductArguments);
                Assert.NotEqual(product["ProjectAssetsFile"], changedProduct["ProjectAssetsFile"]);
                Assert.NotEqual(product["TargetPath"], changedProduct["TargetPath"]);
                var equivalentProductArguments = new List<string>(productArguments)
                {
                    $"-p:InnoProductBuildProperties={Path.Combine(fixture, ".", "Product.props")}"
                };
                Dictionary<string, string> equivalentProduct = await EvaluateAsync(host, project, equivalentProductArguments);
                Assert.Equal(changedProduct["ProjectAssetsFile"], equivalentProduct["ProjectAssetsFile"]);
                Assert.Equal(changedProduct["TargetPath"], equivalentProduct["TargetPath"]);
            }

            var toolArguments = new List<string>(productArguments) { "-p:IsRoslynComponent=true" };
            Dictionary<string, string> compilerTool = await EvaluateAsync(host, project, toolArguments);
            Assert.Equal(neutral["ProjectAssetsFile"], compilerTool["ProjectAssetsFile"]);
            Assert.Equal(neutral["TargetPath"], compilerTool["TargetPath"]);

            var ridArguments = new List<string>(productArguments) { "-p:RuntimeIdentifier=win-x64" };
            Dictionary<string, string> rid = await EvaluateAsync(host, project, ridArguments);
            Assert.Equal("win-x64", rid["RuntimeIdentifier"]);
            Assert.NotEqual(product["TargetPath"], rid["TargetPath"]);

            foreach (List<string> request in new[] { common, productArguments })
                await RunAsync(host, fixture, ["restore", project, .. request, "-v:quiet"]);
            string driver = Path.Combine(fixture, "Build.proj");
            await File.WriteAllTextAsync(driver, $"""
                <Project>
                  <ItemGroup>
                    <Request Include="Shared.csproj" />
                    <Request Include="Shared.csproj">
                      <AdditionalProperties>InnoNativeTarget=test-target;BuildProjectReferences=true;BuildingInsideVisualStudio=false</AdditionalProperties>
                    </Request>
                  </ItemGroup>
                  <Target Name="Build">
                    <MSBuild Projects="@(Request)" Targets="Build" BuildInParallel="true" />
                  </Target>
                </Project>
                """);
            await RunAsync(host, fixture, ["msbuild", driver, "-t:Build", "-m:2", "-nodeReuse:false", "-v:quiet", .. common]);
            Assert.True(File.Exists(neutral["TargetPath"]));
            Assert.True(File.Exists(product["TargetPath"]));
        }
        finally
        {
            Directory.Delete(PathBoundary.RequireUnlinkedPath(owner, fixture), recursive: true);
        }
    }

    private static async Task<Dictionary<string, string>> EvaluateAsync(
        string host,
        string project,
        List<string> properties
    ) {
        using var output = new StringWriter();
        await RunAsync(host, Path.GetDirectoryName(project)!, [
            "msbuild", project, "-nologo", .. properties,
            "-getProperty:ProjectAssetsFile,IntermediateOutputPath,TargetPath,RuntimeIdentifier"
        ], output);
        using JsonDocument document = JsonDocument.Parse(output.ToString());
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (JsonProperty property in document.RootElement.GetProperty("Properties").EnumerateObject())
            values.Add(property.Name, property.Value.GetString()!);
        return values;
    }

    private static async Task RunAsync(
        string host,
        string directory,
        IReadOnlyList<string> arguments,
        StringWriter? output = null
    ) {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        using var error = new StringWriter();
        using var captured = new StringWriter();
        Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(
            host, arguments, directory, cancellation.Token, DotNetSdkEnvironment.Create(host), output ?? captured, error));
        Assert.True(failure is null, $"{failure}{Environment.NewLine}{output ?? captured}{Environment.NewLine}{error}");
    }
}
