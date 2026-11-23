using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Tooling.Architecture;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

internal static class ValidationRunner
{
    internal const string C_TEST_EXECUTOR = "tools/ci/Invoke-IsolatedTests.ps1";

    internal static async Task<ValidationEvidence> RunAsync(
        ValidationPlan plan,
        string directory,
        ValidationEvidence? previous,
        ValidationShard? shard = null,
        IReadOnlyList<ValidationEvidence>? prerequisites = null,
        bool continuation = false,
        string? retainedOutputs = null,
        CancellationToken cancellationToken = default
    ) {
        HashSet<string>? selectedChecks = shard?.checks.ToHashSet(StringComparer.Ordinal);
        ValidationCheck[] checks = plan.checks.Where(check => selectedChecks is null || selectedChecks.Contains(check.id)).ToArray();
        ValidationResult[] imported = shard is null ? [] : ValidationShards.Prerequisites(plan, shard, prerequisites ?? []);
        using DocumentationSnapshot? documentationSnapshot = plan.documentation is null
                                                             ? null
                                                             : await DocumentationStore.OpenAsync(
                                                                 plan.documentation.cacheRoot,
                                                                 plan.documentation.snapshot,
                                                                 cancellationToken: cancellationToken
                                                             ).ConfigureAwait(false);
        DocumentationCatalogReport? documentationCatalog = documentationSnapshot is null
            ? null
            : await ValidationDocumentation.ReadCatalogAsync(plan, documentationSnapshot, cancellationToken).ConfigureAwait(false);
        var results = new List<ValidationResult>(imported);
        List<ValidationRunFailure> failures = [];
        double closingSeconds = 0;
        var batchSeconds = new Dictionary<string, double>(StringComparer.Ordinal);
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string planIdentity = ValidationInputs.PlanIdentity(plan);
        Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(
            Path.Combine(directory, "execution-plan.json"),
            JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }),
            cancellationToken
        ).ConfigureAwait(false);
        Console.WriteLine("Validation phase: acquire the selected workspace ownership and freeze current inputs.");
        using CoordinatedFileLease? workspace = checks.Any(static check => check.kind == "build" && check.exclusion is null)
                                                ? await ValidationWorkspace.AcquireAsync(plan, cancellationToken).ConfigureAwait(false)
                                                : null;
        if (retainedOutputs is not null)
        {
            if (!continuation || workspace is null)
            {
                throw new InvalidDataException("Restoring compilation outputs requires a continuation with workspace ownership.");
            }
            ValidationWorkspace.RestoreOutputs(plan, retainedOutputs, cancellationToken);
        }
        foreach (KeyValuePair<string, ValidationContext> context in plan.contexts)
        {
            await File.WriteAllTextAsync(
                Path.Combine(directory, context.Key + "-sdk-graph.json"),
                context.Value.sdkGraph.GetRawText(),
                cancellationToken
            ).ConfigureAwait(false);
        }
        Dictionary<string, string> current = await ValidationInputs.CaptureAsync(plan.root, cancellationToken).ConfigureAwait(false);
        if (ValidationInputs.Identity(current) != plan.tree)
        {
            string[] changed = current.Keys.Union(plan.inputs.Keys).Where(path => (
                !path.StartsWith('@')
                && current.GetValueOrDefault(path) != plan.inputs.GetValueOrDefault(path)
            )).Order(StringComparer.Ordinal).Take(8).ToArray();
            throw new InvalidDataException(
                "Validation plan inputs changed before execution. Generate a new plan. Changed inputs: " + string.Join(", ", changed)
            );
        }
        Dictionary<string, string> buildSolutions = checks.Where(static check => check.kind == "build" && check.exclusion is null)
            .GroupBy(static check => check.context!, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                group => WriteUnion(group.Key, "Build", group.Select(static check => check.owner).ToArray()),
                StringComparer.Ordinal
            );
        if (retainedOutputs is not null)
        {
            // Restore rebinds SDK-generated absolute paths to this runner and obtains the exact
            // packages before comparing dependency identities. It does not compile the union.
            foreach (KeyValuePair<string, string> solution in buildSolutions)
            {
                await ValidationProcess.RunAsync(
                    plan.dotnet,
                    [
                        "restore",
                        solution.Value,
                        ..plan.contexts[solution.Key].properties.Select(static pair => "-p:" + pair.Key + "=" + pair.Value),
                        "--disable-parallel",
                        "-nr:false"
                    ],
                    plan.root,
                    Path.Combine(directory, solution.Key + "-restore-retained.log"),
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
            }
        }
        var built = new HashSet<string>(StringComparer.Ordinal);
        var tested = new HashSet<string>(StringComparer.Ordinal);
        var executedTests = new HashSet<string>(StringComparer.Ordinal);
        var buildFailures = new Dictionary<string, string>(StringComparer.Ordinal);
        var pendingCompilations = new Dictionary<string, ValidationBuildInputs>(StringComparer.Ordinal);
        var testFailures = new Dictionary<string, string>(StringComparer.Ordinal);
        var testDetails = new Dictionary<string, (bool passed, string detail)>(StringComparer.Ordinal);
        var outputDigests = new Dictionary<string, string>(StringComparer.Ordinal);
        var dependencyInputs = new ValidationDependencyInputs(plan, cancellationToken);
        var compilationGraphs = new Dictionary<string, string>(StringComparer.Ordinal);
        HashSet<string>? invalidated = ValidationShards.InvalidatedChecks(plan, previous);
        var reused = new Dictionary<string, ValidationResult>(StringComparer.Ordinal);
        var reusableCompilations = new Dictionary<string, bool>(StringComparer.Ordinal);
        foreach (ValidationCheck check in checks)
        {
            Console.WriteLine($"Validation check: {check.id}; kind={check.kind}; context={check.context ?? "none"}; owner={check.owner}.");
            string identity = ValidationInputs.CheckIdentity(plan, check, current);
            string contextID = check.context ?? "managed";
            string graphPath = Path.Combine(directory, contextID + "-sdk-graph.json");
            string status = "Failed";
            string detail = string.Empty;
            Dictionary<string, string>? outputs = null;
            string dependencyIdentity = string.Empty;
            ValidationDocumentationInput[]? documentationInputs = null;
            Stopwatch elapsed = Stopwatch.StartNew();
            if (check.exclusion is not null)
            {
                status = "NotApplicable";
                detail = check.exclusion;
            }
            else if (cancellationToken.IsCancellationRequested)
            {
                status = "Blocked";
                detail = "Validation was canceled before this required check completed.";
            }
            else if (check.prerequisites.Any(
                prerequisite => !results.Any(result => result.id == prerequisite && result.status is "Passed" or "Reused")
            )) {
                status = "Blocked";
                detail = "Required prerequisites did not pass: " + string.Join(
                    ", ",
                    check.prerequisites
                        .Select(prerequisite => (
                            id: prerequisite,
                            status: results.SingleOrDefault(result => result.id == prerequisite)?.status ?? "Missing"
                        ))
                        .Where(static prerequisite => prerequisite.status is not ("Passed" or "Reused"))
                        .Select(static prerequisite => prerequisite.id + " (" + prerequisite.status + ")")
                ) + ". Dependent work was not executed.";
            }
            else if (CanReuse(check))
            {
                ValidationResult old = reused[check.id];
                status = "Reused";
                detail = check.kind == "test" ? old.detail : continuation
                    ? "Exact retained functional evidence and output bytes match; source plan: " + previous!.planIdentity
                    : "Exact local deterministic input identity matches retained successful evidence.";
                outputs = old.outputs;
                dependencyIdentity = old.dependencyIdentity;
                documentationInputs = old.documentationInputs;
                if (check.kind == "build")
                {
                    built.Add(contextID);
                }
            }
            else
            {
                try
                {
                    switch (check.kind)
                    {
                        case "release-bundle":
                            ValidationDeliverables.Create(plan, check, cancellationToken);
                            break;
                        case "cold-inputs":
                            foreach (string cache in new[] { "artifacts/native", "artifacts/bindings" })
                            {
                                string path = Path.Combine(plan.root, cache);
                                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any())
                                {
                                    throw new InvalidDataException(
                                        $"Cold validation requires a fresh checkout/cache owner; existing artifacts cannot be relabeled cold: {path}"
                                    );
                                }
                            }
                            break;
                        case "hot-builds":
                            await ShellAsync("tools/ci/Invoke-BuildCacheChecks.ps1", [
                                "-Dotnet",
                                plan.dotnet,
                                "-PlanPath",
                                Path.Combine(directory, "execution-plan.json"),
                                "-Context",
                                contextID,
                                "-ResultsDirectory",
                                Path.Combine(directory, "hot-builds"),
                                "-Iterations",
                                "10"
                            ], "hot-builds.log").ConfigureAwait(false);
                            break;
                        case "instructions":
                            await ShellAsync(ValidationCiContracts.C_RULE_LOADER, [], "instructions.log").ConfigureAwait(false);
                            break;
                        case "gate-contracts":
                        case "ci-contracts":
                            string[] contracts = check.kind == "gate-contracts"
                                ? ValidationCiContracts.GATE_CONTRACTS.Select(path => Path.Combine(plan.root, path)).ToArray()
                                : ValidationCiContracts.Discover(plan.root);
                            if (contracts.Length == 0)
                            {
                                throw new InvalidDataException("Required CI contract discovery is empty.");
                            }
                            foreach (string contract in contracts)
                            {
                                await ShellAsync(
                                    Path.GetRelativePath(plan.root, contract),
                                    [],
                                    Path.GetFileNameWithoutExtension(contract) + ".log"
                                ).ConfigureAwait(false);
                            }
                            break;
                        case "restore-aot":
                            {
                                JsonElement player = plan.contexts[contextID].sdkGraph.EnumerateArray().Single(snapshot => (
                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!)
                                        .Replace('\\', '/') == check.owner
                                ));
                                string runtime = player.GetProperty("Properties").GetProperty("NETCoreSdkRuntimeIdentifier").GetString()!;
                                if (string.IsNullOrWhiteSpace(runtime))
                                {
                                    throw new InvalidDataException("Host NativeAOT restore requires the evaluated SDK runtime identifier.");
                                }
                                await ValidationProcess.RunAsync(
                                    plan.dotnet,
                                    [
                                        "restore",
                                        Path.Combine(plan.root, check.owner),
                                        "--runtime",
                                        runtime,
                                        "-p:Configuration=Release",
                                        "-p:PublishAot=true",
                                        "-p:ArtifactsPath=" + Path.Combine(plan.artifacts, "restore", "native-aot"),
                                        "--disable-parallel",
                                        "-nr:false"
                                    ],
                                    plan.root,
                                    Path.Combine(directory, "native-aot-restore.log"),
                                    cancellationToken: cancellationToken
                                ).ConfigureAwait(false);
                            }
                            break;
                        case "architecture":
                            string[] changedSources = [];
                            if (check.changedSources is not null)
                            {
                                string changedSourcesPath = Path.Combine(directory, "architecture-changed-sources.txt");
                                await File.WriteAllLinesAsync(changedSourcesPath, check.changedSources, cancellationToken)
                                    .ConfigureAwait(false);
                                changedSources = ["--paths-from", changedSourcesPath];
                            }
                            if (ArchitectureValidator.Execute([
                                plan.root,
                                "--configuration",
                                plan.configuration,
                                "--source-only",
                                "--dotnet",
                                plan.dotnet,
                                "--sdk-snapshot",
                                graphPath,
                                .. changedSources
                            ], cancellationToken) != 0
                            ) {
                                throw new InvalidDataException("Repository architecture validation failed.");
                            }
                            break;
                        case "api":
                            if (!built.Contains(contextID))
                            {
                                status = "Blocked";
                                detail = "Compiled API verification requires the successful union build.";
                                break;
                            }
                            if (check.projects is not { Length: > 0 })
                            {
                                throw new InvalidDataException("Required compiled API owner selection is empty.");
                            }
                            string[] projects = check.projects.SelectMany(static project => new[] { "--api-project", project }).ToArray();
                            if (ArchitectureValidator.Execute([
                                plan.root,
                                "--metadata-only",
                                "--configuration",
                                plan.contexts[contextID].properties["Configuration"],
                                "--dotnet",
                                plan.dotnet,
                                "--artifacts-path",
                                plan.contexts[contextID].properties["ArtifactsPath"],
                                "--sdk-snapshot",
                                await ResolveCompilationGraphAsync(contextID).ConfigureAwait(false),
                                ..projects
                            ], cancellationToken) != 0
                            ) {
                                throw new InvalidDataException("Affected compiled API boundaries failed.");
                            }
                            break;
                        case "documentation-missing":
                            status = "Blocked";
                            detail = check.reason;
                            if (check.projects is { Length: > 0 } captureOwners && check.documentation is not null)
                            {
                                string captureDirectory = Path.Combine(directory, "documentation-captures");
                                Directory.CreateDirectory(captureDirectory);
                                string capturePath = Path.Combine(
                                    captureDirectory,
                                    Path.GetFileNameWithoutExtension(check.owner) + ".json"
                                );
                                string[] captureTarget = check.documentation.target is string selectedTarget
                                                         ? ["--target", selectedTarget]
                                                         : [];
                                DocumentationReferences.ExportApi([
                                    "--engine-root",
                                    plan.root,
                                    "--dotnet",
                                    plan.dotnet,
                                    "--configuration",
                                    check.documentation.configuration,
                                    "--artifacts-path",
                                    plan.contexts[contextID].properties["ArtifactsPath"],
                                    "--sdk-snapshot",
                                    await ResolveCompilationGraphAsync(contextID).ConfigureAwait(false),
                                    "--output",
                                    capturePath,
                                    ..captureTarget,
                                    ..captureOwners.SelectMany(static owner => new[] { "--project", owner })
                                ], cancellationToken);
                                detail += " Unverified source facts retained at " + capturePath + "; review and compare the resulting documentation in a subsequent validation.";
                            }
                            break;
                        case "documentation":
                            if (documentationSnapshot is null)
                            {
                                throw new InvalidDataException("Required external documentation snapshot is unavailable.");
                            }
                            documentationCatalog ??= await documentationSnapshot.ExportCatalogAsync(
                                cancellationToken
                            ).ConfigureAwait(false);
                            string[] missing = plan.inputSets.Keys.Where(static path => path.EndsWith(".csproj", StringComparison.Ordinal))
                                // Platform implementations remain source provenance, not reader-facing documentation owners.
                                .Where(static path => !path.StartsWith("platforms/", StringComparison.Ordinal))
                                .Select(static path => Path.GetFileNameWithoutExtension(path))
                                .Where(name => !documentationCatalog.projects.TryGetValue(name, out string? entry) || entry is null)
                                .ToArray();
                            if (missing.Length != 0)
                            {
                                throw new InvalidDataException(
                                    "Required canonical project entries are missing: " + string.Join(", ", missing)
                                );
                            }
                            if (documentationCatalog.contentIdentity != check.documentation!.contentIdentity)
                            {
                                throw new InvalidDataException("Documentation content does not match the frozen plan.");
                            }
                            break;
                        case "documentation-api":
                            ValidationDocumentationCheck referenceCheck = check.documentation
                                ?? throw new InvalidDataException("Required reference selection is absent.");
                            if (plan.documentation is null || check.projects is not { Length: > 0 })
                            {
                                throw new InvalidDataException("Required source reference discovery is empty.");
                            }
                            string[] referenceArguments = referenceCheck.references.SelectMany(static path => new[] { "--reference", path })
                                .Concat(check.projects.SelectMany(static project => new[] { "--project", project }))
                                .ToArray();
                            if (await DocumentationReferences.ValidateApiAsync([
                                "--engine-root",
                                plan.root,
                                "--dotnet",
                                plan.dotnet,
                                "--cache-root",
                                plan.documentation.cacheRoot,
                                "--snapshot",
                                plan.documentation.snapshot,
                                "--artifacts-path",
                                plan.contexts[contextID].properties["ArtifactsPath"],
                                "--sdk-snapshot",
                                await ResolveCompilationGraphAsync(contextID).ConfigureAwait(false),
                                "--output",
                                Path.Combine(directory, check.id.Replace(':', '-') + ".json"),
                                ..referenceArguments
                            ], cancellationToken).ConfigureAwait(false)
                                != 0
                            ) {
                                throw new InvalidDataException("Selected documentation source references differ from current source.");
                            }
                            break;
                        case "agent-documents":
                            if (ArchitectureValidator.Execute([plan.root, "--agent-documents-only"], cancellationToken) != 0)
                            {
                                throw new InvalidDataException("Agent document structure or links failed.");
                            }
                            break;
                        case "documentation-examples":
                            if (documentationSnapshot is null || check.documentation is null || check.documentation.pages.Length == 0)
                            {
                                throw new InvalidDataException("Required canonical example discovery is empty.");
                            }
                            documentationCatalog ??= await documentationSnapshot.ExportCatalogAsync(
                                cancellationToken
                            ).ConfigureAwait(false);
                            string[] pages = check.documentation.pages.SelectMany(static page => new[] { "--page", page }).ToArray();
                            List<string> scriptArguments = [];
                            if (documentationCatalog.pages.Any(page => (
                                check.documentation.pages.Contains(page.id, StringComparer.Ordinal)
                                && page.examples?.mode is "runtime-script" or "editor-script"
                            ))
                            ) {
                                string entryContext = checks.Single(candidate => (
                                    candidate.kind == "build"
                                    && candidate.owner == "entry/InnoEngine.Build/InnoEngine.Build.csproj"
                                    && check.prerequisites.Contains(candidate.id, StringComparer.Ordinal)
                                )).context!;
                                JsonElement snapshot = plan.contexts[entryContext].sdkGraph;
                                string entry = snapshot
                                    .EnumerateArray()
                                    .Single(node => Path.GetFileName(node.GetProperty("Path").GetString()) == "InnoEngine.Build.csproj")
                                    .GetProperty("Properties")
                                    .GetProperty("TargetPath")
                                    .GetString()!;
                                string project = Path.Combine(directory, contextID + "-documentation-project");
                                Directory.CreateDirectory(Path.Combine(project, "Assets"));
                                await File.WriteAllTextAsync(
                                    Path.Combine(project, "Assets", "DocumentationProbe.cs"),
                                    "public sealed class DocumentationProbe { }",
                                    cancellationToken
                                ).ConfigureAwait(false);
                                await ValidationProcess.RunAsync(
                                    plan.dotnet,
                                    [
                                        entry,
                                        "scripts",
                                        "--engine-root",
                                        plan.root,
                                        "--dotnet",
                                        plan.dotnet,
                                        "--project",
                                        project,
                                        "--output",
                                        Path.Combine(project, "Scripts"),
                                        "--ide"
                                    ],
                                    plan.root,
                                    Path.Combine(directory, contextID + "-documentation-projection.log"),
                                    cancellationToken: cancellationToken
                                ).ConfigureAwait(false);
                                XDocument projection = XDocument.Load(Path.Combine(project, "Inno.EditorScripts.csproj"));
                                foreach (string scope in new[] { "Runtime", "Editor" })
                                {
                                    XElement reference = projection
                                        .Descendants("Reference")
                                        .Single(element => (string?)element.Attribute("Include") == "Inno.ScriptApi." + scope);
                                    string path = Path.GetFullPath(reference.Element("HintPath")!.Value, project);
                                    if (!File.Exists(path))
                                    {
                                        throw new InvalidDataException(
                                            $"Current Documentation script projection reference is missing: {path}"
                                        );
                                    }
                                    scriptArguments.AddRange(["--" + scope.ToLowerInvariant() + "-reference", path]);
                                }
                            }
                            string[] targetArguments = check.documentation.target is string exampleTarget
                                                       ? ["--target", exampleTarget]
                                                       : [];
                            if (await DocumentationExamples.ExecuteAsync([
                                "--engine-root",
                                plan.root,
                                "--configuration",
                                plan.configuration,
                                "--dotnet",
                                plan.dotnet,
                                "--cache-root",
                                plan.documentation!.cacheRoot,
                                "--snapshot",
                                plan.documentation.snapshot,
                                "--artifacts-path",
                                plan.contexts[contextID].properties["ArtifactsPath"],
                                "--sdk-snapshot",
                                await ResolveCompilationGraphAsync(contextID).ConfigureAwait(false),
                                "--output",
                                Path.Combine(directory, contextID + "-documentation-examples.json"),
                                ..targetArguments,
                                ..scriptArguments,
                                ..pages
                            ], cancellationToken).ConfigureAwait(false) != 0
                            ) {
                                throw new InvalidDataException(
                                    "Selected documentation examples failed or require unavailable compilation inputs."
                                );
                            }
                            break;
                        case "products":
                            {
                                JsonElement graph = plan.contexts[contextID].sdkGraph;
                                JsonElement editor = graph.EnumerateArray().Single(snapshot => (
                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!)
                                        .Replace('\\', '/') == check.owner
                                ));
                                string assembly = editor.GetProperty("Properties").GetProperty("TargetPath").GetString()!;
                                string backendPattern = editor
                                    .GetProperty("Properties")
                                    .GetProperty("InnoValidationBackendPattern")
                                    .GetString()!;
                                if (string.IsNullOrWhiteSpace(backendPattern))
                                {
                                    throw new InvalidDataException(
                                        "Product acceptance requires its platform-owned renderer startup contract."
                                    );
                                }
                                string productDirectory = Path.Combine(directory, check.id);
                                Directory.CreateDirectory(productDirectory);
                                // Each product step starts the Editor on its own empty project inside the results.
                                string project = Path.Combine(productDirectory, "Smoke");
                                await ShellAsync(
                                    "tests/product/Invoke-EditorSmoke.ps1",
                                    [
                                        "-EditorAssembly",
                                        assembly,
                                        "-ProjectDirectory",
                                        project,
                                        "-BackendPattern",
                                        backendPattern,
                                        "-Dotnet",
                                        plan.dotnet,
                                        "-Frames",
                                        "300",
                                        "-HiddenWindow"
                                    ],
                                    check.id
                                    + "-smoke.log"
                                ).ConfigureAwait(false);
                                string[] renderers = Directory.GetFiles(Path.Combine(project, "Logs"), "*.log").SelectMany(path =>
                                    Regex
                                        .Matches(File.ReadAllText(path), @"Rendering initialized with (\w+)")
                                        .Select(static match => match.Groups[1].Value)).Distinct(StringComparer.Ordinal).ToArray();
                                if (renderers.Length != 1)
                                {
                                    throw new InvalidDataException("Product smoke did not identify one actual initialized renderer.");
                                }
                                foreach (string scenario in new[] { "Workflows", "Lifecycle" })
                                {
                                    await ShellAsync(
                                        "tests/product/Invoke-EditorUiProductJourney.ps1",
                                        [
                                            "-EditorAssembly",
                                            assembly,
                                            "-ResultsDirectory",
                                            Path.Combine(productDirectory, scenario),
                                            "-Dotnet",
                                            plan.dotnet,
                                            "-GraphicsApi",
                                            renderers[0],
                                            "-Scenario",
                                            scenario,
                                            ..(plan.nativeDependencies is null
                                               ? Array.Empty<string>()
                                               : ["-NativeDependencies", Path.Combine(plan.root, plan.nativeDependencies)]),
                                            ..(plan.extended && scenario == "Lifecycle"
                                               ? new[] { "-Cycles", "1000", "-Frames", "180000", "-TimeoutSeconds", "5400" }
                                               : [])
                                        ],
                                        check.id + "-" + scenario + ".log"
                                    ).ConfigureAwait(false);
                                }
                            }
                            break;
                        case "support-pack":
                            {
                                string entry = plan.contexts[contextID].sdkGraph.EnumerateArray()
                                    .Single(snapshot => (
                                        Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!)
                                            .Replace('\\', '/') == check.owner
                                    ))
                                    .GetProperty("Properties")
                                    .GetProperty("TargetPath")
                                    .GetString()!;
                                string deliverables = Path.Combine(plan.artifacts, "deliverables");
                                Directory.CreateDirectory(deliverables);
                                await ValidationProcess.RunAsync(
                                    plan.dotnet,
                                    [
                                        entry,
                                        "support-pack",
                                        "--engine-root",
                                        plan.root,
                                        "--tools-target",
                                        plan.contexts[contextID].properties["InnoToolTarget"],
                                        "--dotnet",
                                        plan.dotnet,
                                        "--configuration",
                                        "Release",
                                        "--archive",
                                        Path.Combine(deliverables, "PlayerSupportPack.zip"),
                                        ..plan.nativeDependencies is null
                                          ? Array.Empty<string>()
                                          : ["--native-dependencies", Path.Combine(plan.root, plan.nativeDependencies)]
                                    ],
                                    plan.root,
                                    Path.Combine(directory, "support-pack.log"),
                                    cancellationToken: cancellationToken
                                ).ConfigureAwait(false);
                                await ShellAsync(
                                    "tools/ci/Test-SupportPackClosure.ps1",
                                    ["-Archive", Path.Combine(deliverables, "PlayerSupportPack.zip")],
                                    "support-pack-closure.log"
                                ).ConfigureAwait(false);
                            }
                            break;
                        case "build":
                            if (buildFailures.TryGetValue(contextID, out string? buildFailure))
                            {
                                throw new InvalidOperationException(buildFailure);
                            }
                            if (!built.Contains(contextID))
                            {
                                string solution = buildSolutions[contextID];
                                try
                                {
                                    var compilationInputs = new ValidationBuildInputs(
                                        plan,
                                        contextID,
                                        outputDigests,
                                        cancellationToken
                                    );
                                    string[] properties = plan.contexts[contextID].properties
                                        .Select(static pair => "-p:" + pair.Key + "=" + pair.Value)
                                        .ToArray();
                                    bool retainDependencies = !compilationInputs.RequiresDependencyCompilation(dependencyInputs);
                                    // Rebuild interleaves Clean and Build across root instances. Drain the one owned clean phase
                                    // before any compiler starts.
                                    if (compilationInputs.RequiresCompilation(dependencyInputs))
                                    {
                                        await ValidationProcess.RunAsync(
                                            plan.dotnet,
                                            [
                                                "clean",
                                                solution,
                                                ..properties,
                                                "-p:InnoPrepareProductNative=false",
                                                "-p:InnoTaskBootstrapOwner="
                                                + Guid.NewGuid().ToString("N"),
                                                ..retainDependencies
                                                  ? ["-p:BuildProjectReferences=false", "-p:InnoCleanProjectReferences=false"]
                                                  : Array.Empty<string>(),
                                                "--disable-build-servers",
                                                "-m:1",
                                                "-nr:false"
                                            ],
                                            plan.root,
                                            Path.Combine(directory, contextID + "-clean.log"),
                                            cancellationToken: cancellationToken
                                        ).ConfigureAwait(false);
                                    }
                                    await ValidationProcess.RunAsync(
                                        plan.dotnet,
                                        [
                                            "build",
                                            solution,
                                            ..properties,
                                            "-p:InnoTaskBootstrapOwner=" + Guid.NewGuid().ToString("N"),
                                            ..retainDependencies
                                              ? ["-p:InnoUseVerifiedProjectOutputs=true"]
                                              : Array.Empty<string>(),
                                            "--disable-build-servers",
                                            "-m:"
                                            + (contextID == "managed" ? Math.Min(Environment.ProcessorCount, 4) : 1),
                                            "-nr:false",
                                            "-clp:PerformanceSummary"
                                        ],
                                        plan.root,
                                        Path.Combine(directory, contextID + "-build.log"),
                                        cancellationToken: cancellationToken
                                    ).ConfigureAwait(false);
                                    built.Add(contextID);
                                    outputDigests.Clear();
                                    dependencyInputs.Reset();
                                    pendingCompilations.Add(contextID, compilationInputs);
                                }
                                catch (Exception failure) when (failure is not OperationCanceledException)
                                {
                                    buildFailures[contextID] = failure.Message;
                                    throw;
                                }
                            }
                            break;
                        case "test":
                            if (!built.Contains(contextID))
                            {
                                status = "Blocked";
                                detail = "The required union build did not pass.";
                                break;
                            }
                            if (testFailures.TryGetValue(contextID, out string? testFailure))
                            {
                                throw new InvalidOperationException(testFailure);
                            }
                            if (!tested.Contains(contextID))
                            {
                                string[] tests = checks
                                    .Where(candidate => (
                                        candidate.kind == "test" && candidate.exclusion is null && candidate.context == contextID
                                        && !CanReuse(candidate)
                                    ))
                                    .Select(static candidate => candidate.owner)
                                    .ToArray();
                                string solution = WriteUnion(contextID, "Tests", tests);
                                string resultsDirectory = Path.Combine(directory, contextID + "-trx");
                                Dictionary<string, string> properties = plan.contexts[contextID].properties;
                                string[] productArguments = contextID == "native"
                                                            ? [
                                                                "-Target",
                                                                properties["InnoNativeTarget"],
                                                                "-ToolTarget",
                                                                properties["InnoToolTarget"],
                                                                "-ProductProperties",
                                                                properties["InnoProductBuildProperties"]
                                                            ]
                                                            : [];
                                try
                                {
                                    long batchStart = Stopwatch.GetTimestamp();
                                    try
                                    {
                                        await ShellAsync(
                                            C_TEST_EXECUTOR,
                                            [
                                                "-Solution",
                                                solution,
                                                "-Configuration",
                                                properties["Configuration"],
                                                "-ArtifactsPath",
                                                properties["ArtifactsPath"],
                                                "-Dotnet",
                                                plan.dotnet,
                                                "-SdkSnapshot",
                                                graphPath,
                                                "-ResultsDirectory",
                                                resultsDirectory,
                                                ..productArguments,
                                                ..plan.nativeDependencies is null
                                                  ? Array.Empty<string>()
                                                  : ["-NativeDependencies", Path.Combine(plan.root, plan.nativeDependencies)]
                                            ],
                                            contextID + "-tests.log"
                                        ).ConfigureAwait(false);
                                    }
                                    catch (Exception failure) when (failure is not OperationCanceledException)
                                    {
                                        // Preserve the process failure even when individual owners have complete successful evidence.
                                        failures.Add(
                                            new ValidationRunFailure(
                                                "test-execution",
                                                failure.Message.Split('\n')[0],
                                                contextID,
                                                contextID + "-tests.log",
                                                Stopwatch.GetElapsedTime(batchStart).TotalSeconds
                                            )
                                        );
                                        await SaveAsync().ConfigureAwait(false);
                                    }
                                    finally
                                    {
                                        batchSeconds[contextID] = Stopwatch.GetElapsedTime(batchStart).TotalSeconds;
                                    }
                                    foreach (KeyValuePair<string, (bool passed, string detail)> evidence in ValidationTestEvidence.Read(
                                        plan.root,
                                        tests.ToDictionary(static project => project, project =>
                                            plan.contexts[contextID].sdkGraph.EnumerateArray()
                                                .Single(snapshot => (
                                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!)
                                                        .Replace('\\', '/') == project
                                                ))
                                                .GetProperty("Properties")
                                                .GetProperty("TargetPath")
                                                .GetString()!, StringComparer.Ordinal),
                                        resultsDirectory
                                    )) {
                                        testDetails.Add(evidence.Key, evidence.Value);
                                    }
                                    elapsed.Restart();
                                    executedTests.UnionWith(tests);
                                    tested.Add(contextID);
                                    outputDigests.Clear();
                                    dependencyInputs.Reset();
                                }
                                catch (Exception failure) when (failure is not OperationCanceledException)
                                {
                                    testFailures[contextID] = failure.Message;
                                    throw;
                                }
                            }
                            if (!executedTests.Contains(check.owner))
                            {
                                throw new InvalidDataException(
                                    $"Previously reusable output changed before {check.id}; this test did not execute and cannot be certified."
                                );
                            }
                            if (!testDetails[check.owner].passed)
                            {
                                throw new InvalidDataException(testDetails[check.owner].detail);
                            }
                            detail = testDetails[check.owner].detail;
                            break;
                        default:
                            throw new InvalidDataException($"Unknown required validation kind: {check.kind}");
                    }
                    if (status != "Blocked")
                    {
                        outputs = ValidationOutputs.Capture(plan, check, outputDigests, cancellationToken);
                        dependencyIdentity = dependencyInputs.Capture(check);
                        documentationInputs = ValidationDocumentationInputs.Capture(plan, check, dependencyInputs);
                        status = "Passed";
                    }
                }
                catch (OperationCanceledException)
                {
                    status = "Blocked";
                    detail = "Validation was canceled; owned child processes were drained and no successful result was recorded.";
                }
                catch (Exception failure)
                {
                    detail = failure.Message;
                }
            }
            results.Add(
                new ValidationResult(
                    check.id,
                    status,
                    identity,
                    elapsed.Elapsed.TotalSeconds,
                    detail,
                    outputs,
                    dependencyIdentity,
                    documentationInputs
                )
            );
            string measurement = check.kind == "test" ? " owner reconciliation" : string.Empty;
            Console.WriteLine($"{status}: {check.id} ({elapsed.Elapsed.TotalSeconds:F2}s{measurement}) {detail}");
            await SaveAsync().ConfigureAwait(false);
        }
        if (cancellationToken.IsCancellationRequested)
        {
            failures.Add(new ValidationRunFailure("execution-canceled", "The run was canceled before closing verification."));
            await SaveAsync().ConfigureAwait(false);
            return new(
                plan.commit,
                plan.tree,
                plan.environment,
                started,
                OwnedResults(),
                ValidationShards.C_EVIDENCE_VERSION,
                failures.ToArray(),
                planIdentity,
                shard: shard?.id,
                batchSeconds: batchSeconds
            );
        }
        // Both closing snapshots read independent owners after all checks; neither may reuse planning-time bytes.
        long closingStart = Stopwatch.GetTimestamp();
        try
        {
            await VerifyClosingStateAsync().ConfigureAwait(false);
        }
        catch (Exception failure)
        {
            closingSeconds = Stopwatch.GetElapsedTime(closingStart).TotalSeconds;
            failures.Add(
                new ValidationRunFailure(
                    failure is OperationCanceledException ? "execution-canceled" : "closing-verification",
                    failure.Message
                )
            );
            await SaveAsync().ConfigureAwait(false);
            throw;
        }
        closingSeconds = Stopwatch.GetElapsedTime(closingStart).TotalSeconds;
        await SaveAsync(completed: true).ConfigureAwait(false);
        return new(
            plan.commit,
            plan.tree,
            plan.environment,
            started,
            OwnedResults(),
            ValidationShards.C_EVIDENCE_VERSION,
            failures.ToArray(),
            planIdentity,
            completed: true,
            shard: shard?.id,
            closingSeconds: closingSeconds,
            batchSeconds: batchSeconds
        );

        async Task VerifyClosingStateAsync()
        {
            Task<Dictionary<string, string>> sourceCapture = ValidationInputs.CaptureAsync(plan.root, cancellationToken);
            Task<string> environmentCapture = Task.Run(() => ValidationEnvironment.CaptureAsync(
                plan.root,
                plan.dotnet,
                plan.contexts.Values,
                cancellationToken: cancellationToken
            ), cancellationToken);
            await Task.WhenAll(sourceCapture, environmentCapture).ConfigureAwait(false);
            Dictionary<string, string> after = await sourceCapture.ConfigureAwait(false);
            outputDigests.Clear();
            dependencyInputs.Reset();
            var unstableCompilations = new HashSet<string>(StringComparer.Ordinal);
            foreach (ValidationCheck check in checks.Where(static check => check.exclusion is null))
            {
                ValidationResult result = results.Single(record => record.id == check.id);
                if (result.status is "Passed" or "Reused"
                    && (!ValidationOutputs.Matches(plan, check, result, outputDigests, cancellationToken)
                        || !dependencyInputs.Matches(check, result.dependencyIdentity))
                ) {
                    if (check.kind == "build")
                    {
                        unstableCompilations.Add(check.context!);
                    }
                    failures.Add(
                        new ValidationRunFailure(
                            "output-stability",
                            "Compiled outputs or restored dependencies changed after verification.",
                            check.id
                        )
                    );
                    await SaveAsync().ConfigureAwait(false);
                }
            }
            bool sourceStable = ValidationInputs.Identity(after) == plan.tree;
            bool environmentStable = plan.environment == await environmentCapture.ConfigureAwait(false);
            if (!sourceStable)
            {
                failures.Add(new ValidationRunFailure("input-stability", "Inputs changed during validation."));
                await SaveAsync().ConfigureAwait(false);
            }
            if (!environmentStable)
            {
                failures.Add(
                    new ValidationRunFailure("environment-stability", "SDK, runtime, shell or imports changed during validation.")
                );
                await SaveAsync().ConfigureAwait(false);
            }
            if (sourceStable && environmentStable)
            {
                foreach (KeyValuePair<string, ValidationBuildInputs> compilation in pendingCompilations.Where(
                    pair => !unstableCompilations.Contains(pair.Key)
                )) {
                    compilation.Value.Record(dependencyInputs);
                }
            }
            if (workspace is not null)
            {
                ValidationWorkspace.RecordCompleted(plan, cancellationToken);
            }
        }

        Task ShellAsync(
            string script,
            string[] arguments,
            string log
        ) => ValidationProcess.RunAsync(
            "pwsh",
            ["-NoProfile", "-File", Path.Combine(plan.root, script), ..arguments],
            plan.root,
            Path.Combine(directory, log),
            cancellationToken: cancellationToken
        );

        async Task<string> ResolveCompilationGraphAsync(string context)
        {
            if (compilationGraphs.TryGetValue(context, out string? existing))
            {
                return existing;
            }
            string[] projects = checks
                .Where(candidate => (
                    candidate.context == context && candidate.exclusion is null
                    && candidate.kind is "api" or "documentation-api" or "documentation-examples"
                    && candidate.prerequisites.All(
                        prerequisite => results.Any(result => result.id == prerequisite && result.status is "Passed" or "Reused")
                    )
                ))
                .SelectMany(static candidate => candidate.projects ?? [])
                // Consumer roots carry effective reference-edge properties, even when only their dependency exposes an API.
                .Concat(
                    checks.Where(candidate => (
                        candidate.kind == "build" && candidate.context == context && candidate.exclusion is null
                        && results.Any(result => result.id == candidate.id && result.status is "Passed" or "Reused")
                    )).Select(static candidate => candidate.owner)
                )
                .Distinct(StringComparer.Ordinal)
                .Order(StringComparer.Ordinal)
                .Select(project => Path.Combine(plan.root, project))
                .ToArray();
            if (projects.Length == 0)
            {
                throw new InvalidDataException("Required compilation reference preparation has no eligible project owners.");
            }
            string graph = await ManagedProjectGraphCapture.CaptureAsync(
                plan.root,
                plan.dotnet,
                projects,
                plan.contexts[context].properties,
                resolveReferences: true,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            string destination = Path.Combine(directory, context + "-resolved-sdk-graph.json");
            await File.WriteAllTextAsync(destination, graph, cancellationToken).ConfigureAwait(false);
            compilationGraphs.Add(context, destination);
            return destination;
        }

        string WriteUnion(
            string context,
            string operation,
            string[] projects
        ) {
            if (projects.Length == 0)
            {
                throw new InvalidDataException("A required build or test union is empty.");
            }
            string destination = Path.Combine(directory, context + "-" + operation + ".slnx");
            // IDE solution filters inherit Build.0 exclusions and configuration remapping. The plan owns this exact union.
            new XDocument(
                new XElement(
                    "Solution",
                    new XElement(
                        "Configurations",
                        new XElement("BuildType", new XAttribute("Name", plan.contexts[context].properties["Configuration"]))
                    ),
                    projects.Select(
                        project => new XElement("Project", new XAttribute("Path", Path.Combine(plan.root, project).Replace('\\', '/')))
                    )
                )
            )
                .Save(destination);
            return destination;
        }

        ValidationResult[] OwnedResults() => results.Where(result => !imported.Contains(result)).ToArray();

        Task SaveAsync(bool completed = false)
            => File.WriteAllTextAsync(
                Path.Combine(directory, "results.json"),
                JsonSerializer.Serialize(
                    new ValidationEvidence(
                        plan.commit,
                        plan.tree,
                        plan.environment,
                        started,
                        OwnedResults(),
                        ValidationShards.C_EVIDENCE_VERSION,
                        failures.ToArray(),
                        planIdentity,
                        completed,
                        shard?.id,
                        closingSeconds,
                        batchSeconds
                    ),
                    new JsonSerializerOptions { WriteIndented = true }
                ),
                CancellationToken.None
            );

        bool CanReuse(ValidationCheck candidate)
        {
            // A context is a single compilation union. Adopt it only when every root and its
            // restored dependencies match, so a partial adoption cannot bypass a required build.
            if (continuation && candidate.kind == "build")
            {
                string context = candidate.context!;
                if (!reusableCompilations.TryGetValue(context, out bool reusable))
                {
                    reusable = checks.Where(check => check.kind == "build" && check.context == context && check.exclusion is null)
                        .All(MatchesRetained);
                    reusableCompilations.Add(context, reusable);
                }
                return reusable;
            }
            return candidate.reusable && MatchesRetained(candidate);
        }

        bool MatchesRetained(ValidationCheck candidate)
        {
            ValidationResult? retained = (continuation || string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")))
                && previous is { completed: true, version: ValidationShards.C_EVIDENCE_VERSION, failures: not null }
                && previous.environment == plan.environment
                && previous.failures.All(fault => fault.stage switch
                {
                    "test-execution" => candidate.kind != "test" || candidate.context != fault.owner
                || previous.results.Any(
                    result => (
                        result.status == "Failed"
                        && plan.checks.Any(check => check.id == result.id && check.kind == "test" && check.context == fault.owner)
                    )
                ),
                    "output-stability" => false,
                    _ => false
                })
                                         ? previous.results.SingleOrDefault(result => result.id == candidate.id)
                                         : null;
            if (retained is null && candidate.kind == "test" && candidate.owner.StartsWith("tests/backends/", StringComparison.Ordinal))
            {
                retained = plan.backendTests?.SingleOrDefault(result => result.id == candidate.id);
            }
            if (retained is not { status: "Passed" or "Reused" }
                || retained.inputIdentity != ValidationInputs.CheckIdentity(plan, candidate, current)
                || !dependencyInputs.Matches(candidate, retained.dependencyIdentity)
                || !ValidationOutputs.Matches(plan, candidate, retained, outputDigests, cancellationToken)
            ) {
                return false;
            }
            reused[candidate.id] = retained;
            return true;
        }
    }
}
