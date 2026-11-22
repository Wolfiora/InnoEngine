using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Supplies independently owned retained UI backend generations.
/// </summary>
public sealed class RmlUiBackendProvider : UIBackendProvider
{
    /// <summary>
    /// Creates a composition-owned registration for the bundled implementation.
    /// </summary>
    public RmlUiBackendProvider()
        : base(UIBackendID.rmlUI) {}

    /// <inheritdoc/>
    public override IUIBackend CreateBackend() => new RmlUiBackend();
}
