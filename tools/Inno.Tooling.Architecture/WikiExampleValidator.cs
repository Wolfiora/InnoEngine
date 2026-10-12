using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class WikiExampleValidator
{
    internal static void Validate(
        string root,
        IReadOnlyList<WikiProjectPage> projects,
        DocumentationSourceModels models,
        WikiScriptReferences scriptReferences,
        WikiProjectReferences? projectReferences,
        string? inventoryPath,
        ICollection<string> failures,
        CancellationToken cancellationToken
    ) {
        var results = new List<WikiExampleResult>();
        var owners = projects.ToDictionary(static project => Path.GetFullPath(project.page),
            StringComparer.OrdinalIgnoreCase);
        foreach (string pagePath in RepositorySourceInventory.Files(Path.Combine(root, "docs"), "*.md"))
        {
            owners.TryGetValue(Path.GetFullPath(pagePath), out WikiProjectPage? project);
            string[] lines = File.ReadAllLines(pagePath);
            string? source = project is null ? null
                : RepositorySourceInventory.Files(Path.GetDirectoryName(project.project)!, "*.cs").FirstOrDefault();
            Compilation? owner = source is null ? null : models.GetModel(source)?.Compilation;
            CSharpCompilation? compiler = owner as CSharpCompilation;
            if (project is null)
                compiler = CSharpCompilation.Create("WikiGuide", references: models.frameworkReferences,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            int example = 0;
            for (int index = 0; index < lines.Length; index++)
            {
                string opening = lines[index].Trim();
                if (opening is not "```csharp" and not "```cs")
                    continue;
                int start = ++index;
                while (index < lines.Length && lines[index].Trim() != "```")
                    index++;
                example++;
                string page = Path.GetRelativePath(root, pagePath).Replace('\\', '/');
                var diagnostics = new List<string>();
                string referenceMode = project is null ? "framework" : "host";
                string referenceIdentity = string.Empty;
                if (index == lines.Length)
                    diagnostics.Add("The C# example has no closing fence.");
                if (compiler is null)
                    diagnostics.Add("The owning project has no available semantic compilation.");
                else
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(string.Join('\n', lines[start..index]),
                        (CSharpParseOptions?)compiler.SyntaxTrees.FirstOrDefault()?.Options,
                        path: page + "#example-" + example, cancellationToken: cancellationToken);
                    OutputKind output = tree.GetRoot(cancellationToken).DescendantNodes()
                        .Any(static node => node is GlobalStatementSyntax) ? OutputKind.ConsoleApplication : OutputKind.DynamicallyLinkedLibrary;
                    CSharpCompilation snippet = compiler.RemoveAllSyntaxTrees().AddSyntaxTrees(tree)
                        .WithAssemblyName("WikiExample_" + example)
                        .WithOptions(compiler.Options.WithOutputKind(output));
                    string[] qualifiedNames = tree.GetRoot(cancellationToken).DescendantNodes()
                        .OfType<QualifiedNameSyntax>().Select(static name => name.ToString()).ToArray();
                    bool editorScript = qualifiedNames.Any(static name => name.StartsWith("InnoEditor.", StringComparison.Ordinal));
                    bool runtimeScript = qualifiedNames.Any(static name => name.StartsWith("InnoEngine.", StringComparison.Ordinal));
                    if (editorScript || runtimeScript)
                    {
                        referenceMode = editorScript ? "editor-script" : "runtime-script";
                        try
                        {
                            snippet = snippet.WithReferences(models.frameworkReferences
                                .Concat(scriptReferences.GetReferences(editorScript)));
                            referenceIdentity = scriptReferences.GetIdentity(editorScript);
                        }
                        catch (InvalidOperationException failure)
                        {
                            diagnostics.Add(failure.Message);
                        }
                    }
                    else if (project is not null && projectReferences is not null)
                    {
                        referenceMode = "host-sdk";
                        try
                        {
                            CSharpCompilation evaluated = projectReferences.GetCompilation(project.project, cancellationToken);
                            SyntaxTree evaluatedTree = CSharpSyntaxTree.ParseText(tree.GetText(cancellationToken),
                                (CSharpParseOptions)evaluated.SyntaxTrees.Single().Options, path: tree.FilePath,
                                cancellationToken: cancellationToken);
                            snippet = evaluated.RemoveAllSyntaxTrees().AddSyntaxTrees(evaluatedTree)
                                .WithOptions(evaluated.Options.WithOutputKind(output));
                        }
                        catch (Exception failure) when (failure is InvalidDataException or InvalidOperationException or JsonException)
                        {
                            diagnostics.Add(failure.Message);
                        }
                    }
                    if (diagnostics.Count == 0)
                    {
                        diagnostics.AddRange(snippet.GetDiagnostics(cancellationToken)
                            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                            .Select(static diagnostic => diagnostic.ToString()));
                    }
                }
                results.Add(new WikiExampleResult(page, example, start + 1, referenceMode, referenceIdentity, diagnostics));
                foreach (string diagnostic in diagnostics)
                    failures.Add($"Wiki '{page}' example {example} at line {start + 1}: {diagnostic}");
            }
        }
        if (inventoryPath is not null)
        {
            string output = Path.GetFullPath(inventoryPath);
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            File.WriteAllText(output, JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    private sealed record WikiExampleResult(
        string page,
        int example,
        int line,
        string referenceMode,
        string referenceIdentity,
        IReadOnlyList<string> errors
    );
}
