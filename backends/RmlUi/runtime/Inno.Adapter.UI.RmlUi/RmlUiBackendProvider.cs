using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Supplies independently owned retained UI backend generations.
/// </summary>
public sealed class RmlUiBackendProvider : UiBackendProvider
{
    /// <summary>
    /// Creates a composition-owned registration for the bundled implementation.
    /// </summary>
    public RmlUiBackendProvider() : base(UiBackendId.rmlUi) { }

    /// <inheritdoc />
    public override IUiBackend CreateBackend() => new RmlUiBackend();
}
