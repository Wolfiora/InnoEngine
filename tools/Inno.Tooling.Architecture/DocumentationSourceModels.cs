using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal sealed class DocumentationSourceModels
{
    private readonly IReadOnlyList<MetadataReference> m_references;
    private readonly IReadOnlyList<MetadataReference> m_frameworkReferences;
    private readonly HashSet<string> m_projectNames;
    private readonly WikiProjectReferences? m_projectReferences;
    private readonly CancellationToken m_cancellation;
    private readonly ICollection<string>? m_failures;
    private readonly HashSet<string> m_unavailableProjects = new(StringComparer.OrdinalIgnoreCase);
    private string? m_projectDirectory;
    private CSharpCompilation? m_compilation;
    private Dictionary<string, SyntaxTree> m_trees = new(StringComparer.OrdinalIgnoreCase);

    internal DocumentationSourceModels(
        string repositoryRoot,
        string configuration,
        WikiProjectReferences? projectReferences = null,
        CancellationToken cancellationToken = default,
        ICollection<string>? failures = null
    ) {
        m_projectReferences = projectReferences;
        m_cancellation = cancellationToken;
        m_failures = failures;
        string[] projects = new[] { "entry", "src", "backends", "platforms", "build", "tools" }
            .SelectMany(folder => RepositorySourceInventory.Files(
                Path.Combine(repositoryRoot, folder), "*.csproj"))
            .Where(IsSourcePath)
            .ToArray();
        m_projectNames = projects.Select(Path.GetFileNameWithoutExtension)
            .OfType<string>().ToHashSet(StringComparer.Ordinal);
        if (m_projectReferences is not null)
        {
            m_references = [];
            m_frameworkReferences = m_projectReferences.GetFrameworkReferences(cancellationToken);
            return;
        }
        var paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory().TrimEnd(Path.DirectorySeparatorChar);
        string frameworkReferences = Path.GetFullPath(Path.Combine(runtimeDirectory, "..", "..", "..",
            "packs", "Microsoft.NETCore.App.Ref", Path.GetFileName(runtimeDirectory), "ref",
            "net" + Environment.Version.Major + ".0"));
        StringComparison pathComparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        string[] platform = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty)
            .Split(Path.PathSeparator);
        foreach (string path in platform.Where(File.Exists))
        {
            if (Directory.Exists(frameworkReferences) && path.StartsWith(runtimeDirectory + Path.DirectorySeparatorChar, pathComparison))
                continue;
            paths[Path.GetFileNameWithoutExtension(path)] = path;
        }
        if (Directory.Exists(frameworkReferences))
            foreach (string reference in Directory.EnumerateFiles(frameworkReferences, "*.dll"))
                paths[Path.GetFileNameWithoutExtension(reference)] = reference;
        m_frameworkReferences = paths.Values.Where(path =>
                path.StartsWith(frameworkReferences + Path.DirectorySeparatorChar, pathComparison)
                || path.StartsWith(runtimeDirectory + Path.DirectorySeparatorChar, pathComparison))
            .Select(static path => (MetadataReference)MetadataReference.CreateFromFile(path)).ToArray();
        var projectOutputs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string project in projects)
        {
            XDocument document = XDocument.Load(project);
            string framework = document.Descendants("TargetFramework").FirstOrDefault()?.Value ?? "net10.0";
            string? target = document.Descendants("InnoProductTarget").FirstOrDefault()?.Value;
            string directory = Path.Combine(Path.GetDirectoryName(project)!,
                target is null ? "bin" : Path.Combine("bin", target),
                configuration, framework);
            if (!Directory.Exists(directory))
                continue;
            string assemblyName = document.Descendants("AssemblyName").FirstOrDefault()?.Value
                ?? Path.GetFileNameWithoutExtension(project);
            string projectOutput = Path.Combine(directory, assemblyName + ".dll");
            if (File.Exists(projectOutput))
                projectOutputs[assemblyName] = projectOutput;
            foreach (string path in Directory.EnumerateFiles(directory, "*.dll").Where(IsManagedAssembly))
                paths.TryAdd(Path.GetFileNameWithoutExtension(path), path);
        }
        // A consumer's copied dependency must not replace the owning project's contract snapshot.
        foreach (KeyValuePair<string, string> output in projectOutputs)
            paths[output.Key] = output.Value;
        m_references = paths.Values.Select(static path =>
        {
            string documentationPath = Path.ChangeExtension(path, ".xml");
            DocumentationProvider? documentation = File.Exists(documentationPath)
                ? new AssemblyDocumentationProvider(documentationPath)
                : null;
            return (MetadataReference)MetadataReference.CreateFromFile(path, documentation: documentation);
        }).ToArray();
    }

    internal bool OwnsAssembly(IAssemblySymbol assembly) => m_projectNames.Contains(assembly.Name);

    internal bool IsUnavailable(string projectDirectory) => m_unavailableProjects.Contains(projectDirectory);

    internal bool IsExcludedProduct(string projectDirectory)
        => m_projectReferences?.IsExcludedProduct(Directory.EnumerateFiles(projectDirectory, "*.csproj").Single()) == true;

    internal IReadOnlyList<MetadataReference> frameworkReferences => m_frameworkReferences;

    internal SemanticModel? GetModel(string path)
    {
        string? projectDirectory = FindProjectDirectory(path);
        if (projectDirectory is null)
            return null;
        CSharpCompilation? compilation = GetCompilation(projectDirectory);
        return compilation is not null && m_trees.TryGetValue(Path.GetFullPath(path), out SyntaxTree? tree)
            ? compilation.GetSemanticModel(tree)
            : null;
    }

    internal CSharpCompilation? GetCompilation(string projectDirectory)
    {
        if (IsExcludedProduct(projectDirectory))
            return null;
        if (m_unavailableProjects.Contains(projectDirectory))
            return null;
        if (!string.Equals(projectDirectory, m_projectDirectory, StringComparison.OrdinalIgnoreCase))
        {
            m_projectDirectory = projectDirectory;
            if (m_projectReferences is not null)
            {
                m_compilation = null;
                m_trees.Clear();
                try
                {
                    m_compilation = m_projectReferences.GetSourceCompilation(
                        Directory.EnumerateFiles(projectDirectory, "*.csproj").Single(), m_cancellation);
                }
                catch (FileNotFoundException failure) when (m_failures is not null)
                {
                    m_unavailableProjects.Add(projectDirectory);
                    m_failures.Add(failure.Message);
                    return null;
                }
                m_trees = m_compilation.SyntaxTrees.ToDictionary(static tree => Path.GetFullPath(tree.FilePath),
                    StringComparer.OrdinalIgnoreCase);
                return m_compilation;
            }
            m_trees = RepositorySourceInventory.Files(projectDirectory, "*.cs")
                .Where(IsSourcePath)
                .ToDictionary(static value => Path.GetFullPath(value), static value =>
                    (SyntaxTree)CSharpSyntaxTree.ParseText(File.ReadAllText(value),
                        new CSharpParseOptions(LanguageVersion.CSharp14), path: value),
                    StringComparer.OrdinalIgnoreCase);
            m_compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(Directory.EnumerateFiles(projectDirectory, "*.csproj").Single()),
                m_trees.Values,
                GetReferences(projectDirectory),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        }
        return m_compilation;
    }

    private IEnumerable<MetadataReference> GetReferences(string projectDirectory)
    {
        var references = m_references.ToDictionary(
            static reference => Path.GetFileNameWithoutExtension(reference.Display)!,
            StringComparer.OrdinalIgnoreCase);
        // Compile-only package contracts are intentionally absent from runtime deployment outputs.
        foreach (string path in DocumentationPackageReferences.Read(projectDirectory))
        {
            string documentationPath = Path.ChangeExtension(path, ".xml");
            DocumentationProvider? documentation = File.Exists(documentationPath)
                ? new AssemblyDocumentationProvider(documentationPath)
                : null;
            references[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path, documentation: documentation);
        }
        return references.Values;
    }

    private static string? FindProjectDirectory(string path)
    {
        DirectoryInfo? directory = new(Path.GetDirectoryName(path)!);
        while (directory is not null)
        {
            if (Directory.EnumerateFiles(directory.FullName, "*.csproj").Any())
                return directory.FullName;
            directory = directory.Parent;
        }
        return null;
    }

    private static bool IsManagedAssembly(string path)
    {
        using FileStream stream = File.OpenRead(path);
        using var image = new PEReader(stream);
        return image.HasMetadata;
    }

    private static bool IsSourcePath(string path)
        => !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(static segment => segment is "bin" or "obj");
}
