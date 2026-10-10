using Inno.Adapter.Storage;
using Inno.Adapter.Rendering;
using Inno.Adapter.Platform;

namespace Inno.Adapter.Default;

/// <summary>
/// Supplies host-owned services whose locations cannot be inferred by the engine distribution.
/// </summary>
public sealed class DefaultAdapterCatalogOptions
{
    /// <summary>
    /// Gets or initializes the explicitly selected platform factory and host integration.
    /// </summary>
    public required IPlatformBackendFactory platform { get; init; }

    /// <summary>
    /// Gets or initializes the immutable storage factory configured for this host.
    /// </summary>
    public required IStorageBackendFactory storage { get; init; }

    /// <summary>
    /// Gets or initializes the explicitly selected rendering factory and its host surface integration.
    /// </summary>
    public required IRenderingBackendFactory rendering { get; init; }
}
