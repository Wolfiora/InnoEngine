using System;
using System.Collections.Generic;
using Inno.Testing.Processes;

namespace Inno.Editor.ImGui.Tests;

internal static class NativeWindowTestHost
{
    private static readonly IReadOnlyDictionary<string, Action> S_CONTRACTS = new Dictionary<string, Action>
    {
        ["ViewportMetricsTests.FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback.False"] = () => new ViewportMetricsTests().FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback(false),
        ["ViewportMetricsTests.FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback.True"] = () => new ViewportMetricsTests().FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback(true),
        ["ViewportMetricsTests.MetricRefreshPreservesRequestedViewportGeometry"] = () => new ViewportMetricsTests().MetricRefreshPreservesRequestedViewportGeometry(),
        ["ViewportMetricsTests.DetachedWindowAppliesRequestsAfterPlatformEventReconciliation"] = () => new ViewportMetricsTests().DetachedWindowAppliesRequestsAfterPlatformEventReconciliation(),
        ["ViewportMetricsTests.LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame"] = () => new ViewportMetricsTests().LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame(),
        ["ViewportMetricsTests.ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction"] = () => new ViewportMetricsTests().ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction(),
        ["ViewportRetirementTests.FaultedRetirementRetainsWindowRendererAndApplicationOwners"] = () => new ViewportRetirementTests().FaultedRetirementRetainsWindowRendererAndApplicationOwners(),
        ["ViewportRetirementTests.DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement"] = () => new ViewportRetirementTests().DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement(),
        ["ViewportRetirementTests.IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint"] = () => new ViewportRetirementTests().IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint(),
        ["ViewportRetirementTests.CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup"] = () => new ViewportRetirementTests().CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup(),
    };
    private static bool m_executingContract;

    internal static bool RunIfRequired(string contract)
    {
        if (m_executingContract || !OperatingSystem.IsMacOS()
            && Environment.GetEnvironmentVariable("INNO_TEST_MAIN_THREAD_HOST") != "1")
            return false;
        NativeProcessTestHost.Run(typeof(NativeWindowTestHost).Assembly.Location, contract);
        return true;
    }

    private static int Main(string[] arguments)
    {
        try
        {
            if (arguments.Length != 1 || !S_CONTRACTS.TryGetValue(arguments[0], out Action? contract))
                throw new ArgumentException("The native test host requires one declared contract.");
            // Cocoa requires the process main thread, rather than an xUnit worker or a new managed thread.
            m_executingContract = true;
            Console.Error.WriteLine($"Native contract started: {arguments[0]}; thread={Environment.CurrentManagedThreadId}.");
            contract();
            Console.Error.WriteLine($"Native contract completed: {arguments[0]}.");
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }
}
