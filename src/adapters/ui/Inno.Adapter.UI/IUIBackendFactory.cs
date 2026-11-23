using System.Collections.Generic;
using Inno.UI;

namespace Inno.Adapter.UI;

/// <summary>
/// Creates isolated UI backends without exposing implementation assemblies to composition code.
/// </summary>
public interface IUIBackendFactory
{
    /// <summary>
    /// Gets exact backend identities available in this composition generation.
    /// </summary>
    IReadOnlyList<UIBackendID> supportedBackends { get; }

    /// <summary>
    /// Creates one caller-owned UI backend.
    /// </summary>
    /// <param name="backend">
    /// The selected backend implementation.
    /// </param>
    /// <returns>
    /// A newly allocated UI backend.
    /// </returns>
    IUIBackend CreateBackend(UIBackendID backend);
}
