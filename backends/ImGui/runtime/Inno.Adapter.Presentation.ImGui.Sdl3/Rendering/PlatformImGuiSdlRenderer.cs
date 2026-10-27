using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using System.Numerics;

using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Native.ImGui;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal sealed unsafe class PlatformImGuiSdlRenderer : IPlatformImGuiRenderer
{
    private SDLRenderer* m_renderer;
    private SDLTexture* m_fontTexture;
    private SDLVertex[] m_vertexScratch = [];
    private int[] m_indexScratch = [];
    private bool m_disposed;

    internal PlatformImGuiSdlRenderer(Sdl3PlatformWindow window)
    {
        m_renderer = SDL.Raw.CreateRenderer(window.GetSdlWindow(), null);
        if (m_renderer == null)
        {
            throw new InvalidOperationException(Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError()) ?? "SDL_CreateRenderer failed.");
        }

        _ = SDL.Raw.SetRenderDrawBlendMode(m_renderer, SDL.Constants.SDL_BLENDMODE_BLEND);
    }

    /// <summary>
    /// Gets whether the caller-visible condition represented by this property is satisfied.
    /// </summary>
    public bool supportsViewports => false;

    /// <inheritdoc />
    public bool supportsVertexOffsets => true;

    /// <summary>
    /// Records ImGui draw data for the main application viewport.
    /// </summary>
    /// <param name="drawData">
    /// The draw data consumed by render main; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void RenderMain(IntPtr drawData) => Render((ImDrawData*)drawData);

    /// <summary>
    /// Synchronizes the main render output with the current drawable dimensions.
    /// </summary>
    /// <param name="pixelWidth">
    /// The pixel width consumed by synchronize main output; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="pixelHeight">
    /// The pixel height consumed by synchronize main output; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void SynchronizeMainOutput(
        int pixelWidth,
        int pixelHeight
    ) {
        _ = pixelWidth;
        _ = pixelHeight;
        SynchronizeOutputSize();
    }

    /// <summary>
    /// Creates and validates a caller-owned viewport value.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    public void CreateViewport(PlatformImGuiViewportTarget target)
        => throw new NotSupportedException("The SDL main renderer does not own detached viewport rendering.");

    /// <summary>
    /// Resizes an auxiliary viewport surface to its current platform dimensions.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    public void ResizeViewport(PlatformImGuiViewportTarget target) { }

    /// <summary>
    /// Records ImGui draw data for the supplied auxiliary viewport.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <param name="drawData">
    /// The draw data consumed by render viewport; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void RenderViewport(
        PlatformImGuiViewportTarget target,
        IntPtr drawData
    ) { }

    /// <summary>
    /// Presents the completed frame for the supplied auxiliary viewport.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    public void PresentViewport(PlatformImGuiViewportTarget target) { }

    /// <summary>
    /// Destroys the auxiliary viewport and releases its rendering resources.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// An already completed task because this renderer owns no additional native windows.
    /// </returns>
    public Task RetireViewport(PlatformImGuiViewportTarget target) => Task.CompletedTask;

    /// <inheritdoc />
    public void DrainViewportRetirements() { }

    internal void Render(ImDrawData* drawData)
    {
        if (m_disposed || m_renderer == null || drawData == null || drawData->Valid == 0)
        {
            return;
        }

        ProcessTextureRequests(drawData);
        EnsureFontTexture();

        _ = SDL.Raw.SetRenderViewport(m_renderer, null);
        _ = SDL.Raw.SetRenderClipRect(m_renderer, null);
        _ = SDL.Raw.SetRenderDrawColor(m_renderer, 20, 20, 22, 255);
        _ = SDL.Raw.RenderClear(m_renderer);

        var clipOff = drawData->DisplayPos;
        var clipScale = drawData->FramebufferScale;

        for (var listIndex = 0; listIndex < drawData->CmdListsCount; listIndex++)
        {
            var drawList = drawData->CmdLists.Data[listIndex];
            if (drawList == null)
            {
                continue;
            }

            var vertexCount = drawList->VtxBuffer.Size;
            if (vertexCount <= 0)
            {
                continue;
            }

            EnsureVertexCapacity(vertexCount);
            var srcVertices = drawList->VtxBuffer.Data;
            for (var i = 0; i < vertexCount; i++)
            {
                m_vertexScratch[i] = ToSdlVertex(srcVertices[i], clipOff, clipScale);
            }

            var cmdBufferCount = drawList->CmdBuffer.Size;
            for (var cmdIndex = 0; cmdIndex < cmdBufferCount; cmdIndex++)
            {
                var drawCmd = drawList->CmdBuffer.Data[cmdIndex];
                if (drawCmd.UserCallback != null)
                {
                    continue;
                }

                var clipRectX = (drawCmd.ClipRect.X - clipOff.X) * clipScale.X;
                var clipRectY = (drawCmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                var clipRectZ = (drawCmd.ClipRect.Z - clipOff.X) * clipScale.X;
                var clipRectW = (drawCmd.ClipRect.W - clipOff.Y) * clipScale.Y;

                if (clipRectZ <= clipRectX || clipRectW <= clipRectY)
                {
                    continue;
                }

                var clipRect = new SDLRect
                {
                    X = (int)clipRectX, Y = (int)clipRectY,
                    W = (int)(clipRectZ - clipRectX), H = (int)(clipRectW - clipRectY)
                };

                if (SDL.Raw.SetRenderClipRect(m_renderer, &clipRect) == 0)
                {
                    continue;
                }

                var elemCount = (int)drawCmd.ElemCount;
                if (elemCount <= 0)
                {
                    continue;
                }

                EnsureIndexCapacity(elemCount);

                var srcIndices = drawList->IdxBuffer.Data;
                var idxOffset = (int)drawCmd.IdxOffset;
                var vtxOffset = (int)drawCmd.VtxOffset;
                for (var i = 0; i < elemCount; i++)
                {
                    m_indexScratch[i] = srcIndices[idxOffset + i] + vtxOffset;
                }

                var textureId = ImGuiNative.Raw.GetTexID(&drawCmd);
                var texture = TextureFromImGui(textureId);
                if (texture == null)
                {
                    texture = m_fontTexture;
                }

                fixed (SDLVertex* pVertices = m_vertexScratch)
                fixed (int* pIndices = m_indexScratch)
                {
                    _ = SDL.Raw.RenderGeometry(
                        m_renderer,
                        texture,
                        pVertices,
                        vertexCount,
                        pIndices,
                        elemCount);
                }
            }
        }

        _ = SDL.Raw.SetRenderClipRect(m_renderer, null);
        _ = SDL.Raw.RenderPresent(m_renderer);
    }

    /// <summary>
    /// Synchronizes the SDL renderer with the current native drawable size.
    /// </summary>
    internal void SynchronizeOutputSize()
    {
        if (m_disposed || m_renderer == null)
        {
            return;
        }

        // A macOS live-resize expose callback can run before SDL's renderer event watcher has
        // consumed the matching pixel-size event. Re-applying the disabled presentation mode
        // makes SDL query the native drawable immediately instead of clipping to its old size.
        _ = SDL.Raw.SetRenderLogicalPresentation(
            m_renderer,
            0,
            0,
            SDLRendererLogicalPresentation.Disabled);
        _ = SDL.Raw.SetRenderViewport(m_renderer, null);
        _ = SDL.Raw.SetRenderClipRect(m_renderer, null);
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }

        if (m_fontTexture != null)
        {
            SDL.Raw.DestroyTexture(m_fontTexture);
            m_fontTexture = null;
        }

        if (m_renderer != null)
        {
            SDL.Raw.DestroyRenderer(m_renderer);
            m_renderer = null;
        }

        m_disposed = true;
    }

    private void ProcessTextureRequests(ImDrawData* drawData)
    {
        var textures = drawData->Textures;
        if (textures == null)
        {
            return;
        }

        for (var i = 0; i < textures->Size; i++)
        {
            var textureData = textures->Data[i];
            if (textureData == null)
            {
                continue;
            }

            switch (textureData->Status)
            {
                case ImTextureStatus.WantCreate:
                    CreateOrUpdateTexture(textureData, createIfMissing: true);
                    break;
                case ImTextureStatus.WantUpdates:
                    CreateOrUpdateTexture(textureData, createIfMissing: true);
                    break;
                case ImTextureStatus.WantDestroy:
                    DestroyTexture(textureData);
                    break;
            }
        }
    }

    private void EnsureFontTexture()
    {
        var io = ImGuiNative.Raw.GetIO();
        var fonts = io->Fonts;
        if (fonts == null)
        {
            return;
        }

        fonts->RendererHasTextures = 1;
        var texData = fonts->TexData;
        if (texData == null)
        {
            return;
        }

        switch (texData->Status)
        {
            case ImTextureStatus.WantCreate:
            case ImTextureStatus.WantUpdates:
                CreateOrUpdateTexture(texData, createIfMissing: true);
                break;
            case ImTextureStatus.WantDestroy:
                DestroyTexture(texData);
                break;
        }

        if (m_fontTexture == null)
        {
            m_fontTexture = TextureFromImGui(texData->TexId);
        }
    }

    private void CreateOrUpdateTexture(
        ImTextureData* textureData,
        bool createIfMissing
    ) {
        var texture = TextureFromImGui(textureData->TexId);
        if (texture == null && createIfMissing)
        {
            texture = CreateTexture(textureData->Width, textureData->Height);
            if (texture == null)
            {
                return;
            }

            ImGuiNative.Raw.SetTexID(textureData, (ulong)(nuint)texture);
        }

        if (texture == null || textureData->Pixels == null)
        {
            return;
        }

        var pitch = ImGuiNative.Raw.GetPitch(textureData);
        _ = SDL.Raw.UpdateTexture(texture, null, textureData->Pixels, pitch);
        ImGuiNative.Raw.SetStatus(textureData, ImTextureStatus.Ok);

        var io = ImGuiNative.Raw.GetIO();
        if (textureData->TexId == ImGuiNative.Raw.GetTexID(&io->Fonts->TexRef))
        {
            m_fontTexture = texture;
        }
    }

    private void DestroyTexture(ImTextureData* textureData)
    {
        var texture = TextureFromImGui(textureData->TexId);
        if (texture != null)
        {
            SDL.Raw.DestroyTexture(texture);
        }

        if (texture == m_fontTexture)
        {
            m_fontTexture = null;
        }

        ImGuiNative.Raw.SetTexID(textureData, 0);
        ImGuiNative.Raw.SetStatus(textureData, ImTextureStatus.Destroyed);
    }

    private SDLTexture* CreateTexture(
        int width,
        int height
    ) {
        if (width <= 0 || height <= 0)
        {
            return null;
        }

        var props = SDL.CreateProperties();
        try
        {
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER, (long)SDLPixelFormat.Rgba32);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER, (long)SDLTextureAccess.Static);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER, width);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER, height);

            var texture = SDL.Raw.CreateTextureWithProperties(m_renderer, props);
            if (texture == null)
            {
                return null;
            }

            _ = SDL.Raw.SetTextureBlendMode(texture, SDL.Constants.SDL_BLENDMODE_BLEND);
            _ = SDL.Raw.SetTextureScaleMode(texture, SDLScaleMode.Linear);
            return texture;
        }
        finally
        {
            SDL.DestroyProperties(props);
        }
    }

    private static SDLVertex ToSdlVertex(
        ImDrawVert vtx,
        ImVec2C displayPos,
        ImVec2C framebufferScale
    ) {
        var color = ImGuiPackedColor.ToSdlFColor(vtx.Col);
        var x = (vtx.Pos.X - displayPos.X) * framebufferScale.X;
        var y = (vtx.Pos.Y - displayPos.Y) * framebufferScale.Y;

        return new SDLVertex
        {
            Position = new SDLFPoint { X = x, Y = y },
            Color = color,
            TexCoord = new SDLFPoint { X = vtx.Uv.X, Y = vtx.Uv.Y }
        };
    }

    private static SDLTexture* TextureFromImGui(ulong textureId)
    {
        return (SDLTexture*)(nuint)textureId;
    }

    private void EnsureVertexCapacity(int requiredCount)
    {
        if (m_vertexScratch.Length < requiredCount)
        {
            m_vertexScratch = new SDLVertex[Math.Max(requiredCount, m_vertexScratch.Length * 2 + 64)];
        }
    }

    private void EnsureIndexCapacity(int requiredCount)
    {
        if (m_indexScratch.Length < requiredCount)
        {
            m_indexScratch = new int[Math.Max(requiredCount, m_indexScratch.Length * 2 + 256)];
        }
    }
}
