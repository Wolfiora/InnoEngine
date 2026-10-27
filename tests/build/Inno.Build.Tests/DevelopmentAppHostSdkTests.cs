using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class DevelopmentAppHostSdkTests
{
    [Fact]
    public async Task DirectAppHostUsesItsLoadedRuntimeBeforeAnUnrelatedPath()
    {
        string executable = OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet";
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..", executable));
        Assert.True(File.Exists(host));
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(Path.Combine(AppContext.BaseDirectory, "SdkHostFixtures"));
        string project = Path.Combine(owner.directory, "SdkHostProbe.csproj");
        Type[] references = [typeof(StandardBuildEnvironment), typeof(BuildCompositionContext), typeof(BuildTargetId)];
        var items = new XElement("ItemGroup");
        foreach (Type reference in references)
            items.Add(new XElement("Reference", new XAttribute("Include", reference.Assembly.GetName().Name!),
                new XElement("HintPath", reference.Assembly.Location)));
        new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
            new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"), new XElement("OutputType", "Exe"),
                new XElement("ImplicitUsings", "disable"), new XElement("UseAppHost", "true")), items)).Save(project);
        await File.WriteAllTextAsync(Path.Combine(owner.directory, "Program.cs"), """
            using System;
            using Inno.Build;
            using Inno.Build.Distribution.Standard;
            Console.Write(StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId("sdk-probe")).dotnetHost);
            """);
        using var buildOutput = new StringWriter();
        using var buildErrors = new StringWriter();
        try
        {
            await ToolchainEnvironment.RunAsync(host, ["build", project, "-nologo", "-nodeReuse:false"],
                owner.directory, CancellationToken.None, DotNetSdkEnvironment.Create(host), buildOutput, buildErrors);
        }
        catch (InvalidOperationException failure)
        {
            throw new InvalidOperationException(buildOutput + Environment.NewLine + buildErrors, failure);
        }
        string output = Path.Combine(owner.directory, "bin", "Debug", "net10.0");
        foreach (string dependency in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll"))
            File.Copy(dependency, Path.Combine(output, Path.GetFileName(dependency)), overwrite: true);
        var environment = new Dictionary<string, string?>
        {
            ["DOTNET_HOST_PATH"] = null,
            ["DOTNET_ROOT"] = Path.GetDirectoryName(host),
            ["PATH"] = owner.directory
        };
        string appHost = Path.Combine(output, OperatingSystem.IsWindows() ? "SdkHostProbe.exe" : "SdkHostProbe");
        string selected = await ToolchainEnvironment.CaptureOutputAsync(appHost, [], owner.directory,
            CancellationToken.None, environment);
        Assert.Equal(host, selected.Trim());
    }
}
