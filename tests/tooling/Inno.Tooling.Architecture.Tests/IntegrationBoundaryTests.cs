using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class IntegrationBoundaryTests
{
    [Theory]
    [InlineData("platforms/Windows/build/Probe", "backends/Bgfx/build/ProbeBackend", "platform base modules cannot select", true)]
    [InlineData("platforms/Windows/integrations/Probe", "backends/Bgfx/build/ProbeBackend", "platform base modules cannot select", false)]
    [InlineData("backends/Bgfx/build/ProbeBackend", "platforms/Windows/integrations/Probe", "reusable backends cannot depend", true)]
    [InlineData("platforms/Browser/integrations/Probe", "platforms/Windows/integrations/ProbeWindows", "one concrete platform cannot depend", true)]
    public async Task CliChecksActualPlatformIntegrationProjectReferences(
        string owner,
        string dependency,
        string expected,
        bool rejected
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoIntegrationBoundaryTests", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
            string destination = CreateProject(dependency, string.Empty);
            CreateProject(owner, "<ItemGroup><ProjectReference Include=\"" + Path.GetRelativePath(Path.Combine(root, owner), destination).Replace('\\', '/') + "\" /></ItemGroup>");
            DirectoryInfo? checkout = new(AppContext.BaseDirectory);
            while (checkout is not null && !File.Exists(Path.Combine(checkout.FullName, "InnoEngine.sln")))
                checkout = checkout.Parent;
            Assert.NotNull(checkout);
            string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
            var start = new ProcessStartInfo(host) { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };
            start.ArgumentList.Add(Path.Combine(checkout!.FullName, "build/cli/Inno.Build.Cli/bin/Debug/net9.0/Inno.Build.Cli.dll"));
            start.ArgumentList.Add("verify");
            start.ArgumentList.Add(root);
            using var process = Process.Start(start)!;
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> error = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            string report = await output + await error;
            Assert.Equal(rejected, report.Contains(expected, StringComparison.Ordinal));
            Assert.DoesNotContain("explicit project dependency is missing", report);

            string CreateProject(
                string relative,
                string references
            ) {
                string directory = Path.Combine(root, relative);
                Directory.CreateDirectory(directory);
                string path = Path.Combine(directory, Path.GetFileName(directory) + ".csproj");
                File.WriteAllText(path, "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup>" + references + "</Project>");
                return path;
            }
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
