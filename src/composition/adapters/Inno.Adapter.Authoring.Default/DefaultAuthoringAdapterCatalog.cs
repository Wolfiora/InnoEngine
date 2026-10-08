using System;
using System.Collections.Generic;

using Inno.Adapter.Audio;
using Inno.Adapter.Default;
using Inno.Adapter.Input;
using Inno.Adapter.Platform;
using Inno.Adapter.Presentation;
using Inno.Adapter.Rendering;
using Inno.Adapter.Storage;
using Inno.Adapter.Text;
using Inno.Adapter.UI;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Adapter.Authoring.Default;

/// <summary>
/// Combines explicitly supplied rendering toolchains and standard ImGui presentation to the built-in runtime adapter catalog.
/// </summary>
public sealed class DefaultAuthoringAdapterCatalog :
    IAuthoringAdapterCatalog,
    IRenderingAuthoringBackendFactory
{
    private readonly DefaultAdapterCatalog m_runtime;
    private readonly RenderingAuthoringBackendCatalog m_renderingAuthoring;
    private readonly PresentationBackendCatalog m_presentation;

    /// <summary>
    /// Creates paired runtime and authoring registrations before any native device is initialized.
    /// </summary>
    /// <param name="options">
    /// The host's explicit runtime service configuration.
    /// </param>
    /// <param name="renderingProviders">
    /// Complete runtime rendering registrations, or null for bundled BGFX.
    /// </param>
    /// <param name="authoringProviders">
    /// Complete matching authoring registrations with explicit product compilation targets.
    /// </param>
    /// <param name="presentationProviders">
    /// Explicit complete presentation registrations. An empty sequence registers none.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The runtime and authoring backend registrations do not match.
    /// </exception>
    public DefaultAuthoringAdapterCatalog(
        DefaultAdapterCatalogOptions options,
        IEnumerable<RenderingAuthoringBackendProvider> authoringProviders,
        IEnumerable<PresentationBackendProvider> presentationProviders,
        IEnumerable<RenderingBackendProvider>? renderingProviders = null
    ) {
        ArgumentNullException.ThrowIfNull(authoringProviders);
        m_runtime = new DefaultAdapterCatalog(options, renderingProviders);
        ArgumentNullException.ThrowIfNull(presentationProviders);
        m_presentation = new PresentationBackendCatalog(presentationProviders);
        m_renderingAuthoring = new RenderingAuthoringBackendCatalog(
            m_runtime.rendering,
            authoringProviders);
    }

    IReadOnlyList<RenderingBackendId> IRenderingAuthoringBackendFactory.supportedBackends
        => m_renderingAuthoring.supportedBackends;

    /// <summary>
    /// Gets the built-in platform backend factory.
    /// </summary>
    public IPlatformBackendFactory platform => m_runtime.platform;

    /// <summary>
    /// Gets the built-in input backend factory.
    /// </summary>
    public IInputBackendFactory input => m_runtime.input;

    /// <summary>
    /// Gets the built-in application-storage backend factory.
    /// </summary>
    public IStorageBackendFactory storage => m_runtime.storage;

    /// <summary>
    /// Gets the built-in runtime rendering backend factory.
    /// </summary>
    public IRenderingBackendFactory rendering => m_runtime.rendering;

    /// <summary>
    /// Gets the built-in audio backend factory.
    /// </summary>
    public IAudioBackendFactory audio => m_runtime.audio;

    /// <summary>
    /// Gets the built-in Unicode text backend factory.
    /// </summary>
    public ITextBackendFactory text => m_runtime.text;

    /// <summary>
    /// Gets the built-in retained-mode UI backend factory.
    /// </summary>
    public IUiBackendFactory ui => m_runtime.ui;

    /// <summary>
    /// Gets the built-in rendering authoring toolchain factory.
    /// </summary>
    public IRenderingAuthoringBackendFactory renderingAuthoring => this;

    /// <summary>
    /// Gets the built-in graphical host-presentation factory.
    /// </summary>
    public IPresentationBackendFactory presentation => m_presentation;

    IShaderCompilerToolchain IRenderingAuthoringBackendFactory.CreateShaderCompilerToolchain(
        RenderingBackendId backend)
        => m_renderingAuthoring.CreateShaderCompilerToolchain(backend);

    ITextureTargetCompiler IRenderingAuthoringBackendFactory.CreateTextureTargetCompiler(
        RenderingBackendId backend)
        => m_renderingAuthoring.CreateTextureTargetCompiler(backend);

}
