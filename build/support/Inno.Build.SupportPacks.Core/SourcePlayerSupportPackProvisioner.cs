using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Builds an absent Player Support Pack from the current engine checkout using the installed .NET SDK.
/// </summary>
public sealed class SourcePlayerSupportPackProvisioner : IPlayerSupportPackProvisioner
{
    private readonly string m_engineRoot;

    /// <summary>
    /// Creates a provisioner for a complete engine checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The directory containing InnoEngine.sln and the Player project.
    /// </param>
    /// <exception cref="DirectoryNotFoundException">
    /// The directory is not an engine checkout.
    /// </exception>
    public SourcePlayerSupportPackProvisioner(string engineRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        m_engineRoot = Path.GetFullPath(engineRoot);
        if (!IsEngineRoot(m_engineRoot))
            throw new DirectoryNotFoundException($"Engine source checkout '{m_engineRoot}' is unavailable.");
    }

    /// <summary>
    /// Finds the checkout that owns the running Editor or build command.
    /// </summary>
    /// <param name="startDirectory">
    /// The host binary directory from which to search upward.
    /// </param>
    /// <returns>
    /// A source provisioner, or null when the host is a source-independent distribution.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    /// An explicit <c>INNO_ENGINE_ROOT</c> does not identify a complete engine checkout.
    /// </exception>
    public static SourcePlayerSupportPackProvisioner? TryCreateForHost(string startDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(startDirectory);
        string? configured = Environment.GetEnvironmentVariable("INNO_ENGINE_ROOT");
        if (!string.IsNullOrWhiteSpace(configured))
            return new SourcePlayerSupportPackProvisioner(configured);
        for (DirectoryInfo? directory = new(Path.GetFullPath(startDirectory));
             directory is not null;
             directory = directory.Parent)
        {
            if (IsEngineRoot(directory.FullName))
                return new SourcePlayerSupportPackProvisioner(directory.FullName);
        }
        return null;
    }

    /// <summary>
    /// Builds and installs the missing target Support Pack from the owning engine checkout.
    /// </summary>
    /// <param name="target">
    /// The target platform and architecture identity.
    /// </param>
    /// <param name="supportPackRoot">
    /// The directory that owns the installed target directories.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the publish process before the replacement commits.
    /// </param>
    /// <returns>
    /// An operation that completes after the target Pack has been installed and verified.
    /// </returns>
    public async ValueTask ProvisionAsync(
        BuildTargetId target,
        string supportPackRoot,
        CancellationToken cancellationToken = default
    )
        => _ = await PlayerSupportPackPublisher.PublishAsync(
            m_engineRoot,
            supportPackRoot,
            target,
            ResolveDotnetHost(),
            cancellationToken).ConfigureAwait(false);

    private static bool IsEngineRoot(string path)
        => File.Exists(Path.Combine(path, "InnoEngine.sln")) &&
           File.Exists(Path.Combine(path, "src", "composition", "player", "Inno.Player", "Inno.Player.csproj"));

    private static string ResolveDotnetHost()
    {
        string? configured = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!string.IsNullOrWhiteSpace(configured))
        {
            if (File.Exists(configured))
                return Path.GetFullPath(configured);
            throw new FileNotFoundException("The configured .NET SDK host does not exist.", configured);
        }

        string executable = OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet";
        var candidates = new List<string>();
        string? dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (!string.IsNullOrWhiteSpace(dotnetRoot))
            candidates.Add(Path.Combine(dotnetRoot, executable));
        candidates.Add(Path.Combine(
            Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..")),
            executable));
        candidates.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet", executable));
        if (OperatingSystem.IsMacOS())
        {
            candidates.Add("/opt/homebrew/bin/dotnet");
            candidates.Add("/usr/local/share/dotnet/dotnet");
        }
        foreach (string candidate in candidates)
            if (File.Exists(candidate))
                return candidate;
        throw new FileNotFoundException(
            "Automatic Player Support Pack generation requires an installed .NET SDK. " +
            "Set DOTNET_HOST_PATH to its dotnet executable.");
    }
}
