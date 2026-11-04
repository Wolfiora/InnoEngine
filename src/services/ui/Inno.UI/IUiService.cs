using System;
using System.Collections.Generic;
using Inno.Core.Mathematics;


namespace Inno.UI;

/// <summary>
/// Provides retained-mode documents and backend-neutral UI frames to scripts and rendering plugins.
/// </summary>
public interface IUiService
{
    /// <summary>
    /// Refreshes visual layout once and captures a batch of element geometry without input or time advancement.
    /// </summary>
    /// <param name="context">
    /// The live context owned by the calling thread.
    /// </param>
    /// <param name="document">
    /// The live document belonging to the context.
    /// </param>
    /// <param name="elementIds">
    /// A non-empty list of non-empty authored IDs in result order; missing and hidden elements remain explicit results.
    /// </param>
    /// <returns>
    /// An immutable snapshot in context pixels. It does not retain a document lease or native memory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A requested ID is empty or an owner handle is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The element list is null.
    /// </exception>
    UiLayoutSnapshot CaptureLayout(
        UiContextHandle context,
        UiDocumentHandle document,
        IReadOnlyList<string> elementIds
    );

    /// <summary>
    /// Gets immutable optional capabilities of this service generation.
    /// </summary>
    UiBackendCapabilities capabilities { get; }

    /// <summary>
    /// Reads the context clock without advancing it.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <returns>
    /// The current elapsed seconds and pause state.
    /// </returns>
    UiContextTime GetTime(UiContextHandle context);

    /// <summary>
    /// Controls automatic time advancement; input remains independently routable.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="paused">
    /// True to freeze automatic time; explicit steps remain available.
    /// </param>
    void SetTimePaused(
        UiContextHandle context,
        bool paused
    );

    /// <summary>
    /// Advances only the context clock without processing input or refreshing layout.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="seconds">
    /// Finite nonnegative time to advance, including while paused.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The step is negative, nonfinite or overflows the clock.
    /// </exception>
    void AdvanceTime(
        UiContextHandle context,
        double seconds
    );

    /// <summary>
    /// Requests an absolute time seek only when the backend can preserve document semantics.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="seconds">
    /// The finite nonnegative requested time.
    /// </param>
    /// <returns>
    /// True if applied; false if unsupported, with all state unchanged.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The requested time is negative or nonfinite.
    /// </exception>
    bool TrySeekTime(
        UiContextHandle context,
        double seconds
    );

    /// <summary>
    /// Refreshes document style and layout at the current context time without processing input.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    void RefreshLayout(UiContextHandle context);

    /// <summary>
    /// Processes the explicitly routed input without advancing time or updating the whole context.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="input">
    /// The current immutable input sample.
    /// </param>
    void ProcessInput(
        UiContextHandle context,
        UiInputSnapshot input
    );


    /// <summary>
    /// Reads a control value on the context owner thread without changing layout or input.
    /// </summary>
    /// <param name="context">
    /// The live context owning the document.
    /// </param>
    /// <param name="document">
    /// The live document containing the control.
    /// </param>
    /// <param name="elementId">
    /// The nonempty authored element ID.
    /// </param>
    /// <returns>
    /// The current value, or None for a missing element or unsupported control.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A handle is stale or the caller violates ownership.
    /// </exception>
    UiControlValue GetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    );

    /// <summary>
    /// Writes a matching control value without generating value-change or submit events.
    /// </summary>
    /// <param name="context">
    /// The live context owning the document.
    /// </param>
    /// <param name="document">
    /// The live document containing the control.
    /// </param>
    /// <param name="elementId">
    /// The nonempty authored element ID.
    /// </param>
    /// <param name="value">
    /// The typed value; normal control clamping still applies.
    /// </param>
    /// <returns>
    /// True when the control accepts the value; false when absent, unsupported or no matching option exists.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The value kind does not match the control.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A handle is stale or the caller violates ownership.
    /// </exception>
    bool SetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiControlValue value
    );


    /// <summary>
    /// Requests focus through the context's existing focus and modal policy on its owner thread.
    /// </summary>
    /// <param name="context">
    /// The context owning the document.
    /// </param>
    /// <param name="document">
    /// The live document containing the target.
    /// </param>
    /// <param name="elementId">
    /// The nonempty authored element ID.
    /// </param>
    /// <returns>
    /// The applied, unchanged, missing or ineligible request outcome.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The element ID is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A handle is stale, belongs to another context or the caller violates ownership.
    /// </exception>
    UiFocusResult FocusElement(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    );

    /// <summary>
    /// Reads the current focused control without changing layout or focus.
    /// </summary>
    /// <param name="context">
    /// The live context to query on its owner thread.
    /// </param>
    /// <returns>
    /// A neutral focus snapshot, or null when no document control has focus.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The context is stale or the caller violates ownership.
    /// </exception>
    UiFocusTarget? GetFocusedElement(UiContextHandle context);

    /// <summary>
    /// Reads the focused text control's projected area without advancing time or refreshing layout.
    /// </summary>
    /// <param name="context">
    /// The live context to query on its owner thread after the owner's layout update.
    /// </param>
    /// <returns>
    /// The positive text area in context pixels, or null when no visible editable text control
    /// has usable geometry. This is a control area, not an exact caret rectangle or a window lease.
    /// </returns>
    /// <remarks>
    /// The view owner maps this area into input coordinates before updating its text-input session.
    /// Requery after focus, document or layout changes; the returned value grants no input ownership.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The context is stale or the caller violates ownership.
    /// </exception>
    Rect? GetTextInputArea(UiContextHandle context);

    /// <summary>
    /// Returns focus to the owning document, preserving modal ownership and emitting normal blur events.
    /// </summary>
    /// <param name="context">
    /// The live context to modify on its owner thread.
    /// </param>
    /// <returns>
    /// True when no document control retains focus; false when focus policy refuses the change.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The context is stale or the caller violates ownership.
    /// </exception>
    bool ClearFocus(UiContextHandle context);

    /// <summary>
    /// Creates one independent UI context.
    /// </summary>
    /// <param name="options">
    /// Initial layout and density for the new context.
    /// </param>
    /// <returns>
    /// A handle owned by the caller until destruction.
    /// </returns>
    UiContextHandle CreateContext(UiContextOptions options);
    /// <summary>
    /// Destroys one UI context and all owned documents.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    void DestroyContext(UiContextHandle context);
    /// <summary>
    /// Changes one context's pixel dimensions and density.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="width">
    /// New viewport width in physical pixels.
    /// </param>
    /// <param name="height">
    /// New viewport height in physical pixels.
    /// </param>
    /// <param name="density">
    /// Physical pixels per logical layout pixel.
    /// </param>
    void SetViewport(
        UiContextHandle context,
        int width,
        int height,
        float density = 1f
    );
    /// <summary>
    /// Loads one explicitly tagged in-memory document.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="source">
    /// Tagged source or named texture source for this operation.
    /// </param>
    /// <returns>
    /// A document handle owned by the supplied context.
    /// </returns>
    UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentSource source
    );
    /// <summary>
    /// Loads one imported document compatible with the selected backend.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    /// <returns>
    /// A document handle owned by the supplied context.
    /// </returns>
    UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentAsset document
    );
    /// <summary>
    /// Shows one document.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    void ShowDocument(
        UiContextHandle context,
        UiDocumentHandle document
    );
    /// <summary>
    /// Hides one document.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    void HideDocument(
        UiContextHandle context,
        UiDocumentHandle document
    );
    /// <summary>
    /// Closes one document.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    void CloseDocument(
        UiContextHandle context,
        UiDocumentHandle document
    );
    /// <summary>
    /// Replaces an element's children with plain Unicode text.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    /// <param name="elementId">
    /// The target document element ID.
    /// </param>
    /// <param name="text">
    /// Escaped Unicode replacement text.
    /// </param>
    /// <returns>
    /// True when the element exists and its text was changed.
    /// </returns>
    bool SetText(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string text
    );
    /// <summary>
    /// Replaces an element's children with an explicitly tagged source-language fragment.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    /// <param name="elementId">
    /// The target document element ID.
    /// </param>
    /// <param name="fragment">
    /// Explicitly tagged replacement document fragment.
    /// </param>
    /// <returns>
    /// True when the element exists and its content was changed.
    /// </returns>
    bool SetContent(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiDocumentFragment fragment
    );
    /// <summary>
    /// Sets one element attribute.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    /// <param name="elementId">
    /// The target document element ID.
    /// </param>
    /// <param name="name">
    /// Attribute name to change.
    /// </param>
    /// <param name="value">
    /// New attribute value.
    /// </param>
    /// <returns>
    /// True when the element exists and its attribute was changed.
    /// </returns>
    bool SetAttribute(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string name,
        string value
    );
    /// <summary>
    /// Activates or deactivates one element class.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="document">
    /// The document owned by the supplied context.
    /// </param>
    /// <param name="elementId">
    /// The target document element ID.
    /// </param>
    /// <param name="className">
    /// CSS class name to toggle.
    /// </param>
    /// <param name="active">
    /// Whether the CSS class is enabled.
    /// </param>
    /// <returns>
    /// True when the element exists and its class was changed.
    /// </returns>
    bool SetClass(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string className,
        bool active
    );
    /// <summary>
    /// Registers an imported image without decoding pixels or acquiring a second GPU resource.
    /// </summary>
    /// <param name="context">
    /// Owning context on the current UI thread.
    /// </param>
    /// <param name="image">
    /// Frozen artifact identity and imported dimensions.
    /// </param>
    /// <returns>
    /// A context-local source registration requiring deterministic disposal.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The image is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The context is unavailable or the caller is not its owner.
    /// </exception>
    IUiImageRegistration RegisterImage(
        UiContextHandle context,
        UiImageSource image
    );

    /// <summary>
    /// Registers an owned pixel source for a UI context.
    /// </summary>
    /// <param name="context">
    /// Owning UI context.
    /// </param>
    /// <param name="source">
    /// Document image source.
    /// </param>
    /// <param name="texture">
    /// Pixel data copied by the backend.
    /// </param>
    void RegisterTexture(
        UiContextHandle context,
        string source,
        UiTextureData texture
    );
    /// <summary>
    /// Tests whether a visible document element receives pointer events at a pixel location.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="position">
    /// Pointer position in context pixels.
    /// </param>
    /// <returns>
    /// True when an interactive visible element occupies the location.
    /// </returns>
    bool HasElementAtPoint(
        UiContextHandle context,
        Vector2 position
    );
    /// <summary>
    /// Processes current-frame input and advances one context.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    void Update(UiContextHandle context);
    /// <summary>
    /// Advances one context with explicitly routed viewport-local input.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="input">
    /// The input snapshot routed to this context.
    /// </param>
    void Update(
        UiContextHandle context,
        UiInputSnapshot input
    );
    /// <summary>
    /// Builds an immutable frame of incremental resources and ordered draws.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <returns>
    /// An immutable frame of ordered UI draws and resources.
    /// </returns>
    UiRenderFrame Render(UiContextHandle context);
    /// <summary>
    /// Drains document events queued by preceding updates.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <returns>
    /// Events emitted since the preceding drain, in dispatch order.
    /// </returns>
    IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context);
}
