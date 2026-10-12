using System;
using System.Globalization;
using System.Linq;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
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
        if (scope == "cache")
        {
            await CleanCachesAsync(context, options, automatic: false, cancellationToken).ConfigureAwait(false);
            return;
        }
        if (scope != "product")
            throw new ArgumentException($"Unsupported cleanup scope '{scope}'.");
        string project = await context.PreflightEditorAsync(cancellationToken).ConfigureAwait(false);
        using var ownership = await ProductCleanExecutor.AcquireAsync(context, cancellationToken).ConfigureAwait(false);
        ProductCleanPlan plan = await ProductCleanPlan.CreateAsync(context, project, cancellationToken).ConfigureAwait(false);
        ProductCleanExecutor.Execute(plan, options.Has("dry-run"), cancellationToken);
    }

    internal static async Task CleanCachesAsync(
        BuildCommandContext context,
        BuildCommandLine options,
        bool automatic,
        CancellationToken cancellation
    ) {
        long ReadBytes(
            string name,
            long fallback
        ) {
            string? value = options.Has(name) ? options.Require(name) : null;
            return value is null ? fallback : checked(long.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture) * 1024 * 1024 * 1024);
        }
        double seconds = double.Parse(options.Read("cache-time-seconds", automatic ? "5" : "600"),
            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        var policy = new BuildCachePolicy(ReadBytes("cache-budget-gib", 64L << 30), ReadBytes("cache-min-free-gib", 20L << 30),
            TimeSpan.FromSeconds(seconds));
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(context.root,
            StandardBuildCaches.CreateOwners(context.root, context.distribution.platforms.SelectMany(static platform => platform.nativeProducts.Values)
                .SelectMany(static product => product.steps).Select(static step => step.component).ToArray()),
            policy, retireAll: !automatic, cancellationToken: cancellation);
        bool dryRun = options.Has("dry-run");
        if (dryRun)
            foreach (BuildCacheArtifactDescriptor artifact in plan.candidates)
                Console.WriteLine($"Would retire {artifact.directory} ({artifact.bytes} bytes)");
        BuildCacheCleanupResult result = await plan.ExecuteAsync(dryRun, cancellation).ConfigureAwait(false);
        Console.WriteLine($"Cache: {plan.totalBytes} bytes; protected {plan.protectedBytes}; reclaimed {result.retiredBytes}; skipped {result.skipped.Count}.");
        foreach (var failure in result.failures)
            Console.Error.WriteLine($"Cache retirement failed: {failure.Key}: {failure.Value}");
        if (!automatic && result.failures.Count != 0)
            throw new InvalidOperationException("Explicit cache cleanup completed with failures.");
    }
}
