using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Architecture;
using Inno.Tooling.Validation;

namespace Inno.Tooling.Cli;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        using var cancellation = new CancellationTokenSource();
        Console.CancelKeyPress += (
            _,
            options
        ) =>
        {
            options.Cancel = true;
            cancellation.Cancel();
        };
        try
        {
            return arguments switch
            {
                ["validate", .. var options] => await ValidationCommand.ExecuteAsync(options, cancellation.Token).ConfigureAwait(false),
                ["docs", .. var options] => await DocumentationReferences.ExecuteAsync(options, cancellation.Token).ConfigureAwait(false),
                ["rules", "select", .. var options] => RepositoryRuleSelection.Execute(options),
                _ => throw new ArgumentException("Expected validate, docs or rules select.")
            };
        }
        catch (OperationCanceledException)
        {
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure.Message);
            return 1;
        }
    }
}
