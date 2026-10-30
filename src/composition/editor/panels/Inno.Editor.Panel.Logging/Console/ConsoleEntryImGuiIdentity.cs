using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;
internal static class ConsoleEntryImGuiIdentity
{
    internal static void Push(string identity)
    {
        Ui.PushId(identity);
    }

    internal static void Pop()
    {
        Ui.PopId();
    }
}
