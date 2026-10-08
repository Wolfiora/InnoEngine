using System;
using Inno.Build.Managed;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.MacOS;

/// <summary>
/// Packages a verified macOS ARM64 Support Pack as a native application bundle.
/// </summary>
public sealed class MacOSArm64GameBuildTarget : IGameBuildTarget
{
    private static readonly IPlayerSupportPackValidator S_SUPPORT_PACK_VALIDATOR = new MacOSSupportPackValidator();


    /// <summary>
    /// Validates the target closure before runtime script compilation.
    /// </summary>
    /// <param name="directory">
    /// The Support Pack directory selected by the build catalog.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// The required platform inputs are incomplete or incompatible.
    /// </exception>
    public void Validate(string directory) => S_SUPPORT_PACK_VALIDATOR.Validate(directory);

    /// <summary>
    /// Gets the macOS ARM64 target identity.
    /// </summary>
    public BuildTargetId id => BuildTargetId.macOSArm64;

    /// <inheritdoc />
    public ManagedDeploymentId defaultManagedDeployment => ManagedDeploymentId.coreClr;

    /// <inheritdoc />
    public string runtimeIdentifier => "osx-arm64";

    /// <summary>
    /// Gets the target name presented by authoring hosts.
    /// </summary>
    public string displayName => "macOS (Apple silicon)";



    /// <summary>
    /// Composes a macOS application bundle in isolated staging.
    /// </summary>
    /// <param name="context">
    /// The verified Support Pack, packaged content, and output staging paths.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels composition before commit.
    /// </param>
    /// <returns>
    /// The staged application bundle path.
    /// </returns>
    public async ValueTask<string> PackageAsync(
        GameBuildPackageContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        string application = Path.Combine(context.outputDirectory, context.profile.productName + ".app");
        string contents = Path.Combine(application, "Contents");
        string executableRoot = Path.Combine(contents, "MacOS");
        string resources = Path.Combine(contents, "Resources");
        await CopyDirectoryAsync(context.managedDeployment.outputDirectory, executableRoot, cancellationToken,
                excludeCompilerReferences: true)
            .ConfigureAwait(false);
        string player = Path.Combine(executableRoot, "Inno.Player.MacOS");
        if (!File.Exists(player))
            throw new InvalidDataException("The macOS managed publication does not contain Inno.Player.MacOS.");
        File.Move(player, Path.Combine(executableRoot, context.profile.productName));
        await CopyDirectoryAsync(context.contentDirectory, Path.Combine(resources, "Content"), cancellationToken,
                excludeCompilerReferences: false)
            .ConfigureAwait(false);
        Directory.CreateDirectory(contents);
        string product = SecurityElement.Escape(context.profile.productName) ?? context.profile.productName;
        string identifier = SecurityElement.Escape(context.profile.applicationId) ?? context.profile.applicationId;
        await File.WriteAllTextAsync(
                Path.Combine(contents, "Info.plist"),
                $"""
                <?xml version="1.0" encoding="UTF-8"?>
                <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
                <plist version="1.0"><dict>
                  <key>CFBundleExecutable</key><string>{product}</string>
                  <key>CFBundleIdentifier</key><string>{identifier}</string>
                  <key>CFBundleName</key><string>{product}</string>
                  <key>CFBundlePackageType</key><string>APPL</string>
                  <key>NSHighResolutionCapable</key><true/>
                </dict></plist>
                """,
                cancellationToken)
            .ConfigureAwait(false);
        return application;
    }

    private static async ValueTask CopyDirectoryAsync(
        string source,
        string destination,
        CancellationToken cancellationToken,
        bool excludeCompilerReferences
    ) {
        Directory.CreateDirectory(destination);
        foreach (string directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relativePath = Path.GetRelativePath(source, directory);
            if (excludeCompilerReferences && IsCompilerReferencePath(relativePath))
                continue;
            Directory.CreateDirectory(Path.Combine(destination, relativePath));
        }
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relativePath = Path.GetRelativePath(source, file);
            if (excludeCompilerReferences && IsCompilerReferencePath(relativePath))
                continue;
            string target = Path.Combine(destination, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await using FileStream input = new(file, FileMode.Open, FileAccess.Read, FileShare.Read, 131072, true);
            await using FileStream output = new(target, FileMode.CreateNew, FileAccess.Write, FileShare.None, 131072, true);
            await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(target, File.GetUnixFileMode(file));
        }
    }

    private static bool IsCompilerReferencePath(string relativePath)
        => relativePath.Equals("References", StringComparison.OrdinalIgnoreCase)
           || relativePath.StartsWith("References" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
}
