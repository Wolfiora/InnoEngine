using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Prepares a desktop runtime closure using an explicitly selected native host and managed target.
/// </summary>
public sealed class DesktopPlayerSupportPackSource : IPlayerSupportPackSource
{
    private readonly BuildTargetId m_target;
    private readonly IPlayerSupportPackValidator m_validator;
    private readonly string m_runtimeIdentifier;
    private readonly string m_nativePlatform;
    private readonly string m_nativeExtension;

    /// <summary>
    /// Captures the platform layout and validator without preparing any tools or files.
    /// </summary>
    /// <param name="target">
    /// The publication platform identity.
    /// </param>
    /// <param name="runtimeIdentifier">
    /// The managed SDK runtime target.
    /// </param>
    /// <param name="nativePlatform">
    /// The native toolchain target identity required by this source.
    /// </param>
    /// <param name="nativeExtension">
    /// The runtime library suffix selected by this platform.
    /// </param>
    /// <param name="validator">
    /// The borrowed platform validator used before publication.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A platform identifier or required target string is blank.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The validator is null.
    /// </exception>
    public DesktopPlayerSupportPackSource(
        BuildTargetId target,
        string runtimeIdentifier,
        string nativePlatform,
        string nativeExtension,
        IPlayerSupportPackValidator validator
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(target.value);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativePlatform);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativeExtension);
        ArgumentNullException.ThrowIfNull(validator);
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
        NativeBuildContext native = await HostNativeToolchain.ResolveAsync(
            new NativeBuildContext(context.engineRoot, ToolchainLayout.C_RELEASE_CONFIGURATION), cancellationToken)
            .ConfigureAwait(false);
        if (native.hostToolchain!.targetId != m_nativePlatform)
            throw new PlatformNotSupportedException($"Target '{m_target}' requires a native '{m_nativePlatform}' host toolchain.");
        IReadOnlyList<NativeBuildProduct> products = await HostNativeBuild.BuildRuntimeAsync(native,
            cancellationToken).ConfigureAwait(false);
        string project = Path.Combine(context.engineRoot, "src", "composition", "player", "Inno.Player", "Inno.Player.csproj");
        await ToolchainEnvironment.RunAsync(context.dotnetHost,
            ["build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--configuration", "Release",
                "--runtime", m_runtimeIdentifier, "--nologo", "-p:DebugType=None", "-p:DebugSymbols=false"],
            context.engineRoot, cancellationToken).ConfigureAwait(false);
        PlayerSupportPackFiles.CopyReferences(
            Path.Combine(Path.GetDirectoryName(project)!, "bin", "Release", "net9.0", m_runtimeIdentifier),
            Path.Combine(context.stagingDirectory, "References"));
        CopyNativeRuntime(products, context.stagingDirectory);
        string link = Path.Combine(context.stagingDirectory, "PlayerLink");
        Directory.CreateDirectory(link);
        File.Copy(Path.Combine(context.engineRoot, "build", "support", "Inno.Build.SupportPacks",
            "Templates", "Desktop", "DesktopPlayer.project.xml"), Path.Combine(link, "Player.csproj"));
        await PlayerSupportPackFiles.CopyPlayerSourcesAsync(
            context, project, link, cancellationToken).ConfigureAwait(false);
        PlayerSupportPackFiles.CopyCompositionInputs(context.engineRoot, link);
        string references = Path.Combine(link, "References");
        Directory.CreateDirectory(references);
        foreach (string reference in Directory.EnumerateFiles(Path.Combine(context.stagingDirectory, "References"), "*.dll"))
            File.Copy(reference, Path.Combine(references, Path.GetFileName(reference)));
        string runtime = Path.Combine(Path.GetDirectoryName(project)!, "bin", "Release", "net9.0", m_runtimeIdentifier, "BGCS.Runtime.dll");
        File.Copy(runtime, Path.Combine(references, "BGCS.Runtime.dll"));
        await CopyNativeToTemplateAsync(context.stagingDirectory, link, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Validates the prepared platform inputs before atomic installation.
    /// </summary>
    /// <param name="directory">
    /// The isolated target pack directory.
    /// </param>
    public void Validate(string directory) => m_validator.Validate(directory);

    private void CopyNativeRuntime(
        IReadOnlyList<NativeBuildProduct> products,
        string staging
    ) {
        string destination = Path.Combine(staging, "native");
        foreach (NativeBuildProduct product in products)
        {
            if (product.targetId != m_nativePlatform)
                throw new InvalidDataException($"Native product '{product.component}' targets '{product.targetId}', not '{m_nativePlatform}'.");
            string[] files = product.files
                .Where(file => string.Equals(Path.GetExtension(file), m_nativeExtension, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (files.Length == 0)
            {
                throw new InvalidDataException(
                    $"Native runtime product '{product.component}' and '{m_target}' is empty.");
            }
            string componentDestination = Path.Combine(destination, product.component, m_target.value);
            Directory.CreateDirectory(componentDestination);
            foreach (string file in files.Order(StringComparer.Ordinal))
                File.Copy(file, Path.Combine(componentDestination, Path.GetFileName(file)));
        }
    }

    private static async ValueTask CopyNativeToTemplateAsync(
        string staging,
        string playerDirectory,
        CancellationToken cancellationToken
    ) {
        string nativeRoot = Path.Combine(staging, "native");
        foreach (string input in Directory.EnumerateFiles(nativeRoot, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string destination = Path.Combine(playerDirectory, "native", Path.GetRelativePath(nativeRoot, input));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await using FileStream source = File.OpenRead(input);
            await using FileStream output = new(destination, FileMode.CreateNew);
            await source.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(destination, File.GetUnixFileMode(input));
        }
    }
}
