using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Runtime;
using Inno.Runtime.Contracts;
using Inno.Text;

namespace Inno.UI.Runtime;

/// <summary>
/// Owns retained UI contexts, imported font leases, and current-frame input for one runtime session.
/// </summary>
public sealed class UIRuntime : RuntimeSubsystem, IUIService
{
    private readonly IUIBackend m_backend;
    private readonly IAssetArtifactLookup m_artifacts;
    private readonly Dictionary<FontRegistration, FontResidency> m_fonts = [];
    private readonly Dictionary<DocumentKey, FontRegistration[]> m_documentFonts = [];
    private InputSnapshot m_input = InputSnapshot.empty;
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
    public UIRuntime(
        IUIBackend backend,
        IAssetArtifactLookup artifacts
    ) {
        ArgumentNullException.ThrowIfNull(backend);
        ArgumentNullException.ThrowIfNull(artifacts);
        m_backend = backend;
        m_artifacts = artifacts;
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
        return UIExecutionContext.EnterScope(this);
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
    public UIContextHandle CreateContext(UIContextOptions options)
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
    public void DestroyContext(UIContextHandle context)
    {
        EnsureActive();
        m_backend.DestroyContext(context);
        foreach (DocumentKey key in m_documentFonts.Keys.Where(key => key.context == context).ToArray())
        {
            FontRegistration[] registrations = m_documentFonts[key];
            m_documentFonts.Remove(key);
            ReleaseFonts(registrations);
        }
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
        UIContextHandle context,
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
    public UIDocumentHandle LoadDocument(
        UIContextHandle context,
        UIDocumentSource source
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
    public UIDocumentHandle LoadDocument(
        UIContextHandle context,
        UIDocumentAsset document
    ) {
        ArgumentNullException.ThrowIfNull(document);
        EnsureActive();
        if (document.isMissing)
        {
            throw new InvalidOperationException("The UI document asset is missing.");
        }
        UIDocumentSource source = document.source
                                  ?? throw new InvalidOperationException("The UI document has no imported runtime payload.");
        if (!string.Equals(document.implementationID, m_backend.implementationID, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"UI document implementation '{document.implementationID}' cannot be loaded by selected backend '{m_backend.implementationID}'."
            );
        }
        EnsureLanguageSupported(source.language);
        string text = source.text;
        var families = new Dictionary<string, string>(StringComparer.Ordinal);
        var registrations = new List<FontRegistration>(document.fonts.Count);
        try
        {
            foreach (UIDocumentFontFace face in document.fonts)
            {
                if (!families.TryGetValue(face.family, out string? versionedFamily))
                {
                    versionedFamily = face.family + "__v"
                                      + document.contentVersion.ToString("x", CultureInfo.InvariantCulture);
                    families.Add(face.family, versionedFamily);
                    text = text.Replace(face.family, versionedFamily, StringComparison.Ordinal);
                }
                registrations.Add(RegisterDocumentFont(face, document.contentVersion, versionedFamily));
            }
            UIDocumentHandle handle = m_backend.LoadDocument(
                context,
                new UIDocumentSource(source.language, text, source.sourceUri)
            );
            if (registrations.Count > 0)
            {
                var key = new DocumentKey(context, handle);
                if (!m_documentFonts.TryAdd(key, registrations.ToArray()))
                {
                    m_backend.CloseDocument(context, handle);
                    throw new InvalidOperationException("The UI backend returned a document handle that is already in use.");
                }
            }
            return handle;
        }
        catch
        {
            ReleaseFonts(registrations);
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
        UIContextHandle context,
        UIDocumentHandle document
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
        UIContextHandle context,
        UIDocumentHandle document
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
        UIContextHandle context,
        UIDocumentHandle document
    ) {
        EnsureActive();
        m_backend.CloseDocument(context, document);
        var key = new DocumentKey(context, document);
        if (m_documentFonts.Remove(key, out FontRegistration[]? registrations))
        {
            ReleaseFonts(registrations);
        }
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
    /// <param name="elementID">
    /// The element id text validated by the set text operation.
    /// </param>
    /// <param name="text">
    /// The text text validated by the set text operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetText(
        UIContextHandle context,
        UIDocumentHandle document,
        string elementID,
        string text
    ) {
        EnsureActive();
        return m_backend.SetText(context, document, elementID, text);
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
    /// <param name="elementID">
    /// The element id text validated by the set content operation.
    /// </param>
    /// <param name="content">
    /// The content consumed by set content; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public bool SetContent(
        UIContextHandle context,
        UIDocumentHandle document,
        string elementID,
        UIDocumentFragment content
    ) {
        EnsureActive();
        EnsureLanguageSupported(content.language);
        return m_backend.SetContent(context, document, elementID, content);
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
    /// <param name="elementID">
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
        UIContextHandle context,
        UIDocumentHandle document,
        string elementID,
        string name,
        string value
    ) {
        EnsureActive();
        return m_backend.SetAttribute(context, document, elementID, name, value);
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
    /// <param name="elementID">
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
        UIContextHandle context,
        UIDocumentHandle document,
        string elementID,
        string className,
        bool active
    ) {
        EnsureActive();
        return m_backend.SetClass(context, document, elementID, className, active);
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
        UIContextHandle context,
        string source,
        UITextureData texture
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
        UIContextHandle context,
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
    public void Update(UIContextHandle context)
    {
        EnsureActive();
        m_backend.Update(
            context,
            new UIInputSnapshot(
                m_input.mousePosition,
                m_input.scrollDelta,
                m_input.modifiers,
                m_input.keysPressed,
                m_input.keysReleased,
                m_input.mouseButtonsPressed,
                m_input.mouseButtonsReleased,
                m_input.textInput
            )
        );
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
        UIContextHandle context,
        UIInputSnapshot input
    ) {
        ArgumentNullException.ThrowIfNull(input);
        EnsureActive();
        m_backend.Update(context, input);
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
    public UIRenderFrame Render(UIContextHandle context)
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
    public IReadOnlyList<UIEvent> DrainEvents(UIContextHandle context)
    {
        EnsureActive();
        return m_backend.DrainEvents(context);
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
    /// Releases native UI state before releasing retained font artifacts.
    /// </summary>
    protected override void OnStop()
    {
        if (m_disposed)
        {
            return;
        }
        List<Exception> failures = [];
        try
        {
            m_backend.Dispose();
        }
        catch (Exception exception)
        {
            failures.Add(exception);
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
        m_documentFonts.Clear();
        if (failures.Count > 0)
        {
            throw new AggregateException("UI runtime retirement failed.", failures);
        }
        m_disposed = true;
    }

    private void EnsureActive() => ObjectDisposedException.ThrowIf(m_disposed, this);

    private void RegisterBackendFont(
        ArtifactLease artifact,
        FontRegistration registration
    ) => m_backend.RegisterFont(
        new UIFontRegistration(
            artifact.ReadAllBytes(),
            registration.faceIndex,
            registration.family,
            registration.style,
            registration.weight
        )
    );

    private FontRegistration RegisterDocumentFont(
        UIDocumentFontFace face,
        long documentVersion,
        string family
    ) {
        var registration = new FontRegistration(face.assetID, documentVersion, 0, family, face.style, face.weight);
        if (m_fonts.TryGetValue(registration, out FontResidency? existing))
        {
            RegisterBackendFont(existing.lease, registration);
            existing.references++;
            return registration;
        }
        ArtifactLease artifact = m_artifacts.AcquireArtifact(face.assetID, "font-data");
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

    private void ReleaseFonts(IEnumerable<FontRegistration> registrations)
    {
        List<Exception>? failures = null;
        foreach (FontRegistration registration in registrations)
        {
            FontResidency residency = m_fonts[registration];
            if (--residency.references > 0)
            {
                continue;
            }
            try
            {
                residency.lease.Dispose();
                m_fonts.Remove(registration);
            }
            catch (Exception exception)
            {
                (failures ??= []).Add(exception);
            }
        }
        if (failures is not null)
        {
            throw new AggregateException("UI font artifact retirement is incomplete.", failures);
        }
    }

    private void EnsureLanguageSupported(UIDocumentLanguageID language)
    {
        if (!m_backend.capabilities.Supports(language))
        {
            throw new NotSupportedException(
                $"UI document language '{language}' is not supported by selected backend '{m_backend.implementationID}'."
            );
        }
    }

    private sealed class FontResidency(ArtifactLease lease)
    {
        internal int references = 1;

        internal ArtifactLease lease { get; } = lease;
    }

    private readonly record struct DocumentKey(
        UIContextHandle context,
        UIDocumentHandle document
    );

    private readonly record struct FontRegistration(
        Guid persistentID,
        long version,
        int faceIndex,
        string family,
        TextFontStyle style,
        int weight
    );
}
