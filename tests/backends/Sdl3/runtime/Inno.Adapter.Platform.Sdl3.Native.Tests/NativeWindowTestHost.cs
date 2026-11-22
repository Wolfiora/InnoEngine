using System;
using System.Collections.Generic;
using Inno.Testing.Processes;

namespace Inno.Adapter.Platform.Sdl3.Native.Tests;

internal static class NativeWindowTestHost
{
    private static readonly IReadOnlyDictionary<string, Action> CONTRACTS = new Dictionary<string, Action>
    {
        ["NativeSurfaceLifetimeTests.DisposedWindowRejectsBorrowingItsNativeSurface"] = () => new NativeSurfaceLifetimeTests()
            .DisposedWindowRejectsBorrowingItsNativeSurface(),
        ["HostIntegrationTests.WrappingFailureDestroysOnlyTheNewlyOwnedWindow"] = () => new HostIntegrationTests()
            .WrappingFailureDestroysOnlyTheNewlyOwnedWindow(),
        ["HostIntegrationTests.SingleWindowCapabilityRejectsCreationBeforeCallingHostAgain"] = () => new HostIntegrationTests()
            .SingleWindowCapabilityRejectsCreationBeforeCallingHostAgain(),
        ["WindowOwnershipTests.ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows"] = () => new WindowOwnershipTests()
            .ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows(),
        ["WindowOwnershipTests.AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt"] = () => new WindowOwnershipTests()
            .AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt(),
        ["WindowOwnershipTests.CrossApplicationReleaseAndCrossThreadCreationAreRejected"] = () => new WindowOwnershipTests()
            .CrossApplicationReleaseAndCrossThreadCreationAreRejected(),
        ["WindowSurfaceTests.OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution"] = () => new WindowSurfaceTests()
            .OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution(),
        ["Sdl3WindowOperationsTests.CreationPreservesOptionsAndLeavesRegistrationToTheOwner.False.False"] = () => new Sdl3WindowOperationsTests().CreationPreservesOptionsAndLeavesRegistrationToTheOwner(
            false,
            false
        ),
        ["Sdl3WindowOperationsTests.CreationPreservesOptionsAndLeavesRegistrationToTheOwner.True.True"] = () => new Sdl3WindowOperationsTests().CreationPreservesOptionsAndLeavesRegistrationToTheOwner(
            true,
            true
        )
    };

    private static readonly NativeContractHost HOST = new(typeof(NativeWindowTestHost).Assembly.Location, CONTRACTS);

    internal static bool RunIfRequired(string contract) => HOST.RunIfRequired(contract);

    private static int Main(string[] arguments) => HOST.Execute(arguments);
}
