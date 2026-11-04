using System;

using Inno.Editor.Diagnostics;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;

namespace Inno.Editor.Panel.Inspector;

internal static class MissingTypeDiagnostics
{
    internal static void Draw(
        Guid typeId,
        IEditorConsole console
    ) {
        string semanticId = typeId.ToString("D");
        foreach (EditorConsoleOccurrence occurrence in console.Capture().occurrences)
        {
            if (occurrence.kind != EditorConsoleEntryKind.Diagnostic
                || !string.Equals(occurrence.semanticId, semanticId, StringComparison.OrdinalIgnoreCase))
                continue;
            EditorUiSession.current.widgets.WrappedText(occurrence.displayMessage);
            if (!string.IsNullOrEmpty(occurrence.file))
                EditorUiSession.current.widgets.WrappedText($"Source: {occurrence.file}:{occurrence.line}:{occurrence.column}");
        }
    }
}
