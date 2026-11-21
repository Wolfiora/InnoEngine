using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeToolchainPreparationTests
{
    [Fact]
    public Task DirectoryAliasesCaptureOnlyTheSelectedCMakeInstallation() => VerifyInstallationInputsAsync(false);

    [UnixFileLinksFact]
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("linux")]
    public Task ExecutableAliasesCaptureOnlyTheSelectedCMakeInstallation() => VerifyInstallationInputsAsync(true);

    private static async Task VerifyInstallationInputsAsync(bool executableAlias)
    {
        string root = Path.Combine(
            new PhysicalPathResolver().Resolve(Path.GetTempPath()),
            "InnoCMakeInputs",
            Guid.NewGuid().ToString("N")
        );
        string prefix = Path.Combine(root, "prefix");
        string installation = Path.Combine(root, "installation");
        string physicalBin = Path.Combine(installation, "bin");
        string modules = Path.Combine(installation, "share");
        string aliasBin = Path.Combine(prefix, "bin");
        string unrelated = Path.Combine(prefix, "share", "other-tool.txt");
        string executable = Path.Combine(physicalBin, "cmake");
        string selected = Path.Combine(aliasBin, "cmake");
        string module = Path.Combine(modules, "module.cmake");
        Directory.CreateDirectory(physicalBin);
        Directory.CreateDirectory(modules);
        Directory.CreateDirectory(Path.GetDirectoryName(unrelated)!);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            File.WriteAllText(executable, "tool-one");
            File.WriteAllText(module, "module-one");
            File.WriteAllText(unrelated, "unrelated-one");
            if (executableAlias)
            {
                Directory.CreateDirectory(aliasBin);
                File.CreateSymbolicLink(selected, executable);
            }
            else if (OperatingSystem.IsWindows())
            {
                string escapedAlias = aliasBin.Replace("'", "''", StringComparison.Ordinal);
                string escapedTarget = physicalBin.Replace("'", "''", StringComparison.Ordinal);
                await ToolchainEnvironment.RunAsync(
                    "powershell.exe",
                    [
                        "-NoProfile",
                        "-NonInteractive",
                        "-Command",
                        $"$ErrorActionPreference='Stop'; New-Item -ItemType Junction -Path '{escapedAlias}' -Target '{escapedTarget}' | Out-Null"
                    ],
                    root,
                    CancellationToken.None
                );
            }
            else
            {
                Directory.CreateSymbolicLink(aliasBin, physicalBin);
            }

            NativeToolchainSelection selection = NativeToolchainPreparation.Freeze(
                "fixture-x64",
                new BuildHostDescriptor("Fixture", "x64"),
                new Dictionary<string, string> { ["cmake"] = selected },
                new Dictionary<string, string>(),
                [],
                [],
                new NativeLinkPlan(staticLibraryExtension: ".a"),
                false
            );
            Assert.Contains(modules, selection.inputPaths);
            Assert.DoesNotContain(Path.GetDirectoryName(unrelated)!, selection.inputPaths);
            string initial = Capture();
            File.WriteAllText(unrelated, "unrelated-two");
            Assert.Equal(initial, Capture());
            DateTime timestamp = File.GetLastWriteTimeUtc(module);
            File.WriteAllText(module, "module-two");
            File.SetLastWriteTimeUtc(module, timestamp);
            string changedModule = Capture();
            Assert.NotEqual(initial, changedModule);
            File.WriteAllText(executable, "tool-two");
            Assert.NotEqual(changedModule, Capture());

            string Capture()
                => NativeBuildFingerprint.Create(
                    new NativeBuildContext(root, "release"),
                    selection.declarations,
                    selection.inputPaths.Select(path => NativeBuildInput.FromPath(root, path)),
                    CancellationToken.None
                );
        }
        finally
        {
            if (executableAlias && File.Exists(selected))
            {
                File.Delete(selected);
            }
            else if (!executableAlias && Directory.Exists(aliasBin))
            {
                Directory.Delete(aliasBin, recursive: false);
            }
            Directory.Delete(root, recursive: true);
        }
    }
}
