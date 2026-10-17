using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Prepares current checkout and SDK inputs and selects a verified, immutable Player Support Pack.
/// </summary>
public sealed class SourcePlayerSupportPackProvisioner : IPlayerSupportPackProvisioner
{
    private readonly string m_engineRoot;
    private readonly string m_dotnetHost;
    private readonly PlayerSupportPackPublisher m_publisher;
    private readonly Action<string> m_reportDiagnostic;

    /// <summary>
    /// Creates a provisioner for a complete engine checkout.
    /// </summary>
    /// <param name="engineRoot">
    /// The directory containing InnoEngine.sln and the Player project.
    /// </param>
    /// <exception cref="DirectoryNotFoundException">
    /// The directory is not an engine checkout.
    /// </exception>
    /// <param name="publisher">
    /// The publisher containing the host-selected platform sources.
    /// </param>
    /// <param name="dotnetHost">
    /// The SDK executable explicitly selected by the host.
    /// </param>
    /// <param name="reportDiagnostic">
    /// The borrowed host reporter for successful-publication indexing and cleanup diagnostics.
    /// </param>
    public SourcePlayerSupportPackProvisioner(
        string engineRoot,
        PlayerSupportPackPublisher publisher,
        string dotnetHost,
        Action<string> reportDiagnostic
    ) {
        ArgumentNullException.ThrowIfNull(publisher);
        ArgumentNullException.ThrowIfNull(reportDiagnostic);
        m_reportDiagnostic = reportDiagnostic;
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        m_publisher = publisher;
        m_dotnetHost = dotnetHost;
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
    /// <param name="publisher">
    /// The publisher containing the host-selected platform sources.
    /// </param>
    /// <param name="dotnetHost">
    /// The SDK executable explicitly selected by the host.
    /// </param>
    /// <param name="reportDiagnostic">
    /// The borrowed host reporter, retained only by the returned provisioner.
    /// </param>
    public static SourcePlayerSupportPackProvisioner? TryCreateForHost(
        string startDirectory,
        PlayerSupportPackPublisher publisher,
        string dotnetHost,
        Action<string> reportDiagnostic
    ) {
        ArgumentNullException.ThrowIfNull(publisher);
        ArgumentNullException.ThrowIfNull(reportDiagnostic);
        ArgumentException.ThrowIfNullOrWhiteSpace(startDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        string? configured = Environment.GetEnvironmentVariable("INNO_ENGINE_ROOT");
        if (!string.IsNullOrWhiteSpace(configured))
            return new SourcePlayerSupportPackProvisioner(configured, publisher, dotnetHost, reportDiagnostic);
        for (DirectoryInfo? directory = new(Path.GetFullPath(startDirectory));
             directory is not null;
             directory = directory.Parent)
        {
            if (IsEngineRoot(directory.FullName))
                return new SourcePlayerSupportPackProvisioner(directory.FullName, publisher, dotnetHost, reportDiagnostic);
        }
        return null;
    }

    /// <summary>
    /// Prepares current target inputs and atomically selects their immutable Support Pack.
    /// </summary>
    /// <param name="target">
    /// The target platform and architecture identity.
    /// </param>
    /// <param name="supportPackRoot">
    /// The directory that owns the installed target directories.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation or publication before a new current fingerprint is selected.
    /// </param>
    /// <returns>
    /// An operation that completes after the target Pack has been installed and verified.
    /// </returns>
    public async ValueTask ProvisionAsync(
        BuildTargetId target,
        string supportPackRoot,
        CancellationToken cancellationToken = default
    ) {
        using PlayerSupportPackPublication installed = await m_publisher.PublishAsync(
            m_engineRoot,
            supportPackRoot,
            target,
            m_dotnetHost,
            cancellationToken).ConfigureAwait(false);
        foreach (string diagnostic in installed.diagnostics)
            m_reportDiagnostic(diagnostic);
    }

    private static bool IsEngineRoot(string path)
        => File.Exists(Path.Combine(path, "InnoEngine.sln")) &&
           File.Exists(Path.Combine(path, "build", "distributions", "Inno.Build.Distribution.Standard", "Inno.Build.Distribution.Standard.csproj"));

}
