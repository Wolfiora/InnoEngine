using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Cli;

internal static class CleanCommand
{
    internal static async Task RunAsync(
        BuildCommandContext context,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string scope = options.Require("scope");
        if (scope != "product")
            throw new ArgumentException($"Unsupported cleanup scope '{scope}'.");
        string project = await context.PreflightEditorAsync(cancellationToken).ConfigureAwait(false);
        using var ownership = await ProductCleanExecutor.AcquireAsync(context, cancellationToken).ConfigureAwait(false);
        ProductCleanPlan plan = await ProductCleanPlan.CreateAsync(context, project, cancellationToken).ConfigureAwait(false);
        ProductCleanExecutor.Execute(plan, options.Has("dry-run"), cancellationToken);
    }
}
