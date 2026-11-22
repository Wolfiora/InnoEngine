using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HostIcon = Inno.Adapter.Presentation.ImGui.ImGuiIcon;

namespace Inno.Editor.ImGui;

/// <summary>
/// Describes the host's complete standard glyph vocabulary as immutable presentation values.
/// </summary>
public static class EditorIconCatalog
{
    /// <summary>
    /// Gets all named glyphs supplied by the installed UI font, in deterministic name order.
    /// </summary>
    public static IReadOnlyList<(string name, string glyph)> standard { get; } = Array.AsReadOnly(
        typeof(HostIcon)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(static field => field.IsLiteral && field.FieldType == typeof(string))
            .Select(static field => (name: field.Name, glyph: (string)field.GetRawConstantValue()!))
            .OrderBy(static value => value.name, StringComparer.Ordinal)
            .ToArray()
    );
}
