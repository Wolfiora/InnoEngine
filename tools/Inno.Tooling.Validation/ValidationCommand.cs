using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

/// <summary>
/// Provides repository validation planning, execution and evidence reconciliation to the existing Build entry.
/// </summary>
public static class ValidationCommand
{
    /// <summary>
    /// Executes an explicit plan, run, full or evidence operation and persists inspectable results.
    /// </summary>
    /// <param name="arguments">
    /// The operation followed by explicit engine-root, base, dotnet, configuration, artifacts-path, output, plan,
    /// evidence, full, extended, cache-input or cache-output options. Cache archives contain immutable native payloads only.
    /// Explicit docs-cache-root and docs-snapshot options select protected external documentation facts.
    /// Optional docs-base-snapshot identifies changed pages and facts without rescanning an authoring checkout.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels child processes while retaining the first failure and completed diagnostics.
    /// </param>
    /// <returns>
    /// Zero when the requested operation succeeds, one for unsuccessful execution, or two when planning requires
    /// explicitly prepared inputs. A blocked preparation report is written to preparation.json instead of producing
    /// a new executable plan. The command never fetches native source inputs automatically.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An operation or option is unknown, duplicated or missing its value.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels validation.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A supplied plan or evidence record is invalid or does not describe the current input.
    /// </exception>
    public static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Length == 0 || arguments[0] is not ("plan" or "run" or "full" or "evidence"))
            throw new ArgumentException("Usage: validate <plan|run|full|evidence> --engine-root ROOT --dotnet HOST [--base REV] [--output DIR] [--plan FILE] [--evidence FILE]");
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 1; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (key is not ("--engine-root" or "--dotnet" or "--base" or "--configuration" or "--artifacts-path" or "--output" or "--plan" or "--evidence" or "--extended" or "--full" or "--cache-input" or "--cache-output" or "--docs-cache-root" or "--docs-snapshot" or "--docs-base-snapshot")
                || index + 1 >= arguments.Length || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException($"Unknown, duplicate or incomplete validation option: {key}");
        }
        string root = Path.GetFullPath(options.GetValueOrDefault("--engine-root", Directory.GetCurrentDirectory()));
        bool extended = options.TryGetValue("--extended", out string? depth) && bool.Parse(depth);
        bool full = arguments[0] == "full" || options.TryGetValue("--full", out string? complete) && bool.Parse(complete);
        string output = Path.GetFullPath(options.GetValueOrDefault("--output", Path.Combine(root, "artifacts", "validation", "runs", Guid.NewGuid().ToString("N"))));
        string artifacts = Path.GetFullPath(options.GetValueOrDefault("--artifacts-path", Path.Combine(root, "artifacts", "validation", "workspaces",
            options.GetValueOrDefault("--configuration", "Debug"))));
        string dotnet = options.GetValueOrDefault("--dotnet", "dotnet");
        Directory.CreateDirectory(output);
        if (options.ContainsKey("--plan") && (options.ContainsKey("--docs-cache-root") || options.ContainsKey("--docs-snapshot")
            || options.ContainsKey("--docs-base-snapshot")))
            throw new ArgumentException("A supplied plan already fixes its documentation selection; regenerate the plan to change it.");
        ValidationPlan plan;
        if (options.TryGetValue("--plan", out string? planPath))
            plan = Read<ValidationPlan>(planPath);
        else
        {
            ValidationPlanningResult selected = await ValidationPlanner.CreateAsync(root, options.GetValueOrDefault("--base", "HEAD"), dotnet,
                options.GetValueOrDefault("--configuration", "Debug"), artifacts, full, cancellationToken, extended,
                options.GetValueOrDefault("--docs-cache-root"), options.GetValueOrDefault("--docs-snapshot"),
                options.GetValueOrDefault("--docs-base-snapshot")).ConfigureAwait(false);
            if (selected.preparation is not null)
            {
                await File.WriteAllTextAsync(Path.Combine(output, "preparation.json"), JsonSerializer.Serialize(selected.preparation,
                    new JsonSerializerOptions { WriteIndented = true }), cancellationToken).ConfigureAwait(false);
                Console.WriteLine("Planning blocked: " + selected.preparation.reason + " No new executable plan was produced. " + output);
                return 2;
            }
            plan = selected.plan ?? throw new InvalidDataException("Planning produced neither an executable plan nor explicit preparation requirements.");
        }
        if (!Path.GetFullPath(plan.root).Equals(root, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            throw new InvalidDataException("The supplied plan belongs to another workspace.");
        if (plan.checks.Length == 0 || plan.checks.Select(static check => check.id).Distinct(StringComparer.Ordinal).Count() != plan.checks.Length)
            throw new InvalidDataException("A validation plan must contain unique, explicit required checks.");
        if (full && !plan.full)
            throw new InvalidDataException("Full validation cannot execute a bounded supplied plan.");
        if (extended && !plan.extended)
            throw new InvalidDataException("Extended acceptance cannot execute a supplied plan without its required depth.");
        if (arguments[0] == "evidence" && options.TryGetValue("--cache-output", out string? retentionArchive) && plan.contexts.ContainsKey("native"))
            Console.WriteLine($"Retained {await NativeArtifactTransfer.ExportAsync(root, retentionArchive, cancellationToken).ConfigureAwait(false)} complete immutable generations; reconciliation must still succeed independently.");
        if (planPath is not null && plan.environment != await ValidationEnvironment.CaptureAsync(root, plan.dotnet, cancellationToken, plan.contexts.Values).ConfigureAwait(false))
            throw new InvalidDataException("The supplied plan belongs to another SDK or execution environment.");
        await File.WriteAllTextAsync(Path.Combine(output, "plan.json"), JsonSerializer.Serialize(plan,
            new JsonSerializerOptions { WriteIndented = true }), cancellationToken).ConfigureAwait(false);
        if (arguments[0] == "plan")
        {
            Console.WriteLine($"Plan: {plan.owners.Length} affected owners, {plan.checks.Count(static check => check.exclusion is null)} required checks. {output}");
            return 0;
        }
        ValidationEvidence? previous = options.TryGetValue("--evidence", out string? evidencePath) ? Read<ValidationEvidence>(evidencePath) : null;
        if (arguments[0] == "evidence")
        {
            if (previous is null)
                throw new ArgumentException("Evidence reconciliation requires --evidence FILE.");
            using DocumentationSnapshot? documentation = plan.documentation is null ? null
                : await DocumentationStore.OpenAsync(plan.documentation.cacheRoot, plan.documentation.snapshot,
                    cancellationToken: cancellationToken).ConfigureAwait(false);
            if (documentation is not null)
                _ = await ValidationDocumentation.ReadCatalogAsync(plan, documentation, cancellationToken).ConfigureAwait(false);
            using var outputOwnership = plan.checks.Any(static check => check.kind == "build" && check.exclusion is null)
                ? await ValidationWorkspace.AcquireReadAsync(plan, cancellationToken).ConfigureAwait(false) : null;
            Dictionary<string, string> current = await ValidationInputs.CaptureAsync(root, cancellationToken).ConfigureAwait(false);
            if (ValidationInputs.Identity(current) != plan.tree || previous.tree != plan.tree || previous.environment != plan.environment)
                throw new InvalidDataException("Evidence does not certify the current tree and environment.");
            ValidateResults(plan, previous);
            var outputDigests = new Dictionary<string, string>(StringComparer.Ordinal);
            var dependencies = new ValidationDependencyInputs(plan, cancellationToken);
            foreach (ValidationCheck check in plan.checks.Where(static check => check.exclusion is null))
                if (!ValidationOutputs.Matches(plan, check, previous.results.Single(result => result.id == check.id), cancellationToken, outputDigests)
                    || !dependencies.Matches(check, previous.results.Single(result => result.id == check.id).dependencyIdentity))
                    throw new InvalidDataException($"Required output evidence is missing or changed: {check.id}");
            return 0;
        }
        if (options.TryGetValue("--cache-input", out string? cacheInput) && File.Exists(cacheInput))
        {
            try
            {
                int imported = await NativeArtifactTransfer.ImportAsync(root, cacheInput, cancellationToken).ConfigureAwait(false);
                Console.WriteLine($"Restored {imported} complete native candidates; publishers must verify their requested input identities before adoption.");
            }
            catch (IOException failure)
            {
                File.Copy(cacheInput, Path.Combine(output, "rejected-native-cache.zip"), overwrite: false);
                Console.Error.WriteLine("Rejected native cache transport; retained for diagnosis. Required producers will execute normally: " + failure.Message);
            }
        }
        ValidationEvidence evidence = await ValidationRunner.RunAsync(plan, output, previous, cancellationToken).ConfigureAwait(false);
        ValidateResults(plan, evidence);
        if (options.TryGetValue("--cache-output", out string? cacheOutput) && plan.contexts.ContainsKey("native"))
            Console.WriteLine($"Exported {await NativeArtifactTransfer.ExportAsync(root, cacheOutput, cancellationToken).ConfigureAwait(false)} verified immutable generations.");
        return 0;
    }

    private static T Read<T>(string path)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(path)) ?? throw new InvalidDataException($"Empty validation document: {path}");

    private static void ValidateResults(
        ValidationPlan plan,
        ValidationEvidence evidence
    ) {
        if (!evidence.completed)
            throw new InvalidDataException("Validation did not complete its closing input, environment and output verification.");
        if (evidence.planIdentity != ValidationInputs.PlanIdentity(plan))
            throw new InvalidDataException("Evidence does not match the exact selected plan.");
        foreach (ValidationCheck check in plan.checks)
        {
            ValidationResult[] results = evidence.results.Where(result => result.id == check.id).ToArray();
            if (results.Length != 1 || results[0].inputIdentity != ValidationInputs.CheckIdentity(plan, check, plan.inputs)
                || (check.exclusion is null ? results[0].status is not ("Passed" or "Reused") : results[0].status != "NotApplicable"))
                throw new InvalidDataException($"Required validation evidence is missing, stale, duplicated or unsuccessful: {check.id}");
        }
        if (evidence.results.Any(static result => result.status is "Failed" or "Blocked") || evidence.results.Length != plan.checks.Length)
            throw new InvalidDataException("Validation contains failed, blocked or unplanned results.");
    }
}
