using System;
using Inno.Editor.Core;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorModule("tests.dispose-observer", order: 900)]
public sealed class ADisposeObserverModule(EditorInteractions interactions) : EditorModule
{
    public static bool historyAvailableDuringStop;
    public static bool historyDisposedDuringDispose;
    public static int disposeCount;

    protected override void OnStop(EditorContext context)
    {
        if (interactions.history.isFaulted)
        {
            return;
        }
        using EditorHistoryTransaction transaction = interactions.history.BeginTransaction("Shutdown probe");
        historyAvailableDuringStop = transaction.Rollback().succeeded;
    }

    protected override void OnDispose()
    {
        disposeCount++;
        try
        {
            using EditorHistoryTransaction transaction = interactions.history.BeginTransaction("Disposed probe");
        }
        catch (ObjectDisposedException)
        {
            historyDisposedDuringDispose = true;
        }
    }
}
