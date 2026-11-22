using System;
using Inno.Core.Execution;
using Inno.Editor.Core;
using Inno.Extensibility.Types;

namespace Inno.Editor.Presentation.Tests;

[EditorModule("tests.state")]
public sealed class TestModule : EditorModule
{
    public static int startCount;
    public static int stopCount;
    public static int stateValue;
    public static int restoredStateValue;
    public static bool rebuildDuringRestore;
    public static bool captureFailure;
    public static bool throwOnStart;
    public static bool throwOnStop;
    public static int pendingStops;

    private readonly TypeCatalog m_types;

    public TestModule(TypeCatalog types)
    {
        m_types = types;
    }

    protected override void Capture(EditorState state)
    {
        if (captureFailure)
        {
            throw new InvalidOperationException("The test module state cannot be captured.");
        }
        state.Set("value", stateValue);
    }

    protected override void Restore(EditorState state)
    {
        restoredStateValue = state.Get("value", 0);
        if (!rebuildDuringRestore)
        {
            return;
        }
        rebuildDuringRestore = false;
        m_types.Rebuild();
    }

    protected override void OnStart(EditorContext context)
    {
        startCount++;
        if (throwOnStart)
        {
            throw new InvalidOperationException("Injected module startup failure.");
        }
        if (startCount == 1)
        {
            m_types.Rebuild();
        }
    }

    protected override void OnStop(EditorContext context)
    {
        stopCount++;
        ShutdownOrder.events.Add("module.stop");
        if (pendingStops-- > 0)
        {
            throw new RetirementPendingException("Module-owned work has not drained.");
        }
        if (throwOnStop)
        {
            throw new InvalidOperationException("Injected module stop failure.");
        }
    }
}
