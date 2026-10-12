using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Owns a successfully selected Pack and immutable diagnostics from work after its commit.
/// </summary>
public sealed class PlayerSupportPackPublication : IDisposable
{
    private readonly PlayerSupportPackLease m_pack;

    internal PlayerSupportPackPublication(
        PlayerSupportPackLease pack,
        IEnumerable<string> diagnostics
    ) {
        m_pack = pack;
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the verified Pack directory while this result holds reading ownership.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The publication's reading ownership has been released.
    /// </exception>
    public string directory => m_pack.directory;

    /// <summary>
    /// Gets indexing or cleanup diagnostics which do not undo the successful selection.
    /// An empty collection means all post-commit work completed.
    /// </summary>
    public IReadOnlyList<string> diagnostics { get; }

    /// <summary>
    /// Releases the selected Pack after every reader and tool has finished; repeated disposal has no effect.
    /// </summary>
    public void Dispose() => m_pack.Dispose();
}
