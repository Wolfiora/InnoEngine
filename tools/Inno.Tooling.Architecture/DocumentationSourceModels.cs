using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal sealed class DocumentationSourceModels
{
    private readonly IReadOnlyList<MetadataReference> m_references;
    private readonly HashSet<string> m_projectNames;
    private string? m_projectDirectory;
    private CSharpCompilation? m_compilation;
    private Dictionary<string, SyntaxTree> m_trees = new(StringComparer.OrdinalIgnoreCase);

    internal DocumentationSourceModels(
        string repositoryRoot,
        string configuration
    ) {
        string[] projects = new[] { "src", "native", "build", "tools" }
            .SelectMany(folder => RepositorySourceInventory.Files(
                Path.Combine(repositoryRoot, folder), "*.csproj"))
            .Where(IsSourcePath)
            .ToArray();
        m_projectNames = projects.Select(Path.GetFileNameWithoutExtension)
            .OfType<string>().ToHashSet(StringComparer.Ordinal);
        var paths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string[] platform = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty)
            .Split(Path.PathSeparator);
        foreach (string path in platform.Where(File.Exists))
            paths[Path.GetFileNameWithoutExtension(path)] = path;
        var projectOutputs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string project in projects)
        {
            XDocument document = XDocument.Load(project);
            string framework = document.Descendants("TargetFramework").FirstOrDefault()?.Value ?? "net9.0";
            string directory = Path.Combine(Path.GetDirectoryName(project)!,
                Path.GetFileNameWithoutExtension(project) == "Inno.Player.Browser" ? "bin/browser-wasm" : "bin",
                configuration, framework);
            if (!Directory.Exists(directory))
                continue;
            string assemblyName = document.Descendants("AssemblyName").FirstOrDefault()?.Value
                ?? Path.GetFileNameWithoutExtension(project);
            string projectOutput = Path.Combine(directory, assemblyName + ".dll");
            if (File.Exists(projectOutput))
                projectOutputs[assemblyName] = projectOutput;
            foreach (string path in Directory.EnumerateFiles(directory, "*.dll"))
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

    internal SemanticModel? GetModel(string path)
    {
        string? projectDirectory = FindProjectDirectory(path);
        if (projectDirectory is null)
            return null;
        if (!string.Equals(projectDirectory, m_projectDirectory, StringComparison.OrdinalIgnoreCase))
        {
            m_projectDirectory = projectDirectory;
            m_trees = RepositorySourceInventory.Files(projectDirectory, "*.cs")
                .Where(IsSourcePath)
                .ToDictionary(static value => Path.GetFullPath(value), static value =>
                    (SyntaxTree)CSharpSyntaxTree.ParseText(File.ReadAllText(value), path: value),
                    StringComparer.OrdinalIgnoreCase);
            m_compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(Directory.EnumerateFiles(projectDirectory, "*.csproj").Single()),
                m_trees.Values,
                m_references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        }
        return m_trees.TryGetValue(Path.GetFullPath(path), out SyntaxTree? tree)
            ? m_compilation!.GetSemanticModel(tree)
            : null;
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

    private static bool IsSourcePath(string path)
        => !path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .Any(static segment => segment is "bin" or "obj");
}
