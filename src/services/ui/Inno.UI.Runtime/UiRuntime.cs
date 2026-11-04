using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Inno.Assets;
using Inno.Core.Execution;
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Runtime;
using Inno.Runtime.Contracts;
using Inno.Text;

namespace Inno.UI.Runtime;

/// <summary>
/// Owns retained UI contexts, imported resource leases, and current-frame input for one runtime session.
/// </summary>
public sealed class UiRuntime : RuntimeSubsystem, IUiService
{
    private readonly int m_ownerThread = Environment.CurrentManagedThreadId;
    private readonly IUiBackend m_backend;
    private readonly HashSet<ImageResidency> m_images = [];
    private readonly IAssetArtifactLookup m_artifacts;
    private readonly Dictionary<FontRegistration, FontResidency> m_fonts = [];
    private readonly Dictionary<DocumentKey, DocumentResources> m_documentResources = [];
    private readonly List<DocumentResources> m_pendingResources = [];
    private readonly HashSet<UiContextHandle> m_retiringContexts = [];
    private InputSnapshot m_input = InputSnapshot.empty;
    private bool m_stopping;
    private bool m_backendRetired;
    private bool m_disposed;

    /// <summary>
    /// Creates a UI runtime and assumes ownership of its backend.
    /// </summary>
    /// <param name="backend">
    /// The retained-mode UI backend.
    /// </param>
    /// <param name="artifacts">
    /// The immutable asset artifact lookup.
    /// </param>
    public UiRuntime(
        IUiBackend backend,
        IAssetArtifactLookup artifacts
    ) {
        m_backend = backend ?? throw new ArgumentNullException(nameof(backend));
        m_artifacts = artifacts ?? throw new ArgumentNullException(nameof(artifacts));
    }

    /// <inheritdoc />
    public UiBackendCapabilities capabilities => m_backend.capabilities;

    /// <inheritdoc cref="IUiService.GetTime" />
    public UiContextTime GetTime(UiContextHandle context)
    {
        EnsureActive();
        return m_backend.GetTime(context);
    }

    /// <inheritdoc cref="IUiService.SetTimePaused" />
    public void SetTimePaused(
        UiContextHandle context,
        bool paused
    ) {
        EnsureActive();
        m_backend.SetTimePaused(context, paused);
    }

    /// <inheritdoc cref="IUiService.AdvanceTime" />
    public void AdvanceTime(
        UiContextHandle context,
        double seconds
    ) {
        EnsureActive();
        m_backend.AdvanceTime(context, seconds);
    }

    /// <inheritdoc cref="IUiService.TrySeekTime" />
    public bool TrySeekTime(
        UiContextHandle context,
        double seconds
    ) {
        EnsureActive();
        return m_backend.TrySeekTime(context, seconds);
    }

    /// <inheritdoc cref="IUiService.RefreshLayout" />
    public void RefreshLayout(UiContextHandle context)
    {
        EnsureActive();
        m_backend.RefreshLayout(context);
    }

    /// <inheritdoc cref="IUiService.ProcessInput" />
    public void ProcessInput(
        UiContextHandle context,
        UiInputSnapshot input
    ) {
        EnsureActive();
        m_backend.ProcessInput(context, input);
    }

    /// <inheritdoc />
    public UiControlValue GetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) {
        EnsureActive();
        return m_backend.GetValue(context, document, elementId);
    }

    /// <inheritdoc />
    public bool SetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiControlValue value
    ) {
        EnsureActive();
        return m_backend.SetValue(context, document, elementId, value);
    }

    /// <inheritdoc />
    public UiFocusResult FocusElement(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) {
        EnsureActive();
        return m_backend.FocusElement(context, document, elementId);
    }

    /// <inheritdoc />
    public UiFocusTarget? GetFocusedElement(UiContextHandle context)
    {
        EnsureActive();
        return m_backend.GetFocusedElement(context);
    }

    /// <inheritdoc />
    public bool ClearFocus(UiContextHandle context)
    {
        EnsureActive();
        return m_backend.ClearFocus(context);
    }

    /// <summary>
    /// Captures the input service snapshot and binds script-facing UI for the complete runtime frame.
    /// </summary>
    /// <param name="frame">
    /// The current runtime frame.
    /// </param>
    protected override void OnBeginFrame(RuntimeFrame frame)
    {
        m_input = InputExecutionContext.current.snapshot;
        OwnFrameScope(EnterExecutionScope());
    }

    /// <summary>
    /// Binds this runtime to the current asynchronous execution context.
    /// </summary>
    /// <returns>
    /// The caller-owned binding scope.
    /// </returns>
    public IDisposable EnterExecutionScope()
    {
        EnsureActive();
        return UiExecutionContext.EnterScope(this);
    }

    /// <summary>
    /// Creates a context using this implementation's validated inputs.
    /// </summary>
    /// <param name="options">
    /// The validated configuration that controls this operation.
    /// </param>
    /// <returns>
    /// The validated ui context handle that represents the completed operation.
    /// </returns>
    public UiContextHandle CreateContext(UiContextOptions options)
    {
        EnsureActive();
        return m_backend.CreateContext(options);
    }

    /// <summary>
    /// Destroys the context after its in-flight references retire.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void DestroyContext(UiContextHandle context)
    {
        EnsureActive(allowContextRetirement: true);
        if (!m_retiringContexts.Contains(context))
        {
            try
            {
                m_backend.DestroyContext(context);
            }
            catch
            {
                m_stopping = true;
                throw;
            }
            m_retiringContexts.Add(context);
        }
        DocumentKey[] keys = m_documentResources.Keys.Where(key => key.context == context).ToArray();
        foreach (DocumentKey key in keys)
            m_documentResources[key].documentClosed = true;
        foreach (ImageResidency image in m_images.Where(image => image.context == context).ToArray())
            image.Dispose();
        foreach (DocumentKey key in keys)
        {
            DocumentResources resources = m_documentResources[key];
            ReleaseResources(resources);
            m_documentResources.Remove(key);
        }
        m_retiringContexts.Remove(context);
    }

    /// <summary>
    /// Updates the viewport state and applies the resulting invariants.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="width">
    /// The width in logical units or pixels required by this operation.
    /// </param>
    /// <param name="height">
    /// The height in logical units or pixels required by this operation.
    /// </param>
    /// <param name="density">
    /// The density consumed by set viewport; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void SetViewport(
        UiContextHandle context,
        int width,
        int height,
        float density = 1f
    ) {
        EnsureActive();
        m_backend.SetViewport(context, width, height, density);
    }

    /// <summary>
    /// Loads a document into the specified independent UI context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="source">
    /// The source value or location read by this operation.
    /// </param>
    /// <returns>
    /// The validated ui document handle that represents the completed operation.
    /// </returns>
    public UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentSource source
    ) {
        ArgumentNullException.ThrowIfNull(source);
        EnsureActive();
        EnsureLanguageSupported(source.language);
        return m_backend.LoadDocument(context, source);
    }

    /// <summary>
    /// Loads a document into the specified independent UI context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by load document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated ui document handle that represents the completed operation.
    /// </returns>
    public UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentAsset document
    ) {
        ArgumentNullException.ThrowIfNull(document);
        EnsureActive();
        if (document.isMissing)
            throw new InvalidOperationException("The UI document asset is missing.");
        UiDocumentSource source = document.source
            ?? throw new InvalidOperationException("The UI document has no imported runtime payload.");
        if (!string.Equals(document.implementationId, m_backend.implementationId, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"UI document implementation '{document.implementationId}' cannot be loaded by selected backend '{m_backend.implementationId}'.");
        EnsureLanguageSupported(source.language);
        var families = new Dictionary<string, string>(StringComparer.Ordinal);
        var resources = new DocumentResources();
        var bindings = new Dictionary<string, string>(StringComparer.Ordinal);
        try
        {
            foreach (UiDocumentFontFace face in document.fonts)
            {
                if (!families.TryGetValue(face.family, out string? versionedFamily))
                {
                    versionedFamily = face.family + "__v"
                        + document.contentVersion.ToString("x", CultureInfo.InvariantCulture);
                    families.Add(face.family, versionedFamily);
                    bindings.Add(face.family, versionedFamily);
                }
                resources.fonts.Add(RegisterDocumentFont(face, document.contentVersion, versionedFamily));
            }
            foreach (UiDocumentImage declaration in document.images)
            {
                IUiImageRegistration image = RegisterImage(context, declaration.image);
                resources.images.Add(image);
                bindings.Add(declaration.source, image.source);
            }
            UiDocumentHandle handle = m_backend.LoadDocument(context,
                new UiDocumentSource(source.language, source.text, source.sourceUri, bindings));
            if (resources.fonts.Count > 0 || resources.images.Count > 0)
            {
                var key = new DocumentKey(context, handle);
                if (!m_documentResources.TryAdd(key, resources))
                {
                    m_backend.CloseDocument(context, handle);
                    throw new InvalidOperationException("The UI backend returned a document handle that is already in use.");
                }
            }
            return handle;
        }
        catch (Exception failure)
        {
            try
            {
                ReleaseResources(resources);
            }
            catch (Exception cleanupFailure)
            {
                m_pendingResources.Add(resources);
                throw new AggregateException("UI document preparation and resource retirement failed.", failure, cleanupFailure);
            }
            throw;
        }
    }

    /// <summary>
    /// Makes the selected document visible in its owning UI context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void ShowDocument(
        UiContextHandle context,
        UiDocumentHandle document
    ) {
        EnsureActive();
        m_backend.ShowDocument(context, document);
    }

    /// <summary>
    /// Hides the selected document while retaining its state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void HideDocument(
        UiContextHandle context,
        UiDocumentHandle document
    ) {
        EnsureActive();
        m_backend.HideDocument(context, document);
    }

    /// <summary>
    /// Closes the selected document and releases its retained state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void CloseDocument(
        UiContextHandle context,
        UiDocumentHandle document
    ) {
        EnsureActive();
        var key = new DocumentKey(context, document);
        if (!m_documentResources.TryGetValue(key, out DocumentResources? resources))
        {
            m_backend.CloseDocument(context, document);
            return;
        }
        if (!resources.documentClosed)
        {
            m_backend.CloseDocument(context, document);
            resources.documentClosed = true;
        }
        ReleaseResources(resources);
        m_documentResources.Remove(key);
    }

    /// <summary>
    /// Updates the text state and applies the resulting invariants.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by set text; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="elementId">
    /// The element id text validated by the set text operation.
    /// </param>
    /// <param name="text">
    /// The text text validated by the set text operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetText(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string text
    ) {
        EnsureActive();
        return m_backend.SetText(context, document, elementId, text);
    }

    /// <summary>
    /// Updates the content state and applies the resulting invariants.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by set content; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="elementId">
    /// The element id text validated by the set content operation.
    /// </param>
    /// <param name="content">
    /// The content consumed by set content; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetContent(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiDocumentFragment content
    ) {
        EnsureActive();
        EnsureLanguageSupported(content.language);
        return m_backend.SetContent(context, document, elementId, content);
    }

    /// <summary>
    /// Updates the attribute state and applies the resulting invariants.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by set attribute; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="elementId">
    /// The element id text validated by the set attribute operation.
    /// </param>
    /// <param name="name">
    /// The human-readable name used for presentation and diagnostics.
    /// </param>
    /// <param name="value">
    /// The concrete value read or transformed by this operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetAttribute(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string name,
        string value
    ) {
        EnsureActive();
        return m_backend.SetAttribute(context, document, elementId, name, value);
    }

    /// <summary>
    /// Updates the class state and applies the resulting invariants.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by set class; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="elementId">
    /// The element id text validated by the set class operation.
    /// </param>
    /// <param name="className">
    /// The class name text validated by the set class operation.
    /// </param>
    /// <param name="active">
    /// Whether active behavior is enabled while set class executes.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetClass(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string className,
        bool active
    ) {
        EnsureActive();
        return m_backend.SetClass(context, document, elementId, className, active);
    }

    /// <summary>
    /// Registers a named RGBA texture source for UI document drawing.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="source">
    /// The source value or location read by this operation.
    /// </param>
    /// <param name="texture">
    /// The texture consumed by register texture; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void RegisterTexture(
        UiContextHandle context,
        string source,
        UiTextureData texture
    ) {
        EnsureActive();
        m_backend.RegisterTexture(context, source, texture);
    }

    /// <summary>
    /// Tests whether an interactive document element occupies the supplied point.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="position">
    /// The position consumed by has element at point; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasElementAtPoint(
        UiContextHandle context,
        Vector2 position
    ) {
        EnsureActive();
        return m_backend.HasElementAtPoint(context, position);
    }

    /// <summary>
    /// Recomputes owned state from the current validated inputs.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Update(UiContextHandle context)
    {
        EnsureActive();
        m_backend.Update(context, new UiInputSnapshot(
            m_input.mousePosition,
            m_input.modifiers,
            m_input.sequence));
    }

    /// <summary>
    /// Advances one UI context with input routed from its rendered view.
    /// </summary>
    /// <param name="context">
    /// The context that owns the active document.
    /// </param>
    /// <param name="input">
    /// Input mapped into the context's local pixel coordinates.
    /// </param>
    public void Update(
        UiContextHandle context,
        UiInputSnapshot input
    ) {
        EnsureActive();
        m_backend.Update(context, input ?? throw new ArgumentNullException(nameof(input)));
    }

    /// <summary>
    /// Records value rendering for the current frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated ui render frame that represents the completed operation.
    /// </returns>
    public UiRenderFrame Render(UiContextHandle context)
    {
        EnsureActive();
        return m_backend.Render(context);
    }

    /// <summary>
    /// Returns and clears events emitted by this UI context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context)
    {
        EnsureActive();
        return m_backend.DrainEvents(context);
    }

    /// <summary>
    /// Retires native UI before releasing image registrations and retained artifacts.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// The backend or a retained resource has not retired; the owner must retain this runtime and retry.
    /// </exception>
    protected override void OnStop()
    {
        if (m_disposed)
            return;
        EnsureOwnerThread();
        m_stopping = true;
        if (!m_backendRetired)
        {
            try
            {
                m_backend.Dispose();
                m_backendRetired = true;
            }
            catch (Exception exception)
            {
                throw new RetirementPendingException("UI backend retirement is incomplete; its assets remain retained.", exception);
            }
        }
        List<Exception> failures = [];
        foreach (ImageResidency image in m_images.ToArray())
        {
            try
            {
                image.Dispose();
            }
            catch (Exception exception)
            {
                failures.Add(exception);
            }
        }
        foreach ((FontRegistration registration, FontResidency residency) in m_fonts.ToArray())
        {
            try
            {
                residency.lease.Dispose();
                m_fonts.Remove(registration);
            }
            catch (Exception exception)
            {
                failures.Add(exception);
            }
        }
        if (failures.Count > 0)
            throw new RetirementPendingException("UI resources have not retired.",
                new AggregateException("UI runtime resource retirement failed.", failures));
        m_documentResources.Clear();
        m_pendingResources.Clear();
        m_retiringContexts.Clear();
        m_disposed = true;
    }

    /// <inheritdoc />
    public UiLayoutSnapshot CaptureLayout(
        UiContextHandle context,
        UiDocumentHandle document,
        IReadOnlyList<string> elementIds
    ) {
        EnsureActive();
        return m_backend.CaptureLayout(context, document, elementIds);
    }

    /// <inheritdoc />
    public IUiImageRegistration RegisterImage(
        UiContextHandle context,
        UiImageSource image
    ) {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(image);
        ArtifactLease artifact = m_artifacts.AcquireArtifact(image.artifact.assetId, image.artifact.slot.sourceOutputName);
        var residency = new ImageResidency(this, context, artifact);
        m_images.Add(residency);
        try
        {
            residency.registration = m_backend.RegisterImage(context, image);
            return residency;
        }
        catch (Exception failure)
        {
            try
            {
                residency.Dispose();
            }
            catch (Exception cleanupFailure)
            {
                m_stopping = true;
                throw new AggregateException("UI image registration and artifact retirement failed.", failure, cleanupFailure);
            }
            throw;
        }
    }

    private void EnsureActive(bool allowContextRetirement = false)
    {
        EnsureOwnerThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_stopping || (!allowContextRetirement && m_retiringContexts.Count != 0))
            throw new InvalidOperationException("UI resource retirement is incomplete; retry retirement before further operations.");
        if (m_pendingResources.Count != 0)
            throw new InvalidOperationException("UI document preparation left pending resource retirement; retire the owning runtime before further operations.");
    }

    private void EnsureOwnerThread()
    {
        if (Environment.CurrentManagedThreadId != m_ownerThread)
            throw new InvalidOperationException("UI runtime operations require the owner thread.");
    }

    private void RegisterBackendFont(
        ArtifactLease artifact,
        FontRegistration registration
    )
        => m_backend.RegisterFont(new UiFontRegistration(
            artifact.ReadAllBytes(),
            registration.faceIndex,
            registration.family,
            registration.style,
            registration.weight));

    private FontRegistration RegisterDocumentFont(
        UiDocumentFontFace face,
        long documentVersion,
        string family
    ) {
        var registration = new FontRegistration(face.assetId, documentVersion, 0,
            family, face.style, face.weight);
        if (m_fonts.TryGetValue(registration, out FontResidency? existing))
        {
            RegisterBackendFont(existing.lease, registration);
            existing.references++;
            return registration;
        }
        ArtifactLease artifact = m_artifacts.AcquireArtifact(face.assetId, "font-data");
        try
        {
            RegisterBackendFont(artifact, registration);
            m_fonts.Add(registration, new FontResidency(artifact));
            return registration;
        }
        catch
        {
            artifact.Dispose();
            throw;
        }
    }

    private void ReleaseResources(DocumentResources resources)
    {
        List<Exception>? failures = null;
        for (int index = resources.images.Count - 1; index >= 0; index--)
        {
            try
            {
                resources.images[index].Dispose();
                resources.images.RemoveAt(index);
            }
            catch (Exception exception)
            {
                (failures ??= []).Add(exception);
            }
        }
        for (int index = resources.fonts.Count - 1; index >= 0; index--)
        {
            FontRegistration registration = resources.fonts[index];
            FontResidency residency = m_fonts[registration];
            try
            {
                if (residency.references == 1)
                {
                    residency.lease.Dispose();
                    m_fonts.Remove(registration);
                }
                else
                    residency.references--;
                resources.fonts.RemoveAt(index);
            }
            catch (Exception exception)
            {
                (failures ??= []).Add(exception);
            }
        }
        if (failures is not null)
            throw new AggregateException("UI document resource retirement is incomplete.", failures);
    }

    private sealed class ImageResidency : IUiImageRegistration
    {
        private readonly UiRuntime m_owner;
        private ArtifactLease? m_artifact;

        internal ImageResidency(
            UiRuntime owner,
            UiContextHandle context,
            ArtifactLease artifact
        ) {
            m_owner = owner;
            this.context = context;
            m_artifact = artifact;
        }

        /// <inheritdoc />
        public string source => registration?.source
            ?? throw new ObjectDisposedException(nameof(IUiImageRegistration));

        /// <inheritdoc />
        public void Dispose()
        {
            if (Environment.CurrentManagedThreadId != m_owner.m_ownerThread)
                throw new InvalidOperationException("A UI image must be retired on its owner thread.");
            registration?.Dispose();
            registration = null;
            m_artifact?.Dispose();
            m_artifact = null;
            m_owner.m_images.Remove(this);
        }

        internal UiContextHandle context { get; }
        internal IUiImageRegistration? registration { get; set; }
    }

    private sealed class DocumentResources
    {
        internal readonly List<FontRegistration> fonts = [];
        internal readonly List<IUiImageRegistration> images = [];
        internal bool documentClosed;
    }

    private sealed class FontResidency(ArtifactLease lease)
    {
        internal ArtifactLease lease { get; } = lease;
        internal int references = 1;
    }

    private readonly record struct DocumentKey(
        UiContextHandle context,
        UiDocumentHandle document
    );

    private void EnsureLanguageSupported(UiDocumentLanguageId language)
    {
        if (!m_backend.capabilities.Supports(language))
            throw new NotSupportedException(
                $"UI document language '{language}' is not supported by selected backend '{m_backend.implementationId}'.");
    }

    private readonly record struct FontRegistration(
        Guid persistentId,
        long version,
        int faceIndex,
        string family,
        TextFontStyle style,
        int weight
    );


}
