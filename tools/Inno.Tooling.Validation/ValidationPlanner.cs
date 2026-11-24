using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Tooling.Architecture;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

internal static partial class ValidationPlanner
{
    internal static async Task<ValidationPlanningResult> CreateAsync(
        string root,
        string baseline,
        string dotnet,
        string configuration,
        string artifacts,
        bool full,
        bool extended = false,
        string? documentationCache = null,
        string? documentationSnapshot = null,
        string? documentationBaseline = null,
        ValidationProfile profile = ValidationProfile.PullRequest,
        string? nativeDependencies = null,
        string? backendStore = null,
        string? rebuildBackends = null,
        CancellationToken cancellationToken = default
    ) {
        if (rebuildBackends is not null && backendStore is null)
        {
            throw new ArgumentException("Explicit backend production requires a backend store.");
        }
        if (backendStore is not null)
        {
            if (nativeDependencies is not null)
            {
                throw new ArgumentException("The backend store owns the native selection for this plan.");
            }
            string store = PathBoundary.RequireUnlinkedPath(root, Path.GetFullPath(backendStore, root));
            PathBoundary.RequireContained(Path.Combine(root, "artifacts"), store);
            backendStore = Path.GetRelativePath(root, store).Replace('\\', '/');
            nativeDependencies = backendStore + "/selection.json";
        }
        if (profile == ValidationProfile.NativeProducer && (nativeDependencies is not null || full || extended))
        {
            throw new ArgumentException(
                "Native producer acceptance requires explicit source production and cannot certify full functional acceptance."
            );
        }
        if (nativeDependencies is not null)
        {
            string selected = Path.GetFullPath(nativeDependencies, root);
            nativeDependencies = Path.GetRelativePath(root, PathBoundary.RequireUnlinkedPath(root, selected))
                .Replace('\\', '/');
            if (extended)
            {
                throw new ArgumentException("Consume-only validation cannot certify a cold third-party rebuild.");
            }
        }
        Stopwatch elapsed = Stopwatch.StartNew();
        var timings = new Dictionary<string, double>(StringComparer.Ordinal);
        double previousStage = 0;
        Dictionary<string, string> inputs = await ValidationInputs.CaptureAsync(root, cancellationToken).ConfigureAwait(false);
        string commit = (await GitAsync("rev-parse", "HEAD").ConfigureAwait(false)).Trim();
        string mergeBase = (await GitAsync("merge-base", baseline, commit).ConfigureAwait(false)).Trim();
        var changes = new HashSet<string>(
            (await GitAsync("diff", "--name-only", "-z", "--no-renames", mergeBase, "HEAD").ConfigureAwait(false))
                .Split('\0', StringSplitOptions.RemoveEmptyEntries),
            StringComparer.Ordinal
        );
        changes.UnionWith(
            (await GitAsync(
                "diff",
                "--name-only",
                "-z",
                "--no-renames",
                "HEAD"
            ).ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries)
        );
        changes.UnionWith(
            (await GitAsync(
                "diff",
                "--cached",
                "--name-only",
                "-z",
                "--no-renames",
                "HEAD"
            ).ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries)
        );
        changes.UnionWith(await ValidationInputs.FindContentChangesAsync(root, commit, inputs, cancellationToken).ConfigureAwait(false));
        changes.UnionWith(
            (await GitAsync(
                "ls-files",
                "--others",
                "--exclude-standard",
                "-z"
            ).ConfigureAwait(false)).Split('\0', StringSplitOptions.RemoveEmptyEntries)
        );
        string[] paths = inputs.Keys
            .Where(static path => (
                path.EndsWith(".csproj", StringComparison.Ordinal)
                && path.Split('/')[0] is "src" or "entry" or "build" or "backends" or "platforms" or "tools" or "tests"
            ))
            .Where(path => inputs[path] != "missing")
            .Order(StringComparer.Ordinal)
            .ToArray();
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
        if (nativeDependencies is not null)
        {
            properties["InnoNativeDependencyManifest"] = Path.Combine(root, nativeDependencies);
        }
        ValidationEnvironment.ToolSnapshot toolSnapshot = await ValidationEnvironment.CaptureToolsAsync(
            root,
            dotnet,
            cancellationToken
        ).ConfigureAwait(false);
        DotNetSdkDescriptor sdk = toolSnapshot.sdk;
        RecordStage("sdk-environment");
        string captured = await ManagedProjectGraphCapture.CaptureSelectedAsync(
            root,
            sdk,
            paths.Select(path => Path.Combine(root, path)).ToArray(),
            properties,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
        RecordStage("sdk-capture");
        using JsonDocument graph = JsonDocument.Parse(captured);
        string[] submoduleInputs = inputs
            .Where(pair => (
                pair.Value.StartsWith("unavailable:", StringComparison.Ordinal)
                || new DirectoryInfo(Path.GetFullPath(pair.Key, root)) is { Exists: true, LinkTarget: null }
            ))
            .Select(static pair => pair.Key)
            .Order(StringComparer.Ordinal)
            .ToArray();
        RequireCompilerInputs(graph.RootElement);
        ValidationProject[] projects = graph.RootElement
            .EnumerateArray()
            .Select(snapshot => ValidationProject.Read(root, snapshot))
            .ToArray();
        Dictionary<string, ValidationProject> byPath = projects.ToDictionary(static project => project.path, StringComparer.Ordinal);
        RecordStage("managed-graph");
        var affected = new HashSet<string>(StringComparer.Ordinal);
        var documentationOwners = new HashSet<string>(StringComparer.Ordinal);
        ValidationDocumentation.Selection? documentation = await ValidationDocumentation.CaptureAsync(
            documentationCache,
            documentationSnapshot,
            byPath,
            documentationBaseline,
            cancellationToken
        ).ConfigureAwait(false);
        List<string> diagnostics = [];
        Dictionary<string, string[]> contracts = ValidationCiContracts.Select(
            root,
            inputs.Keys,
            changes,
            full || extended || profile == ValidationProfile.Full,
            out HashSet<string> workflowInputs
        );
        foreach (string change in changes)
        {
            ValidationProject[] owners = projects.Where(project => (
                project.inputs.Contains(change, StringComparer.Ordinal)
                || change.StartsWith(DirectoryOf(project.path), StringComparison.Ordinal)
            )).ToArray();
            foreach (ValidationProject owner in owners)
            {
                affected.Add(owner.path);
            }
            if (owners.Length == 0
                && inputs.GetValueOrDefault(change, "missing") != "missing"
                && !IsDocumentation(change)
                && change != ValidationRunner.C_TEST_EXECUTOR
                && !workflowInputs.Contains(change) && !IsEditorProductInput(change) && IsExecutable(change)
            ) {
                if (change is "global.json" or "Directory.Solution.targets" or "InnoEngine.sln")
                {
                    affected.UnionWith(paths);
                }
                else
                {
                    throw new InvalidDataException($"Executable input '{change}' has no declared project or workflow owner.");
                }
            }
        }
        var removedEntries = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        string[] removed = changes.Where(path => (
            !IsDocumentation(path)
            && (!inputs.TryGetValue(path, out string? value) || value == "missing")
        )).ToArray();
        if (removed.Length != 0)
        {
            string checkout = Path.Combine(Path.GetTempPath(), "InnoValidationBase-" + Guid.NewGuid().ToString("N"));
            await GitAsync("worktree", "add", "--quiet", "--detach", checkout, mergeBase).ConfigureAwait(false);
            try
            {
                string listing = await GitAsync("ls-tree", "-rz", "--name-only", mergeBase).ConfigureAwait(false);
                string[] priorPaths = listing.Split('\0', StringSplitOptions.RemoveEmptyEntries);
                string[] priorProjects = priorPaths
                    .Where(path => (
                        path.EndsWith(".csproj", StringComparison.Ordinal)
                        && path.Split('/')[0] is "src" or "entry" or "build" or "backends" or "platforms" or "tools" or "tests"
                    ))
                    .Select(path => Path.Combine(checkout, path))
                    .ToArray();
                var baselineProperties = new Dictionary<string, string>(properties)
                {
                    ["ArtifactsPath"] = Path.Combine(checkout, "artifacts", "baseline")
                };
                baselineProperties.Remove("InnoNativeDependencyManifest");
                string prior = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                    checkout,
                    sdk,
                    priorProjects,
                    baselineProperties,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                using JsonDocument snapshots = JsonDocument.Parse(prior);
                HashSet<string> deleted = removed.ToHashSet(StringComparer.Ordinal);
                var mapped = new HashSet<string>(StringComparer.Ordinal);
                Dictionary<string, string[]> priorContracts = ValidationCiContracts.Select(
                    checkout,
                    priorPaths,
                    deleted,
                    false,
                    out HashSet<string> priorScripts
                );
                mapped.UnionWith(deleted.Where(priorScripts.Contains));
                if (priorContracts.Count > 0)
                {
                    HashSet<string> survivingInputs = changes
                        .Concat(priorContracts.Keys.Where(test => File.Exists(Path.Combine(root, test))))
                        .ToHashSet(StringComparer.Ordinal);
                    contracts = ValidationCiContracts.Select(
                        root,
                        inputs.Keys,
                        survivingInputs,
                        full || extended || profile == ValidationProfile.Full,
                        out _
                    );
                }
                MapRemoved(snapshots.RootElement, "managed");
                JsonElement? baselineEditor = DevelopmentProject(snapshots.RootElement, checkout);
                if (baselineEditor.HasValue && deleted.Any(path => !IsWorkflowInput(path) && IsExecutable(path)))
                {
                    JsonElement host = baselineEditor.Value.GetProperty("Properties");
                    string? hostTarget = host.GetProperty("InnoDevelopmentTarget").GetString();
                    string? productProperties = host.GetProperty("InnoDevelopmentEditorBuildProperties").GetString();
                    if (!string.IsNullOrEmpty(hostTarget) && !string.IsNullOrEmpty(productProperties))
                    {
                        var native = new Dictionary<string, string>(baselineProperties)
                        {
                            ["InnoNativeTarget"] = hostTarget,
                            ["InnoToolTarget"] = hostTarget,
                            ["InnoProductBuildProperties"] = productProperties
                        };
                        string priorNative = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                            checkout,
                            sdk,
                            priorProjects,
                            native,
                            cancellationToken: cancellationToken
                        ).ConfigureAwait(false);
                        using JsonDocument nativeGraph = JsonDocument.Parse(priorNative);
                        MapRemoved(nativeGraph.RootElement, "native");
                    }
                }
                foreach (string path in deleted.Except(mapped))
                {
                    if (!IsEditorProductInput(path) && IsExecutable(path))
                    {
                        throw new InvalidDataException($"Removed executable input '{path}' has no declared owner at {mergeBase}.");
                    }
                }
                diagnostics.Add($"Resolved deleted inputs from Git merge base {mergeBase}.");
                void MapRemoved(
                    JsonElement baselineGraph,
                    string context
                ) {
                    var sets = new Dictionary<string, string[]>(StringComparer.Ordinal);
                    var entries = new ValidationEntryInputs(
                        checkout,
                        baselineGraph,
                        context,
                        sets,
                        values => values.Where(deleted.Contains).ToArray()
                    );
                    HashSet<string> matches = sets.Where(pair => pair.Value.Length != 0)
                        .Select(pair => pair.Key)
                        .ToHashSet(StringComparer.Ordinal);
                    mapped.UnionWith(sets.Values.SelectMany(values => values));
                    ValidationProject[] priorOwners = baselineGraph
                        .EnumerateArray()
                        .Select(node => ValidationProject.Read(checkout, node))
                        .ToArray();
                    HashSet<string> direct = priorOwners.Where(owner => owner.inputs.Any(deleted.Contains))
                        .Select(owner => owner.path)
                        .ToHashSet(StringComparer.Ordinal);
                    var surviving = new HashSet<string>(StringComparer.Ordinal);
                    foreach (ValidationProject priorOwner in priorOwners)
                    {
                        if (!byPath.ContainsKey(priorOwner.path) || !entries.Sources(priorOwner.path).Any(matches.Contains))
                        {
                            continue;
                        }
                        if (priorOwner.test && priorOwner.subjects.Length != 0 && !direct.Contains(priorOwner.path)
                            && !priorOwner.subjects.Any(direct.Contains)
                        ) {
                            continue;
                        }
                        surviving.Add(priorOwner.path);
                    }
                    removedEntries.Add(context, surviving);
                    affected.UnionWith(
                        priorOwners
                            .Where(
                                owner => (
                                    surviving.Contains(owner.path)
                                    && (owner.test || direct.Contains(owner.path) || owner.references.Any(direct.Contains))
                                )
                            )
                            .Select(owner => owner.path)
                    );
                }
            }
            finally
            {
                await ValidationProcess.RunAsync(
                    "git",
                    ["worktree", "remove", "--force", checkout],
                    root,
                    null,
                    cancellationToken: CancellationToken.None
                ).ConfigureAwait(false);
            }
        }
        if (extended || profile == ValidationProfile.Full)
        {
            full = true;
        }
        if (full)
        {
            affected.UnionWith(paths);
        }
        const string C_EDITOR = "entry/InnoEngine.Editor/InnoEngine.Editor.csproj";
        bool releaseConfiguration = full
            || changes.Any(
                path => path == "Directory.Build.targets" || path.EndsWith(".Release.props", StringComparison.OrdinalIgnoreCase)
            );
        bool supportPack = releaseConfiguration
            || changes.Any(
                path => (
                    path.StartsWith("build/support/", StringComparison.Ordinal)
                    || path.Contains("SupportPack", StringComparison.Ordinal) && path.StartsWith("build/", StringComparison.Ordinal)
                    || path == ValidationRunner.C_SUPPORT_PACK
                )
            );
        bool playerExport = releaseConfiguration
            || changes.Any(
                path => (
                    path.StartsWith("build/", StringComparison.Ordinal)
                    && path.Contains("/Player/", StringComparison.Ordinal)
                    && !path.Contains("SupportPack", StringComparison.Ordinal)
                    || path.StartsWith("platforms/", StringComparison.Ordinal) && path.Contains("/player/", StringComparison.Ordinal)
                )
            );
        bool releaseProduct = supportPack || playerExport;
        bool hotBuild = extended || changes.Contains(ValidationRunner.C_BUILD_CACHE);
        bool editorProduct = changes.Any(path => (
            IsEditorProductInput(path) || path.StartsWith(DirectoryOf(C_EDITOR), StringComparison.Ordinal)
            || path.StartsWith("src/composition/editor/panels/", StringComparison.Ordinal)
        ));
        if (byPath.ContainsKey(C_EDITOR) && (editorProduct || hotBuild || releaseProduct))
        {
            affected.Add(C_EDITOR);
        }
        if (byPath.ContainsKey("entry/InnoEngine.Build/InnoEngine.Build.csproj") && releaseProduct)
        {
            affected.Add("entry/InnoEngine.Build/InnoEngine.Build.csproj");
        }
        HashSet<string> directOwners = affected.ToHashSet(StringComparer.Ordinal);
        bool expanded;
        do
        {
            expanded = false;
            foreach (ValidationProject project in projects)
            {
                if (project.references.Any(affected.Contains))
                {
                    expanded |= affected.Add(project.path);
                }
            }
        }
        while (expanded);
        if (!full)
        {
            // MSBuild still compiles all references of each selected entry.
            affected.RemoveWhere(owner =>
            {
                ValidationProject candidate = byPath[owner];
                if (directOwners.Contains(owner))
                {
                    return false;
                }
                if (!candidate.test)
                {
                    return true;
                }
                if (candidate.subjects.Length > 0)
                {
                    return !candidate.subjects.Any(directOwners.Contains);
                }
                return !candidate.references.Any(reference => (
                    directOwners.Contains(reference)
                    || reference.StartsWith("tests/", StringComparison.Ordinal) && byPath[reference].references.Any(directOwners.Contains)
                ));
            });
        }
        if (profile == ValidationProfile.NativeProducer)
        {
            full = false;
            affected.Clear();
            affected.UnionWith(projects.Where(static project => project.requiresNativeProducer).Select(static project => project.path));
            diagnostics.Add(
                "Explicit native producer acceptance selects declared producer regressions; functional consumers remain separately required."
            );
        }
        bool secondaryHost = !OperatingSystem.IsWindows() && Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
        var executionExclusions = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (ValidationProject project in projects)
        {
            if (project.environment.Length != 0 && project.environment is not ("any" or "native-host")
                && !ValidationTestHosts.Applies(project.environment.Split(';'))
            ) {
                executionExclusions.Add(
                    project.path,
                    $"This owner requires OS environment '{project.environment}'; it is not validated on this host."
                );
            }
            else if (project.requiresNativeProducer
                     && (nativeDependencies is not null || !extended && profile != ValidationProfile.NativeProducer)
            ) {
                executionExclusions.Add(
                    project.path,
                    "This producer regression may invoke third-party compilation; it runs only under explicit native-producer or extended acceptance."
                );
            }
        }
        JsonElement? development = DevelopmentProject(graph.RootElement, root);
        string target = development?.GetProperty("Properties").GetProperty("InnoDevelopmentTarget").GetString() ?? string.Empty;
        List<ValidationNativeRequest> requestedProducts = [];
        if (rebuildBackends is not null)
        {
            using JsonDocument targets = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, ".github/native-targets.json")));
            using JsonDocument hosts = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root, ".github/validation-hosts.json")));
            string? runner = hosts.RootElement.EnumerateArray()
                .Single(host => host.GetProperty("target").GetString() == target)
                .GetProperty("runner")
                .GetString();
            string[] requested = rebuildBackends.Split(',', StringSplitOptions.TrimEntries);
            if (requested.Distinct(StringComparer.Ordinal).Count() != requested.Length)
            {
                throw new ArgumentException("Duplicate requested backend target.");
            }
            foreach (string id in requested)
            {
                JsonElement registration = targets.RootElement
                    .EnumerateArray()
                    .FirstOrDefault(row => row.GetProperty("target").GetString() == id);
                if (registration.ValueKind != JsonValueKind.Object)
                {
                    throw new ArgumentException($"Unknown requested backend target '{id}'.");
                }
                if (registration.GetProperty("runner").GetString() != runner)
                {
                    continue;
                }
                foreach (JsonElement product in graph.RootElement.EnumerateArray()
                    .Select(node => node.GetProperty("Properties"))
                    .Where(
                        item => (
                            item.GetProperty("InnoProductTarget").GetString() == id
                            && item.GetProperty("InnoProductId").GetString()?.Length > 0
                        )
                    )
                ) {
                    foreach (JsonElement mode in registration.GetProperty("configurations").EnumerateArray())
                    {
                        requestedProducts.Add(
                            new ValidationNativeRequest(id, mode.GetString()!, product.GetProperty("InnoProductId").GetString()!)
                        );
                    }
                }
                if (!requestedProducts.Any(request => request.target == id))
                {
                    throw new InvalidDataException($"No SDK product declarations for backend target '{id}'.");
                }
            }
        }
        DocumentationPageInfo[] examplePages = documentation is null
                                               ? []
                                               : ValidationDocumentation.SelectExamples(documentation, affected, full);
        var exampleExclusions = new Dictionary<DocumentationPageInfo, string>();
        foreach (DocumentationPageInfo page in examplePages)
        {
            string[] reasons = (page.examples?.projects ?? []).Select(name => byPath[documentation!.owners[name].path])
                .Select(project => (
                    executionExclusions.GetValueOrDefault(project.path)
                    ?? (target.Length != 0 && project.productTarget.Length != 0 && project.productTarget != target
                        ? $"Example owner '{project.path}' requires product target '{project.productTarget}'; this host selects '{target}'."
                        : null)
                ))
                .OfType<string>()
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            if (reasons.Length != 0)
            {
                exampleExclusions.Add(page, string.Join(" ", reasons));
            }
        }
        examplePages = examplePages.Where(page => !exampleExclusions.ContainsKey(page)).ToArray();
        foreach (DocumentationPageInfo page in examplePages)
        {
        foreach (string owner in page.examples?.projects ?? [])
        {
            documentationOwners.Add(documentation!.owners[owner].path);
        }
        }
        foreach (string owner in documentation?.changedReferenceOwners ?? new HashSet<string>())
        {
            if (documentation!.catalog.references.Any(
                reference => reference.model == "source-clr" && reference.projects.Contains(owner, StringComparer.Ordinal)
            )) {
                documentationOwners.Add(documentation.owners[owner].path);
            }
        }
        HashSet<string> scriptPages = examplePages.Where(static page => page.examples?.mode is "runtime-script" or "editor-script")
            .Select(static page => page.id)
            .ToHashSet(StringComparer.Ordinal);
        HashSet<string> nativeDocumentOwners = examplePages.Where(page => scriptPages.Contains(page.id))
            .SelectMany(static page => page.examples!.projects)
            .Select(owner => documentation!.owners[owner].path)
            .ToHashSet(StringComparer.Ordinal);
        bool scriptExamples = scriptPages.Count != 0;
        if (scriptExamples)
        {
            const string C_BUILD_ENTRY = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
            if (!byPath.ContainsKey(C_BUILD_ENTRY))
            {
                throw new InvalidDataException(
                    "Script documentation examples require the existing Build entry and its evaluated projection closure."
                );
            }
            documentationOwners.Add(C_BUILD_ENTRY);
            const string C_EDITOR_ENTRY = "entry/InnoEngine.Editor/InnoEngine.Editor.csproj";
            if (!byPath.ContainsKey(C_EDITOR_ENTRY))
            {
                throw new InvalidDataException(
                    "Script documentation examples require the declared Editor product and its public script closure."
                );
            }
            documentationOwners.Add(C_EDITOR_ENTRY);
        }

        var nativeOwners = new HashSet<string>(
            projects.Where(static project => project.native).Select(static project => project.path),
            StringComparer.Ordinal
        );
        do
        {
            expanded = false;
            foreach (ValidationProject project in projects)
            {
                if (project.references.Any(nativeOwners.Contains))
                {
                    expanded |= nativeOwners.Add(project.path);
                }
            }
        }
        while (expanded);
        if (secondaryHost)
        {
            foreach (ValidationProject project in projects.Where(project => (
                !nativeOwners.Contains(project.path)
                && project.environment is ("" or "any") && project.productTarget.Length == 0
            ))
            ) {
                executionExclusions.TryAdd(project.path, "Shared managed entries execute on the Windows CI host.");
            }
        }
        foreach (ValidationProject project in projects.Where(static project => project.layer == "domain"))
        {
            if (nativeOwners.Contains(project.path))
            {
                throw new InvalidDataException($"Domain test transitively requires native preparation: {project.path}");
            }
        }
        if (profile == ValidationProfile.Fast)
        {
            full = false;
            foreach (ValidationProject project in projects)
            {
                if (nativeOwners.Contains(project.path) || project.test && project.layer is not ("domain" or "contract"))
                {
                    executionExclusions.TryAdd(
                        project.path,
                        "Branch fast checks defer this execution prerequisite to pull-request validation."
                    );
                }
            }
            diagnostics.Add("Fast profile: native products and real SDK integration are deferred to the pull-request gate.");
        }
        // All owners of an executable example share its evaluated output context, including Host
        // examples that reuse an owner selected by a script example. Unrelated pages stay managed.
        do
        {
            expanded = false;
            foreach (DocumentationPageInfo page in examplePages)
            {
                string[] owners = (page.examples?.projects ?? []).Select(name => documentation!.owners[name].path).ToArray();
                if (owners.Any(owner => nativeOwners.Contains(owner) || nativeDocumentOwners.Contains(owner)))
                {
                    foreach (string owner in owners)
                    {
                        expanded |= nativeDocumentOwners.Add(owner);
                    }
                }
            }
        }
        while (expanded);
        var contexts = new Dictionary<string, ValidationContext>(StringComparer.Ordinal)
        {
            ["managed"] = new(new Dictionary<string, string>
            {
                ["Configuration"] = configuration,
                ["ArtifactsPath"] = artifacts,
                ["BuildProjectReferences"] = "true",
                ["BuildingInsideVisualStudio"] = "false",
                ["ShouldUnsetParentConfigurationAndPlatform"] = "false"
            }, graph.RootElement.Clone())
        };
        if (nativeDependencies is not null)
        {
            contexts["managed"].properties["InnoNativeDependencyManifest"] = Path.Combine(root, nativeDependencies);
        }
        Dictionary<string, ValidationProject> nativeProjects = byPath;
        if (scriptExamples || nativeOwners.Any(owner => (
            !executionExclusions.ContainsKey(owner)
            && (affected.Contains(owner) || documentationOwners.Contains(owner))
        ))
        ) {
            string[] unavailable = inputs
                .Where(static pair => pair.Value.StartsWith("unavailable:", StringComparison.Ordinal))
                .Select(static pair => pair.Key)
                .ToArray();
            if (unavailable.Length != 0 && nativeDependencies is null)
            {
                return new(
                    null,
                    new ValidationPreparation(
                        "Blocked",
                        "NativeSources",
                        root,
                        commit,
                        mergeBase,
                        ValidationInputs.Identity(inputs),
                        "Selected owners require unavailable pinned native sources. Prepare these inputs explicitly, then generate a new plan.",
                        nativeOwners
                            .Where(
                                owner => (
                                    !executionExclusions.ContainsKey(owner)
                                    && (affected.Contains(owner) || documentationOwners.Contains(owner))
                                )
                            )
                            .Concat(nativeDocumentOwners)
                            .Distinct(StringComparer.Ordinal)
                            .Order(StringComparer.Ordinal)
                            .ToArray(),
                        unavailable.ToDictionary(
                            static path => path,
                            path => inputs[path]["unavailable:".Length..],
                            StringComparer.Ordinal
                        )
                    )
                );
            }
            if (backendStore is null && nativeDependencies is not null && !File.Exists(Path.Combine(root, nativeDependencies)))
            {
                throw new InvalidDataException(
                    "Native dependency selection is missing: " + nativeDependencies
                    + ". Bind the matching backend package selection before native validation."
                );
            }
            if (development is null)
            {
                throw new InvalidDataException("Native validation requires the existing development product declaration.");
            }
            JsonElement host = development.Value.GetProperty("Properties");
            target = host.GetProperty("InnoDevelopmentTarget").GetString() ?? string.Empty;
            string productProperties = host.GetProperty("InnoDevelopmentEditorBuildProperties").GetString() ?? string.Empty;
            if (target.Length == 0 || !File.Exists(productProperties))
            {
                throw new InvalidDataException("This SDK host has no supported native validation product. Required checks are blocked.");
            }
            var nativeProperties = new Dictionary<string, string>(contexts["managed"].properties)
            {
                ["ArtifactsPath"] = Path.Combine(artifacts, "native", target),
                ["InnoNativeTarget"] = target,
                ["InnoToolTarget"] = target,
                ["InnoProductBuildProperties"] = productProperties
            };
            var evaluationProperties = new Dictionary<string, string>(properties);
            foreach (KeyValuePair<string, string> pair in nativeProperties)
            {
                evaluationProperties[pair.Key] = pair.Value;
            }
            string nativeCapture = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                root,
                sdk,
                paths.Select(path => Path.Combine(root, path)).ToArray(),
                evaluationProperties,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            using JsonDocument nativeGraph = JsonDocument.Parse(nativeCapture);
            RequireCompilerInputs(nativeGraph.RootElement);
            contexts.Add("native", new ValidationContext(nativeProperties, nativeGraph.RootElement.Clone()));
            nativeProjects = nativeGraph.RootElement.EnumerateArray()
                .Select(snapshot => ValidationProject.Read(root, snapshot))
                .ToDictionary(static project => project.path, StringComparer.Ordinal);
            RecordStage("native-graph");
        }

        string[] policyInputs = inputs.Keys
            .Where(static path => (
                path == "AGENTS.md" || path.StartsWith(".agents/rulebook/", StringComparison.Ordinal)
                || path is ValidationCiContracts.C_RULE_LOADER or ValidationCiContracts.C_RULE_EXAMPLES
            ))
            .Order(StringComparer.Ordinal)
            .ToArray();
        Dictionary<string, string[]> inputSets = projects.ToDictionary(
            static project => project.path,
            project => CapturedInputs(project.inputs.Concat(nativeProjects[project.path].inputs)),
            StringComparer.Ordinal
        );
        Dictionary<string, ValidationEntryInputs> entryInputs = contexts.ToDictionary(
            static pair => pair.Key,
            pair => new ValidationEntryInputs(root, pair.Value.sdkGraph, pair.Key, inputSets, CapturedInputs),
            StringComparer.Ordinal
        );
        const string C_VALIDATOR = "tools/Inno.Tooling.Validation/Inno.Tooling.Validation.csproj";
        IEnumerable<string> validatorInputs = byPath.ContainsKey(C_VALIDATOR)
                                              ? entryInputs["managed"].Sources(C_VALIDATOR).SelectMany(key => inputSets[key])
                                              : [];
        string[] toolingInputs = CapturedInputs(
            validatorInputs.Concat(
                inputs.Keys.Where(static path => (
                    path is "global.json"
                    or "Directory.Build.props" or "Directory.Build.targets" or "Directory.Solution.targets" or "InnoEngine.sln"
                ))
            )
        ).Order(StringComparer.Ordinal).ToArray();
        Dictionary<string, string> changedSets = inputSets.Where(pair => pair.Value.Any(changes.Contains)).ToDictionary(
            static pair => pair.Key,
            pair => pair.Value.First(changes.Contains),
            StringComparer.Ordinal
        );
        bool toolingChanged = toolingInputs.Any(changes.Contains)
                              || removedEntries.TryGetValue("managed", out HashSet<string>? priorTooling)
                                 && priorTooling.Contains(C_VALIDATOR);
        bool effectiveBuildChange = changes.Count > 0
                                    && changes.All(path => (
                                        path != ValidationRunner.C_TEST_EXECUTOR && !IsWorkflowInput(path) && !IsEditorProductInput(path)
                                        && (removedEntries.Count > 0 && removed.Contains(path, StringComparer.Ordinal)
                                            || inputs.TryGetValue(path, out string? value)
                                               && value != "missing"
                                               && projects.Any(project => project.inputs.Contains(path, StringComparer.Ordinal)))
                                    ));
        inputSets.Add("tooling", toolingInputs);
        inputSets.Add("native-sources", submoduleInputs);
        // Style and XML documentation rules judge one file at a time. An incremental profile checks the changed sources;
        // a changed validator can alter any verdict, so it selects every source again.
        const string C_ARCHITECTURE = "tools/Inno.Tooling.Architecture/Inno.Tooling.Architecture.csproj";
        bool architectureChanged = changes.Overlaps(
            ["global.json", "Directory.Build.props", "Directory.Build.targets", "Directory.Solution.targets", "InnoEngine.sln"]
        )
                                   || changes.Any(path => path.StartsWith(DirectoryOf(C_ARCHITECTURE), StringComparison.Ordinal));
        string[]? changedSources = full
                                   || architectureChanged
                                   || profile is not (ValidationProfile.PullRequest or ValidationProfile.Fast or ValidationProfile.Master)
                                   ? null
                                   : changes
                                       .Where(path => (
                                           path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                                           && inputs.TryGetValue(path, out string? value)
                                           && value != "missing"
                                       ))
                                       .Order(StringComparer.Ordinal)
                                       .ToArray();
        if (changedSources is not null)
        {
            diagnostics.Add($"Architecture style and XML documentation rules cover {changedSources.Length} changed C# source(s).");
        }
        List<ValidationCheck> checks =
        [
            new ValidationCheck(
                "instructions",
                "instructions",
                "AGENTS.md",
                "Rule front matter, size limits and routing examples.",
                "any",
                [],
                policyInputs,
                true,
                null
            ),
            new ValidationCheck(
                "architecture",
                "architecture",
                "tools/Inno.Tooling.Architecture",
                "Cheap repository-wide source, ownership and architecture invariants.",
                "any",
                [],
                inputs.Keys
                    .Where(static path => (
                        path.Split('/')[0] is "src" or "entry" or "backends" or "platforms" or "build" or "tools"
                        || path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                        || path.EndsWith(".Tests.ps1", StringComparison.OrdinalIgnoreCase)
                        || path.EndsWith(".cs.txt", StringComparison.OrdinalIgnoreCase)
                           && !path.StartsWith(".agents/records/", StringComparison.Ordinal)
                        || Path.GetExtension(path) is ".csproj" or ".props" or ".targets"
                        || path is "InnoEngine.sln" or "Directory.Build.props" or "Directory.Build.targets" or "Directory.Solution.targets"
                    ))
                    .Order(StringComparer.Ordinal)
                    .ToArray(),
                true,
                null,
                changedSources: changedSources
            )
        ];
        if (changes.Any(static path => path.StartsWith(".agents/", StringComparison.Ordinal) || path is "AGENTS.md" or "CONTRIBUTING.md")
            || full
        ) {
            checks.Add(
                new ValidationCheck(
                    "agent-documents",
                    "agent-documents",
                    ".agents",
                    "Rule, skill and record structure and links; no product compilation.",
                    "any",
                    [],
                    inputs.Keys
                        .Where(
                            static path => path.StartsWith(".agents/", StringComparison.Ordinal) || path is "AGENTS.md" or "CONTRIBUTING.md"
                        )
                        .Append("@tooling")
                        .Order(StringComparer.Ordinal)
                        .ToArray(),
                    true,
                    null
                )
            );
        }
        if (documentation is not null)
        {
            checks.Add(
                new ValidationCheck(
                    "documentation",
                    "documentation",
                    "InnoEngine.Docs",
                    "Explicitly selected canonical project entries and immutable reference payloads.",
                    "any",
                    [],
                    inputs.Keys
                        .Where(static path => path.EndsWith(".csproj", StringComparison.Ordinal))
                        .Append("@tooling")
                        .ToArray(),
                    true,
                    null,
                    documentation: new ValidationDocumentationCheck([], [], documentation.catalog.contentIdentity, configuration, null)
                )
            );
        }
        foreach (KeyValuePair<string, string[]> contract in contracts)
        {
            if (!secondaryHost)
            {
                checks.Add(
                    new ValidationCheck(
                        "ci-contract:" + contract.Key,
                        "ci-contracts",
                        contract.Key,
                        "Check the behavior of the affected CI script.",
                        "any",
                        [],
                        contract.Value,
                        true,
                        null,
                        projects: [contract.Key]
                    )
                );
            }
        }
        foreach (ValidationProject project in projects.OrderBy(static project => project.path, StringComparer.Ordinal))
        {
            bool native = nativeOwners.Contains(project.path);
            string context = native || nativeDocumentOwners.Contains(project.path) ? "native" : "managed";
            Dictionary<string, ValidationProject> selectedGraph = context == "native" ? nativeProjects : byPath;
            var closure = new HashSet<string>(StringComparer.Ordinal);
            Visit(selectedGraph[project.path]);
            if (project.test && project.layer is not ("domain" or "contract" or "integration" or "native" or "product"))
            {
                throw new InvalidDataException($"Test execution layer is absent or unknown: {project.path} ({project.layer})");
            }
            if (project.layer == "domain" && native)
            {
                throw new InvalidDataException($"Domain test transitively requires native preparation: {project.path}");
            }
            string[] selectedInputs = closure.Select(static path => "@" + path)
                .Append("@tooling")
                .Concat(context == "native" ? ["@native-sources"] : [])
                .Concat(context == "native" && nativeDependencies is not null ? [nativeDependencies] : [])
                .Order(StringComparer.Ordinal)
                .ToArray();
            string[]? sourceSets = entryInputs.TryGetValue(context, out ValidationEntryInputs? graphInputs)
                                   ? graphInputs.Sources(project.path)
                                   : null;
            bool selected = affected.Contains(project.path);
            string? matchedSet = sourceSets?.FirstOrDefault(changedSets.ContainsKey);
            bool baselineMatch = removed.Length > 0
                                 && (removedEntries.TryGetValue(context, out HashSet<string>? priorEntries)
                                     && priorEntries.Contains(project.path));
            if (selected && !full && effectiveBuildChange && sourceSets is not null && project.path != C_EDITOR
                && profile is ValidationProfile.PullRequest or ValidationProfile.Fast or ValidationProfile.Master
                && matchedSet is null && !baselineMatch && !toolingChanged
            ) {
                selected = false;
                // A conditional source still belongs to its project when compiled through another entry.
                if (!project.inputs.Any(changes.Contains))
                {
                    affected.Remove(project.path);
                }
                diagnostics.Add($"No changed input in the evaluated {context} entry for {project.path}.");
            }
            if (selected && !full && matchedSet is not null)
            {
                diagnostics.Add($"Selected {context} entry {project.path}: {changedSets[matchedSet]} is consumed by {matchedSet}.");
            }
            if (selected && !full && baselineMatch)
            {
                diagnostics.Add($"Selected {context} entry {project.path} for removed inputs at {mergeBase}.");
            }
            bool compile = selected || documentationOwners.Contains(project.path);
            string environment = project.environment.Length != 0 ? project.environment : native ? "native-host" : "any";
            string? exclusion = executionExclusions.GetValueOrDefault(project.path)
                ?? (project.productTarget.Length != 0 && target.Length != 0 && project.productTarget != target
                ? $"Product '{project.productTarget}' requires its own supported target environment; current SDK host selects '{target}'."
                : null);
            if (compile)
            {
                checks.Add(
                    new ValidationCheck(
                        "build:" + project.path,
                        "build",
                        project.path,
                        "Changed owner or transitive consumer; compile the shared union once.",
                        environment,
                        [],
                        selectedInputs,
                        false,
                        exclusion,
                        context,
                        "compilation",
                        sourceSets: sourceSets
                    )
                );
            }
            if (project.test)
            {
                checks.Add(
                    new ValidationCheck(
                        "test:" + project.path,
                        "test",
                        project.path,
                        selected
                    ? "Affected behavior or contract consumer; compiled SupportedOSPlatform requirements distinguish host exclusions from missing execution."
                    : "No affected input in its evaluated closure.",
                        environment,
                        ["build:" + project.path],
                        [..selectedInputs, ValidationRunner.C_TEST_EXECUTOR],
                        project.reusable && project.layer != "product",
                        selected ? exclusion : "Outside the affected evaluated dependency closure.",
                        context,
                        project.layer,
                        sourceSets: sourceSets
                    )
                );
            }

            void Visit(ValidationProject node)
            {
                if (!closure.Add(node.path))
                {
                    return;
                }
                foreach (string dependency in node.references)
                {
                    if (selectedGraph.TryGetValue(dependency, out ValidationProject? next))
                    {
                        Visit(next);
                    }
                    else
                    {
                        throw new InvalidDataException($"Unknown evaluated dependency '{dependency}' from '{node.path}'.");
                    }
                }
            }
        }
        if (profile != ValidationProfile.Fast
            && checks.Any(check => check.kind == "build" && check.exclusion is null && affected.Contains(check.owner))
            && !secondaryHost && !checks.Any(static check => check.kind is "products" or "test" && check.exclusion is null)
            && !affected.Contains(C_EDITOR) && !changes.Any(IsWorkflowInput)
        ) {
            throw new InvalidDataException(
                "Affected production inputs have no selected behavior tests. Declare their real public contract consumers."
            );
        }
        ValidationCheck[] builds = checks.Where(static check => check.kind == "build" && check.exclusion is null).ToArray();
        foreach (IGrouping<string?, ValidationCheck> group in builds.GroupBy(static check => check.context))
        {
            string[] apiOwners = group.Select(static check => check.owner)
                .Where(owner => !byPath[owner].test)
                .Distinct(StringComparer.Ordinal)
                .Order(StringComparer.Ordinal)
                .ToArray();
            checks.Add(
                new ValidationCheck(
                    "compiled-api:" + group.Key,
                    "api",
                    "tools/Inno.Tooling.Architecture",
                    "Public/protected signatures in affected owners and their evaluated dependencies.",
                    "any",
                    group.Select(static check => check.id).ToArray(),
                    group
                        .SelectMany(static check => check.inputs)
                        .Distinct()
                        .Order(StringComparer.Ordinal)
                        .ToArray(),
                    true,
                    apiOwners.Length == 0 ? "Only test implementations changed; their unchanged production dependencies have no affected API owner." : null,
                    group.Key,
                    "contract",
                    apiOwners
                )
            );
        }
        if (documentation is not null)
        {
            AddDocumentationChecks();
        }
        ValidationCheck? editorBuild = builds.SingleOrDefault(
            static check => check.owner == "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"
        );
        if (editorBuild is not null && (full || editorProduct || !releaseProduct && !hotBuild))
        {
            checks.Add(
                new ValidationCheck(
                    "editor-products",
                    "products",
                    editorBuild.owner,
                    "Real renderer startup, product workflows and native panel retirement for the affected Editor product.",
                    "native-host",
                    [editorBuild.id],
                    editorBuild.inputs
                        .Concat(
                            inputs.Keys.Where(static path => (
                                path.StartsWith("tests/product/fixtures/editor-ui/", StringComparison.Ordinal)
                                || path is "tests/product/Invoke-EditorSmoke.ps1" or "tests/product/Invoke-EditorUiProductJourney.ps1"
                            ))
                        )
                        .Distinct()
                        .Order(StringComparer.Ordinal)
                        .ToArray(),
                    false,
                    null,
                    "native",
                    "product",
                    scenarios: new[] { "Workflows", "Lifecycle" }.Where(scenario => (
                        full || changes.Any(path => (
                            path == "tests/product/Invoke-EditorUiProductJourney.ps1"
                            || scenario == "Lifecycle" && (path.EndsWith("PanelLifecycle.editor.cs", StringComparison.Ordinal)
                                                           || path.StartsWith("src/composition/editor/panels/", StringComparison.Ordinal))
                            || scenario == "Workflows" && (path.Contains("/Workflow", StringComparison.Ordinal)
                                                           || path.StartsWith("src/composition/editor/features/", StringComparison.Ordinal))
                        ))
                    )).ToArray()
                )
            );
        }
        if (editorBuild is not null && hotBuild)
        {
            checks.Add(
                new ValidationCheck(
                    "native-hot-builds",
                    "hot-builds",
                    editorBuild.owner,
                    "An unchanged native build must retain content selections and start no native compiler or binding generation.",
                    "native-host",
                    [editorBuild.id],
                    editorBuild.inputs
                        .Append(ValidationRunner.C_BUILD_CACHE)
                        .ToArray(),
                    false,
                    null,
                    "native",
                    "product"
                )
            );
        }
        if (extended)
        {
            checks.Insert(
                0,
                new ValidationCheck(
                    "cold-inputs",
                    "cold-inputs",
                    "build",
                    "Explicit cold acceptance requires an initially unprepared native cache.",
                    "native-host",
                    [],
                    ["@tooling"],
                    false,
                    null,
                    "native",
                    "product"
                )
            );
        }
        if (releaseProduct && editorBuild?.context == "native")
        {
            const string C_BUILD_ENTRY = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
            if (!byPath.ContainsKey(C_BUILD_ENTRY))
            {
                throw new InvalidDataException("Complete product acceptance requires the declared Build entry.");
            }
            string releaseContext = configuration == "Release" ? "native" : "native-release";
            Dictionary<string, ValidationProject> releaseProjects = nativeProjects;
            if (releaseContext != "native")
            {
                if (backendStore is not null)
                {
                    var releaseInputsProperties = new Dictionary<string, string>(properties)
                    {
                        ["Configuration"] = "Release"
                    };
                    string backendCapture = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                        root,
                        sdk,
                        paths.Select(path => Path.Combine(root, path)).ToArray(),
                        releaseInputsProperties,
                        cancellationToken: cancellationToken
                    ).ConfigureAwait(false);
                    using JsonDocument backendGraph = JsonDocument.Parse(backendCapture);
                    RequireCompilerInputs(backendGraph.RootElement);
                    contexts.Add("backend-release", new ValidationContext(releaseInputsProperties, backendGraph.RootElement.Clone()));
                }
                var releaseProperties = new Dictionary<string, string>(contexts["native"].properties)
                {
                    ["Configuration"] = "Release",
                    ["ArtifactsPath"] = Path.Combine(artifacts, "native-release", target)
                };
                var evaluationProperties = new Dictionary<string, string>(properties);
                foreach (KeyValuePair<string, string> pair in releaseProperties)
                {
                    evaluationProperties[pair.Key] = pair.Value;
                }
                string releaseCapture = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                    root,
                    sdk,
                    paths.Select(path => Path.Combine(root, path)).ToArray(),
                    evaluationProperties,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                using JsonDocument releaseGraph = JsonDocument.Parse(releaseCapture);
                RequireCompilerInputs(releaseGraph.RootElement);
                contexts.Add(releaseContext, new ValidationContext(releaseProperties, releaseGraph.RootElement.Clone()));
                releaseProjects = releaseGraph.RootElement.EnumerateArray()
                    .Select(snapshot => ValidationProject.Read(root, snapshot))
                    .ToDictionary(static project => project.path, StringComparer.Ordinal);
                foreach (ValidationProject project in releaseProjects.Values)
                {
                    inputSets[project.path] = inputSets[project.path]
                        .Concat(CapturedInputs(project.inputs))
                        .Distinct(StringComparer.Ordinal)
                        .ToArray();
                }
            }
            if (!entryInputs.TryGetValue(releaseContext, out ValidationEntryInputs? releaseEntries))
            {
                entryInputs.Add(
                    releaseContext,
                    releaseEntries = new(root, contexts[releaseContext].sdkGraph, releaseContext, inputSets, CapturedInputs)
                );
            }
            var releaseInputs = new HashSet<string>(StringComparer.Ordinal) { "@tooling", "@native-sources" };
            List<string> releaseBuilds = [];
            string[] releaseRoots = releaseConfiguration ? [editorBuild.owner, C_BUILD_ENTRY] : [C_BUILD_ENTRY];
            foreach (string owner in releaseRoots)
            {
                VisitRelease(owner);
                string id = (releaseContext == "native" ? "build:" : "release-build:") + owner;
                releaseBuilds.Add(id);
                if (releaseContext != "native")
                {
                    checks.Add(
                        new ValidationCheck(
                            id,
                            "build",
                            owner,
                            "Release product acceptance prepares its configuration once before execution and packaging.",
                            "native-host",
                            [],
                            releaseInputs
                                .Order(StringComparer.Ordinal)
                                .ToArray(),
                            false,
                            null,
                            releaseContext,
                            "compilation",
                            sourceSets: releaseEntries.Sources(owner)
                        )
                    );
                }
            }
            if (releaseContext != "native")
            {
                checks.Add(
                    new ValidationCheck(
                        "compiled-api:release",
                        "api",
                        "tools/Inno.Tooling.Architecture",
                        "Release symbols retain the same public boundary requirements.",
                        "native-host",
                        releaseBuilds.ToArray(),
                        releaseInputs
                            .Order(StringComparer.Ordinal)
                            .ToArray(),
                        true,
                        null,
                        releaseContext,
                        "contract",
                        releaseInputs
                            .Where(input => input.StartsWith('@') && releaseProjects.ContainsKey(input[1..]))
                            .Select(static input => input[1..])
                            .Order(StringComparer.Ordinal)
                            .ToArray()
                    )
                );
            }
            if (releaseConfiguration && releaseContext != "native")
            {
                checks.Add(
                    new ValidationCheck(
                        "editor-release-products",
                        "products",
                        editorBuild.owner,
                        "Execute the actual Release Editor renderer, workflow and lifecycle before packaging.",
                        "native-host",
                        releaseBuilds.ToArray(),
                        editorBuild.inputs
                            .Concat(releaseInputs)
                            .Distinct(StringComparer.Ordinal)
                            .ToArray(),
                        false,
                        null,
                        releaseContext,
                        "product"
                    )
                );
            }
            checks.Add(
                new ValidationCheck(
                    "support-pack",
                    "support-pack",
                    C_BUILD_ENTRY,
                    "Publish and verify the source-independent Release Player Support Pack under its existing read lease.",
                    "native-host",
                    releaseBuilds.ToArray(),
                    releaseInputs
                        .Order(StringComparer.Ordinal)
                        .ToArray(),
                    false,
                    null,
                    releaseContext,
                    "product"
                )
            );
            if (playerExport)
            {
            JsonElement[] players = contexts[releaseContext].sdkGraph.EnumerateArray().Where(snapshot => (
                snapshot.GetProperty("Properties").GetProperty("InnoProductId").GetString() == "player"
                && snapshot.GetProperty("Properties").GetProperty("InnoProductTarget").GetString() == target
                && !string.Equals(
                    snapshot.GetProperty("Properties").GetProperty("IsTestProject").GetString(),
                    "true",
                    StringComparison.OrdinalIgnoreCase
                )
                && !string.Equals(
                    snapshot.GetProperty("Properties").GetProperty("IsTestSupportProject").GetString(),
                    "true",
                    StringComparison.OrdinalIgnoreCase
                )
            )).ToArray();
            if (players.Length != 1)
            {
                throw new InvalidDataException("Full host acceptance requires exactly one actual Player product declaration.");
            }
            string playerOwner = Path.GetRelativePath(root, players[0].GetProperty("Path").GetString()!).Replace('\\', '/');
            VisitRelease(playerOwner);
            checks.Add(
                new ValidationCheck(
                    "native-aot-restore",
                    "restore-aot",
                    playerOwner,
                    "Restore the actual host Player with NativeAOT SDK inputs in an independently owned restore layout.",
                    "native-host",
                    releaseBuilds.ToArray(),
                    releaseInputs
                        .Order(StringComparer.Ordinal)
                        .ToArray(),
                    false,
                    null,
                    releaseContext,
                    "contract"
                )
            );
            }
            void VisitRelease(string owner)
            {
                if (!releaseInputs.Add("@" + owner))
                {
                    return;
                }
                if (!releaseProjects.TryGetValue(owner, out ValidationProject? project))
                {
                    throw new InvalidDataException($"Unknown Release dependency: {owner}");
                }
                foreach (string reference in project.references)
                {
                    VisitRelease(reference);
                }
            }
        }
        if (requestedProducts.Any(request => request.configuration == "Release") && configuration != "Release"
            && !contexts.ContainsKey("backend-release")
        ) {
            var release = new Dictionary<string, string>(properties) { ["Configuration"] = "Release" };
            string capture = await ManagedProjectGraphCapture.CaptureSelectedAsync(
                root,
                sdk,
                paths.Select(path => Path.Combine(root, path)).ToArray(),
                release,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            using JsonDocument releaseGraph = JsonDocument.Parse(capture);
            contexts.Add("backend-release", new ValidationContext(release, releaseGraph.RootElement.Clone()));
        }
        RecordStage("selection-and-release-graph");
        string environmentIdentity = await ValidationEnvironment.CaptureAsync(
            root,
            sdk.hostPath,
            contexts.Values,
            toolSnapshot,
            cancellationToken
        ).ConfigureAwait(false);
        RecordStage("execution-environment");
        Dictionary<string, int> phases = contexts.Keys
            .Select(static (
                string context,
                int index
            ) => new KeyValuePair<string, int>(context, index + 1))
            .ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.Ordinal);
        // Policy and architecture govern compilation; unrelated documentation/contract failures do not.
        // Cold acceptance is a native preparation prerequisite, not a global stop for managed checks.
        checks = checks.Select(check => check with
        {
            prerequisites = check.prerequisites
                .Concat(check.kind == "build" ? ["instructions", "architecture"] : [])
                .Concat(
                    extended && check.context is not (null or "managed") && check.kind != "cold-inputs"
                    ? ["cold-inputs"]
                    : []
                )
                .Distinct(StringComparer.Ordinal)
                .ToArray()
        }).ToList();
        ValidationCheck[] execution = checks
            .OrderBy(check => (
                check.kind == "cold-inputs" ? -1 : check.context is null ? 0 : phases.GetValueOrDefault(check.context, phases.Count + 1)
            ))
            .ThenBy(
                static check => check.kind switch
                {
                    "build" => 0,
                    "api" => 1,
                    "documentation-api" or "documentation-examples" => 2,
                    "test" => 3,
                    _ => 4
                }
            )
            .ToArray();
        var preceding = new HashSet<string>(StringComparer.Ordinal);
        foreach (ValidationCheck check in execution)
        {
            if (check.exclusion is null && check.prerequisites.Any(prerequisite => !preceding.Contains(prerequisite)))
            {
                throw new InvalidDataException($"Required prerequisites are missing or scheduled after '{check.id}'.");
            }
            preceding.Add(check.id);
        }
        execution = execution.Select(check => (
            check.kind == "test" && check.layer == "native" && check.exclusion is null
            && check.context is not null && contexts[check.context].properties.ContainsKey("InnoNativeTarget")
            ? check with
            {
                consumedProducts = CollectNativeRequests(contexts[check.context], [check.owner]).Distinct()
                    .OrderBy(static request => request.target, StringComparer.Ordinal)
                    .ThenBy(static request => request.configuration, StringComparer.Ordinal)
                    .ThenBy(static request => request.product, StringComparer.Ordinal)
                    .ToArray()
            }
            : check
        )).ToArray();
        ValidationNativeRequest[] requests = contexts.Values.Where(context => context.properties.ContainsKey("InnoNativeTarget"))
            .SelectMany(context => CollectNativeRequests(
                context,
                checks.Where(check => (
                    check.exclusion is null && check.kind == "build"
                    && check.context is not null && ReferenceEquals(contexts[check.context], context)
                )).Select(static check => check.owner)
            ))
            .Concat(
                checks.Where(static check => check.exclusion is null && check.kind == "support-pack")
                    .Select(
                        check => new ValidationNativeRequest(contexts[check.context!].properties["InnoNativeTarget"], "Release", "player")
                    )
            )
            .Concat(requestedProducts)
            .Distinct()
            .OrderBy(static request => request.target, StringComparer.Ordinal)
            .ThenBy(static request => request.configuration, StringComparer.Ordinal)
            .ThenBy(static request => request.product, StringComparer.Ordinal)
            .ToArray();
        if (backendStore is not null)
        {
            foreach (ValidationContext context in contexts.Values)
            {
                context.properties["InnoBackendPackageDirectory"] = Path.Combine(root, backendStore);
                context.properties["InnoBackendHostTarget"] = target;
            }
        }
        return new(
            new ValidationPlan(
                root,
                mergeBase,
                commit,
                ValidationInputs.Identity(inputs),
                environmentIdentity,
                configuration,
                sdk.hostPath,
                artifacts,
                full,
                changes.Order(StringComparer.Ordinal).ToArray(),
                affected
                    .Order(StringComparer.Ordinal)
                    .ToArray(),
                execution,
                diagnostics.ToArray(),
                inputs,
                inputSets,
                contexts,
                extended,
                timings,
                documentation?.binding,
                documentation?.sourceIdentities.ToDictionary(
                    static pair => pair.Key,
                    static pair => pair.Value.identity,
                    StringComparer.Ordinal
                ),
                profile,
                nativeDependencies,
                requests,
                ValidationShards.Create(execution),
                backendStore,
                backendStore is null
                ? null
                : target,
                portableTestChanges: !full
                                     && profile is (ValidationProfile.Fast or ValidationProfile.PullRequest or ValidationProfile.Master)
                                     && changes.Count > 0 && removed.Length == 0 && affected.Count > 0
                                     && affected.All(owner => byPath[owner] is { test: true, layer: "domain" })
                                     && changes.All(change => (
                                         change.EndsWith(".cs", StringComparison.Ordinal)
                                         && affected.Any(owner => (
                                             change.StartsWith(DirectoryOf(owner), StringComparison.Ordinal)
                                             && byPath[owner].inputs.Contains(change, StringComparer.Ordinal)
                                         ))
                                     ))
                                     && execution.Any(static check => check.exclusion is null && check.kind == "test")
                                     && execution.All(static check => (
                                         check.exclusion is not null || check.kind is "instructions" or "architecture"
                                         || check.context == "managed" && check.kind is ("test" or "build")
                                     ))
            ),
            null
        );

        IEnumerable<ValidationNativeRequest> CollectNativeRequests(
            ValidationContext context,
            IEnumerable<string> roots
        ) {
            string id = contexts.Single(pair => ReferenceEquals(pair.Value, context)).Key;
            if (!entryInputs.TryGetValue(id, out ValidationEntryInputs? entries))
            {
                entryInputs.Add(id, entries = new(root, context.sdkGraph, id, inputSets, CapturedInputs));
            }
            var pending = new Stack<(string instance, bool productOwned)>(
                roots.Select(owner => (entries.Entry(owner), false))
            );
            var visited = new HashSet<(string instance, bool productOwned)>();
            while (pending.TryPop(out (string instance, bool productOwned) request))
            {
                if (!visited.Add(request))
                {
                    continue;
                }
                (string key, JsonElement variant, string[] references) node = entries.Instance(request.instance);
                JsonElement variant = node.variant;
                string owner = variant.GetProperty("Path").GetString()!;
                JsonElement declared = variant.GetProperty("Properties");
                bool productOwned = request.productOwned
                                    || !string.IsNullOrWhiteSpace(declared.GetProperty("InnoTestNativeProduct").GetString())
                                    || !string.IsNullOrWhiteSpace(declared.GetProperty("InnoProductId").GetString());
                foreach (string property in new[] { "InnoTestNativeProduct", "InnoProductId" })
                {
                    if (declared.GetProperty(property).GetString() is { Length: > 0 } product)
                    {
                        string targetProperty = property == "InnoTestNativeProduct" ? "InnoTestNativeTarget" : "InnoProductTarget";
                        string declaredTarget = declared.GetProperty(targetProperty).GetString() ?? string.Empty;
                        yield return new ValidationNativeRequest(
                            declaredTarget.Length == 0 ? context.properties["InnoNativeTarget"] : declaredTarget,
                            context.properties["Configuration"],
                            product
                        );
                    }
                }
                // Product preparation supplies its exact binding selection to references. Only a standalone
                // binding consumer needs the component's explicit product declaration as an additional request.
                if (!productOwned && declared.GetProperty("BindGenGeneratedBindings").GetString() == "true"
                    && declared.GetProperty("InnoBindingProductId").GetString() is { Length: > 0 } bindingProduct
                ) {
                    yield return new ValidationNativeRequest(
                        context.properties["InnoNativeTarget"],
                        context.properties["Configuration"],
                        bindingProduct
                    );
                }
                JsonElement items = variant.GetProperty("Items");
                foreach (JsonElement dependency in items.GetProperty("InnoNativeDependency").EnumerateArray())
                {
                    string product = dependency.GetProperty("Identity").GetString()!;
                    string dependencyTarget = dependency.GetProperty("TargetId").GetString()!;
                    string dependencyConfiguration = dependency.GetProperty("Configuration").GetString()!;
                    if (string.IsNullOrWhiteSpace(product) || string.IsNullOrWhiteSpace(dependencyTarget)
                        || dependencyConfiguration is not ("Debug" or "Release")
                    ) {
                        throw new InvalidDataException(
                            "A native build dependency must declare product, target and configuration: " + owner
                        );
                    }
                    yield return new ValidationNativeRequest(dependencyTarget, dependencyConfiguration, product);
                }
                foreach (string reference in node.references)
                {
                    pending.Push((reference, productOwned));
                }
            }
        }

        void AddDocumentationChecks()
        {
            ValidationDocumentation.Selection selected = documentation!;
            foreach (KeyValuePair<DocumentationPageInfo, string> excluded in exampleExclusions)
            {
                checks.Add(
                    new ValidationCheck(
                        "documentation-examples:excluded:" + excluded.Key.id,
                        "documentation-examples",
                        "InnoEngine.Docs",
                        "The example retains its declared product and environment obligations in the corresponding matrix cell.",
                        "any",
                        [],
                        ["@tooling"],
                        false,
                        excluded.Value,
                        layer: "contract",
                        documentation: new ValidationDocumentationCheck(
                            [],
                            [excluded.Key.id],
                            ValidationDocumentation.PageIdentity([excluded.Key]),
                            configuration,
                            null
                        )
                    )
                );
            }
            HashSet<string> owners = affected.Concat(selected.changedReferenceOwners.Select(owner => selected.owners[owner].path))
                .Where(owner => (
                    !byPath[owner].test && !executionExclusions.ContainsKey(owner)
                    && (byPath[owner].productTarget.Length == 0 || byPath[owner].productTarget == target)
                ))
                .Where(owner => selected.owners.GetValueOrDefault(Path.GetFileNameWithoutExtension(owner))?.nativeReference != true)
                .ToHashSet(StringComparer.Ordinal);
            var covered = new HashSet<string>(StringComparer.Ordinal);
            IEnumerable<
                IGrouping<
                    (string context, string? configuration, string? target),
                    (DocumentationReferenceInfo reference, string name, string path)
                >
            > captures = selected.catalog.references
                .Where(reference => (
                    reference.model == "source-clr" && reference.configuration == configuration
                    && (reference.target is null || reference.target == target)
                ))
                .SelectMany(reference => reference.projects
                    .Where(name => (
                        selected.owners.TryGetValue(name, out ValidationDocumentation.Owner? owner)
                        && owners.Contains(owner.path)
                    ))
                    .Select(name => (reference, name, path: selected.owners[name].path)))
                .GroupBy(
                    item => (context: nativeOwners.Contains(item.path) || nativeDocumentOwners.Contains(item.path)
                                      ? "native"
                                      : "managed", item.reference.configuration, item.reference.target)
                );
            foreach (
                IGrouping<
                    (string context, string? configuration, string? target),
                    (DocumentationReferenceInfo reference, string name, string path)
                > group in captures
            ) {
                string[] paths = group
                    .Select(static item => item.path)
                    .Distinct(StringComparer.Ordinal)
                    .Order(StringComparer.Ordinal)
                    .ToArray();
                covered.UnionWith(paths);
                ValidationCheck[] required = builds.Where(check => paths.Contains(check.owner, StringComparer.Ordinal)).ToArray();
                string[] references = group
                    .Select(static item => item.reference.path)
                    .Distinct(StringComparer.Ordinal)
                    .Order(StringComparer.Ordinal)
                    .ToArray();
                string identity = ValidationInputs.Identity(
                    group.Select(item => new KeyValuePair<string, string>(
                        item.path,
                        selected.sourceIdentities[item.reference.path + "\0" + item.name].identity
                    ))
                );
                checks.Add(
                    new ValidationCheck(
                        "documentation-api:"
                        + group.Key.context
                        + ":"
                        + (group.Key.target ?? "default"),
                        "documentation-api",
                        "InnoEngine.Docs",
                        "Selected owner declarations and XML from protected source reference payloads.",
                        "any",
                        required.Select(static check => check.id).Append("documentation").ToArray(),
                        required
                            .SelectMany(static check => check.inputs)
                            .Append("@tooling")
                            .Distinct(StringComparer.Ordinal)
                            .ToArray(),
                        true,
                        null,
                        group.Key.context,
                        "contract",
                        paths,
                        new ValidationDocumentationCheck(references, [], identity, configuration, group.Key.target)
                    )
                );
            }
            foreach (string owner in owners.Except(covered).Order(StringComparer.Ordinal))
            {
                ValidationCheck? prepared = builds.SingleOrDefault(check => check.owner == owner);
                checks.Add(
                    new ValidationCheck(
                        "documentation-api-missing:" + owner,
                        "documentation-missing",
                        owner,
                        "Required source API capture is absent for the selected configuration and target. Prepared owners may export unverified facts for review; capture does not pass this requirement.",
                        "any",
                        prepared is null ? ["documentation"] : ["documentation", prepared.id],
                        prepared?.inputs ?? ["@" + owner],
                        false,
                        null,
                        prepared?.context,
                        "contract",
                        prepared is null ? null : [owner],
                        prepared is null ? null : new ValidationDocumentationCheck(
                            [],
                            [],
                            string.Empty,
                            configuration,
                            prepared.context == "native" ? target : null
                        )
                    )
                );
            }
            // Foreign product execution stays excluded. Its explicitly empty authored public surface
            // can still be checked against actual SDK-selected source without borrowing host binaries.
            IEnumerable<
                IGrouping<string?, (DocumentationReferenceInfo reference, string name, string path)>
            > emptyForeign = selected.catalog.references
                .Where(reference => (reference.model == "source-clr" && reference.configuration == configuration))
                .SelectMany(reference => reference.projects
                    .Where(name => (
                        selected.owners.TryGetValue(name, out ValidationDocumentation.Owner? owner)
                        && !owner.test && !owner.nativeReference && byPath[owner.path].productTarget.Length != 0
                        && byPath[owner.path].productTarget != target
                        && (affected.Contains(owner.path) || selected.changedReferenceOwners.Contains(name))
                        && selected.sourceIdentities[reference.path + "\0" + name].empty
                    ))
                    .Select(name => (reference, name, path: selected.owners[name].path)))
                .GroupBy(static item => item.reference.target);
            foreach (IGrouping<string?, (DocumentationReferenceInfo reference, string name, string path)> group in emptyForeign)
            {
                string[] paths = group
                    .Select(static item => item.path)
                    .Distinct(StringComparer.Ordinal)
                    .Order(StringComparer.Ordinal)
                    .ToArray();
                string[] references = group
                    .Select(static item => item.reference.path)
                    .Distinct(StringComparer.Ordinal)
                    .Order(StringComparer.Ordinal)
                    .ToArray();
                string identity = ValidationInputs.Identity(
                    group.Select(item => new KeyValuePair<string, string>(
                        item.path,
                        selected.sourceIdentities[item.reference.path + "\0" + item.name].identity
                    ))
                );
                checks.Add(
                    new ValidationCheck(
                        "documentation-api:source:" + (group.Key ?? "default"),
                        "documentation-api",
                        "InnoEngine.Docs",
                        "Verify the recorded empty public surface from actual target-selected source; this does not certify foreign product execution.",
                        "any",
                        ["documentation"],
                        CapturedInputs(paths.SelectMany(path => byPath[path].inputs))
                            .Append("@tooling")
                            .Order(StringComparer.Ordinal)
                            .ToArray(),
                        true,
                        null,
                        "managed",
                        "contract",
                        paths,
                        new ValidationDocumentationCheck(references, [], identity, configuration, group.Key)
                    )
                );
            }
            foreach (IGrouping<string, DocumentationPageInfo> group in examplePages.GroupBy(page => (
                scriptPages.Contains(page.id)
                || (page.examples?.projects ?? []).Any(name => (
                    nativeOwners.Contains(selected.owners[name].path)
                    || nativeDocumentOwners.Contains(selected.owners[name].path)
                )) ? "native" : "managed"
            ))
            ) {
                string[] paths = group.SelectMany(static page => page.examples?.projects ?? [])
                    .Select(name => selected.owners[name].path)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();
                ValidationCheck[] required = builds.Where(check => (
                    paths.Contains(check.owner, StringComparer.Ordinal)
                    || group.Key == "native"
                    && scriptExamples
                    && check.owner is "entry/InnoEngine.Build/InnoEngine.Build.csproj" or "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"
                )).ToArray();
                string[] pages = group.Select(static page => page.id).Order(StringComparer.Ordinal).ToArray();
                string[] scriptReferences = ValidationDocumentation.ExampleReferences(selected, group.ToArray());
                checks.Add(
                    new ValidationCheck(
                        "documentation-examples:" + group.Key,
                        "documentation-examples",
                        "InnoEngine.Docs",
                        "Canonical example blocks and their declared host or script context; no Markdown reparse or namespace guessing.",
                        "any",
                        required.Select(static check => check.id).Append("documentation").ToArray(),
                        required
                            .SelectMany(static check => check.inputs)
                            .Append("@tooling")
                            .Distinct(StringComparer.Ordinal)
                            .ToArray(),
                        true,
                        null,
                        group.Key,
                        "contract",
                        paths,
                        new ValidationDocumentationCheck(
                            scriptReferences,
                            pages,
                            ValidationDocumentation.ExampleIdentity(selected, group.ToArray(), scriptReferences),
                            configuration,
                            group.Key == "native" ? target : null
                        )
                    )
                );
            }
        }

        Task<string> GitAsync(params string[] arguments)
            => ValidationProcess.RunAsync("git", arguments, root, null, cancellationToken: cancellationToken);

        string[] CapturedInputs(IEnumerable<string> declared)
        {
            string[] paths = declared.ToArray();
            return paths.Where(inputs.ContainsKey).Concat(
                submoduleInputs.Where(parent =>
                    paths.Any(path => path.StartsWith(parent + "/", StringComparison.Ordinal)))
            ).Distinct(StringComparer.Ordinal).ToArray();
        }

        void RequireCompilerInputs(JsonElement snapshot)
        {
            foreach (JsonElement project in snapshot.EnumerateArray())
            {
            foreach (JsonElement variant in project.GetProperty("Variants").EnumerateArray().Append(project))
            {
            foreach (JsonElement item in variant.GetProperty("Items").GetProperty("Compile").EnumerateArray())
            {
                string path = Path.GetRelativePath(root, item.GetProperty("FullPath").GetString()!).Replace('\\', '/');
                if (path.StartsWith("../", StringComparison.Ordinal) || Path.IsPathRooted(path) || inputs.ContainsKey(path)
                    || submoduleInputs.Any(parent => path.StartsWith(parent + "/", StringComparison.Ordinal))
                ) {
                    continue;
                }
                if (File.Exists(ValidationInputs.Resolve(root, path)))
                {
                    throw new InvalidDataException(
                        $"The SDK selected an untracked ignored compiler input: {path}. Make its source ownership explicit before validation."
                    );
                }
            }
            }
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

    private static JsonElement? DevelopmentProject(
        JsonElement graph,
        string root
    ) => graph.EnumerateArray().Cast<JsonElement?>().FirstOrDefault(snapshot => (
        Path.GetRelativePath(root, snapshot!.Value.GetProperty("Path").GetString()!)
            .Replace('\\', '/') == "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"
    ));

    private static string DirectoryOf(string project) => project[..(project.LastIndexOf('/') + 1)];

    private static bool IsWorkflowInput(string path)
        => path.StartsWith(".github/", StringComparison.Ordinal) || path.StartsWith("tools/ci/", StringComparison.Ordinal)
           || path.StartsWith("tests/ci/", StringComparison.Ordinal);

    private static bool IsEditorProductInput(string path)
        => path is "tests/product/Invoke-EditorSmoke.ps1" or "tests/product/Invoke-EditorUiProductJourney.ps1"
           || path.StartsWith("tests/product/fixtures/editor-ui/", StringComparison.Ordinal);

    private static bool IsExecutable(string path)
        => Path.GetExtension(path).ToLowerInvariant() is ".cs" or ".csproj" or ".ps1" or ".sh" or ".py"
           or ".cc" or ".cxx" or ".h" or ".hpp" or ".m" or ".mm" or ".psm1" or ".cmd" or ".bat" or ".js" or ".ts"
           or ".cpp" or ".c" or ".targets" or ".props" or ".sln" or ".slnx" or ".cmake" || path == "global.json";

    private static bool IsDocumentation(string path)
        => path.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
           || (path.StartsWith("docs/", StringComparison.Ordinal) || path.StartsWith(".agents/records/", StringComparison.Ordinal))
              && (Path.GetExtension(path) is ".json" or ".csv" or ".png" or ".jpg" or ".svg"
                  || path.EndsWith(".cs.txt", StringComparison.Ordinal));
}
