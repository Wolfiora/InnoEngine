using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;

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
        if (!options.Has("dry-run"))
            StandardBuildCaches.CreateOwners(context.root, context.distribution.platforms.SelectMany(static platform => platform.nativeProducts.Values)
                .SelectMany(static product => product.plan.steps).Select(static step => step.component).ToArray());
        await BuildMaintenanceApplication.CleanAsync(context.root, policy, !automatic, options.Has("dry-run"),
            !automatic && options.Has("target") ? options.Require("target") : null, cancellation).ConfigureAwait(false);
    }
}
