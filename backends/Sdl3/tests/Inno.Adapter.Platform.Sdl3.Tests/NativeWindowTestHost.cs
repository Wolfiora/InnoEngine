using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

internal static class NativeWindowTestHost
{
    private static readonly IReadOnlyDictionary<string, Action> S_CONTRACTS = new Dictionary<string, Action>
    {
        ["HostIntegrationTests.WrappingFailureDestroysOnlyTheNewlyOwnedWindow"] = () => new HostIntegrationTests().WrappingFailureDestroysOnlyTheNewlyOwnedWindow(),
        ["HostIntegrationTests.SingleWindowCapabilityRejectsCreationBeforeCallingHostAgain"] = () => new HostIntegrationTests().SingleWindowCapabilityRejectsCreationBeforeCallingHostAgain(),
        ["WindowOwnershipTests.ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows"] = () => new WindowOwnershipTests().ApplicationDirectoryContainsOnlyCreatedOrAdoptedWindows(),
        ["WindowOwnershipTests.AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt"] = () => new WindowOwnershipTests().AdoptedWindowIsInvalidatedBeforeItsOriginalOwnerDestroysIt(),
        ["WindowOwnershipTests.CrossApplicationReleaseAndCrossThreadCreationAreRejected"] = () => new WindowOwnershipTests().CrossApplicationReleaseAndCrossThreadCreationAreRejected(),
        ["WindowSurfaceTests.OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution"] = () => new WindowSurfaceTests().OwnedAndBorrowedWrappersUseTheSameHostSurfaceResolution(),
        ["Sdl3WindowOperationsTests.CreationPreservesOptionsAndLeavesRegistrationToTheOwner.False.False"] = () => new Sdl3WindowOperationsTests().CreationPreservesOptionsAndLeavesRegistrationToTheOwner(false, false),
        ["Sdl3WindowOperationsTests.CreationPreservesOptionsAndLeavesRegistrationToTheOwner.True.True"] = () => new Sdl3WindowOperationsTests().CreationPreservesOptionsAndLeavesRegistrationToTheOwner(true, true),
    };
    private static bool m_executingContract;

    internal static bool RunIfRequired(string contract)
    {
        if (m_executingContract || !OperatingSystem.IsMacOS()
            && Environment.GetEnvironmentVariable("INNO_TEST_MAIN_THREAD_HOST") != "1")
            return false;
        var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        start.ArgumentList.Add(typeof(NativeWindowTestHost).Assembly.Location);
        start.ArgumentList.Add(contract);
        using var process = Process.Start(start) ?? throw new InvalidOperationException("The native contract host did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(90_000))
        {
            process.Kill(entireProcessTree: true);
            process.WaitForExit();
            throw new TimeoutException("The main-thread native contract exceeded its process budget: " + contract);
        }
        Assert.True(process.ExitCode == 0,
            $"Main-thread contract '{contract}' failed.\n{output.GetAwaiter().GetResult()}\n{error.GetAwaiter().GetResult()}");
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
            contract();
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }
}
