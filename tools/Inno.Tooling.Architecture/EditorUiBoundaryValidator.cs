using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;
internal static class EditorUiBoundaryValidator
{
    internal static void Validate(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string editorRoot = Path.Combine(repositoryRoot, "src/composition/editor");
        var projects = new List<string>(RepositorySourceInventory.Files(editorRoot, "*.csproj"));
        string sdkHost = Path.Combine(repositoryRoot, "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj");
        if (File.Exists(sdkHost))
            projects.Add(sdkHost);
        foreach (string path in projects)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            string relative = Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/');
            bool feature = relative.StartsWith("src/composition/editor/features/", StringComparison.Ordinal);
            bool panel = relative.StartsWith("src/composition/editor/panels/", StringComparison.Ordinal);
            foreach (XElement reference in XDocument.Load(path).Descendants("ProjectReference"))
            {
                string dependency = Path.GetFileNameWithoutExtension(((string? )reference.Attribute("Include") ?? "").Replace('\\', '/'));
                if (feature
                    && (dependency.Contains(
                    "ImGui",
                    StringComparison.Ordinal
                )
                    || dependency.StartsWith(
                    "Inno.Native.",
                    StringComparison.Ordinal
                )
                    || dependency.StartsWith(
                    "Inno.Editor.Panel.",
                    StringComparison.Ordinal
                )
                    || dependency is "Inno.Editor.Workbench" or "Inno.Editor.Hosting"))
                    failures.Add($"{relative}: Editor feature editing must be presentation-independent; remove {dependency}.");
                if (panel && dependency.StartsWith("Inno.Editor.Panel.", StringComparison.Ordinal))
                    failures.Add($"{relative}: Panels cannot depend on other Panels; move shared behavior to its feature owner ({dependency}).");
                if (name == "Inno.Build.Cli" && dependency.StartsWith("Inno.Editor.Panel.", StringComparison.Ordinal))
                {
                    string include = (string?)reference.Attribute("Include") ?? string.Empty;
                    string referencedProject = Path.GetFullPath(include.Replace('\\', '/'), Path.GetDirectoryName(path)!);
                    if (!DeclaresScriptingApi(referencedProject))
                        failures.Add($"{relative}: the script SDK host must reference API owners instead of transitive Panel carriers ({dependency}).");
                }
                if (name != "Inno.Editor.ImGui" && name != "Inno.Editor.Hosting" && dependency.StartsWith("Inno.Native.", StringComparison.Ordinal))
                    failures.Add($"{relative}: native UI access belongs to the Editor ImGui adapter, not {name}.");
                if (name is "Inno.Editor.Graph" or "Inno.Editor.Graph.ImGui"
                    && (dependency.Contains(
                    "Shader",
                    StringComparison.Ordinal
                )
                    || dependency.Contains(
                    "Rendering",
                    StringComparison.Ordinal
                )))
                    failures.Add($"{relative}: reusable Graph mechanisms cannot depend on Shader or Rendering semantics.");
            }
        }
    }

    private static bool DeclaresScriptingApi(string project)
    {
        string manifest = Path.Combine(Path.GetDirectoryName(project)!, "Properties/ScriptingApi.cs");
        if (!File.Exists(manifest))
            return false;
        var syntax = CSharpSyntaxTree.ParseText(
            File.ReadAllText(manifest),
            new CSharpParseOptions(LanguageVersion.CSharp14)
        ).GetCompilationUnitRoot();
        foreach (AttributeListSyntax list in syntax.AttributeLists)
        {
            if (list.Target?.Identifier.ValueText != "assembly")
                continue;
            foreach (AttributeSyntax attribute in list.Attributes)
                if (attribute.Name.GetLastToken().ValueText is "ScriptingApiExport" or "ScriptingApiExportAttribute")
                    return true;
        }
        return false;
    }
}
