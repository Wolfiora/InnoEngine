using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Native.UI;
using Inno.Text;
using Inno.UI;

namespace Inno.Adapter.UI.RmlUi;

/// <summary>
/// Implements retained-mode UI document processing through the pinned RmlUi native bridge.
/// </summary>
public sealed unsafe class RmlUiBackend : IUiBackend
{
    private static readonly UiBackendCapabilities S_CAPABILITIES = new(
        [RmlUiIdentifiers.documentLanguage],
        supportsFontCollectionFaces: false);
    private static readonly Regex S_FONT_FAMILY = new(
        @"(?im)(font-family\s*:\s*)([^;}]+)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private InnoUIRmlUiAdapterRuntime* m_runtime;
    private readonly Dictionary<string, string> m_fontAliases = new(StringComparer.OrdinalIgnoreCase);
    private bool m_disposed;

    /// <summary>
    /// Gets the implementation id text used by the current instance.
    /// </summary>
    public string implementationId => RmlUiIdentifiers.backend.value;

    /// <summary>
    /// Gets the immutable feature and limit set reported by the active graphics backend.
    /// </summary>
    public UiBackendCapabilities capabilities => S_CAPABILITIES;

    /// <summary>
    /// Creates and validates one isolated native UI engine.
    /// </summary>
    public RmlUiBackend()
    {
        m_runtime = RmlUiNativeErrors.CreateSession();
        if (m_runtime == null)
            throw CreateNativeException("create UI runtime", Result.UnknownError);
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
        ulong value = 0;
        ThrowIfFailed(RmlUiNativeRuntime.CreateContext(
            m_runtime, options.name, options.width, options.height, options.density, ref value), "create UI context");
        return new UiContextHandle(value);
    }

    /// <summary>
    /// Destroys the context after its in-flight references retire.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void DestroyContext(UiContextHandle context)
    {
        EnsureActive();
        ThrowIfFailed(RmlUiNativeRuntime.DestroyContext(m_runtime, Require(context)), "destroy UI context");
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
        float density
    ) {
        EnsureActive();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (!float.IsFinite(density) || density <= 0f)
            throw new ArgumentOutOfRangeException(nameof(density));
        ThrowIfFailed(RmlUiNativeRuntime.SetViewport(m_runtime, Require(context), width, height, density), "set UI viewport");
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
        EnsureActive();
        ArgumentNullException.ThrowIfNull(source);
        EnsureRml(source.language);
        ulong document = 0;
        ThrowIfFailed(RmlUiNativeRuntime.LoadDocument(
            m_runtime, Require(context), RewriteFontFamilies(source.text), source.sourceUri, ref document), "load UI document");
        return new UiDocumentHandle(document);
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
        ThrowIfFailed(RmlUiNativeRuntime.ShowDocument(m_runtime, Require(context), Require(document)), "show UI document");
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
        ThrowIfFailed(RmlUiNativeRuntime.HideDocument(m_runtime, Require(context), Require(document)), "hide UI document");
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
        ThrowIfFailed(RmlUiNativeRuntime.CloseDocument(m_runtime, Require(context), Require(document)), "close UI document");
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
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        ArgumentNullException.ThrowIfNull(text);
        byte changed = 0;
        ThrowIfFailed(RmlUiNativeRuntime.SetInnerMarkup(
            m_runtime, Require(context), Require(document), elementId, WebUtility.HtmlEncode(text), ref changed), "set UI text");
        return changed != 0;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        EnsureRml(content.language);
        byte changed = 0;
        ThrowIfFailed(RmlUiNativeRuntime.SetInnerMarkup(
            m_runtime,
            Require(context),
            Require(document),
            elementId,
            RewriteFontFamilies(content.text),
            ref changed), "set UI content");
        return changed != 0;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(value);
        byte changed = 0;
        ThrowIfFailed(RmlUiNativeRuntime.SetAttribute(
            m_runtime,
            Require(context),
            Require(document),
            elementId,
            name,
            value,
            ref changed), "set UI attribute");
        return changed != 0;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        ArgumentException.ThrowIfNullOrWhiteSpace(className);
        byte changed = 0;
        ThrowIfFailed(RmlUiNativeRuntime.SetClass(
            m_runtime,
            Require(context),
            Require(document),
            elementId,
            className,
            active ? (byte)1 : (byte)0,
            ref changed), "set UI class");
        return changed != 0;
    }

    /// <summary>
    /// Registers a font face for document text rendering.
    /// </summary>
    /// <param name="registration">
    /// The registration consumed by register font; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void RegisterFont(UiFontRegistration registration)
    {
        EnsureActive();
        if (registration.faceIndex != 0)
            throw new UiCapabilityUnavailableException(UiBackendCapability.FontCollectionFaceSelection);
        // All weights and styles of one declared family must share one native family.
        // Document imports give each family a private name before it reaches this backend.
        string familyHash = Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(registration.family.ToLowerInvariant()))).ToLowerInvariant();
        string physicalFamily = $"__inno_{familyHash[..24]}";
        ReadOnlySpan<byte> data = registration.data.Span;
        using var family = new RmlUiUtf8String(physicalFamily);
        fixed (byte* bytes = data)
        {
            ThrowIfFailed(RmlUiNativeRuntime.LoadFont(
                m_runtime,
                bytes,
                checked((uint)data.Length),
                family.pointer,
                (int)registration.style,
                registration.weight,
                0), "register UI font");
        }
        m_fontAliases[registration.family] = physicalFamily;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(source);
        ArgumentNullException.ThrowIfNull(texture);
        ReadOnlySpan<byte> pixels = texture.pixels.Span;
        using var nativeSource = new RmlUiUtf8String(source);
        fixed (byte* pixelPointer = pixels)
        {
            ThrowIfFailed(RmlUiNativeRuntime.RegisterTexture(
                m_runtime,
                Require(context),
                nativeSource.pointer,
                texture.width,
                texture.height,
                pixelPointer,
                checked((uint)pixels.Length)), "register UI texture");
        }
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
        byte hit = 0;
        ThrowIfFailed(RmlUiNativeRuntime.HasElementAtPoint(m_runtime, Require(context),
            (int)position.x, (int)position.y, ref hit), "query UI element hit");
        return hit != 0;
    }

    /// <summary>
    /// Recomputes owned state from the current validated inputs.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="input">
    /// The input consumed by update; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void Update(
        UiContextHandle context,
        UiInputSnapshot input
    ) {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(input);
        ulong handle = Require(context);
        int modifiers = (int)input.modifiers;
        ThrowIfFailed(RmlUiNativeRuntime.ProcessMouseMove(
            m_runtime, handle, (int)input.mousePosition.x, (int)input.mousePosition.y, modifiers), "process UI pointer position");
        if (input.scrollDelta.x != 0f || input.scrollDelta.y != 0f)
        {
            ThrowIfFailed(RmlUiNativeRuntime.ProcessMouseWheel(
                m_runtime, handle, input.scrollDelta.x, input.scrollDelta.y, modifiers), "process UI pointer wheel");
        }
        foreach (MouseButton button in input.buttonsPressed)
            ThrowIfFailed(RmlUiNativeRuntime.ProcessMouseButton(m_runtime, handle, (int)button, 1, modifiers), "process UI pointer press");
        foreach (MouseButton button in input.buttonsReleased)
            ThrowIfFailed(RmlUiNativeRuntime.ProcessMouseButton(m_runtime, handle, (int)button, 0, modifiers), "process UI pointer release");
        foreach (KeyCode key in input.keysPressed)
            ThrowIfFailed(RmlUiNativeRuntime.ProcessKey(m_runtime, handle, (int)key, 1, modifiers), "process UI key press");
        foreach (KeyCode key in input.keysReleased)
            ThrowIfFailed(RmlUiNativeRuntime.ProcessKey(m_runtime, handle, (int)key, 0, modifiers), "process UI key release");
        foreach (string text in input.textInput)
        {
            using var nativeText = new RmlUiUtf8String(text);
            ThrowIfFailed(RmlUiNativeRuntime.ProcessText(m_runtime, handle, nativeText.pointer), "process UI text input");
        }
        ThrowIfFailed(RmlUiNativeRuntime.Update(m_runtime, handle), "update UI context");
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
        ulong handle = Require(context);
        InnoUIRmlUiAdapterFrameInfo info = default;
        ThrowIfFailed(RmlUiNativeFrames.Render(m_runtime, handle, ref info), "render UI context");

        var builder = new UiRenderFrameBuilder();
        try
        {
            for (int meshIndex = 0; meshIndex < checked((int)info.MeshUpdateCount); ++meshIndex)
            {
                InnoUIRmlUiAdapterMeshInfo mesh = default;
                ThrowIfFailed(RmlUiNativeFrames.GetMeshInfo(m_runtime, handle, checked((ulong)meshIndex), ref mesh), "query UI mesh");
                InnoUIRmlUiAdapterVertex[] nativeVertices = new InnoUIRmlUiAdapterVertex[checked((int)mesh.VertexCount)];
                uint[] indices = new uint[checked((int)mesh.IndexCount)];
                CopyMeshVertices(handle, mesh.Id, nativeVertices);
                CopyMeshIndices(handle, mesh.Id, indices);
                UiVertex[] vertices = new UiVertex[nativeVertices.Length];
                for (int vertexIndex = 0; vertexIndex < vertices.Length; ++vertexIndex)
                {
                    InnoUIRmlUiAdapterVertex value = nativeVertices[vertexIndex];
                    vertices[vertexIndex] = new(value.X, value.Y, value.U, value.V, value.Color);
                }
                builder.AddMeshUpdate(new(mesh.Id), mesh.Revision, vertices, indices);
            }
            foreach (ulong mesh in CopyReleasedMeshes(handle, info.ReleasedMeshCount))
                builder.AddReleasedMesh(new(mesh));

            InnoUIRmlUiAdapterDrawCommand[] nativeCommands = new InnoUIRmlUiAdapterDrawCommand[checked((int)info.CommandCount)];
            CopyCommands(handle, nativeCommands);
            foreach (InnoUIRmlUiAdapterDrawCommand value in nativeCommands)
                builder.AddCommand(new(
                    new(value.MeshId),
                    new(value.TextureId),
                    value.ScissorEnabled != 0,
                    new(value.ScissorX, value.ScissorY, value.ScissorWidth, value.ScissorHeight)));

            for (int index = 0; index < checked((int)info.TextureUpdateCount); index++)
            {
                InnoUIRmlUiAdapterTextureInfo texture = default;
                ThrowIfFailed(RmlUiNativeFrames.GetTextureInfo(m_runtime, handle, checked((ulong)index), ref texture), "query UI texture");
                byte[] pixels = new byte[checked((int)texture.ByteLength)];
                if (pixels.Length > 0)
                    ThrowIfFailed(RmlUiNativeFrames.CopyTexturePixels(
                        m_runtime,
                        handle,
                        texture.Id,
                        pixels.AsSpan()), "copy UI texture");
                builder.AddTextureUpdate(new(texture.Id), texture.Revision, texture.Width, texture.Height, pixels);
            }
            foreach (ulong texture in CopyReleasedTextures(handle, info.ReleasedTextureCount))
                builder.AddReleasedTexture(new(texture));

            UiRenderFrame frame = builder.Build();
            ThrowIfFailed(RmlUiNativeFrames.FinishFrame(m_runtime, handle), "finish UI frame transfer");
            return frame;
        }
        catch
        {
            // Native deltas remain pending, so a failed transfer can be retried without losing resources.
            throw;
        }
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
        ulong handle = Require(context);
        ulong eventCount = 0;
        ThrowIfFailed(RmlUiNativeFrames.GetEventCount(m_runtime, handle, ref eventCount), "query UI events");
        UiEvent[] events = new UiEvent[checked((int)eventCount)];
        try
        {
            for (int index = 0; index < events.Length; index++)
            {
                InnoUIRmlUiAdapterEventInfo info = default;
                ThrowIfFailed(RmlUiNativeFrames.GetEventInfo(m_runtime, handle, checked((ulong)index), ref info), "query UI event");
                byte[] targetBytes = new byte[checked((int)info.TargetIdLength + 1)];
                ThrowIfFailed(RmlUiNativeFrames.CopyEventTargetId(
                    m_runtime,
                    handle,
                    checked((ulong)index),
                    targetBytes.AsSpan()), "copy UI event target");
                events[index] = new UiEvent(
                    (UiEventType)info.Type,
                    new UiDocumentHandle(info.Document),
                    Encoding.UTF8.GetString(targetBytes, 0, checked((int)info.TargetIdLength)));
            }
            return events;
        }
        finally
        {
            ThrowIfFailed(RmlUiNativeFrames.ClearEvents(m_runtime, handle), "clear UI events");
        }
    }

    /// <summary>
    /// Releases every native context and document owned by this backend.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        m_disposed = true;
        RmlUiNativeErrors.DestroySession(m_runtime);
        m_runtime = null;
    }

    private void CopyMeshVertices(
        ulong context,
        ulong mesh,
        InnoUIRmlUiAdapterVertex[] values
    ) {
        if (values.Length == 0)
            return;
        ThrowIfFailed(RmlUiNativeFrames.CopyMeshVertices(m_runtime, context, mesh, values), "copy UI mesh vertices");
    }

    private void CopyMeshIndices(
        ulong context,
        ulong mesh,
        uint[] values
    ) {
        if (values.Length == 0)
            return;
        ThrowIfFailed(RmlUiNativeFrames.CopyMeshIndices(
            m_runtime,
            context,
            mesh,
            values.AsSpan()), "copy UI mesh indices");
    }

    private void CopyCommands(
        ulong context,
        InnoUIRmlUiAdapterDrawCommand[] values
    ) {
        if (values.Length == 0)
            return;
        ThrowIfFailed(RmlUiNativeFrames.CopyCommands(m_runtime, context, values), "copy UI commands");
    }

    private ulong[] CopyReleasedMeshes(
        ulong context,
        ulong count
    ) {
        ulong[] values = new ulong[checked((int)count)];
        if (values.Length > 0)
            ThrowIfFailed(RmlUiNativeFrames.CopyReleasedMeshes(
                m_runtime,
                context,
                values.AsSpan()), "copy released UI meshes");
        return values;
    }

    private ulong[] CopyReleasedTextures(
        ulong context,
        ulong count
    ) {
        ulong[] values = new ulong[checked((int)count)];
        if (values.Length > 0)
            ThrowIfFailed(RmlUiNativeFrames.CopyReleasedTextures(
                m_runtime,
                context,
                values.AsSpan()), "copy released UI textures");
        return values;
    }

    private static void EnsureRml(UiDocumentLanguageId language)
    {
        if (language != RmlUiIdentifiers.documentLanguage)
            throw new NotSupportedException($"RmlUi adapter cannot decode UI language '{language}'.");
    }

    private string RewriteFontFamilies(string source)
        => S_FONT_FAMILY.Replace(source, match =>
        {
            string[] requested = match.Groups[2].Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var resolved = new List<string>(requested.Length);
            foreach (string item in requested)
            {
                string logical = item.Trim('\'', '"');
                resolved.Add(m_fontAliases.TryGetValue(logical, out string? alias) ? alias : item);
            }
            return match.Groups[1].Value + string.Join(", ", resolved);
        });

    private static ulong Require(UiContextHandle context)
        => context.isValid ? context.value : throw new ArgumentException("A valid UI context handle is required.", nameof(context));

    private static ulong Require(UiDocumentHandle document)
        => document.isValid ? document.value : throw new ArgumentException("A valid UI document handle is required.", nameof(document));

    private static void ThrowIfFailed(
        Result result,
        string operation
    ) {
        if (result != Result.Success)
            throw CreateNativeException(operation, result);
    }

    private static InvalidOperationException CreateNativeException(
        string operation,
        Result result
    ) {

        string message = result switch
            {
                Result.InvalidArgument => "invalid argument",
                Result.OutOfMemory => "native allocation failed",
                Result.InvalidHandle => "invalid or stale handle",
                Result.BackendError => "backend operation failed",
                Result.BufferTooSmall => "transfer buffer is too small",
                Result.NotFound => "requested resource was not found",
                _ => "unknown native error"
            };
        return new InvalidOperationException($"Failed to {operation}: {message}.");
    }

    private void EnsureActive() => ObjectDisposedException.ThrowIf(m_disposed, this);
}
