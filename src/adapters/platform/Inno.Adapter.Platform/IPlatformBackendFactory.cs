using System.Collections.Generic;
using Inno.Platform;

namespace Inno.Adapter.Platform;

/// <summary>
/// Creates backend-neutral platform applications from explicit backend selections.
/// </summary>
public interface IPlatformBackendFactory
{
    /// <summary>
    /// Gets the exact registrations available in this composition snapshot.
    /// </summary>
    IReadOnlyList<PlatformBackendId> supportedBackends { get; }

    /// <summary>
    /// Creates a new platform application for the selected backend.
    /// </summary>
    /// <param name="backend">
    /// platform implementation selected by the composition root.
    /// </param>
    /// <returns>
    /// A caller-owned backend-neutral platform application.
    /// </returns>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when the catalog does not contain the selected backend.
    /// </exception>
    IPlatformApplication CreateApplication(PlatformBackendId backend);
}
