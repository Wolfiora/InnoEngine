using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Architecture;

internal static class MSBuildProjectGraphValidator
{
    private static readonly HashSet<string> EXECUTABLE_PRODUCTS = new(StringComparer.Ordinal)
    {
        "InnoEngine.Editor", "InnoEngine.Build"
    };

    // Suites that prove one behavior across several production projects, so no single project names them.
    // Each one lives in the test directory of the production directory that owns most of that behavior.
    private static readonly Dictionary<string, string> CROSS_PROJECT_TEST_SUBJECTS = new(StringComparer.Ordinal)
    {
        // Native producer selection, caching and publication across the Build pipeline and its toolchains.
        ["Inno.Build.NativeProducer"] = "tests/build/pipeline",
        // Declared product profiles resolved through the Build pipeline and every registered platform.
        ["Inno.Build.ProductProfiles"] = "tests/build/pipeline",
        // Product composition across the Build pipeline, packaging and platform contributions.
        ["Inno.Build.Products"] = "tests/build/pipeline",
        // Support pack production and consumption across the Build pipeline and the Editor exporter.
        ["Inno.Build.SupportPacks"] = "tests/build/pipeline",
        // Editor feature projects that each carry fewer than five tests.
        ["Inno.Editor.Features"] = "tests/composition/editor/features",
        // Native layout, keyboard and lifecycle behavior of the Editor panels composed together.
        ["Inno.Editor.Panels"] = "tests/composition/editor/panels"
    };

    internal static async Task<IReadOnlyDictionary<string, string>> ValidateAsync(
        string root,
        string configuration,
        string dotnetHost,
        string? artifactsPath,
        string? outputPath,
        ICollection<string> failures,
        string? capturedGraph = null,
        IReadOnlySet<string>? selectedOwners = null,
        CancellationToken cancellationToken = default
    ) {
        if (!Path.IsPathFullyQualified(dotnetHost) || !File.Exists(dotnetHost))
        {
            throw new ArgumentException("Architecture SDK evaluation requires an existing absolute dotnet executable.", nameof(dotnetHost));
        }
        string[] projects = RepositorySourceInventory.Projects(root)
            .Where(project => selectedOwners is null || selectedOwners.Contains(Relative(root, project)))
            .Order(StringComparer.Ordinal)
            .ToArray();
        // A test directory mirrors the directory of its subject; production roots other than src keep their name.
        Dictionary<string, string> owners = RepositorySourceInventory.Projects(root)
            .GroupBy(static project => Path.GetFileNameWithoutExtension(project), StringComparer.Ordinal)
            .ToDictionary(
                static project => project.Key,
                project => TestDirectory(Path.GetDirectoryName(Path.GetDirectoryName(Relative(root, project.First()))!)!),
                StringComparer.Ordinal
            );
        List<EvaluatedProject> evaluated = [];
        var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Configuration"] = configuration,
            ["DesignTimeBuild"] = "true",
            ["BuildProjectReferences"] = "false",
            ["InnoProductRestore"] = "false"
        };
        if (artifactsPath is not null)
        {
            properties.Add("ArtifactsPath", artifactsPath);
        }
        string graph = capturedGraph
                       ?? await ManagedProjectGraphCapture.CaptureAsync(
                           root,
                           dotnetHost,
                           projects,
                           properties,
                           cancellationToken: cancellationToken
                       ).ConfigureAwait(false);
        using JsonDocument snapshots = JsonDocument.Parse(graph);
        foreach (JsonElement snapshot in snapshots.RootElement.EnumerateArray())
        {
            cancellationToken.ThrowIfCancellationRequested();
            EvaluatedProject project = ReadSnapshot(root, snapshot);
            if (selectedOwners is not null && !selectedOwners.Contains(project.path))
            {
                continue;
            }
            ValidateProject(root, project, owners, failures);
            if (snapshot.TryGetProperty("Variants", out JsonElement variants))
            {
                EvaluatedProject[] effective = variants.EnumerateArray().Select(variant => ReadSnapshot(root, variant)).ToArray();
                foreach (EvaluatedProject variant in effective)
                {
                    ValidateProject(root, variant, owners, failures);
                }
                project = project with
                {
                    references = effective.SelectMany(static variant => variant.references).Concat(project.references).Distinct().ToArray(),
                    native = project.native || effective.Any(static variant => variant.native)
                };
            }
            evaluated.Add(project);
        }
        if (!projects.Select(project => Relative(root, project))
            .Order(StringComparer.Ordinal)
            .SequenceEqual(evaluated.Select(static project => project.path).Order(StringComparer.Ordinal))
        ) {
            throw new InvalidDataException("The SDK snapshot does not cover the exact current project inventory.");
        }
        ValidateGraph(evaluated, failures);
        ValidateTestSupport(evaluated, failures);
        if (outputPath is not null)
        {
            string destination = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await File.WriteAllTextAsync(
                destination,
                JsonSerializer.Serialize(evaluated, new JsonSerializerOptions { WriteIndented = true }),
                cancellationToken
            ).ConfigureAwait(false);
        }
        Console.WriteLine($"Evaluated MSBuild ownership for {evaluated.Count}/{projects.Length} projects without executing build targets.");
        return evaluated.ToDictionary(
            project => Path.GetFullPath(project.path, root),
            static project => project.targetPath,
            StringComparer.OrdinalIgnoreCase
        );
    }

    private static EvaluatedProject ReadSnapshot(
        string root,
        JsonElement snapshot
    ) {
        string project = snapshot.GetProperty("Path").GetString()!;
        JsonElement properties = snapshot.GetProperty("Properties");
        JsonElement items = snapshot.GetProperty("Items");
        EvaluatedReference[] references = items.GetProperty("ProjectReference")
            .EnumerateArray()
            .Select(item => new EvaluatedReference(
                Relative(root, item.GetProperty("FullPath").GetString()!),
                !string.Equals(Value(item, "ReferenceOutputAssembly"), "false", StringComparison.OrdinalIgnoreCase),
                Value(item, "OutputItemType")
            ))
            .ToArray();
        EvaluatedSource[] sources = items.GetProperty("Compile")
            .EnumerateArray()
            .Select(item => new EvaluatedSource(
                Relative(root, item.GetProperty("FullPath").GetString()!),
                Value(item, "Link")
            ))
            .ToArray();
        return new(
            Relative(root, project),
            Value(properties, "AssemblyName"),
            Value(properties, "OutputType"),
            Value(properties, "InnoProductId"),
            Value(properties, "InnoProductTarget"),
            Value(properties, "InnoNativeTarget"),
            Value(properties, "ImplicitUsings"),
            items.GetProperty("Using").GetArrayLength(),
            references,
            sources,
            Value(properties, "TargetPath"),
            Value(properties, "InnoValidationLayer"),
            Value(properties, "IsTestProject").Equals("true", StringComparison.OrdinalIgnoreCase),
            Value(properties, "IsTestSupportProject").Equals("true", StringComparison.OrdinalIgnoreCase)
            || items.TryGetProperty("PackageReference", out JsonElement packages)
               && packages.EnumerateArray().Any(static item => RepositorySourceInventory.IsTestPackage(Value(item, "Identity"))),
            Value(properties, "InnoTestNativeProduct").Length != 0 || Value(properties, "InnoProductId").Length != 0,
            Value(properties, "InnoTestNativeProduct").Length != 0
        );
    }

    private static void ValidateProject(
        string root,
        EvaluatedProject project,
        IReadOnlyDictionary<string, string> owners,
        ICollection<string> failures
    ) {
        bool test = RepositorySourceInventory.IsTestPath(project.path);
        if ((project.isTest || project.testSupport) && !test)
        {
            failures.Add($"{project.path}: effective test projects must belong to the root tests directory.");
        }
        if (project.isTest && project.layer is not ("domain" or "contract" or "integration" or "native" or "product"))
        {
            failures.Add($"{project.path}: test execution layer must declare its actual execution requirements.");
        }
        if (project.isTest)
        {
            ValidateTestIdentity(project, owners, failures);
        }
        if (project.implicitUsings.Equals("enable", StringComparison.OrdinalIgnoreCase)
            || project.implicitUsings.Equals("true", StringComparison.OrdinalIgnoreCase)
            || project.usingItems != 0
        ) {
            failures.Add($"{project.path}: effective compilation-wide using imports are forbidden.");
        }
        if (!test
            && project.outputType == "Exe"
            && project.path != "tools/Inno.Tooling.Cli/Inno.Tooling.Cli.csproj"
            && !EXECUTABLE_PRODUCTS.Contains(project.assemblyName)
        ) {
            failures.Add($"{project.path}: effective production executable is not an approved product.");
        }
        if (project.path.StartsWith("platforms/", StringComparison.Ordinal) && project.outputType == "Exe"
            && (project.productId.Length == 0 || project.target.Length == 0 || project.nativeTarget != project.target)
        ) {
            failures.Add($"{project.path}: effective product target and native ABI must be explicitly assigned and equal.");
        }
        foreach (EvaluatedReference reference in project.references)
        {
            if (!File.Exists(Path.GetFullPath(reference.path, root)))
            {
                failures.Add($"{project.path}: effective project dependency is missing: {reference.path}.");
            }
            if (!reference.runtime || reference.outputItemType == "Analyzer")
            {
                continue;
            }
            PlatformOwnershipValidator.ValidateDependency(project.path, reference.path, failures);
            if (project.assemblyName == "Inno.Rendering"
                && !reference.path.StartsWith("src/foundation/", StringComparison.Ordinal)
            ) {
                failures.Add($"{project.path}: effective Rendering Core dependencies must belong to Foundation: {reference.path}.");
            }
            if (project.path.StartsWith("src/foundation/", StringComparison.Ordinal)
                && !reference.path.StartsWith("src/foundation/", StringComparison.Ordinal)
            ) {
                failures.Add($"{project.path}: effective Foundation dependency belongs to an upper layer: {reference.path}.");
            }
        }
        foreach (EvaluatedSource source in project.sources)
        {
            bool generated = source.path.Contains("/Generated/", StringComparison.Ordinal)
                             || source.path.Contains("/obj/", StringComparison.Ordinal);
            string owner = Path.GetDirectoryName(project.path)!.Replace('\\', '/') + "/";
            if (!generated && (source.link.Length != 0 || !source.path.StartsWith(owner, StringComparison.Ordinal)))
            {
                failures.Add($"{project.path}: effective hand-authored Compile source has a second owner: {source.path}.");
            }
        }
    }

    private static void ValidateTestIdentity(
        EvaluatedProject project,
        IReadOnlyDictionary<string, string> owners,
        ICollection<string> failures
    ) {
        const string C_TESTS = ".Tests";
        string name = Path.GetFileNameWithoutExtension(project.path);
        if (!name.EndsWith(C_TESTS, StringComparison.Ordinal))
        {
            failures.Add($"{project.path}: test project name must end with {C_TESTS}.");
            return;
        }
        string subject = name[..^C_TESTS.Length];
        // A binding project already names the native layer.
        string named = subject.StartsWith("Inno.Native.", StringComparison.Ordinal) ? "native" : string.Empty;
        foreach (string layer in new[] { "integration", "native", "product" })
        {
            if (subject.EndsWith("." + layer, StringComparison.OrdinalIgnoreCase))
            {
                named = layer;
                subject = subject[..^(layer.Length + 1)];
                break;
            }
        }
        if (named.Length == 0 ? project.layer is not ("domain" or "contract") : project.layer != named)
        {
            failures.Add(
                $"{project.path}: test project name and execution layer '{project.layer}' must agree; "
                + "integration, native and product suites end with .Integration.Tests, .Native.Tests and .Product.Tests."
            );
        }
        if (project.nativeProduct && project.layer is not ("native" or "product"))
        {
            failures.Add($"{project.path}: a test that deploys a native product must declare the native or product execution layer.");
        }
        if (!CROSS_PROJECT_TEST_SUBJECTS.TryGetValue(subject, out string? directory) && !owners.TryGetValue(subject, out directory))
        {
            failures.Add($"{project.path}: test project must be named after the project it tests or a declared cross-project suite.");
            return;
        }
        if (Path.GetDirectoryName(Path.GetDirectoryName(project.path))!.Replace('\\', '/') != directory)
        {
            failures.Add($"{project.path}: test project must live in {directory}, the test directory of the project it tests.");
        }
    }

    private static string TestDirectory(string directory)
    {
        string normalized = directory.Replace('\\', '/');
        if (RepositorySourceInventory.IsTestPath(normalized + "/"))
        {
            return normalized;
        }
        return normalized == "src"
               ? "tests"
               : "tests/" + (normalized.StartsWith("src/", StringComparison.Ordinal) ? normalized["src/".Length..] : normalized);
    }

    private static void ValidateTestSupport(
        IReadOnlyList<EvaluatedProject> projects,
        ICollection<string> failures
    ) {
        const string C_SUPPORT = "tests/support";
        const string C_SHARED = "Inno.Testing.";
        foreach (EvaluatedProject project in projects)
        {
            if (project.isTest || !RepositorySourceInventory.IsTestPath(project.path))
            {
                continue;
            }
            string name = Path.GetFileNameWithoutExtension(project.path);
            string directory = Path.GetDirectoryName(Path.GetDirectoryName(project.path))!.Replace('\\', '/');
            if (project.path == "tests/product/Inno.Testing.EditorJourney/Inno.Testing.EditorJourney.csproj" && project.outputType == "Exe")
            {
                continue;
            }
            if (directory == C_SUPPORT)
            {
                if (!name.StartsWith(C_SHARED, StringComparison.Ordinal))
                {
                    failures.Add($"{project.path}: shared test support in {C_SUPPORT} must be named {C_SHARED}*.");
                }
                continue;
            }
            if (!directory.EndsWith("/fixtures", StringComparison.Ordinal))
            {
                failures.Add(
                    $"{project.path}: test support belongs to {C_SUPPORT} when several test directories share it, "
                    + "or to the fixtures directory of the one test directory that owns it."
                );
                continue;
            }
            // A module fixture is loaded as test input and stays with its owner; a helper library has one owning directory.
            string owner = directory[..^"fixtures".Length];
            if (name.StartsWith(C_SHARED, StringComparison.Ordinal))
            {
                foreach (EvaluatedProject consumer in projects.Where(consumer => (
                    !consumer.path.StartsWith(owner, StringComparison.Ordinal)
                    && consumer.references.Any(reference => reference.path.Equals(project.path, StringComparison.OrdinalIgnoreCase))
                ))) {
                    failures.Add(
                        $"{project.path}: test support used from {consumer.path} is shared by several test directories "
                        + $"and belongs to {C_SUPPORT}."
                    );
                }
            }
        }
    }

    private static void ValidateGraph(
        IReadOnlyList<EvaluatedProject> projects,
        ICollection<string> failures
    ) {
        Dictionary<string, EvaluatedProject> graph = projects.ToDictionary(
            static project => project.path,
            StringComparer.OrdinalIgnoreCase
        );
        // Reload fixtures intentionally compile distinct generations with the same assembly identity.
        foreach (IGrouping<string, EvaluatedProject> identity in projects
            .Where(static project => !RepositorySourceInventory.IsTestPath(project.path))
            .GroupBy(static project => project.assemblyName, StringComparer.Ordinal)
        ) {
            if (identity.Count() != 1)
            {
                failures.Add($"Effective assembly identity '{identity.Key}' has multiple project owners.");
            }
        }
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var active = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void Visit(EvaluatedProject project)
        {
            if (active.Contains(project.path))
            {
                failures.Add($"{project.path}: effective runtime project references contain a cycle.");
                return;
            }
            if (!visited.Add(project.path))
            {
                return;
            }
            active.Add(project.path);
            foreach (EvaluatedReference reference in project.references.Where(
                static reference => reference.runtime && reference.outputItemType != "Analyzer"
            )) {
                if (graph.TryGetValue(reference.path, out EvaluatedProject? dependency))
                {
                    Visit(dependency);
                }
            }
            active.Remove(project.path);
        }
        foreach (EvaluatedProject project in projects)
        {
            Visit(project);
        }
        foreach (EvaluatedProject domain in projects.Where(static project => project.isTest && project.layer == "domain"))
        {
            var pending = new Stack<EvaluatedProject>();
            visited.Clear();
            pending.Push(domain);
            while (pending.TryPop(out EvaluatedProject? current))
            {
                if (!visited.Add(current.path))
                {
                    continue;
                }
                if (current.native)
                {
                    failures.Add($"{domain.path}: domain test build closure requires native preparation through {current.path}.");
                }
                foreach (EvaluatedReference reference in current.references)
                {
                    if (graph.TryGetValue(reference.path, out EvaluatedProject? dependency))
                    {
                        pending.Push(dependency);
                    }
                    else
                    {
                        failures.Add($"{domain.path}: domain test build dependency has no evaluated owner: {reference.path}.");
                    }
                }
            }
        }
        foreach (EvaluatedProject player in projects.Where(
            static project => project.assemblyName.StartsWith("Inno.Player.", StringComparison.Ordinal)
        )) {
            var pending = new Stack<EvaluatedProject>();
            visited.Clear();
            pending.Push(player);
            while (pending.TryPop(out EvaluatedProject? current))
            {
                if (!visited.Add(current.path))
                {
                    continue;
                }
                if (ArchitectureRules.IsForbiddenPlayerDependency(current.assemblyName))
                {
                    failures.Add($"{player.path}: effective Player closure includes forbidden authoring/build dependency {current.path}.");
                }
                foreach (EvaluatedReference reference in current.references.Where(
                    static reference => reference.runtime && reference.outputItemType != "Analyzer"
                )) {
                    if (graph.TryGetValue(reference.path, out EvaluatedProject? dependency))
                    {
                        pending.Push(dependency);
                    }
                }
            }
        }
    }

    private static string Value(
        JsonElement element,
        string property
    ) => element.TryGetProperty(property, out JsonElement value) ? value.GetString() ?? string.Empty : string.Empty;

    private static string Relative(
        string root,
        string path
    ) => Path.GetRelativePath(root, path).Replace('\\', '/');

    private sealed record EvaluatedProject(
        string path,
        string assemblyName,
        string outputType,
        string productId,
        string target,
        string nativeTarget,
        string implicitUsings,
        int usingItems,
        IReadOnlyList<EvaluatedReference> references,
        IReadOnlyList<EvaluatedSource> sources,
        string targetPath,
        string layer,
        bool isTest,
        bool testSupport,
        bool native,
        bool nativeProduct
    );

    private sealed record EvaluatedReference(
        string path,
        bool runtime,
        string outputItemType
    );

    private sealed record EvaluatedSource(
        string path,
        string link
    );
}
