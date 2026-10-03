using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;

namespace Inno.Build.SupportPacks;

internal sealed class DesktopPlayerSupportPackSource : IPlayerSupportPackSource
{
    private static readonly string[] S_METADATA_EXTENSIONS = [".dbg", ".map", ".pdb", ".xml"];
    private readonly BuildTargetId m_target;
    private readonly IPlayerSupportPackValidator m_validator;
    private readonly string m_runtimeIdentifier;
    private readonly string m_nativePlatform;
    private readonly string m_nativeExtension;

    internal DesktopPlayerSupportPackSource(
        BuildTargetId target,
        string runtimeIdentifier,
        string nativePlatform,
        string nativeExtension,
        IPlayerSupportPackValidator validator
    ) {
        m_target = target;
        m_validator = validator;
        m_runtimeIdentifier = runtimeIdentifier;
        m_nativePlatform = nativePlatform;
        m_nativeExtension = nativeExtension;
    }

    /// <summary>
    /// Gets the platform identity whose closure this source prepares.
    /// </summary>
    public BuildTargetId target => m_target;

    /// <summary>
    /// Prepares the target runtime and compilation inputs in isolated staging.
    /// </summary>
    /// <param name="context">
    /// The selected source checkout, SDK and staging directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the process tree before installation.
    /// </param>
    /// <returns>
    /// Completion after the platform closure has been staged, or a propagated build failure.
    /// </returns>
    public async ValueTask PrepareAsync(
        PlayerSupportPackBuildContext context,
        CancellationToken cancellationToken
    ) {
        await HostNativeBuild.BuildRuntimeAsync(
            new NativeBuildContext(context.engineRoot, ToolchainLayout.C_RELEASE_CONFIGURATION),
            cancellationToken).ConfigureAwait(false);
        string project = Path.Combine(context.engineRoot, "src", "composition", "player", "Inno.Player", "Inno.Player.csproj");
        string published = Path.Combine(context.stagingDirectory, ".runtime");
        await ToolchainEnvironment.RunAsync(context.dotnetHost,
            ["publish", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--configuration", "Release",
                "--runtime", m_runtimeIdentifier, "--self-contained", "true", "--output", published, "--nologo",
                "-p:PublishSingleFile=true", "-p:IncludeNativeLibrariesForSelfExtract=true", "-p:DebugType=None",
                "-p:DebugSymbols=false", "-p:CopyOutputSymbolsToPublishDirectory=false",
                "-p:CopyDebugSymbolFilesFromPackages=false", "-p:AllowedReferenceRelatedFileExtensions="],
            context.engineRoot, cancellationToken).ConfigureAwait(false);
        ComposeRuntimeClosure(published, context.stagingDirectory, cancellationToken);
        Directory.Delete(published, recursive: true);
        PlayerSupportPackFiles.CopyReferences(
            Path.Combine(Path.GetDirectoryName(project)!, "bin", "Release", "net9.0", m_runtimeIdentifier),
            Path.Combine(context.stagingDirectory, "References"));
        CopyNativeRuntime(context, context.stagingDirectory);
    }

    /// <summary>
    /// Validates the prepared platform inputs before atomic installation.
    /// </summary>
    /// <param name="directory">
    /// The isolated target pack directory.
    /// </param>
    public void Validate(string directory) => m_validator.Validate(directory);

    private void CopyNativeRuntime(
        PlayerSupportPackBuildContext context,
        string staging
    ) {
        string nativeProducts = Path.Combine(context.engineRoot, ".lib");
        string[] components = ["bgfx", "sdl3", "miniaudio", "text", "ui"];
        string destination = Path.Combine(staging, "native");
        foreach (string component in components)
        {
            string source = Path.Combine(nativeProducts, component, m_nativePlatform);
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException(
                    $"Release native runtime output for '{component}' and '{m_target}' does not exist at '{source}'.");
            }
            string[] files = Directory.EnumerateFiles(source, "*release*", SearchOption.TopDirectoryOnly)
                .Where(file => string.Equals(Path.GetExtension(file), m_nativeExtension, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (files.Length == 0)
            {
                throw new InvalidDataException(
                    $"Release native runtime output for '{component}' and '{m_target}' is empty.");
            }
            string componentDestination = Path.Combine(destination, component, m_target.value);
            Directory.CreateDirectory(componentDestination);
            foreach (string file in files.Order(StringComparer.Ordinal))
                File.Copy(file, Path.Combine(componentDestination, Path.GetFileName(file)));
        }
    }

    private static void ComposeRuntimeClosure(
        string publishedRuntime,
        string staging,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(publishedRuntime))
            throw new DirectoryNotFoundException("The Player publish stage produced no runtime directory.");

        Directory.CreateDirectory(staging);
        foreach (string source in Directory.EnumerateFiles(publishedRuntime, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (S_METADATA_EXTENSIONS.Contains(Path.GetExtension(source)))
                continue;
            string relativePath = Path.GetRelativePath(publishedRuntime, source);
            string destination = Path.Combine(staging, relativePath);
            string? destinationDirectory = Path.GetDirectoryName(destination);
            if (!string.IsNullOrEmpty(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);
            File.Copy(source, destination);
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(destination, File.GetUnixFileMode(source));
        }
    }
}
