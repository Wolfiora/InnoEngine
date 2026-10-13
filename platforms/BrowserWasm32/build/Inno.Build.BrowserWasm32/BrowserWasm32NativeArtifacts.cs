using System;
using System.IO;
using Inno.Build.Toolchains;

namespace Inno.Build.BrowserWasm32;

/// <summary>
/// Identifies the immutable native closure produced by one browser toolchain request.
/// </summary>
public sealed class BrowserWasm32NativeArtifacts : IDisposable
{
    private readonly NativeBuildProduct m_product;
    private readonly NativeBindingGenerationResult m_bindings;

    internal BrowserWasm32NativeArtifacts(
        NativeBuildProduct product,
        NativeBindingGenerationResult bindings
    ) {
        m_product = product;
        m_bindings = bindings;
        fingerprint = product.fingerprint;
        directory = Path.Combine(product.directory, "Outputs");
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

    /// <summary>
    /// Releases the complete archive closure after managed compilation and copying finish.
    /// Published paths must not be accessed afterwards; repeated disposal has no effect.
    /// </summary>
    public void Dispose()
    {
        try
        {
            m_product.Dispose();
        }
        finally
        {
            m_bindings.Dispose();
        }
    }
}
