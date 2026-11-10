using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Exports documentation project facts from the same source inventory used by architecture coverage.
/// </summary>
public static class DocumentationReferences
{
    /// <summary>
    /// Writes a deterministic project inventory without claiming evaluated API surfaces or source verification.
    /// </summary>
    /// <param name="arguments">
    /// Explicit engine-root and output options. Output is a derived reference artifact, not a project selection table.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels source enumeration before atomic output publication.
    /// </param>
    /// <returns>
    /// Zero after publishing the current source-project inventory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An option is missing, duplicated or unsupported.
    /// </exception>
    /// <exception cref="IOException">
    /// A source path escapes the engine boundary or the destination cannot be published.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Required source discovery is empty or project identities are ambiguous.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels before publication.
    /// </exception>
    public static int ExportProjects(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (key is not ("--engine-root" or "--output") || index + 1 >= arguments.Length
                || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown, duplicate or incomplete reference export option: " + key);
        }
        if (!options.TryGetValue("--engine-root", out string? engineRoot) || !options.TryGetValue("--output", out string? output))
            throw new ArgumentException("Usage: docs export-projects --engine-root ROOT --output PROJECTS.json");
        string root = Path.GetFullPath(engineRoot);
        var names = new HashSet<string>(StringComparer.Ordinal);
        var projects = new List<object>();
        foreach (string project in RepositorySourceInventory.Projects(root)
            .OrderBy(project => Path.GetRelativePath(root, project).Replace('\\', '/'), StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(root, project);
            string name = Path.GetFileNameWithoutExtension(project);
            if (!names.Add(name))
                throw new InvalidDataException("Duplicate source-project identity: " + name);
            // API surfaces require evaluated extraction; inventory must never invent them from a project name.
            projects.Add(new { name, path = Path.GetRelativePath(root, project).Replace('\\', '/'), entry = (string?)null,
                surfaces = Array.Empty<string>(), isTest = WikiProjectCoverageValidator.IsDeclaredTestProject(project) });
        }
        if (projects.Count == 0)
            throw new InvalidDataException("Source-project inventory is empty.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(output), JsonSerializer.SerializeToUtf8Bytes(projects,
            new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Exported {projects.Count} source projects. API surfaces and source validation remain unverified.");
        return 0;
    }
}
