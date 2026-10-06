using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Adapter.Audio;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;

namespace Inno.Adapter;

/// <summary>
/// Selects one implementation for every replaceable host backend family.
/// </summary>
public readonly struct AdapterSelection()
{
    /// <summary>
    /// Gets the built-in default adapter selection shipped by the engine distribution.
    /// </summary>
    public static AdapterSelection defaultValue { get; } = new();

    /// <summary>
    /// Gets the selected platform backend.
    /// </summary>
    public PlatformBackendId platform { get; init; } = PlatformBackendId.sdl3;

    /// <summary>
    /// Gets the selected input backend.
    /// </summary>
    public InputBackendId input { get; init; } = InputBackendId.events;

    /// <summary>
    /// Gets the selected storage backend.
    /// </summary>
    public StorageBackendId storage { get; init; } = StorageBackendId.fileSystem;

    /// <summary>
    /// Gets the selected rendering backend.
    /// </summary>
    public RenderingBackendId rendering { get; init; } = RenderingBackendId.bgfx;

    /// <summary>
    /// Gets the selected audio backend.
    /// </summary>
    public AudioBackendId audio { get; init; } = AudioBackendId.miniAudio;

    /// <summary>
    /// Gets the selected Unicode text backend.
    /// </summary>
    public TextBackendId text { get; init; } = TextBackendId.freeTypeHarfBuzz;

    /// <summary>
    /// Gets the selected retained-mode UI backend.
    /// </summary>
    public UiBackendId ui { get; init; } = UiBackendId.rmlUi;

    /// <summary>
    /// Validates every selected registration before the composition creates any service.
    /// </summary>
    /// <param name="catalog">
    /// The complete, composition-owned factory snapshot.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The catalog is null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// A selected implementation is unassigned or absent from its domain catalog.
    /// </exception>
    public void Validate(IAdapterCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        Require(platform, catalog.platform.supportedBackends, "Platform");
        Require(input, catalog.input.supportedBackends, "Input");
        Require(storage, catalog.storage.supportedBackends, "Storage");
        Require(rendering, catalog.rendering.supportedBackends, "Rendering");
        Require(audio, catalog.audio.supportedBackends, "Audio");
        Require(text, catalog.text.supportedBackends, "Text");
        Require(ui, catalog.ui.supportedBackends, "UI");
    }

    private static void Require<TBackend>(
        TBackend backend,
        IReadOnlyList<TBackend> available,
        string domain
    ) where TBackend : struct, IEquatable<TBackend> {
        if (backend.Equals(default) || !available.Contains(backend))
            throw new NotSupportedException(domain + " backend '" + backend + "' is not registered.");
    }
}
