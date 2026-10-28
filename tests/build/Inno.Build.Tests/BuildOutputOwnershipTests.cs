using Inno.Testing.Source;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildOutputOwnershipTests
{
    [Fact]
    public async Task ConcurrentSdkRequestsKeepExplicitOutputOwnersForTheSameTarget()
    {
        string engine = TestRepository.GetRoot(typeof(BuildOutputOwnershipTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoOutputOwnership", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            new XDocument(new XElement("Project", new XElement("Import",
                new XAttribute("Project", Path.Combine(engine, "Directory.Build.props")))))
                .Save(Path.Combine(root, "Directory.Build.props"));
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), "<Project />");
            File.WriteAllText(Path.Combine(root, "Api.cs"), "public static class Api { public static int value => 42; }");
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"),
                    new XElement("ImplicitUsings", "disable"), new XElement("Nullable", "enable"),
                    new XElement("EnableDefaultCompileItems", "false")),
                new XElement("ItemGroup", new XElement("Compile", new XAttribute("Include", "Api.cs")))))
                .Save(project);
            string first = Path.Combine(root, "requests", "first");
            string second = Path.Combine(root, "requests", "second");
            string[][] outputs = await Task.WhenAll(Build(project, first), Build(project, second));
            for (int index = 0; index < outputs.Length; index++)
            {
                string owner = Path.GetFullPath(index == 0 ? first : second) + Path.DirectorySeparatorChar;
                Assert.Equal(3, outputs[index].Length);
                foreach (string path in outputs[index])
                {
                    Assert.StartsWith(owner, path, StringComparison.Ordinal);
                    Assert.True(File.Exists(path), $"The SDK-selected output does not exist: {path}");
                }
            }
            Assert.All(outputs[0], path => Assert.DoesNotContain(path, outputs[1]));
            Assert.False(Directory.Exists(Path.Combine(root, "bin")));
            Assert.False(Directory.Exists(Path.Combine(root, "obj")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static async Task<string[]> Build(
        string project,
        string outputOwner
    ) {
        string sdk = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(sdk)
        {
            WorkingDirectory = Path.GetDirectoryName(project)!, UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        string evaluated = Path.Combine(Path.GetDirectoryName(outputOwner)!, Path.GetFileName(outputOwner) + ".json");
        Directory.CreateDirectory(Path.GetDirectoryName(evaluated)!);
        foreach (string argument in new[] { "build", project, "--artifacts-path", outputOwner,
            "-p:InnoNativeTarget=fixture-target", "-p:InnoNativeBuildFingerprint=fixture-fingerprint",
            "-target:Build", "-getProperty:TargetPath,ProjectAssetsFile", "-getItem:IntermediateAssembly", "-getResultOutputFile:" + evaluated,
            "--disable-build-servers", "-m:1", "-nodeReuse:false", "-v:minimal" })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("The SDK did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(2));
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
        }
        Assert.True(process.ExitCode == 0, await output + await error);
        using JsonDocument result = JsonDocument.Parse(File.ReadAllText(evaluated));
        JsonElement properties = result.RootElement.GetProperty("Properties");
        JsonElement intermediate = Assert.Single(result.RootElement.GetProperty("Items").GetProperty("IntermediateAssembly").EnumerateArray());
        return
        [
            Path.GetFullPath(properties.GetProperty("TargetPath").GetString()!),
            Path.GetFullPath(properties.GetProperty("ProjectAssetsFile").GetString()!),
            Path.GetFullPath(intermediate.GetProperty("FullPath").GetString()!)
        ];
    }
}
