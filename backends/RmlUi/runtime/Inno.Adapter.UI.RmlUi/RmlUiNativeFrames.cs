using System;
using Inno.Native.UI;

namespace Inno.Adapter.UI.RmlUi;

internal static unsafe class RmlUiNativeFrames
{
    internal static Result ClearEvents(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.ClearEvents(
                runtime,
                context,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result CopyCommands(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        Span<InnoUIRmlUiAdapterDrawCommand> commands
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterDrawCommand* commandsPointer = commands)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyCommands(
                    runtime,
                    context,
                    (InnoUIRmlUiAdapterDrawCommandNative*)commandsPointer,
                    checked((ulong)commands.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyEventTargetId(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong index,
        Span<byte> targetId
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (byte* targetIdPointer = targetId)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyEventTargetId(
                    runtime,
                    context,
                    index,
                    (sbyte*)targetIdPointer,
                    checked((ulong)targetId.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyMeshIndices(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong mesh,
        Span<uint> indices
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (uint* indicesPointer = indices)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyMeshIndices(
                    runtime,
                    context,
                    mesh,
                    (uint*)indicesPointer,
                    checked((ulong)indices.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyMeshVertices(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong mesh,
        Span<InnoUIRmlUiAdapterVertex> vertices
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterVertex* verticesPointer = vertices)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyMeshVertices(
                    runtime,
                    context,
                    mesh,
                    (InnoUIRmlUiAdapterVertexNative*)verticesPointer,
                    checked((ulong)vertices.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyReleasedMeshes(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        Span<ulong> meshes
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (ulong* meshesPointer = meshes)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyReleasedMeshes(
                    runtime,
                    context,
                    (ulong*)meshesPointer,
                    checked((ulong)meshes.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyReleasedTextures(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        Span<ulong> textures
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (ulong* texturesPointer = textures)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyReleasedTextures(
                    runtime,
                    context,
                    (ulong*)texturesPointer,
                    checked((ulong)textures.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result CopyTexturePixels(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong texture,
        Span<byte> pixels
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (byte* pixelsPointer = pixels)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.CopyTexturePixels(
                    runtime,
                    context,
                    texture,
                    (byte*)pixelsPointer,
                    checked((ulong)pixels.Length),
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result FinishFrame(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        RmlUiNativeErrors.CheckBridge(
            UiNative.Raw.FinishFrame(
                runtime,
                context,
                &result,
                &error),
            error);
        return result;
    }

    internal static Result GetEventCount(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ref ulong count
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (ulong* countPointer = &count)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.GetEventCount(
                    runtime,
                    context,
                    countPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result GetEventInfo(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong index,
        ref InnoUIRmlUiAdapterEventInfo eventInfo
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterEventInfo* eventInfoPointer = &eventInfo)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.GetEventInfo(
                    runtime,
                    context,
                    index,
                    (InnoUIRmlUiAdapterEventInfoNative*)eventInfoPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result GetMeshInfo(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong index,
        ref InnoUIRmlUiAdapterMeshInfo mesh
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterMeshInfo* meshPointer = &mesh)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.GetMeshInfo(
                    runtime,
                    context,
                    index,
                    (InnoUIRmlUiAdapterMeshInfoNative*)meshPointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result GetTextureInfo(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ulong index,
        ref InnoUIRmlUiAdapterTextureInfo texture
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterTextureInfo* texturePointer = &texture)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.GetTextureInfo(
                    runtime,
                    context,
                    index,
                    (InnoUIRmlUiAdapterTextureInfoNative*)texturePointer,
                    &result,
                    &error),
                error);
        return result;
    }

    internal static Result Render(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context,
        ref InnoUIRmlUiAdapterFrameInfo frame
    ) {
        Result result = default;
        BgcsInnoRmluiError* error = null;
        fixed (InnoUIRmlUiAdapterFrameInfo* framePointer = &frame)
            RmlUiNativeErrors.CheckBridge(
                UiNative.Raw.Render(
                    runtime,
                    context,
                    (InnoUIRmlUiAdapterFrameInfoNative*)framePointer,
                    &result,
                    &error),
                error);
        return result;
    }
}
