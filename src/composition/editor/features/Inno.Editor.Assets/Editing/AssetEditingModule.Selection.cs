using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Scripting.Api;

namespace Inno.Editor.Assets;
/// <summary>
/// Applies validated selection operations through the authoritative asset and History owners.
/// </summary>
public sealed partial class AssetEditingModule
{
    /// <summary>
    /// Checks the entire source selection before a batch mutation, including extension validation and read-only ownership.
    /// </summary>
    /// <param name="editor">
    /// The immediate Editor context.
    /// </param>
    /// <param name="targets">
    /// Fresh source entries in selection order.
    /// </param>
    /// <returns>
    /// The first rejection, or a valid result when every source can be deleted.
    /// </returns>
    [ScriptingApiIgnore]
    public AssetOperationValidation ValidateDeleteSelection(
        EditorContext editor,
        IReadOnlyList<AssetFileEntry> targets
    ) {
        ArgumentNullException.ThrowIfNull(editor);
        ArgumentNullException.ThrowIfNull(targets);
        if (targets.Count == 0)
            return AssetOperationValidation.Invalid("The selection contains unavailable entries.");
        foreach (AssetFileEntry target in targets)
        {
            if (target.isReadOnly
                || target.identity.runtimeIdentity is null
                || !TryCreateContext(
                editor,
                target.assetPath.ToString(),
                out AssetEditorContext? context
            )
                || context is null)
                return AssetOperationValidation.Invalid("Every selected source must be available and writable.");
            AssetOperationValidation validation = m_editors.Resolve(context.assetType).ValidateDelete(context);
            if (!validation.isValid)
                return validation;
        }

        return AssetOperationValidation.valid;
    }

    /// <summary>
    /// Deletes minimal selected source roots in one History transaction, restoring earlier deletions if a later one fails.
    /// </summary>
    /// <param name="editor">
    /// The immediate Editor context.
    /// </param>
    /// <param name="targets">
    /// Fresh selected source entries; nested entries are covered by their selected directory.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Validation or a mutation fails; rollback failures remain explicit.
    /// </exception>
    [ScriptingApiIgnore]
    public void DeleteSelection(
        EditorContext editor,
        IReadOnlyList<AssetFileEntry> targets
    ) {
        AssetOperationValidation validation = ValidateDeleteSelection(editor, targets);
        if (!validation.isValid)
            throw new InvalidOperationException(validation.message);
        string[] roots = SelectedRoots(targets);
        using EditorHistoryTransaction transaction = m_interactions.history.BeginTransaction("Delete Assets");
        foreach (string path in roots)
        {
            if (!TryCreateContext(editor, path, out AssetEditorContext? context) || context is null || !DeleteWithHistory(context, m_interactions.history))
                throw new InvalidOperationException($"Selected source '{path}' could not be deleted.");
        }

        transaction.Commit();
    }

    /// <summary>
    /// Validates every minimal selected source and rejects destination collisions before a batch move.
    /// </summary>
    /// <param name="targets">
    /// Live source entries in selection order.
    /// </param>
    /// <param name="targetDirectory">
    /// The writable project destination.
    /// </param>
    /// <returns>
    /// True when every source can move to a distinct destination.
    /// </returns>
    [ScriptingApiIgnore]
    public bool CanMoveSelection(
        IReadOnlyList<AssetFileEntry> targets,
        string targetDirectory
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        if (targets.Count == 0 || targets.Any(value => value.isReadOnly || value.identity.runtimeIdentity is null))
            return false;
        var destinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string source in SelectedRoots(targets))
            if (!TryPrepareMove(source, targetDirectory, true, out _, out _, out string destination) || !destinations.Add(destination))
                return false;
        return true;
    }

    /// <summary>
    /// Moves selected source roots in one transaction; a later failure restores earlier moves.
    /// </summary>
    /// <param name="targets">
    /// Live entries, including descendants covered by a selected directory.
    /// </param>
    /// <param name="targetDirectory">
    /// The writable project destination.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Any source or destination rejects the operation.
    /// </exception>
    [ScriptingApiIgnore]
    public void MoveSelection(
        IReadOnlyList<AssetFileEntry> targets,
        string targetDirectory
    ) {
        if (!CanMoveSelection(targets, targetDirectory))
            throw new InvalidOperationException("The selected assets cannot move to this directory.");
        string[] roots = SelectedRoots(targets);
        using EditorHistoryTransaction transaction = m_interactions.history.BeginTransaction("Move Assets");
        foreach (string source in roots)
            _ = MoveToDirectoryWithHistory(source, targetDirectory, m_interactions.history);
        transaction.Commit();
    }

    private static string[] SelectedRoots(IReadOnlyList<AssetFileEntry> targets)
    {
        string[] paths = targets.Select(value => value.assetPath.ToString()).Distinct(StringComparer.Ordinal).ToArray();
        return paths.Where(path => !paths.Any(parent => parent != path && path.StartsWith(parent + "/", StringComparison.Ordinal))).ToArray();
    }
}
