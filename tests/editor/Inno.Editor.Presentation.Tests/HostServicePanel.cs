using Inno.Editor.Core;

namespace Inno.Editor.Presentation.Tests;

[EditorPanel("tests.host-service", "Host Service")]
public sealed class HostServicePanel : EditorPanel
{
    public static ITestHostService? current;

    public HostServicePanel(ITestHostService service)
    {
        current = service;
    }

    protected override void OnDraw(EditorContext context) => _ = context;
}
