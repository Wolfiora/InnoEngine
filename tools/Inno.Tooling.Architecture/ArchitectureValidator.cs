using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Checks repository dependency, API documentation and source ownership invariants.
/// </summary>
public static partial class ArchitectureValidator
{
    private static readonly string[] S_PRODUCTION_ROOTS = ["src", "backends", "platforms", "build", "tools"];
    private static readonly HashSet<string> S_IGNORED_DIRECTORIES = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin",
        "obj",
        "Generated"
    };
    private static readonly HashSet<string> S_FORBIDDEN_DIRECTORY_NAMES = new(StringComparer.OrdinalIgnoreCase)
    {
        "Legacy",
        "Compatibility",
        "Migration",
        "Former",
        "Deprecated"
    };

    /// <summary>
    /// Executes repository validation or an explicitly requested documentation maintenance operation.
    /// </summary>
    /// <param name="arguments">
    /// The optional repository path, configuration, XML maintenance flag, explicit SDK and evaluated graph output.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels owned SDK evaluation processes and drains them before returning.
    /// </param>
    /// <returns>
    /// Zero when validation succeeds, or one when any invariant is violated.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An option is unknown or more than one repository path is supplied.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The requested path is not inside an engine checkout.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels evaluated SDK project inspection.
    /// </exception>
    public static int Execute(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        bool expandXml = false;
        string configuration = "Debug";
        bool selectedConfiguration = false;
        string? dotnetHost = null;
        string? projectGraphPath = null;
        var paths = new List<string>();
        for (int index = 0; index < arguments.Length; index++)
        {
            string argument = arguments[index];
            if (argument == "--expand-xml" && !expandXml)
                expandXml = true;
            else if (argument == "--dotnet" && dotnetHost is null && index + 1 < arguments.Length)
                dotnetHost = arguments[++index];
            else if (argument == "--project-graph" && projectGraphPath is null && index + 1 < arguments.Length)
                projectGraphPath = arguments[++index];
            else if (argument == "--configuration" && !selectedConfiguration && index + 1 < arguments.Length)
            {
                configuration = arguments[++index] switch
                {
                    "Debug" => "Debug",
                    "Release" => "Release",
                    _ => throw new ArgumentException("Architecture configuration must be Debug or Release.", nameof(arguments))
                };
                selectedConfiguration = true;
            }
            else if (!argument.StartsWith("--", StringComparison.Ordinal) && paths.Count == 0)
                paths.Add(argument);
            else
                throw new ArgumentException("Usage: verify [engine-root] [--configuration Debug|Release] [--expand-xml] [--dotnet SDK] [--project-graph output.json]", nameof(arguments));
        }
        if (projectGraphPath is not null && dotnetHost is null)
            throw new ArgumentException("Evaluated project graph output requires an explicitly selected --dotnet SDK.", nameof(arguments));
        string repositoryRoot = ResolveRepositoryRoot(paths);
        if (expandXml)
        {
            int changedFileCount = ExpandXmlDocumentation(repositoryRoot);
            Console.WriteLine($"Expanded XML documentation in {changedFileCount} file(s).");
            return 0;
        }
        List<string> failures = [];

        var documentationModels = new DocumentationSourceModels(repositoryRoot, configuration);
        foreach (string rootName in S_PRODUCTION_ROOTS)
        {
            string root = Path.Combine(repositoryRoot, rootName);
            if (!Directory.Exists(root))
                continue;
            ValidateDirectories(repositoryRoot, root, failures);
            ValidateSources(repositoryRoot, root, failures, documentationModels);
            ValidateProjects(repositoryRoot, root, failures);
        }

        ValidateProjectReferences(repositoryRoot, failures);
        ValidateProductionSolutionFolders(repositoryRoot, failures);
        ValidateTestSolutionFolders(repositoryRoot, failures);
        ArchitectureRules.Validate(repositoryRoot, failures);
        PublicApiBoundaryValidator.Validate(repositoryRoot, configuration, failures);
        if (dotnetHost is not null)
            MSBuildProjectGraphValidator.ValidateAsync(repositoryRoot, configuration, dotnetHost,
                projectGraphPath, failures, cancellationToken).GetAwaiter().GetResult();
        if (failures.Count == 0)
        {
            Console.WriteLine("InnoEngine architecture validation passed.");
            return 0;
        }

        Console.Error.WriteLine($"InnoEngine architecture validation failed with {failures.Count} violation(s):");
        foreach (string failure in failures.Order(StringComparer.Ordinal))
            Console.Error.WriteLine("  " + failure);
        return 1;
    }

    private static string ResolveRepositoryRoot(IReadOnlyList<string> arguments)
    {
        string start = arguments.Count == 0
            ? Directory.GetCurrentDirectory()
            : Path.GetFullPath(arguments[0]);
        DirectoryInfo? current = new(start);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "InnoEngine.sln")))
                return current.FullName;
            current = current.Parent;
        }
        throw new DirectoryNotFoundException("Could not locate the InnoEngine repository root.");
    }

    private static void ValidateDirectories(
        string repositoryRoot,
        string root,
        ICollection<string> failures
    ) {
        foreach (string directory in EnumerateDirectories(root))
        {
            string name = Path.GetFileName(directory);
            if (S_FORBIDDEN_DIRECTORY_NAMES.Contains(name))
            {
                failures.Add($"{Relative(repositoryRoot, directory)}: forbidden compatibility directory name '{name}'.");
            }
        }
    }

    private static void ValidateSources(
        string repositoryRoot,
        string root,
        ICollection<string> failures,
        DocumentationSourceModels documentationModels
    ) {
        foreach (string path in EnumerateFiles(root, "*.cs"))
        {
            string source = File.ReadAllText(path);
            if (IsGenerated(source))
                continue;
            string relative = Relative(repositoryRoot, path);
            CSharpStyleValidator.Validate(relative, source, failures);
            if (relative.StartsWith("tools/Inno.Tooling.Architecture/", StringComparison.Ordinal)
                || relative.Contains("/tests/", StringComparison.Ordinal))
                continue;
            PublicApiDocumentationValidator.Validate(relative, source, failures,
                source.Contains("<inheritdoc", StringComparison.Ordinal) ? documentationModels.GetModel(path) : null,
                documentationModels);
            if (RuntimeContentBoundaryValidator.ShouldAudit(relative))
                RuntimeContentBoundaryValidator.Validate(relative, documentationModels.GetModel(path), failures);
            if (relative.StartsWith("platforms/", StringComparison.Ordinal) && relative.Contains("/Targets/", StringComparison.Ordinal))
            {
                SemanticModel? model = documentationModels.GetModel(path);
                if (model is not null)
                {
                    foreach (VariableDeclaratorSyntax field in model.SyntaxTree.GetRoot()
                        .DescendantNodes().OfType<VariableDeclaratorSyntax>())
                    {
                        if (model.GetDeclaredSymbol(field) is IFieldSymbol symbol
                            && symbol.Type is INamedTypeSymbol type
                            && (type.ToDisplayString() == "Inno.Build.IGameContentCompiler"
                                || type.AllInterfaces.Any(static contract => contract.ToDisplayString() == "Inno.Build.IGameContentCompiler")))
                            failures.Add($"{relative}: platform packaging cannot own a content compiler; bind it in the distribution.");
                    }
                }
            }
            GenerationCleanupValidator.Validate(relative, source, failures);
            AddSourceFailure(source.Contains("InternalsVisibleTo", StringComparison.Ordinal), relative,
                "friend assemblies are forbidden", failures);
            AddSourceFailure(source.Contains("[Obsolete", StringComparison.Ordinal), relative,
                "obsolete compatibility APIs are forbidden", failures);
            AddSourceFailure(source.Contains("TypeForwardedTo", StringComparison.Ordinal), relative,
                "type forwarding is forbidden", failures);
            AddSourceFailure(CompatibilityFieldPattern().IsMatch(source), relative,
                "schema compatibility fields are forbidden", failures);
            AddSourceFailure(GlobalUsingPattern().IsMatch(source), relative,
                "global using directives are forbidden", failures);
            AddSourceFailure(UninformativeXmlPattern().IsMatch(source), relative,
                "public API XML contains an uninformative generated placeholder", failures);

            int lineNumber = 0;
            using var reader = new StringReader(source);
            string? line;
            while ((line = reader.ReadLine()) is not null)
            {
                lineNumber++;
                if (SingleLineXmlPattern().IsMatch(line))
                {
                    failures.Add($"{relative}:{lineNumber}: XML documentation elements must use expanded multi-line form.");
                }
            }
        }
    }

    private static int ExpandXmlDocumentation(string repositoryRoot)
    {
        int changedFileCount = 0;
        foreach (string rootName in S_PRODUCTION_ROOTS)
        {
            string root = Path.Combine(repositoryRoot, rootName);
            if (!Directory.Exists(root))
                continue;
            foreach (string path in EnumerateFiles(root, "*.cs"))
            {
                string source = File.ReadAllText(path);
                if (IsGenerated(source))
                    continue;
                string newline = source.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
                string expanded = ExpandableXmlPattern().Replace(source, match =>
                {
                    string indentation = match.Groups[1].Value;
                    string element = match.Groups[2].Value;
                    string attributes = match.Groups[3].Value;
                    string content = match.Groups[4].Value.Trim();
                    return string.Join(
                        newline,
                        $"{indentation}/// <{element}{attributes}>",
                        $"{indentation}/// {content}",
                        $"{indentation}/// </{element}>");
                });
                if (string.Equals(source, expanded, StringComparison.Ordinal))
                    continue;
                File.WriteAllText(path, expanded);
                changedFileCount++;
            }
        }
        return changedFileCount;
    }

    private static void ValidateProjects(
        string repositoryRoot,
        string root,
        ICollection<string> failures
    ) {
        foreach (string path in EnumerateFiles(root, "*.csproj"))
        {
            XDocument project = XDocument.Load(path, LoadOptions.SetLineInfo);
            string relative = Relative(repositoryRoot, path);
            foreach (XElement implicitUsings in project.Descendants("ImplicitUsings"))
            {
                if (string.Equals(implicitUsings.Value.Trim(), "enable", StringComparison.OrdinalIgnoreCase))
                    failures.Add($"{relative}: implicit usings must be disabled.");
            }
            foreach (XElement usingItem in project.Descendants("Using"))
                failures.Add($"{relative}: MSBuild Using items are forbidden ({usingItem.Attribute("Include")?.Value}).");
        }
    }

    private static void ValidateProjectReferences(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        foreach (string projectPath in EnumerateFiles(repositoryRoot, "*.csproj"))
        {
            if (IsIgnoredPath(projectPath))
                continue;
            string projectRelative = Relative(repositoryRoot, projectPath);
            XDocument project = XDocument.Load(projectPath);
            foreach (XElement reference in project.Descendants("ProjectReference"))
            {
                string? include = reference.Attribute("Include")?.Value;
                if (string.IsNullOrWhiteSpace(include))
                    continue;
                string target = Path.GetFullPath(include, Path.GetDirectoryName(projectPath)!);
                string targetRelative = Relative(repositoryRoot, target);
                if (projectRelative.StartsWith("src/foundation/", StringComparison.Ordinal) &&
                    !targetRelative.StartsWith("src/foundation/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: Foundation cannot reference upper-layer project {targetRelative}.");
                }
                if (projectRelative.StartsWith("build/", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("build/cli/", StringComparison.Ordinal) &&
                    targetRelative.StartsWith("src/composition/editor/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: Build cannot reference Editor project {targetRelative}.");
                }
                if (projectRelative.StartsWith("src/runtime/", StringComparison.Ordinal) &&
                    (targetRelative.StartsWith("src/composition/", StringComparison.Ordinal) ||
                     targetRelative.StartsWith("build/", StringComparison.Ordinal)))
                {
                    failures.Add($"{projectRelative}: Runtime cannot reference {targetRelative}.");
                }
                if (targetRelative.Contains("Inno.Native.Bgfx", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Adapter.Rendering.Bgfx", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Native.Bgfx", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("backends/Bgfx/build/Inno.Build.Toolchains.Bgfx", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("tests/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: BGFX native code is restricted to the BGFX adapter.");
                }
                if (targetRelative.Contains("Inno.Native.Sdl3", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Adapter.Platform.Sdl3", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Adapter.Presentation.ImGui.Sdl3", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Native.Sdl3", StringComparison.Ordinal) &&
                    !(projectRelative.StartsWith("platforms/", StringComparison.Ordinal)
                        && projectRelative.Contains("/integrations/Inno.Integration.", StringComparison.Ordinal)
                        && projectRelative.Contains(".Sdl3/", StringComparison.Ordinal)) &&
                    !projectRelative.StartsWith("backends/Sdl3/build/Inno.Build.Toolchains.Sdl3", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("tests/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: SDL3 native code is restricted to the SDL3 platform adapter.");
                }
                if (targetRelative.Contains("Inno.Native.MiniAudio", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Adapter.Audio.MiniAudio", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Native.MiniAudio", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("backends/MiniAudio/build/Inno.Build.Toolchains.MiniAudio", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("tests/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: miniaudio native code is restricted to the MiniAudio adapter.");
                }
                if (targetRelative.Contains("Inno.Native.UI", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Adapter.UI.RmlUi", StringComparison.Ordinal) &&
                    !projectRelative.Contains("Inno.Native.UI", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("backends/RmlUi/build/Inno.Build.Toolchains.UI", StringComparison.Ordinal) &&
                    !projectRelative.StartsWith("tests/", StringComparison.Ordinal))
                {
                    failures.Add($"{projectRelative}: RmlUi native code is restricted to the RmlUi adapter.");
                }
            }
        }
    }

    private static void ValidateTestSolutionFolders(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string solutionPath = Path.Combine(repositoryRoot, "InnoEngine.sln");
        var names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var solutionFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var parents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        bool readingNestedProjects = false;

        foreach (string line in File.ReadLines(solutionPath))
        {
            Match project = SolutionProjectPattern().Match(line);
            if (project.Success)
            {
                string id = project.Groups["id"].Value;
                names[id] = project.Groups["name"].Value;
                paths[id] = project.Groups["path"].Value.Replace('\\', '/');
                if (string.Equals(project.Groups["type"].Value, "{2150E333-8FDC-42A3-9474-1A3956D46DE8}", StringComparison.OrdinalIgnoreCase))
                    solutionFolders.Add(id);
                continue;
            }

            if (line.Contains("GlobalSection(NestedProjects)", StringComparison.Ordinal))
            {
                readingNestedProjects = true;
                continue;
            }
            if (readingNestedProjects && line.Contains("EndGlobalSection", StringComparison.Ordinal))
            {
                readingNestedProjects = false;
                continue;
            }
            if (!readingNestedProjects)
                continue;

            Match nesting = SolutionNestingPattern().Match(line);
            if (nesting.Success)
                parents[nesting.Groups[1].Value] = nesting.Groups[2].Value;
        }

        string? testsRoot = solutionFolders.FirstOrDefault(id =>
            string.Equals(names.GetValueOrDefault(id), "tests", StringComparison.Ordinal) && !parents.ContainsKey(id));
        if (testsRoot is null)
        {
            failures.Add("InnoEngine.sln: missing root tests Solution Folder.");
            return;
        }

        var declaredTestProjects = paths
            .Where(static pair => pair.Value.StartsWith("tests/", StringComparison.Ordinal) &&
                                  pair.Value.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(static pair => pair.Value, static pair => pair.Key, StringComparer.OrdinalIgnoreCase);
        string testsDirectory = Path.Combine(repositoryRoot, "tests");
        foreach (string projectPath in EnumerateFiles(testsDirectory, "*.csproj"))
        {
            string relative = Relative(repositoryRoot, projectPath);
            if (!declaredTestProjects.ContainsKey(relative))
                failures.Add($"{relative}: test project is missing from InnoEngine.sln.");
        }

        foreach ((string projectPath, string projectId) in declaredTestProjects)
        {
            if (!parents.TryGetValue(projectId, out string? parentId) ||
                string.Equals(parentId, testsRoot, StringComparison.OrdinalIgnoreCase) ||
                !solutionFolders.Contains(parentId) ||
                !HasSolutionAncestor(parentId, testsRoot, parents))
            {
                failures.Add($"{projectPath}: test project must be nested below a tests/<domain> Solution Folder.");
                continue;
            }

            string projectName = names[projectId];
            bool isFixture = projectName.Contains(".TestModule", StringComparison.Ordinal) ||
                             projectName.Contains(".TestAssembly", StringComparison.Ordinal) ||
                             projectName.Contains(".TestDependency", StringComparison.Ordinal);
            if (isFixture && !HasNamedSolutionAncestor(parentId, "fixtures", names, parents))
                failures.Add($"{projectPath}: test support project must be nested below a fixtures Solution Folder.");

            string actualPath = GetSolutionFolderPath(projectId, names, solutionFolders, parents);
            string physicalGroup = Path.GetDirectoryName(Path.GetDirectoryName(projectPath))?
                .Replace('\\', '/').TrimEnd('/') ?? string.Empty;
            if (!string.Equals(physicalGroup, actualPath, StringComparison.Ordinal))
            {
                failures.Add(
                    $"{projectPath}: physical test folder '{physicalGroup}' must match Solution Folder '{actualPath}'.");
            }
        }
    }

    private static void ValidateProductionSolutionFolders(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string solutionPath = Path.Combine(repositoryRoot, "InnoEngine.sln");
        var names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var solutionFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var parents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        bool readingNestedProjects = false;

        foreach (string line in File.ReadLines(solutionPath))
        {
            Match project = SolutionProjectPattern().Match(line);
            if (project.Success)
            {
                string id = project.Groups["id"].Value;
                names[id] = project.Groups["name"].Value;
                paths[id] = project.Groups["path"].Value.Replace('\\', '/');
                if (string.Equals(project.Groups["type"].Value, "{2150E333-8FDC-42A3-9474-1A3956D46DE8}", StringComparison.OrdinalIgnoreCase))
                    solutionFolders.Add(id);
                continue;
            }

            if (line.Contains("GlobalSection(NestedProjects)", StringComparison.Ordinal))
            {
                readingNestedProjects = true;
                continue;
            }
            if (readingNestedProjects && line.Contains("EndGlobalSection", StringComparison.Ordinal))
            {
                readingNestedProjects = false;
                continue;
            }
            if (!readingNestedProjects)
                continue;

            Match nesting = SolutionNestingPattern().Match(line);
            if (!nesting.Success)
                continue;
            string childId = nesting.Groups[1].Value;
            string parentId = nesting.Groups[2].Value;
            if (!parents.TryAdd(childId, parentId))
                failures.Add($"InnoEngine.sln: project or folder '{names.GetValueOrDefault(childId, childId)}' has more than one Solution Folder parent.");
        }

        var declaredSourceProjects = paths
            .Where(static pair => S_PRODUCTION_ROOTS.Any(root =>
                                      pair.Value.StartsWith(root + "/", StringComparison.Ordinal)) &&
                                  pair.Value.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(static pair => pair.Value, static pair => pair.Key, StringComparer.OrdinalIgnoreCase);
        foreach (string projectPath in S_PRODUCTION_ROOTS
                     .Select(root => Path.Combine(repositoryRoot, root))
                     .Where(Directory.Exists)
                     .SelectMany(root => EnumerateFiles(root, "*.csproj")))
        {
            string relative = Relative(repositoryRoot, projectPath);
            if (!declaredSourceProjects.ContainsKey(relative))
                failures.Add($"{relative}: project is missing from InnoEngine.sln.");
        }

        foreach ((string projectPath, string projectId) in declaredSourceProjects)
        {
            string projectName = names[projectId];
            string sourceRoot = projectPath[..projectPath.IndexOf('/')];
            string? expectedPath = sourceRoot switch
            {
                "tools" => sourceRoot,
                "backends" or "platforms" => Path.GetDirectoryName(Path.GetDirectoryName(projectPath))?.Replace('\\', '/'),
                "build" => Path.GetDirectoryName(Path.GetDirectoryName(projectPath))?.Replace('\\', '/'),
                _ => ClassifySourceSolutionPath(projectName)
            };
            if (expectedPath is null)
            {
                failures.Add($"{projectPath}: source project '{projectName}' has no conceptual Solution Folder classification.");
                continue;
            }

            string projectDirectory = Path.GetDirectoryName(Path.Combine(repositoryRoot, projectPath))!;
            if (EnumerateFiles(projectDirectory, "*.csproj").Any(path => Path.GetFullPath(path) != Path.GetFullPath(Path.Combine(repositoryRoot, projectPath))))
                expectedPath = Path.GetDirectoryName(projectPath)!.Replace('\\', '/');
            string actualPath = GetSolutionFolderPath(projectId, names, solutionFolders, parents);
            if (!string.Equals(actualPath, expectedPath, StringComparison.Ordinal))
                failures.Add($"{projectPath}: expected Solution Folder '{expectedPath}', found '{actualPath}'.");

            string physicalGroup = sourceRoot == "tools"
                ? sourceRoot
                : Path.GetDirectoryName(Path.GetDirectoryName(projectPath))?
                    .Replace('\\', '/').TrimEnd('/') ?? string.Empty;
            if (expectedPath == Path.GetDirectoryName(projectPath)?.Replace('\\', '/'))
                physicalGroup = expectedPath;
            if (!string.Equals(physicalGroup, expectedPath, StringComparison.Ordinal))
            {
                failures.Add(
                    $"{projectPath}: physical source folder '{physicalGroup}' must match conceptual folder '{expectedPath}'.");
            }
        }
    }

    private static string? ClassifySourceSolutionPath(string projectName)
    {
        if (projectName.StartsWith("Inno.Core.", StringComparison.Ordinal))
            return "src/foundation/core";
        if (projectName.StartsWith("Inno.Extensibility.", StringComparison.Ordinal))
            return "src/foundation/extensibility";
        if (string.Equals(projectName, "Inno.Scripting.Api", StringComparison.Ordinal))
            return "src/foundation/scripting";
        if (string.Equals(projectName, "Inno.Shell", StringComparison.Ordinal))
            return "src/composition/shell";
        if (string.Equals(projectName, "Inno.Player.Runtime", StringComparison.Ordinal))
            return "src/composition/player";
        if (string.Equals(projectName, "Inno.Editor.Annotations", StringComparison.Ordinal))
            return "src/composition/editor/contracts";

        if (projectName.StartsWith("Inno.Editor.Panel.", StringComparison.Ordinal))
            return "src/composition/editor/panels";
        if (string.Equals(projectName, "Inno.Editor.Hosting", StringComparison.Ordinal))
            return "src/composition/editor/hosting";
        if (string.Equals(projectName, "Inno.Editor.ImGui", StringComparison.Ordinal))
            return "src/composition/editor/presentation";
        if (string.Equals(projectName, "Inno.Editor.Core", StringComparison.Ordinal) ||
            string.Equals(projectName, "Inno.Editor.Diagnostics", StringComparison.Ordinal) ||
            string.Equals(projectName, "Inno.Editor.Graph", StringComparison.Ordinal) ||
            string.Equals(projectName, "Inno.Editor.Inspection", StringComparison.Ordinal) ||
            string.Equals(projectName, "Inno.Editor.Interactions", StringComparison.Ordinal) ||
            string.Equals(projectName, "Inno.Editor.Settings", StringComparison.Ordinal))
        {
            return "src/composition/editor/framework";
        }
        if (projectName.StartsWith("Inno.Editor.", StringComparison.Ordinal))
            return "src/composition/editor/features";
        if (string.Equals(projectName, "Inno.Adapter", StringComparison.Ordinal))
            return "src/adapters/common";
        if (string.Equals(projectName, "Inno.Adapter.Default", StringComparison.Ordinal))
            return "src/composition/adapters";
        if (string.Equals(projectName, "Inno.Adapter.Authoring.Default", StringComparison.Ordinal))
            return "src/composition/adapters";
        if (projectName.StartsWith("Inno.Adapter.Presentation", StringComparison.Ordinal))
            return "src/adapters/presentation";
        if (projectName.StartsWith("Inno.Adapter.Platform", StringComparison.Ordinal))
            return "src/adapters/platform";
        if (projectName.StartsWith("Inno.Adapter.Input", StringComparison.Ordinal))
            return "src/adapters/input";
        if (projectName.StartsWith("Inno.Adapter.Storage", StringComparison.Ordinal))
            return "src/adapters/storage";
        if (projectName.StartsWith("Inno.Adapter.Rendering", StringComparison.Ordinal))
            return "src/adapters/rendering";
        if (projectName.StartsWith("Inno.Adapter.Text", StringComparison.Ordinal))
            return "src/adapters/text";
        if (projectName.StartsWith("Inno.Adapter.UI", StringComparison.Ordinal))
            return "src/adapters/ui";
        if (projectName.StartsWith("Inno.Adapter.Audio", StringComparison.Ordinal))
            return "src/adapters/audio";
        if (string.Equals(projectName, "Inno.Content", StringComparison.Ordinal))
            return "src/content/deployment";
        if (projectName.StartsWith("Inno.References", StringComparison.Ordinal))
            return "src/content/references";
        if (projectName.StartsWith("Inno.Assets", StringComparison.Ordinal))
            return "src/content/assets";
        if (projectName.StartsWith("Inno.Scene", StringComparison.Ordinal))
            return "src/content/scene";
        if (projectName.StartsWith("Inno.Animation", StringComparison.Ordinal))
            return "src/content/animation";
        if (string.Equals(projectName, "Inno.Platform", StringComparison.Ordinal))
            return "src/services/platform";
        if (projectName.StartsWith("Inno.Input", StringComparison.Ordinal))
            return "src/services/input";
        if (projectName.StartsWith("Inno.Storage", StringComparison.Ordinal))
            return "src/services/storage";
        if (projectName.StartsWith("Inno.Rendering", StringComparison.Ordinal))
            return "src/services/rendering";
        if (projectName.StartsWith("Inno.Text", StringComparison.Ordinal))
            return "src/services/text";
        if (projectName.StartsWith("Inno.UI", StringComparison.Ordinal))
            return "src/services/ui";
        if (projectName.StartsWith("Inno.Audio", StringComparison.Ordinal))
            return "src/services/audio";
        if (string.Equals(projectName, "Inno.Runtime", StringComparison.Ordinal))
            return "src/runtime/engine";
        if (string.Equals(projectName, "Inno.Runtime.Contracts", StringComparison.Ordinal))
            return "src/runtime/contracts";
        if (string.Equals(projectName, "Inno.Runtime.Generators", StringComparison.Ordinal))
            return "src/runtime/generators";
        if (string.Equals(projectName, "Inno.Engine.Default", StringComparison.Ordinal))
            return "src/composition/default";
        if (projectName.StartsWith("Inno.Scripting", StringComparison.Ordinal))
            return "src/runtime/scripting";
        if (projectName.StartsWith("Inno.Plugins", StringComparison.Ordinal))
            return "src/runtime/plugins";
        return null;
    }

    private static string GetSolutionFolderPath(
        string projectId,
        IReadOnlyDictionary<string, string> names,
        IReadOnlySet<string> solutionFolders,
        IReadOnlyDictionary<string, string> parents
    ) {
        if (!parents.TryGetValue(projectId, out string? currentId))
            return "<solution-root>";

        var segments = new List<string>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (visited.Add(currentId))
        {
            if (!solutionFolders.Contains(currentId))
                return "<invalid-parent>";
            segments.Add(names.GetValueOrDefault(currentId, currentId));
            if (!parents.TryGetValue(currentId, out currentId!))
                break;
        }
        segments.Reverse();
        return string.Join('/', segments);
    }

    private static bool HasSolutionAncestor(
        string startingId,
        string expectedAncestorId,
        IReadOnlyDictionary<string, string> parents
    ) {
        string currentId = startingId;
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (visited.Add(currentId))
        {
            if (string.Equals(currentId, expectedAncestorId, StringComparison.OrdinalIgnoreCase))
                return true;
            if (!parents.TryGetValue(currentId, out currentId!))
                return false;
        }
        return false;
    }

    private static bool HasNamedSolutionAncestor(
        string startingId,
        string expectedName,
        IReadOnlyDictionary<string, string> names,
        IReadOnlyDictionary<string, string> parents
    ) {
        string currentId = startingId;
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        while (visited.Add(currentId))
        {
            if (string.Equals(names.GetValueOrDefault(currentId), expectedName, StringComparison.Ordinal))
                return true;
            if (!parents.TryGetValue(currentId, out currentId!))
                return false;
        }
        return false;
    }

    private static bool IsAnyDomain(
        string path,
        params string[] domains
    )
        => domains.Any(domain => path.StartsWith($"src/{domain}/", StringComparison.Ordinal));

    private static void AddSourceFailure(
        bool condition,
        string path,
        string message,
        ICollection<string> failures
    ) {
        if (condition)
            failures.Add($"{path}: {message}.");
    }

    private static IEnumerable<string> EnumerateDirectories(string root)
        => RepositorySourceInventory.Directories(root);

    private static IEnumerable<string> EnumerateFiles(
        string root,
        string pattern
    ) => RepositorySourceInventory.Files(root, pattern);

    private static bool IsIgnoredPath(string path) => path.Split(Path.DirectorySeparatorChar).Any(S_IGNORED_DIRECTORIES.Contains);

    private static bool IsGenerated(string source)
        => source.Contains("<auto-generated>", StringComparison.OrdinalIgnoreCase) ||
           source.Contains("[GeneratedCode", StringComparison.Ordinal);

    private static string Relative(
        string repositoryRoot,
        string path
    ) => Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/');

    [GeneratedRegex(@"\b(schemaVersion|formatVersion|formerVersion)\b", RegexOptions.IgnoreCase)]
    private static partial Regex CompatibilityFieldPattern();

    [GeneratedRegex(@"^\s*global\s+using\b", RegexOptions.Multiline)]
    private static partial Regex GlobalUsingPattern();

    [GeneratedRegex(@"Executes this contract at the caller-controlled boundary|Transforms validated inputs into a deterministic result|Gets caller-visible|input consumed by this operation|state exposed by this contract|value associated with this contract|Models the .* domain value|Performs the .* operation|Runs the .* operation|Gets the .* value owned by the current instance")]
    private static partial Regex UninformativeXmlPattern();

    [GeneratedRegex(@"^\s*///\s*<(summary|param|typeparam|returns|exception|remarks)\b[^>]*>.+</\1>\s*$")]
    private static partial Regex SingleLineXmlPattern();

    [GeneratedRegex(@"(?m)^([ \t]*)///[ \t]*<(summary|param|typeparam|returns|exception|remarks)([^>]*)>(.+)</\2>[ \t]*\r?$")]
    private static partial Regex ExpandableXmlPattern();

    [GeneratedRegex("""^Project\("(?<type>\{[^}]+\})"\) = "(?<name>[^"]+)", "(?<path>[^"]+)", "(?<id>\{[^}]+\})"$""")]
    private static partial Regex SolutionProjectPattern();

    [GeneratedRegex(@"^\s*(\{[^}]+\})\s*=\s*(\{[^}]+\})\s*$")]
    private static partial Regex SolutionNestingPattern();
}
