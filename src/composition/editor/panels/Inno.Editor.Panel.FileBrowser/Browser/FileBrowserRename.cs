using Inno.Core.Identity;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using System;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.ImGui.ImGuiWidget;

namespace Inno.Editor.Panel.FileBrowser;
internal enum FileBrowserPresentation
{
    Tree,
    List,
    Grid
}

internal sealed class FileBrowserRename(AssetBrowserModule assets)
{
    private RuntimeIdentity? m_activeTarget;
    private FileBrowserPresentation m_lastPresentation = FileBrowserPresentation.List;
    private FileBrowserPresentation m_activePresentation;
    internal RuntimeIdentity? activeTarget => m_activeTarget;
    internal void Update(EditorContext context)
    {
        AssetFileEntry? target = assets.edits.interactions.selection.selectedTarget as AssetFileEntry;
        bool isActive = target is not null && assets.edits.interactions.For(FileBrowserInteractionIds.C_AREA, target).IsActive(FileBrowserInteractionIds.C_RENAME);
        if (isActive && target?.identity.runtimeIdentity == m_activeTarget)
            return;
        m_activeTarget = isActive ? target!.identity.runtimeIdentity : null;
        m_activePresentation = m_lastPresentation;
    }

    internal void MarkInteraction(FileBrowserPresentation presentation)
    {
        m_lastPresentation = presentation;
    }

    internal bool IsEditing(
        EditorContext context,
        string relativePath,
        FileBrowserPresentation presentation
    ) {
        Update(context);
        return ResolveTarget() is AssetFileEntry target
            && m_activePresentation == presentation
            && string.Equals(
            target.assetPath.ToString(),
            relativePath,
            StringComparison.Ordinal
        );
    }

    internal void Draw(
        EditorContext context,
        string id,
        string relativePath,
        FileBrowserPresentation presentation,
        float width,
        float rowHeight
    ) {
        if (!IsEditing(context, relativePath, presentation) || m_activeTarget is null)
            return;
        _ = assets.edits.interactions.For(
            FileBrowserInteractionIds.C_AREA,
            ResolveTarget()
        ).Present(
            FileBrowserInteractionIds.C_RENAME,
            new InlineRenamePresentation(id, MathF.Max(1f, width), MathF.Max(1f, rowHeight))
        );
    }

    private AssetFileEntry? ResolveTarget() => m_activeTarget is RuntimeIdentity identity
        && assets.edits.interactions.TryResolveIdentity(
        identity,
        out IdentityObject? target
    ) ? target as AssetFileEntry : null;
}
