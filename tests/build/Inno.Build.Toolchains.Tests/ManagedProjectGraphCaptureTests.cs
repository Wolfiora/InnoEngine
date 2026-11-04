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
    public async Task BatchResolutionPreservesActualReferencesCompilerSymbolsAndImportedSources()
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
            File.WriteAllText(second, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup><Import Project=\"../Imported.props\" /><ItemGroup><ProjectReference Include=\"../A/A.csproj\" /></ItemGroup></Project>");
            var start = new ProcessStartInfo(dotnet) { WorkingDirectory = root, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
            foreach (string argument in new[] { "build", second, "-m:1", "-nr:false", "--disable-build-servers" })
                start.ArgumentList.Add(argument);
            using Process process = Process.Start(start)!;
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> errors = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await output + await errors);
            string captured = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet, [first, second],
                new Dictionary<string, string>
                {
                    ["Configuration"] = "Debug", ["DesignTimeBuild"] = "true", ["BuildProjectReferences"] = "false", ["InnoProductRestore"] = "false"
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
