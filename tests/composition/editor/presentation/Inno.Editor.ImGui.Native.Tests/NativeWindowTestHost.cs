using System;
using System.Collections.Generic;
using Inno.Testing.Processes;

namespace Inno.Editor.ImGui.Native.Tests;

internal static class NativeWindowTestHost
{
    private static readonly IReadOnlyDictionary<string, Action> CONTRACTS = new Dictionary<string, Action>
    {
        ["ViewportMetricsTests.FramePreparationRunsBeforeNativeCreationAndRejectsReentrantOperations"] = () => new ViewportMetricsTests()
            .FramePreparationRunsBeforeNativeCreationAndRejectsReentrantOperations(),
        ["ViewportMetricsTests.FailedPreparationDoesNotBeginOrPublishANativeFrame"] = () => new ViewportMetricsTests()
            .FailedPreparationDoesNotBeginOrPublishANativeFrame(),
        ["ViewportMetricsTests.AFailedDrawRetiresItsNativeFrameBeforeTheNextRequest"] = () => new ViewportMetricsTests()
            .AFailedDrawRetiresItsNativeFrameBeforeTheNextRequest(),
        ["ViewportMetricsTests.ContextAdvertisesOnlyTheSelectedRenderersVertexOffsetCapability.False"] = () => new ViewportMetricsTests().ContextAdvertisesOnlyTheSelectedRenderersVertexOffsetCapability(
            false
        ),
        ["ViewportMetricsTests.ContextAdvertisesOnlyTheSelectedRenderersVertexOffsetCapability.True"] = () => new ViewportMetricsTests().ContextAdvertisesOnlyTheSelectedRenderersVertexOffsetCapability(
            true
        ),
        ["ViewportMetricsTests.FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback.False"] = () => new ViewportMetricsTests().FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback(
            false
        ),
        ["ViewportMetricsTests.FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback.True"] = () => new ViewportMetricsTests().FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback(
            true
        ),
        ["ViewportMetricsTests.MetricRefreshPreservesRequestedViewportGeometry"] = () => new ViewportMetricsTests()
            .MetricRefreshPreservesRequestedViewportGeometry(),
        ["ViewportMetricsTests.DetachedWindowAppliesRequestsAfterPlatformEventReconciliation"] = () => new ViewportMetricsTests()
            .DetachedWindowAppliesRequestsAfterPlatformEventReconciliation(),
        ["ViewportMetricsTests.LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame"] = () => new ViewportMetricsTests()
            .LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame(),
        ["ViewportMetricsTests.ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction"] = () => new ViewportMetricsTests()
            .ViewportCallbacksRegisterOneSurfaceAndReleaseItBeforeNativeDestruction(),
        ["ViewportRetirementTests.FaultedRetirementRetainsWindowRendererAndApplicationOwners"] = () => new ViewportRetirementTests()
            .FaultedRetirementRetainsWindowRendererAndApplicationOwners(),
        ["ViewportRetirementTests.DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement"] = () => new ViewportRetirementTests()
            .DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement(),
        ["ViewportRetirementTests.IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint"] = () => new ViewportRetirementTests()
            .IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint(),
        ["ViewportRetirementTests.CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup"] = () => new ViewportRetirementTests()
            .CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup(),
        ["FontCoverageTests.EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback.0.75"] = () => new FontCoverageTests().EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback(
            0.75f
        ),
        ["FontCoverageTests.EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback.1"] = () => new FontCoverageTests().EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback(
            1f
        ),
        ["FontCoverageTests.EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback.1.5"] = () => new FontCoverageTests().EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback(
            1.5f
        ),
        ["FontCoverageTests.EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback.2"] = () => new FontCoverageTests().EveryDefaultTextFaceHasEnglishAndIconsWithoutLanguageFallback(
            2f
        ),
        ["FontCoverageTests.CustomFontStyleUsesOnlyItsOwnTextSourceAndIcons"] = () => new FontCoverageTests()
            .CustomFontStyleUsesOnlyItsOwnTextSourceAndIcons(),
        ["FontCoverageTests.RetiringTenContextsReleasesTheirSourcesAndPreservesAnotherContext"] = () => new FontCoverageTests()
            .RetiringTenContextsReleasesTheirSourcesAndPreservesAnotherContext(),
        ["FontCoverageTests.SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow"] = () => new FontCoverageTests()
            .SuccessfulDisposalPermitsReplacementOnTheSameLiveWindow(),
        ["FontCoverageTests.ApplicationShutdownRetiresBothRegisteredContexts"] = () => new FontCoverageTests()
            .ApplicationShutdownRetiresBothRegisteredContexts()
    };

    private static readonly NativeContractHost HOST = new(typeof(NativeWindowTestHost).Assembly.Location, CONTRACTS);

    internal static bool RunIfRequired(string contract) => HOST.RunIfRequired(contract);

    private static int Main(string[] arguments) => HOST.Execute(arguments);
}
