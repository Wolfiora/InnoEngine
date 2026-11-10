using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Cli.Tests;

public sealed class ScriptCommandTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoScriptCommand", Guid.NewGuid().ToString("N"));

    [Fact]
    public async Task ScriptExportAndIdeProjectionRequireNoNativeSourceOrToolsTarget()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "Project", "Assets"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "Microsoft Visual Studio Solution File, Format Version 12.00");
        File.WriteAllText(Path.Combine(m_root, "Project", "Assets", "Probe.cs"),
            "using InnoEngine.Scene; public sealed class Probe : GameBehavior { }");
        string unrelatedCatalog = Path.Combine(m_root, "artifacts", "cache-owners");
        Directory.CreateDirectory(unrelatedCatalog);
        File.WriteAllText(Path.Combine(unrelatedCatalog, "unrelated.json"), "unavailable product-cache metadata");
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        TextWriter previousError = Console.Error;
        using var errors = new StringWriter(CultureInfo.InvariantCulture);
        try
        {
            Console.SetError(errors);
            int result = await BuildApplication.Run(["scripts", "--engine-root", m_root,
                "--project", Path.Combine(m_root, "Project"), "--output", Path.Combine(m_root, "Scripts"),
                "--dotnet", host, "--ide"], CancellationToken.None);
            Assert.True(result == 0, errors.ToString());
            Assert.DoesNotContain("automatic cache maintenance", errors.ToString(), StringComparison.Ordinal);
            Assert.True(File.Exists(Path.Combine(m_root, "Scripts", "Inno.GameScripts.dll")));
            Assert.True(File.Exists(Path.Combine(m_root, "Project", "Inno.GameScripts.csproj")));
            Assert.NotEmpty(Directory.GetFiles(Path.Combine(m_root, "Project", "Library"),
                "Inno.ScriptApi.Runtime.dll", SearchOption.AllDirectories));
            Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts", "native")));
            Assert.False(Directory.Exists(Path.Combine(m_root, "extern")));
        }
        finally
        {
            Console.SetError(previousError);
        }
    }

    [Fact]
    public async Task PrecancelledScriptExportCreatesNoWorkspace()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Equal(2, await BuildApplication.Run(["scripts", "--engine-root", m_root,
            "--project", Path.Combine(m_root, "Project"), "--output", Path.Combine(m_root, "Scripts")], cancellation.Token));
        Assert.False(Directory.Exists(m_root));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
