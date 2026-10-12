using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Cli;

internal static class ProductCleanExecutor
{
    internal static ValueTask<FileLease> AcquireAsync(
        BuildCommandContext context,
        CancellationToken cancellationToken
    ) => FileLease.AcquireAsync(Path.Combine(context.root, "artifacts", "products", "editor", context.target.value,
        context.configuration, "product.lock"), TimeSpan.FromSeconds(10), cancellationToken);

    internal static void Execute(
        ProductCleanPlan plan,
        bool dryRun,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        foreach (string directory in plan.directories)
            PathBoundary.RequireUnlinkedPath(plan.root, directory);
        Console.WriteLine($"{(dryRun ? "Would invalidate" : "Invalidating")} {plan.receipt}");
        if (!dryRun)
            File.Delete(plan.receipt);
        foreach (string directory in plan.directories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine($"{(dryRun ? "Would clean" : "Cleaning")} {directory}");
            if (!dryRun)
                ToolchainEnvironment.DeleteDirectory(directory);
        }
    }
}
