using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Inno.Tooling.Architecture.Tests;

internal static class ArchitectureCommandFixture
{
    internal static (int exitCode, string output) Run(
        string root,
        params string[] arguments
    ) => RunBounded(root, TimeSpan.FromMinutes(2), arguments);

    internal static (int exitCode, string output) RunComplete(
        string root,
        params string[] arguments
    ) => RunBounded(root, TimeSpan.FromMinutes(15), arguments);

    private static (int exitCode, string output) RunBounded(
        string root,
        TimeSpan budget,
        string[] arguments
    ) {
        using var cancellation = new CancellationTokenSource(budget);
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        TextWriter previousOutput = Console.Out;
        TextWriter previousError = Console.Error;
        try
        {
            Console.SetOut(output);
            Console.SetError(output);
            int exitCode = ArchitectureValidator.Execute([root, .. arguments], cancellation.Token);
            return (exitCode, output.ToString());
        }
        finally
        {
            Console.SetOut(previousOutput);
            Console.SetError(previousError);
        }
    }
}
