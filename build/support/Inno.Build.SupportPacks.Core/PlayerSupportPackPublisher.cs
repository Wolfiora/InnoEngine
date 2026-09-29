using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Creates the deployment-only Player closure in isolated staging and installs it atomically.
/// </summary>
public static class PlayerSupportPackPublisher
{
    /// <summary>
    /// Publishes a verified, self-contained Player Support Pack from an engine checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The engine checkout containing the Player composition project.
    /// </param>
    /// <param name="outputRoot">
    /// The directory that owns target-specific Support Packs.
    /// </param>
    /// <param name="target">
    /// The platform and architecture to publish.
    /// </param>
    /// <param name="dotnetHost">
    /// The .NET SDK host executable.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation before the atomic installation.
    /// </param>
    /// <returns>
    /// The verified installed Support Pack directory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A path is empty or the target has no supported publisher.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The engine checkout or a required release native runtime directory is missing.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A required native library or the staged deployment closure is invalid.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Publication was canceled before the target replacement committed.
    /// </exception>
    public static ValueTask<string> PublishAsync(
        string engineRoot,
        string outputRoot,
        BuildTargetId target,
        string dotnetHost,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        if (target != BuildTargetId.macOSArm64 && target != BuildTargetId.windowsX64)
            throw new ArgumentException($"Support Pack target '{target}' is not implemented.", nameof(target));
        return PublishCoreAsync(new PublishRequest(
            Path.GetFullPath(engineRoot), Path.GetFullPath(outputRoot), target, dotnetHost), cancellationToken);
    }

    private sealed record PublishRequest(
        string engineRoot,
        string outputRoot,
        BuildTargetId target,
        string dotnetHost
    );

    private static readonly HashSet<string> S_PUBLISH_METADATA_EXTENSIONS = new(StringComparer.OrdinalIgnoreCase)
    {
        ".dbg", ".map", ".pdb", ".xml"
    };

    private static async ValueTask<string> PublishCoreAsync(
        PublishRequest command,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(command);
        string solution = Path.Combine(command.engineRoot, "InnoEngine.sln");
        if (!File.Exists(solution))
            throw new DirectoryNotFoundException($"Engine root '{command.engineRoot}' has no InnoEngine.sln.");
        string playerProject = Path.Combine(
            command.engineRoot,
            "src",
            "composition",
            "player",
            "Inno.Player",
            "Inno.Player.csproj");
        if (!File.Exists(playerProject))
            throw new FileNotFoundException("The Player composition project does not exist.", playerProject);

        Directory.CreateDirectory(command.outputRoot);
        string stagingRoot = Path.Combine(
            command.outputRoot,
            ".support-pack-" + command.target.value + "-" + Guid.NewGuid().ToString("N"));
        string staging = Path.Combine(stagingRoot, command.target.value);
        string publishedRuntime = stagingRoot + ".published-runtime";
        string destination = Path.Combine(command.outputRoot, command.target.value);
        string backup = destination + ".replaced-" + Guid.NewGuid().ToString("N");
        try
        {
            await RunPublishAsync(command, playerProject, publishedRuntime, cancellationToken).ConfigureAwait(false);
            ComposeRuntimeClosure(publishedRuntime, staging, cancellationToken);
            CopyCompilerReferences(playerProject, command.target, staging);
            CopyNativeRuntime(command, staging);
            cancellationToken.ThrowIfCancellationRequested();
            _ = new PlayerSupportPackCatalog(stagingRoot).Resolve(command.target);
            bool replaced = Directory.Exists(destination);
            if (replaced)
                Directory.Move(destination, backup);
            try
            {
                Directory.Move(staging, destination);
            }
            catch
            {
                if (Directory.Exists(destination))
                    Directory.Delete(destination, recursive: true);
                if (Directory.Exists(backup))
                    Directory.Move(backup, destination);
                throw;
            }
            if (Directory.Exists(backup))
                Directory.Delete(backup, recursive: true);
            return destination;
        }
        finally
        {
            if (Directory.Exists(stagingRoot))
                Directory.Delete(stagingRoot, recursive: true);
            if (Directory.Exists(publishedRuntime))
                Directory.Delete(publishedRuntime, recursive: true);
        }
    }

    private static async ValueTask RunPublishAsync(
        PublishRequest command,
        string playerProject,
        string staging,
        CancellationToken cancellationToken
    ) {
        string runtimeIdentifier = command.target == BuildTargetId.macOSArm64
            ? "osx-arm64"
            : "win-x64";
        var startInfo = new ProcessStartInfo
        {
            FileName = command.dotnetHost,
            WorkingDirectory = command.engineRoot,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add("publish");
        startInfo.ArgumentList.Add(playerProject);
        startInfo.ArgumentList.Add("--disable-build-servers");
        startInfo.ArgumentList.Add("-m:1");
        startInfo.ArgumentList.Add("-nodeReuse:false");
        startInfo.ArgumentList.Add("--configuration");
        startInfo.ArgumentList.Add("Release");
        startInfo.ArgumentList.Add("--runtime");
        startInfo.ArgumentList.Add(runtimeIdentifier);
        startInfo.ArgumentList.Add("--self-contained");
        startInfo.ArgumentList.Add("true");
        startInfo.ArgumentList.Add("--output");
        startInfo.ArgumentList.Add(staging);
        startInfo.ArgumentList.Add("--nologo");
        startInfo.ArgumentList.Add("-p:DebugType=None");
        startInfo.ArgumentList.Add("-p:PublishSingleFile=true");
        startInfo.ArgumentList.Add("-p:IncludeNativeLibrariesForSelfExtract=true");
        startInfo.ArgumentList.Add("-p:DebugSymbols=false");
        startInfo.ArgumentList.Add("-p:CopyOutputSymbolsToPublishDirectory=false");
        startInfo.ArgumentList.Add("-p:CopyDebugSymbolFilesFromPackages=false");
        startInfo.ArgumentList.Add("-p:AllowedReferenceRelatedFileExtensions=");

        using var process = new Process { StartInfo = startInfo };
        if (!process.Start())
            throw new InvalidOperationException("The .NET publish process could not be started.");
        Task<string> output = process.StandardOutput.ReadToEndAsync(cancellationToken);
        Task<string> error = process.StandardError.ReadToEndAsync(cancellationToken);
        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            throw;
        }
        string standardOutput = await output.ConfigureAwait(false);
        string standardError = await error.ConfigureAwait(false);
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Player Support Pack publish failed with exit code {process.ExitCode}." +
                Environment.NewLine + standardOutput + Environment.NewLine + standardError);
        }
    }

    private static void CopyNativeRuntime(
        PublishRequest command,
        string staging
    ) {
        string nativeProducts = Path.Combine(command.engineRoot, ".lib");
        string nativePlatform = command.target == BuildTargetId.macOSArm64
            ? "osx-arm64"
            : "windows-x64";
        string nativeExtension = command.target == BuildTargetId.macOSArm64
            ? ".dylib"
            : ".dll";
        string[] components = ["bgfx", "sdl3", "miniaudio", "text", "ui"];
        string destination = Path.Combine(staging, "native");
        foreach (string component in components)
        {
            string source = Path.Combine(nativeProducts, component, nativePlatform);
            if (!Directory.Exists(source))
            {
                throw new DirectoryNotFoundException(
                    $"Release native runtime output for '{component}' and '{command.target}' does not exist at '{source}'.");
            }
            string[] files = Directory.EnumerateFiles(source, "*release*", SearchOption.TopDirectoryOnly)
                .Where(file => string.Equals(Path.GetExtension(file), nativeExtension, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (files.Length == 0)
            {
                throw new InvalidDataException(
                    $"Release native runtime output for '{component}' and '{command.target}' is empty.");
            }
            string componentDestination = Path.Combine(destination, component, command.target.value);
            Directory.CreateDirectory(componentDestination);
            foreach (string file in files.Order(StringComparer.Ordinal))
                File.Copy(file, Path.Combine(componentDestination, Path.GetFileName(file)));
        }
    }

    private static void CopyCompilerReferences(
        string playerProject,
        BuildTargetId target,
        string staging
    ) {
        string runtimeIdentifier = target == BuildTargetId.macOSArm64 ? "osx-arm64" : "win-x64";
        string buildOutput = Path.Combine(
            Path.GetDirectoryName(playerProject)!, "bin", "Release", "net9.0", runtimeIdentifier);
        string[] references = Directory.Exists(buildOutput)
            ? Directory.EnumerateFiles(buildOutput, "Inno.*.dll", SearchOption.TopDirectoryOnly).ToArray()
            : [];
        if (references.Length == 0)
            throw new InvalidDataException("Player publish produced no target runtime compilation references.");
        string referenceDirectory = Path.Combine(staging, "References");
        Directory.CreateDirectory(referenceDirectory);
        foreach (string reference in references.Order(StringComparer.Ordinal))
            File.Copy(reference, Path.Combine(referenceDirectory, Path.GetFileName(reference)));
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
            if (S_PUBLISH_METADATA_EXTENSIONS.Contains(Path.GetExtension(source)))
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
