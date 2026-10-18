using System;
using System.IO;
using Inno.Build.SupportPacks;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Cli;

internal static class SupportPackCommand
{
    internal static async Task RunAsync(
        BuildCommandContext context,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string output = options.Read("output", Path.Combine(context.root, "artifacts", "support-packs"));
        using PlayerSupportPackPublication pack = await context.distribution.supportPacks.PublishAsync(context.root, output, context.target,
            context.composition.dotnetHost, cancellationToken).ConfigureAwait(false);
        Console.WriteLine(pack.directory);
        foreach (string diagnostic in pack.diagnostics)
            Console.Error.WriteLine(diagnostic);
    }
}
