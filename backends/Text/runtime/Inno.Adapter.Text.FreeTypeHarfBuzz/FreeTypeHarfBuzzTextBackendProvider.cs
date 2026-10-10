using System;
using Inno.Text;

namespace Inno.Adapter.Text.FreeTypeHarfBuzz;

/// <summary>
/// Supplies the FreeTypeHarfBuzz implementation through the neutral text creation boundary.
/// </summary>
public sealed class FreeTypeHarfBuzzTextBackendProvider : TextBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public FreeTypeHarfBuzzTextBackendProvider() : base(TextBackendId.freeTypeHarfBuzz) { }

    /// <inheritdoc />
    public override ITextBackend CreateBackend()
    {
        return new FreeTypeHarfBuzzTextBackend();
    }
}

