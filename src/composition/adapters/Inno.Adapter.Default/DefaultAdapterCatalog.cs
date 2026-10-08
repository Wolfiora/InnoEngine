using System;
using System.Collections.Generic;

using Inno.Adapter.Audio;
using Inno.Adapter.Audio.MiniAudio;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.Text.FreeTypeHarfBuzz;
using Inno.Adapter.UI;
using Inno.Adapter.UI.RmlUi;

namespace Inno.Adapter.Default;

/// <summary>
/// Composes independent, open provider catalogs for the standard engine distribution.
/// </summary>
/// <remarks>
/// Replacement sequences define the complete registrations for their domain.
/// The composition owns providers; service creation and disposal remain with each caller.
/// </remarks>
public sealed class DefaultAdapterCatalog : IAdapterCatalog
{
    private readonly IPlatformBackendFactory m_platform;
    private readonly InputBackendCatalog m_input;
    private readonly IStorageBackendFactory m_storage;
    private readonly RenderingBackendCatalog m_rendering;
    private readonly AudioBackendCatalog m_audio;
    private readonly TextBackendCatalog m_text;
    private readonly UiBackendCatalog m_ui;

    /// <summary>
    /// Captures each domain registration snapshot without initializing native services.
    /// </summary>
    /// <param name="options">
    /// Explicit host configuration for location-dependent services.
    /// </param>
    /// <param name="renderingProviders">
    /// Complete rendering registrations, or null to use the bundled implementation.
    /// </param>
    /// <param name="uiProviders">
    /// Complete ui registrations, or null to use the bundled implementation.
    /// </param>
    /// <param name="inputProviders">
    /// Complete input registrations, or null to use the bundled implementation.
    /// </param>
    /// <param name="audioProviders">
    /// Complete audio registrations, or null to use the bundled implementation.
    /// </param>
    /// <param name="textProviders">
    /// Complete text registrations, or null to use the bundled implementation.
    /// </param>
    public DefaultAdapterCatalog(
        DefaultAdapterCatalogOptions options,
        IEnumerable<RenderingBackendProvider>? renderingProviders = null,
        IEnumerable<UiBackendProvider>? uiProviders = null,
        IEnumerable<InputBackendProvider>? inputProviders = null,
        IEnumerable<AudioBackendProvider>? audioProviders = null,
        IEnumerable<TextBackendProvider>? textProviders = null
    ) {
        ArgumentNullException.ThrowIfNull(options);
        m_platform = options.platform ?? throw new ArgumentException("Host platform must be configured.", nameof(options));
        m_input = new InputBackendCatalog(inputProviders ?? [new EventInputBackendProvider()]);
        m_storage = options.storage ?? throw new ArgumentException("Host storage must be configured.", nameof(options));
        m_rendering = new RenderingBackendCatalog(renderingProviders ?? [new BgfxRenderingBackendProvider()]);
        m_audio = new AudioBackendCatalog(audioProviders ?? [new MiniAudioBackendProvider()]);
        m_text = new TextBackendCatalog(textProviders ?? [new FreeTypeHarfBuzzTextBackendProvider()]);
        m_ui = new UiBackendCatalog(uiProviders ?? [new RmlUiBackendProvider()]);
    }

    /// <summary>
    /// Gets the composition-owned platform factory snapshot.
    /// </summary>
    public IPlatformBackendFactory platform => m_platform;

    /// <summary>
    /// Gets the composition-owned input factory snapshot.
    /// </summary>
    public IInputBackendFactory input => m_input;

    /// <summary>
    /// Gets the composition-owned storage factory snapshot.
    /// </summary>
    public IStorageBackendFactory storage => m_storage;

    /// <summary>
    /// Gets the composition-owned rendering factory snapshot.
    /// </summary>
    public IRenderingBackendFactory rendering => m_rendering;

    /// <summary>
    /// Gets the composition-owned audio factory snapshot.
    /// </summary>
    public IAudioBackendFactory audio => m_audio;

    /// <summary>
    /// Gets the composition-owned text factory snapshot.
    /// </summary>
    public ITextBackendFactory text => m_text;

    /// <summary>
    /// Gets the composition-owned ui factory snapshot.
    /// </summary>
    public IUiBackendFactory ui => m_ui;
}
