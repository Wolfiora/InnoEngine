using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

/// <summary>
/// Provides repository validation planning, execution and evidence reconciliation to the existing Build entry.
/// </summary>
public static class ValidationCommand
{
    /// <summary>
    /// Executes planning, checks, evidence reconciliation, shard joining or exact functional acceptance comparison.
    /// </summary>
    /// <param name="arguments">
    /// Pack-backends writes the selected staging inventory into a platform archive. Import-backends verifies an archive digest and target before installation.
    /// The operation followed by explicit engine-root, base, dotnet, configuration, artifacts-path, output, plan,
    /// evidence, full, extended, cache-input or cache-output options. Cache archives contain immutable native payloads only.
    /// Explicit docs-cache-root and docs-snapshot options select protected external documentation facts.
    /// A baseline-plan supplies the accepted SDK graph for ownership of removed inputs at the exact merge base.
    /// Optional docs-base-snapshot identifies changed pages and facts without rescanning an authoring checkout.
    /// Profile selects execution depth; native-dependencies selects immutable consume-only dependencies.
    /// Shard and prerequisites select a frozen partition and its completed receipts. Join validates every partition
    /// without recapturing source inputs or executing checks.
    /// Acceptance compares a freshly captured full plan with accepted-plan and evidence from an authenticated producer.
    /// Resume requires accepted-plan and evidence from an authenticated default-branch producer. It verifies retained
    /// output and dependency bytes and executes missing checks; the caller owns remote provenance authentication.
    /// Retained-outputs supplies authenticated compiled files for a fresh workspace; restoration occurs under its writer lease.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels child processes while retaining the first failure and completed diagnostics.
    /// </param>
    /// <returns>
    /// Zero when the requested operation succeeds, one for unsuccessful execution, or two when planning requires
    /// explicitly prepared inputs. A blocked preparation report is written to preparation.json instead of producing
    /// a new executable plan. Rebinding a frozen plan returns three when the runner environment requires fresh planning.
    /// The command never fetches native source inputs automatically.
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
        if (arguments.Length == 0
            || arguments[0] is not ("plan"
                                    or "run"
                                    or "full"
                                    or "evidence"
                                    or "join"
                                    or "acceptance"
                                    or "resume"
                                    or "pack-backends"
                                    or "import-backends"
                                    or "backend-inputs"
                                    or "bind-backends")
        ) {
            throw new ArgumentException(
                "Usage: validate <plan|run|full|evidence|join|acceptance|resume|pack-backends|import-backends|backend-inputs|bind-backends> --engine-root ROOT --dotnet HOST [--base REV] [--output DIR] [--plan FILE] [--accepted-plan FILE] [--evidence FILE] [--profile NAME] [--native-dependencies FILE] [--shard ID] [--prerequisites DIR]"
            );
        }
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        string[]? transportOptions = arguments[0] switch
        {
            "bind-backends" => ["--plan", "--engine-root", "--dotnet", "--packages", "--output"],
            "backend-inputs" => ["--plan", "--engine-root", "--dotnet", "--target", "--output"],
            "pack-backends" => ["--artifacts-path", "--inventory", "--output"],
            "import-backends" => ["--archive", "--sha256", "--target", "--output"],
            _ => null
        };
        for (int index = 1; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            bool allowed = transportOptions?.Contains(key, StringComparer.Ordinal)
                           ?? key is ("--engine-root"
                                      or "--dotnet"
                                      or "--base"
                                      or "--configuration"
                                      or "--artifacts-path"
                                      or "--output"
                                      or "--plan"
                                      or "--accepted-plan"
                                      or "--evidence"
                                      or "--extended"
                                      or "--full"
                                      or "--cache-input"
                                      or "--cache-output"
                                      or "--docs-cache-root"
                                      or "--docs-snapshot"
                                      or "--docs-base-snapshot"
                                      or "--profile"
                                      or "--native-dependencies"
                                      or "--shard"
                                      or "--prerequisites"
                                      or "--retained-outputs"
                                      or "--backend-store");
            if (arguments[0] == "backend-inputs" && key is ("--selection" or "--configuration"))
            {
                allowed = true;
            }
            if (arguments[0] == "plan" && key == "--baseline-plan")
            {
                allowed = true;
            }
            if (arguments[0] == "bind-backends" && key == "--test-evidence")
            {
                allowed = true;
            }
            if (!allowed || index + 1 >= arguments.Length || !options.TryAdd(key, arguments[index + 1]))
            {
                throw new ArgumentException($"Unknown, duplicate or incomplete validation option: {key}");
            }
        }
        if (transportOptions is not null)
        {
            if (transportOptions.Any(key => !options.ContainsKey(key)))
            {
                throw new ArgumentException("Required backend transport options: " + string.Join(", ", transportOptions));
            }
            if (arguments[0] == "bind-backends")
            {
                string engine = Path.GetFullPath(options["--engine-root"]);
                ValidationPlan frozen = ValidationPlanTransport.Read(options["--plan"], engine, options["--dotnet"]);
                ValidationPlan bound = await ValidationPlanner.BindBackendsAsync(
                    frozen,
                    options["--packages"],
                    cancellationToken,
                    options.GetValueOrDefault("--test-evidence")
                ).ConfigureAwait(false);
                AtomicFile.WriteAllBytes(options["--output"], ValidationPlanTransport.Write(bound));
            }
            else if (arguments[0] == "backend-inputs")
            {
                string engine = Path.GetFullPath(options["--engine-root"]);
                ValidationPlan frozen = ValidationPlanTransport.Read(options["--plan"], engine, options["--dotnet"]);
                byte[]? native = options.TryGetValue("--selection", out string? selected)
                                 ? await File.ReadAllBytesAsync(selected, cancellationToken).ConfigureAwait(false)
                                 : null;
                BackendInputPlan selection = await ValidationPlanner.BackendInputsAsync(
                    frozen,
                    options["--target"],
                    cancellationToken,
                    native,
                    options.GetValueOrDefault("--configuration")
                ).ConfigureAwait(false);
                AtomicFile.WriteAllBytes(options["--output"], JsonSerializer.SerializeToUtf8Bytes(selection));
            }
            else if (arguments[0] == "pack-backends")
            {
                BackendPrebuiltArchive.Create(options["--artifacts-path"], options["--inventory"], options["--output"], cancellationToken);
            }
            else
            {
                await BackendPrebuiltArchive.ExtractAsync(
                    options["--archive"],
                    options["--sha256"],
                    options["--target"],
                    options["--output"],
                    cancellationToken
                ).ConfigureAwait(false);
            }
            return 0;
        }
        string root = Path.GetFullPath(options.GetValueOrDefault("--engine-root", Directory.GetCurrentDirectory()));
        bool extended = options.TryGetValue("--extended", out string? depth) && bool.Parse(depth);
        bool full = arguments[0] == "full" || options.TryGetValue("--full", out string? complete) && bool.Parse(complete);
        ValidationProfile profile = ValidationProfiles.Parse(options.GetValueOrDefault("--profile", full ? "full" : "pull-request"));
        if (profile is (ValidationProfile.Fast or ValidationProfile.Performance) && (full || extended))
        {
            throw new ArgumentException("The selected profile cannot certify full or extended acceptance.");
        }
        string output = Path.GetFullPath(
            options.GetValueOrDefault("--output", Path.Combine(root, "artifacts", "validation", "runs", Guid.NewGuid().ToString("N")))
        );
        string artifacts = Path.GetFullPath(
            options.GetValueOrDefault(
                "--artifacts-path",
                Path.Combine(
                    root,
                    "artifacts",
                    "validation",
                    "workspaces",
                    options.GetValueOrDefault("--configuration", "Debug")
                )
            )
        );
        string dotnet = options.GetValueOrDefault("--dotnet", "dotnet");
        if (options.ContainsKey("--retained-outputs") && arguments[0] != "resume")
        {
            throw new ArgumentException("Retained outputs may only be installed by resume.");
        }
        Directory.CreateDirectory(output);
        if (options.ContainsKey("--plan") && (options.ContainsKey("--native-dependencies") || options.ContainsKey("--backend-store")))
        {
            throw new ArgumentException("The supplied plan already freezes its native dependency selection.");
        }
        if (options.ContainsKey("--plan") && (options.ContainsKey("--docs-cache-root") || options.ContainsKey("--docs-snapshot")
                                              || options.ContainsKey("--docs-base-snapshot"))
        ) {
            throw new ArgumentException("A supplied plan already fixes its documentation selection; regenerate the plan to change it.");
        }
        ValidationPlan plan;
        if (options.TryGetValue("--plan", out string? planPath))
        {
            plan = ValidationPlanTransport.Read(planPath, root, dotnet);
        }
        else
        {
            ValidationPlanningResult selected = await ValidationPlanner.CreateAsync(
                root,
                options.GetValueOrDefault("--base", "HEAD"),
                dotnet,
                options.GetValueOrDefault("--configuration", "Debug"),
                artifacts,
                full,
                cancellationToken,
                extended,
                options.GetValueOrDefault("--docs-cache-root"),
                options.GetValueOrDefault("--docs-snapshot"),
                options.GetValueOrDefault("--docs-base-snapshot"),
                profile,
                options.GetValueOrDefault("--native-dependencies"),
                options.GetValueOrDefault("--backend-store"),
                options.GetValueOrDefault("--baseline-plan")
            ).ConfigureAwait(false);
            if (selected.preparation is not null)
            {
                await File.WriteAllTextAsync(
                    Path.Combine(output, "preparation.json"),
                    JsonSerializer.Serialize(selected.preparation, new JsonSerializerOptions { WriteIndented = true }),
                    cancellationToken
                ).ConfigureAwait(false);
                Console.WriteLine("Planning blocked: " + selected.preparation.reason + " No new executable plan was produced. " + output);
                return 2;
            }
            plan = selected.plan
                   ?? throw new InvalidDataException("Planning produced neither an executable plan nor explicit preparation requirements.");
        }
        if (options.ContainsKey("--profile") && profile != plan.profile)
        {
            throw new InvalidDataException("The supplied plan uses a different execution profile.");
        }
        if (plan.nativeDependencies is not null && (options.ContainsKey("--cache-input") || options.ContainsKey("--cache-output")))
        {
            throw new ArgumentException("Consume-only dependency validation does not use producer cache transport.");
        }
        if (plan.backendStore is not null && plan.backendPackages is null && arguments[0] != "plan")
        {
            throw new InvalidDataException("Backend acquisition must bind the exact packages before executing or certifying this plan.");
        }
        if (arguments[0] == "acceptance")
        {
            if (planPath is null || !options.TryGetValue("--accepted-plan", out string? acceptedPath)
                || !options.TryGetValue("--evidence", out string? acceptedEvidence)
            ) {
                throw new ArgumentException("Acceptance comparison requires current plan, accepted-plan and evidence.");
            }
            ValidationPlan accepted = ValidationPlanTransport.Read(acceptedPath, root, dotnet);
            bool reusable = ValidationAcceptance.Matches(plan, accepted, Read<ValidationEvidence>(acceptedEvidence));
            await File.WriteAllTextAsync(
                Path.Combine(output, "acceptance.json"),
                JsonSerializer.Serialize(new
                {
                    reusable,
                    planIdentity = ValidationInputs.PlanIdentity(plan),
                    acceptedPlanIdentity = ValidationInputs.PlanIdentity(accepted),
                    reason = reusable
                             ? "Exact completed functional inputs, checks and execution environment match."
                             : "Functional inputs, execution environment or acceptance policy changed; execute the current plan."
                }),
                cancellationToken
            ).ConfigureAwait(false);
            return 0;
        }
        if (arguments[0] == "join")
        {
            if (planPath is null || !options.TryGetValue("--prerequisites", out string? receiptsDirectory))
            {
                throw new ArgumentException("Joining shards requires the frozen plan and its receipt directory.");
            }
            ValidationEvidence joined = ValidationShards.Join(plan, ReadReceipts(receiptsDirectory));
            await File.WriteAllTextAsync(
                Path.Combine(output, "results.json"),
                JsonSerializer.Serialize(joined, new JsonSerializerOptions { WriteIndented = true }),
                cancellationToken
            ).ConfigureAwait(false);
            return 0;
        }
        ValidationShard? shard = options.TryGetValue("--shard", out string? shardID) ? ValidationShards.Require(plan, shardID) : null;
        if (shard is not null && arguments[0] is not ("run" or "resume"))
        {
            throw new ArgumentException("A shard is executed through validate run with a frozen plan.");
        }
        if (!Path.GetFullPath(plan.root)
            .Equals(root, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
        ) {
            throw new InvalidDataException("The supplied plan belongs to another workspace.");
        }
        if (plan.checks.Length == 0
            || plan.checks.Select(static check => check.id).Distinct(StringComparer.Ordinal).Count() != plan.checks.Length
        ) {
            throw new InvalidDataException("A validation plan must contain unique, explicit required checks.");
        }
        if (full && !plan.full)
        {
            throw new InvalidDataException("Full validation cannot execute a bounded supplied plan.");
        }
        if (extended && !plan.extended)
        {
            throw new InvalidDataException("Extended acceptance cannot execute a supplied plan without its required depth.");
        }
        if (arguments[0] == "evidence"
            && options.TryGetValue("--cache-output", out string? retentionArchive)
            && plan.contexts.ContainsKey("native")
        ) {
            Console.WriteLine(
                $"Retained {await NativeArtifactTransfer.ExportAsync(root, retentionArchive, cancellationToken).ConfigureAwait(false)} complete immutable generations; reconciliation must still succeed independently."
            );
        }
        if (planPath is not null
            && plan.environment != await ValidationEnvironment.CaptureAsync(
                root,
                plan.dotnet,
                cancellationToken,
                plan.contexts.Values
            ).ConfigureAwait(false)
        ) {
            if (arguments[0] == "plan")
            {
                Console.WriteLine("The execution environment changed. Regenerate the plan on this runner.");
                return 3;
            }
            throw new InvalidDataException("The supplied plan belongs to another SDK or execution environment.");
        }
        await File.WriteAllBytesAsync(
            Path.Combine(output, "plan.json"),
            ValidationPlanTransport.Write(plan),
            cancellationToken
        ).ConfigureAwait(false);
        if (arguments[0] == "plan")
        {
            Console.WriteLine(
                $"Plan: {plan.owners.Length} affected owners, {plan.checks.Count(static check => check.exclusion is null)} required checks. {output}"
            );
            return 0;
        }
        ValidationEvidence? previous = options.TryGetValue("--evidence", out string? evidencePath)
                                       ? Read<ValidationEvidence>(evidencePath)
                                       : null;
        bool continuation = arguments[0] == "resume";
        string? retainedOutputs = options.GetValueOrDefault("--retained-outputs");
        if (continuation)
        {
            if (planPath is null || previous is null || !options.TryGetValue("--accepted-plan", out string? acceptedPath))
            {
                throw new ArgumentException("Resume requires a frozen full plan, accepted-plan and completed evidence.");
            }
            ValidationAcceptance.RequireContinuation(plan, ValidationPlanTransport.Read(acceptedPath, root, dotnet), previous);
            if (previous.environment != plan.environment)
            {
                retainedOutputs = null;
                Console.WriteLine(
                    "The accepted execution environment changed. Retained compilation is not imported; the current frozen checks must execute."
                );
            }
        }
        if (arguments[0] == "evidence")
        {
            if (previous is null)
            {
                throw new ArgumentException("Evidence reconciliation requires --evidence FILE.");
            }
            using DocumentationSnapshot? documentation = plan.documentation is null
                                                         ? null
                                                         : await DocumentationStore.OpenAsync(
                                                             plan.documentation.cacheRoot,
                                                             plan.documentation.snapshot,
                                                             cancellationToken: cancellationToken
                                                         ).ConfigureAwait(false);
            if (documentation is not null)
            {
                _ = await ValidationDocumentation.ReadCatalogAsync(plan, documentation, cancellationToken).ConfigureAwait(false);
            }
            using CoordinatedFileLease? outputOwnership = plan.checks.Any(static check => check.kind == "build" && check.exclusion is null)
                ? await ValidationWorkspace.AcquireReadAsync(plan, cancellationToken).ConfigureAwait(false)
                : null;
            Dictionary<string, string> current = await ValidationInputs.CaptureAsync(root, cancellationToken).ConfigureAwait(false);
            if (ValidationInputs.Identity(current) != plan.tree || previous.tree != plan.tree || previous.environment != plan.environment)
            {
                throw new InvalidDataException("Evidence does not certify the current tree and environment.");
            }
            _ = ValidationShards.ValidateEvidence(plan, previous);
            var outputDigests = new Dictionary<string, string>(StringComparer.Ordinal);
            var dependencies = new ValidationDependencyInputs(plan, cancellationToken);
            foreach (ValidationCheck check in plan.checks.Where(static check => check.exclusion is null))
            {
                if (!ValidationOutputs.Matches(
                    plan,
                    check,
                    previous.results.Single(result => result.id == check.id),
                    cancellationToken,
                    outputDigests
                )
                    || !dependencies.Matches(check, previous.results.Single(result => result.id == check.id).dependencyIdentity)
                ) {
                    throw new InvalidDataException($"Required output evidence is missing or changed: {check.id}");
                }
            }
            return 0;
        }
        if (options.TryGetValue("--cache-input", out string? cacheInput) && File.Exists(cacheInput))
        {
            try
            {
                int imported = await NativeArtifactTransfer.ImportAsync(root, cacheInput, cancellationToken).ConfigureAwait(false);
                Console.WriteLine(
                    $"Restored {imported} complete native candidates; publishers must verify their requested input identities before adoption."
                );
            }
            catch (IOException failure)
            {
                File.Copy(cacheInput, Path.Combine(output, "rejected-native-cache.zip"), overwrite: false);
                Console.Error.WriteLine(
                    "Rejected native cache transport; retained for diagnosis. Required producers will execute normally: " + failure.Message
                );
            }
        }
        ValidationEvidence evidence = await ValidationRunner.RunAsync(
            plan,
            output,
            previous,
            cancellationToken,
            shard,
            options.TryGetValue("--prerequisites", out string? receipts) ? ReadReceipts(receipts) : null,
            continuation,
            retainedOutputs
        ).ConfigureAwait(false);
        _ = ValidationShards.ValidateEvidence(plan, evidence, shard?.id);
        if (options.TryGetValue("--cache-output", out string? cacheOutput) && plan.contexts.ContainsKey("native"))
        {
            Console.WriteLine(
                $"Exported {await NativeArtifactTransfer.ExportAsync(root, cacheOutput, cancellationToken).ConfigureAwait(false)} verified immutable generations."
            );
        }
        return 0;
    }

    private static ValidationEvidence[] ReadReceipts(string directory)
        => Directory.GetFiles(directory, "results.json", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .Select(Read<ValidationEvidence>)
            .ToArray();

    private static T Read<T>(string path)
        => JsonSerializer.Deserialize<T>(File.ReadAllText(path)) ?? throw new InvalidDataException($"Empty validation document: {path}");
}
