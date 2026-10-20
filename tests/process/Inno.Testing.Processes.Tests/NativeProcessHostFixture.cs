using System;
using System.Diagnostics;
using System.Threading;

namespace Inno.Testing.Processes.Tests;

internal static class NativeProcessHostFixture
{
    private static int Main(string[] arguments)
    {
        if (arguments.Length != 1)
            return 2;
        Console.WriteLine("Fixture stdout: " + arguments[0]);
        Console.Error.WriteLine("Fixture stderr: " + arguments[0]);
        if (arguments[0] == "success")
            return 0;
        if (arguments[0] == "failure")
            return 3;
        if (arguments[0] == "timeout")
        {
            var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            start.ArgumentList.Add(typeof(NativeProcessHostFixture).Assembly.Location);
            start.ArgumentList.Add("leaf");
            using Process child = Process.Start(start) ?? throw new InvalidOperationException("The child fixture did not start.");
            Console.WriteLine("Child PID: " + child.Id);
        }
        Thread.Sleep(60_000);
        return 4;
    }
}
