using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes the selected SDK's evaluated managed source graph without compiling a product.
/// </summary>
public sealed class ManagedBuildInputSnapshot
{
    private readonly NativeBuildContext m_context;
    private readonly DotNetSdkDescriptor m_sdk;
    private readonly string m_project;
    private readonly IReadOnlyDictionary<string, string> m_properties;

    private ManagedBuildInputSnapshot(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        string fingerprint
    ) {
        m_context = context;
        m_sdk = sdk;
        m_project = project;
        m_properties = properties;
        this.fingerprint = fingerprint;
    }

    /// <summary>
    /// Gets the content identity of the evaluated graph, compiler inputs and selected SDK.
    /// </summary>
    public string fingerprint { get; }

    /// <summary>
    /// Evaluates an explicit managed graph using its frozen SDK and hashes its declared compilation inputs.
    /// </summary>
    /// <param name="context">
    /// The borrowed operation that owns phase-local content reads and tool statistics.
    /// </param>
    /// <param name="sdk">
    /// The SDK already selected for this project; evaluation must use its exact CLI entry.
    /// </param>
    /// <param name="project">
    /// The existing entry project whose references define the managed source closure.
    /// </param>
    /// <param name="properties">
    /// The immutable compiler and product properties also passed to the actual managed build.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels SDK evaluation, drains its process and removes this operation's request directory.
    /// </param>
    /// <returns>
    /// A snapshot that can be freshly verified after waiting or executing; no Player compilation is performed.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// A required operation, SDK or property collection is null.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// A required source, compiler, restored package input or SDK declaration is absent.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected SDK cannot evaluate the declared graph.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Evaluation or hashing was canceled.
    /// </exception>
    public static async ValueTask<ManagedBuildInputSnapshot> CaptureAsync(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(sdk);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(project);
        cancellationToken.ThrowIfCancellationRequested();
        string selected = Path.GetFullPath(project, context.engineRoot);
        if (!File.Exists(selected))
            throw new FileNotFoundException("The managed input entry project is absent.", selected);
        var frozen = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(
            properties.ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.OrdinalIgnoreCase));
        string identity = await ManagedBuildInputReader.CaptureAsync(context, sdk, selected, frozen,
            cancellationToken).ConfigureAwait(false);
        return new(context, sdk, selected, frozen, identity);
    }

    /// <summary>
    /// Re-evaluates the graph and re-reads its current bytes in a fresh verification phase.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels evaluation and verification without changing any published output.
    /// </param>
    /// <returns>
    /// Completion when the complete current input identity still matches this snapshot.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The graph, SDK, source, resource, analyzer or reference inputs changed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Verification was canceled.
    /// </exception>
    public async ValueTask VerifyAsync(CancellationToken cancellationToken = default)
    {
        string current = await ManagedBuildInputReader.CaptureAsync(
            m_context.BeginInputVerification("managed-inputs"), m_sdk, m_project, m_properties,
            cancellationToken).ConfigureAwait(false);
        if (current != fingerprint)
            throw new InvalidOperationException("Managed compilation inputs changed after planning; the candidate cannot be accepted.");
    }
}
