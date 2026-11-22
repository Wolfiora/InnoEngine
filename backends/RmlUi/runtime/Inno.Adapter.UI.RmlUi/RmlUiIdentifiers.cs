using Inno.Adapter.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Publishes stable identities owned by the bundled RmlUi adapter.
/// </summary>
public static class RmlUiIdentifiers
{
    /// <summary>
    /// Gets the runtime implementation identity.
    /// </summary>
    public static UIBackendID backend { get; } = UIBackendID.rmlUI;

    /// <summary>
    /// Gets the RML source-language identity.
    /// </summary>
    public static UIDocumentLanguageID documentLanguage { get; } = new("inno.ui-language.rml");
}
