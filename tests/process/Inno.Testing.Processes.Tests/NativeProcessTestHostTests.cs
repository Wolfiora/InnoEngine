using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Xunit;

namespace Inno.Testing.Processes.Tests;

public sealed class NativeProcessTestHostTests
{
    [Fact]
    public void SuccessfulContractDrainsItsOutput() => NativeProcessTestHost.Run(typeof(NativeProcessHostFixture).Assembly.Location, "success");

    [Fact]
    public void FailedContractRetainsBothStreamsAndIdentity()
    {
        InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() =>
            NativeProcessTestHost.Run(typeof(NativeProcessHostFixture).Assembly.Location, "failure"));
        Assert.Contains("Fixture stdout: failure", failure.Message);
        Assert.Contains("Fixture stderr: failure", failure.Message);
        Assert.Contains("OutputDrained=True", failure.Message);
        Assert.Contains("Command:", failure.Message);
    }

    [Fact]
    public void TimeoutRetainsPartialOutputAndTerminatesDescendants()
    {
        TimeoutException failure = Assert.Throws<TimeoutException>(() =>
            NativeProcessTestHost.Run(typeof(NativeProcessHostFixture).Assembly.Location, "timeout", 3_000));
        Assert.Contains("Fixture stdout: timeout", failure.Message);
        Assert.Contains("Fixture stderr: timeout", failure.Message);
        const string childPrefix = "Child PID: ";
        int start = failure.Message.IndexOf(childPrefix, StringComparison.Ordinal);
        Assert.True(start >= 0, failure.Message);
        ReadOnlySpan<char> identifier = failure.Message.AsSpan(start + childPrefix.Length);
        int end = identifier.IndexOfAny('\r', '\n');
        int childId = int.Parse(end < 0 ? identifier : identifier[..end], CultureInfo.InvariantCulture);
        Assert.True(SpinWait.SpinUntil(() => HasExited(childId), TimeSpan.FromSeconds(5)), failure.Message);
    }

    [Fact]
    public void RelativeAssemblyAndInvalidBudgetFailBeforeStarting()
    {
        Assert.Throws<ArgumentException>(() => NativeProcessTestHost.Run("relative.dll", "success"));
        Assert.Throws<ArgumentOutOfRangeException>(() => NativeProcessTestHost.Run(typeof(NativeProcessHostFixture).Assembly.Location, "success", 0));
    }

    private static bool HasExited(int processId)
    {
        try
        {
            using Process child = Process.GetProcessById(processId);
            return child.HasExited;
        }
        catch (ArgumentException)
        {
            return true;
        }
    }
}
