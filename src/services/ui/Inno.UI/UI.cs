using System;
using System.Collections.Generic;

using Inno.Core.Execution;

namespace Inno.UI;

/// <summary>
/// Binds one UI service to the current asynchronous execution context.
/// </summary>
public static class UiExecutionContext
{
    private static readonly ExecutionSlot<IUiService> S_CURRENT_SCOPE = new("ui");

    /// <summary>
    /// Gets the UI service bound to the current execution context.
    /// </summary>
    public static IUiService current => S_CURRENT_SCOPE.current;

    /// <summary>
    /// Binds a UI service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="ui">
    /// The host-owned UI service.
    /// </param>
    /// <returns>
    /// The caller-owned binding scope.
    /// </returns>
    public static IDisposable EnterScope(IUiService ui)
    {
        ArgumentNullException.ThrowIfNull(ui);
        return S_CURRENT_SCOPE.Enter(ui);
    }
}

/// <summary>
/// Provides script-friendly retained-mode UI operations.
/// </summary>
public static class UI
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
    public static UiLayoutSnapshot CaptureLayout(
        UiContextHandle context,
        UiDocumentHandle document,
        IReadOnlyList<string> elementIds
    ) => UiExecutionContext.current.CaptureLayout(context, document, elementIds);

    /// <summary>
    /// Gets immutable optional capabilities of this service generation.
    /// </summary>
    public static UiBackendCapabilities capabilities => UiExecutionContext.current.capabilities;

    /// <summary>
    /// Reads the context clock without advancing it.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <returns>
    /// The current elapsed seconds and pause state.
    /// </returns>
    public static UiContextTime GetTime(UiContextHandle context) => UiExecutionContext.current.GetTime(context);

    /// <summary>
    /// Controls automatic time advancement; input remains independently routable.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="paused">
    /// True to freeze automatic time; explicit steps remain available.
    /// </param>
    public static void SetTimePaused(
        UiContextHandle context,
        bool paused
    ) => UiExecutionContext.current.SetTimePaused(context, paused);

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
    public static void AdvanceTime(
        UiContextHandle context,
        double seconds
    ) => UiExecutionContext.current.AdvanceTime(context, seconds);

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
    public static bool TrySeekTime(
        UiContextHandle context,
        double seconds
    ) => UiExecutionContext.current.TrySeekTime(context, seconds);

    /// <summary>
    /// Refreshes document style and layout at the current context time without processing input.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    public static void RefreshLayout(UiContextHandle context) => UiExecutionContext.current.RefreshLayout(context);

    /// <summary>
    /// Processes the explicitly routed input without advancing time or updating the whole context.
    /// </summary>
    /// <param name="context">
    /// The live context on its owner thread.
    /// </param>
    /// <param name="input">
    /// The current immutable input sample.
    /// </param>
    public static void ProcessInput(
        UiContextHandle context,
        UiInputSnapshot input
    ) => UiExecutionContext.current.ProcessInput(context, input);

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
    public static UiControlValue GetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) => UiExecutionContext.current.GetValue(context, document, elementId);

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
    public static bool SetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiControlValue value
    ) => UiExecutionContext.current.SetValue(context, document, elementId, value);

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
    public static UiFocusResult FocusElement(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) => UiExecutionContext.current.FocusElement(context, document, elementId);

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
    public static UiFocusTarget? GetFocusedElement(UiContextHandle context) =>
        UiExecutionContext.current.GetFocusedElement(context);

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
    public static bool ClearFocus(UiContextHandle context) => UiExecutionContext.current.ClearFocus(context);

    /// <summary>
    /// Creates one independent UI context.
    /// </summary>
    /// <param name="options">
    /// The validated configuration that controls this operation.
    /// </param>
    /// <returns>
    /// The validated ui context handle that represents the completed operation.
    /// </returns>
    public static UiContextHandle CreateContext(UiContextOptions options) => UiExecutionContext.current.CreateContext(options);
    /// <summary>
    /// Destroys one UI context and all owned documents.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public static void DestroyContext(UiContextHandle context) => UiExecutionContext.current.DestroyContext(context);
    /// <summary>
    /// Changes one context's pixel dimensions and density.
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
    public static void SetViewport(
        UiContextHandle context,
        int width,
        int height,
        float density = 1f
    )
        => UiExecutionContext.current.SetViewport(context, width, height, density);
    /// <summary>
    /// Loads one explicitly tagged in-memory document.
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
    public static UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentSource source
    )
        => UiExecutionContext.current.LoadDocument(context, source);
    /// <summary>
    /// Loads one imported document compatible with the selected backend.
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
    public static UiDocumentHandle LoadDocument(
        UiContextHandle context,
        UiDocumentAsset document
    )
        => UiExecutionContext.current.LoadDocument(context, document);
    /// <summary>
    /// Shows one document.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by show document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void ShowDocument(
        UiContextHandle context,
        UiDocumentHandle document
    )
        => UiExecutionContext.current.ShowDocument(context, document);
    /// <summary>
    /// Hides one document.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by hide document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void HideDocument(
        UiContextHandle context,
        UiDocumentHandle document
    )
        => UiExecutionContext.current.HideDocument(context, document);
    /// <summary>
    /// Closes one document.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="document">
    /// The document consumed by close document; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void CloseDocument(
        UiContextHandle context,
        UiDocumentHandle document
    )
        => UiExecutionContext.current.CloseDocument(context, document);
    /// <summary>
    /// Replaces an element's children with plain Unicode text.
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
    public static bool SetText(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string text
    )
        => UiExecutionContext.current.SetText(context, document, elementId, text);
    /// <summary>
    /// Replaces an element's children with an explicitly tagged source-language fragment.
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
    /// <param name="fragment">
    /// The fragment consumed by set content; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool SetContent(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiDocumentFragment fragment
    )
        => UiExecutionContext.current.SetContent(context, document, elementId, fragment);
    /// <summary>
    /// Sets one element attribute.
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
    public static bool SetAttribute(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string name,
        string value
    )
        => UiExecutionContext.current.SetAttribute(context, document, elementId, name, value);
    /// <summary>
    /// Activates or deactivates one element class.
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
    public static bool SetClass(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        string className,
        bool active
    )
        => UiExecutionContext.current.SetClass(context, document, elementId, className, active);
    /// <summary>
    /// Registers one named RGBA8 texture source.
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
    public static void RegisterTexture(
        UiContextHandle context,
        string source,
        UiTextureData texture
    )
        => UiExecutionContext.current.RegisterTexture(context, source, texture);
    /// <summary>
    /// Processes current-frame input and advances one context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public static void Update(UiContextHandle context) => UiExecutionContext.current.Update(context);
    /// <summary>
    /// Advances one context with explicitly routed viewport-local input.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="input">
    /// The input consumed by update; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public static void Update(
        UiContextHandle context,
        UiInputSnapshot input
    ) => UiExecutionContext.current.Update(context, input);
    /// <summary>
    /// Builds an immutable frame of incremental resources and ordered draws.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated ui render frame that represents the completed operation.
    /// </returns>
    public static UiRenderFrame Render(UiContextHandle context) => UiExecutionContext.current.Render(context);
    /// <summary>
    /// Drains document events queued by preceding updates.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public static IReadOnlyList<UiEvent> DrainEvents(UiContextHandle context) => UiExecutionContext.current.DrainEvents(context);
}
