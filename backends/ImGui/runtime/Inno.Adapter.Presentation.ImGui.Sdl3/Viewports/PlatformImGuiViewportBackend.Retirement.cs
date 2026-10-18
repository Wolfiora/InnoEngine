using System;
using System.Threading.Tasks;
using Inno.Core.Execution;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Presentation.ImGui;

internal sealed unsafe partial class PlatformImGuiViewportBackend
{
    internal void ProcessRetirements()
    {
        for (int index = m_retiringViewports.Count - 1; index >= 0; index--)
        {
            ViewportWindowData data = m_retiringViewports[index];
            Task retirement = data.retirement!;
            if (!retirement.IsCompleted)
                continue;
            if (!retirement.IsCompletedSuccessfully)
                throw new RetirementPendingException("Rendering failed to release a detached SDL window.", retirement.Exception);
            DestroyRetiredWindow(data);
            m_retiringViewports.RemoveAt(index);
        }
        if (m_callbackFailure is Exception callbackFailure)
        {
            if (m_retiringViewports.Count != 0)
                throw new RetirementPendingException("An ImGui callback failed; its renderer still borrows a detached window.", callbackFailure);
            m_callbackFailure = null;
            throw new InvalidOperationException("An ImGui window callback failed and its resources have retired.", callbackFailure);
        }
    }

    private static void DestroyViewportWindow(ViewportWindowData data)
    {
        if (data.detached)
            return;
        data.detached = true;
        data.drawable = false;
        data.owner.m_retiringViewports.Add(data);
        try
        {
            data.owner.m_application.ReleaseWindow(data.registeredWindow);
            data.retirement = data.externalRenderer is not null && data.externalTarget is not null
                ? data.externalRenderer.RetireViewport(data.externalTarget)
                : Task.CompletedTask;
        }
        catch (Exception failure)
        {
            data.retirement = Task.FromException(failure);
        }
        finally
        {
            data.viewport = default;
            if (data.gcHandle.IsAllocated)
                data.gcHandle.Free();
        }
    }

    private static void DestroyRetiredWindow(ViewportWindowData data)
    {
        if (!data.fontTexture.IsNull)
        {
            SDL.DestroyTexture(data.fontTexture);
            data.fontTexture = SDLTexturePtr.Null;
        }
        if (!data.renderer.IsNull)
        {
            SDL.DestroyRenderer(data.renderer);
            data.renderer = SDLRenderer.Null;
        }
        if (!data.window.IsNull)
        {
            SDL.DestroyWindow(data.window);
            data.window = SDLWindow.Null;
        }
        data.externalRenderer = null;
        data.externalTarget = null;
        data.vertexScratch = [];
        data.indexScratch = [];
    }
}
