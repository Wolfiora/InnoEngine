using System;
using System.Text;

using Inno.Native.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Maps typed control values and value events across the backend boundary.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    /// <inheritdoc />
    public UiControlValue GetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId
    ) {
        EnsureActive();
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        using var id = new RmlUiUtf8String(elementId);
        InnoUIRmlUiAdapterControlValueInfo info = default;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        ulong contextId = Require(context);
        ulong documentId = Require(document);
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetValue(
            m_runtime, contextId, documentId, (sbyte*)id.pointer, (InnoUIRmlUiAdapterControlValueInfoNative*)&info, null, 0, &result, &error), error);
        if (result != Result.BufferTooSmall)
            ThrowIfFailed(result, "query UI control value");
        string text = string.Empty;
        if (info.TextLength != 0)
        {
            byte[] bytes = new byte[checked((int)info.TextLength)];
            fixed (byte* buffer = bytes)
                RmlUiNativeErrors.CheckBridge(UiNative.Raw.GetValue(
                    m_runtime, contextId, documentId, (sbyte*)id.pointer, (InnoUIRmlUiAdapterControlValueInfoNative*)&info,
                    (sbyte*)buffer, (ulong)bytes.Length, &result, &error), error);
            ThrowIfFailed(result, "read UI control value");
            text = Encoding.UTF8.GetString(bytes, 0, checked((int)info.TextLength));
        }
        return DecodeValue(info, text);
    }

    /// <inheritdoc />
    public bool SetValue(
        UiContextHandle context,
        UiDocumentHandle document,
        string elementId,
        UiControlValue value
    ) {
        EnsureActive();
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        if (value.kind == UiControlValueKind.None)
            throw new ArgumentException("A control write requires a typed value.", nameof(value));
        using var id = new RmlUiUtf8String(elementId);
        using var text = new RmlUiUtf8String(value.text ?? string.Empty);
        byte accepted = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.SetValue(
            m_runtime, Require(context), Require(document), (sbyte*)id.pointer,
            (ControlValueKind)value.kind, (sbyte*)text.pointer, value.number,
            value.boolean ? (byte)1 : (byte)0, &accepted, &result, &error), error);
        if (result == Result.InvalidArgument)
            throw new ArgumentException("The value is incompatible with the requested control.", nameof(value));
        ThrowIfFailed(result, "write UI control value");
        return accepted != 0;
    }

    private UiControlValue ReadEventValue(
        ulong context,
        ulong index,
        InnoUIRmlUiAdapterControlValueInfo info
    ) {
        string text = string.Empty;
        if (info.TextLength != 0)
        {
            byte[] bytes = new byte[checked((int)info.TextLength)];
            Result result = default;
            BgcsInnoRmluiError* error = null;
            fixed (byte* buffer = bytes)
                RmlUiNativeErrors.CheckBridge(UiNative.Raw.CopyEventValue(
                    m_runtime, context, index, (sbyte*)buffer, (ulong)bytes.Length, &result, &error), error);
            ThrowIfFailed(result, "read UI event value");
            text = Encoding.UTF8.GetString(bytes);
        }
        return DecodeValue(info, text);
    }

    private static UiControlValue DecodeValue(
        InnoUIRmlUiAdapterControlValueInfo info,
        string text
    ) => info.Kind switch
    {
        ControlValueKind.None => default,
        ControlValueKind.Text => UiControlValue.Text(text),
        ControlValueKind.Boolean => UiControlValue.Boolean(info.Checked != 0),
        ControlValueKind.Number => UiControlValue.Number(info.Number),
        ControlValueKind.Selection => UiControlValue.Selection(text),
        _ => throw new InvalidOperationException("The UI backend returned an unknown control value kind.")
    };
}
