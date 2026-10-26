using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;
internal static class EditorUiBoundaryValidator
{
    internal static void Validate(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string editorRoot = Path.Combine(repositoryRoot, "src/composition/editor");
        foreach (string path in RepositorySourceInventory.Files(editorRoot, "*.csproj"))
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
}
