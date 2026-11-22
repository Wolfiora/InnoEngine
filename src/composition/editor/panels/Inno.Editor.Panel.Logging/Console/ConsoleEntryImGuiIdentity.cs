using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;

internal static class ConsoleEntryImGuiIdentity
{
    internal static void Push(string identity) => Gui.PushID(identity);

    internal static void Pop() => Gui.PopID();
}
