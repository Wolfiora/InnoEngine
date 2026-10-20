using System;
using Inno.Native.UI;

namespace Inno.Adapter.UI.RmlUi;

internal static unsafe class RmlUiNativeRuntime
{
    internal static Result CloseDocument(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.CloseDocument(
                runtime,
                context,
                document,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result CreateContext(
        InnoUIRmlUiAdapterRuntime* runtime,
        string name,
        int width,
        int height,
        float density,
        ref ulong context
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        using var nameText = new RmlUiUtf8String(name);
        fixed (ulong* contextPointer = &context)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CreateContext(
                    runtime,
                    (sbyte*)nameText.pointer,
                    width,
                    height,
                    density,
                    contextPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result DestroyContext(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.DestroyContext(
                runtime,
                context,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result HasElementAtPoint(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        int x,
        int y,
        ref byte hit
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (byte* hitPointer = &hit)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.HasElementAtPoint(
                    runtime,
                    context,
                    x,
                    y,
                    hitPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result HideDocument(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.HideDocument(
                runtime,
                context,
                document,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result LoadDocument(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        string markup,
        string sourceUrl,
        ref ulong document
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        using var markupText = new RmlUiUtf8String(markup);
        using var sourceUrlText = new RmlUiUtf8String(sourceUrl);
        fixed (ulong* documentPointer = &document)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.LoadDocument(
                    runtime,
                    context,
                    (sbyte*)markupText.pointer,
                    (sbyte*)sourceUrlText.pointer,
                    documentPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result LoadFont(
        InnoUIRmlUiAdapterRuntime* runtime,
        byte* data,
        ulong dataLength,
        byte* family,
        int style,
        int weight,
        byte fallback
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.LoadFont(
                runtime,
                data,
                dataLength,
                (sbyte*)family,
                style,
                weight,
                fallback,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ProcessKey(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        int key,
        byte down,
        int modifiers
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ProcessKey(
                runtime,
                context,
                key,
                down,
                modifiers,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ProcessMouseButton(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        int button,
        byte down,
        int modifiers
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ProcessMouseButton(
                runtime,
                context,
                button,
                down,
                modifiers,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ProcessMouseMove(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        int x,
        int y,
        int modifiers
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ProcessMouseMove(
                runtime,
                context,
                x,
                y,
                modifiers,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ProcessMouseWheel(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        float x,
        float y,
        int modifiers
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ProcessMouseWheel(
                runtime,
                context,
                x,
                y,
                modifiers,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ProcessText(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        byte* text
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ProcessText(
                runtime,
                context,
                (sbyte*)text,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result RegisterTexture(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        byte* source,
        int width,
        int height,
        byte* pixels,
        ulong pixelCapacity
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.RegisterTexture(
                runtime,
                context,
                (sbyte*)source,
                width,
                height,
                pixels,
                pixelCapacity,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result SetAttribute(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document,
        string elementId,
        string name,
        string value,
        ref byte changed
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        using var elementIdText = new RmlUiUtf8String(elementId);
        using var nameText = new RmlUiUtf8String(name);
        using var valueText = new RmlUiUtf8String(value);
        fixed (byte* changedPointer = &changed)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.SetAttribute(
                    runtime,
                    context,
                    document,
                    (sbyte*)elementIdText.pointer,
                    (sbyte*)nameText.pointer,
                    (sbyte*)valueText.pointer,
                    changedPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result SetClass(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document,
        string elementId,
        string className,
        byte active,
        ref byte changed
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        using var elementIdText = new RmlUiUtf8String(elementId);
        using var classNameText = new RmlUiUtf8String(className);
        fixed (byte* changedPointer = &changed)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.SetClass(
                    runtime,
                    context,
                    document,
                    (sbyte*)elementIdText.pointer,
                    (sbyte*)classNameText.pointer,
                    active,
                    changedPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result SetInnerMarkup(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document,
        string elementId,
        string markup,
        ref byte changed
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        using var elementIdText = new RmlUiUtf8String(elementId);
        using var markupText = new RmlUiUtf8String(markup);
        fixed (byte* changedPointer = &changed)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.SetInnerMarkup(
                    runtime,
                    context,
                    document,
                    (sbyte*)elementIdText.pointer,
                    (sbyte*)markupText.pointer,
                    changedPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result SetViewport(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        int width,
        int height,
        float density
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.SetViewport(
                runtime,
                context,
                width,
                height,
                density,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result ShowDocument(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong document
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ShowDocument(
                runtime,
                context,
                document,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result Update(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.Update(
                runtime,
                context,
                &result,
                &error),
            error);
        return result;
    }
}
