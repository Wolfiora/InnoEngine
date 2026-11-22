using System;
using Inno.Core.Execution;
using Inno.Editor.Core;

namespace Inno.Editor.Presentation.Tests;

[EditorPanel("tests.panel", "Test", defaultOpen: false, menuPath: "Testing")]
public sealed class TestPanel(TestModule module) : EditorPanel
{
    public static int attachCount;
    public static int detachCount;
    public static bool firstAttachPrecededModuleStart;
    public static bool throwFromWindowPadding;
    public static bool throwOnAttach;
    public static bool throwOnDetach;
    public static int pendingDetaches;

    public override bool useWindowPadding
        => throwFromWindowPadding ? throw new InvalidOperationException("Injected panel presentation failure.") : true;

    protected override void OnAttach(EditorContext context)
    {
        _ = module;
        if (attachCount == 0)
        {
            firstAttachPrecededModuleStart = TestModule.startCount == 0;
        }
        attachCount++;
        if (throwOnAttach)
        {
            throw new InvalidOperationException("Injected panel attachment failure.");
        }
    }

    protected override void OnDetach(EditorContext context)
    {
        detachCount++;
        ShutdownOrder.events.Add("panel.detach");
        if (pendingDetaches-- > 0)
        {
            throw new RetirementPendingException("Panel-owned work has not drained.");
        }
        if (throwOnDetach)
        {
            throw new InvalidOperationException("Injected panel detach failure.");
        }
    }

    protected override void OnDraw(EditorContext context) {}
}
