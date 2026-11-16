using System.Collections.Generic;
using Inno.Core.Mathematics;


namespace Inno.UI;

/// <summary>
/// Provides retained-mode documents and backend-neutral UI frames to scripts and rendering plugins.
/// </summary>
public interface IUiService
{
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
    /// Plain Unicode replacement text; the backend performs source-language escaping.
    /// </param>
    /// <returns>
    /// True when the element exists and the replacement was applied; false when the element is absent.
    /// This is not a comparison with the preceding text.
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
    /// True when the element exists and the fragment was applied; false when the element is absent.
    /// This is not a comparison with the preceding content.
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
    /// True when the element exists and the attribute was assigned; false when the element is absent.
    /// This is not a comparison with the preceding value.
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
    /// True when the element exists and its class state was assigned; false when the element is absent.
    /// This is not a comparison with the preceding state.
    /// </returns>
    bool SetClass(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string className,
        bool active
    );
    /// <summary>
    /// Registers one named RGBA8 texture source.
    /// </summary>
    /// <param name="context">
    /// The context that owns the requested document or state.
    /// </param>
    /// <param name="source">
    /// Tagged source or named texture source for this operation.
    /// </param>
    /// <param name="texture">
    /// Immutable RGBA texture data.
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
