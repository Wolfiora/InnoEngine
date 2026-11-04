using System;
using System.Text;

using Inno.Core.Mathematics;

using Inno.Native.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Maps explicit document focus to the native context authority.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    /// <inheritdoc />
    public Rect? GetTextInputArea(UiContextHandle context)
    {
        EnsureActive();
        byte available = 0;
        float x = 0, y = 0, width = 0, height = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetTextInputArea(
            m_runtime, Require(context), &available, &x, &y, &width, &height, &result, &error), error);
        ThrowIfFailed(result, "query text input area");
        return available != 0 ? new Rect(x, y, width, height) : null;
    }

    /// <inheritdoc />
    public UiFocusResult FocusElement(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) {
        EnsureActive();
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        using var id = new RmlUiUtf8String(elementId);
        FocusOutcome outcome = default;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.FocusElement(
            m_runtime, Require(context), Require(document), (sbyte*)id.pointer, &outcome, &result, &error), error);
        ThrowIfFailed(result, "focus UI element");
        if (outcome is FocusOutcome.Focused or FocusOutcome.Unchanged)
            ResetGamepadNavigation(context);
        return outcome switch
        {
            FocusOutcome.Focused => UiFocusResult.Focused,
            FocusOutcome.Unchanged => UiFocusResult.Unchanged,
            FocusOutcome.NotFound => UiFocusResult.NotFound,
            FocusOutcome.NotFocusable => UiFocusResult.NotFocusable,
            _ => throw new InvalidOperationException("The UI backend returned an unknown focus outcome.")
        };
    }

    /// <inheritdoc />
    public UiFocusTarget? GetFocusedElement(UiContextHandle context)
    {
        EnsureActive();
        ulong document = 0;
        ulong length = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        ulong handle = Require(context);
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetFocusedElement(
            m_runtime, handle, &document, null, 0, &length, &result, &error), error);
        if (result != Result.BufferTooSmall)
            ThrowIfFailed(result, "query UI focus");
        if (document == 0)
            return null;
        if (length == 0)
            return new UiFocusTarget(new UiDocumentHandle(document), string.Empty);
        byte[] bytes = new byte[checked((int)length)];
        fixed (byte* buffer = bytes)
            RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetFocusedElement(
                m_runtime, handle, &document, (sbyte*)buffer, (ulong)bytes.Length, &length, &result, &error), error);
        ThrowIfFailed(result, "read UI focus");
        return new UiFocusTarget(new UiDocumentHandle(document), Encoding.UTF8.GetString(bytes, 0, checked((int)length)));
    }

    /// <inheritdoc />
    public bool ClearFocus(UiContextHandle context)
    {
        EnsureActive();
        byte cleared = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.ClearFocus(
            m_runtime, Require(context), &cleared, &result, &error), error);
        ThrowIfFailed(result, "clear UI focus");
        ResetGamepadNavigation(context);
        return cleared != 0;
    }
}
