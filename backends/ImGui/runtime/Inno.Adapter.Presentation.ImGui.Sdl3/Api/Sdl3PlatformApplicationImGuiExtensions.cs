using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Extension APIs that add ImGui integration on top of <see cref="Sdl3PlatformApplication"/>.
/// </summary>
public static class Sdl3PlatformApplicationImGuiExtensions
{
    private sealed class ImGuiState : ISdl3ApplicationExtension
    {
        private readonly Sdl3PlatformApplication m_application;
        private IDisposable? m_registration;

        internal ImGuiState(Sdl3PlatformApplication application)
        {
            m_application = application;
        }

        internal Dictionary<uint, PlatformImGuiContext> contexts { get; } = [];

        internal void Register()
        {
            m_registration = m_application.RegisterExtension(this);
        }

        void ISdl3ApplicationExtension.ProcessNativeEvent(
            Sdl3PlatformApplication application,
            scoped ReadOnlySpan<byte> nativeEventData
        ) {
            if (nativeEventData.Length < Marshal.SizeOf<SDLEvent>())
            {
                return;
            }

            SDLEvent sdlEvent = MemoryMarshal.Read<SDLEvent>(nativeEventData);
            foreach (PlatformImGuiContext context in contexts.Values)
                context.ProcessEvent(ref sdlEvent);
        }

        void ISdl3ApplicationExtension.PrepareLiveResizeWindow(
            Sdl3PlatformApplication application,
            uint windowId
        ) {
            foreach (PlatformImGuiContext context in contexts.Values)
                context.PrepareLiveResizeWindow(windowId);
        }

        void ISdl3ApplicationExtension.OnApplicationDisposing(Sdl3PlatformApplication application)
        {
            foreach (PlatformImGuiContext context in contexts.Values)
                context.Dispose();
            contexts.Clear();
            s_states.Remove(application);
            m_registration?.Dispose();
            m_registration = null;
        }
    }

    private static readonly ConditionalWeakTable<Sdl3PlatformApplication, ImGuiState> s_states = new();

    /// <summary>
    /// Creates one ImGui context and transfers the supplied renderer ownership only after successful creation.
    /// </summary>
    /// <param name="application">
    /// Target platform application instance.
    /// </param>
    /// <param name="window">
    /// Target platform window.
    /// </param>
    /// <param name="interaction">
    /// Explicit immutable keyboard and wheel conventions supplied by the product.
    /// </param>
    /// <param name="contextFlags">
    /// ImGui context feature flags.
    /// </param>
    /// <param name="renderer">
    /// Optional presentation backend; ownership transfers on success and remains with the caller on failure.
    /// </param>
    /// <returns>
    /// The newly created context, which owns its renderer until complete retirement and disposal.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// An application, window, or interaction configuration is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The window does not belong to the supplied application.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The window is closed or already owns an ImGui context.
    /// </exception>
    public static PlatformImGuiContext CreateImGuiContext(
        this Sdl3PlatformApplication application,
        Sdl3PlatformWindow window,
        ImGuiContextFlags contextFlags,
        ImGuiInteractionOptions interaction,
        IPlatformImGuiRenderer? renderer = null
    ) {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(window);
        ArgumentNullException.ThrowIfNull(interaction);

        if (window.isClosed)
            throw new InvalidOperationException("Cannot create an ImGui context for a closed window.");
        bool registered = false;
        foreach (Sdl3PlatformWindow candidate in application.GetWindows())
            registered |= ReferenceEquals(candidate, window);
        if (!registered)
            throw new ArgumentException("The window must belong to the supplied SDL application.", nameof(window));

        ImGuiState state = s_states.GetValue(application, CreateState);
        if (state.contexts.ContainsKey(window.windowId))
            throw new InvalidOperationException("This window already owns an ImGui context.");

        var context = new PlatformImGuiContext(application, window, contextFlags, interaction, renderer);
        state.contexts[window.windowId] = context;
        return context;
    }

    /// <summary>
    /// Retires and disposes the window's context, retaining its application registration if retirement fails.
    /// </summary>
    /// <param name="application">
    /// Target platform application instance.
    /// </param>
    /// <param name="window">
    /// Target platform window.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The application or window is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Retirement cannot complete at the current owner-thread safe point; the context remains owned.
    /// </exception>
    public static void DestroyImGuiContext(
        this Sdl3PlatformApplication application,
        Sdl3PlatformWindow window
    ) {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(window);

        if (!s_states.TryGetValue(application, out ImGuiState? state))
            return;
        if (!state.contexts.TryGetValue(window.windowId, out PlatformImGuiContext? context))
            return;
        context.Dispose();
        state.contexts.Remove(window.windowId);
    }

    private static ImGuiState CreateState(Sdl3PlatformApplication application)
    {
        var state = new ImGuiState(application);
        state.Register();
        return state;
    }
}
