using System;
using Inno.Editor.Interactions;
using Inno.Scripting.Api;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Owns session-local controls, native adaptation and reversible edit gestures.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private EditorEditGesture? m_propertyGesture;
    private (string Owner, string Path) m_propertyEditKey;
    private bool m_propertyEditTouched;
    /// <summary>
    /// Gets the current continuous-edit merge boundary, or null for a discrete edit.
    /// </summary>
    [ScriptingApiIgnore]
    public string? editMergeKey => m_propertyGesture?.mergeKey;

    /// <summary>
    /// Starts a History boundary only when a changed control is actively producing continuous input.
    /// </summary>
    /// <param name="history">
    /// The authoritative domain History receiving the control's existing mutation records.
    /// </param>
    /// <param name="path">
    /// The stable owner-qualified property path.
    /// </param>
    /// <param name="name">
    /// The operation name shown by Undo and Redo.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// A previous gesture cannot complete or History rejects the transaction.
    /// </exception>
    [ScriptingApiIgnore]
    public void BeginPropertyEdit(
        IEditorHistory history,
        string path,
        string name
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (NativeImGui.IsAnyItemActive())
            BeginEdit(history, path, name);
    }

    /// <summary>
    /// Begins or touches an explicitly captured continuous edit, such as a viewport manipulation.
    /// </summary>
    /// <param name="history">
    /// The authoritative domain History.
    /// </param>
    /// <param name="path">
    /// The stable owner-qualified edit identity.
    /// </param>
    /// <param name="name">
    /// The user-facing Undo name.
    /// </param>
    [ScriptingApiIgnore]
    public void BeginEdit(
        IEditorHistory history,
        string path,
        string name
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(history);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var key = (m_session.owner, path);
        if (m_propertyGesture is not null && m_propertyEditKey != key)
            CommitEdits();
        if (m_propertyGesture is null)
        {
            m_propertyGesture = new EditorEditGesture(history, name);
            m_propertyEditKey = key;
        }

        m_propertyEditTouched = true;
    }

    /// <summary>
    /// Checks whether this owner still owns a particular edit, including after a close or generation boundary.
    /// </summary>
    /// <param name="path">
    /// The stable edit identity.
    /// </param>
    /// <returns>
    /// True only while the matching owner still has an active transaction.
    /// </returns>
    [ScriptingApiIgnore]
    public bool IsEditing(string path) => m_propertyGesture is not null && m_propertyEditKey == (m_session.owner, path);
    /// <summary>
    /// Observes the current property after drawing, including release and cancellation without a new value sample.
    /// </summary>
    /// <param name="path">
    /// The owner-qualified path passed to the property edit boundary.
    /// </param>
    [ScriptingApiIgnore]
    public void CompletePropertyEdit(string path)
    {
        m_session.EnsureDrawing();
        if (m_propertyGesture is null || m_propertyEditKey != (m_session.owner, path))
            return;
        m_propertyEditTouched = true;
        if (NativeImGui.IsKeyPressed((Inno.Native.ImGui.ImGuiKey)ImGuiKey.Escape, false))
            CancelEdits();
        else if (!NativeImGui.IsAnyItemActive())
            CommitEdits();
    }

    /// <summary>
    /// Completes the current control edit before a document command, reload, Play transition or shutdown.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A nested transaction prevents completion; ownership is retained.
    /// </exception>
    [ScriptingApiIgnore]
    public void CommitEdits()
    {
        if (m_propertyGesture is null)
            return;
        m_propertyGesture.Commit();
        m_propertyGesture.Dispose();
        m_propertyGesture = null;
    }

    /// <summary>
    /// Restores the control's initial domain values before discarding its transient presentation state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Rollback is unavailable; the gesture remains owned for recovery.
    /// </exception>
    [ScriptingApiIgnore]
    public void CancelEdits()
    {
        if (m_propertyGesture is null)
            return;
        EditorHistoryResult result = m_propertyGesture.Cancel();
        if (!result.succeeded)
            throw new InvalidOperationException($"The property gesture could not roll back: {result.message}");
        m_propertyGesture.Dispose();
        m_propertyGesture = null;
    }

    internal void BeginEditingFrame() => m_propertyEditTouched = false;
    internal void CompleteEditingFrame()
    {
        if (m_propertyGesture is not null && !m_propertyEditTouched)
            CancelEdits();
    }
}
