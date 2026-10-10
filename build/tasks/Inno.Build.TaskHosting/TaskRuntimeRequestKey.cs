using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Graph;

namespace Inno.Build.TaskHosting;

internal static class TaskRuntimeRequestKey
{
    internal static bool NeedsRestore(
        string project,
        IDictionary properties
    ) {
        var globals = properties.Cast<DictionaryEntry>().ToDictionary(
            static entry => (string)entry.Key, static entry => (string)entry.Value!, StringComparer.OrdinalIgnoreCase);
        using var collection = new ProjectCollection();
        var graph = new ProjectGraph(project, globals, collection);
        return graph.ProjectNodes.Any(static node => node.ProjectInstance.GetPropertyValue("ProjectAssetsFile") is string assets
            && assets.Length != 0 && !File.Exists(assets));
    }

    internal static string Capture(
        string project,
        string artifacts,
        IReadOnlyList<string> targets,
        IDictionary properties,
        CancellationToken cancellation
    ) {
        var globals = properties.Cast<DictionaryEntry>().ToDictionary(
            static entry => (string)entry.Key, static entry => (string)entry.Value!, StringComparer.OrdinalIgnoreCase);
        using var collection = new ProjectCollection();
        var graph = new ProjectGraph(project, globals, collection);
        StringComparer paths = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var inputs = new HashSet<string>(paths);
        var declarations = new List<string> { project, artifacts, string.Join(";", targets) };
        declarations.AddRange(globals.OrderBy(static entry => entry.Key, StringComparer.OrdinalIgnoreCase)
            .Select(static entry => entry.Key + "=" + entry.Value));
        foreach (var node in graph.ProjectNodes.OrderBy(static node => node.ProjectInstance.FullPath, paths))
        {
            cancellation.ThrowIfCancellationRequested();
            var instance = node.ProjectInstance;
            inputs.Add(instance.FullPath);
            foreach (string import in instance.ImportPaths)
                if (!import.EndsWith(".nuget.g.props", StringComparison.OrdinalIgnoreCase)
                    && !import.EndsWith(".nuget.g.targets", StringComparison.OrdinalIgnoreCase))
                    inputs.Add(import);
            declarations.AddRange(instance.GlobalProperties.OrderBy(static entry => entry.Key, StringComparer.OrdinalIgnoreCase)
                .Select(entry => instance.FullPath + "|" + entry.Key + "=" + entry.Value));
            foreach (string property in new[] { "TargetFramework", "DefineConstants", "LangVersion", "Nullable", "PathMap",
                "NETCoreSdkVersion", "MSBuildToolsPath", "MSBuildSDKsPath", "TreatWarningsAsErrors", "Optimize" })
                declarations.Add(instance.FullPath + "|" + property + "=" + instance.GetPropertyValue(property));
            foreach (var item in instance.Items)
            {
                bool source = item.ItemType is "Compile" or "EmbeddedResource" or "Analyzer";
                bool copy = item.GetMetadataValue("CopyToOutputDirectory") is "Always" or "PreserveNewest";
                if (source || copy)
                {
                    string input = Path.GetFullPath(item.EvaluatedInclude, Path.GetDirectoryName(instance.FullPath)!);
                    if (File.Exists(input))
                        inputs.Add(input);
                }
                if (item.ItemType == "Reference" && item.GetMetadataValue("HintPath") is string hint && hint.Length != 0)
                {
                    string input = Path.GetFullPath(hint, Path.GetDirectoryName(instance.FullPath)!);
                    if (File.Exists(input))
                        inputs.Add(input);
                }
                if (item.ItemType == "PackageReference")
                    declarations.Add(instance.FullPath + "|package:" + item.EvaluatedInclude + "=" + item.GetMetadataValue("Version"));
            }
            for (DirectoryInfo? directory = new(Path.GetDirectoryName(instance.FullPath)!); directory is not null; directory = directory.Parent)
            {
                string sdkSelection = Path.Combine(directory.FullName, "global.json");
                if (File.Exists(sdkSelection))
                {
                    inputs.Add(sdkSelection);
                    break;
                }
            }
        }
        string sdkRoot = Path.GetDirectoryName(typeof(ProjectGraph).Assembly.Location)!;
        foreach (string name in new[] { "Microsoft.Build.dll", "Microsoft.Build.Framework.dll", "Microsoft.Build.Utilities.Core.dll",
            "Microsoft.Build.Tasks.Core.dll", "MSBuild.runtimeconfig.json", "MSBuild.deps.json" })
        {
            string path = Path.Combine(sdkRoot, name);
            if (File.Exists(path))
                inputs.Add(path);
        }
        string compiler = Path.Combine(sdkRoot, "Roslyn", "bincore");
        if (Directory.Exists(compiler))
            foreach (string path in Directory.EnumerateFiles(compiler))
                inputs.Add(path);
        using var digest = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (string declaration in declarations)
            digest.AppendData(Encoding.UTF8.GetBytes(declaration + "\n"));
        foreach (string input in inputs.Order(paths))
        {
            cancellation.ThrowIfCancellationRequested();
            digest.AppendData(Encoding.UTF8.GetBytes(input + "\n"));
            using FileStream stream = File.OpenRead(input);
            digest.AppendData(SHA256.HashData(stream));
        }
        return Convert.ToHexString(digest.GetHashAndReset());
    }
}
