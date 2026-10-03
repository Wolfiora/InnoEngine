using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Text;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class TextToolchain
{
    /// <summary>
    /// Builds and installs the component for the current native host.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// Completion after the component has been built and installed in the selected checkout.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string configuration = context.configuration;
        string repositoryRoot = context.engineRoot;
        string nativeDirectory = Path.Combine(repositoryRoot, "native", "Inno.Native.Text", "Native");
        string platform = ResolvePlatform();
        string buildRoot = Path.Combine(context.GetNativeBuildRoot(typeof(TextToolchain).Assembly), platform);
        string outputRoot = Path.Combine(repositoryRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, "text", platform);

        string buildType = configuration == ToolchainLayout.C_DEBUG_CONFIGURATION ? "Debug" : "Release";
        string buildDirectory = Path.Combine(buildRoot, configuration);
        string architectureOptions = OperatingSystem.IsMacOS()
            ? "-DCMAKE_OSX_ARCHITECTURES=arm64"
            : string.Empty;
        string generatorOptions = OperatingSystem.IsWindows()
            ? "-G \"Visual Studio 17 2022\" -A x64"
            : $"-DCMAKE_BUILD_TYPE={buildType}";
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"-S \"{nativeDirectory}\" -B \"{buildDirectory}\" {generatorOptions} {architectureOptions}",
            repositoryRoot, cancellationToken);
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"--build \"{buildDirectory}\" --config {buildType} --target inno-text",
            repositoryRoot, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        string source = FindLibrary(buildDirectory, buildType);
        Directory.CreateDirectory(outputRoot);
        string outputName = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(source), configuration);
        string destination = Path.Combine(outputRoot, outputName);
        File.Copy(source, destination, overwrite: true);
        Console.WriteLine($"Text native build complete. Output: {destination}");
    }

    private static string ResolvePlatform()
    {
        Architecture architecture = RuntimeInformation.ProcessArchitecture;
        if (OperatingSystem.IsMacOS() && architecture == Architecture.Arm64)
            return "osx-arm64";
        if (OperatingSystem.IsWindows() && architecture == Architecture.X64)
            return "windows-x64";
        if (OperatingSystem.IsLinux() && architecture == Architecture.X64)
            return "linux-x64";
        if (OperatingSystem.IsLinux() && architecture == Architecture.Arm64)
            return "linux-arm64";
        throw new PlatformNotSupportedException("Text supports macOS ARM64, Windows x64, and Linux x64/ARM64 hosts.");
    }

    private static string FindLibrary(
        string buildDirectory,
        string buildType
    ) {
        string expected = OperatingSystem.IsWindows() ? "inno-text.dll"
            : OperatingSystem.IsMacOS() ? "libinno-text.dylib"
            : "libinno-text.so";
        string[] candidates = Directory.GetFiles(buildDirectory, expected, SearchOption.AllDirectories)
            .Where(path => !path.Contains("CMakeFiles", StringComparison.Ordinal))
            .Where(path => !OperatingSystem.IsWindows()
                || path.Contains(Path.DirectorySeparatorChar + buildType + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        return candidates.Length == 1
            ? candidates[0]
            : throw new FileNotFoundException($"Expected exactly one '{expected}' output but found {candidates.Length}.");
    }
}
