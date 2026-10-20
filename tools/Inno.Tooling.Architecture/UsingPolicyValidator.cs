using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class UsingPolicyValidator
{
    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        foreach (string file in RepositorySourceInventory.Files(root, "*"))
        {
            string relative = Path.GetRelativePath(root, file).Replace('\\', '/');
            if (file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                || file.EndsWith(".cs.txt", StringComparison.OrdinalIgnoreCase))
            {
                var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), new CSharpParseOptions(LanguageVersion.CSharp14));
                if (tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>().Any(static item => item.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword)))
                    failures.Add($"{relative}: global using directives are forbidden.");
                continue;
            }
            if (Path.GetExtension(file) is not (".csproj" or ".props" or ".targets"))
                continue;
            XDocument document = XDocument.Load(file);
            foreach (XElement property in document.Descendants("ImplicitUsings"))
                if (property.Value.Trim().Equals("enable", StringComparison.OrdinalIgnoreCase)
                    || property.Value.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
                    failures.Add($"{relative}: implicit usings must be disabled.");
            foreach (XElement item in document.Descendants("Using"))
                if (item.Attribute("Include") is not null || item.Attribute("Update") is not null)
                    failures.Add($"{relative}: MSBuild Using items are forbidden.");
        }
    }
}
