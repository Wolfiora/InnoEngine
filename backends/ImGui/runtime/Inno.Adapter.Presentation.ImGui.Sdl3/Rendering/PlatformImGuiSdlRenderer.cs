using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Inno.Adapter.Platform.Sdl3;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;

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
        if (m_renderer is null)
        {
            throw new InvalidOperationException(Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError()) ?? "SDL_CreateRenderer failed.");
        }

        _ = SDL.Raw.SetRenderDrawBlendMode(m_renderer, SDL.Constants.SDL_BLENDMODE_BLEND);
    }

    /// <summary>
    /// Gets whether the caller-visible condition represented by this property is satisfied.
    /// </summary>
    public bool supportsViewports => false;

    /// <inheritdoc/>
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
    public void ResizeViewport(PlatformImGuiViewportTarget target) {}

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
    ) {}

    /// <summary>
    /// Presents the completed frame for the supplied auxiliary viewport.
    /// </summary>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    public void PresentViewport(PlatformImGuiViewportTarget target) {}

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

    /// <inheritdoc/>
    public void DrainViewportRetirements() {}

    internal void Render(ImDrawData* drawData)
    {
        if (m_disposed || m_renderer is null || drawData is null || drawData->Valid == 0)
        {
            return;
        }

        ProcessTextureRequests(drawData);
        EnsureFontTexture();

        _ = SDL.Raw.SetRenderViewport(m_renderer, null);
        _ = SDL.Raw.SetRenderClipRect(m_renderer, null);
        _ = SDL.Raw.SetRenderDrawColor(m_renderer, 20, 20, 22, 255);
        _ = SDL.Raw.RenderClear(m_renderer);

        ImVec2C clipOff = drawData->DisplayPos;
        ImVec2C clipScale = drawData->FramebufferScale;

        for (int listIndex = 0; listIndex < drawData->CmdListsCount; listIndex++)
        {
            ImDrawList* drawList = drawData->CmdLists.Data[listIndex];
            if (drawList is null)
            {
                continue;
            }

            int vertexCount = drawList->VtxBuffer.Size;
            if (vertexCount <= 0)
            {
                continue;
            }

            EnsureVertexCapacity(vertexCount);
            ImDrawVert* srcVertices = drawList->VtxBuffer.Data;
            for (int i = 0; i < vertexCount; i++)
            {
                m_vertexScratch[i] = ToSdlVertex(srcVertices[i], clipOff, clipScale);
            }

            int cmdBufferCount = drawList->CmdBuffer.Size;
            for (int cmdIndex = 0; cmdIndex < cmdBufferCount; cmdIndex++)
            {
                ImDrawCmd drawCmd = drawList->CmdBuffer.Data[cmdIndex];
                if (drawCmd.UserCallback is not null)
                {
                    continue;
                }

                float clipRectX = (drawCmd.ClipRect.X - clipOff.X) * clipScale.X;
                float clipRectY = (drawCmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                float clipRectZ = (drawCmd.ClipRect.Z - clipOff.X) * clipScale.X;
                float clipRectW = (drawCmd.ClipRect.W - clipOff.Y) * clipScale.Y;

                if (clipRectZ <= clipRectX || clipRectW <= clipRectY)
                {
                    continue;
                }

                var clipRect = new SDLRect
                {
                    X = (int)clipRectX,
                    Y = (int)clipRectY,
                    W = (int)(clipRectZ - clipRectX),
                    H = (int)(clipRectW - clipRectY)
                };

                if (SDL.Raw.SetRenderClipRect(m_renderer, &clipRect) == 0)
                {
                    continue;
                }

                int elemCount = (int)drawCmd.ElemCount;
                if (elemCount <= 0)
                {
                    continue;
                }

                EnsureIndexCapacity(elemCount);

                ushort* srcIndices = drawList->IdxBuffer.Data;
                int idxOffset = (int)drawCmd.IdxOffset;
                int vtxOffset = (int)drawCmd.VtxOffset;
                for (int i = 0; i < elemCount; i++)
                {
                    m_indexScratch[i] = srcIndices[idxOffset + i] + vtxOffset;
                }

                ulong textureId = ImGuiNative.Raw.GetTexID(&drawCmd);
                SDLTexture* texture = TextureFromImGui(textureId);
                if (texture is null)
                {
                    texture = m_fontTexture;
                }

                fixed (SDLVertex* pVertices = m_vertexScratch)
                fixed (int* pIndices = m_indexScratch)
                {
                    _ = SDL.Raw.RenderGeometry(m_renderer, texture, pVertices, vertexCount, pIndices, elemCount);
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
        if (m_disposed || m_renderer is null)
        {
            return;
        }

        // A macOS live-resize expose callback can run before SDL's renderer event watcher has
        // consumed the matching pixel-size event. Re-applying the disabled presentation mode
        // makes SDL query the native drawable immediately instead of clipping to its old size.
        _ = SDL.Raw.SetRenderLogicalPresentation(m_renderer, 0, 0, SDLRendererLogicalPresentation.Disabled);
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

        if (m_fontTexture is not null)
        {
            SDL.Raw.DestroyTexture(m_fontTexture);
            m_fontTexture = null;
        }

        if (m_renderer is not null)
        {
            SDL.Raw.DestroyRenderer(m_renderer);
            m_renderer = null;
        }

        m_disposed = true;
    }

    private void ProcessTextureRequests(ImDrawData* drawData)
    {
        ImVectorImTextureDataPtr* textures = drawData->Textures;
        if (textures is null)
        {
            return;
        }

        for (int i = 0; i < textures->Size; i++)
        {
            ImTextureData* textureData = textures->Data[i];
            if (textureData is null)
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
        ImGuiIO* io = ImGuiNative.Raw.GetIO();
        ImFontAtlas* fonts = io->Fonts;
        if (fonts is null)
        {
            return;
        }

        fonts->RendererHasTextures = 1;
        ImTextureData* texData = fonts->TexData;
        if (texData is null)
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

        if (m_fontTexture is null)
        {
            m_fontTexture = TextureFromImGui(texData->TexId);
        }
    }

    private void CreateOrUpdateTexture(
        ImTextureData* textureData,
        bool createIfMissing
    ) {
        SDLTexture* texture = TextureFromImGui(textureData->TexId);
        if (texture is null && createIfMissing)
        {
            texture = CreateTexture(textureData->Width, textureData->Height);
            if (texture is null)
            {
                return;
            }

            ImGuiNative.Raw.SetTexID(textureData, (ulong)(nuint)texture);
        }

        if (texture is null || textureData->Pixels is null)
        {
            return;
        }

        int pitch = ImGuiNative.Raw.GetPitch(textureData);
        _ = SDL.Raw.UpdateTexture(texture, null, textureData->Pixels, pitch);
        ImGuiNative.Raw.SetStatus(textureData, ImTextureStatus.Ok);

        ImGuiIO* io = ImGuiNative.Raw.GetIO();
        if (textureData->TexId == ImGuiNative.Raw.GetTexID(&io->Fonts->TexRef))
        {
            m_fontTexture = texture;
        }
    }

    private void DestroyTexture(ImTextureData* textureData)
    {
        SDLTexture* texture = TextureFromImGui(textureData->TexId);
        if (texture is not null)
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

        uint props = SDL.CreateProperties();
        try
        {
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER, (long)SDLPixelFormat.Rgba32);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER, (long)SDLTextureAccess.Static);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER, width);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER, height);

            SDLTexture* texture = SDL.Raw.CreateTextureWithProperties(m_renderer, props);
            if (texture is null)
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
        SDLFColor color = ImGuiPackedColor.ToSdlFColor(vtx.Col);
        float x = (vtx.Pos.X - displayPos.X) * framebufferScale.X;
        float y = (vtx.Pos.Y - displayPos.Y) * framebufferScale.Y;

        return new SDLVertex
        {
            Position = new SDLFPoint { X = x, Y = y },
            Color = color,
            TexCoord = new SDLFPoint { X = vtx.Uv.X, Y = vtx.Uv.Y }
        };
    }

    private static SDLTexture* TextureFromImGui(ulong textureId) => (SDLTexture*)(nuint)textureId;

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
