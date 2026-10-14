using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Runs publisher-owned cache maintenance without backend, generator or Editor dependencies.
/// </summary>
public static class BuildMaintenanceApplication
{
    /// <summary>
    /// Executes the cache-clean command using the checkout's explicit publisher catalog.
    /// </summary>
    /// <param name="arguments">
    /// The clean command and its validated named options.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels planning or retirement without deleting protected outputs.
    /// </param>
    /// <returns>
    /// Zero on success, two on cancellation, or one with a diagnostic on failure.
    /// </returns>
    public static async Task<int> Run(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        try
        {
            ArgumentNullException.ThrowIfNull(arguments);
            cancellationToken.ThrowIfCancellationRequested();
            if (arguments.Length == 0 || arguments[0] != "clean")
                throw new ArgumentException("Maintenance requires the clean command.");
            var options = new Dictionary<string, string>(StringComparer.Ordinal);
            var allowed = new HashSet<string>(StringComparer.Ordinal)
            {
                "scope", "engine-root", "target", "tools-target", "dotnet", "configuration", "dry-run",
                "cache-budget-gib", "cache-min-free-gib", "cache-time-seconds"
            };
            for (int index = 1; index < arguments.Length; index++)
            {
                string argument = arguments[index];
                if (!argument.StartsWith("--", StringComparison.Ordinal) || !allowed.Contains(argument[2..]))
                    throw new ArgumentException($"Unknown maintenance option '{argument}'.");
                string value = "true";
                if (argument != "--dry-run")
                {
                    if (++index >= arguments.Length || arguments[index].StartsWith("--", StringComparison.Ordinal))
                        throw new ArgumentException($"Missing value for '{argument}'.");
                    value = arguments[index];
                }
                if (!options.TryAdd(argument[2..], value))
                    throw new ArgumentException($"Duplicate maintenance option '{argument}'.");
            }
            if (options.GetValueOrDefault("scope") != "cache" || !options.TryGetValue("engine-root", out string? root))
                throw new ArgumentException("Cache maintenance requires --scope cache and --engine-root.");
            long Bytes(
                string name,
                long fallback
            ) => options.TryGetValue(name, out string? value)
                ? checked(long.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture) * (1L << 30)) : fallback;
            double seconds = double.Parse(options.GetValueOrDefault("cache-time-seconds", "600"),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            await CleanAsync(root, new BuildCachePolicy(Bytes("cache-budget-gib", 64L << 30),
                Bytes("cache-min-free-gib", 20L << 30), TimeSpan.FromSeconds(seconds)),
                true, options.ContainsKey("dry-run"), options.GetValueOrDefault("target"), cancellationToken).ConfigureAwait(false);
            return 0;
        }
        catch (OperationCanceledException) { return 2; }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }

    /// <summary>
    /// Uses the same owner inventory, dependency protection and retirement plan for automatic and explicit cleanup.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute checkout containing registered publishers.
    /// </param>
    /// <param name="policy">
    /// The bounded size and duration policy.
    /// </param>
    /// <param name="retireAll">
    /// Whether to select every unreferenced generation instead of pressure candidates.
    /// </param>
    /// <param name="dryRun">
    /// Whether to report the plan without retiring it.
    /// </param>
    /// <param name="targetId">
    /// An optional exact producer target filter; shared dependencies remain protected.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels inventory and retirement.
    /// </param>
    /// <returns>
    /// A completed, bounded cleanup operation with protected owners retained.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Explicit retirement encountered failures.
    /// </exception>
    /// <exception cref="IOException">
    /// Ownership metadata or protected input cannot be validated.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller canceled before completion.
    /// </exception>
    public static async Task CleanAsync(
        string engineRoot,
        BuildCachePolicy policy,
        bool retireAll,
        bool dryRun,
        string? targetId,
        CancellationToken cancellationToken
    ) {
        BuildCacheCleanupPlan plan = BuildCacheCleanupPlan.Create(engineRoot,
            BuildCacheOwnerCatalog.ReadOwners(engineRoot, cancellationToken), policy, retireAll, targetId, cancellationToken);
        if (dryRun)
            foreach (BuildCacheArtifactDescriptor artifact in plan.candidates)
                Console.WriteLine($"Would retire {artifact.directory} ({artifact.bytes} bytes)");
        BuildCacheCleanupResult result = await plan.ExecuteAsync(dryRun, cancellationToken).ConfigureAwait(false);
        Console.WriteLine($"Cache: {plan.totalBytes} bytes; protected {plan.protectedBytes}; reclaimed {result.retiredBytes}; skipped {result.skipped.Count}.");
        foreach (var failure in result.failures)
            Console.Error.WriteLine($"Cache retirement failed: {failure.Key}: {failure.Value}");
        if (retireAll && result.failures.Count != 0)
            throw new InvalidOperationException("Explicit cache cleanup completed with failures.");
    }
}
