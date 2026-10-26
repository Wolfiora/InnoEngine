using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Reflection;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Extensibility.Modules;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
internal static class InspectorTypeOrigin
{
    internal static void Draw(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.assetBreadcrumbText);
        try
        {
            EditorUI.current.widgets.WrappedText("Source: " + Describe(type));
        }
        finally
        {
            Ui.PopStyleColor();
        }
    }

    internal static string Describe(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Assembly assembly = type.Assembly;
        string assemblyName = assembly.GetName().Name ?? type.Namespace ?? type.Name;
        try
        {
            return $"{assembly.GetInnoAssemblyDomain()}/{assembly.GetInnoAssemblyScope()} · {assemblyName}";
        }
        catch (InvalidOperationException)
        {
            return assemblyName;
        }
    }
}
