using Inno.Adapter.Audio;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;

namespace Inno.Adapter.Default;

/// <summary>
/// Explicitly selects the shared backends shipped by the standard runtime composition.
/// </summary>
public static class StandardAdapterSelection
{
    /// <summary>
    /// Creates a complete selection while leaving the location-dependent storage choice to the product.
    /// </summary>
    /// <param name="storage">
    /// The storage implementation explicitly registered by the product.
    /// </param>
    /// <returns>
    /// A fully assigned standard selection; its catalog must still be validated before creating resources.
    /// </returns>
    public static AdapterSelection Create(StorageBackendId storage) => new()
    {
        platform = PlatformBackendId.sdl3,
        input = InputBackendId.events,
        storage = storage,
        rendering = RenderingBackendId.bgfx,
        audio = AudioBackendId.miniAudio,
        text = TextBackendId.freeTypeHarfBuzz,
        ui = UiBackendId.rmlUi
    };
}
