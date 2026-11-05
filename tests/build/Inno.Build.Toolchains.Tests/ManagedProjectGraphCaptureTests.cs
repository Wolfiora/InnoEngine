using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class ManagedProjectGraphCaptureTests
{
    [Fact]
    public async Task RootOutputsBelongToTheEvaluatedInstanceEvenWithNonrepeatableProperties()
    {
        string parent = Path.Combine(Path.GetTempPath(), "InnoGraphTests");
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("An explicit test SDK is required."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00\n");
            string project = Path.Combine(root, "Example.csproj");
            File.WriteAllText(project, """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                    <AssemblyName>Example.$([System.Guid]::NewGuid())</AssemblyName>
                  </PropertyGroup>
                </Project>
                """);
            using JsonDocument graph = JsonDocument.Parse(await ManagedProjectGraphCapture.CaptureAsync(root, dotnet, [project],
                new Dictionary<string, string> { ["Configuration"] = "Debug", ["DesignTimeBuild"] = "true" }));
            JsonElement owner = Assert.Single(graph.RootElement.EnumerateArray());
            JsonElement instance = Assert.Single(owner.GetProperty("Instances").EnumerateArray());
            Assert.Equal(instance.GetProperty("TargetPath").GetString(), owner.GetProperty("Properties").GetProperty("TargetPath").GetString());
            Assert.Empty(owner.GetProperty("Variants").EnumerateArray());
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(parent) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The test output escaped its declared owner.");
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("net10.0", false)]
    [InlineData("net10.0-browser", false)]
    [InlineData("net10.0-browser", true)]
    public async Task ReferencePacksFollowSdkFrameworkDeclarationsAndExplicitVersionOverrides(
        string framework,
        bool explicitVersion
    ) {
        string parent = Path.Combine(Path.GetTempPath(), "InnoGraphTests");
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("An explicit test SDK is required."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00\n");
            string project = Path.Combine(root, "Example.csproj");
            File.WriteAllText(project, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>" + framework + "</TargetFramework></PropertyGroup>"
                + (explicitVersion ? "<ItemGroup><FrameworkReference Update=\"Microsoft.NETCore.App\" TargetingPackVersion=\"99.0.1\" /></ItemGroup>" : string.Empty)
                + "</Project>");
            string captured = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet, [project],
                new Dictionary<string, string> { ["Configuration"] = "Debug", ["DesignTimeBuild"] = "true", ["BuildProjectReferences"] = "false" });
            using JsonDocument graph = JsonDocument.Parse(captured);
            JsonElement owner = Assert.Single(graph.RootElement.EnumerateArray());
            string pack = Assert.Single(owner.GetProperty("FrameworkPacks").EnumerateArray()).GetString()!;
            Assert.Equal("Microsoft.NETCore.App.Ref", Path.GetFileName(Path.GetDirectoryName(pack)));
            if (explicitVersion)
                Assert.Equal("99.0.1", Path.GetFileName(pack));
            else
                Assert.True(Directory.Exists(pack));
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(parent) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The test output escaped its declared owner.");
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task EquivalentVariantInputsSharePayloadWhileConditionalInputsAndPropertyOriginsRemainVisible()
    {
        string parent = Path.Combine(Path.GetTempPath(), "InnoGraphTests");
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("An explicit test SDK is required."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        try
        {
            foreach (string name in new[] { "A", "B", "C" })
                Directory.CreateDirectory(Path.Combine(root, name));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00\n");
            File.WriteAllText(Path.Combine(root, "Extra.cs"), "namespace Example; public class Extra { }");
            string first = Path.Combine(root, "A", "A.csproj");
            File.WriteAllText(first, """
                <Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                <ItemGroup Condition="'$(Flavor)' == 'two'"><Compile Include="../Extra.cs" /></ItemGroup></Project>
                """);
            foreach ((string name, string flavor) in new[] { ("B", "one"), ("C", "two") })
                File.WriteAllText(Path.Combine(root, name, name + ".csproj"),
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>"
                    + "<ItemGroup><ProjectReference Include=\"../A/A.csproj\" AdditionalProperties=\"Flavor=" + flavor + "\" /></ItemGroup></Project>");
            string captured = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet,
                [first, Path.Combine(root, "B", "B.csproj"), Path.Combine(root, "C", "C.csproj")],
                new Dictionary<string, string> { ["Configuration"] = "Debug", ["DesignTimeBuild"] = "true", ["BuildProjectReferences"] = "false" });
            using JsonDocument graph = JsonDocument.Parse(captured);
            JsonElement owner = graph.RootElement.EnumerateArray().Single(node => node.GetProperty("Path").GetString() == first);
            Assert.Contains(owner.GetProperty("EquivalentGlobalProperties").EnumerateArray(),
                properties => properties.TryGetProperty("Flavor", out JsonElement flavor) && flavor.GetString() == "one");
            JsonElement variant = Assert.Single(owner.GetProperty("Variants").EnumerateArray());
            Assert.Contains(variant.GetProperty("EquivalentGlobalProperties").EnumerateArray(),
                properties => properties.TryGetProperty("Flavor", out JsonElement flavor) && flavor.GetString() == "two");
            Assert.Contains(variant.GetProperty("Items").GetProperty("Compile").EnumerateArray(),
                item => item.GetProperty("FullPath").GetString() == Path.Combine(root, "Extra.cs"));
            Assert.DoesNotContain(owner.GetProperty("Items").GetProperty("Compile").EnumerateArray(),
                item => item.GetProperty("FullPath").GetString() == Path.Combine(root, "Extra.cs"));
            foreach ((string name, string flavor) in new[] { ("B", "one"), ("C", "two") })
            {
                JsonElement consumer = graph.RootElement.EnumerateArray().Single(node => node.GetProperty("Path").GetString() == Path.Combine(root, name, name + ".csproj"));
                JsonElement instance = Assert.Single(consumer.GetProperty("Instances").EnumerateArray());
                string reference = Assert.Single(instance.GetProperty("References").EnumerateArray()).GetString()!;
                JsonElement selected = owner.GetProperty("Instances").EnumerateArray().Single(node => node.GetProperty("Identity").GetString() == reference);
                Assert.Equal(flavor, selected.GetProperty("GlobalProperties").GetProperty("Flavor").GetString());
            }
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(parent) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The test output escaped its declared owner.");
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BatchResolutionPreservesActualReferencesCompilerSymbolsAndImportedSources(bool buildReferences)
    {
        string parent = Path.Combine(Path.GetTempPath(), "InnoGraphTests");
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("An explicit test SDK is required."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        Directory.CreateDirectory(Path.Combine(root, "A"));
        Directory.CreateDirectory(Path.Combine(root, "B"));
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00\n");
            string first = Path.Combine(root, "A", "A.csproj");
            string second = Path.Combine(root, "B", "B.csproj");
            File.WriteAllText(first, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
            File.WriteAllText(Path.Combine(root, "A", "Contract.cs"), "namespace Contracts; public class Contract { }");
            File.WriteAllText(Path.Combine(root, "Imported.cs"), "namespace Consumers; public class Consumer : Contracts.Contract { }");
            File.WriteAllText(Path.Combine(root, "Imported.props"), "<Project><PropertyGroup><DefineConstants>$(DefineConstants);IMPORTED</DefineConstants></PropertyGroup><ItemGroup><Compile Include=\"../Imported.cs\" /></ItemGroup></Project>");
            File.WriteAllText(second, "<Project Sdk=\"Microsoft.NET.Sdk\" TreatAsLocalProperty=\"BuildProjectReferences\"><PropertyGroup><TargetFramework>net10.0</TargetFramework><BuildProjectReferences>true</BuildProjectReferences></PropertyGroup><Import Project=\"../Imported.props\" /><ItemGroup><ProjectReference Include=\"../A/A.csproj\" /></ItemGroup></Project>");
            var start = new ProcessStartInfo(dotnet) { WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
            foreach (string argument in new[] { "build", second, "-m:1", "-nr:false", "--disable-build-servers" })
                start.ArgumentList.Add(argument);
            using Process process = Process.Start(start)!;
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> errors = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await output + await errors);
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), """
                <Project>
                  <Target Name="RejectCompilationDuringReferenceCapture" BeforeTargets="CoreCompile">
                    <Error Text="Reference capture must not compile an already prepared dependency." />
                  </Target>
                </Project>
                """);
            string captured = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet, [first, second],
                new Dictionary<string, string>
                {
                    ["Configuration"] = "Debug", ["DesignTimeBuild"] = "true", ["BuildProjectReferences"] = buildReferences.ToString(), ["InnoProductRestore"] = "false"
                }, resolveReferences: true);
            using JsonDocument graph = JsonDocument.Parse(captured);
            Assert.Equal(2, graph.RootElement.GetArrayLength());
            JsonElement consumer = graph.RootElement.EnumerateArray().Single(node => node.GetProperty("Path").GetString() == second);
            string[] defines = consumer.GetProperty("Properties").GetProperty("DefineConstants").GetString()!.Split(';');
            Assert.Contains("IMPORTED", defines);
            Assert.Contains("NET10_0", defines);
            Assert.Contains(consumer.GetProperty("Items").GetProperty("ReferencePath").EnumerateArray(),
                item => Path.GetFileName(item.GetProperty("FullPath").GetString()) == "A.dll");
            Assert.Contains(consumer.GetProperty("Items").GetProperty("ReferencePath").EnumerateArray(),
                item => item.TryGetProperty("FrameworkReferenceName", out JsonElement value) && value.GetString() == "Microsoft.NETCore.App");
            Assert.Contains(consumer.GetProperty("Items").GetProperty("Compile").EnumerateArray(),
                item => item.GetProperty("FullPath").GetString() == Path.Combine(root, "Imported.cs"));
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(Path.GetFullPath(parent) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The test output escaped its declared owner.");
            Directory.Delete(root, recursive: true);
        }
    }
}
