using Inno.Core.Events;
using Inno.Editor.Interactions;
using EditorControls = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using System;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Owns editor controls and transient presentation state for one host, independently from other ImGui contexts.
/// </summary>
public sealed unsafe class EditorUiSession : IDisposable
{
    [ThreadStatic]
    private static EditorUiSession? s_current;
    private readonly int m_ownerThread = Environment.CurrentManagedThreadId;
    private EventHub? m_lifecycleHub;
    private Scope? m_scope;
    private OwnerScope? m_ownerScope;
    private nint m_nativeContext;
    private int m_styleInputFrame = -1;
    private bool m_disposed;
    private string m_owner = "root";
    /// <summary>
    /// Creates independent controls and style state; the presentation backend retains its native context ownership.
    /// </summary>
    public EditorUiSession()
    {
        widgets = new EditorControls(this);
        menus = new EditorMenuRenderer(this);
    }

    /// <summary>
    /// Gets the current synchronous drawing scope; this accessor never creates or owns a session.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// No UI drawing scope is active on the calling thread.
    /// </exception>
    public static EditorUiSession current => s_current ?? throw new InvalidOperationException("Editor drawing requires an active UI session scope.");
    /// <summary>
    /// Gets the controls whose transient state belongs exclusively to this session.
    /// </summary>
    public EditorControls widgets { get; }

    /// <summary>
    /// Gets the menu presenter whose search state belongs exclusively to this session.
    /// </summary>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public EditorMenuRenderer menus { get; }
    /// <summary>
    /// Gets whether this session has successfully retired its presentation state.
    /// </summary>
    public bool isDisposed => m_disposed;
    internal string owner => m_owner;

    internal bool shouldDeferStyleChanges
    {
        get
        {
            EnsureOwner();
            ObjectDisposedException.ThrowIf(m_disposed, this);
            if (m_nativeContext == 0)
                return false;
            ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
            return context is null || (nint)context != m_nativeContext || context->WithinFrameScope != 0;
        }
    }

    /// <summary>
    /// Activates this session for a synchronous callback while preserving the enclosing scope.
    /// </summary>
    /// <returns>
    /// An owner-thread scope that must be released in reverse activation order.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The session has retired.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller is not the owner thread.
    /// </exception>
    public IDisposable Enter()
    {
        EnsureOwner();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        var scope = new Scope(this, s_current, m_scope);
        m_scope = scope;
        s_current = this;
        return scope;
    }

    /// <summary>
    /// Releases presentation state after all drawing scopes and domain edit gestures have ended.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A drawing scope is active or the caller is not the owner thread.
    /// </exception>
    public void Dispose()
    {
        EnsureOwner();
        if (m_disposed)
            return;
        if (m_scope is not null)
            throw new InvalidOperationException("The UI session still owns an active drawing scope.");
        widgets.Retire();
        menus.Clear();
        m_lifecycleHub?.Dispose();
        m_lifecycleHub = null;
        m_disposed = true;
        GC.SuppressFinalize(this);
    }

    internal void Connect(EditorInteractions interactions)
    {
        EnsureOwner();
        if (m_lifecycleHub is not null)
            throw new InvalidOperationException("The UI session already belongs to an interaction runtime.");
        m_lifecycleHub = interactions.events.CreateHub(int.MaxValue);
        m_lifecycleHub.Listen<EditorOperationPreparingEvent>(_ => widgets.CommitEdits());
    }

    internal IDisposable EnterOwner(string owner)
    {
        EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        var scope = new OwnerScope(this, m_owner, m_ownerScope);
        m_owner = owner;
        m_ownerScope = scope;
        return scope;
    }

    internal void RetireOwner(string owner)
    {
        widgets.RetireOwner(owner);
        menus.RetireOwner(owner);
    }

    internal void EnsureDrawing() => _ = RequireContext();

    internal void PrepareStyleInput()
    {
        ImGuiNativeContext* context = RequireContext();
        if (context->WithinFrameScope != 0)
        {
            if (m_styleInputFrame == context->FrameCount)
                return;
            m_styleInputFrame = context->FrameCount;
        }
        else
            m_styleInputFrame = context->FrameCount + 1;
        widgets.style.CommitPending();
    }

    internal void RequireFramePreparation()
    {
        if (RequireContext()->WithinFrameScope != 0)
            throw new InvalidOperationException("Style preparation must run before native frame creation.");
    }

    internal ImGuiNativeContext* RequireDrawingFrame()
    {
        ImGuiNativeContext* context = RequireContext();
        if (context->WithinFrameScope == 0 || context->CurrentWindow is null)
            throw new InvalidOperationException("Drawing requires an active native frame and window.");
        return context;
    }

    /// <summary>
    /// Borrows the owned native context for the current synchronous presentation operation.
    /// The caller must not retain this pointer after that operation.
    /// </summary>
    internal ImGuiNativeContext* RequireContext()
    {
        EnsureOwner();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!ReferenceEquals(s_current, this))
            throw new InvalidOperationException("Controls must be drawn within their owning UI session scope.");
        nint context = (nint)NativeImGui.Raw.GetCurrentContext();
        if (context == 0)
            throw new InvalidOperationException("Editor controls require a live ImGui context.");
        if (m_nativeContext == 0)
            m_nativeContext = context;
        else if (m_nativeContext != context)
            throw new InvalidOperationException("The UI session belongs to a different ImGui context.");
        return (ImGuiNativeContext*)context;
    }

    private void EnsureOwner()
    {
        if (Environment.CurrentManagedThreadId != m_ownerThread)
            throw new InvalidOperationException("Editor UI sessions must be used and retired on their owner thread.");
    }

    private sealed class Scope(
        EditorUiSession owner,
        EditorUiSession? previous,
        Scope? previousScope
    ) : IDisposable
    {
        private bool m_disposed;
        /// <inheritdoc />
        public void Dispose()
        {
            owner.EnsureOwner();
            if (m_disposed)
                return;
            if (!ReferenceEquals(s_current, owner) || !ReferenceEquals(owner.m_scope, this))
                throw new InvalidOperationException("UI drawing scopes must retire in reverse activation order.");
            owner.m_scope = previousScope;
            s_current = previous;
            m_disposed = true;
        }
    }

    private sealed class OwnerScope(
        EditorUiSession session,
        string previous,
        OwnerScope? previousScope
    ) : IDisposable
    {
        private bool m_disposed;
        /// <inheritdoc />
        public void Dispose()
        {
            if (m_disposed)
                return;
            session.EnsureOwner();
            if (!ReferenceEquals(session.m_ownerScope, this))
                throw new InvalidOperationException("UI owner scopes must retire in reverse activation order.");
            session.m_owner = previous;
            session.m_ownerScope = previousScope;
            m_disposed = true;
        }
    }
}
