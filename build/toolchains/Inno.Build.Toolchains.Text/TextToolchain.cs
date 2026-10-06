using System.IO;
using System.Linq;
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
    /// The complete shared library product validated and published for this operation.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        string nativeDirectory = Path.Combine(context.engineRoot, "native", "Inno.Native.Text", "Native");
        string platform = context.hostToolchain!.targetId;
        return await NativeArtifactPublisher.PublishAsync(context, NativeBuildRecipe.CreateForComponent(context, typeof(TextToolchain).Assembly, "text", platform, [Path.Combine(nativeDirectory, "CMakeLists.txt"),
                Path.Combine(nativeDirectory, "include"),
                Path.Combine(nativeDirectory, "src"),
                Path.Combine(nativeDirectory, "Generated"),
                Path.Combine(context.engineRoot, "extern", "freetype"),
                Path.Combine(context.engineRoot, "extern", "harfbuzz")], []),
            async (
                scoped,
                output,
                token
            ) => {
                string buildType = scoped.configuration == "debug" ? "Debug" : "Release";
                string buildDirectory = scoped.GetNativeBuildRoot(typeof(TextToolchain).Assembly);
                string[] configure = OperatingSystem.IsWindows()
                    ? ["-S", nativeDirectory, "-B", buildDirectory, "-G", "Visual Studio 17 2022", "-A", "x64"]
                    : ["-S", nativeDirectory, "-B", buildDirectory, "-DCMAKE_BUILD_TYPE=" + buildType];
                await ToolchainEnvironment.RunAsync(scoped, "cmake", configure,
                    scoped.engineRoot, token).ConfigureAwait(false);
                await ToolchainEnvironment.RunAsync(scoped, "cmake",
                    ["--build", buildDirectory, "--config", buildType, "--target", "inno-text"],
                    scoped.engineRoot, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                string source = FindLibrary(buildDirectory, buildType);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(source), scoped.configuration);
                File.Copy(source, Path.Combine(output, name));
            }, cancellationToken).ConfigureAwait(false);
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
