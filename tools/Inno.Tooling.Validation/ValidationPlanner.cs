using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Tooling.Architecture;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

internal static class ValidationPlanner
{
    internal static async Task<ValidationPlanningResult> CreateAsync(
        string root,
        string baseline,
        string dotnet,
        string configuration,
        string artifacts,
        bool full,
        CancellationToken cancellation,
        bool extended = false,
        string? documentationCache = null,
        string? documentationSnapshot = null,
        string? documentationBaseline = null
    ) {
        var elapsed = Stopwatch.StartNew();
        var timings = new Dictionary<string, double>(StringComparer.Ordinal);
        double previousStage = 0;
        Dictionary<string, string> inputs = await ValidationInputs.CaptureAsync(root, cancellation).ConfigureAwait(false);
        string commit = (await GitAsync("rev-parse", "HEAD").ConfigureAwait(false)).Trim();
        string mergeBase = (await GitAsync("merge-base", baseline, commit).ConfigureAwait(false)).Trim();
        var changes = new HashSet<string>((await GitAsync("diff", "--name-only", "-z", "--no-renames", mergeBase, "HEAD").ConfigureAwait(false))
            .Split('\0', StringSplitOptions.RemoveEmptyEntries), StringComparer.Ordinal);
        changes.UnionWith((await GitAsync("diff", "--name-only", "-z", "--no-renames", "HEAD").ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries));
        changes.UnionWith((await GitAsync("diff", "--cached", "--name-only", "-z", "--no-renames", "HEAD").ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries));
        changes.UnionWith(await ValidationInputs.FindContentChangesAsync(root, commit, inputs, cancellation).ConfigureAwait(false));
        changes.UnionWith((await GitAsync("ls-files", "--others", "--exclude-standard", "-z").ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries));
        string[] paths = inputs.Keys.Where(static path => path.EndsWith(".csproj", StringComparison.Ordinal)
            && path.Split('/')[0] is "src" or "entry" or "build" or "backends" or "platforms" or "tools" or "tests")
            .Where(path => inputs[path] != "missing").Order(StringComparer.Ordinal).ToArray();
        RecordStage("source-inputs");
        var properties = new Dictionary<string, string>
        {
            ["Configuration"] = configuration,
            ["DesignTimeBuild"] = "true",
            ["BuildProjectReferences"] = "true",
            ["BuildingInsideVisualStudio"] = "false",
            ["ShouldUnsetParentConfigurationAndPlatform"] = "false",
            ["InnoProductRestore"] = "false",
            ["ArtifactsPath"] = artifacts
        };
        Task<ValidationEnvironment.ToolSnapshot> toolCapture = Task.Run(() => ValidationEnvironment.CaptureToolsAsync(root, dotnet, cancellation), cancellation);
        Task<string> graphCapture = ManagedProjectGraphCapture.CaptureAsync(root, dotnet,
            paths.Select(path => Path.Combine(root, path)).ToArray(), properties, cancellation);
        // Both read-only owners finish before a plan is emitted, including when either fails or is canceled.
        await Task.WhenAll(toolCapture, graphCapture).ConfigureAwait(false);
        string captured = await graphCapture.ConfigureAwait(false);
        RecordStage("sdk-capture");
        using JsonDocument graph = JsonDocument.Parse(captured);
        string[] submoduleInputs = inputs.Where(pair => pair.Value.StartsWith("unavailable:", StringComparison.Ordinal)
            || new DirectoryInfo(Path.GetFullPath(pair.Key, root)) is { Exists: true, LinkTarget: null })
            .Select(static pair => pair.Key).Order(StringComparer.Ordinal).ToArray();
        RequireCompilerInputs(graph.RootElement);
        ValidationProject[] projects = graph.RootElement.EnumerateArray().Select(snapshot => ValidationProject.Read(root, snapshot)).ToArray();
        var byPath = projects.ToDictionary(static project => project.path, StringComparer.Ordinal);
        RecordStage("managed-graph");
        var affected = new HashSet<string>(StringComparer.Ordinal);
        var documentationOwners = new HashSet<string>(StringComparer.Ordinal);
        ValidationDocumentation.Selection? documentation = await ValidationDocumentation.CaptureAsync(documentationCache,
            documentationSnapshot, byPath, cancellation, documentationBaseline).ConfigureAwait(false);
        var diagnostics = new List<string>();
        foreach (string change in changes)
        {
            ValidationProject[] owners = projects.Where(project => project.inputs.Contains(change, StringComparer.Ordinal)
                || change.StartsWith(DirectoryOf(project.path), StringComparison.Ordinal)).ToArray();
            foreach (ValidationProject owner in owners)
                affected.Add(owner.path);
            if (owners.Length == 0 && !IsDocumentation(change) && change != ValidationRunner.C_TEST_EXECUTOR
                && change != ValidationCiContracts.C_RULE_LOADER && !ValidationCiContracts.Owns(change))
            {
                diagnostics.Add($"Unknown or shared input '{change}': conservatively selecting every owner.");
                full = true;
            }
        }
        bool changedTestExecutor = changes.Contains(ValidationRunner.C_TEST_EXECUTOR);
        if (changedTestExecutor)
            affected.UnionWith(projects.Where(static project => project.test).Select(static project => project.path));
        if (extended)
            full = true;
        if (full)
            affected.UnionWith(paths);
        bool expanded;
        do
        {
            expanded = false;
            foreach (ValidationProject project in projects)
                if (project.references.Any(affected.Contains))
                    expanded |= affected.Add(project.path);
        } while (expanded);
        bool deepNative = extended || changes.Any(static path => !IsDocumentation(path) && (path.StartsWith("build/", StringComparison.Ordinal)
            || path.StartsWith("extern/", StringComparison.Ordinal) || path.Contains("/native/", StringComparison.Ordinal)
            || path.Contains("/build/", StringComparison.Ordinal) || path.Contains("reload", StringComparison.OrdinalIgnoreCase)
            || path is "Directory.Build.props" or "Directory.Build.targets" or "Directory.Solution.targets" or "global.json"));
        var executionExclusions = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (ValidationProject project in projects)
        {
            if (project.environment.Length != 0 && project.environment is not ("any" or "native-host")
                && !ValidationTestHosts.Applies(project.environment.Split(';')))
                executionExclusions.Add(project.path, $"This owner requires OS environment '{project.environment}'; it is not validated on this host.");
            else if (project.layer == "performance" && !deepNative && !changedTestExecutor && !ChangedTestClosure(project))
                executionExclusions.Add(project.path, "Performance sampling requires extended acceptance or an affected build/native/reload or performance-test input.");
        }
        JsonElement? development = graph.RootElement.EnumerateArray().Cast<JsonElement?>().FirstOrDefault(snapshot =>
            Path.GetRelativePath(root, snapshot!.Value.GetProperty("Path").GetString()!).Replace('\\', '/') == "entry/InnoEngine.Editor/InnoEngine.Editor.csproj");
        string target = development?.GetProperty("Properties").GetProperty("InnoDevelopmentTarget").GetString() ?? string.Empty;
        DocumentationPageInfo[] examplePages = documentation is null ? [] : ValidationDocumentation.SelectExamples(documentation, affected, full);
        var exampleExclusions = new Dictionary<DocumentationPageInfo, string>();
        foreach (DocumentationPageInfo page in examplePages)
        {
            string[] reasons = (page.examples?.projects ?? []).Select(name => byPath[documentation!.owners[name].path])
                .Select(project => executionExclusions.GetValueOrDefault(project.path)
                    ?? (target.Length != 0 && project.productTarget.Length != 0 && project.productTarget != target
                        ? $"Example owner '{project.path}' requires product target '{project.productTarget}'; this host selects '{target}'." : null))
                .OfType<string>().Distinct(StringComparer.Ordinal).ToArray();
            if (reasons.Length != 0)
                exampleExclusions.Add(page, string.Join(" ", reasons));
        }
        examplePages = examplePages.Where(page => !exampleExclusions.ContainsKey(page)).ToArray();
        foreach (DocumentationPageInfo page in examplePages)
        foreach (string owner in page.examples?.projects ?? [])
            documentationOwners.Add(documentation!.owners[owner].path);
        foreach (string owner in documentation?.changedReferenceOwners ?? new HashSet<string>())
            if (documentation!.catalog.references.Any(reference => reference.model == "source-clr" && reference.projects.Contains(owner, StringComparer.Ordinal)))
                documentationOwners.Add(documentation.owners[owner].path);
        var scriptPages = examplePages.Where(static page => page.examples?.mode is "runtime-script" or "editor-script")
            .Select(static page => page.id).ToHashSet(StringComparer.Ordinal);
        var nativeDocumentOwners = examplePages.Where(page => scriptPages.Contains(page.id))
            .SelectMany(static page => page.examples!.projects).Select(owner => documentation!.owners[owner].path).ToHashSet(StringComparer.Ordinal);
        bool scriptExamples = scriptPages.Count != 0;
        if (scriptExamples)
        {
            const string C_BUILD_ENTRY = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
            if (!byPath.ContainsKey(C_BUILD_ENTRY))
                throw new InvalidDataException("Script documentation examples require the existing Build entry and its evaluated projection closure.");
            documentationOwners.Add(C_BUILD_ENTRY);
            const string C_EDITOR_ENTRY = "entry/InnoEngine.Editor/InnoEngine.Editor.csproj";
            if (!byPath.ContainsKey(C_EDITOR_ENTRY))
                throw new InvalidDataException("Script documentation examples require the declared Editor product and its public script closure.");
            documentationOwners.Add(C_EDITOR_ENTRY);
        }

        var nativeOwners = new HashSet<string>(projects.Where(static project => project.native).Select(static project => project.path), StringComparer.Ordinal);
        do
        {
            expanded = false;
            foreach (ValidationProject project in projects)
                if (project.references.Any(nativeOwners.Contains))
                    expanded |= nativeOwners.Add(project.path);
        } while (expanded);
        foreach (ValidationProject project in projects.Where(static project => project.layer == "domain"))
            if (nativeOwners.Contains(project.path))
                throw new InvalidDataException($"Domain test transitively requires native preparation: {project.path}");
        // All owners of an executable example share its evaluated output context, including Host
        // examples that reuse an owner selected by a script example. Unrelated pages stay managed.
        do
        {
            expanded = false;
            foreach (DocumentationPageInfo page in examplePages)
            {
                string[] owners = (page.examples?.projects ?? []).Select(name => documentation!.owners[name].path).ToArray();
                if (owners.Any(owner => nativeOwners.Contains(owner) || nativeDocumentOwners.Contains(owner)))
                    foreach (string owner in owners)
                        expanded |= nativeDocumentOwners.Add(owner);
            }
        } while (expanded);
        var contexts = new Dictionary<string, ValidationContext>(StringComparer.Ordinal)
        {
            ["managed"] = new(new Dictionary<string, string>
            {
                ["Configuration"] = configuration, ["ArtifactsPath"] = artifacts,
                ["BuildProjectReferences"] = "true", ["BuildingInsideVisualStudio"] = "false",
                ["ShouldUnsetParentConfigurationAndPlatform"] = "false"
            }, graph.RootElement.Clone())
        };
        Dictionary<string, ValidationProject> nativeProjects = byPath;
        if (scriptExamples || nativeOwners.Any(owner => !executionExclusions.ContainsKey(owner)
            && (affected.Contains(owner) || documentationOwners.Contains(owner))))
        {
            string[] unavailable = inputs.Where(static pair => pair.Value.StartsWith("unavailable:", StringComparison.Ordinal)).Select(static pair => pair.Key).ToArray();
            if (unavailable.Length != 0)
                return new(null, new("Blocked", documentation is null ? "Documentation" : "NativeSources", root, commit, mergeBase,
                    ValidationInputs.Identity(inputs), documentation is null
                        ? "Select the required external documentation snapshot before preparing native inputs."
                        : "Selected owners require unavailable pinned native sources. Prepare these inputs explicitly, then generate a new plan.",
                    nativeOwners.Where(owner => !executionExclusions.ContainsKey(owner) && (affected.Contains(owner) || documentationOwners.Contains(owner)))
                        .Concat(nativeDocumentOwners).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray(),
                    documentation is null ? new(StringComparer.Ordinal) : unavailable.ToDictionary(static path => path,
                        path => inputs[path]["unavailable:".Length..], StringComparer.Ordinal)));
            if (development is null)
                throw new InvalidDataException("Native validation requires the existing development product declaration.");
            JsonElement host = development.Value.GetProperty("Properties");
            target = host.GetProperty("InnoDevelopmentTarget").GetString() ?? string.Empty;
            string productProperties = host.GetProperty("InnoDevelopmentEditorBuildProperties").GetString() ?? string.Empty;
            if (target.Length == 0 || !File.Exists(productProperties))
                throw new InvalidDataException("This SDK host has no supported native validation product. Required checks are blocked.");
            var nativeProperties = new Dictionary<string, string>(contexts["managed"].properties)
            {
                ["ArtifactsPath"] = Path.Combine(artifacts, "native", target),
                ["InnoNativeTarget"] = target,
                ["InnoToolTarget"] = target,
                ["InnoProductBuildProperties"] = productProperties,
                ["InnoTestNativeTarget"] = target
            };
            var evaluationProperties = new Dictionary<string, string>(properties);
            foreach (var pair in nativeProperties)
                evaluationProperties[pair.Key] = pair.Value;
            string nativeCapture = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet,
                paths.Select(path => Path.Combine(root, path)).ToArray(), evaluationProperties, cancellation).ConfigureAwait(false);
            using JsonDocument nativeGraph = JsonDocument.Parse(nativeCapture);
            RequireCompilerInputs(nativeGraph.RootElement);
            contexts.Add("native", new(nativeProperties, nativeGraph.RootElement.Clone()));
            nativeProjects = nativeGraph.RootElement.EnumerateArray().Select(snapshot => ValidationProject.Read(root, snapshot))
                .ToDictionary(static project => project.path, StringComparer.Ordinal);
            RecordStage("native-graph");
        }

        string[] policyInputs = inputs.Keys.Where(static path => path == "AGENTS.md" || path.StartsWith(".agents/rulebook/", StringComparison.Ordinal)
            || path == ValidationCiContracts.C_RULE_LOADER).Concat(RuleReviewInputs()).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        var toolingOwners = new HashSet<string>(StringComparer.Ordinal);
        AddToolOwner("tools/Inno.Tooling.Validation/Inno.Tooling.Validation.csproj");
        string[] toolingInputs = CapturedInputs(toolingOwners.SelectMany(owner => byPath[owner].inputs)
            .Concat(inputs.Keys.Where(static path => path is "global.json" or "Directory.Build.props" or "Directory.Build.targets"
                or "Directory.Solution.targets" or "InnoEngine.sln"))).Order(StringComparer.Ordinal).ToArray();
        var inputSets = projects.ToDictionary(static project => project.path,
            project => CapturedInputs(project.inputs.Concat(nativeProjects[project.path].inputs)), StringComparer.Ordinal);
        inputSets.Add("tooling", toolingInputs);
        inputSets.Add("native-sources", submoduleInputs);
        var checks = new List<ValidationCheck>
        {
            new("instructions", "instructions", "AGENTS.md", "Rule preservation, discoverability, budgets and links.", "any", [], policyInputs, true, null),
            new("architecture", "architecture", "tools/Inno.Tooling.Architecture", "Cheap repository-wide source, ownership and architecture invariants.", "any", [],
                inputs.Keys.Where(static path => path.Split('/')[0] is "src" or "entry" or "backends" or "platforms" or "build" or "tools"
                    || path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                    || path.EndsWith(".cs.txt", StringComparison.OrdinalIgnoreCase) && !path.StartsWith(".agents/records/", StringComparison.Ordinal)
                    || Path.GetExtension(path) is ".csproj" or ".props" or ".targets"
                    || path is "InnoEngine.sln" or "Directory.Build.props" or "Directory.Build.targets" or "Directory.Solution.targets")
                    .Order(StringComparer.Ordinal).ToArray(), true, null)
        };
        if (changes.Any(static path => path.StartsWith(".agents/", StringComparison.Ordinal) || path is "AGENTS.md" or "CONTRIBUTING.md") || full)
            checks.Add(new("agent-documents", "agent-documents", ".agents", "Rule, skill and record structure and links; no product compilation.", "any", [],
                inputs.Keys.Where(static path => path.StartsWith(".agents/", StringComparison.Ordinal) || path is "AGENTS.md" or "CONTRIBUTING.md")
                    .Append("@tooling").Order(StringComparer.Ordinal).ToArray(), true, null));
        if (documentation is not null || affected.Any(owner => !byPath[owner].test) || full)
            checks.Add(new("documentation", documentation is null ? "documentation-missing" : "documentation", "InnoEngine.Docs",
                documentation is null ? "Required external documentation snapshot was not supplied. Pass --docs-cache-root and --docs-snapshot."
                    : "Canonical external project entries and selected immutable reference payloads.", "any", [],
                inputs.Keys.Where(static path => path.EndsWith(".csproj", StringComparison.Ordinal)).Append("@tooling").ToArray(), true, null,
                documentation: documentation is null ? null : new([], [], documentation.catalog.contentIdentity, configuration, null)));
        if (full || changes.Any(static path => path.StartsWith(".github/", StringComparison.Ordinal) || IsCiContractInput(path)
            || path.StartsWith("tools/Inno.Tooling.Validation/", StringComparison.Ordinal)))
            checks.Add(new("ci-contracts", "ci-contracts", "tests/ci", "Fail-closed aggregation contracts for missing, skipped, canceled, empty and tampered evidence.",
                "any", [], inputs.Keys.Where(IsCiContractInput).Append("@tooling").ToArray(), true, null));
        foreach (ValidationProject project in projects.OrderBy(static project => project.path, StringComparer.Ordinal))
        {
            bool native = nativeOwners.Contains(project.path);
            string context = native || nativeDocumentOwners.Contains(project.path) ? "native" : "managed";
            Dictionary<string, ValidationProject> selectedGraph = context == "native" ? nativeProjects : byPath;
            var closure = new HashSet<string>(StringComparer.Ordinal);
            Visit(selectedGraph[project.path]);
            if (project.test && project.layer is not ("domain" or "contract" or "integration" or "native" or "product" or "performance"))
                throw new InvalidDataException($"Test execution layer is absent or unknown: {project.path} ({project.layer})");
            if (project.layer == "domain" && native)
                throw new InvalidDataException($"Domain test transitively requires native preparation: {project.path}");
            string[] selectedInputs = closure.Select(static path => "@" + path).Append("@tooling")
                .Concat(context == "native" ? ["@native-sources"] : []).Order(StringComparer.Ordinal).ToArray();
            bool selected = affected.Contains(project.path);
            bool compile = selected || documentationOwners.Contains(project.path);
            string environment = project.environment.Length != 0 ? project.environment : native ? "native-host" : "any";
            string? exclusion = executionExclusions.GetValueOrDefault(project.path)
                ?? (project.productTarget.Length != 0 && target.Length != 0 && project.productTarget != target
                    ? $"Product '{project.productTarget}' requires its own supported target environment; current SDK host selects '{target}'." : null);
            if (compile)
                checks.Add(new("build:" + project.path, "build", project.path, "Changed owner or transitive consumer; compile the shared union once.",
                    environment, [], selectedInputs, false, exclusion, context, "compilation"));
            if (project.test)
                checks.Add(new("test:" + project.path, "test", project.path, selected
                    ? "Affected behavior or contract consumer; compiled SupportedOSPlatform requirements distinguish host exclusions from missing execution."
                    : "No affected input in its evaluated closure.",
                    environment, ["build:" + project.path], [..selectedInputs, ValidationRunner.C_TEST_EXECUTOR], project.reusable && project.layer is not ("performance" or "product"),
                    selected ? exclusion : "Outside the affected evaluated dependency closure.", context, project.layer));

            void Visit(ValidationProject node)
            {
                if (!closure.Add(node.path))
                    return;
                foreach (string dependency in node.references)
                    if (selectedGraph.TryGetValue(dependency, out ValidationProject? next))
                        Visit(next);
                    else
                        throw new InvalidDataException($"Unknown evaluated dependency '{dependency}' from '{node.path}'.");
            }
        }
        if (affected.Any(owner => !executionExclusions.ContainsKey(owner)) && !checks.Any(static check => check.kind == "test" && check.exclusion is null))
            throw new InvalidDataException("Affected production inputs have no selected behavior tests. Declare their real public contract consumers.");
        ValidationCheck[] builds = checks.Where(static check => check.kind == "build" && check.exclusion is null).ToArray();
        foreach (IGrouping<string?, ValidationCheck> group in builds.GroupBy(static check => check.context))
        {
            string[] apiOwners = group.Select(static check => check.owner).Where(owner => !byPath[owner].test)
                .Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
            checks.Add(new("compiled-api:" + group.Key, "api", "tools/Inno.Tooling.Architecture", "Public/protected signatures in affected owners and their evaluated dependencies.",
                "any", group.Select(static check => check.id).ToArray(), group.SelectMany(static check => check.inputs).Distinct().Order(StringComparer.Ordinal).ToArray(), true,
                apiOwners.Length == 0 ? "Only test implementations changed; their unchanged production dependencies have no affected API owner." : null,
                group.Key, "contract", apiOwners));
        }
        if (documentation is not null)
            AddDocumentationChecks();
        ValidationCheck? editorBuild = builds.SingleOrDefault(static check => check.owner == "entry/InnoEngine.Editor/InnoEngine.Editor.csproj");
        if (editorBuild is not null)
            checks.Add(new("editor-products", "products", editorBuild.owner, "Real renderer startup, product workflows and native panel retirement for the affected Editor product.",
                "native-host", [editorBuild.id], editorBuild.inputs.Concat(inputs.Keys.Where(static path => path.StartsWith("tests/fixtures/editor-ui/", StringComparison.Ordinal)
                    || path is "tests/Invoke-EditorSmoke.ps1" or "tests/Invoke-EditorUiProductJourney.ps1")).Distinct().Order(StringComparer.Ordinal).ToArray(), false, null, "native", "product"));
        if (editorBuild is not null && deepNative)
            checks.Add(new("native-hot-builds", "hot-builds", editorBuild.owner,
                "Ten unchanged native builds must retain content selections and start no native compiler or binding generation.",
                "native-host", [editorBuild.id], editorBuild.inputs.Append("tests/Invoke-BuildCacheChecks.ps1").ToArray(), false, null, "native", "performance"));
        if (extended)
            checks.Insert(0, new("cold-inputs", "cold-inputs", "build", "Explicit cold acceptance requires an initially unprepared native cache.",
                "native-host", [], ["@tooling"], false, null, "native", "performance"));
        if (full && editorBuild?.context == "native")
        {
            const string C_BUILD_ENTRY = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
            if (!byPath.ContainsKey(C_BUILD_ENTRY))
                throw new InvalidDataException("Complete product acceptance requires the declared Build entry.");
            string releaseContext = configuration == "Release" ? "native" : "native-release";
            Dictionary<string, ValidationProject> releaseProjects = nativeProjects;
            if (releaseContext != "native")
            {
                var releaseProperties = new Dictionary<string, string>(contexts["native"].properties)
                {
                    ["Configuration"] = "Release",
                    ["ArtifactsPath"] = Path.Combine(artifacts, "native-release", target)
                };
                var evaluationProperties = new Dictionary<string, string>(properties);
                foreach (var pair in releaseProperties)
                    evaluationProperties[pair.Key] = pair.Value;
                string releaseCapture = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet,
                    paths.Select(path => Path.Combine(root, path)).ToArray(), evaluationProperties, cancellation).ConfigureAwait(false);
                using JsonDocument releaseGraph = JsonDocument.Parse(releaseCapture);
                RequireCompilerInputs(releaseGraph.RootElement);
                contexts.Add(releaseContext, new(releaseProperties, releaseGraph.RootElement.Clone()));
                releaseProjects = releaseGraph.RootElement.EnumerateArray().Select(snapshot => ValidationProject.Read(root, snapshot))
                    .ToDictionary(static project => project.path, StringComparer.Ordinal);
                foreach (ValidationProject project in releaseProjects.Values)
                    inputSets[project.path] = inputSets[project.path].Concat(CapturedInputs(project.inputs)).Distinct(StringComparer.Ordinal).ToArray();
            }
            var releaseInputs = new HashSet<string>(StringComparer.Ordinal) { "@tooling", "@native-sources" };
            var releaseBuilds = new List<string>();
            string[] releaseRoots = [editorBuild.owner, C_BUILD_ENTRY];
            foreach (string owner in releaseRoots)
            {
                VisitRelease(owner);
                string id = (releaseContext == "native" ? "build:" : "release-build:") + owner;
                releaseBuilds.Add(id);
                if (releaseContext != "native")
                    checks.Add(new(id, "build", owner, "Release product acceptance prepares its configuration once before execution and packaging.",
                        "native-host", [], releaseInputs.Order(StringComparer.Ordinal).ToArray(), false, null, releaseContext, "compilation"));
            }
            if (releaseContext != "native")
            {
                checks.Add(new("compiled-api:release", "api", "tools/Inno.Tooling.Architecture", "Release symbols retain the same public boundary requirements.",
                    "native-host", releaseBuilds.ToArray(), releaseInputs.Order(StringComparer.Ordinal).ToArray(), true, null, releaseContext, "contract",
                    releaseInputs.Where(input => input.StartsWith('@') && releaseProjects.ContainsKey(input[1..])).Select(static input => input[1..]).Order(StringComparer.Ordinal).ToArray()));
                checks.Add(new("editor-release-products", "products", editorBuild.owner, "Execute the actual Release Editor renderer, workflow and lifecycle before packaging.",
                    "native-host", releaseBuilds.ToArray(), editorBuild.inputs.Concat(releaseInputs).Distinct(StringComparer.Ordinal).ToArray(), false, null, releaseContext, "product"));
            }
            checks.Add(new("support-pack", "support-pack", C_BUILD_ENTRY, "Publish and verify the source-independent Release Player Support Pack under its existing read lease.",
                "native-host", releaseBuilds.ToArray(), releaseInputs.Order(StringComparer.Ordinal).ToArray(), false, null, releaseContext, "product"));
            JsonElement[] players = contexts[releaseContext].sdkGraph.EnumerateArray().Where(snapshot =>
                snapshot.GetProperty("Properties").GetProperty("InnoProductId").GetString() == "player"
                && snapshot.GetProperty("Properties").GetProperty("InnoProductTarget").GetString() == target).ToArray();
            if (players.Length != 1)
                throw new InvalidDataException("Full host acceptance requires exactly one actual Player product declaration.");
            string playerOwner = Path.GetRelativePath(root, players[0].GetProperty("Path").GetString()!).Replace('\\', '/');
            VisitRelease(playerOwner);
            checks.Add(new("native-aot-restore", "restore-aot", playerOwner,
                "Restore the actual host Player with NativeAOT SDK inputs in an independently owned restore layout.",
                "native-host", releaseBuilds.ToArray(), releaseInputs.Order(StringComparer.Ordinal).ToArray(), false, null, releaseContext, "contract"));
            // Restore-only inputs still invalidate the bundle, but only built roots provide compiled output instances.
            checks.Add(new("release-bundle", "release-bundle", editorBuild.owner, "Archive the exact verified Release outputs after every required functional result passes.",
                "native-host", checks.Where(static check => check.exclusion is null).Select(static check => check.id).ToArray(),
                releaseInputs.Order(StringComparer.Ordinal).ToArray(), false, null, releaseContext, "product", releaseRoots));

            void VisitRelease(string owner)
            {
                if (!releaseInputs.Add("@" + owner))
                    return;
                if (!releaseProjects.TryGetValue(owner, out ValidationProject? project))
                    throw new InvalidDataException($"Unknown Release dependency: {owner}");
                foreach (string reference in project.references)
                    VisitRelease(reference);
            }
        }
        RecordStage("selection-and-release-graph");
        ValidationEnvironment.ToolSnapshot toolSnapshot = await toolCapture.ConfigureAwait(false);
        DotNetSdkDescriptor sdk = toolSnapshot.sdk;
        string environmentIdentity = await ValidationEnvironment.CaptureAsync(root, sdk.hostPath, cancellation, contexts.Values, toolSnapshot).ConfigureAwait(false);
        RecordStage("execution-environment");
        var phases = contexts.Keys.Select(static (
            string context,
            int index
        ) => new KeyValuePair<string, int>(context, index + 1))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        // Policy and architecture govern compilation; unrelated documentation/contract failures do not.
        // Cold acceptance is a native preparation prerequisite, not a global stop for managed checks.
        checks = checks.Select(check => check with
        {
            prerequisites = check.prerequisites
                .Concat(check.kind == "build" ? ["instructions", "architecture"] : Array.Empty<string>())
                .Concat(extended && check.context is not (null or "managed") && check.kind != "cold-inputs"
                    ? ["cold-inputs"] : Array.Empty<string>())
                .Distinct(StringComparer.Ordinal).ToArray()
        }).ToList();
        ValidationCheck[] execution = checks.OrderBy(check => check.kind == "cold-inputs" ? -1 : check.kind == "release-bundle" ? int.MaxValue
                : check.context is null ? 0 : phases.GetValueOrDefault(check.context, phases.Count + 1))
            .ThenBy(static check => check.kind switch { "build" => 0, "api" => 1, "documentation-api" or "documentation-examples" => 2, "test" => 3, _ => 4 }).ToArray();
        var preceding = new HashSet<string>(StringComparer.Ordinal);
        foreach (ValidationCheck check in execution)
        {
            if (check.exclusion is null && check.prerequisites.Any(prerequisite => !preceding.Contains(prerequisite)))
                throw new InvalidDataException($"Required prerequisites are missing or scheduled after '{check.id}'.");
            preceding.Add(check.id);
        }
        return new(new(root, mergeBase, commit, ValidationInputs.Identity(inputs), environmentIdentity, configuration, sdk.hostPath, artifacts,
            full, changes.Order(StringComparer.Ordinal).ToArray(), affected.Order(StringComparer.Ordinal).ToArray(), execution, diagnostics.ToArray(), inputs, inputSets, contexts, extended, timings, documentation?.binding), null);

        void AddDocumentationChecks()
        {
            ValidationDocumentation.Selection selected = documentation!;
            foreach (var excluded in exampleExclusions)
                checks.Add(new("documentation-examples:excluded:" + excluded.Key.id, "documentation-examples", "InnoEngine.Docs",
                    "The example retains its declared product and environment obligations in the corresponding matrix cell.", "any", [],
                    ["@tooling"], false, excluded.Value, layer: "contract", documentation: new([], [excluded.Key.id],
                        ValidationDocumentation.PageIdentity([excluded.Key]), configuration, null)));
            var owners = affected.Concat(selected.changedReferenceOwners.Select(owner => selected.owners[owner].path))
                .Where(owner => !byPath[owner].test && !executionExclusions.ContainsKey(owner)
                && (byPath[owner].productTarget.Length == 0 || byPath[owner].productTarget == target))
                .Where(owner => selected.owners.GetValueOrDefault(Path.GetFileNameWithoutExtension(owner))?.nativeReference != true)
                .ToHashSet(StringComparer.Ordinal);
            var covered = new HashSet<string>(StringComparer.Ordinal);
            var captures = selected.catalog.references.Where(reference => reference.model == "source-clr" && reference.configuration == configuration
                && (reference.target is null || reference.target == target))
                .SelectMany(reference => reference.projects.Where(name => selected.owners.TryGetValue(name, out ValidationDocumentation.Owner? owner)
                        && owners.Contains(owner.path))
                    .Select(name => (reference, name, path: selected.owners[name].path)))
                .GroupBy(item => (context: nativeOwners.Contains(item.path) || nativeDocumentOwners.Contains(item.path) ? "native" : "managed", item.reference.configuration, item.reference.target));
            foreach (var group in captures)
            {
                string[] paths = group.Select(static item => item.path).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
                covered.UnionWith(paths);
                ValidationCheck[] required = builds.Where(check => paths.Contains(check.owner, StringComparer.Ordinal)).ToArray();
                string[] references = group.Select(static item => item.reference.path).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
                string identity = ValidationInputs.Identity(group.Select(item => new KeyValuePair<string, string>(item.path,
                    selected.sourceIdentities[item.reference.path + "\0" + item.name].identity)));
                checks.Add(new("documentation-api:" + group.Key.context + ":" + (group.Key.target ?? "default"), "documentation-api", "InnoEngine.Docs",
                    "Selected owner declarations and XML from protected source reference payloads.", "any",
                    required.Select(static check => check.id).Append("documentation").ToArray(),
                    required.SelectMany(static check => check.inputs).Append("@tooling").Distinct(StringComparer.Ordinal).ToArray(), true, null,
                    group.Key.context, "contract", paths, new(references, [], identity, configuration, group.Key.target)));
            }
            foreach (string owner in owners.Except(covered).Order(StringComparer.Ordinal))
            {
                ValidationCheck? prepared = builds.SingleOrDefault(check => check.owner == owner);
                checks.Add(new("documentation-api-missing:" + owner, "documentation-missing", owner,
                    "Required source API capture is absent for the selected configuration and target. Prepared owners may export unverified facts for review; capture does not pass this requirement.",
                    "any", prepared is null ? ["documentation"] : ["documentation", prepared.id],
                    prepared?.inputs ?? ["@" + owner], false, null, prepared?.context, "contract",
                    prepared is null ? null : [owner], prepared is null ? null : new([], [], "", configuration,
                        prepared.context == "native" ? target : null)));
            }
            // Foreign product execution stays excluded. Its explicitly empty authored public surface
            // can still be checked against actual SDK-selected source without borrowing host binaries.
            var emptyForeign = selected.catalog.references.Where(reference => reference.model == "source-clr"
                    && reference.configuration == configuration)
                .SelectMany(reference => reference.projects.Where(name => selected.owners.TryGetValue(name, out ValidationDocumentation.Owner? owner)
                        && !owner.test && !owner.nativeReference && byPath[owner.path].productTarget.Length != 0
                        && byPath[owner.path].productTarget != target
                        && (affected.Contains(owner.path) || selected.changedReferenceOwners.Contains(name))
                        && selected.sourceIdentities[reference.path + "\0" + name].empty)
                    .Select(name => (reference, name, path: selected.owners[name].path)))
                .GroupBy(static item => item.reference.target);
            foreach (var group in emptyForeign)
            {
                string[] paths = group.Select(static item => item.path).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
                string[] references = group.Select(static item => item.reference.path).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
                string identity = ValidationInputs.Identity(group.Select(item => new KeyValuePair<string, string>(item.path,
                    selected.sourceIdentities[item.reference.path + "\0" + item.name].identity)));
                checks.Add(new("documentation-api:source:" + (group.Key ?? "default"), "documentation-api", "InnoEngine.Docs",
                    "Verify the recorded empty public surface from actual target-selected source; this does not certify foreign product execution.",
                    "any", ["documentation"], CapturedInputs(paths.SelectMany(path => byPath[path].inputs)).Append("@tooling").Order(StringComparer.Ordinal).ToArray(),
                    true, null, "managed", "contract", paths, new(references, [], identity, configuration, group.Key)));
            }
            foreach (var group in examplePages.GroupBy(page => scriptPages.Contains(page.id)
                || (page.examples?.projects ?? []).Any(name => nativeOwners.Contains(selected.owners[name].path)
                    || nativeDocumentOwners.Contains(selected.owners[name].path)) ? "native" : "managed"))
            {
                string[] paths = group.SelectMany(static page => page.examples?.projects ?? []).Select(name => selected.owners[name].path)
                    .Distinct(StringComparer.Ordinal).ToArray();
                ValidationCheck[] required = builds.Where(check => paths.Contains(check.owner, StringComparer.Ordinal)
                    || group.Key == "native" && scriptExamples && check.owner is "entry/InnoEngine.Build/InnoEngine.Build.csproj" or "entry/InnoEngine.Editor/InnoEngine.Editor.csproj").ToArray();
                string[] pages = group.Select(static page => page.id).Order(StringComparer.Ordinal).ToArray();
                checks.Add(new("documentation-examples:" + group.Key, "documentation-examples", "InnoEngine.Docs",
                    "Canonical example blocks and their declared host or script context; no Markdown reparse or namespace guessing.", "any",
                    required.Select(static check => check.id).Append("documentation").ToArray(),
                    required.SelectMany(static check => check.inputs).Append("@tooling").Distinct(StringComparer.Ordinal).ToArray(), true, null,
                    group.Key, "contract", paths, new([], pages, ValidationDocumentation.PageIdentity(group), configuration,
                        group.Key == "native" ? target : null)));
            }
        }

        Task<string> GitAsync(params string[] arguments) => ValidationProcess.RunAsync("git", arguments, root, null, cancellation);

        void AddToolOwner(string path)
        {
            // A consumer/fixture may execute a prebuilt tool without containing its source project.
            // Its actual binary closure is still captured by ValidationEnvironment.
            if (!byPath.TryGetValue(path, out ValidationProject? project) || !toolingOwners.Add(path))
                return;
            foreach (string reference in project.references)
                AddToolOwner(reference);
        }

        IEnumerable<string> RuleReviewInputs()
        {
            const string C_REGISTRY = ".agents/rulebook/coverage.json";
            if (!inputs.ContainsKey(C_REGISTRY) || inputs[C_REGISTRY] == "missing")
                yield break;
            using JsonDocument registry = ReadRegistry();
            foreach (JsonElement unit in registry.RootElement.GetProperty("units").EnumerateArray())
            {
                if (unit.TryGetProperty("translation", out JsonElement translation))
                    yield return ReviewPath(translation);
                if (unit.TryGetProperty("approvedAddition", out JsonElement addition)
                    && addition.TryGetProperty("translation", out translation))
                    yield return ReviewPath(translation);
            }

            string ReviewPath(JsonElement translation)
            {
                string path = translation.GetProperty("review").GetString()!.Split('#', 2)[0];
                ValidationInputs.Resolve(root, path);
                return path;
            }

            JsonDocument ReadRegistry()
            {
                try
                {
                    return JsonDocument.Parse(File.ReadAllBytes(ValidationInputs.Resolve(root, C_REGISTRY)));
                }
                catch (JsonException failure)
                {
                    throw new InvalidDataException("Rule registry is invalid: " + C_REGISTRY, failure);
                }
            }
        }

        bool ChangedTestClosure(ValidationProject project)
        {
            var visited = new HashSet<string>(StringComparer.Ordinal);
            return Visit(project);

            bool Visit(ValidationProject owner)
            {
                if (!visited.Add(owner.path))
                    return false;
                return changes.Any(path => path.StartsWith("tests/", StringComparison.Ordinal)
                        && (path.StartsWith(DirectoryOf(owner.path), StringComparison.Ordinal) || owner.inputs.Contains(path, StringComparer.Ordinal)))
                    || owner.references.Any(reference => byPath.TryGetValue(reference, out ValidationProject? dependency) && Visit(dependency));
            }
        }

        string[] CapturedInputs(IEnumerable<string> declared)
        {
            string[] paths = declared.ToArray();
            return paths.Where(inputs.ContainsKey).Concat(submoduleInputs.Where(parent =>
                paths.Any(path => path.StartsWith(parent + "/", StringComparison.Ordinal)))).Distinct(StringComparer.Ordinal).ToArray();
        }

        void RequireCompilerInputs(JsonElement snapshot)
        {
            foreach (JsonElement project in snapshot.EnumerateArray())
            foreach (JsonElement variant in project.GetProperty("Variants").EnumerateArray().Append(project))
            foreach (JsonElement item in variant.GetProperty("Items").GetProperty("Compile").EnumerateArray())
            {
                string path = Path.GetRelativePath(root, item.GetProperty("FullPath").GetString()!).Replace('\\', '/');
                if (path.StartsWith("../", StringComparison.Ordinal) || Path.IsPathRooted(path) || inputs.ContainsKey(path)
                    || submoduleInputs.Any(parent => path.StartsWith(parent + "/", StringComparison.Ordinal)))
                    continue;
                if (File.Exists(ValidationInputs.Resolve(root, path)))
                    throw new InvalidDataException($"The SDK selected an untracked ignored compiler input: {path}. Make its source ownership explicit before validation.");
            }
        }

        void RecordStage(string name)
        {
            double now = elapsed.Elapsed.TotalSeconds;
            timings.Add(name, now - previousStage);
            Console.WriteLine($"Planning {name}: {now - previousStage:F2}s");
            previousStage = now;
        }
    }

    private static string DirectoryOf(string project) => project[..(project.LastIndexOf('/') + 1)];

    private static bool IsCiContractInput(string path)
        => path.StartsWith("tests/ci/", StringComparison.Ordinal)
            || path.StartsWith("tests/fixtures/editor-ui/", StringComparison.Ordinal)
            || path == "tests/Invoke-EditorUiProductJourney.ps1";

    private static bool IsDocumentation(string path)
        => path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
            || path == ".agents/rulebook/coverage.json"
            || (path.StartsWith("docs/", StringComparison.Ordinal) || path.StartsWith(".agents/records/", StringComparison.Ordinal))
                && (Path.GetExtension(path) is ".json" or ".csv" or ".png" or ".jpg" or ".svg" || path.EndsWith(".cs.txt", StringComparison.Ordinal));
}
