using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Selects repository rules for the caller's changed paths and work tags.
/// </summary>
public static class RepositoryRuleSelection
{
    /// <summary>
    /// Validates the rule catalog and prints the rules applicable to this operation.
    /// </summary>
    /// <param name="arguments">
    /// Options for paths, tags, rules, the engine root and an explicitly selected Docs checkout.
    /// </param>
    /// <returns>
    /// Zero after the catalog and requested selections have been validated.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An option, path, tag or rule is invalid.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A rule has malformed metadata or exceeds its size budget.
    /// </exception>
    public static int Execute(string[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        List<string> paths = [];
        List<string> tags = [];
        List<string> ids = [];
        string? requestedRoot = null;
        string? docs = null;
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (index + 1 >= arguments.Length)
            {
                throw new ArgumentException($"Missing value for {key}.");
            }
            string value = arguments[index + 1];
            switch (key)
            {
                case "--engine-root":
                    requestedRoot = Path.GetFullPath(value);
                    break;
                case "--docs-root":
                    docs = Path.TrimEndingDirectorySeparator(Path.GetFullPath(value));
                    break;
                case "--paths":
                case "--tags":
                case "--rules":
                    List<string> values = key == "--paths" ? paths : key == "--tags" ? tags : ids;
                    values.AddRange(value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
                    break;
                default:
                    throw new ArgumentException($"Unknown option: {key}");
            }
        }
        string root = requestedRoot ?? FindEngineRoot();
        int bytes = Encoding.UTF8.GetByteCount(File.ReadAllText(Path.Combine(root, "AGENTS.md")));
        if (bytes > 8192)
        {
            throw new InvalidDataException("Root instructions exceed 8192 bytes.");
        }
        List<Rule> catalog = Read(root, string.Empty).ToList();
        if (docs != null)
        {
            catalog.AddRange(Read(docs, "docs/"));
        }
        var selected = new Dictionary<Rule, List<string>>();
        void Include(
            Rule rule,
            string reason
        ) {
            if (!selected.TryGetValue(rule, out List<string>? reasons))
            {
                reasons = [];
                selected.Add(rule, reasons);
            }
            if (!reasons.Contains(reason))
            {
                reasons.Add(reason);
            }
        }
        foreach (string id in ids)
        {
            Include(catalog.SingleOrDefault(rule => rule.id == id) ?? throw new ArgumentException($"Unknown rule: {id}"), "explicit rule");
        }
        foreach (string tag in tags)
        {
            Rule[] matches = catalog.Where(rule => rule.tags.Contains(tag)).ToArray();
            if (matches.Length == 0)
            {
                throw new ArgumentException($"Unknown tag: {tag}");
            }
            foreach (Rule rule in matches)
            {
                Include(rule, $"tag: {tag}");
            }
        }
        foreach (string path in paths)
        {
            string full = Path.GetFullPath(path, root);
            string owner = docs != null && IsContained(Path.GetRelativePath(docs, full)) ? docs : root;
            string relative = Path.GetRelativePath(owner, full).Replace('\\', '/');
            if (!IsContained(relative))
            {
                throw new ArgumentException($"Path is outside the repository: {path}");
            }
            foreach (Rule rule in catalog.Where(rule => rule.root == owner && rule.patterns.Any(pattern => pattern.IsMatch(relative))))
            {
                Include(rule, $"path: {relative}");
            }
        }
        Console.WriteLine($"Rules are valid: {catalog.Count} files. Root: {bytes} bytes.");
        if (selected.Count > 0)
        {
            Console.WriteLine($"Read each file completely: {selected.Count} files, {selected.Keys.Sum(rule => rule.bytes)} bytes.");
            foreach ((Rule rule, List<string> reasons) in selected)
            {
                string shown = rule.root == root ? Path.GetRelativePath(root, rule.file).Replace('\\', '/') : rule.file;
                Console.WriteLine($"{shown} | {string.Join("; ", reasons)}");
            }
        }
        return 0;
    }

    private static string FindEngineRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "InnoEngine.sln")))
            {
                return directory.FullName;
            }
            directory = directory.Parent;
        }
        throw new DirectoryNotFoundException("Select the engine checkout with --engine-root.");
    }

    private static bool IsContained(string path)
        => !Path.IsPathRooted(path) && path != ".." && !path.Replace('\\', '/').StartsWith("../", StringComparison.Ordinal);

    private static IEnumerable<Rule> Read(
        string root,
        string prefix
    ) {
        string directory = Path.Combine(root, ".agents", "rulebook");
        foreach (string file in Directory.EnumerateFiles(directory, "*.md", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            string relative = Path.GetRelativePath(directory, file).Replace('\\', '/');
            if (relative == "README.md")
            {
                continue;
            }
            string id = prefix + relative[..^3];
            string content = File.ReadAllText(file).Replace("\r\n", "\n", StringComparison.Ordinal);
            int length = Encoding.UTF8.GetByteCount(content);
            if (length > 16_384)
            {
                throw new InvalidDataException($"Rule file exceeds 16384 bytes: {id}");
            }
            Match header = Regex.Match(content, @"\A---\n((?:[a-z]+: \[[^\]\n]*\]\n)+)---\n");
            if (!header.Success)
            {
                throw new InvalidDataException($"Rule file needs front matter with paths or tags: {id}");
            }
            var fields = new Dictionary<string, string[]>();
            foreach (string line in header.Groups[1].Value.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                int colon = line.IndexOf(':');
                string key = line[..colon];
                string[] values = line[(colon + 3)..^1].Split(',').Select(value => value.Trim().Trim('"')).ToArray();
                if (key is not ("paths" or "tags") || !fields.TryAdd(key, values) || values.Contains(string.Empty))
                {
                    throw new InvalidDataException($"Invalid or repeated front matter field: {id} -> {key}");
                }
            }
            string[] tags = fields.GetValueOrDefault("tags", []);
            if (tags.Any(tag => !Regex.IsMatch(tag, "^[a-z0-9-]+$")))
            {
                throw new InvalidDataException($"Invalid rule tag: {id}");
            }
            Regex[] patterns = fields.GetValueOrDefault("paths", []).Select(glob =>
            {
                if (glob.StartsWith('/') || glob.Contains('\\') || glob.Split('/').Any(piece => piece is "." or ".."))
                {
                    throw new InvalidDataException($"Rule path is not a repository-relative glob: {id} -> {glob}");
                }
                string pattern = Regex.Escape(glob).Replace(@"\*\*/", "(?:.*/)?").Replace(@"\*\*", ".*").Replace(@"\*", "[^/]*");
                return new Regex("^" + pattern + "$");
            }).ToArray();
            yield return new Rule(id, root, file, length, tags, patterns);
        }
    }

    private sealed record Rule(
        string id,
        string root,
        string file,
        int bytes,
        string[] tags,
        Regex[] patterns
    );
}
