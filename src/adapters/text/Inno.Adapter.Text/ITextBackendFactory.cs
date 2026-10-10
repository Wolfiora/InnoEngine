using System.Collections.Generic;
using Inno.Text;

namespace Inno.Adapter.Text;

/// <summary>
/// Creates isolated text backends without exposing implementation assemblies to composition code.
/// </summary>
public interface ITextBackendFactory
{
    /// <summary>
    /// Gets the exact registrations available in this composition snapshot.
    /// </summary>
    IReadOnlyList<TextBackendId> supportedBackends { get; }

    /// <summary>
    /// Creates one caller-owned text backend.
    /// </summary>
    /// <param name="backend">
    /// The selected backend implementation.
    /// </param>
    /// <returns>
    /// A newly allocated text backend.
    /// </returns>
    ITextBackend CreateBackend(TextBackendId backend);
}
