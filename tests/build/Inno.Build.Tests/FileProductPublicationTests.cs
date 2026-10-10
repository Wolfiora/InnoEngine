using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class FileProductPublicationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoFileProductPublicationTests", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task SingleFilePublicationRetainsTheNativeClosureAndRejectsMissingInputs()
    {
        string engine = ToolchainEnvironment.FindRepoRoot();
        string projectRoot = Path.Combine(m_root, new string('p', 70), new string('p', 70), "PlayerLink");
        Directory.CreateDirectory(projectRoot);
        string project = Path.Combine(projectRoot, "Player.csproj");
        File.Copy(Path.Combine(engine, "platforms", "Windows", "build", "Inno.Build.Windows", "Templates", "WindowsPlayer.project.xml"), project);
        File.Copy(Path.Combine(engine, "global.json"), Path.Combine(m_root, "global.json"));
        File.WriteAllText(Path.Combine(projectRoot, "Program.cs"), "using System; Console.WriteLine(\"Published\");");
        File.WriteAllText(Path.Combine(projectRoot, "PlayerDeploymentDefinition.g.cs"), "// The fixture has no game registrations.");
        string code = Path.Combine(m_root, "Code");
        Directory.CreateDirectory(code);
        string sourceAssembly = typeof(ManagedDeploymentId).Assembly.Location;
        File.Copy(sourceAssembly, Path.Combine(code, Path.GetFileName(sourceAssembly)));
        string nativeRelative = Path.Combine("native", "fixture", "target", "fixture.dll");
        string nativeInput = Path.Combine(projectRoot, nativeRelative);
        Directory.CreateDirectory(Path.GetDirectoryName(nativeInput)!);
        File.Copy(sourceAssembly, nativeInput);

        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        CoreClrDeploymentCompiler compiler = new(host);
        using CancellationTokenSource timeout = new(TimeSpan.FromMinutes(3));
        DotNetSdkDescriptor selectedSdk = await DotNetSdkResolver.ResolveAsync(host, project, timeout.Token);
        XDocument definition = XDocument.Load(project);
        definition.Root!.Add(new XElement("Target",
            new XAttribute("Name", "VerifySelectedSdk"),
            new XAttribute("BeforeTargets", "PrepareForBuild"),
            new XElement("Error",
                new XAttribute("Condition", $"'$(NETCoreSdkVersion)' != '{selectedSdk.sdkIdentity}'"),
                new XAttribute("Text", "Publication changed the project-selected SDK."))));
        definition.Save(project);
        string output = Path.Combine(m_root, "Published");
        ManagedDeploymentResult result = await compiler.CompileAsync(new ManagedDeploymentRequest(
            project, RuntimeInformation.RuntimeIdentifier, code, output, Path.Combine(m_root, "Logs")), timeout.Token);
        string deployed = Path.Combine(output, nativeRelative);
        Assert.Contains(nativeRelative, result.files);
        Assert.Equal(File.ReadAllBytes(nativeInput), File.ReadAllBytes(deployed));
        string[] executionPaths = File.ReadAllLines(Path.Combine(m_root, "Logs", "managed-working-directory.txt"));
        Assert.Equal(projectRoot, executionPaths[0]);
        if (OperatingSystem.IsWindows())
        {
            Assert.True(executionPaths[1].Length < projectRoot.Length);
            Assert.False(Directory.Exists(executionPaths[1]));
        }

        File.Delete(nativeInput);
        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await compiler.CompileAsync(new ManagedDeploymentRequest(project, RuntimeInformation.RuntimeIdentifier,
                code, Path.Combine(m_root, "Rejected"), Path.Combine(m_root, "RejectedLogs")), timeout.Token));
        Assert.Contains("explicitly deployed native runtime closure", failure.Message);
        Assert.True(File.Exists(deployed));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
