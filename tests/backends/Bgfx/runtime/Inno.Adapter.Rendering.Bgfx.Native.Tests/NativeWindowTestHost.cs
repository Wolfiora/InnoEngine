using System;
using System.Collections.Generic;
using Inno.Testing.Processes;

namespace Inno.Adapter.Rendering.Bgfx.Native.Tests;

internal static class NativeWindowTestHost
{
    private static readonly IReadOnlyDictionary<string, Action> CONTRACTS = new Dictionary<string, Action>
    {
        ["BgfxShutdownTests.ShutdownAfterAnOffscreenGraphReleasesTheProcessOwner"] = () => new BgfxShutdownTests()
            .ShutdownAfterAnOffscreenGraphReleasesTheProcessOwner(),
        ["BgfxSurfaceRetirementTests.MultiThreadedSurfaceAcknowledgesDestructionAfterTheFollowingSubmission"] = () => new BgfxSurfaceRetirementTests()
            .MultiThreadedSurfaceAcknowledgesDestructionAfterTheFollowingSubmission(),
        ["BgfxSurfaceRetirementTests.InlineSurfaceUsesTheSameConservativeAcknowledgment"] = () => new BgfxSurfaceRetirementTests()
            .InlineSurfaceUsesTheSameConservativeAcknowledgment(),
        ["BgfxSurfaceRetirementTests.InvalidIntegrationFailsBeforeTakingTheProcessOwner"] = () => new BgfxSurfaceRetirementTests()
            .InvalidIntegrationFailsBeforeTakingTheProcessOwner()
    };

    private static readonly NativeContractHost HOST = new(typeof(NativeWindowTestHost).Assembly.Location, CONTRACTS);

    internal static bool RunIfRequired(string contract) => HOST.RunIfRequired(contract);

    private static int Main(string[] arguments) => HOST.Execute(arguments);
}
