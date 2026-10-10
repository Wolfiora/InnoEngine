using System.IO;

namespace Inno.Build.Browser;

/// <summary>
/// Identifies the immutable native closure produced by one browser toolchain request.
/// </summary>
public sealed class BrowserNativeArtifacts
{
    internal BrowserNativeArtifacts(
        string fingerprint,
        string directory
    ) {
        this.fingerprint = fingerprint;
        this.directory = directory;
    }

    /// <summary>
    /// Gets the identity of the SDK, source, configuration and selected binding generations.
    /// </summary>
    public string fingerprint { get; }

    /// <summary>
    /// Gets the absolute immutable install root containing component archive directories.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Gets the immutable MSBuild selection file used to reject mismatched managed binding inputs.
    /// </summary>
    public string bindingSelectionPath => Path.Combine(directory, "Metadata", "BindingSelection.props");
}
