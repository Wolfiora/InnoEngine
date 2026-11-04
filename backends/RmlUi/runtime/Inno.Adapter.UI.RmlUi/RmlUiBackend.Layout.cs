using System;
using System.Collections.Generic;

using Inno.Core.Mathematics;
using Inno.Native.UI;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Captures owned layout facts and converts native geometry to neutral values.
/// </summary>
public sealed unsafe partial class RmlUiBackend
{
    /// <inheritdoc />
    public UiLayoutSnapshot CaptureLayout(
        UiContextHandle context,
        UiDocumentHandle document,
        IReadOnlyList<string> elementIds
    ) {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(elementIds);
        if (elementIds.Count == 0)
            throw new ArgumentException("A layout capture requires at least one element ID.", nameof(elementIds));
        foreach (string elementId in elementIds)
            ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        ulong contextId = Require(context);
        ulong documentId = Require(document);
        ulong revision = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(UiNative.Raw.PrepareLayout(m_runtime, contextId, &revision, &result, &error), error);
        ThrowIfFailed(result, "prepare UI layout capture");
        var elements = new UiElementLayout[elementIds.Count];
        for (int index = 0; index < elementIds.Count; index++)
        {
            string elementId = elementIds[index];
            using var id = new RmlUiUtf8String(elementId);
            InnoUIRmlUiAdapterElementLayoutInfo info = default;
            error = null;
            RmlUiNativeErrors.CheckBridge(UiNative.Raw.ReadLayout(
                m_runtime, contextId, documentId, (sbyte*)id.pointer, revision,
                (InnoUIRmlUiAdapterElementLayoutInfoNative*)&info, &result, &error), error);
            ThrowIfFailed(result, "capture UI element layout");
            elements[index] = new UiElementLayout(
                elementId,
                (UiElementLayoutStatus)info.Status,
                new Rect(info.ContentX, info.ContentY, info.ContentWidth, info.ContentHeight),
                new Rect(info.BorderX, info.BorderY, info.BorderWidth, info.BorderHeight),
                new Rect(info.ClipX, info.ClipY, info.ClipWidth, info.ClipHeight),
                new Matrix(info.M11, info.M12, info.M13, info.M14, info.M21, info.M22, info.M23, info.M24, info.M31, info.M32, info.M33, info.M34, info.M41, info.M42, info.M43, info.M44));
        }
        return new UiLayoutSnapshot(context, document, revision, elements);
    }
}
