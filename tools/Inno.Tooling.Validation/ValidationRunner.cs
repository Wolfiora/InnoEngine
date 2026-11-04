using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Tooling.Architecture;

namespace Inno.Tooling.Validation;

internal static class ValidationRunner
{
    internal static async Task<ValidationEvidence> RunAsync(
        ValidationPlan plan,
        string directory,
        ValidationEvidence? previous,
        CancellationToken cancellation
    ) {
        var results = new List<ValidationResult>();
        DateTimeOffset started = DateTimeOffset.UtcNow;
        string planIdentity = ValidationInputs.PlanIdentity(plan);
        Directory.CreateDirectory(directory);
        using var workspace = plan.checks.Any(static check => check.kind == "build" && check.exclusion is null)
            ? await ValidationWorkspace.AcquireAsync(plan, cancellation).ConfigureAwait(false) : null;
        foreach (var context in plan.contexts)
            await File.WriteAllTextAsync(Path.Combine(directory, context.Key + "-sdk-graph.json"), context.Value.sdkGraph.GetRawText(), cancellation).ConfigureAwait(false);
        Dictionary<string, string> current = await ValidationInputs.CaptureAsync(plan.root, cancellation).ConfigureAwait(false);
        if (ValidationInputs.Identity(current) != plan.tree)
            throw new InvalidDataException("Validation plan inputs changed before execution. Generate a new plan.");
        var built = new HashSet<string>(StringComparer.Ordinal);
        var tested = new HashSet<string>(StringComparer.Ordinal);
        var executedTests = new HashSet<string>(StringComparer.Ordinal);
        var buildFailures = new Dictionary<string, string>(StringComparer.Ordinal);
        var testFailures = new Dictionary<string, string>(StringComparer.Ordinal);
        var outputDigests = new Dictionary<string, string>(StringComparer.Ordinal);
        var dependencyInputs = new ValidationDependencyInputs(plan, cancellation);
        foreach (ValidationCheck check in plan.checks.OrderBy(static check => check.kind switch
        {
            "cold-inputs" => -1,
            "instructions" or "architecture" or "documentation" or "ci-contracts" => 0,
            "build" => 1,
            "api" => 2,
            "release-bundle" => 4,
            _ => 3
        }).ThenBy(static check => check.context?.StartsWith("native", StringComparison.Ordinal) == true ? 1 : 0))
        {
            string identity = ValidationInputs.CheckIdentity(plan, check, current);
            string contextId = check.context ?? "managed";
            string graphPath = Path.Combine(directory, contextId + "-sdk-graph.json");
            string status = "Failed";
            string detail = string.Empty;
            Dictionary<string, string>? outputs = null;
            string dependencyIdentity = string.Empty;
            var elapsed = Stopwatch.StartNew();
            ValidationResult? old = previous?.results.SingleOrDefault(result => result.id == check.id);
            if (check.exclusion is not null)
            {
                status = "NotApplicable";
                detail = check.exclusion;
            }
            else if (cancellation.IsCancellationRequested)
            {
                status = "Blocked";
                detail = "Validation was canceled before this required check completed.";
            }
            else if (check.prerequisites.Any(prerequisite => !results.Any(result => result.id == prerequisite && result.status is "Passed" or "Reused"))
                || (check.kind is "build" or "api" or "test" && results.Any(static result => result.status is "Failed" or "Blocked")))
            {
                status = "Blocked";
                detail = "A required preceding check did not pass; dependent work was not executed.";
            }
            else if (check.reusable && previous?.environment == plan.environment && old is { status: "Passed" or "Reused" }
                && old.inputIdentity == identity && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
                && dependencyInputs.Matches(check, old.dependencyIdentity)
                && ValidationOutputs.Matches(plan, check, old, cancellation, outputDigests))
            {
                status = "Reused";
                detail = "Exact local deterministic input identity matches retained successful evidence.";
                outputs = old.outputs;
                dependencyIdentity = old.dependencyIdentity;
            }
            else
            {
                try
                {
                    switch (check.kind)
                    {
                        case "release-bundle":
                            ValidationDeliverables.Create(plan, check, cancellation);
                            break;
                        case "cold-inputs":
                            foreach (string cache in new[] { "artifacts/native", "artifacts/bindings" })
                            {
                                string path = Path.Combine(plan.root, cache);
                                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any())
                                    throw new InvalidDataException($"Cold validation requires a fresh checkout/cache owner; existing artifacts cannot be relabeled cold: {path}");
                            }
                            break;
                        case "hot-builds":
                            await ShellAsync("tests/Invoke-BuildCacheChecks.ps1", ["-Dotnet", plan.dotnet,
                                "-PlanPath", Path.Combine(directory, "plan.json"), "-Context", contextId,
                                "-ResultsDirectory", Path.Combine(directory, "hot-builds"), "-Iterations", "10"], "hot-builds.log").ConfigureAwait(false);
                            break;
                        case "instructions":
                            await ShellAsync("tests/ci/Test-AgentRules.ps1", [], "instructions.log").ConfigureAwait(false);
                            break;
                        case "ci-contracts":
                            await ShellAsync("tests/ci/Test-ValidationReport.Tests.ps1", [], "ci-contracts.log").ConfigureAwait(false);
                            await ShellAsync("tests/ci/Test-ReleaseSource.Tests.ps1", [], "release-source-contracts.log").ConfigureAwait(false);
                            await ShellAsync("tests/ci/Select-ValidationCadence.Tests.ps1", [], "cadence-contracts.log").ConfigureAwait(false);
                            await ShellAsync("tests/ci/Test-ReleaseBundle.Tests.ps1", [], "release-bundle-contracts.log").ConfigureAwait(false);
                            await ShellAsync("tests/ci/Test-IsolatedExecution.Tests.ps1", [], "isolated-execution-contracts.log").ConfigureAwait(false);
                            await ShellAsync("tests/ci/Test-SupportPackClosure.Tests.ps1", [], "support-pack-contracts.log").ConfigureAwait(false);
                            break;
                        case "restore-aot":
                            {
                                JsonElement player = plan.contexts[contextId].sdkGraph.EnumerateArray().Single(snapshot =>
                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner);
                                string runtime = player.GetProperty("Properties").GetProperty("NETCoreSdkRuntimeIdentifier").GetString()!;
                                if (string.IsNullOrWhiteSpace(runtime))
                                    throw new InvalidDataException("Host NativeAOT restore requires the evaluated SDK runtime identifier.");
                                await ValidationProcess.RunAsync(plan.dotnet, ["restore", Path.Combine(plan.root, check.owner),
                                    "--runtime", runtime, "-p:Configuration=Release", "-p:PublishAot=true",
                                    "-p:ArtifactsPath=" + Path.Combine(plan.artifacts, "restore", "native-aot"),
                                    "--disable-parallel", "-nr:false"], plan.root, Path.Combine(directory, "native-aot-restore.log"), cancellation).ConfigureAwait(false);
                            }
                            break;
                        case "architecture":
                            if (ArchitectureValidator.Execute([plan.root, "--configuration", plan.configuration, "--source-only",
                                "--dotnet", plan.dotnet, "--sdk-snapshot", graphPath], cancellation) != 0)
                                throw new InvalidDataException("Repository architecture validation failed.");
                            break;
                        case "api":
                            if (!built.Contains(contextId))
                            {
                                status = "Blocked";
                                detail = "Compiled API verification requires the successful union build.";
                                break;
                            }
                            if (check.projects is not { Length: > 0 })
                                throw new InvalidDataException("Required compiled API owner selection is empty.");
                            string[] projects = check.projects.SelectMany(static project => new[] { "--api-project", project }).ToArray();
                            if (ArchitectureValidator.Execute([plan.root, "--metadata-only", "--configuration", plan.contexts[contextId].properties["Configuration"],
                                "--dotnet", plan.dotnet, "--artifacts-path", plan.contexts[contextId].properties["ArtifactsPath"], "--sdk-snapshot", graphPath, ..projects], cancellation) != 0)
                                throw new InvalidDataException("Affected compiled API boundaries failed.");
                            break;
                        case "documentation":
                            if (ArchitectureValidator.Execute([plan.root, "--documentation-only"], cancellation) != 0)
                                throw new InvalidDataException("Documentation ownership, content or links failed.");
                            break;
                        case "wiki":
                            string[] pages = check.inputs.Where(static input => input.StartsWith("docs/", StringComparison.Ordinal))
                                .SelectMany(static input => new[] { "--wiki-page", input }).ToArray();
                            var scriptArguments = new List<string>();
                            if (check.inputs.Where(static input => input.StartsWith("docs/", StringComparison.Ordinal))
                                .Any(page => ValidationDocumentation.RequiresScriptReferences(Path.Combine(plan.root, page))))
                            {
                                string entryContext = plan.checks.Single(static candidate => candidate.kind == "build"
                                    && candidate.owner == "entry/InnoEngine.Build/InnoEngine.Build.csproj").context!;
                                JsonElement snapshot = plan.contexts[entryContext].sdkGraph;
                                string entry = snapshot.EnumerateArray().Single(node => Path.GetFileName(node.GetProperty("Path").GetString()) == "InnoEngine.Build.csproj")
                                    .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
                                string project = Path.Combine(directory, contextId + "-wiki-project");
                                Directory.CreateDirectory(Path.Combine(project, "Assets"));
                                await File.WriteAllTextAsync(Path.Combine(project, "Assets", "WikiProbe.cs"), "public sealed class WikiProbe { }", cancellation).ConfigureAwait(false);
                                await ValidationProcess.RunAsync(plan.dotnet, [entry, "scripts", "--engine-root", plan.root,
                                    "--tools-target", plan.contexts[contextId].properties["InnoToolTarget"], "--dotnet", plan.dotnet,
                                    "--project", project, "--output", Path.Combine(project, "Scripts"), "--ide"], plan.root,
                                    Path.Combine(directory, contextId + "-wiki-projection.log"), cancellation).ConfigureAwait(false);
                                XDocument projection = XDocument.Load(Path.Combine(project, "Inno.EditorScripts.csproj"));
                                foreach (string scope in new[] { "Runtime", "Editor" })
                                {
                                    XElement reference = projection.Descendants("Reference").Single(element => (string?)element.Attribute("Include") == "Inno.ScriptApi." + scope);
                                    string path = Path.GetFullPath(reference.Element("HintPath")!.Value, project);
                                    if (!File.Exists(path))
                                        throw new InvalidDataException($"Current Wiki script projection reference is missing: {path}");
                                    scriptArguments.AddRange(["--wiki-" + scope.ToLowerInvariant() + "-reference", path]);
                                }
                            }
                            string[] targetArguments = contextId == "native" ? ["--wiki-target", plan.contexts[contextId].properties["InnoNativeTarget"]] : [];
                            if (ArchitectureValidator.Execute([plan.root, "--wiki", "--wiki-only", "--configuration", plan.configuration,
                                "--dotnet", plan.dotnet, "--artifacts-path", plan.contexts[contextId].properties["ArtifactsPath"], "--wiki-api", Path.Combine(directory, contextId + "-wiki-api.json"),
                                "--wiki-examples", Path.Combine(directory, contextId + "-wiki-examples.json"), "--sdk-snapshot", graphPath, ..targetArguments, ..scriptArguments, ..pages], cancellation) != 0)
                                throw new InvalidDataException("Scoped Wiki signatures or compiled examples failed.");
                            break;
                        case "products":
                            {
                                JsonElement graph = plan.contexts[contextId].sdkGraph;
                                JsonElement editor = graph.EnumerateArray().Single(snapshot =>
                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner);
                                string assembly = editor.GetProperty("Properties").GetProperty("TargetPath").GetString()!;
                                string backendPattern = editor.GetProperty("Properties").GetProperty("InnoValidationBackendPattern").GetString()!;
                                if (string.IsNullOrWhiteSpace(backendPattern))
                                    throw new InvalidDataException("Product acceptance requires its platform-owned renderer startup contract.");
                                string productDirectory = Path.Combine(directory, check.id);
                                Directory.CreateDirectory(productDirectory);
                                string project = Path.Combine(productDirectory, "Source");
                                await ValidationProductInput.PrepareAsync(plan, productDirectory, project, cancellation).ConfigureAwait(false);
                                await ShellAsync("tests/Invoke-EditorSmoke.ps1", ["-EditorAssembly", assembly, "-ProjectDirectory", project,
                                    "-BackendPattern", backendPattern, "-Dotnet", plan.dotnet, "-Frames", "300", "-HiddenWindow"], check.id + "-smoke.log").ConfigureAwait(false);
                                string[] renderers = Directory.GetFiles(Path.Combine(project, "Logs"), "*.log").SelectMany(path =>
                                    System.Text.RegularExpressions.Regex.Matches(File.ReadAllText(path), @"Rendering initialized with (\w+)")
                                        .Select(static match => match.Groups[1].Value)).Distinct(StringComparer.Ordinal).ToArray();
                                if (renderers.Length != 1)
                                    throw new InvalidDataException("Product smoke did not identify one actual initialized renderer.");
                                foreach (string scenario in new[] { "Workflows", "Lifecycle" })
                                    await ShellAsync("tests/Invoke-EditorUiProductJourney.ps1", ["-EditorAssembly", assembly, "-ProjectDirectory", project,
                                        "-ResultsDirectory", Path.Combine(productDirectory, scenario), "-Dotnet", plan.dotnet,
                                        "-GraphicsApi", renderers[0], "-Scenario", scenario,
                                        ..(plan.extended && scenario == "Lifecycle" ? new[] { "-Cycles", "1000", "-Frames", "180000", "-TimeoutSeconds", "5400" } : [])],
                                        check.id + "-" + scenario + ".log").ConfigureAwait(false);
                            }
                            break;
                        case "support-pack":
                            {
                                string entry = plan.contexts[contextId].sdkGraph.EnumerateArray().Single(snapshot =>
                                    Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner)
                                    .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
                                string deliverables = Path.Combine(plan.artifacts, "deliverables");
                                Directory.CreateDirectory(deliverables);
                                await ValidationProcess.RunAsync(plan.dotnet, [entry, "support-pack", "--engine-root", plan.root,
                                    "--tools-target", plan.contexts[contextId].properties["InnoToolTarget"], "--dotnet", plan.dotnet,
                                    "--configuration", "Release", "--archive", Path.Combine(deliverables, "PlayerSupportPack.zip")],
                                    plan.root, Path.Combine(directory, "support-pack.log"), cancellation).ConfigureAwait(false);
                                await ShellAsync("tests/ci/Test-SupportPackClosure.ps1", ["-Archive", Path.Combine(deliverables, "PlayerSupportPack.zip")],
                                    "support-pack-closure.log").ConfigureAwait(false);
                            }
                            break;
                        case "build":
                            if (buildFailures.TryGetValue(contextId, out string? buildFailure))
                                throw new InvalidOperationException(buildFailure);
                            if (!built.Contains(contextId))
                            {
                                string[] builds = plan.checks.Where(candidate => candidate.kind == "build" && candidate.exclusion is null && candidate.context == contextId)
                                    .Select(static candidate => candidate.owner).ToArray();
                                string filter = WriteFilter(contextId + "-Build.slnf", builds);
                                try
                                {
                                    var compilationInputs = new ValidationBuildInputs(plan, contextId, outputDigests, cancellation);
                                    string[] compilation = compilationInputs.RequiresCompilation(dependencyInputs) ? ["--no-incremental"] : [];
                                    await ValidationProcess.RunAsync(plan.dotnet,
                                        ["build", filter, ..compilation, ..plan.contexts[contextId].properties.Select(static pair => "-p:" + pair.Key + "=" + pair.Value),
                                            "--disable-build-servers", "-m:" + (contextId == "managed" ? Math.Min(Environment.ProcessorCount, 4) : 1),
                                            "-nr:false"], plan.root, Path.Combine(directory, contextId + "-build.log"), cancellation).ConfigureAwait(false);
                                    built.Add(contextId);
                                    outputDigests.Clear();
                                    dependencyInputs.Reset();
                                    compilationInputs.Record(dependencyInputs);
                                }
                                catch (Exception failure) when (failure is not OperationCanceledException)
                                {
                                    buildFailures[contextId] = failure.Message;
                                    throw;
                                }
                            }
                            break;
                        case "test":
                            if (!built.Contains(contextId))
                            {
                                status = "Blocked";
                                detail = "The required union build did not pass.";
                                break;
                            }
                            if (testFailures.TryGetValue(contextId, out string? testFailure))
                                throw new InvalidOperationException(testFailure);
                            if (!tested.Contains(contextId))
                            {
                                string[] tests = plan.checks.Where(candidate => candidate.kind == "test" && candidate.exclusion is null && candidate.context == contextId
                                    && !CanReuse(candidate))
                                    .Select(static candidate => candidate.owner).ToArray();
                                string filter = WriteFilter(contextId + "-Tests.slnf", tests);
                                string resultsDirectory = Path.Combine(directory, contextId + "-trx");
                                Dictionary<string, string> properties = plan.contexts[contextId].properties;
                                string[] productArguments = contextId == "native" ? ["-Target", properties["InnoNativeTarget"], "-ToolTarget", properties["InnoToolTarget"],
                                    "-ProductProperties", properties["InnoProductBuildProperties"]] : [];
                                try
                                {
                                    await ShellAsync("tests/Invoke-IsolatedTests.ps1",
                                        ["-Solution", filter, "-Configuration", properties["Configuration"], "-ArtifactsPath", properties["ArtifactsPath"],
                                            "-Dotnet", plan.dotnet, "-SdkSnapshot", graphPath, "-ResultsDirectory", resultsDirectory, ..productArguments], contextId + "-tests.log").ConfigureAwait(false);
                                    VerifyDiscovery(tests, resultsDirectory);
                                    executedTests.UnionWith(tests);
                                    tested.Add(contextId);
                                    outputDigests.Clear();
                                    dependencyInputs.Reset();
                                }
                                catch (Exception failure) when (failure is not OperationCanceledException)
                                {
                                    testFailures[contextId] = failure.Message;
                                    throw;
                                }
                            }
                            if (!executedTests.Contains(check.owner))
                                throw new InvalidDataException($"Previously reusable output changed before {check.id}; this test did not execute and cannot be certified.");
                            break;
                        default:
                            throw new InvalidDataException($"Unknown required validation kind: {check.kind}");
                    }
                    if (status != "Blocked")
                    {
                        outputs = ValidationOutputs.Capture(plan, check, cancellation, outputDigests);
                        dependencyIdentity = dependencyInputs.Capture(check);
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
            results.Add(new(check.id, status, identity, elapsed.Elapsed.TotalSeconds, detail, outputs, dependencyIdentity));
            Console.WriteLine($"{status}: {check.id} ({elapsed.Elapsed.TotalSeconds:F2}s) {detail}");
            await SaveAsync().ConfigureAwait(false);
        }
        if (cancellation.IsCancellationRequested)
        {
            results.Add(new("execution-canceled", "Blocked", plan.tree, 0, "The run was canceled and cannot certify completion."));
            await SaveAsync().ConfigureAwait(false);
            return new(plan.commit, plan.tree, plan.environment, started, results.ToArray(), planIdentity);
        }
        Dictionary<string, string> after = await ValidationInputs.CaptureAsync(plan.root, cancellation).ConfigureAwait(false);
        outputDigests.Clear();
        dependencyInputs.Reset();
        foreach (ValidationCheck check in plan.checks.Where(static check => check.exclusion is null))
        {
            ValidationResult result = results.Single(record => record.id == check.id);
            if (result.status is "Passed" or "Reused" && (!ValidationOutputs.Matches(plan, check, result, cancellation, outputDigests)
                || !dependencyInputs.Matches(check, result.dependencyIdentity)))
            {
                results.Add(new("output-stability:" + check.id, "Failed", result.inputIdentity, 0, "Required compiled outputs or restored dependencies changed after their check completed."));
                await SaveAsync().ConfigureAwait(false);
            }
        }
        if (ValidationInputs.Identity(after) != plan.tree)
        {
            results.Add(new("input-stability", "Failed", plan.tree, 0, "Inputs changed during validation; results cannot certify this workspace."));
            await SaveAsync().ConfigureAwait(false);
        }
        if (plan.environment != await ValidationEnvironment.CaptureAsync(plan.root, plan.dotnet, cancellation, plan.contexts.Values).ConfigureAwait(false))
        {
            results.Add(new("environment-stability", "Failed", plan.environment, 0, "SDK, runtime, shell or external import contents changed during validation."));
            await SaveAsync().ConfigureAwait(false);
        }
        if (workspace is not null)
            ValidationWorkspace.RecordCompleted(plan, cancellation);
        return new(plan.commit, plan.tree, plan.environment, started, results.ToArray(), planIdentity);

        Task ShellAsync(
            string script,
            string[] arguments,
            string log
        ) => ValidationProcess.RunAsync("pwsh", ["-NoProfile", "-File", Path.Combine(plan.root, script), ..arguments],
            plan.root, Path.Combine(directory, log), cancellation);

        string WriteFilter(
            string name,
            string[] projects
        ) {
            if (projects.Length == 0)
                throw new InvalidDataException("A required build or test union is empty.");
            string destination = Path.Combine(directory, name);
            File.WriteAllText(destination, JsonSerializer.Serialize(new { solution = new { path = Path.Combine(plan.root, "InnoEngine.sln"), projects } }));
            return destination;
        }

        Task SaveAsync() => File.WriteAllTextAsync(Path.Combine(directory, "results.json"),
            JsonSerializer.Serialize(new ValidationEvidence(plan.commit, plan.tree, plan.environment, started, results.ToArray(), planIdentity),
                new JsonSerializerOptions { WriteIndented = true }), CancellationToken.None);

        bool CanReuse(ValidationCheck candidate)
        {
            ValidationResult? retained = previous?.results.SingleOrDefault(result => result.id == candidate.id);
            return candidate.reusable && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
                && previous?.environment == plan.environment && retained is { status: "Passed" or "Reused" }
                && retained.inputIdentity == ValidationInputs.CheckIdentity(plan, candidate, current)
                && dependencyInputs.Matches(candidate, retained.dependencyIdentity)
                && ValidationOutputs.Matches(plan, candidate, retained, cancellation, outputDigests);
        }
    }

    internal static void VerifyDiscovery(
        string[] projects,
        string directory
    ) {
        string[] reports = Directory.GetFiles(directory, "*.trx", SearchOption.AllDirectories);
        var assemblies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string report in reports)
        {
            XDocument document = XDocument.Load(report);
            XElement counters = document.Descendants().Single(element => element.Name.LocalName == "Counters");
            int? executed = (int?)counters.Attribute("executed");
            XElement[] records = document.Descendants().Where(static element => element.Name.LocalName == "UnitTestResult").ToArray();
            if (executed is not >= 0 || (int?)counters.Attribute("failed") is not 0
                || (int?)counters.Attribute("total") != executed || (int?)counters.Attribute("passed") != executed
                || (int?)counters.Attribute("error") is not 0 || records.Length != executed
                || records.Any(static result => (string?)result.Attribute("outcome") != "Passed"))
                throw new InvalidDataException($"Invalid or failed test execution: {report}");
            if (executed == 0)
                continue;
            foreach (XElement definition in document.Descendants().Where(static element => element.Name.LocalName == "UnitTest"))
                assemblies.Add(Path.GetFileNameWithoutExtension((string?)definition.Attribute("storage") ?? string.Empty));
        }
        foreach (string project in projects)
            if (!assemblies.Contains(Path.GetFileNameWithoutExtension(project)))
                throw new InvalidDataException($"No executed test evidence for required owner: {project}");
    }

}
