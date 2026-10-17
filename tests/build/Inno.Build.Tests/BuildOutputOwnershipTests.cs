using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace Inno.Build.Tests;

public sealed class BuildOutputOwnershipTests
{
    [Fact]
    public async Task ConcurrentSdkRequestsKeepExplicitOutputOwnersForTheSameTarget()
    {
        DirectoryInfo? engine = new(AppContext.BaseDirectory);
        while (engine is not null && !File.Exists(Path.Combine(engine.FullName, "InnoEngine.sln")))
            engine = engine.Parent;
        Assert.NotNull(engine);
        string root = Path.Combine(Path.GetTempPath(), "InnoOutputOwnership", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            new XDocument(new XElement("Project", new XElement("Import",
                new XAttribute("Project", Path.Combine(engine.FullName, "Directory.Build.props")))))
                .Save(Path.Combine(root, "Directory.Build.props"));
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), "<Project />");
            File.WriteAllText(Path.Combine(root, "Api.cs"), "public static class Api { public static int value => 42; }");
            string project = Path.Combine(root, "Fixture.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net9.0"),
                    new XElement("ImplicitUsings", "disable"), new XElement("Nullable", "enable"),
                    new XElement("EnableDefaultCompileItems", "false")),
                new XElement("ItemGroup", new XElement("Compile", new XAttribute("Include", "Api.cs")))))
                .Save(project);
            string first = Path.Combine(root, "requests", "first");
            string second = Path.Combine(root, "requests", "second");
            await Task.WhenAll(Build(project, first), Build(project, second));
            foreach (string owner in new[] { first, second })
            {
                Assert.True(File.Exists(Path.Combine(owner, "bin", "Fixture", "debug", "Fixture.dll")));
                Assert.True(File.Exists(Path.Combine(owner, "obj", "Fixture", "project.assets.json")));
                Assert.True(File.Exists(Path.Combine(owner, "obj", "Fixture", "debug", "Fixture.dll")));
            }
            Assert.False(Directory.Exists(Path.Combine(root, "bin")));
            Assert.False(Directory.Exists(Path.Combine(root, "obj")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static async Task Build(
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
        foreach (string argument in new[] { "build", project, "--artifacts-path", outputOwner,
            "-p:InnoNativeTarget=fixture-target", "-p:InnoNativeBuildFingerprint=fixture-fingerprint",
            "--disable-build-servers", "-m:1", "-nodeReuse:false", "-v:minimal" })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("The SDK did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        Assert.True(process.ExitCode == 0, await output + await error);
    }
}
